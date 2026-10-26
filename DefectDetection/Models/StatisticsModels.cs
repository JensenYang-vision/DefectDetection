using System;

namespace DefectDetection.Models
{
    /// <summary>
    /// 统计数据模型
    /// </summary>
    public class StatisticsData
    {
        /// <summary>
        /// 统计日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 总检测数量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 合格数量
        /// </summary>
        public int QualifiedCount { get; set; }

        /// <summary>
        /// 不合格数量
        /// </summary>
        public int UnqualifiedCount { get; set; }

        /// <summary>
        /// 合格率
        /// </summary>
        public double QualifiedRate => TotalCount > 0 ? (double)QualifiedCount / TotalCount * 100 : 0;

        /// <summary>
        /// 划痕缺陷数量
        /// </summary>
        public int ScratchCount { get; set; }

        /// <summary>
        /// 缺料缺陷数量
        /// </summary>
        public int MissingCount { get; set; }

        /// <summary>
        /// 变形缺陷数量
        /// </summary>
        public int DeformCount { get; set; }

        /// <summary>
        /// 尺寸超差数量
        /// </summary>
        public int SizeErrorCount { get; set; }

        /// <summary>
        /// 其他缺陷数量
        /// </summary>
        public int OtherDefectCount { get; set; }

        /// <summary>
        /// 重置统计
        /// </summary>
        public void Reset()
        {
            TotalCount = 0;
            QualifiedCount = 0;
            UnqualifiedCount = 0;
            ScratchCount = 0;
            MissingCount = 0;
            DeformCount = 0;
            SizeErrorCount = 0;
            OtherDefectCount = 0;
        }

        /// <summary>
        /// 更新统计
        /// </summary>
        public void Update(DetectionResult result)
        {
            TotalCount++;

            if (result.IsQualified)
            {
                QualifiedCount++;
            }
            else
            {
                UnqualifiedCount++;

                // 统计缺陷类型
                switch (result.DefectType)
                {
                    case "划痕":
                        ScratchCount++;
                        break;
                    case "缺料":
                        MissingCount++;
                        break;
                    case "变形":
                        DeformCount++;
                        break;
                    case "尺寸超差":
                        SizeErrorCount++;
                        break;
                    default:
                        OtherDefectCount++;
                        break;
                }
            }
        }
    }
}
