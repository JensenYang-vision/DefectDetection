using DefectDetection.Models;

namespace DefectDetection.Helpers.Motion
{
    /// <summary>
    /// 运动控制工厂 - 根据配置创建模拟运动、正运动ZMotion、雷赛DMC3400或固高GTS-400-PCIe实例
    /// </summary>
    public static class MotionFactory
    {
        /// <summary>
        /// 根据运动类型创建运动控制实例
        /// </summary>
        /// <param name="config">运动参数配置</param>
        /// <param name="motionType">运动控制卡类型</param>
        /// <param name="controllerIP">控制器IP地址（仅正运动使用）</param>
        /// <returns>运动控制实例</returns>
        public static IMotion CreateMotion(MotionConfig config, MotionType motionType, string controllerIP = "192.168.0.11")
        {
            switch (motionType)
            {
                case MotionType.Simulated:
                    return new SimulatedMotion(config);

                case MotionType.ZMotion:
                    return new ZMotion(config, controllerIP);

                case MotionType.LeiSai:
                    return new LeiSaiMotion(config);

                case MotionType.GaoGong:
                    return new GaoGongMotion(config);

                default:
                    return new SimulatedMotion(config);
            }
        }

        /// <summary>
        /// 根据系统配置创建运动控制实例
        /// </summary>
        /// <param name="systemConfig">系统配置</param>
        /// <returns>运动控制实例</returns>
        public static IMotion CreateMotion(SystemConfig systemConfig)
        {
            return CreateMotion(
                systemConfig.Motion,
                systemConfig.MotionType,
                systemConfig.ZMotionIP);
        }
    }
}
