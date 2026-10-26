using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DefectDetection.Models;

namespace DefectDetection.Helpers.Data
{
    /// <summary>
    /// 统计帮助类 - 实时统计检测数据，支持缓存、刷新与导出
    /// </summary>
    public class StatHelper
    {
        private readonly object _lockObj = new object();
        private StatisticsData _todayData;
        private readonly Dictionary<string, StatisticsData> _historyData = new Dictionary<string, StatisticsData>();
        private readonly List<DetectionResult> _recentResults = new List<DetectionResult>();
        private const int MaxRecentResults = 1000;

        /// <summary>
        /// 统计数据变更事件
        /// </summary>
        public event Action<StatisticsData> StatisticsChanged;

        /// <summary>
        /// 当前今日统计
        /// </summary>
        public StatisticsData TodayData
        {
            get
            {
                lock (_lockObj)
                {
                    CheckDateRollover();
                    return _todayData;
                }
            }
        }

        public StatHelper()
        {
            _todayData = new StatisticsData { Date = DateTime.Now.Date };
        }

        /// <summary>
        /// 添加检测结果，更新统计
        /// </summary>
        public void AddResult(DetectionResult result)
        {
            lock (_lockObj)
            {
                CheckDateRollover();

                _todayData.Update(result);

                _recentResults.Add(result);
                while (_recentResults.Count > MaxRecentResults)
                {
                    _recentResults.RemoveAt(0);
                }
            }

            StatisticsChanged?.Invoke(_todayData);
        }

        /// <summary>
        /// 获取今日统计
        /// </summary>
        public StatisticsData GetTodayStatistics()
        {
            lock (_lockObj)
            {
                CheckDateRollover();
                return _todayData;
            }
        }

        /// <summary>
        /// 获取最近N条检测结果
        /// </summary>
        public List<DetectionResult> GetRecentResults(int count = 100)
        {
            lock (_lockObj)
            {
                return _recentResults.TakeLast(count).ToList();
            }
        }

        /// <summary>
        /// 重置今日统计
        /// </summary>
        public void ResetToday()
        {
            lock (_lockObj)
            {
                _todayData.Reset();
            }
            StatisticsChanged?.Invoke(_todayData);
        }

        /// <summary>
        /// 导出统计数据到CSV文件
        /// </summary>
        public bool ExportToCsv(string filePath)
        {
            try
            {
                lock (_lockObj)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("检测时间,零件ID,是否合格,缺陷类型,中心X(像素),中心Y(像素),中心X(物理),中心Y(物理),宽度,高度,孔径,检测耗时(ms)");

                    foreach (var r in _recentResults)
                    {
                        sb.AppendLine($"{r.DetectionTime:yyyy-MM-dd HH:mm:ss.fff}," +
                                      $"{r.PartId}," +
                                      $"{(r.IsQualified ? "合格" : "不合格")}," +
                                      $"{r.DefectType ?? ""}," +
                                      $"{r.CenterX_Pixel:F2}," +
                                      $"{r.CenterY_Pixel:F2}," +
                                      $"{r.CenterX_Physical:F2}," +
                                      $"{r.CenterY_Physical:F2}," +
                                      $"{r.Width:F2}," +
                                      $"{r.Height:F2}," +
                                      $"{r.HoleDiameter:F2}," +
                                      $"{r.DetectionTimeMs:F1}");
                    }

                    File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 导出统计摘要到文本文件
        /// </summary>
        public bool ExportSummary(string filePath)
        {
            try
            {
                lock (_lockObj)
                {
                    var sb = new StringBuilder();
                    sb.AppendLine("===== 生产统计摘要 =====");
                    sb.AppendLine($"统计日期: {_todayData.Date:yyyy-MM-dd}");
                    sb.AppendLine($"总检测数: {_todayData.TotalCount}");
                    sb.AppendLine($"合格数量: {_todayData.QualifiedCount}");
                    sb.AppendLine($"不合格数量: {_todayData.UnqualifiedCount}");
                    sb.AppendLine($"合格率: {_todayData.QualifiedRate:F2}%");
                    sb.AppendLine("");
                    sb.AppendLine("===== 缺陷分类统计 =====");
                    sb.AppendLine($"划痕: {_todayData.ScratchCount}");
                    sb.AppendLine($"缺料: {_todayData.MissingCount}");
                    sb.AppendLine($"变形: {_todayData.DeformCount}");
                    sb.AppendLine($"尺寸超差: {_todayData.SizeErrorCount}");
                    sb.AppendLine($"其他: {_todayData.OtherDefectCount}");

                    File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 检查日期翻转（跨天时重置统计）
        /// </summary>
        private void CheckDateRollover()
        {
            var today = DateTime.Now.Date;
            if (_todayData.Date != today)
            {
                // 保存昨日统计到历史
                _historyData[_todayData.Date.ToString("yyyy-MM-dd")] = _todayData;

                // 新建今日统计
                _todayData = new StatisticsData { Date = today };
            }
        }
    }
}
