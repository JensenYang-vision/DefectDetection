using System;

namespace DefectDetection.Models
{
    /// <summary>
    /// 设备状态枚举
    /// </summary>
    public enum DeviceState
    {
        /// <summary>
        /// 未连接
        /// </summary>
        Disconnected,

        /// <summary>
        /// 已连接（待机）
        /// </summary>
        Connected,

        /// <summary>
        /// 回零中
        /// </summary>
        Homing,

        /// <summary>
        /// 检测中
        /// </summary>
        Detecting,

        /// <summary>
        /// 运动中
        /// </summary>
        Moving,

        /// <summary>
        /// 异常停机
        /// </summary>
        Error,

        /// <summary>
        /// 已停止
        /// </summary>
        Stopped
    }

    /// <summary>
    /// 相机状态枚举
    /// </summary>
    public enum CameraState
    {
        /// <summary>
        /// 未连接
        /// </summary>
        Disconnected,

        /// <summary>
        /// 已连接
        /// </summary>
        Connected,

        /// <summary>
        /// 采集中
        /// </summary>
        Capturing,

        /// <summary>
        /// 异常
        /// </summary>
        Error
    }

    /// <summary>
    /// 设备状态信息
    /// </summary>
    public class DeviceStateInfo
    {
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        public DeviceState State { get; set; }

        /// <summary>
        /// 是否已回零
        /// </summary>
        public bool IsHomed { get; set; }

        /// <summary>
        /// 是否报警
        /// </summary>
        public bool IsAlarm { get; set; }

        /// <summary>
        /// 报警代码
        /// </summary>
        public int AlarmCode { get; set; }

        /// <summary>
        /// 报警信息
        /// </summary>
        public string AlarmMessage { get; set; }

        /// <summary>
        /// X轴当前位置
        /// </summary>
        public double PositionX { get; set; }

        /// <summary>
        /// Y轴当前位置
        /// </summary>
        public double PositionY { get; set; }

        /// <summary>
        /// Z轴当前位置
        /// </summary>
        public double PositionZ { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }
    }
}
