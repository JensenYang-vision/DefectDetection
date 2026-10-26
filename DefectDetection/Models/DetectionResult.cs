using System;

namespace DefectDetection.Models
{
    /// <summary>
    /// 检测结果数据模型
    /// </summary>
    public class DetectionResult
    {
        /// <summary>
        /// 检测时间
        /// </summary>
        public DateTime DetectionTime { get; set; }

        /// <summary>
        /// 零件ID
        /// </summary>
        public string PartId { get; set; }

        /// <summary>
        /// 是否合格
        /// </summary>
        public bool IsQualified { get; set; }

        /// <summary>
        /// 缺陷类型（无缺陷时为空）
        /// </summary>
        public string DefectType { get; set; }

        /// <summary>
        /// 零件中心X坐标（像素）
        /// </summary>
        public double CenterX_Pixel { get; set; }

        /// <summary>
        /// 零件中心Y坐标（像素）
        /// </summary>
        public double CenterY_Pixel { get; set; }

        /// <summary>
        /// 零件中心X坐标（物理）
        /// </summary>
        public double CenterX_Physical { get; set; }

        /// <summary>
        /// 零件中心Y坐标（物理）
        /// </summary>
        public double CenterY_Physical { get; set; }

        /// <summary>
        /// 零件宽度
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// 零件高度
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// 孔径
        /// </summary>
        public double HoleDiameter { get; set; }

        /// <summary>
        /// 检测耗时（毫秒）
        /// </summary>
        public double DetectionTimeMs { get; set; }
    }
}
