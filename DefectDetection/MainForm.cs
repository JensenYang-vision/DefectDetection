using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using DefectDetection.Helpers.Camera;
using DefectDetection.Helpers.Data;
using DefectDetection.Helpers.Motion;
using DefectDetection.Models;
using DefectDetection.Services;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace DefectDetection
{
    public partial class MainForm : Form
    {
        // 帮助类
        private ConfigHelper _configHelper;
        private LogHelper _logHelper;
        private StatHelper _statHelper;
        private ICamera _camera;
        private IMotion _motion;

        // 服务
        private VisionDetectionService _visionService;
        private SortingService _sortingService;
        private StateMachineService _stateMachine;
        private SchedulerService _schedulerService;

        // 系统配置
        private SystemConfig _config;

        public MainForm()
        {
            InitializeComponent();
            InitializeServices();
            InitializeUI();
            SubscribeEvents();
        }

        /// <summary>
        /// 初始化服务
        /// </summary>
        private void InitializeServices()
        {
            try
            {
                // 1. 初始化数据层
                _configHelper = new ConfigHelper();
                _config = _configHelper.Load();

                _logHelper = new LogHelper();
                _statHelper = new StatHelper();

                // 2. 初始化设备驱动层（使用工厂模式）
                _camera = CameraFactory.CreateCamera(_config);
                _motion = MotionFactory.CreateMotion(_config);

                _logHelper.LogSystem($"设备初始化: 相机={_config.CameraType}, 运动={_config.MotionType}");

                // 3. 初始化业务逻辑层
                _stateMachine = new StateMachineService(_logHelper);
                _visionService = new VisionDetectionService(_config.Vision, _logHelper);
                _sortingService = new SortingService(_motion, _config.Motion, _logHelper);
                _schedulerService = new SchedulerService(
                    _camera, _visionService, _sortingService,
                    _stateMachine, _statHelper, _logHelper);

                _logHelper.LogSystem("系统初始化完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"系统初始化失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 初始化UI
        /// </summary>
        private void InitializeUI()
        {
            // 更新状态栏
            UpdateStatusBar();
            UpdateStatisticsDisplay();

            // 加载日志
            var recentLogs = _logHelper.GetRecentLogs(50);
            foreach (var log in recentLogs)
            {
                AppendLog(log);
            }
        }

        /// <summary>
        /// 订阅事件
        /// </summary>
        private void SubscribeEvents()
        {
            // 相机事件
            _camera.StateChanged += OnCameraStateChanged;
            _camera.ImageCaptured += OnImageCaptured;
            _camera.ErrorOccurred += OnCameraError;

            // 运动控制事件
            _motion.StateChanged += OnMotionStateChanged;
            _motion.PositionChanged += OnPositionChanged;
            _motion.AlarmOccurred += OnMotionAlarm;
            _motion.ErrorOccurred += OnMotionError;

            // 视觉检测事件
            _visionService.DetectionCompleted += OnDetectionCompleted;
            _visionService.ErrorOccurred += OnVisionError;

            // 分拣服务事件
            _sortingService.SortingCompleted += OnSortingCompleted;
            _sortingService.StepChanged += OnSortingStepChanged;
            _sortingService.ErrorOccurred += OnSortingError;

            // 状态机事件
            _stateMachine.StateChanged += OnDeviceStateChanged;

            // 调度服务事件
            _schedulerService.CycleCompleted += OnCycleCompleted;
            _schedulerService.ErrorOccurred += OnSchedulerError;
            _schedulerService.StatusMessage += OnStatusMessage;

            // 统计事件
            _statHelper.StatisticsChanged += OnStatisticsChanged;

            // 日志事件
            _logHelper.LogAdded += OnLogAdded;
        }

        #region 相机事件处理

        private void OnCameraStateChanged(CameraState state)
        {
            InvokeUI(() =>
            {
                lblCameraStatus.Text = state switch
                {
                    CameraState.Disconnected => "未连接",
                    CameraState.Connected => "已连接",
                    CameraState.Capturing => "采集中",
                    CameraState.Error => "异常",
                    _ => "未知"
                };
                lblCameraStatus.ForeColor = state switch
                {
                    CameraState.Disconnected => Color.Gray,
                    CameraState.Connected => Color.Green,
                    CameraState.Capturing => Color.Blue,
                    CameraState.Error => Color.Red,
                    _ => Color.Black
                };
            });
        }

        private void OnImageCaptured(Mat image)
        {
            InvokeUI(() =>
            {
                try
                {
                    var bitmap = BitmapConverter.ToBitmap(image);

                    // 原始图像区域：始终显示实时画面
                    var oldOriginal = picOriginalImage.Image;
                    picOriginalImage.Image = bitmap.Clone() as Image;
                    oldOriginal?.Dispose();

                    // 处理后图像区域：连续采集时也实时显示画面
                    // （执行检测时会被 OnDetectionCompleted 替换为效果图）
                    var oldProcessed = picProcessedImage.Image;
                    picProcessedImage.Image = bitmap;
                    oldProcessed?.Dispose();
                }
                catch (Exception ex)
                {
                    _logHelper.LogException("显示图像异常", ex);
                }
            });
        }

        private void OnCameraError(string error)
        {
            InvokeUI(() =>
            {
                MessageBox.Show($"相机异常: {error}", "相机错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            });
        }

        #endregion

        #region 运动控制事件处理

        private void OnMotionStateChanged(DeviceState state)
        {
            InvokeUI(() =>
            {
                lblMotionStatus.Text = state switch
                {
                    DeviceState.Disconnected => "未连接",
                    DeviceState.Connected => "已连接",
                    DeviceState.Homing => "回零中",
                    DeviceState.Moving => "运动中",
                    DeviceState.Error => "异常",
                    _ => "未知"
                };
                lblMotionStatus.ForeColor = state switch
                {
                    DeviceState.Disconnected => Color.Gray,
                    DeviceState.Connected => Color.Green,
                    DeviceState.Homing => Color.Blue,
                    DeviceState.Moving => Color.Orange,
                    DeviceState.Error => Color.Red,
                    _ => Color.Black
                };
            });
        }

        private void OnPositionChanged(double x, double y, double z)
        {
            InvokeUI(() =>
            {
                lblPosX.Text = $"X: {x:F2} mm";
                lblPosY.Text = $"Y: {y:F2} mm";
                lblPosZ.Text = $"Z: {z:F2} mm";
            });
        }

        private void OnMotionAlarm(int code, string message)
        {
            InvokeUI(() =>
            {
                MessageBox.Show($"运动控制报警 [{code}]: {message}", "报警", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            });
        }

        private void OnMotionError(string error)
        {
            InvokeUI(() =>
            {
                MessageBox.Show($"运动控制异常: {error}", "运动错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            });
        }

        #endregion

        #region 视觉检测事件处理

        private void OnDetectionCompleted(DetectionResult result, Mat processedImage)
        {
            InvokeUI(() =>
            {
                try
                {
                    // 显示处理后图像
                    var bitmap = BitmapConverter.ToBitmap(processedImage);
                    var oldImage = picProcessedImage.Image;
                    picProcessedImage.Image = bitmap;
                    oldImage?.Dispose();

                    // 显示检测结果
                    lblResult.Text = result.IsQualified ? "合格" : "不合格";
                    lblResult.ForeColor = result.IsQualified ? Color.Green : Color.Red;
                    lblDefectType.Text = result.DefectType ?? "无";
                    lblDetectionTime.Text = $"{result.DetectionTimeMs:F1} ms";
                    lblPartSize.Text = $"宽: {result.Width:F2} mm, 高: {result.Height:F2} mm";
                    lblPartPosition.Text = $"X: {result.CenterX_Physical:F2} mm, Y: {result.CenterY_Physical:F2} mm";
                }
                catch (Exception ex)
                {
                    _logHelper.LogException("显示检测结果异常", ex);
                }
            });
        }

        private void OnVisionError(string error)
        {
            InvokeUI(() =>
            {
                MessageBox.Show($"视觉检测异常: {error}", "检测错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            });
        }

        #endregion

        #region 分拣服务事件处理

        private void OnSortingCompleted(bool success)
        {
            InvokeUI(() =>
            {
                lblSortingStatus.Text = success ? "分拣完成" : "分拣失败";
                lblSortingStatus.ForeColor = success ? Color.Green : Color.Red;
            });
        }

        private void OnSortingStepChanged(string step)
        {
            InvokeUI(() =>
            {
                lblCurrentStep.Text = step;
            });
        }

        private void OnSortingError(string error)
        {
            InvokeUI(() =>
            {
                MessageBox.Show($"分拣异常: {error}", "分拣错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            });
        }

        #endregion

        #region 状态机事件处理

        private void OnDeviceStateChanged(DeviceState oldState, DeviceState newState)
        {
            InvokeUI(() =>
            {
                lblDeviceState.Text = _stateMachine.GetStateDescription();
                lblDeviceState.ForeColor = newState switch
                {
                    DeviceState.Disconnected => Color.Gray,
                    DeviceState.Connected => Color.Green,
                    DeviceState.Homing => Color.Blue,
                    DeviceState.Detecting => Color.Orange,
                    DeviceState.Moving => Color.Orange,
                    DeviceState.Error => Color.Red,
                    DeviceState.Stopped => Color.Yellow,
                    _ => Color.Black
                };
            });
        }

        #endregion

        #region 调度服务事件处理

        private void OnCycleCompleted(DetectionResult result)
        {
            InvokeUI(() =>
            {
                lblCycleCount.Text = $"已完成: {_statHelper.TodayData.TotalCount}";
            });
        }

        private void OnSchedulerError(string error)
        {
            InvokeUI(() =>
            {
                MessageBox.Show($"调度异常: {error}", "调度错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            });
        }

        private void OnStatusMessage(string message)
        {
            InvokeUI(() =>
            {
                lblStatusMessage.Text = message;
            });
        }

        #endregion

        #region 统计与日志事件处理

        private void OnStatisticsChanged(StatisticsData data)
        {
            InvokeUI(() =>
            {
                UpdateStatisticsDisplay();
            });
        }

        private void OnLogAdded(LogEntry entry)
        {
            InvokeUI(() =>
            {
                AppendLog(entry);
            });
        }

        #endregion

        #region UI更新方法

        private void InvokeUI(Action action)
        {
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }
        }

        private void UpdateStatusBar()
        {
            lblDeviceState.Text = _stateMachine.GetStateDescription();
            lblCameraStatus.Text = _camera.State.ToString();
            lblMotionStatus.Text = _motion.State.ToString();
        }

        private void UpdateStatisticsDisplay()
        {
            var data = _statHelper.TodayData;
            lblTotalCount.Text = $"总数: {data.TotalCount}";
            lblQualifiedCount.Text = $"合格: {data.QualifiedCount}";
            lblUnqualifiedCount.Text = $"不合格: {data.UnqualifiedCount}";
            lblQualifiedRate.Text = $"合格率: {data.QualifiedRate:F1}%";
        }

        private void AppendLog(LogEntry entry)
        {
            var color = entry.Level switch
            {
                Models.LogLevel.Debug => Color.Gray,
                Models.LogLevel.Info => Color.Black,
                Models.LogLevel.Warning => Color.Orange,
                Models.LogLevel.Error => Color.Red,
                Models.LogLevel.Fatal => Color.DarkRed,
                _ => Color.Black
            };

            txtLog.SelectionStart = txtLog.TextLength;
            txtLog.SelectionLength = 0;
            txtLog.SelectionColor = color;
            txtLog.AppendText(entry.ToString() + Environment.NewLine);
            txtLog.ScrollToCaret();

            // 限制日志显示数量
            if (txtLog.Lines.Length > 1000)
            {
                txtLog.Text = string.Join(Environment.NewLine, txtLog.Lines[^500..]);
            }
        }

        #endregion

        #region 按钮事件处理

        private async void btnConnectCamera_Click(object sender, EventArgs e)
        {
            try
            {
                btnConnectCamera.Enabled = false;
                _logHelper.LogOperation("连接相机...");

                var cameraIndex = _config.UseSimulatedCamera ? 0 : _config.CameraIndex;
                var success = _camera.Initialize(cameraIndex);

                if (success)
                {
                    // 更新状态机：相机已连接
                    _stateMachine.TryChangeState(DeviceState.Connected, out _);
                    _logHelper.LogOperation("相机连接成功");
                    MessageBox.Show("相机连接成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    _logHelper.LogWarning("相机连接失败");
                    MessageBox.Show("相机连接失败", "失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                _logHelper.LogException("连接相机异常", ex);
                MessageBox.Show($"连接相机异常: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnConnectCamera.Enabled = true;
            }
        }

        private void btnDisconnectCamera_Click(object sender, EventArgs e)
        {
            try
            {
                _camera.Close();
                _logHelper.LogOperation("相机已断开");
            }
            catch (Exception ex)
            {
                _logHelper.LogException("断开相机异常", ex);
            }
        }

        private void btnStartCapture_Click(object sender, EventArgs e)
        {
            try
            {
                if (_camera.IsContinuousCapture)
                {
                    _camera.StopContinuousCapture();
                    btnStartCapture.Text = "开始采集";
                    _logHelper.LogOperation("停止连续采集");
                }
                else
                {
                    if (_camera.StartContinuousCapture())
                    {
                        btnStartCapture.Text = "停止采集";
                        _logHelper.LogOperation("开始连续采集");
                    }
                }
            }
            catch (Exception ex)
            {
                _logHelper.LogException("采集控制异常", ex);
            }
        }

        private async void btnConnectMotion_Click(object sender, EventArgs e)
        {
            try
            {
                btnConnectMotion.Enabled = false;
                _logHelper.LogOperation("连接运动控制卡...");

                var success = _motion.Initialize();

                if (success)
                {
                    // 更新状态机：运动控制卡已连接
                    _stateMachine.TryChangeState(DeviceState.Connected, out _);

                    // 使能各轴
                    _motion.EnableAxis(0);
                    _motion.EnableAxis(1);
                    _motion.EnableAxis(2);

                    _logHelper.LogOperation("运动控制卡连接成功");
                    MessageBox.Show("运动控制卡连接成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    _logHelper.LogWarning("运动控制卡连接失败");
                    MessageBox.Show("运动控制卡连接失败", "失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                _logHelper.LogException("连接运动控制卡异常", ex);
                MessageBox.Show($"连接运动控制卡异常: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnConnectMotion.Enabled = true;
            }
        }

        private void btnDisconnectMotion_Click(object sender, EventArgs e)
        {
            try
            {
                _motion.Dispose();
                _logHelper.LogOperation("运动控制卡已断开");
            }
            catch (Exception ex)
            {
                _logHelper.LogException("断开运动控制卡异常", ex);
            }
        }

        private async void btnHome_Click(object sender, EventArgs e)
        {
            try
            {
                if (_motion.State != DeviceState.Connected)
                {
                    MessageBox.Show("设备未连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                btnHome.Enabled = false;
                _logHelper.LogOperation("开始回零...");

                var success = _motion.Home();

                if (success)
                {
                    _logHelper.LogOperation("回零成功");
                    MessageBox.Show("回零成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    _logHelper.LogWarning("回零失败");
                    MessageBox.Show("回零失败", "失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                _logHelper.LogException("回零异常", ex);
                MessageBox.Show($"回零异常: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnHome.Enabled = true;
            }
        }

        private async void btnSingleCycle_Click(object sender, EventArgs e)
        {
            try
            {
                btnSingleCycle.Enabled = false;
                _logHelper.LogOperation("执行单次检测分拣流程...");

                var success = await _schedulerService.ExecuteSingleCycle();

                if (success)
                {
                    _logHelper.LogOperation("单次流程执行成功");
                }
                else
                {
                    _logHelper.LogWarning("单次流程执行失败");
                }
            }
            catch (Exception ex)
            {
                _logHelper.LogException("单次流程异常", ex);
                MessageBox.Show($"单次流程异常: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSingleCycle.Enabled = true;
            }
        }

        private void btnStartAuto_Click(object sender, EventArgs e)
        {
            try
            {
                if (_schedulerService.IsRunning)
                {
                    _schedulerService.StopAutoRun();
                    btnStartAuto.Text = "启动自动";
                    _logHelper.LogOperation("停止自动流程");
                }
                else
                {
                    if (_schedulerService.StartAutoRun())
                    {
                        btnStartAuto.Text = "停止自动";
                        _logHelper.LogOperation("启动自动流程");
                    }
                }
            }
            catch (Exception ex)
            {
                _logHelper.LogException("自动流程控制异常", ex);
                MessageBox.Show($"自动流程控制异常: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEmergencyStop_Click(object sender, EventArgs e)
        {
            try
            {
                _motion.EmergencyStop();
                _schedulerService.StopAutoRun();
                _logHelper.LogOperation("急停触发");
                MessageBox.Show("急停已触发", "急停", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                _logHelper.LogException("急停异常", ex);
            }
        }

        private void btnClearAlarm_Click(object sender, EventArgs e)
        {
            try
            {
                if (_motion.ClearAlarm())
                {
                    _logHelper.LogOperation("报警已清除");
                    MessageBox.Show("报警已清除", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logHelper.LogException("清除报警异常", ex);
            }
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            try
            {
                var configForm = new ConfigForm(_configHelper, _logHelper);
                configForm.ShowDialog();
            }
            catch (Exception ex)
            {
                _logHelper.LogException("打开配置窗口异常", ex);
            }
        }

        private void btnExportLog_Click(object sender, EventArgs e)
        {
            try
            {
                using var dialog = new SaveFileDialog();
                dialog.Filter = "文本文件|*.txt|所有文件|*.*";
                dialog.FileName = $"日志_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (_logHelper.ExportLogs(dialog.FileName))
                    {
                        MessageBox.Show("日志导出成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("日志导出失败", "失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                _logHelper.LogException("导出日志异常", ex);
            }
        }

        private void btnExportStatistics_Click(object sender, EventArgs e)
        {
            try
            {
                using var dialog = new SaveFileDialog();
                dialog.Filter = "CSV文件|*.csv|所有文件|*.*";
                dialog.FileName = $"统计数据_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (_statHelper.ExportToCsv(dialog.FileName))
                    {
                        MessageBox.Show("统计导出成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("统计导出失败", "失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                _logHelper.LogException("导出统计异常", ex);
            }
        }

        #endregion

        #region 窗体事件

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                _logHelper.LogOperation("系统关闭");

                // 停止自动流程
                _schedulerService?.StopAutoRun();

                // 释放资源
                _schedulerService?.Dispose();
                _camera?.Dispose();
                _motion?.Dispose();
                _logHelper?.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"关闭异常: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            base.OnFormClosing(e);
        }

        #endregion
    }
}
