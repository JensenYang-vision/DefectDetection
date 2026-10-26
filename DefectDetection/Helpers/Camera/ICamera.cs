using System;
using DefectDetection.Models;
using OpenCvSharp;

namespace DefectDetection.Helpers.Camera
{
    /// <summary>
    /// 相机接口 - 所有相机实现（模拟/海康/其他）必须实现此接口
    /// </summary>
    public interface ICamera : IDisposable
    {
        /// <summary>
        /// 相机状态变化事件
        /// </summary>
        event Action<CameraState> StateChanged;

        /// <summary>
        /// 图像采集事件
        /// </summary>
        event Action<Mat> ImageCaptured;

        /// <summary>
        /// 异常事件
        /// </summary>
        event Action<string> ErrorOccurred;

        /// <summary>
        /// 当前相机状态
        /// </summary>
        CameraState State { get; }

        /// <summary>
        /// 是否已连接
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// 是否正在连续采集
        /// </summary>
        bool IsContinuousCapture { get; }

        /// <summary>
        /// 初始化相机
        /// </summary>
        /// <param name="cameraIndex">相机索引</param>
        /// <returns>是否成功</returns>
        bool Initialize(int cameraIndex = 0);

        /// <summary>
        /// 打开相机
        /// </summary>
        bool Open();

        /// <summary>
        /// 关闭相机
        /// </summary>
        void Close();

        /// <summary>
        /// 单帧采集
        /// </summary>
        Mat CaptureSingle();

        /// <summary>
        /// 开始连续采集
        /// </summary>
        bool StartContinuousCapture();

        /// <summary>
        /// 停止连续采集
        /// </summary>
        void StopContinuousCapture();

        /// <summary>
        /// 设置曝光值
        /// </summary>
        bool SetExposure(double exposure);

        /// <summary>
        /// 设置增益
        /// </summary>
        bool SetGain(double gain);

        /// <summary>
        /// 设置帧率
        /// </summary>
        bool SetFrameRate(double frameRate);

        /// <summary>
        /// 获取当前帧（Bitmap格式）
        /// </summary>
        System.Drawing.Bitmap GetCurrentFrame();
    }
}
