using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using DefectDetection.Models;

namespace DefectDetection.Helpers.Motion
{
    /// <summary>
    /// 固高GTS-400-PCIe运动控制卡 - 基于固高GTS SDK实现（4轴）
    /// 需要安装固高GTS驱动并将gts.dll放到程序目录
    /// </summary>
    public class GaoGongMotion : IMotion
    {
        #region 固高GTS SDK DllImport

        [DllImport("gts.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short GT_Open();

        [DllImport("gts.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short GT_Close();

        [DllImport("gts.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short GT_Reset();

        [DllImport("gts.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short GT_ClrSts(short axis, short count);

        [DllImport("gts.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short GT_AxisOn(short axis);

        [DllImport("gts.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short GT_AxisOff(short axis);

        [DllImport("gts.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short GT_PrfTrap(short axis);

        [DllImport("gts.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short GT_SetTrapPrm(ref TTrapPrm pPrm, short axis);

        [DllImport("gts.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short GT_SetVel(short axis, double vel);

        [DllImport("gts.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short GT_SetAcc(short axis, double acc);

        [DllImport("gts.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short GT_SetDec(short axis, double dec);

        [DllImport("gts.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short GT_PrfPos(short axis, double pos);

        [DllImport("gts.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short GT_PrfVel(short axis, double vel);

        [DllImport("gts.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short GT_Sts(short axis, ref short sts);

        [DllImport("gts.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short GT_GetPrfPos(short axis, ref double pos);

        [DllImport("gts.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short GT_ZeroPos(short axis, short count);

        [DllImport("gts.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short GT_Home(short axis, short dir, short mode, double vel, short count);

        [DllImport("gts.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short GT_Stop(short axis, short count);

        [DllImport("gts.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short GT_EStop(short axis, short count);

        [DllImport("gts.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short GT_GetAxisAlmSts(short axis, ref short sts);

        [DllImport("gts.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern short GT_GetAxisSts(short axis, ref short sts);

        #endregion

        /// <summary>
        /// 固高梯形曲线参数结构体
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct TTrapPrm
        {
            public double acc;
            public double dec;
            public double smoothTime;
        }

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

        public GaoGongMotion(MotionConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            CheckSdkAvailability();
        }

        /// <summary>
        /// 检查固高SDK是否可用
        /// </summary>
        private void CheckSdkAvailability()
        {
            try
            {
                var dllPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gts.dll");
                if (!System.IO.File.Exists(dllPath))
                {
                    _sdkAvailable = false;
                    ErrorOccurred?.Invoke("固高gts.dll未找到，请安装固高GTS驱动或将gts.dll放到程序目录");
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
                    ErrorOccurred?.Invoke("固高SDK不可用，请使用模拟运动模式");
                    return false;
                }

                // 打开控制卡
                short ret = GT_Open();
                if (ret != 0)
                {
                    SetState(DeviceState.Error);
                    ErrorOccurred?.Invoke($"打开固高控制卡失败，错误码: {ret}");
                    return false;
                }

                // 清除状态
                GT_ClrSts(0, 4);

                // 配置各轴参数
                ConfigureAxes();

                SetState(DeviceState.Connected);
                return true;
            }
            catch (DllNotFoundException)
            {
                _sdkAvailable = false;
                SetState(DeviceState.Error);
                ErrorOccurred?.Invoke("gts.dll未找到，请安装固高GTS驱动");
                return false;
            }
            catch (Exception ex)
            {
                SetState(DeviceState.Error);
                ErrorOccurred?.Invoke($"初始化固高异常: {ex.Message}");
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
                TTrapPrm trapPrm = new TTrapPrm();

                // 设置X轴参数
                GT_PrfTrap(0);
                trapPrm.acc = _config.AccelX;
                trapPrm.dec = _config.AccelX;
                trapPrm.smoothTime = 25;
                GT_SetTrapPrm(ref trapPrm, 0);
                GT_SetVel(0, _config.SpeedX);

                // 设置Y轴参数
                GT_PrfTrap(1);
                trapPrm.acc = _config.AccelY;
                trapPrm.dec = _config.AccelY;
                trapPrm.smoothTime = 25;
                GT_SetTrapPrm(ref trapPrm, 1);
                GT_SetVel(1, _config.SpeedY);

                // 设置Z轴参数
                GT_PrfTrap(2);
                trapPrm.acc = _config.AccelZ;
                trapPrm.dec = _config.AccelZ;
                trapPrm.smoothTime = 25;
                GT_SetTrapPrm(ref trapPrm, 2);
                GT_SetVel(2, _config.SpeedZ);
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

                short ret = GT_AxisOn((short)axis);
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

                short ret = GT_AxisOff((short)axis);
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
                        for (short i = 0; i < 3; i++)
                        {
                            if (_state != DeviceState.Homing) break;

                            short ret = GT_Home(i, 0, 0, 10, 1);
                            if (ret != 0)
                            {
                                ErrorOccurred?.Invoke($"轴{i}回零失败，错误码: {ret}");
                                break;
                            }

                            // 等待回零完成
                            while (_state == DeviceState.Homing)
                            {
                                short sts = 0;
                                GT_Sts(i, ref sts);
                                if ((sts & 0x0400) != 0) break; // 回零完成标志位
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
                        short ret = GT_PrfPos((short)axis, position);
                        if (ret != 0)
                        {
                            ErrorOccurred?.Invoke($"轴{axis}运动失败，错误码: {ret}");
                            SetState(DeviceState.Error);
                            return;
                        }

                        // 等待运动完成
                        while (_state == DeviceState.Moving)
                        {
                            short sts = 0;
                            GT_Sts((short)axis, ref sts);
                            if ((sts & 0x0400) != 0) break; // 运动完成标志位

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
                        // 固高不支持直接的XY插补，需要分别移动X和Y轴
                        short retX = GT_PrfPos(0, x);
                        short retY = GT_PrfPos(1, y);

                        if (retX != 0 || retY != 0)
                        {
                            ErrorOccurred?.Invoke($"XY运动失败，X错误码: {retX}, Y错误码: {retY}");
                            SetState(DeviceState.Error);
                            return;
                        }

                        // 等待运动完成
                        while (_state == DeviceState.Moving)
                        {
                            short stsX = 0, stsY = 0;
                            GT_Sts(0, ref stsX);
                            GT_Sts(1, ref stsY);
                            if (((stsX & 0x0400) != 0) && ((stsY & 0x0400) != 0)) break;

                            UpdatePosition();
                            Thread.Sleep(50);
                        }

                        UpdatePosition();
                        SetState(DeviceState.Connected);
                        MotionCompleted?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        ErrorOccurred?.Invoke($"XY运动异常: {ex.Message}");
                        SetState(DeviceState.Error);
                    }
                });

                return true;
            }
            catch (Exception ex)
            {
                SetState(DeviceState.Error);
                ErrorOccurred?.Invoke($"XY运动异常: {ex.Message}");
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
                        // 固高不支持直接的XYZ插补，需要分别移动X、Y和Z轴
                        short retX = GT_PrfPos(0, x);
                        short retY = GT_PrfPos(1, y);
                        short retZ = GT_PrfPos(2, z);

                        if (retX != 0 || retY != 0 || retZ != 0)
                        {
                            ErrorOccurred?.Invoke($"XYZ运动失败，X错误码: {retX}, Y错误码: {retY}, Z错误码: {retZ}");
                            SetState(DeviceState.Error);
                            return;
                        }

                        // 等待运动完成
                        while (_state == DeviceState.Moving)
                        {
                            short stsX = 0, stsY = 0, stsZ = 0;
                            GT_Sts(0, ref stsX);
                            GT_Sts(1, ref stsY);
                            GT_Sts(2, ref stsZ);
                            if (((stsX & 0x0400) != 0) && ((stsY & 0x0400) != 0) && ((stsZ & 0x0400) != 0)) break;

                            UpdatePosition();
                            Thread.Sleep(50);
                        }

                        UpdatePosition();
                        SetState(DeviceState.Connected);
                        MotionCompleted?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        ErrorOccurred?.Invoke($"XYZ运动异常: {ex.Message}");
                        SetState(DeviceState.Error);
                    }
                });

                return true;
            }
            catch (Exception ex)
            {
                SetState(DeviceState.Error);
                ErrorOccurred?.Invoke($"XYZ运动异常: {ex.Message}");
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
                    for (short i = 0; i < 3; i++)
                    {
                        GT_Stop(i, 1);
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
                    for (short i = 0; i < 3; i++)
                    {
                        GT_EStop(i, 1);
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
                DeviceName = "固高GTS-400-PCIe",
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

                double posX = 0, posY = 0, posZ = 0;
                GT_GetPrfPos(0, ref posX);
                GT_GetPrfPos(1, ref posY);
                GT_GetPrfPos(2, ref posZ);

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
                if (_state == DeviceState.Disconnected) return;

                for (short i = 0; i < 3; i++)
                {
                    short almSts = 0;
                    GT_GetAxisAlmSts(i, ref almSts);

                    if (almSts != 0)
                    {
                        _isAlarm = true;
                        _alarmCode = almSts;
                        _alarmMessage = $"轴{i}报警，状态码: {almSts}";
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
                GT_Reset();
                GT_Close();
            }

            SetState(DeviceState.Disconnected);
        }
    }
}
