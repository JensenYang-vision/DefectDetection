using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using DefectDetection.Models;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace DefectDetection.Helpers.Camera
{
    /// <summary>
    /// 海康工业相机 - 基于海康MVS SDK实现
    /// 需要安装海康MVS SDK并将MvCameraControl.dll放到程序目录
    /// </summary>
    public class HikCamera : ICamera
    {
        #region 海康MVS SDK DllImport

        // 海康MVS SDK 常用接口
        [DllImport("MvCameraControl.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int MV_CC_CreateHandle(ref IntPtr handle, ref MV_CC_DEVICE_INFO deviceInfo);

        [DllImport("MvCameraControl.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int MV_CC_DestroyHandle(IntPtr handle);

        [DllImport("MvCameraControl.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int MV_CC_OpenDevice(IntPtr handle);

        [DllImport("MvCameraControl.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int MV_CC_CloseDevice(IntPtr handle);

        [DllImport("MvCameraControl.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int MV_CC_StartGrabbing(IntPtr handle);

        [DllImport("MvCameraControl.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int MV_CC_StopGrabbing(IntPtr handle);

        [DllImport("MvCameraControl.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int MV_CC_GetOneFrameTimeout(IntPtr handle, IntPtr pData, uint dataSize, ref MV_FRAME_OUT_INFO frameInfo, uint timeout);

        [DllImport("MvCameraControl.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int MV_CC_SetEnumValue(IntPtr handle, string nodeName, uint value);

        [DllImport("MvCameraControl.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int MV_CC_SetFloatValue(IntPtr handle, string nodeName, double value);

        [DllImport("MvCameraControl.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int MV_CC_GetImageInfo(IntPtr handle, ref MV_IMAGE_INFO imageInfo);

        [DllImport("MvCameraControl.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int MV_CC_EnumDevices(uint nTLayerType, ref MV_CC_DEVICE_INFO_LIST deviceList);

        [DllImport("MvCameraControl.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int MV_CC_ConvertPixelType(IntPtr handle, ref MV_CC_PIXEL_CONVERT_PARAM convertParam);

        // 海康SDK结构体
        [StructLayout(LayoutKind.Sequential)]
        private struct MV_CC_DEVICE_INFO
        {
            public ushort nMajorVer;
            public ushort nMinorVer;
            public uint nMacAddrHigh;
            public uint nMacAddrLow;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] chSerialNumber;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] chModelName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] chManufacturerName;
            public uint nTLayerType;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MV_CC_DEVICE_INFO_LIST
        {
            public uint nDeviceNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public IntPtr[] pDeviceInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MV_FRAME_OUT_INFO
        {
            public ushort nWidth;
            public ushort nHeight;
            public uint enPixelType;
            public uint nFrameNum;
            public uint nDevTimeStampHigh;
            public uint nDevTimeStampLow;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MV_IMAGE_INFO
        {
            public uint nWidth;
            public uint nHeight;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MV_CC_PIXEL_CONVERT_PARAM
        {
            public ushort nWidth;
            public ushort nHeight;
            public uint enSrcPixelType;
            public IntPtr pSrcData;
            public uint nSrcDataLen;
            public uint enDstPixelType;
            public IntPtr pDstBuffer;
            public uint nDstBufferSize;
        }

        #endregion

        private IntPtr _handle = IntPtr.Zero;
        private CameraState _state = CameraState.Disconnected;
        private VisionConfig _config;
        private bool _isContinuousCapture;
        private CancellationTokenSource _cts;
        private Task _captureTask;
        private readonly object _lockObj = new object();
        private int _cameraIndex;
        private bool _sdkAvailable = true;

        public event Action<CameraState> StateChanged;
        public event Action<Mat> ImageCaptured;
        public event Action<string> ErrorOccurred;

        public CameraState State => _state;
        public bool IsConnected => _state == CameraState.Connected || _state == CameraState.Capturing;
        public bool IsContinuousCapture => _isContinuousCapture;

        public HikCamera(VisionConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            CheckSdkAvailability();
        }

        /// <summary>
        /// 检查海康SDK是否可用
        /// </summary>
        private void CheckSdkAvailability()
        {
            try
            {
                // 尝试查找SDK DLL
                var dllPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MvCameraControl.dll");
                if (!System.IO.File.Exists(dllPath))
                {
                    _sdkAvailable = false;
                    ErrorOccurred?.Invoke("海康MVS SDK未找到，请安装海康MVS或将MvCameraControl.dll放到程序目录");
                }
            }
            catch
            {
                _sdkAvailable = false;
            }
        }

        public bool Initialize(int cameraIndex = 0)
        {
            try
            {
                if (!_sdkAvailable)
                {
                    ErrorOccurred?.Invoke("海康SDK不可用，请使用模拟相机模式");
                    return false;
                }

                _cameraIndex = cameraIndex;

                lock (_lockObj)
                {
                    // 枚举设备
                    var deviceList = new MV_CC_DEVICE_INFO_LIST();
                    int ret = MV_CC_EnumDevices(0x00000001 | 0x00000002, ref deviceList); // GigE + USB

                    if (ret != 0 || deviceList.nDeviceNum == 0)
                    {
                        SetState(CameraState.Error);
                        ErrorOccurred?.Invoke("未检测到海康相机设备");
                        return false;
                    }

                    if (cameraIndex >= deviceList.nDeviceNum)
                    {
                        ErrorOccurred?.Invoke($"相机索引{cameraIndex}超出范围，共{deviceList.nDeviceNum}个设备");
                        return false;
                    }

                    // 创建设备句柄
                    var deviceInfo = new MV_CC_DEVICE_INFO();
                    // 这里简化处理，实际需要从deviceList中获取设备信息
                    ret = MV_CC_CreateHandle(ref _handle, ref deviceInfo);

                    if (ret != 0)
                    {
                        SetState(CameraState.Error);
                        ErrorOccurred?.Invoke($"创建相机句柄失败，错误码: {ret}");
                        return false;
                    }

                    // 打开设备
                    ret = MV_CC_OpenDevice(_handle);
                    if (ret != 0)
                    {
                        SetState(CameraState.Error);
                        ErrorOccurred?.Invoke($"打开相机失败，错误码: {ret}");
                        return false;
                    }

                    // 设置参数
                    ApplyCameraParams();

                    SetState(CameraState.Connected);
                    return true;
                }
            }
            catch (DllNotFoundException)
            {
                _sdkAvailable = false;
                SetState(CameraState.Error);
                ErrorOccurred?.Invoke("海康MvCameraControl.dll未找到，请安装海康MVS SDK");
                return false;
            }
            catch (Exception ex)
            {
                SetState(CameraState.Error);
                ErrorOccurred?.Invoke($"初始化海康相机异常: {ex.Message}");
                return false;
            }
        }

        public bool Open()
        {
            try
            {
                if (_handle == IntPtr.Zero)
                {
                    ErrorOccurred?.Invoke("相机未初始化");
                    return false;
                }

                int ret = MV_CC_OpenDevice(_handle);
                if (ret != 0)
                {
                    ErrorOccurred?.Invoke($"打开相机失败，错误码: {ret}");
                    return false;
                }

                SetState(CameraState.Connected);
                return true;
            }
            catch (Exception ex)
            {
                SetState(CameraState.Error);
                ErrorOccurred?.Invoke($"打开相机异常: {ex.Message}");
                return false;
            }
        }

        public void Close()
        {
            try
            {
                StopContinuousCapture();

                lock (_lockObj)
                {
                    if (_handle != IntPtr.Zero)
                    {
                        MV_CC_StopGrabbing(_handle);
                        MV_CC_CloseDevice(_handle);
                        MV_CC_DestroyHandle(_handle);
                        _handle = IntPtr.Zero;
                    }
                }

                SetState(CameraState.Disconnected);
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"关闭相机异常: {ex.Message}");
            }
        }

        public Mat CaptureSingle()
        {
            try
            {
                if (_handle == IntPtr.Zero)
                {
                    ErrorOccurred?.Invoke("相机未连接");
                    return null;
                }

                // 获取图像信息
                var imageInfo = new MV_IMAGE_INFO();
                MV_CC_GetImageInfo(_handle, ref imageInfo);

                if (imageInfo.nWidth == 0 || imageInfo.nHeight == 0)
                {
                    ErrorOccurred?.Invoke("无法获取图像尺寸");
                    return null;
                }

                // 分配缓冲区
                uint bufferSize = imageInfo.nWidth * imageInfo.nHeight * 3;
                var buffer = Marshal.AllocHGlobal((int)bufferSize);
                var frameInfo = new MV_FRAME_OUT_INFO();

                try
                {
                    int ret = MV_CC_GetOneFrameTimeout(_handle, buffer, bufferSize, ref frameInfo, 1000);
                    if (ret != 0)
                    {
                        ErrorOccurred?.Invoke($"采集图像失败，错误码: {ret}");
                        return null;
                    }

                    // 将原始数据转为OpenCV Mat
                    var mat = new Mat((int)imageInfo.nHeight, (int)imageInfo.nWidth, MatType.CV_8UC3);
                    var imageData = new byte[bufferSize];
                    Marshal.Copy(buffer, imageData, 0, (int)bufferSize);
                    System.Runtime.InteropServices.Marshal.Copy(imageData, 0, mat.Data, (int)bufferSize);

                    return mat;
                }
                finally
                {
                    Marshal.FreeHGlobal(buffer);
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"单帧采集异常: {ex.Message}");
                return null;
            }
        }

        public bool StartContinuousCapture()
        {
            try
            {
                if (_handle == IntPtr.Zero)
                {
                    ErrorOccurred?.Invoke("相机未连接");
                    return false;
                }

                if (_isContinuousCapture)
                {
                    return true;
                }

                int ret = MV_CC_StartGrabbing(_handle);
                if (ret != 0)
                {
                    ErrorOccurred?.Invoke($"开始取流失败，错误码: {ret}");
                    return false;
                }

                _cts = new CancellationTokenSource();
                _isContinuousCapture = true;
                SetState(CameraState.Capturing);

                _captureTask = Task.Run(() =>
                {
                    while (_isContinuousCapture && !_cts.Token.IsCancellationRequested)
                    {
                        try
                        {
                            var frame = CaptureSingle();
                            if (frame != null)
                            {
                                ImageCaptured?.Invoke(frame);
                            }

                            Thread.Sleep((int)(1000 / _config.FrameRate));
                        }
                        catch (Exception ex)
                        {
                            ErrorOccurred?.Invoke($"连续采集异常: {ex.Message}");
                            break;
                        }
                    }

                    _isContinuousCapture = false;
                    MV_CC_StopGrabbing(_handle);
                    if (_state == CameraState.Capturing)
                    {
                        SetState(CameraState.Connected);
                    }
                }, _cts.Token);

                return true;
            }
            catch (Exception ex)
            {
                _isContinuousCapture = false;
                SetState(CameraState.Error);
                ErrorOccurred?.Invoke($"开始连续采集异常: {ex.Message}");
                return false;
            }
        }

        public void StopContinuousCapture()
        {
            try
            {
                _isContinuousCapture = false;
                _cts?.Cancel();

                if (_captureTask != null)
                {
                    _captureTask.Wait(1000);
                    _captureTask = null;
                }

                _cts?.Dispose();
                _cts = null;

                if (_handle != IntPtr.Zero)
                {
                    MV_CC_StopGrabbing(_handle);
                }

                if (_state == CameraState.Capturing)
                {
                    SetState(CameraState.Connected);
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"停止连续采集异常: {ex.Message}");
            }
        }

        public bool SetExposure(double exposure)
        {
            try
            {
                if (_handle == IntPtr.Zero) return false;
                int ret = MV_CC_SetFloatValue(_handle, "ExposureTime", exposure);
                if (ret == 0)
                {
                    _config.Exposure = exposure;
                    return true;
                }
                ErrorOccurred?.Invoke($"设置曝光失败，错误码: {ret}");
                return false;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"设置曝光异常: {ex.Message}");
                return false;
            }
        }

        public bool SetGain(double gain)
        {
            try
            {
                if (_handle == IntPtr.Zero) return false;
                int ret = MV_CC_SetFloatValue(_handle, "Gain", gain);
                if (ret == 0)
                {
                    _config.Gain = gain;
                    return true;
                }
                ErrorOccurred?.Invoke($"设置增益失败，错误码: {ret}");
                return false;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"设置增益异常: {ex.Message}");
                return false;
            }
        }

        public bool SetFrameRate(double frameRate)
        {
            try
            {
                if (_handle == IntPtr.Zero) return false;
                int ret = MV_CC_SetFloatValue(_handle, "AcquisitionFrameRate", frameRate);
                if (ret == 0)
                {
                    _config.FrameRate = frameRate;
                    return true;
                }
                ErrorOccurred?.Invoke($"设置帧率失败，错误码: {ret}");
                return false;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"设置帧率异常: {ex.Message}");
                return false;
            }
        }

        public Bitmap GetCurrentFrame()
        {
            try
            {
                var mat = CaptureSingle();
                if (mat != null)
                {
                    var bitmap = BitmapConverter.ToBitmap(mat);
                    mat.Dispose();
                    return bitmap;
                }
                return null;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"获取当前帧异常: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 应用相机参数
        /// </summary>
        private void ApplyCameraParams()
        {
            try
            {
                MV_CC_SetFloatValue(_handle, "ExposureTime", _config.Exposure);
                MV_CC_SetFloatValue(_handle, "Gain", _config.Gain);
                MV_CC_SetFloatValue(_handle, "AcquisitionFrameRate", _config.FrameRate);
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"应用相机参数异常: {ex.Message}");
            }
        }

        private void SetState(CameraState newState)
        {
            if (_state != newState)
            {
                _state = newState;
                StateChanged?.Invoke(newState);
            }
        }

        public void Dispose()
        {
            Close();
        }
    }
}
