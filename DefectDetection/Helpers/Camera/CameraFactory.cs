using DefectDetection.Models;

namespace DefectDetection.Helpers.Camera
{
    /// <summary>
    /// 相机工厂 - 根据配置创建模拟相机或海康相机实例
    /// </summary>
    public static class CameraFactory
    {
        /// <summary>
        /// 根据配置创建相机实例
        /// </summary>
        /// <param name="config">视觉参数配置</param>
        /// <param name="useSimulated">是否使用模拟相机</param>
        /// <returns>相机实例</returns>
        public static ICamera CreateCamera(VisionConfig config, bool useSimulated = true)
        {
            if (useSimulated)
            {
                return new SimulatedCamera(config);
            }
            else
            {
                return new HikCamera(config);
            }
        }

        /// <summary>
        /// 根据系统配置创建相机实例
        /// </summary>
        /// <param name="systemConfig">系统配置</param>
        /// <returns>相机实例</returns>
        public static ICamera CreateCamera(SystemConfig systemConfig)
        {
            return CreateCamera(systemConfig.Vision, systemConfig.UseSimulatedCamera);
        }
    }
}
