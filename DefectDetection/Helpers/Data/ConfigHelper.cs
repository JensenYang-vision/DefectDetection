using System;
using System.IO;
using System.Text.Json;
using DefectDetection.Models;

namespace DefectDetection.Helpers.Data
{
    /// <summary>
    /// 配置帮助类 - 负责参数的读写、持久化与重置
    /// </summary>
    public class ConfigHelper
    {
        private readonly string _configFilePath;
        private SystemConfig _config;
        private readonly object _lockObj = new object();

        /// <summary>
        /// 配置变更事件
        /// </summary>
        public event Action<SystemConfig> ConfigChanged;

        /// <summary>
        /// 当前配置
        /// </summary>
        public SystemConfig Config => _config;

        public ConfigHelper(string configFileName = "config.json")
        {
            // 配置文件保存在程序运行目录下的 Config 文件夹
            var configDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");
            if (!Directory.Exists(configDir))
            {
                Directory.CreateDirectory(configDir);
            }
            _configFilePath = Path.Combine(configDir, configFileName);
            _config = new SystemConfig();
        }

        /// <summary>
        /// 加载配置（从文件读取，若文件不存在则使用默认配置并保存）
        /// </summary>
        public SystemConfig Load()
        {
            lock (_lockObj)
            {
                try
                {
                    if (File.Exists(_configFilePath))
                    {
                        var json = File.ReadAllText(_configFilePath);
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            WriteIndented = true
                        };
                        _config = JsonSerializer.Deserialize<SystemConfig>(json, options) ?? new SystemConfig();
                    }
                    else
                    {
                        _config = new SystemConfig();
                        Save();
                    }

                    ConfigChanged?.Invoke(_config);
                    return _config;
                }
                catch (Exception)
                {
                    // 读取失败时使用默认配置
                    _config = new SystemConfig();
                    Save();
                    ConfigChanged?.Invoke(_config);
                    return _config;
                }
            }
        }

        /// <summary>
        /// 保存配置到文件
        /// </summary>
        public bool Save()
        {
            lock (_lockObj)
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    };
                    var json = JsonSerializer.Serialize(_config, options);
                    File.WriteAllText(_configFilePath, json);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 更新视觉参数并保存
        /// </summary>
        public bool UpdateVisionConfig(Action<VisionConfig> updateAction)
        {
            lock (_lockObj)
            {
                try
                {
                    updateAction?.Invoke(_config.Vision);
                    return Save();
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 更新运动参数并保存
        /// </summary>
        public bool UpdateMotionConfig(Action<MotionConfig> updateAction)
        {
            lock (_lockObj)
            {
                try
                {
                    updateAction?.Invoke(_config.Motion);
                    return Save();
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 更新系统参数并保存
        /// </summary>
        public bool UpdateSystemConfig(Action<SystemConfig> updateAction)
        {
            lock (_lockObj)
            {
                try
                {
                    updateAction?.Invoke(_config);
                    return Save();
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 恢复默认配置
        /// </summary>
        public SystemConfig ResetToDefault()
        {
            lock (_lockObj)
            {
                _config = new SystemConfig();
                Save();
                ConfigChanged?.Invoke(_config);
                return _config;
            }
        }

        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        public string GetConfigFilePath()
        {
            return _configFilePath;
        }
    }
}
