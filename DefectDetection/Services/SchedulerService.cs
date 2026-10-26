using System;
using System.Threading;
using System.Threading.Tasks;
using DefectDetection.Helpers.Camera;
using DefectDetection.Helpers.Data;
using DefectDetection.Models;
using OpenCvSharp;

namespace DefectDetection.Services
{
    /// <summary>
    /// 调度服务 - 协调视觉检测、运动分拣、数据采集的多线程并行处理
    /// </summary>
    public class SchedulerService
    {
        private readonly ICamera _camera;
        private readonly VisionDetectionService _visionService;
        private readonly SortingService _sortingService;
        private readonly StateMachineService _stateMachine;
        private readonly StatHelper _statHelper;
        private readonly LogHelper _logHelper;

        private CancellationTokenSource _cts;
        private Task _autoTask;
        private bool _isRunning = false;
        private bool _isSingleCycleRunning = false;

        /// <summary>
        /// 自动流程完成事件
        /// </summary>
        public event Action<DetectionResult> CycleCompleted;

        /// <summary>
        /// 异常事件
        /// </summary>
        public event Action<string> ErrorOccurred;

        /// <summary>
        /// 状态消息事件
        /// </summary>
        public event Action<string> StatusMessage;

        /// <summary>
        /// 是否正在运行自动流程
        /// </summary>
        public bool IsRunning => _isRunning;

        public SchedulerService(
            ICamera camera,
            VisionDetectionService visionService,
            SortingService sortingService,
            StateMachineService stateMachine,
            StatHelper statHelper,
            LogHelper logHelper)
        {
            _camera = camera ?? throw new ArgumentNullException(nameof(camera));
            _visionService = visionService ?? throw new ArgumentNullException(nameof(visionService));
            _sortingService = sortingService ?? throw new ArgumentNullException(nameof(sortingService));
            _stateMachine = stateMachine ?? throw new ArgumentNullException(nameof(stateMachine));
            _statHelper = statHelper ?? throw new ArgumentNullException(nameof(statHelper));
            _logHelper = logHelper ?? throw new ArgumentNullException(nameof(logHelper));
        }

        /// <summary>
        /// 启动自动流程
        /// </summary>
        public bool StartAutoRun()
        {
            try
            {
                if (_isRunning)
                {
                    ErrorOccurred?.Invoke("自动流程已在运行中");
                    return false;
                }

                // 检查状态
                if (!_stateMachine.CanStartAutoSorting(out string reason))
                {
                    ErrorOccurred?.Invoke($"无法启动自动流程: {reason}");
                    return false;
                }

                // 检查设备
                if (!_camera.IsConnected)
                {
                    ErrorOccurred?.Invoke("相机未连接");
                    return false;
                }

                _cts = new CancellationTokenSource();
                _isRunning = true;

                _autoTask = Task.Run(() => AutoRunLoop(_cts.Token), _cts.Token);

                _logHelper.LogOperation("启动自动流程");
                StatusMessage?.Invoke("自动流程已启动");

                return true;
            }
            catch (Exception ex)
            {
                _logHelper.LogException("启动自动流程异常", ex);
                ErrorOccurred?.Invoke($"启动自动流程异常: {ex.Message}");
                _isRunning = false;
                return false;
            }
        }

        /// <summary>
        /// 停止自动流程
        /// </summary>
        public void StopAutoRun()
        {
            try
            {
                if (!_isRunning) return;

                _cts?.Cancel();
                _sortingService.StopSorting();

                _logHelper.LogOperation("停止自动流程");
                StatusMessage?.Invoke("自动流程已停止");

                _isRunning = false;
            }
            catch (Exception ex)
            {
                _logHelper.LogException("停止自动流程异常", ex);
            }
        }

        /// <summary>
        /// 执行单次检测分拣流程
        /// </summary>
        public async Task<bool> ExecuteSingleCycle()
        {
            // 防止重复点击
            if (_isSingleCycleRunning)
            {
                ErrorOccurred?.Invoke("单次流程正在执行中，请勿重复点击");
                return false;
            }

            _isSingleCycleRunning = true;

            try
            {
                // 检查状态
                if (!_stateMachine.CanStartAutoSorting(out string reason))
                {
                    ErrorOccurred?.Invoke($"无法执行单次流程: {reason}");
                    return false;
                }

                StatusMessage?.Invoke("正在采集图像...");

                // 1. 采集图像
                var image = _camera.CaptureSingle();
                if (image == null || image.Empty())
                {
                    ErrorOccurred?.Invoke("图像采集失败");
                    return false;
                }

                // 2. 执行检测
                StatusMessage?.Invoke("正在检测...");
                _stateMachine.TryChangeState(DeviceState.Detecting, out _);

                var result = await _visionService.DetectAsync(image);
                image.Dispose();

                if (result == null)
                {
                    ErrorOccurred?.Invoke("检测失败");
                    _stateMachine.TryChangeState(DeviceState.Connected, out _);
                    return false;
                }

                // 3. 更新统计
                _statHelper.AddResult(result);

                // 4. 执行分拣
                StatusMessage?.Invoke("正在分拣...");
                _stateMachine.TryChangeState(DeviceState.Moving, out _);

                var sortingSuccess = _sortingService.StartSorting(result);
                if (!sortingSuccess)
                {
                    ErrorOccurred?.Invoke("启动分拣失败");
                    _stateMachine.TryChangeState(DeviceState.Connected, out _);
                    return false;
                }

                // 异步等待分拣完成（不阻塞UI）
                await WaitSortingCompleteAsync();

                // 5. 恢复状态
                _stateMachine.TryChangeState(DeviceState.Connected, out _);

                StatusMessage?.Invoke($"单次流程完成: {(result.IsQualified ? "合格" : "不合格")}");
                CycleCompleted?.Invoke(result);

                return true;
            }
            catch (Exception ex)
            {
                _logHelper.LogException("单次流程异常", ex);
                ErrorOccurred?.Invoke($"单次流程异常: {ex.Message}");
                _stateMachine.ForceSetState(DeviceState.Connected);
                return false;
            }
            finally
            {
                _isSingleCycleRunning = false;
            }
        }

        /// <summary>
        /// 异步等待分拣完成（不阻塞UI线程）
        /// </summary>
        private async Task WaitSortingCompleteAsync()
        {
            while (_sortingService.IsSorting)
            {
                await Task.Delay(100);
            }
        }

        /// <summary>
        /// 自动循环流程
        /// </summary>
        private async Task AutoRunLoop(CancellationToken token)
        {
            try
            {
                _logHelper.LogOperation("自动循环流程启动");

                while (!token.IsCancellationRequested && _isRunning)
                {
                    // 执行单次流程
                    var success = await ExecuteSingleCycle();

                    if (!success)
                    {
                        _logHelper.LogWarning("单次流程失败，继续下一次");
                    }

                    // 短暂延迟，避免过快循环
                    await Task.Delay(500, token);
                }

                _logHelper.LogOperation("自动循环流程结束");
            }
            catch (OperationCanceledException)
            {
                _logHelper.LogOperation("自动循环流程被取消");
            }
            catch (Exception ex)
            {
                _logHelper.LogException("自动循环流程异常", ex);
                ErrorOccurred?.Invoke($"自动流程异常: {ex.Message}");
            }
            finally
            {
                _isRunning = false;
                _stateMachine.ForceSetState(DeviceState.Connected);
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            StopAutoRun();
            _cts?.Dispose();
        }
    }
}
