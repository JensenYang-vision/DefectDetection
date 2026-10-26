using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DefectDetection.Helpers.Data;
using DefectDetection.Models;
using OpenCvSharp;

namespace DefectDetection.Services
{
    /// <summary>
    /// 视觉检测服务 - 负责图像预处理、尺寸检测、缺陷检测、合格判定
    /// </summary>
    public class VisionDetectionService
    {
        private readonly VisionConfig _config;
        private readonly LogHelper _logHelper;

        /// <summary>
        /// 检测完成事件
        /// </summary>
        public event Action<DetectionResult, Mat> DetectionCompleted;

        /// <summary>
        /// 异常事件
        /// </summary>
        public event Action<string> ErrorOccurred;

        public VisionDetectionService(VisionConfig config, LogHelper logHelper)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logHelper = logHelper ?? throw new ArgumentNullException(nameof(logHelper));
        }

        /// <summary>
        /// 执行检测
        /// </summary>
        public DetectionResult Detect(Mat image)
        {
            var sw = Stopwatch.StartNew();
            DetectionResult result = null;
            Mat processedImage = null;

            try
            {
                if (image == null || image.Empty())
                {
                    ErrorOccurred?.Invoke("检测图像为空");
                    return null;
                }

                // 1. 图像预处理
                processedImage = PreprocessImage(image);

                // 2. 检测零件位置与尺寸
                var (found, centerX, centerY, width, height, holeDiameter) = DetectPartGeometry(processedImage);

                if (!found)
                {
                    result = new DetectionResult
                    {
                        DetectionTime = DateTime.Now,
                        PartId = Guid.NewGuid().ToString("N").Substring(0, 8),
                        IsQualified = false,
                        DefectType = "未检测到零件",
                        DetectionTimeMs = sw.Elapsed.TotalMilliseconds
                    };
                }
                else
                {
                    // 3. 像素坐标转物理坐标
                    var physicalX = centerX / _config.PixelEquivalent;
                    var physicalY = centerY / _config.PixelEquivalent;
                    var physicalWidth = width / _config.PixelEquivalent;
                    var physicalHeight = height / _config.PixelEquivalent;
                    var physicalHole = holeDiameter / _config.PixelEquivalent;

                    // 4. 合格判定
                    var (isQualified, defectType) = JudgeQualified(physicalWidth, physicalHeight, processedImage);

                    result = new DetectionResult
                    {
                        DetectionTime = DateTime.Now,
                        PartId = Guid.NewGuid().ToString("N").Substring(0, 8),
                        IsQualified = isQualified,
                        DefectType = defectType,
                        CenterX_Pixel = centerX,
                        CenterY_Pixel = centerY,
                        CenterX_Physical = physicalX,
                        CenterY_Physical = physicalY,
                        Width = physicalWidth,
                        Height = physicalHeight,
                        HoleDiameter = physicalHole,
                        DetectionTimeMs = sw.Elapsed.TotalMilliseconds
                    };
                }

                sw.Stop();
                _logHelper.LogDetection($"检测完成: {(result.IsQualified ? "合格" : "不合格")}" +
                                       (result.DefectType != null ? $", 缺陷: {result.DefectType}" : "") +
                                       $", 耗时: {result.DetectionTimeMs:F1}ms");

                DetectionCompleted?.Invoke(result, processedImage ?? image);
                return result;
            }
            catch (Exception ex)
            {
                _logHelper.LogException("视觉检测异常", ex);
                ErrorOccurred?.Invoke($"检测异常: {ex.Message}");
                return null;
            }
            finally
            {
                processedImage?.Dispose();
            }
        }

        /// <summary>
        /// 异步检测
        /// </summary>
        public Task<DetectionResult> DetectAsync(Mat image)
        {
            return Task.Run(() => Detect(image));
        }

        /// <summary>
        /// 图像预处理：灰度化 → 高斯模糊 → 反二值化
        /// 使用BinaryInv让暗色零件变为白色（前景），背景变为黑色，便于轮廓提取
        /// </summary>
        private Mat PreprocessImage(Mat src)
        {
            var gray = new Mat();
            Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

            var blurred = new Mat();
            Cv2.GaussianBlur(gray, blurred, new OpenCvSharp.Size(5, 5), 0);

            var binary = new Mat();
            Cv2.Threshold(blurred, binary, 0, 255, ThresholdTypes.BinaryInv | ThresholdTypes.Otsu);

            gray.Dispose();
            blurred.Dispose();

            return binary;
        }

