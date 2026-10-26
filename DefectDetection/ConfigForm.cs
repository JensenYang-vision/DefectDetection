using System;
using System.Drawing;
using System.Windows.Forms;
using DefectDetection.Helpers.Data;
using DefectDetection.Models;

namespace DefectDetection
{
    /// <summary>
    /// 参数配置窗体
    /// </summary>
    public partial class ConfigForm : Form
    {
        private readonly ConfigHelper _configHelper;
        private readonly LogHelper _logHelper;
        private SystemConfig _config;

        public ConfigForm(ConfigHelper configHelper, LogHelper logHelper)
        {
            _configHelper = configHelper ?? throw new ArgumentNullException(nameof(configHelper));
            _logHelper = logHelper ?? throw new ArgumentNullException(nameof(logHelper));
            _config = _configHelper.Config;

            InitializeComponent();
            LoadConfig();
        }

        /// <summary>
        /// 加载配置到界面
        /// </summary>
        private void LoadConfig()
        {
            // 系统配置
            chkUseSimulatedCamera.Checked = _config.UseSimulatedCamera;
            chkUseSimulatedMotion.Checked = _config.UseSimulatedMotion;
            nudCameraIndex.Value = _config.CameraIndex;

            // 视觉参数
            nudExposure.Value = (decimal)_config.Vision.Exposure;
            nudGain.Value = (decimal)_config.Vision.Gain;
            nudFrameRate.Value = (decimal)_config.Vision.FrameRate;
            nudWidthUpper.Value = (decimal)_config.Vision.WidthUpperLimit;
            nudWidthLower.Value = (decimal)_config.Vision.WidthLowerLimit;
            nudHeightUpper.Value = (decimal)_config.Vision.HeightUpperLimit;
            nudHeightLower.Value = (decimal)_config.Vision.HeightLowerLimit;
            nudDefectSensitivity.Value = _config.Vision.DefectSensitivity;
            nudPixelEquivalent.Value = (decimal)_config.Vision.PixelEquivalent;

            // 运动参数
            nudSpeedX.Value = (decimal)_config.Motion.SpeedX;
            nudSpeedY.Value = (decimal)_config.Motion.SpeedY;
            nudSpeedZ.Value = (decimal)_config.Motion.SpeedZ;
            nudAccelX.Value = (decimal)_config.Motion.AccelX;
            nudAccelY.Value = (decimal)_config.Motion.AccelY;
            nudAccelZ.Value = (decimal)_config.Motion.AccelZ;
            nudLimitXPos.Value = (decimal)_config.Motion.LimitX_Positive;
            nudLimitXNeg.Value = (decimal)_config.Motion.LimitX_Negative;
            nudLimitYPos.Value = (decimal)_config.Motion.LimitY_Positive;
            nudLimitYNeg.Value = (decimal)_config.Motion.LimitY_Negative;
            nudLimitZPos.Value = (decimal)_config.Motion.LimitZ_Positive;
            nudLimitZNeg.Value = (decimal)_config.Motion.LimitZ_Negative;
            nudGrabHeight.Value = (decimal)_config.Motion.GrabHeight;
            nudPlaceHeightA.Value = (decimal)_config.Motion.PlaceHeightA;
            nudPlaceHeightB.Value = (decimal)_config.Motion.PlaceHeightB;
            nudAreaX.Value = (decimal)_config.Motion.AreaX;
            nudAreaY.Value = (decimal)_config.Motion.AreaY;
            nudAreaBX.Value = (decimal)_config.Motion.AreaBX;
            nudAreaBY.Value = (decimal)_config.Motion.AreaBY;
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 系统配置
                _configHelper.UpdateSystemConfig(cfg =>
                {
                    cfg.UseSimulatedCamera = chkUseSimulatedCamera.Checked;
                    cfg.UseSimulatedMotion = chkUseSimulatedMotion.Checked;
                    cfg.CameraIndex = (int)nudCameraIndex.Value;
                });

                // 视觉参数
                _configHelper.UpdateVisionConfig(cfg =>
                {
                    cfg.Exposure = (double)nudExposure.Value;
                    cfg.Gain = (double)nudGain.Value;
                    cfg.FrameRate = (double)nudFrameRate.Value;
                    cfg.WidthUpperLimit = (double)nudWidthUpper.Value;
                    cfg.WidthLowerLimit = (double)nudWidthLower.Value;
                    cfg.HeightUpperLimit = (double)nudHeightUpper.Value;
                    cfg.HeightLowerLimit = (double)nudHeightLower.Value;
                    cfg.DefectSensitivity = (int)nudDefectSensitivity.Value;
                    cfg.PixelEquivalent = (double)nudPixelEquivalent.Value;
                });

                // 运动参数
                _configHelper.UpdateMotionConfig(cfg =>
                {
                    cfg.SpeedX = (double)nudSpeedX.Value;
                    cfg.SpeedY = (double)nudSpeedY.Value;
                    cfg.SpeedZ = (double)nudSpeedZ.Value;
                    cfg.AccelX = (double)nudAccelX.Value;
                    cfg.AccelY = (double)nudAccelY.Value;
                    cfg.AccelZ = (double)nudAccelZ.Value;
                    cfg.LimitX_Positive = (double)nudLimitXPos.Value;
                    cfg.LimitX_Negative = (double)nudLimitXNeg.Value;
                    cfg.LimitY_Positive = (double)nudLimitYPos.Value;
                    cfg.LimitY_Negative = (double)nudLimitYNeg.Value;
                    cfg.LimitZ_Positive = (double)nudLimitZPos.Value;
                    cfg.LimitZ_Negative = (double)nudLimitZNeg.Value;
                    cfg.GrabHeight = (double)nudGrabHeight.Value;
                    cfg.PlaceHeightA = (double)nudPlaceHeightA.Value;
                    cfg.PlaceHeightB = (double)nudPlaceHeightB.Value;
                    cfg.AreaX = (double)nudAreaX.Value;
                    cfg.AreaY = (double)nudAreaY.Value;
                    cfg.AreaBX = (double)nudAreaBX.Value;
                    cfg.AreaBY = (double)nudAreaBY.Value;
                });

                _logHelper.LogOperation("参数配置已保存");
                MessageBox.Show("配置保存成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                _logHelper.LogException("保存配置异常", ex);
                MessageBox.Show($"保存配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 恢复默认
        /// </summary>
        private void btnReset_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("确定要恢复默认配置吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                _config = _configHelper.ResetToDefault();
                LoadConfig();
                _logHelper.LogOperation("配置已恢复默认");
                MessageBox.Show("已恢复默认配置，请点击保存以生效", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
