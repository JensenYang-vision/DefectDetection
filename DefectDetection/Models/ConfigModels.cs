using System;

namespace DefectDetection.Models
{
    /// <summary>
    /// 视觉参数配置
    /// </summary>
    public class VisionConfig
    {
        /// <summary>
        /// 曝光值
        /// </summary>
        public double Exposure { get; set; } = 5000;

        /// <summary>
        /// 增益值
        /// </summary>
        public double Gain { get; set; } = 1.0;

        /// <summary>
        /// 帧率
        /// </summary>
        public double FrameRate { get; set; } = 30;

        /// <summary>
        /// 尺寸检测阈值-宽度上限
        /// </summary>
        public double WidthUpperLimit { get; set; } = 50.0;

        /// <summary>
        /// 尺寸检测阈值-宽度下限
        /// </summary>
        public double WidthLowerLimit { get; set; } = 30.0;

        /// <summary>
        /// 尺寸检测阈值-高度上限
        /// </summary>
        public double HeightUpperLimit { get; set; } = 50.0;

        /// <summary>
        /// 尺寸检测阈值-高度下限
        /// </summary>
        public double HeightLowerLimit { get; set; } = 30.0;

        /// <summary>
        /// 缺陷识别灵敏度（0-100）
        /// </summary>
        public int DefectSensitivity { get; set; } = 50;

        /// <summary>
        /// 像素当量（像素/毫米）
        /// </summary>
        public double PixelEquivalent { get; set; } = 10.0;
    }

    /// <summary>
    /// 运动参数配置
    /// </summary>
    public class MotionConfig
    {
        /// <summary>
        /// X轴速度
        /// </summary>
        public double SpeedX { get; set; } = 100;

        /// <summary>
        /// Y轴速度
        /// </summary>
        public double SpeedY { get; set; } = 100;

        /// <summary>
        /// Z轴速度
        /// </summary>
        public double SpeedZ { get; set; } = 50;

        /// <summary>
        /// X轴加速度
        /// </summary>
        public double AccelX { get; set; } = 500;

        /// <summary>
        /// Y轴加速度
        /// </summary>
        public double AccelY { get; set; } = 500;

        /// <summary>
        /// Z轴加速度
        /// </summary>
        public double AccelZ { get; set; } = 300;

        /// <summary>
        /// X轴限位-正
        /// </summary>
        public double LimitX_Positive { get; set; } = 500;

        /// <summary>
        /// X轴限位-负
        /// </summary>
        public double LimitX_Negative { get; set; } = 0;

        /// <summary>
        /// Y轴限位-正
        /// </summary>
        public double LimitY_Positive { get; set; } = 500;

        /// <summary>
        /// Y轴限位-负
        /// </summary>
        public double LimitY_Negative { get; set; } = 0;

        /// <summary>
        /// Z轴限位-正
        /// </summary>
        public double LimitZ_Positive { get; set; } = 100;

        /// <summary>
        /// Z轴限位-负
        /// </summary>
        public double LimitZ_Negative { get; set; } = 0;

        /// <summary>
        /// 定位精度（毫米）
        /// </summary>
        public double PositionAccuracy { get; set; } = 0.1;

        /// <summary>
        /// 定位等待时间（毫秒）
        /// </summary>
        public int PositionWaitTime { get; set; } = 500;

        /// <summary>
        /// 抓取高度（毫米）
        /// </summary>
        public double GrabHeight { get; set; } = 10;

        /// <summary>
        /// 放置高度A区（毫米）
        /// </summary>
        public double PlaceHeightA { get; set; } = 20;

        /// <summary>
        /// 放置高度B区（毫米）
        /// </summary>
        public double PlaceHeightB { get; set; } = 20;

        /// <summary>
        /// A区X坐标
        /// </summary>
        public double AreaX { get; set; } = 400;

        /// <summary>
        /// A区Y坐标
        /// </summary>
        public double AreaY { get; set; } = 100;

        /// <summary>
        /// B区X坐标
        /// </summary>
        public double AreaBX { get; set; } = 400;

        /// <summary>
        /// B区Y坐标
        /// </summary>
        public double AreaBY { get; set; } = 400;
    }

    /// <summary>
    /// 相机类型枚举
    /// </summary>
    public enum CameraType
    {
        /// <summary>
        /// 模拟相机（电脑摄像头）
        /// </summary>
        Simulated,

        /// <summary>
        /// 海康工业相机
        /// </summary>
        HikVision
    }

    /// <summary>
    /// 运动控制类型枚举
    /// </summary>
    public enum MotionType
    {
        /// <summary>
        /// 模拟运动控制
        /// </summary>
        Simulated,

        /// <summary>
        /// 正运动ZMotion
        /// </summary>
        ZMotion,

        /// <summary>
        /// 雷赛DMC3400（4轴脉冲）
        /// </summary>
        LeiSai,

        /// <summary>
        /// 固高GTS-400-PCIe（4轴）
        /// </summary>
        GaoGong
    }

    /// <summary>
    /// 系统配置
    /// </summary>
    public class SystemConfig
    {
        /// <summary>
        /// 相机类型
        /// </summary>
        public CameraType CameraType { get; set; } = CameraType.Simulated;

        /// <summary>
        /// 运动控制类型
        /// </summary>
        public MotionType MotionType { get; set; } = MotionType.Simulated;

        /// <summary>
        /// 是否使用模拟相机（向后兼容）
        /// </summary>
        public bool UseSimulatedCamera
        {
            get => CameraType == CameraType.Simulated;
            set => CameraType = value ? CameraType.Simulated : CameraType.HikVision;
        }

        /// <summary>
        /// 是否使用模拟运动控制（向后兼容）
        /// </summary>
        public bool UseSimulatedMotion
        {
            get => MotionType == MotionType.Simulated;
            set => MotionType = value ? MotionType.Simulated : MotionType.ZMotion;
        }

        /// <summary>
        /// 相机索引
        /// </summary>
        public int CameraIndex { get; set; } = 0;

        /// <summary>
        /// 正运动控制器IP地址
        /// </summary>
        public string ZMotionIP { get; set; } = "192.168.0.11";

        /// <summary>
        /// 日志级别
        /// </summary>
        public string LogLevel { get; set; } = "Info";

        /// <summary>
        /// 自动保存间隔（秒）
        /// </summary>
        public int AutoSaveInterval { get; set; } = 60;

        /// <summary>
        /// 视觉参数
        /// </summary>
        public VisionConfig Vision { get; set; } = new VisionConfig();

        /// <summary>
        /// 运动参数
        /// </summary>
        public MotionConfig Motion { get; set; } = new MotionConfig();
    }
}