        /// <summary>
        /// 检测零件几何信息：中心坐标、宽高、孔径
        /// binary: 反二值化后的图像（零件=白色，背景=黑色）
        /// </summary>
        private (bool found, double centerX, double centerY, double width, double height, double holeDiameter)
            DetectPartGeometry(Mat binary)
        {
            // 使用List模式查找所有轮廓（比External更可靠，不受边缘影响）
            Cv2.FindContours(binary, out OpenCvSharp.Point[][] contours, out _, RetrievalModes.List, ContourApproximationModes.ApproxSimple);

            if (contours.Length == 0)
            {
                return (false, 0, 0, 0, 0, 0);
            }

            // 找到面积最大的轮廓作为零件主体
            var mainContour = GetMaxAreaContour(contours);
            if (mainContour == null || mainContour.Length < 5)
            {
                return (false, 0, 0, 0, 0, 0);
            }

            // 最小外接矩形
            var rect = Cv2.MinAreaRect(mainContour);
            var centerX = rect.Center.X;
            var centerY = rect.Center.Y;
            var width = rect.Size.Width;
            var height = rect.Size.Height;

            // 确保 width >= height
            if (width < height)
            {
                (width, height) = (height, width);
            }

            // 检测内部孔洞
            var holeDiameter = DetectHole(binary);

            return (true, centerX, centerY, width, height, holeDiameter);
        }

        /// <summary>
        /// 获取面积最大的轮廓
        /// </summary>
        private OpenCvSharp.Point[] GetMaxAreaContour(OpenCvSharp.Point[][] contours)
        {
            OpenCvSharp.Point[] maxContour = null;
            double maxArea = 0;

            foreach (var contour in contours)
            {
                var area = Cv2.ContourArea(contour);
                if (area > maxArea)
                {
                    maxArea = area;
                    maxContour = contour;
                }
            }

            return maxContour;
        }

        /// <summary>
        /// 检测孔洞直径
        /// binary: 反二值化后的图像（零件=白色，背景=黑色，孔洞=黑色）
        /// 使用Tree模式查找层级关系，找到零件内部的子轮廓即为孔洞
        /// </summary>
        private double DetectHole(Mat binary)
        {
            try
            {
                Cv2.FindContours(binary, out OpenCvSharp.Point[][] contours, out HierarchyIndex[] hierarchy,
                    RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

                if (contours.Length == 0 || hierarchy == null)
                    return 0;

                // 找到面积最大的轮廓（零件主体）
                var mainContour = GetMaxAreaContour(contours);
                if (mainContour == null) return 0;

                double maxDiameter = 0;

                // 遍历所有轮廓，找到零件内部的子轮廓（孔洞）
                for (int i = 0; i < contours.Length; i++)
                {
                    // hierarchy[i].Parent >= 0 表示有父轮廓（即内部轮廓）
                    if (hierarchy[i].Parent >= 0)
                    {
                        var area = Cv2.ContourArea(contours[i]);
                        if (area < 100) continue; // 过滤噪点

                        // 假设孔洞为圆形，由面积计算直径
                        var diameter = 2 * Math.Sqrt(area / Math.PI);
                        if (diameter > maxDiameter)
                        {
                            maxDiameter = diameter;
                        }
                    }
                }

                return maxDiameter;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 合格判定：尺寸是否在阈值范围内，是否存在表面缺陷
        /// </summary>
        private (bool isQualified, string defectType) JudgeQualified(double width, double height, Mat binary)
        {
            // 1. 尺寸判定
            if (width < _config.WidthLowerLimit || width > _config.WidthUpperLimit ||
                height < _config.HeightLowerLimit || height > _config.HeightUpperLimit)
            {
                return (false, "尺寸超差");
            }

            // 2. 表面缺陷检测（基于二值图的连通域分析）
            var defectType = DetectSurfaceDefect(binary);
            if (defectType != null)
            {
                return (false, defectType);
            }

            return (true, null);
        }

        /// <summary>
        /// 表面缺陷检测：划痕、缺料、变形
        /// binary: 反二值化后的图像（零件=白色，背景=黑色）
        /// 使用Tree模式查找零件内部的子轮廓作为缺陷
        /// </summary>
        private string DetectSurfaceDefect(Mat binary)
        {
            try
            {
                Cv2.FindContours(binary, out OpenCvSharp.Point[][] contours, out HierarchyIndex[] hierarchy,
                    RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

                if (contours.Length == 0 || hierarchy == null)
                    return null;

                int scratchLikeCount = 0;
                int missingLikeCount = 0;

                for (int i = 0; i < contours.Length; i++)
                {
                    // 只检查零件内部的子轮廓（缺陷）
                    if (hierarchy[i].Parent < 0) continue;

                    var area = Cv2.ContourArea(contours[i]);
                    if (area < 50) continue; // 过滤噪点

                    var rect = Cv2.BoundingRect(contours[i]);
                    var aspectRatio = (double)Math.Max(rect.Width, rect.Height) / Math.Min(rect.Width, rect.Height);

                    // 长宽比大 → 划痕
                    if (aspectRatio > 4 && area < 2000)
                    {
                        scratchLikeCount++;
                    }
                    // 面积较大且不规则 → 缺料
                    else if (area > 1000)
                    {
                        missingLikeCount++;
                    }
                }

                // 根据灵敏度阈值判定
                var threshold = 100 - _config.DefectSensitivity; // 灵敏度越高，阈值越低

                if (scratchLikeCount > threshold / 10)
                {
                    return "划痕";
                }
                if (missingLikeCount > threshold / 15)
                {
                    return "缺料";
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
