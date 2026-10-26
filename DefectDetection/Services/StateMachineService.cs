using System;
using DefectDetection.Helpers.Data;
using DefectDetection.Models;

namespace DefectDetection.Services
{
    /// <summary>
    /// 状态机管控服务 - 管理设备状态转换，实现状态拦截，避免多任务冲突
    /// </summary>
    public class StateMachineService
    {
        private DeviceState _currentState = DeviceState.Disconnected;
        private readonly object _lockObj = new object();
        private readonly LogHelper _logHelper;

        /// <summary>
        /// 状态变化事件
        /// </summary>
        public event Action<DeviceState, DeviceState> StateChanged;

        /// <summary>
        /// 当前状态
        /// </summary>
        public DeviceState CurrentState
        {
            get
            {
                lock (_lockObj)
                {
                    return _currentState;
                }
            }
        }

        public StateMachineService(LogHelper logHelper)
        {
            _logHelper = logHelper ?? throw new ArgumentNullException(nameof(logHelper));
        }

        /// <summary>
        /// 尝试转换状态
        /// </summary>
        public bool TryChangeState(DeviceState newState, out string errorMessage)
        {
            lock (_lockObj)
            {
                errorMessage = "";

                // 检查状态转换是否合法
                if (!CanChangeTo(newState, out errorMessage))
                {
                    _logHelper.LogWarning($"状态转换被拒绝: {_currentState} → {newState}, 原因: {errorMessage}");
                    return false;
                }

                var oldState = _currentState;
                _currentState = newState;

                _logHelper.LogSystem($"设备状态变更: {oldState} → {newState}");
                StateChanged?.Invoke(oldState, newState);

                return true;
            }
        }

        /// <summary>
        /// 强制设置状态（用于异常处理）
        /// </summary>
        public void ForceSetState(DeviceState newState)
        {
            lock (_lockObj)
            {
                var oldState = _currentState;
                _currentState = newState;
                _logHelper.LogSystem($"设备状态强制变更: {oldState} → {newState}");
                StateChanged?.Invoke(oldState, newState);
            }
        }

        /// <summary>
        /// 检查是否可以启动自动分拣
        /// </summary>
        public bool CanStartAutoSorting(out string reason)
        {
            lock (_lockObj)
            {
                reason = "";

                if (_currentState == DeviceState.Disconnected)
                {
                    reason = "设备未连接";
                    return false;
                }

                if (_currentState == DeviceState.Error)
                {
                    reason = "设备异常，请先处理异常";
                    return false;
                }

                if (_currentState == DeviceState.Homing)
                {
                    reason = "设备正在回零，请等待完成";
                    return false;
                }

                if (_currentState == DeviceState.Detecting)
                {
                    reason = "设备正在检测中，请等待完成";
                    return false;
                }

                if (_currentState == DeviceState.Moving)
                {
                    reason = "设备正在运动中，请等待完成";
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 检查是否可以执行手动操作
        /// </summary>
        public bool CanManualOperate(out string reason)
        {
            lock (_lockObj)
            {
                reason = "";

                if (_currentState == DeviceState.Disconnected)
                {
                    reason = "设备未连接";
                    return false;
                }

                if (_currentState == DeviceState.Error)
                {
                    reason = "设备异常，请先处理异常";
                    return false;
                }

                if (_currentState == DeviceState.Homing)
                {
                    reason = "设备正在回零，请等待完成";
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 检查状态转换是否合法
        /// </summary>
        private bool CanChangeTo(DeviceState newState, out string reason)
        {
            reason = "";

            // 定义合法的状态转换
            switch (_currentState)
            {
                case DeviceState.Disconnected:
                    // 只能转到 Connected
                    if (newState != DeviceState.Connected)
                    {
                        reason = "设备未连接，只能连接到设备";
                        return false;
                    }
                    break;

                case DeviceState.Connected:
                    // 可以转到 Homing, Detecting, Error, Stopped, Disconnected
                    if (newState != DeviceState.Homing &&
                        newState != DeviceState.Detecting &&
                        newState != DeviceState.Error &&
                        newState != DeviceState.Stopped &&
                        newState != DeviceState.Disconnected)
                    {
                        reason = $"从 {_currentState} 不能转换到 {newState}";
                        return false;
                    }
                    break;

                case DeviceState.Homing:
                    // 只能转到 Connected 或 Error
                    if (newState != DeviceState.Connected && newState != DeviceState.Error)
                    {
                        reason = "回零中只能转到就绪或异常";
                        return false;
                    }
                    break;

                case DeviceState.Detecting:
                    // 可以转到 Moving, Connected, Error
                    if (newState != DeviceState.Moving &&
                        newState != DeviceState.Connected &&
                        newState != DeviceState.Error)
                    {
                        reason = "检测中只能转到运动中、就绪或异常";
                        return false;
                    }
                    break;

                case DeviceState.Moving:
                    // 可以转到 Connected, Error
                    if (newState != DeviceState.Connected && newState != DeviceState.Error)
                    {
                        reason = "运动中只能转到就绪或异常";
                        return false;
                    }
                    break;

                case DeviceState.Error:
                    // 可以转到 Connected, Disconnected
                    if (newState != DeviceState.Connected && newState != DeviceState.Disconnected)
                    {
                        reason = "异常状态只能转到就绪或断开";
                        return false;
                    }
                    break;

                case DeviceState.Stopped:
                    // 可以转到 Connected, Disconnected
                    if (newState != DeviceState.Connected && newState != DeviceState.Disconnected)
                    {
                        reason = "已停止状态只能转到就绪或断开";
                        return false;
                    }
                    break;
            }

            return true;
        }

        /// <summary>
        /// 获取状态描述
        /// </summary>
        public string GetStateDescription()
        {
            lock (_lockObj)
            {
                return _currentState switch
                {
                    DeviceState.Disconnected => "未连接",
                    DeviceState.Connected => "已连接（待机）",
                    DeviceState.Homing => "回零中",
                    DeviceState.Detecting => "检测中",
                    DeviceState.Moving => "运动中",
                    DeviceState.Error => "异常停机",
                    DeviceState.Stopped => "已停止",
                    _ => "未知"
                };
            }
        }
    }

    /// <summary>
    /// LogHelper扩展方法
    /// </summary>
    public static class LogHelperExtensions
    {
        public static void LogWarning(this LogHelper logHelper, string message, string details = "")
        {
            logHelper.WriteLog(Models.LogLevel.Warning, Models.LogType.System, message, details);
        }
    }
}
