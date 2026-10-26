using System;
using DefectDetection.Models;

namespace DefectDetection.Helpers.Motion
{
    /// <summary>
    /// 运动控制接口 - 所有运动控制实现（模拟/正运动/其他）必须实现此接口
    /// </summary>
    public interface IMotion : IDisposable
    {
        /// <summary>
        /// 状态变化事件
        /// </summary>
        event Action<DeviceState> StateChanged;

        /// <summary>
        /// 位置变化事件
        /// </summary>
        event Action<double, double, double> PositionChanged;

        /// <summary>
        /// 报警事件
        /// </summary>
        event Action<int, string> AlarmOccurred;

        /// <summary>
        /// 异常事件
        /// </summary>
        event Action<string> ErrorOccurred;

        /// <summary>
        /// 运动完成事件
        /// </summary>
        event Action MotionCompleted;

        /// <summary>
        /// 当前设备状态
        /// </summary>
        DeviceState State { get; }

        /// <summary>
        /// 是否已回零
        /// </summary>
        bool IsHomed { get; }

        /// <summary>
        /// 是否报警
        /// </summary>
        bool IsAlarm { get; }

        /// <summary>
        /// 是否模拟模式
        /// </summary>
        bool IsSimulated { get; }

        /// <summary>
        /// X轴位置
        /// </summary>
        double PositionX { get; }

        /// <summary>
        /// Y轴位置
        /// </summary>
        double PositionY { get; }

        /// <summary>
        /// Z轴位置
        /// </summary>
        double PositionZ { get; }

        /// <summary>
        /// 初始化运动控制卡
        /// </summary>
        bool Initialize();

        /// <summary>
        /// 轴使能
        /// </summary>
        bool EnableAxis(int axis);

        /// <summary>
        /// 轴失能
        /// </summary>
        bool DisableAxis(int axis);

        /// <summary>
        /// 回零
        /// </summary>
        bool Home();

        /// <summary>
        /// 单轴点位运动
        /// </summary>
        bool MoveSingleAxis(int axis, double position);

        /// <summary>
        /// XY直线插补
        /// </summary>
        bool MoveXY(double x, double y);

        /// <summary>
        /// 三轴联动
        /// </summary>
        bool MoveXYZ(double x, double y, double z);

        /// <summary>
        /// 停止运动
        /// </summary>
        void Stop();

        /// <summary>
        /// 急停
        /// </summary>
        void EmergencyStop();

        /// <summary>
        /// 清除报警
        /// </summary>
        bool ClearAlarm();

        /// <summary>
        /// 获取设备状态信息
        /// </summary>
        DeviceStateInfo GetStateInfo();
    }
}
