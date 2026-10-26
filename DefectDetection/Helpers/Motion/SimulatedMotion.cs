using System;
using System.Threading;
using System.Threading.Tasks;
using DefectDetection.Models;

namespace DefectDetection.Helpers.Motion
{
    /// <summary>
    /// 模拟运动控制 - 无需实体硬件，模拟XYZ三轴运动
    /// </summary>
    public class SimulatedMotion : IMotion
    {
        private MotionConfig _config;
        private DeviceState _state = DeviceState.Disconnected;
        private bool _isHomed = false;

        private double _positionX = 0;
        private double _positionY = 0;
        private double _positionZ = 0;

        private bool _xAxisEnabled = false;
        private bool _yAxisEnabled = false;
        private bool _zAxisEnabled = false;

        private bool _isAlarm = false;
        private int _alarmCode = 0;
        private string _alarmMessage = "";

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
        public bool IsSimulated => true;
        public double PositionX => _positionX;
        public double PositionY => _positionY;
        public double PositionZ => _positionZ;

        public SimulatedMotion(MotionConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public bool Initialize()
        {
            try
            {
                _positionX = 0;
                _positionY = 0;
                _positionZ = 0;
                SetState(DeviceState.Connected);
                return true;
            }
            catch (Exception ex)
            {
                SetState(DeviceState.Error);
                ErrorOccurred?.Invoke($"初始化模拟运动异常: {ex.Message}");
                return false;
            }
        }

        public bool EnableAxis(int axis)
        {
            try
            {
                if (_state == DeviceState.Disconnected || _state == DeviceState.Error)
                {
                    ErrorOccurred?.Invoke("设备未连接或异常");
                    return false;
                }

                switch (axis)
                {
                    case 0: _xAxisEnabled = true; break;
                    case 1: _yAxisEnabled = true; break;
                    case 2: _zAxisEnabled = true; break;
                    default:
                        ErrorOccurred?.Invoke($"无效的轴号: {axis}");
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
                switch (axis)
                {
                    case 0: _xAxisEnabled = false; break;
                    case 1: _yAxisEnabled = false; break;
                    case 2: _zAxisEnabled = false; break;
                    default:
                        ErrorOccurred?.Invoke($"无效的轴号: {axis}");
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
                if (_state == DeviceState.Disconnected || _state == DeviceState.Error)
                {
                    ErrorOccurred?.Invoke("设备未连接或异常");
                    return false;
                }

                if (!_xAxisEnabled || !_yAxisEnabled || !_zAxisEnabled)
                {
                    ErrorOccurred?.Invoke("轴未使能");
                    return false;
                }

                SetState(DeviceState.Homing);

                _motionTask = Task.Run(() =>
                {
                    for (int i = 0; i <= 100; i += 10)
                    {
                        if (_state != DeviceState.Homing) break;

                        _positionX = _positionX * (100 - i) / 100;
                        _positionY = _positionY * (100 - i) / 100;
                        _positionZ = _positionZ * (100 - i) / 100;

                        PositionChanged?.Invoke(_positionX, _positionY, _positionZ);
                        Thread.Sleep(100);
                    }

                    _positionX = 0;
                    _positionY = 0;
                    _positionZ = 0;
                    _isHomed = true;
                    PositionChanged?.Invoke(_positionX, _positionY, _positionZ);
                    SetState(DeviceState.Connected);
                    MotionCompleted?.Invoke();
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
                SimulateMove(axis, position);
                SetState(DeviceState.Connected);
                MotionCompleted?.Invoke();

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
                SimulateMoveXY(x, y);
                SetState(DeviceState.Connected);
                MotionCompleted?.Invoke();

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
                SimulateMoveXYZ(x, y, z);
                SetState(DeviceState.Connected);
                MotionCompleted?.Invoke();

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
            return new DeviceStateInfo
            {
                DeviceName = "模拟运动控制",
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

        private void SimulateMove(int axis, double targetPosition)
        {
            double currentPosition;
            double speed;

            switch (axis)
            {
                case 0: currentPosition = _positionX; speed = _config.SpeedX; break;
                case 1: currentPosition = _positionY; speed = _config.SpeedY; break;
                case 2: currentPosition = _positionZ; speed = _config.SpeedZ; break;
                default: return;
            }

            double step = speed / 10;
            double direction = targetPosition > currentPosition ? 1 : -1;

            while (Math.Abs(targetPosition - currentPosition) > 0.1)
            {
                if (_state != DeviceState.Moving) break;

                currentPosition += step * direction;
                if ((direction > 0 && currentPosition > targetPosition) ||
                    (direction < 0 && currentPosition < targetPosition))
                {
                    currentPosition = targetPosition;
                }

                switch (axis)
                {
                    case 0: _positionX = currentPosition; break;
                    case 1: _positionY = currentPosition; break;
                    case 2: _positionZ = currentPosition; break;
                }

                PositionChanged?.Invoke(_positionX, _positionY, _positionZ);
                Thread.Sleep(50);
            }
        }

        private void SimulateMoveXY(double targetX, double targetY)
        {
            double stepX = _config.SpeedX / 10;
            double stepY = _config.SpeedY / 10;
            double dirX = targetX > _positionX ? 1 : -1;
            double dirY = targetY > _positionY ? 1 : -1;

            while (Math.Abs(targetX - _positionX) > 0.1 || Math.Abs(targetY - _positionY) > 0.1)
            {
                if (_state != DeviceState.Moving) break;

                if (Math.Abs(targetX - _positionX) > 0.1)
                {
                    _positionX += stepX * dirX;
                    if ((dirX > 0 && _positionX > targetX) || (dirX < 0 && _positionX < targetX))
                        _positionX = targetX;
                }

                if (Math.Abs(targetY - _positionY) > 0.1)
                {
                    _positionY += stepY * dirY;
                    if ((dirY > 0 && _positionY > targetY) || (dirY < 0 && _positionY < targetY))
                        _positionY = targetY;
                }

                PositionChanged?.Invoke(_positionX, _positionY, _positionZ);
                Thread.Sleep(50);
            }
        }

        private void SimulateMoveXYZ(double targetX, double targetY, double targetZ)
        {
            double stepX = _config.SpeedX / 10;
            double stepY = _config.SpeedY / 10;
            double stepZ = _config.SpeedZ / 10;

            double dirX = targetX > _positionX ? 1 : -1;
            double dirY = targetY > _positionY ? 1 : -1;
            double dirZ = targetZ > _positionZ ? 1 : -1;

            while (Math.Abs(targetX - _positionX) > 0.1 ||
                   Math.Abs(targetY - _positionY) > 0.1 ||
                   Math.Abs(targetZ - _positionZ) > 0.1)
            {
                if (_state != DeviceState.Moving) break;

                if (Math.Abs(targetX - _positionX) > 0.1)
                {
                    _positionX += stepX * dirX;
                    if ((dirX > 0 && _positionX > targetX) || (dirX < 0 && _positionX < targetX))
                        _positionX = targetX;
                }

                if (Math.Abs(targetY - _positionY) > 0.1)
                {
                    _positionY += stepY * dirY;
                    if ((dirY > 0 && _positionY > targetY) || (dirY < 0 && _positionY < targetY))
                        _positionY = targetY;
                }

                if (Math.Abs(targetZ - _positionZ) > 0.1)
                {
                    _positionZ += stepZ * dirZ;
                    if ((dirZ > 0 && _positionZ > targetZ) || (dirZ < 0 && _positionZ < targetZ))
                        _positionZ = targetZ;
                }

                PositionChanged?.Invoke(_positionX, _positionY, _positionZ);
                Thread.Sleep(50);
            }
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
            DisableAxis(0);
            DisableAxis(1);
            DisableAxis(2);
            SetState(DeviceState.Disconnected);
        }
    }
}
