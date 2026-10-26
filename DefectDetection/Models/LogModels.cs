using System;

namespace DefectDetection.Models
{
    /// <summary>
    /// 日志级别枚举
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 调试
        /// </summary>
        Debug,

        /// <summary>
        /// 信息
        /// </summary>
        Info,

        /// <summary>
        /// 警告
        /// </summary>
        Warning,

        /// <summary>
        /// 错误
        /// </summary>
        Error,

        /// <summary>
        /// 致命
        /// </summary>
        Fatal
    }

    /// <summary>
    /// 日志类型枚举
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 操作日志
        /// </summary>
        Operation,

        /// <summary>
        /// 检测日志
        /// </summary>
        Detection,

        /// <summary>
        /// 异常日志
        /// </summary>
        Exception,

        /// <summary>
        /// 系统日志
        /// </summary>
        System
    }

    /// <summary>
    /// 日志记录模型
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// 日志时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 日志级别
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        public LogType Type { get; set; }

        /// <summary>
        /// 日志消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 详细信息
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        /// 格式化为字符串
        /// </summary>
        public override string ToString()
        {
            return $"[{Time:yyyy-MM-dd HH:mm:ss.fff}] [{Level}] [{Type}] {Message}" +
                   (string.IsNullOrEmpty(Details) ? "" : $" | {Details}") +
                   (string.IsNullOrEmpty(Exception) ? "" : $" | 异常: {Exception}");
        }
    }
}
