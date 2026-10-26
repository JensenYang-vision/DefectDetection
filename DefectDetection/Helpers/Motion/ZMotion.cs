using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using DefectDetection.Models;

namespace DefectDetection.Helpers.Motion
{
    /// <summary>
    /// 正运动ZMotion运动控制卡 - 基于ZMotion PC SDK实现
    /// 需要安装正运动ZDevelop并将zmcaux.dll放到程序目录
    /// </summary>
    public class ZMotion : IMotion
    {
        #region ZMotion PC SDK DllImport

        [DllImport("zmcaux.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int ZAux_Direct_Open(string ipPort, ref IntPtr handle);

        [DllImport("zmcaux.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int ZAux_Direct_Close(IntPtr handle);

        [DllImport("zmcaux.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int ZAux_Direct_SetUnits(IntPtr handle, int axis, float units);

        [DllImport("zmcaux.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int ZAux_Direct_SetSpeed(IntPtr handle, int axis, float speed);

        [DllImport("zmcaux.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int ZAux_Direct_SetAccel(IntPtr handle, int axis, float accel);

        [DllImport("zmcaux.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int ZAux_Direct_SetDecel(IntPtr handle, int axis, float decel);

        [DllImport("zmcaux.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int ZAux_Direct_Single_Move(IntPtr handle, int axis, float distance);

        [DllImport("zmcaux.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int ZAux_Direct_MoveAbs(IntPtr handle, int axisNum, int[] axisList, float[] distance);

        [DllImport("zmcaux.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int ZAux_Direct_GetIfIdle(IntPtr handle, int axis, ref int idle);

        [DllImport("zmcaux.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int ZAux_Direct_GetDpos(IntPtr handle, int axis, ref float dpos);

        [DllImport("zmcaux.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int ZAux_Direct_GetMpos(IntPtr handle, int axis, ref float mpos);

        [DllImport("zmcaux.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int ZAux_Direct_GetAxisStatus(IntPtr handle, int axis, ref int status);

        [DllImport("zmcaux.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int ZAux_Direct_GetAlmStatus(IntPtr handle, int axis, ref int almStatus);

        [DllImport("zmcaux.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int ZAux_Direct_Datum(IntPtr handle, int axis);

        [DllImport("zmcaux.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int ZAux_Direct_Single_Cancel(IntPtr handle, int axis, int mode);

        [DllImport("zmcaux.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int ZAux_Direct_SetAtype(IntPtr handle, int axis, int type);

        [DllImport("zmcaux.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int ZAux_Direct_SetAxisEnable(IntPtr handle, int axis, int enable);

        #endregion

        private IntPtr _handle = IntPtr.Zero;
        private MotionConfig _config;
        private string _controllerIP;
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

        public ZMotion(MotionConfig config, string controllerIP = "192.168.0.11")
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _controllerIP = controllerIP ?? "192.168.0.11";
            CheckSdkAvailability();
        }

        /// <summary>
        /// 检查ZMotion SDK是否可用
        /// </summary>
        private void CheckSdkAvailability()
        {
            try
            {
                var dllPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "zmcaux.dll");
                if (!System.IO.File.Exists(dllPath))
                {
                    _sdkAvailable = false;
                    ErrorOccurred?.Invoke("正运动zmcaux.dll未找到，请安装正运动ZDevelop或将zmcaux.dll放到程序目录");
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
                    ErrorOccurred?.Invoke("正运动SDK不可用，请使用模拟运动模式");
                    return false;
                }

                // 连接控制器（使用配置的IP地址）
                string ipPort = _controllerIP;
                int ret = ZAux_Direct_Open(ipPort, ref _handle);

                if (ret != 0 || _handle == IntPtr.Zero)
                {
                    SetState(DeviceState.Error);
                    ErrorOccurred?.Invoke($"连接正运动控制器失败，错误码: {ret}，请检查IP地址和网络连接");
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
                ErrorOccurred?.Invoke("zmcaux.dll未找到，请安装正运动ZDevelop SDK");
                return false;
            }
            catch (Exception ex)
            {
                SetState(DeviceState.Error);
                ErrorOccurred?.Invoke($"初始化正运动异常: {ex.Message}");
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
                // 设置轴类型（脉冲轴）
                for (int i = 0; i < 3; i++)
                {
                    ZAux_Direct_SetAtype(_handle, i, 1); // 1=脉冲轴
                }

                // 设置X轴参数
                ZAux_Direct_SetUnits(_handle, 0, 1.0f);
                ZAux_Direct_SetSpeed(_handle, 0, (float)_config.SpeedX);
                ZAux_Direct_SetAccel(_handle, 0, (float)_config.AccelX);
                ZAux_Direct_SetDecel(_handle, 0, (float)_config.AccelX);

                // 设置Y轴参数
                ZAux_Direct_SetUnits(_handle, 1, 1.0f);
                ZAux_Direct_SetSpeed(_handle, 1, (float)_config.SpeedY);
                ZAux_Direct_SetAccel(_handle, 1, (float)_config.AccelY);
                ZAux_Direct_SetDecel(_handle, 1, (float)_config.AccelY);

                // 设置Z轴参数
                ZAux_Direct_SetUnits(_handle, 2, 1.0f);
                ZAux_Direct_SetSpeed(_handle, 2, (float)_config.SpeedZ);
                ZAux_Direct_SetAccel(_handle, 2, (float)_config.AccelZ);
                ZAux_Direct_SetDecel(_handle, 2, (float)_config.AccelZ);
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
                if (_handle == IntPtr.Zero)
                {
                    ErrorOccurred?.Invoke("设备未连接");
                    return false;
                }

                int ret = ZAux_Direct_SetAxisEnable(_handle, axis, 1);
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
                if (_handle == IntPtr.Zero) return false;

                int ret = ZAux_Direct_SetAxisEnable(_handle, axis, 0);
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
                if (_handle == IntPtr.Zero)
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
                        for (int i = 0; i < 3; i++)
                        {
                            if (_state != DeviceState.Homing) break;

                            int ret = ZAux_Direct_Datum(_handle, i);
                            if (ret != 0)
                            {
                                ErrorOccurred?.Invoke($"轴{i}回零失败，错误码: {ret}");
                                break;
                            }

                            // 等待回零完成
                            while (_state == DeviceState.Homing)
                            {
                                int idle = 0;
                                ZAux_Direct_GetIfIdle(_handle, i, ref idle);
                                if (idle == 1) break;
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
                if (_handle == IntPtr.Zero)
                {
                    ErrorOccurred?.Invoke("设备未连接");
                    return false;
                }

                if (!_isHomed)
                {
                    ErrorOccurred?.Invoke("设备未回零");
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
                        int ret = ZAux_Direct_Single_Move(_handle, axis, (float)position);
                        if (ret != 0)
                        {
                            ErrorOccurred?.Invoke($"轴{axis}运动失败，错误码: {ret}");
                            SetState(DeviceState.Error);
                            return;
                        }

                        // 等待运动完成
                        while (_state == DeviceState.Moving)
                        {
                            int idle = 0;
                            ZAux_Direct_GetIfIdle(_handle, axis, ref idle);
                            if (idle == 1) break;

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
                if (_handle == IntPtr.Zero)
                {
                    ErrorOccurred?.Invoke("设备未连接");
                    return false;
                }

                if (!_isHomed)
                {
                    ErrorOccurred?.Invoke("设备未回零");
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
                        int[] axisList = { 0, 1 };
                        float[] positions = { (float)x, (float)y };

                        int ret = ZAux_Direct_MoveAbs(_handle, 2, axisList, positions);
                        if (ret != 0)
                        {
                            ErrorOccurred?.Invoke($"XY插补失败，错误码: {ret}");
                            SetState(DeviceState.Error);
                            return;
                        }

                        // 等待运动完成
                        while (_state == DeviceState.Moving)
                        {
                            int idleX = 0, idleY = 0;
                            ZAux_Direct_GetIfIdle(_handle, 0, ref idleX);
                            ZAux_Direct_GetIfIdle(_handle, 1, ref idleY);
                            if (idleX == 1 && idleY == 1) break;

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
                if (_handle == IntPtr.Zero)
                {
                    ErrorOccurred?.Invoke("设备未连接");
                    return false;
                }

                if (!_isHomed)
                {
                    ErrorOccurred?.Invoke("设备未回零");
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
                        int[] axisList = { 0, 1, 2 };
                        float[] positions = { (float)x, (float)y, (float)z };

                        int ret = ZAux_Direct_MoveAbs(_handle, 3, axisList, positions);
                        if (ret != 0)
                        {
                            ErrorOccurred?.Invoke($"三轴联动失败，错误码: {ret}");
                            SetState(DeviceState.Error);
                            return;
                        }

                        while (_state == DeviceState.Moving)
                        {
                            int idleX = 0, idleY = 0, idleZ = 0;
                            ZAux_Direct_GetIfIdle(_handle, 0, ref idleX);
                            ZAux_Direct_GetIfIdle(_handle, 1, ref idleY);
                            ZAux_Direct_GetIfIdle(_handle, 2, ref idleZ);
                            if (idleX == 1 && idleY == 1 && idleZ == 1) break;

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

                if (_handle != IntPtr.Zero)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        ZAux_Direct_Single_Cancel(_handle, i, 2); // 2=减速停止
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
                if (_handle != IntPtr.Zero)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        ZAux_Direct_Single_Cancel(_handle, i, 0); // 0=立即停止
                    }
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
                DeviceName = "正运动ZMotion",
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
                if (_handle == IntPtr.Zero) return;

                float posX = 0, posY = 0, posZ = 0;
                ZAux_Direct_GetDpos(_handle, 0, ref posX);
                ZAux_Direct_GetDpos(_handle, 1, ref posY);
                ZAux_Direct_GetDpos(_handle, 2, ref posZ);

                _positionX = posX;
                _positionY = posY;
                _positionZ = posZ;

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
                if (_handle == IntPtr.Zero) return;

                for (int i = 0; i < 3; i++)
                {
                    int almStatus = 0;
                    ZAux_Direct_GetAlmStatus(_handle, i, ref almStatus);

                    if (almStatus != 0)
                    {
                        _isAlarm = true;
                        _alarmCode = almStatus;
                        _alarmMessage = $"轴{i}报警，状态码: {almStatus}";
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

            if (_handle != IntPtr.Zero)
            {
                ZAux_Direct_Close(_handle);
                _handle = IntPtr.Zero;
            }

            SetState(DeviceState.Disconnected);
        }
    }
}
