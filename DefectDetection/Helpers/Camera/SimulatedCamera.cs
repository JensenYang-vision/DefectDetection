using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using DefectDetection.Models;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace DefectDetection.Helpers.Camera
{
    /// <summary>
    /// 模拟相机 - 优先使用电脑摄像头，无摄像头时自动生成合成零件测试图
    /// </summary>
    public class SimulatedCamera : ICamera
    {
        private VideoCapture _videoCapture;
        private CameraState _state = CameraState.Disconnected;
        private VisionConfig _config;
        private bool _isContinuousCapture;
        private CancellationTokenSource _cts;
        private Task _captureTask;
        private readonly object _lockObj = new object();
        private bool _useSyntheticImage = false;
        private Random _random = new Random();
        private int _frameCounter = 0;

        public event Action<CameraState> StateChanged;
        public event Action<Mat> ImageCaptured;
        public event Action<string> ErrorOccurred;

        public CameraState State => _state;
        public bool IsConnected => _state == CameraState.Connected || _state == CameraState.Capturing;
        public bool IsContinuousCapture => _isContinuousCapture;

        public SimulatedCamera(VisionConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public bool Initialize(int cameraIndex = 0)
        {
            try
            {
                lock (_lockObj)
                {
                    if (_videoCapture != null)
                    {
                        _videoCapture.Release();
                        _videoCapture = null;
                    }

                    _videoCapture = new VideoCapture(cameraIndex);

                    if (!_videoCapture.IsOpened())
                    {
                       // 摄像头不可用，切换到合成图像模式
                       _useSyntheticImage = true;
                        _videoCapture = null;
                        SetState(CameraState.Connected);
                        return true;
                    }

                    _videoCapture.Set(VideoCaptureProperties.FrameWidth, 640);
                    _videoCapture.Set(VideoCaptureProperties.FrameHeight, 480);
                    _videoCapture.Set(VideoCaptureProperties.Fps, _config.FrameRate);

                    SetState(CameraState.Connected);
                    return true;
                }
            }
            catch (Exception ex)
            {
                SetState(CameraState.Error);
                ErrorOccurred?.Invoke($"初始化相机异常: {ex.Message}");
                return false;
            }
        }

        public bool Open()
        {
            try
            {
                if (_videoCapture == null || !_videoCapture.IsOpened())
                {
                    ErrorOccurred?.Invoke("相机未初始化");
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
                    if (_videoCapture != null)
                    {
                        _videoCapture.Release();
                        _videoCapture = null;
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
                // 合成图像模式：生成模拟零件测试图
                if (_useSyntheticImage)
                {
                    return GenerateSyntheticPartImage();
                }

                if (_videoCapture == null || !_videoCapture.IsOpened())
                {
                    ErrorOccurred?.Invoke("相机未连接");
                    return null;
                }

                var frame = new Mat();
                if (_videoCapture.Read(frame) && !frame.Empty())
                {
                    return frame;
                }
                else
                {
                    frame.Dispose();
                    ErrorOccurred?.Invoke("采集图像失败");
                    return null;
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke($"单帧采集异常: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 生成合成零件测试图像（白色背景 + 暗色矩形零件 + 可选孔洞）
        /// 符合检测算法要求：灰度化后零件为暗色区域，背景为白色
        /// </summary>
        private Mat GenerateSyntheticPartImage()
        {
            _frameCounter++;

            // 创建白色背景 640x480
            var image = new Mat(480, 640, MatType.CV_8UC3, new Scalar(240, 240, 240));

            // 随机生成零件尺寸（像素），对应约 35-45mm（在默认阈值 30-50mm 范围内）
            int partWidth = _random.Next(350, 450);
            int partHeight = _random.Next(350, 450);

            // 随机位置（居中附近偏移）
            int centerX = 320 + _random.Next(-30, 30);
            int centerY = 240 + _random.Next(-30, 30);

            int x = centerX - partWidth / 2;
            int y = centerY - partHeight / 2;

            // 绘制暗色矩形零件（模拟金属零件在背光下的剪影效果）
            Cv2.Rectangle(image,
                new OpenCvSharp.Point(x, y),
                new OpenCvSharp.Point(x + partWidth, y + partHeight),
                new Scalar(60, 60, 60), -1);

            // 绘制零件内部圆孔（模拟通孔）
            int holeRadius = _random.Next(30, 50);
            Cv2.Circle(image,
                new OpenCvSharp.Point(centerX, centerY),
                holeRadius,
                new Scalar(240, 240, 240), -1);

            // 添加轻微噪声模拟真实图像
            var noise = new Mat(image.Size(), MatType.CV_8UC3);
            Cv2.Randn(noise, new Scalar(0, 0, 0), new Scalar(5, 5, 5));
            Cv2.Add(image, noise, image);
            noise.Dispose();

            // 每10帧随机生成一个"不合格"零件（尺寸超差）
            if (_frameCounter % 10 == 0)
            {
                // 生成超大零件（超出50mm阈值）
                int bigWidth = _random.Next(520, 580);
                int bigHeight = _random.Next(350, 420);
                Cv2.Rectangle(image,
                    new OpenCvSharp.Point(100, 80),
                    new OpenCvSharp.Point(100 + bigWidth, 80 + bigHeight),
                    new Scalar(50, 50, 50), -1);

                // 添加划痕缺陷
                Cv2.Line(image,
                    new OpenCvSharp.Point(150, 150),
                    new OpenCvSharp.Point(400, 200),
                    new Scalar(100, 100, 100), 3);
            }

            return image;
        }

        public bool StartContinuousCapture()
        {
            try
            {
                if (_videoCapture == null || !_videoCapture.IsOpened())
                {
                    ErrorOccurred?.Invoke("相机未连接");
                    return false;
                }

                if (_isContinuousCapture)
                {
                    return true;
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
                if (_videoCapture == null || !_videoCapture.IsOpened())
                {
                    return false;
                }

                _videoCapture.Set(VideoCaptureProperties.Exposure, exposure);
                _config.Exposure = exposure;
                return true;
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
                if (_videoCapture == null || !_videoCapture.IsOpened())
                {
                    return false;
                }

                _videoCapture.Set(VideoCaptureProperties.Gain, gain);
                _config.Gain = gain;
                return true;
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
                if (_videoCapture == null || !_videoCapture.IsOpened())
                {
                    return false;
                }

                _videoCapture.Set(VideoCaptureProperties.Fps, frameRate);
                _config.FrameRate = frameRate;
                return true;
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
