using System;
using System.Threading;
using System.Threading.Tasks;
using DefectDetection.Helpers.Data;
using DefectDetection.Helpers.Motion;
using DefectDetection.Models;

namespace DefectDetection.Services
{
    /// <summary>
    /// 运动分拣服务 - 根据检测结果执行分拣动作
    /// </summary>
    public class SortingService
    {
        private readonly IMotion _motion;
        private readonly MotionConfig _config;
        private readonly LogHelper _logHelper;
        private CancellationTokenSource _sortingCts;

        /// <summary>
        /// 分拣步骤变更事件
        /// </summary>
        public event Action<string> StepChanged;

        /// <summary>
        /// 分拣完成事件
        /// </summary>
        public event Action<bool> SortingCompleted;

        /// <summary>
        /// 错误事件
        /// </summary>
        public event Action<string> ErrorOccurred;

        public SortingService(IMotion motion, MotionConfig config, LogHelper logHelper)
        {
            _motion = motion ?? throw new ArgumentNullException(nameof(motion));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logHelper = logHelper ?? throw new ArgumentNullException(nameof(logHelper));
        }

        /// <summary>
        /// 启动分拣流程
        /// </summary>
        public bool StartSorting(DetectionResult result)
        {
            try
            {
                if (result == null)
                {
                    ErrorOccurred?.Invoke("检测结果为空");
                    return false;
                }

                _sortingCts = new CancellationTokenSource();

                Task.Run(() => ExecuteSortingFlow(result, _sortingCts.Token), _sortingCts.Token);

                return true;
            }
            catch (Exception ex)
            {
                _logHelper.LogException("启动分拣失败", ex);
                ErrorOccurred?.Invoke($"启动分拣失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 停止分拣
        /// </summary>
        public void StopSorting()
        {
            try
            {
                _sortingCts?.Cancel();
                _motion.Stop();
                _logHelper.LogOperation("分拣流程已停止");
            }
            catch (Exception ex)
            {
                _logHelper.LogException("停止分拣失败", ex);
            }
        }

        /// <summary>
        /// 是否正在分拣中
        /// </summary>
        public bool IsSorting => _motion.State == DeviceState.Moving;

        /// <summary>
        /// 等待分拣完成（阻塞方式，保留兼容）
        /// </summary>
        public void WaitSortingComplete()
        {
            try
            {
                while (IsSorting)
                {
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                _logHelper.LogException("等待分拣完成失败", ex);
            }
        }

        /// <summary>
        /// 执行分拣流程
        /// </summary>
        private async Task ExecuteSortingFlow(DetectionResult result, CancellationToken token)
        {
            try
            {
                // 步骤1: 移动到检测位置
                StepChanged?.Invoke("移动到检测位置");
                await MoveToPosition(result.CenterX_Physical, result.CenterY_Physical, 0, token);

                if (token.IsCancellationRequested) return;

                // 步骤2: 下降抓取
                StepChanged?.Invoke("下降抓取");
                await MoveToPosition(result.CenterX_Physical, result.CenterY_Physical, _config.GrabHeight, token);

                if (token.IsCancellationRequested) return;

                // 模拟抓取动作
                await Task.Delay(300, token);

                // 步骤3: 上升
                StepChanged?.Invoke("上升");
                await MoveToPosition(result.CenterX_Physical, result.CenterY_Physical, 0, token);

                if (token.IsCancellationRequested) return;

                // 步骤4: 移动到分拣区域
                double targetX, targetY;
                string areaName;

                if (result.IsQualified)
                {
                    targetX = _config.AreaX;
                    targetY = _config.AreaY;
                    areaName = "A区（合格）";
                }
                else
                {
                    targetX = _config.AreaBX;
                    targetY = _config.AreaBY;
                    areaName = "B区（不合格）";
                }

                StepChanged?.Invoke($"移动到{areaName}");
                await MoveToPosition(targetX, targetY, 0, token);

                if (token.IsCancellationRequested) return;

                // 步骤5: 下降放置
                StepChanged?.Invoke("下降放置");
                double placeHeight = result.IsQualified ? _config.PlaceHeightA : _config.PlaceHeightB;
                await MoveToPosition(targetX, targetY, placeHeight, token);

                if (token.IsCancellationRequested) return;

                // 模拟放置动作
                await Task.Delay(300, token);

                // 步骤6: 上升
                StepChanged?.Invoke("上升");
                await MoveToPosition(targetX, targetY, 0, token);

                if (token.IsCancellationRequested) return;

                // 步骤7: 回到原点
                StepChanged?.Invoke("回到原点");
                await MoveToPosition(0, 0, 0, token);

                StepChanged?.Invoke("分拣完成");
                SortingCompleted?.Invoke(true);

                _logHelper.LogOperation($"分拣完成: 零件{result.PartId} → {areaName}");
            }
            catch (OperationCanceledException)
            {
                _logHelper.LogOperation("分拣流程被取消");
                SortingCompleted?.Invoke(false);
            }
            catch (Exception ex)
            {
                _logHelper.LogException("分拣流程异常", ex);
                ErrorOccurred?.Invoke($"分拣异常: {ex.Message}");
                SortingCompleted?.Invoke(false);
            }
        }

        /// <summary>
        /// 移动到指定位置
        /// </summary>
        private async Task MoveToPosition(double x, double y, double z, CancellationToken token)
        {
            if (token.IsCancellationRequested) return;

            // XY轴联动
            if (!_motion.MoveXY(x, y))
            {
                throw new Exception($"XY轴移动失败: X={x}, Y={y}");
            }

            // 等待XY到位
            await WaitForPosition(_config.PositionWaitTime, token);

            if (token.IsCancellationRequested) return;

            // Z轴移动
            if (!_motion.MoveSingleAxis(2, z))
            {
                throw new Exception($"Z轴移动失败: Z={z}");
            }

            // 等待Z到位
            await WaitForPosition(_config.PositionWaitTime, token);
        }

        /// <summary>
        /// 等待定位完成
        /// </summary>
        private async Task WaitForPosition(int waitTimeMs, CancellationToken token)
        {
            await Task.Delay(waitTimeMs, token);
        }
    }
}
