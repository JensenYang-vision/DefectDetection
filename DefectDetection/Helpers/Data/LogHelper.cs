using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DefectDetection.Models;

namespace DefectDetection.Helpers.Data
{
    /// <summary>
    /// 日志帮助类 - 分级记录日志，按日期归档，支持查询与导出
    /// </summary>
    public class LogHelper : IDisposable
    {
        private readonly string _logBaseDir;
        private readonly object _lockObj = new object();
        private StreamWriter _currentWriter;
        private string _currentLogDate;
        private string _currentLogPath;
        private LogLevel _minLevel = LogLevel.Debug;

        // 内存中保留最近N条日志，供界面实时显示
        private readonly LinkedList<LogEntry> _recentLogs = new LinkedList<LogEntry>();
        private const int MaxRecentLogs = 500;

        /// <summary>
        /// 新日志事件（供UI订阅实时显示）
        /// </summary>
        public event Action<LogEntry> LogAdded;

        public LogHelper(LogLevel minLevel = LogLevel.Debug)
        {
            _minLevel = minLevel;
            _logBaseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            if (!Directory.Exists(_logBaseDir))
            {
                Directory.CreateDirectory(_logBaseDir);
            }
        }

        /// <summary>
        /// 记录操作日志
        /// </summary>
        public void LogOperation(string message, string details = "")
        {
            WriteLog(LogLevel.Info, LogType.Operation, message, details);
        }

        /// <summary>
        /// 记录检测日志
        /// </summary>
        public void LogDetection(string message, string details = "")
        {
            WriteLog(LogLevel.Info, LogType.Detection, message, details);
        }

        /// <summary>
        /// 记录异常日志
        /// </summary>
        public void LogException(string message, Exception ex = null, string details = "")
        {
            WriteLog(LogLevel.Error, LogType.Exception, message, details, ex?.ToString());
        }

        /// <summary>
        /// 记录系统日志
        /// </summary>
        public void LogSystem(string message, LogLevel level = LogLevel.Info, string details = "")
        {
            WriteLog(level, LogType.System, message, details);
        }

        /// <summary>
        /// 记录调试日志
        /// </summary>
        public void LogDebug(string message, string details = "")
        {
            WriteLog(LogLevel.Debug, LogType.System, message, details);
        }

        /// <summary>
        /// 通用日志写入
        /// </summary>
        public void WriteLog(LogLevel level, LogType type, string message, string details = "", string exception = "")
        {
            if (level < _minLevel) return;

            var entry = new LogEntry
            {
                Time = DateTime.Now,
                Level = level,
                Type = type,
                Message = message,
                Details = details,
                Exception = exception
            };

            lock (_lockObj)
            {
                // 添加到内存缓存
                _recentLogs.AddLast(entry);
                while (_recentLogs.Count > MaxRecentLogs)
                {
                    _recentLogs.RemoveFirst();
                }

                // 写入文件
                WriteToFile(entry);
            }

            // 触发事件（在锁外触发，避免死锁）
            LogAdded?.Invoke(entry);
        }

        private void WriteToFile(LogEntry entry)
        {
            try
            {
                var today = entry.Time.ToString("yyyy-MM-dd");

                // 日期变更时切换文件
                if (_currentLogDate != today)
                {
                    _currentWriter?.Flush();
                    _currentWriter?.Dispose();

                    var dateDir = Path.Combine(_logBaseDir, today);
                    if (!Directory.Exists(dateDir))
                    {
                        Directory.CreateDirectory(dateDir);
                    }

                    _currentLogPath = Path.Combine(dateDir, $"log_{today}.txt");
                    _currentWriter = new StreamWriter(_currentLogPath, true) { AutoFlush = true };
                    _currentLogDate = today;
                }

                _currentWriter?.WriteLine(entry.ToString());
            }
            catch (Exception)
            {
                // 日志写入失败不应影响程序运行
            }
        }

        /// <summary>
        /// 获取最近的日志
        /// </summary>
        public List<LogEntry> GetRecentLogs(int count = 100)
        {
            lock (_lockObj)
            {
                return _recentLogs.TakeLast(count).ToList();
            }
        }

        /// <summary>
        /// 按条件查询日志
        /// </summary>
        public List<LogEntry> QueryLogs(DateTime? date = null, LogLevel? level = null, LogType? type = null)
        {
            var results = new List<LogEntry>();

            try
            {
                var targetDate = date?.ToString("yyyy-MM-dd") ?? DateTime.Now.ToString("yyyy-MM-dd");
                var dateDir = Path.Combine(_logBaseDir, targetDate);

                if (!Directory.Exists(dateDir)) return results;

                var logFiles = Directory.GetFiles(dateDir, "log_*.txt");
                foreach (var file in logFiles)
                {
                    foreach (var line in File.ReadLines(file))
                    {
                        var entry = ParseLogLine(line);
                        if (entry == null) continue;

                        if (level.HasValue && entry.Level != level.Value) continue;
                        if (type.HasValue && entry.Type != type.Value) continue;

                        results.Add(entry);
                    }
                }
            }
            catch (Exception)
            {
                // 查询失败返回空列表
            }

            return results;
        }

        /// <summary>
        /// 导出日志到指定文件
        /// </summary>
        public bool ExportLogs(string exportPath, DateTime? date = null, LogLevel? level = null, LogType? type = null)
        {
            try
            {
                var logs = QueryLogs(date, level, type);
                var lines = logs.Select(l => l.ToString());
                File.WriteAllLines(exportPath, lines);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取所有日志日期
        /// </summary>
        public List<string> GetLogDates()
        {
            try
            {
                if (!Directory.Exists(_logBaseDir)) return new List<string>();

                return Directory.GetDirectories(_logBaseDir)
                    .Select(d => Path.GetFileName(d))
                    .OrderByDescending(d => d)
                    .ToList();
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }

        private LogEntry ParseLogLine(string line)
        {
            try
            {
                // 格式: [2026-08-11 10:30:00.000] [Info] [Operation] Message | Details
                if (string.IsNullOrWhiteSpace(line)) return null;

                var parts = line.Split(new[] { "] [" }, 3, StringSplitOptions.None);
                if (parts.Length < 3) return null;

                var timeStr = parts[0].TrimStart('[');
                var time = DateTime.Parse(timeStr);

                var levelStr = parts[1];
                var level = Enum.Parse<LogLevel>(levelStr);

                var rest = parts[2];
                var typeEnd = rest.IndexOf(']');
                if (typeEnd < 0) return null;

                var typeStr = rest.Substring(0, typeEnd);
                var type = Enum.Parse<LogType>(typeStr);

                var messagePart = rest.Substring(typeEnd + 1).Trim();
                string message, details = "", exception = "";

                var pipeIndex = messagePart.IndexOf(" | ");
                if (pipeIndex >= 0)
                {
                    message = messagePart.Substring(0, pipeIndex);
                    var remain = messagePart.Substring(pipeIndex + 3);

                    if (remain.StartsWith("异常: "))
                    {
                        exception = remain;
                    }
                    else
                    {
                        details = remain;
                    }
                }
                else
                {
                    message = messagePart;
                }

                return new LogEntry
                {
                    Time = time,
                    Level = level,
                    Type = type,
                    Message = message,
                    Details = details,
                    Exception = exception
                };
            }
            catch
            {
                return null;
            }
        }

        public void Dispose()
        {
            lock (_lockObj)
            {
                _currentWriter?.Flush();
                _currentWriter?.Dispose();
                _currentWriter = null;
            }
        }
    }
}
