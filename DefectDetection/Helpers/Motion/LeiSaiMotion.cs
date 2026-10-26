using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using DefectDetection.Models;

namespace DefectDetection.Helpers.Motion
{
    /// <summary>
    /// 雷赛DMC3400运动控制卡 - 基于雷赛DMC3400 SDK实现（4轴脉冲）
    /// 需要安装雷赛DMC3400驱动并将dmc3400.dll放到程序目录
    /// </summary>
    public class LeiSaiMotion : IMotion
    {
        #region 雷赛DMC3400 SDK DllImport

        [DllImport("dmc3400.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short dmc_board_init();

        [DllImport("dmc3400.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short dmc_board_reset();

        [DllImport("dmc3400.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short dmc_set_pulse_outmode(ushort CardNo, ushort axis, ushort outmode);

        [DllImport("dmc3400.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short dmc_set_profile(ushort CardNo, ushort axis, double Min_Vel, double Max_Vel, double Tacc, double Tdec, double Stop_Vel);

        [DllImport("dmc3400.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short dmc_pmove(ushort CardNo, ushort axis, double Dist, ushort posi_mode);

        [DllImport("dmc3400.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short dmc_line2(ushort CardNo, ushort axis1, ushort axis2, double Distance1, double Distance2, ushort posi_mode);

        [DllImport("dmc3400.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short dmc_line3(ushort CardNo, ushort axis1, ushort axis2, ushort axis3, double Distance1, double Distance2, double Distance3, ushort posi_mode);

        [DllImport("dmc3400.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short dmc_check_done(ushort CardNo, ushort axis);

        [DllImport("dmc3400.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern double dmc_get_position(ushort CardNo, ushort axis);

        [DllImport("dmc3400.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short dmc_set_position(ushort CardNo, ushort axis, double position);

        [DllImport("dmc3400.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short dmc_axis_io_status(ushort CardNo, ushort axis);

        [DllImport("dmc3400.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short dmc_home_move(ushort CardNo, ushort axis, ushort home_dir, ushort home_mode, double vel, ushort EZ_count);

        [DllImport("dmc3400.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short dmc_stop(ushort CardNo, ushort axis, ushort stop_mode);

        [DllImport("dmc3400.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short dmc_emg_stop(ushort CardNo);

        [DllImport("dmc3400.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short dmc_write_sevon_pin(ushort CardNo, ushort axis, ushort on_off);

        #endregion

        private ushort _cardNo = 0;
        private MotionConfig _config;
        private DeviceState _state = DeviceState.Disconnected;
        private bool _isHomed = false;

        private double _positionX = 0;
        private double _positionY = 0;
        private double _positionZ = 0;

        private bool _isAlarm = false;
        private int _alarmCode = 0;
        private string _alarmMessage = "";
        private bool _sdkAvailable = true;

        private CancellationTokenSource _cts;
        private Task _motionTask;
        private readonly object _lockObj = new object();

        public event Action<DeviceState> StateChanged;
        public event Action<double, double, double> PositionChanged;
        public event Action<int, string> AlarmOccurred;
        public event Action<string> ErrorOccurred;
        public event Action MotionCompleted;

        public DeviceState State => _state;
        public bool IsHomed => _isHomed;
        public bool IsAlarm => _isAlarm;
        public bool IsSimulated => false;
        public double PositionX => _positionX;
        public double PositionY => _positionY;
        public double PositionZ => _positionZ;

        public LeiSaiMotion(MotionConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            CheckSdkAvailability();
        }

        /// <summary>
        /// 检查雷赛SDK是否可用
        /// </summary>
        private void CheckSdkAvailability()
        {
            try
            {
                var dllPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dmc3400.dll");
                if (!System.IO.File.Exists(dllPath))
                {
                    _sdkAvailable = false;
                    ErrorOccurred?.Invoke("雷赛dmc3400.dll未找到，请安装雷赛DMC3400驱动或将dmc3400.dll放到程序目录");
                }
            }
            catch
            {
                _sdkAvailable = false;
            }
        }

        public bool Initialize()
        {
            try
            {
                if (!_sdkAvailable)
                {
                    ErrorOccurred?.Invoke("雷赛SDK不可用，请使用模拟运动模式");
                    return false;
                }

                // 初始化控制卡
                short ret = dmc_board_init();
                if (ret != 0)
                {
                    SetState(DeviceState.Error);
                    ErrorOccurred?.Invoke($"初始化雷赛控制卡失败，错误码: {ret}");
                    return false;
                }

                // 配置各轴参数
                ConfigureAxes();

                SetState(DeviceState.Connected);
                return true;
            }
            catch (DllNotFoundException)
            {
                _sdkAvailable = false;
                SetState(DeviceState.Error);
                ErrorOccurred?.Invoke("dmc3400.dll未找到，请安装雷赛DMC3400驱动");
                return false;
            }
            catch (Exception ex)
            {
                SetState(DeviceState.Error);
                ErrorOccurred?.Invoke($"初始化雷赛异常: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 配置各轴参数
        /// </summary>
        private void ConfigureAxes()
        {
            try
            {
                // 设置脉冲输出模式（0=脉冲+方向）
                for (ushort i = 0; i < 4; i++)
                {
                    dmc_set_pulse_outmode(_cardNo, i, 0);
                }

                // 设置X轴参数
                dmc_set_profile(_cardNo, 0, 0, _config.SpeedX, _config.AccelX, _config.AccelX, 0);

                // 设置Y轴参数
                dmc_set_profile(_cardNo, 1, 0, _config.SpeedY, _config.AccelY, _config.AccelY, 0);

                // 设置Z轴参数
                dmc_set_profile(_cardNo, 2, 0, _config.SpeedZ, _config.AccelZ, _config.AccelZ, 0);
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"配置轴参数异常: {ex.Message}");
            }
        }

        public bool EnableAxis(int axis)
        {
            try
            {
                if (_state == DeviceState.Disconnected)
                {
                    ErrorOccurred?.Invoke("设备未连接");
                    return false;
                }

                short ret = dmc_write_sevon_pin(_cardNo, (ushort)axis, 1);
                if (ret != 0)
                {
                    ErrorOccurred?.Invoke($"轴{axis}使能失败，错误码: {ret}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"轴使能异常: {ex.Message}");
                return false;
            }
        }

        public bool DisableAxis(int axis)
        {
            try
            {
                if (_state == DeviceState.Disconnected)
                {
                    return false;
                }

                short ret = dmc_write_sevon_pin(_cardNo, (ushort)axis, 0);
                if (ret != 0)
                {
                    ErrorOccurred?.Invoke($"轴{axis}失能失败，错误码: {ret}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"轴失能异常: {ex.Message}");
                return false;
            }
        }

        public bool Home()
        {
            try
            {
                if (_state == DeviceState.Disconnected)
                {
                    ErrorOccurred?.Invoke("设备未连接");
                    return false;
                }

                SetState(DeviceState.Homing);

                _motionTask = Task.Run(() =>
                {
                    try
                    {
                        // 依次回零X、Y、Z轴
                        for (ushort i = 0; i < 3; i++)
                        {
                            if (_state != DeviceState.Homing) break;

                            short ret = dmc_home_move(_cardNo, i, 0, 0, 10, 1);
                            if (ret != 0)
                            {
                                ErrorOccurred?.Invoke($"轴{i}回零失败，错误码: {ret}");
                                break;
                            }

                            // 等待回零完成
                            while (_state == DeviceState.Homing)
                            {
                                short done = dmc_check_done(_cardNo, i);
                                if (done == 0) break;
                                Thread.Sleep(100);
                            }

                            // 读取当前位置
                            UpdatePosition();
                        }

                        _isHomed = true;
                        UpdatePosition();
                        SetState(DeviceState.Connected);
                        MotionCompleted?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        ErrorOccurred?.Invoke($"回零异常: {ex.Message}");
                        SetState(DeviceState.Error);
                    }
                });

                return true;
            }
            catch (Exception ex)
            {
                SetState(DeviceState.Error);
                ErrorOccurred?.Invoke($"回零异常: {ex.Message}");
                return false;
            }
        }

        public bool MoveSingleAxis(int axis, double position)
        {
            try
            {
                if (_state == DeviceState.Disconnected || _state == DeviceState.Error)
                {
                    ErrorOccurred?.Invoke("设备未连接或异常");
                    return false;
                }

                if (!_isHomed)
                {
                    ErrorOccurred?.Invoke("设备未回零，请先执行回零操作");
                    return false;
                }

                if (!CheckLimit(axis, position))
                {
                    return false;
                }

                SetState(DeviceState.Moving);

                _motionTask = Task.Run(() =>
                {
                    try
                    {
                        short ret = dmc_pmove(_cardNo, (ushort)axis, position, 1);
                        if (ret != 0)
                        {
                            ErrorOccurred?.Invoke($"轴{axis}运动失败，错误码: {ret}");
                            SetState(DeviceState.Error);
                            return;
                        }

                        // 等待运动完成
                        while (_state == DeviceState.Moving)
                        {
                            short done = dmc_check_done(_cardNo, (ushort)axis);
                            if (done == 0) break;

                            UpdatePosition();
                            Thread.Sleep(50);
                        }

                        UpdatePosition();
                        SetState(DeviceState.Connected);
                        MotionCompleted?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        ErrorOccurred?.Invoke($"单轴运动异常: {ex.Message}");
                        SetState(DeviceState.Error);
                    }
                });

                return true;
            }
            catch (Exception ex)
            {
                SetState(DeviceState.Error);
                ErrorOccurred?.Invoke($"单轴运动异常: {ex.Message}");
                return false;
            }
        }

        public bool MoveXY(double x, double y)
        {
            try
            {
                if (_state == DeviceState.Disconnected || _state == DeviceState.Error)
                {
                    ErrorOccurred?.Invoke("设备未连接或异常");
                    return false;
                }

                if (!_isHomed)
                {
                    ErrorOccurred?.Invoke("设备未回零，请先执行回零操作");
                    return false;
                }

                if (!CheckLimit(0, x) || !CheckLimit(1, y))
                {
                    return false;
                }

                SetState(DeviceState.Moving);

                _motionTask = Task.Run(() =>
                {
                    try
                    {
                        short ret = dmc_line2(_cardNo, 0, 1, x, y, 1);
                        if (ret != 0)
                        {
                            ErrorOccurred?.Invoke($"XY插补失败，错误码: {ret}");
                            SetState(DeviceState.Error);
                            return;
                        }

                        // 等待运动完成
                        while (_state == DeviceState.Moving)
                        {
                            short doneX = dmc_check_done(_cardNo, 0);
                            short doneY = dmc_check_done(_cardNo, 1);
                            if (doneX == 0 && doneY == 0) break;

                            UpdatePosition();
                            Thread.Sleep(50);
                        }

                        UpdatePosition();
                        SetState(DeviceState.Connected);
                        MotionCompleted?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        ErrorOccurred?.Invoke($"XY插补异常: {ex.Message}");
                        SetState(DeviceState.Error);
                    }
                });

                return true;
            }
            catch (Exception ex)
            {
                SetState(DeviceState.Error);
                ErrorOccurred?.Invoke($"XY插补异常: {ex.Message}");
                return false;
            }
        }

        public bool MoveXYZ(double x, double y, double z)
        {
            try
            {
                if (_state == DeviceState.Disconnected || _state == DeviceState.Error)
                {
                    ErrorOccurred?.Invoke("设备未连接或异常");
                    return false;
                }

                if (!_isHomed)
                {
                    ErrorOccurred?.Invoke("设备未回零，请先执行回零操作");
                    return false;
                }

                if (!CheckLimit(0, x) || !CheckLimit(1, y) || !CheckLimit(2, z))
                {
                    return false;
                }

                SetState(DeviceState.Moving);

                _motionTask = Task.Run(() =>
                {
                    try
                    {
                        short ret = dmc_line3(_cardNo, 0, 1, 2, x, y, z, 1);
                        if (ret != 0)
                        {
                            ErrorOccurred?.Invoke($"三轴联动失败，错误码: {ret}");
                            SetState(DeviceState.Error);
                            return;
                        }

                        while (_state == DeviceState.Moving)
                        {
                            short doneX = dmc_check_done(_cardNo, 0);
                            short doneY = dmc_check_done(_cardNo, 1);
                            short doneZ = dmc_check_done(_cardNo, 2);
                            if (doneX == 0 && doneY == 0 && doneZ == 0) break;

                            UpdatePosition();
                            Thread.Sleep(50);
                        }

                        UpdatePosition();
                        SetState(DeviceState.Connected);
                        MotionCompleted?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        ErrorOccurred?.Invoke($"三轴联动异常: {ex.Message}");
                        SetState(DeviceState.Error);
                    }
                });

                return true;
            }
            catch (Exception ex)
            {
                SetState(DeviceState.Error);
                ErrorOccurred?.Invoke($"三轴联动异常: {ex.Message}");
                return false;
            }
        }

        public void Stop()
        {
            try
            {
                _cts?.Cancel();

                if (_state != DeviceState.Disconnected)
                {
                    for (ushort i = 0; i < 3; i++)
                    {
                        dmc_stop(_cardNo, i, 1); // 1=减速停止
                    }
                }

                _motionTask?.Wait(1000);

                if (_state == DeviceState.Moving || _state == DeviceState.Homing)
                {
                    SetState(DeviceState.Connected);
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"停止运动异常: {ex.Message}");
            }
        }

        public void EmergencyStop()
        {
            try
            {
                if (_state != DeviceState.Disconnected)
                {
                    dmc_emg_stop(_cardNo);
                }

                Stop();
                SetState(DeviceState.Error);
                _isAlarm = true;
                _alarmCode = 999;
                _alarmMessage = "急停触发";
                AlarmOccurred?.Invoke(_alarmCode, _alarmMessage);
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"急停异常: {ex.Message}");
            }
        }

        public bool ClearAlarm()
        {
            try
            {
                _isAlarm = false;
                _alarmCode = 0;
                _alarmMessage = "";

                if (_state == DeviceState.Error)
                {
                    SetState(DeviceState.Connected);
                }

                return true;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"清除报警异常: {ex.Message}");
                return false;
            }
        }

        public DeviceStateInfo GetStateInfo()
        {
            UpdatePosition();
            CheckAlarm();

            return new DeviceStateInfo
            {
                DeviceName = "雷赛DMC3400",
                State = _state,
                IsHomed = _isHomed,
                IsAlarm = _isAlarm,
                AlarmCode = _alarmCode,
                AlarmMessage = _alarmMessage,
                PositionX = _positionX,
                PositionY = _positionY,
                PositionZ = _positionZ,
                LastUpdateTime = DateTime.Now
            };
        }

        /// <summary>
        /// 更新当前位置
        /// </summary>
        private void UpdatePosition()
        {
            try
            {
                if (_state == DeviceState.Disconnected) return;

                _positionX = dmc_get_position(_cardNo, 0);
                _positionY = dmc_get_position(_cardNo, 1);
                _positionZ = dmc_get_position(_cardNo, 2);

                PositionChanged?.Invoke(_positionX, _positionY, _positionZ);
            }
            catch
            {
                // 读取位置失败不影响运行
            }
        }

        /// <summary>
        /// 检查报警状态
        /// </summary>
        private void CheckAlarm()
        {
            try
            {
                if (_state == DeviceState.Disconnected) return;

                for (ushort i = 0; i < 3; i++)
                {
                    short ioStatus = dmc_axis_io_status(_cardNo, i);

                    if (ioStatus != 0)
                    {
                        _isAlarm = true;
                        _alarmCode = ioStatus;
                        _alarmMessage = $"轴{i}报警，状态码: {ioStatus}";
                        return;
                    }
                }

                _isAlarm = false;
                _alarmCode = 0;
                _alarmMessage = "";
            }
            catch
            {
                // 检查报警失败不影响运行
            }
        }

        private bool CheckLimit(int axis, double position)
        {
            double limitPositive, limitNegative;

            switch (axis)
            {
                case 0:
                    limitPositive = _config.LimitX_Positive;
                    limitNegative = _config.LimitX_Negative;
                    break;
                case 1:
                    limitPositive = _config.LimitY_Positive;
                    limitNegative = _config.LimitY_Negative;
                    break;
                case 2:
                    limitPositive = _config.LimitZ_Positive;
                    limitNegative = _config.LimitZ_Negative;
                    break;
                default:
                    ErrorOccurred?.Invoke($"无效的轴号: {axis}");
                    return false;
            }

            if (position < limitNegative || position > limitPositive)
            {
                ErrorOccurred?.Invoke($"轴{axis}超程: 位置{position}超出范围[{limitNegative}, {limitPositive}]");
                return false;
            }

            return true;
        }

        private void SetState(DeviceState newState)
        {
            if (_state != newState)
            {
                _state = newState;
                StateChanged?.Invoke(newState);
            }
        }

        public void Dispose()
        {
            Stop();

            if (_state != DeviceState.Disconnected)
            {
                dmc_board_reset();
            }

            SetState(DeviceState.Disconnected);
        }
    }
}
