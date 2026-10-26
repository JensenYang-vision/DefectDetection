namespace DefectDetection
{
    partial class ConfigForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageSystem = new System.Windows.Forms.TabPage();
            this.tabPageVision = new System.Windows.Forms.TabPage();
            this.tabPageMotion = new System.Windows.Forms.TabPage();

            // 系统配置控件
            this.grpSystem = new System.Windows.Forms.GroupBox();
            this.chkUseSimulatedCamera = new System.Windows.Forms.CheckBox();
            this.chkUseSimulatedMotion = new System.Windows.Forms.CheckBox();
            this.lblCameraIndex = new System.Windows.Forms.Label();
            this.nudCameraIndex = new System.Windows.Forms.NumericUpDown();

            // 视觉参数控件
            this.grpVisionCamera = new System.Windows.Forms.GroupBox();
            this.lblExposure = new System.Windows.Forms.Label();
            this.nudExposure = new System.Windows.Forms.NumericUpDown();
            this.lblGain = new System.Windows.Forms.Label();
            this.nudGain = new System.Windows.Forms.NumericUpDown();
            this.lblFrameRate = new System.Windows.Forms.Label();
            this.nudFrameRate = new System.Windows.Forms.NumericUpDown();

            this.grpVisionDetection = new System.Windows.Forms.GroupBox();
            this.lblWidthUpper = new System.Windows.Forms.Label();
            this.nudWidthUpper = new System.Windows.Forms.NumericUpDown();
            this.lblWidthLower = new System.Windows.Forms.Label();
            this.nudWidthLower = new System.Windows.Forms.NumericUpDown();
            this.lblHeightUpper = new System.Windows.Forms.Label();
            this.nudHeightUpper = new System.Windows.Forms.NumericUpDown();
            this.lblHeightLower = new System.Windows.Forms.Label();
            this.nudHeightLower = new System.Windows.Forms.NumericUpDown();
            this.lblDefectSensitivity = new System.Windows.Forms.Label();
            this.nudDefectSensitivity = new System.Windows.Forms.NumericUpDown();
            this.lblPixelEquivalent = new System.Windows.Forms.Label();
            this.nudPixelEquivalent = new System.Windows.Forms.NumericUpDown();

            // 运动参数控件
            this.grpMotionSpeed = new System.Windows.Forms.GroupBox();
            this.lblSpeedX = new System.Windows.Forms.Label();
            this.nudSpeedX = new System.Windows.Forms.NumericUpDown();
            this.lblSpeedY = new System.Windows.Forms.Label();
            this.nudSpeedY = new System.Windows.Forms.NumericUpDown();
            this.lblSpeedZ = new System.Windows.Forms.Label();
            this.nudSpeedZ = new System.Windows.Forms.NumericUpDown();
            this.lblAccelX = new System.Windows.Forms.Label();
            this.nudAccelX = new System.Windows.Forms.NumericUpDown();
            this.lblAccelY = new System.Windows.Forms.Label();
            this.nudAccelY = new System.Windows.Forms.NumericUpDown();
            this.lblAccelZ = new System.Windows.Forms.Label();
            this.nudAccelZ = new System.Windows.Forms.NumericUpDown();

            this.grpMotionLimit = new System.Windows.Forms.GroupBox();
            this.lblLimitXPos = new System.Windows.Forms.Label();
            this.nudLimitXPos = new System.Windows.Forms.NumericUpDown();
            this.lblLimitXNeg = new System.Windows.Forms.Label();
            this.nudLimitXNeg = new System.Windows.Forms.NumericUpDown();
            this.lblLimitYPos = new System.Windows.Forms.Label();
            this.nudLimitYPos = new System.Windows.Forms.NumericUpDown();
            this.lblLimitYNeg = new System.Windows.Forms.Label();
            this.nudLimitYNeg = new System.Windows.Forms.NumericUpDown();
            this.lblLimitZPos = new System.Windows.Forms.Label();
            this.nudLimitZPos = new System.Windows.Forms.NumericUpDown();
            this.lblLimitZNeg = new System.Windows.Forms.Label();
            this.nudLimitZNeg = new System.Windows.Forms.NumericUpDown();

            this.grpMotionPosition = new System.Windows.Forms.GroupBox();
            this.lblGrabHeight = new System.Windows.Forms.Label();
            this.nudGrabHeight = new System.Windows.Forms.NumericUpDown();
            this.lblPlaceHeightA = new System.Windows.Forms.Label();
            this.nudPlaceHeightA = new System.Windows.Forms.NumericUpDown();
            this.lblPlaceHeightB = new System.Windows.Forms.Label();
            this.nudPlaceHeightB = new System.Windows.Forms.NumericUpDown();
            this.lblAreaX = new System.Windows.Forms.Label();
            this.nudAreaX = new System.Windows.Forms.NumericUpDown();
            this.lblAreaY = new System.Windows.Forms.Label();
            this.nudAreaY = new System.Windows.Forms.NumericUpDown();
            this.lblAreaBX = new System.Windows.Forms.Label();
            this.nudAreaBX = new System.Windows.Forms.NumericUpDown();
            this.lblAreaBY = new System.Windows.Forms.Label();
            this.nudAreaBY = new System.Windows.Forms.NumericUpDown();

            // 按钮
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();

            this.tabControl.SuspendLayout();
            this.tabPageSystem.SuspendLayout();
            this.tabPageVision.SuspendLayout();
            this.tabPageMotion.SuspendLayout();
            this.grpSystem.SuspendLayout();
            this.grpVisionCamera.SuspendLayout();
            this.grpVisionDetection.SuspendLayout();
            this.grpMotionSpeed.SuspendLayout();
            this.grpMotionLimit.SuspendLayout();
            this.grpMotionPosition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCameraIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudExposure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFrameRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidthUpper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidthLower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeightUpper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeightLower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDefectSensitivity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPixelEquivalent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpeedX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpeedY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpeedZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAccelX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAccelY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAccelZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLimitXPos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLimitXNeg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLimitYPos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLimitYNeg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLimitZPos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLimitZNeg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrabHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPlaceHeightA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPlaceHeightB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAreaX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAreaY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAreaBX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAreaBY)).BeginInit();
            this.SuspendLayout();

            //
            // tabControl
            //
            this.tabControl.Controls.Add(this.tabPageSystem);
            this.tabControl.Controls.Add(this.tabPageVision);
            this.tabControl.Controls.Add(this.tabPageMotion);
            this.tabControl.Location = new System.Drawing.Point(10, 10);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(560, 450);
            this.tabControl.TabIndex = 0;

            //
            // tabPageSystem
            //
            this.tabPageSystem.Controls.Add(this.grpSystem);
            this.tabPageSystem.Location = new System.Drawing.Point(4, 22);
            this.tabPageSystem.Name = "tabPageSystem";
            this.tabPageSystem.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSystem.Size = new System.Drawing.Size(552, 424);
            this.tabPageSystem.TabIndex = 0;
            this.tabPageSystem.Text = "系统配置";
            this.tabPageSystem.UseVisualStyleBackColor = true;

            //
            // grpSystem
            //
            this.grpSystem.Controls.Add(this.chkUseSimulatedCamera);
            this.grpSystem.Controls.Add(this.chkUseSimulatedMotion);
            this.grpSystem.Controls.Add(this.lblCameraIndex);
            this.grpSystem.Controls.Add(this.nudCameraIndex);
            this.grpSystem.Location = new System.Drawing.Point(10, 10);
            this.grpSystem.Name = "grpSystem";
            this.grpSystem.Size = new System.Drawing.Size(530, 150);
            this.grpSystem.TabIndex = 0;
            this.grpSystem.TabStop = false;
            this.grpSystem.Text = "设备模式";

            this.chkUseSimulatedCamera.Text = "使用模拟相机（无工业相机时使用电脑摄像头）";
            this.chkUseSimulatedCamera.Location = new System.Drawing.Point(10, 25);
            this.chkUseSimulatedCamera.Size = new System.Drawing.Size(350, 20);
            this.chkUseSimulatedCamera.Checked = true;

            this.chkUseSimulatedMotion.Text = "使用模拟运动控制（无运动控制卡时模拟动作）";
            this.chkUseSimulatedMotion.Location = new System.Drawing.Point(10, 55);
            this.chkUseSimulatedMotion.Size = new System.Drawing.Size(350, 20);
            this.chkUseSimulatedMotion.Checked = true;

            this.lblCameraIndex.Text = "摄像头索引:";
            this.lblCameraIndex.Location = new System.Drawing.Point(10, 90);
            this.lblCameraIndex.Size = new System.Drawing.Size(80, 20);

            this.nudCameraIndex = new System.Windows.Forms.NumericUpDown();
            this.nudCameraIndex.Location = new System.Drawing.Point(100, 90);
            this.nudCameraIndex.Size = new System.Drawing.Size(80, 23);
            this.nudCameraIndex.Minimum = 0;
            this.nudCameraIndex.Maximum = 10;
            this.nudCameraIndex.Value = 0;

            //
            // tabPageVision
            //
            this.tabPageVision.Controls.Add(this.grpVisionCamera);
            this.tabPageVision.Controls.Add(this.grpVisionDetection);
            this.tabPageVision.Location = new System.Drawing.Point(4, 22);
            this.tabPageVision.Name = "tabPageVision";
            this.tabPageVision.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageVision.Size = new System.Drawing.Size(552, 424);
            this.tabPageVision.TabIndex = 1;
            this.tabPageVision.Text = "视觉参数";
            this.tabPageVision.UseVisualStyleBackColor = true;

            //
            // grpVisionCamera
            //
            this.grpVisionCamera.Controls.Add(this.lblExposure);
            this.grpVisionCamera.Controls.Add(this.nudExposure);
            this.grpVisionCamera.Controls.Add(this.lblGain);
            this.grpVisionCamera.Controls.Add(this.nudGain);
            this.grpVisionCamera.Controls.Add(this.lblFrameRate);
            this.grpVisionCamera.Controls.Add(this.nudFrameRate);
            this.grpVisionCamera.Location = new System.Drawing.Point(10, 10);
            this.grpVisionCamera.Name = "grpVisionCamera";
            this.grpVisionCamera.Size = new System.Drawing.Size(530, 120);
            this.grpVisionCamera.TabIndex = 0;
            this.grpVisionCamera.TabStop = false;
            this.grpVisionCamera.Text = "相机参数";

            int y = 25;
            this.lblExposure.Text = "曝光值:";
            this.lblExposure.Location = new System.Drawing.Point(10, y);
            this.lblExposure.Size = new System.Drawing.Size(70, 20);
            this.nudExposure = new System.Windows.Forms.NumericUpDown();
            this.nudExposure.Location = new System.Drawing.Point(90, y);
            this.nudExposure.Size = new System.Drawing.Size(100, 23);
            this.nudExposure.Minimum = 100;
            this.nudExposure.Maximum = 100000;
            this.nudExposure.Value = 5000;

            this.lblGain.Text = "增益:";
            this.lblGain.Location = new System.Drawing.Point(210, y);
            this.lblGain.Size = new System.Drawing.Size(50, 20);
            this.nudGain = new System.Windows.Forms.NumericUpDown();
            this.nudGain.Location = new System.Drawing.Point(270, y);
            this.nudGain.Size = new System.Drawing.Size(80, 23);
            this.nudGain.Minimum = 0;
            this.nudGain.Maximum = 100;
            this.nudGain.Value = 1;
            this.nudGain.DecimalPlaces = 1;
            this.nudGain.Increment = 0.1M;

            this.lblFrameRate.Text = "帧率:";
            this.lblFrameRate.Location = new System.Drawing.Point(370, y);
            this.lblFrameRate.Size = new System.Drawing.Size(50, 20);
            this.nudFrameRate = new System.Windows.Forms.NumericUpDown();
            this.nudFrameRate.Location = new System.Drawing.Point(430, y);
            this.nudFrameRate.Size = new System.Drawing.Size(80, 23);
            this.nudFrameRate.Minimum = 1;
            this.nudFrameRate.Maximum = 120;
            this.nudFrameRate.Value = 30;

            y += 35;
            // 第二行可以添加更多相机参数

            //
            // grpVisionDetection
            //
            this.grpVisionDetection.Controls.Add(this.lblWidthUpper);
            this.grpVisionDetection.Controls.Add(this.nudWidthUpper);
            this.grpVisionDetection.Controls.Add(this.lblWidthLower);
            this.grpVisionDetection.Controls.Add(this.nudWidthLower);
            this.grpVisionDetection.Controls.Add(this.lblHeightUpper);
            this.grpVisionDetection.Controls.Add(this.nudHeightUpper);
            this.grpVisionDetection.Controls.Add(this.lblHeightLower);
            this.grpVisionDetection.Controls.Add(this.nudHeightLower);
            this.grpVisionDetection.Controls.Add(this.lblDefectSensitivity);
            this.grpVisionDetection.Controls.Add(this.nudDefectSensitivity);
            this.grpVisionDetection.Controls.Add(this.lblPixelEquivalent);
            this.grpVisionDetection.Controls.Add(this.nudPixelEquivalent);
            this.grpVisionDetection.Location = new System.Drawing.Point(10, 140);
            this.grpVisionDetection.Name = "grpVisionDetection";
            this.grpVisionDetection.Size = new System.Drawing.Size(530, 200);
            this.grpVisionDetection.TabIndex = 1;
            this.grpVisionDetection.TabStop = false;
            this.grpVisionDetection.Text = "检测参数";

            y = 25;
            this.lblWidthUpper.Text = "宽度上限(mm):";
            this.lblWidthUpper.Location = new System.Drawing.Point(10, y);
            this.lblWidthUpper.Size = new System.Drawing.Size(90, 20);
            this.nudWidthUpper = new System.Windows.Forms.NumericUpDown();
            this.nudWidthUpper.Location = new System.Drawing.Point(110, y);
            this.nudWidthUpper.Size = new System.Drawing.Size(80, 23);
            this.nudWidthUpper.Minimum = 0;
            this.nudWidthUpper.Maximum = 1000;
            this.nudWidthUpper.Value = 50;
            this.nudWidthUpper.DecimalPlaces = 1;

            this.lblWidthLower.Text = "宽度下限(mm):";
            this.lblWidthLower.Location = new System.Drawing.Point(210, y);
            this.lblWidthLower.Size = new System.Drawing.Size(90, 20);
            this.nudWidthLower = new System.Windows.Forms.NumericUpDown();
            this.nudWidthLower.Location = new System.Drawing.Point(310, y);
            this.nudWidthLower.Size = new System.Drawing.Size(80, 23);
            this.nudWidthLower.Minimum = 0;
            this.nudWidthLower.Maximum = 1000;
            this.nudWidthLower.Value = 30;
            this.nudWidthLower.DecimalPlaces = 1;

            y += 35;
            this.lblHeightUpper.Text = "高度上限(mm):";
            this.lblHeightUpper.Location = new System.Drawing.Point(10, y);
            this.lblHeightUpper.Size = new System.Drawing.Size(90, 20);
            this.nudHeightUpper = new System.Windows.Forms.NumericUpDown();
            this.nudHeightUpper.Location = new System.Drawing.Point(110, y);
            this.nudHeightUpper.Size = new System.Drawing.Size(80, 23);
            this.nudHeightUpper.Minimum = 0;
            this.nudHeightUpper.Maximum = 1000;
            this.nudHeightUpper.Value = 50;
            this.nudHeightUpper.DecimalPlaces = 1;

            this.lblHeightLower.Text = "高度下限(mm):";
            this.lblHeightLower.Location = new System.Drawing.Point(210, y);
            this.lblHeightLower.Size = new System.Drawing.Size(90, 20);
            this.nudHeightLower = new System.Windows.Forms.NumericUpDown();
            this.nudHeightLower.Location = new System.Drawing.Point(310, y);
            this.nudHeightLower.Size = new System.Drawing.Size(80, 23);
            this.nudHeightLower.Minimum = 0;
            this.nudHeightLower.Maximum = 1000;
            this.nudHeightLower.Value = 30;
            this.nudHeightLower.DecimalPlaces = 1;

            y += 35;
            this.lblDefectSensitivity.Text = "缺陷灵敏度:";
            this.lblDefectSensitivity.Location = new System.Drawing.Point(10, y);
            this.lblDefectSensitivity.Size = new System.Drawing.Size(90, 20);
            this.nudDefectSensitivity = new System.Windows.Forms.NumericUpDown();
            this.nudDefectSensitivity.Location = new System.Drawing.Point(110, y);
            this.nudDefectSensitivity.Size = new System.Drawing.Size(80, 23);
            this.nudDefectSensitivity.Minimum = 0;
            this.nudDefectSensitivity.Maximum = 100;
            this.nudDefectSensitivity.Value = 50;

            this.lblPixelEquivalent.Text = "像素当量:";
            this.lblPixelEquivalent.Location = new System.Drawing.Point(210, y);
            this.lblPixelEquivalent.Size = new System.Drawing.Size(90, 20);
            this.nudPixelEquivalent = new System.Windows.Forms.NumericUpDown();
            this.nudPixelEquivalent.Location = new System.Drawing.Point(310, y);
            this.nudPixelEquivalent.Size = new System.Drawing.Size(80, 23);
            this.nudPixelEquivalent.Minimum = 0;
            this.nudPixelEquivalent.Maximum = 1000;
            this.nudPixelEquivalent.Value = 10;
            this.nudPixelEquivalent.DecimalPlaces = 2;

            //
            // tabPageMotion
            //
            this.tabPageMotion.Controls.Add(this.grpMotionSpeed);
            this.tabPageMotion.Controls.Add(this.grpMotionLimit);
            this.tabPageMotion.Controls.Add(this.grpMotionPosition);
            this.tabPageMotion.Location = new System.Drawing.Point(4, 22);
            this.tabPageMotion.Name = "tabPageMotion";
            this.tabPageMotion.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMotion.Size = new System.Drawing.Size(552, 424);
            this.tabPageMotion.TabIndex = 2;
            this.tabPageMotion.Text = "运动参数";
            this.tabPageMotion.UseVisualStyleBackColor = true;

            //
            // grpMotionSpeed
            //
            this.grpMotionSpeed.Controls.Add(this.lblSpeedX);
            this.grpMotionSpeed.Controls.Add(this.nudSpeedX);
            this.grpMotionSpeed.Controls.Add(this.lblSpeedY);
            this.grpMotionSpeed.Controls.Add(this.nudSpeedY);
            this.grpMotionSpeed.Controls.Add(this.lblSpeedZ);
            this.grpMotionSpeed.Controls.Add(this.nudSpeedZ);
            this.grpMotionSpeed.Controls.Add(this.lblAccelX);
            this.grpMotionSpeed.Controls.Add(this.nudAccelX);
            this.grpMotionSpeed.Controls.Add(this.lblAccelY);
            this.grpMotionSpeed.Controls.Add(this.nudAccelY);
            this.grpMotionSpeed.Controls.Add(this.lblAccelZ);
            this.grpMotionSpeed.Controls.Add(this.nudAccelZ);
            this.grpMotionSpeed.Location = new System.Drawing.Point(10, 10);
            this.grpMotionSpeed.Name = "grpMotionSpeed";
            this.grpMotionSpeed.Size = new System.Drawing.Size(530, 120);
            this.grpMotionSpeed.TabIndex = 0;
            this.grpMotionSpeed.TabStop = false;
            this.grpMotionSpeed.Text = "速度与加速度";

            y = 25;
            this.lblSpeedX.Text = "X速度:";
            this.lblSpeedX.Location = new System.Drawing.Point(10, y);
            this.lblSpeedX.Size = new System.Drawing.Size(60, 20);
            this.nudSpeedX = new System.Windows.Forms.NumericUpDown();
            this.nudSpeedX.Location = new System.Drawing.Point(70, y);
            this.nudSpeedX.Size = new System.Drawing.Size(70, 23);
            this.nudSpeedX.Minimum = 0;
            this.nudSpeedX.Maximum = 10000;
            this.nudSpeedX.Value = 100;

            this.lblSpeedY.Text = "Y速度:";
            this.lblSpeedY.Location = new System.Drawing.Point(150, y);
            this.lblSpeedY.Size = new System.Drawing.Size(60, 20);
            this.nudSpeedY = new System.Windows.Forms.NumericUpDown();
            this.nudSpeedY.Location = new System.Drawing.Point(210, y);
            this.nudSpeedY.Size = new System.Drawing.Size(70, 23);
            this.nudSpeedY.Minimum = 0;
            this.nudSpeedY.Maximum = 10000;
            this.nudSpeedY.Value = 100;

            this.lblSpeedZ.Text = "Z速度:";
            this.lblSpeedZ.Location = new System.Drawing.Point(290, y);
            this.lblSpeedZ.Size = new System.Drawing.Size(60, 20);
            this.nudSpeedZ = new System.Windows.Forms.NumericUpDown();
            this.nudSpeedZ.Location = new System.Drawing.Point(350, y);
            this.nudSpeedZ.Size = new System.Drawing.Size(70, 23);
            this.nudSpeedZ.Minimum = 0;
            this.nudSpeedZ.Maximum = 10000;
            this.nudSpeedZ.Value = 50;

            y += 35;
            this.lblAccelX.Text = "X加速:";
            this.lblAccelX.Location = new System.Drawing.Point(10, y);
            this.lblAccelX.Size = new System.Drawing.Size(60, 20);
            this.nudAccelX = new System.Windows.Forms.NumericUpDown();
            this.nudAccelX.Location = new System.Drawing.Point(70, y);
            this.nudAccelX.Size = new System.Drawing.Size(70, 23);
            this.nudAccelX.Minimum = 0;
            this.nudAccelX.Maximum = 100000;
            this.nudAccelX.Value = 500;

            this.lblAccelY.Text = "Y加速:";
            this.lblAccelY.Location = new System.Drawing.Point(150, y);
            this.lblAccelY.Size = new System.Drawing.Size(60, 20);
            this.nudAccelY = new System.Windows.Forms.NumericUpDown();
            this.nudAccelY.Location = new System.Drawing.Point(210, y);
            this.nudAccelY.Size = new System.Drawing.Size(70, 23);
            this.nudAccelY.Minimum = 0;
            this.nudAccelY.Maximum = 100000;
            this.nudAccelY.Value = 500;

            this.lblAccelZ.Text = "Z加速:";
            this.lblAccelZ.Location = new System.Drawing.Point(290, y);
            this.lblAccelZ.Size = new System.Drawing.Size(60, 20);
            this.nudAccelZ = new System.Windows.Forms.NumericUpDown();
            this.nudAccelZ.Location = new System.Drawing.Point(350, y);
            this.nudAccelZ.Size = new System.Drawing.Size(70, 23);
            this.nudAccelZ.Minimum = 0;
            this.nudAccelZ.Maximum = 100000;
            this.nudAccelZ.Value = 300;

            //
            // grpMotionLimit
            //
            this.grpMotionLimit.Controls.Add(this.lblLimitXPos);
            this.grpMotionLimit.Controls.Add(this.nudLimitXPos);
            this.grpMotionLimit.Controls.Add(this.lblLimitXNeg);
            this.grpMotionLimit.Controls.Add(this.nudLimitXNeg);
            this.grpMotionLimit.Controls.Add(this.lblLimitYPos);
            this.grpMotionLimit.Controls.Add(this.nudLimitYPos);
            this.grpMotionLimit.Controls.Add(this.lblLimitYNeg);
            this.grpMotionLimit.Controls.Add(this.nudLimitYNeg);
            this.grpMotionLimit.Controls.Add(this.lblLimitZPos);
            this.grpMotionLimit.Controls.Add(this.nudLimitZPos);
            this.grpMotionLimit.Controls.Add(this.lblLimitZNeg);
            this.grpMotionLimit.Controls.Add(this.nudLimitZNeg);
            this.grpMotionLimit.Location = new System.Drawing.Point(10, 140);
            this.grpMotionLimit.Name = "grpMotionLimit";
            this.grpMotionLimit.Size = new System.Drawing.Size(530, 120);
            this.grpMotionLimit.TabIndex = 1;
            this.grpMotionLimit.TabStop = false;
            this.grpMotionLimit.Text = "限位设置";

            y = 25;
            this.lblLimitXPos.Text = "X+限位:";
            this.lblLimitXPos.Location = new System.Drawing.Point(10, y);
            this.lblLimitXPos.Size = new System.Drawing.Size(60, 20);
            this.nudLimitXPos = new System.Windows.Forms.NumericUpDown();
            this.nudLimitXPos.Location = new System.Drawing.Point(70, y);
            this.nudLimitXPos.Size = new System.Drawing.Size(70, 23);
            this.nudLimitXPos.Minimum = 0;
            this.nudLimitXPos.Maximum = 10000;
            this.nudLimitXPos.Value = 500;

            this.lblLimitXNeg.Text = "X-限位:";
            this.lblLimitXNeg.Location = new System.Drawing.Point(150, y);
            this.lblLimitXNeg.Size = new System.Drawing.Size(60, 20);
            this.nudLimitXNeg = new System.Windows.Forms.NumericUpDown();
            this.nudLimitXNeg.Location = new System.Drawing.Point(210, y);
            this.nudLimitXNeg.Size = new System.Drawing.Size(70, 23);
            this.nudLimitXNeg.Minimum = -10000;
            this.nudLimitXNeg.Maximum = 10000;
            this.nudLimitXNeg.Value = 0;

            this.lblLimitYPos.Text = "Y+限位:";
            this.lblLimitYPos.Location = new System.Drawing.Point(290, y);
            this.lblLimitYPos.Size = new System.Drawing.Size(60, 20);
            this.nudLimitYPos = new System.Windows.Forms.NumericUpDown();
            this.nudLimitYPos.Location = new System.Drawing.Point(350, y);
            this.nudLimitYPos.Size = new System.Drawing.Size(70, 23);
            this.nudLimitYPos.Minimum = 0;
            this.nudLimitYPos.Maximum = 10000;
            this.nudLimitYPos.Value = 500;

            y += 35;
            this.lblLimitYNeg.Text = "Y-限位:";
            this.lblLimitYNeg.Location = new System.Drawing.Point(10, y);
            this.lblLimitYNeg.Size = new System.Drawing.Size(60, 20);
            this.nudLimitYNeg = new System.Windows.Forms.NumericUpDown();
            this.nudLimitYNeg.Location = new System.Drawing.Point(70, y);
            this.nudLimitYNeg.Size = new System.Drawing.Size(70, 23);
            this.nudLimitYNeg.Minimum = -10000;
            this.nudLimitYNeg.Maximum = 10000;
            this.nudLimitYNeg.Value = 0;

            this.lblLimitZPos.Text = "Z+限位:";
            this.lblLimitZPos.Location = new System.Drawing.Point(150, y);
            this.lblLimitZPos.Size = new System.Drawing.Size(60, 20);
            this.nudLimitZPos = new System.Windows.Forms.NumericUpDown();
            this.nudLimitZPos.Location = new System.Drawing.Point(210, y);
            this.nudLimitZPos.Size = new System.Drawing.Size(70, 23);
            this.nudLimitZPos.Minimum = 0;
            this.nudLimitZPos.Maximum = 10000;
            this.nudLimitZPos.Value = 100;

            this.lblLimitZNeg.Text = "Z-限位:";
            this.lblLimitZNeg.Location = new System.Drawing.Point(290, y);
            this.lblLimitZNeg.Size = new System.Drawing.Size(60, 20);
            this.nudLimitZNeg = new System.Windows.Forms.NumericUpDown();
            this.nudLimitZNeg.Location = new System.Drawing.Point(350, y);
            this.nudLimitZNeg.Size = new System.Drawing.Size(70, 23);
            this.nudLimitZNeg.Minimum = -10000;
            this.nudLimitZNeg.Maximum = 10000;
            this.nudLimitZNeg.Value = 0;

            //
            // grpMotionPosition
            //
            this.grpMotionPosition.Controls.Add(this.lblGrabHeight);
            this.grpMotionPosition.Controls.Add(this.nudGrabHeight);
            this.grpMotionPosition.Controls.Add(this.lblPlaceHeightA);
            this.grpMotionPosition.Controls.Add(this.nudPlaceHeightA);
            this.grpMotionPosition.Controls.Add(this.lblPlaceHeightB);
            this.grpMotionPosition.Controls.Add(this.nudPlaceHeightB);
            this.grpMotionPosition.Controls.Add(this.lblAreaX);
            this.grpMotionPosition.Controls.Add(this.nudAreaX);
            this.grpMotionPosition.Controls.Add(this.lblAreaY);
            this.grpMotionPosition.Controls.Add(this.nudAreaY);
            this.grpMotionPosition.Controls.Add(this.lblAreaBX);
            this.grpMotionPosition.Controls.Add(this.nudAreaBX);
            this.grpMotionPosition.Controls.Add(this.lblAreaBY);
            this.grpMotionPosition.Controls.Add(this.nudAreaBY);
            this.grpMotionPosition.Location = new System.Drawing.Point(10, 270);
            this.grpMotionPosition.Name = "grpMotionPosition";
            this.grpMotionPosition.Size = new System.Drawing.Size(530, 120);
            this.grpMotionPosition.TabIndex = 2;
            this.grpMotionPosition.TabStop = false;
            this.grpMotionPosition.Text = "位置设置";

            y = 25;
            this.lblGrabHeight.Text = "抓取高度:";
            this.lblGrabHeight.Location = new System.Drawing.Point(10, y);
            this.lblGrabHeight.Size = new System.Drawing.Size(70, 20);
            this.nudGrabHeight = new System.Windows.Forms.NumericUpDown();
            this.nudGrabHeight.Location = new System.Drawing.Point(80, y);
            this.nudGrabHeight.Size = new System.Drawing.Size(60, 23);
            this.nudGrabHeight.Minimum = 0;
            this.nudGrabHeight.Maximum = 1000;
            this.nudGrabHeight.Value = 10;
            this.nudGrabHeight.DecimalPlaces = 1;

            this.lblPlaceHeightA.Text = "A区高度:";
            this.lblPlaceHeightA.Location = new System.Drawing.Point(150, y);
            this.lblPlaceHeightA.Size = new System.Drawing.Size(70, 20);
            this.nudPlaceHeightA = new System.Windows.Forms.NumericUpDown();
            this.nudPlaceHeightA.Location = new System.Drawing.Point(220, y);
            this.nudPlaceHeightA.Size = new System.Drawing.Size(60, 23);
            this.nudPlaceHeightA.Minimum = 0;
            this.nudPlaceHeightA.Maximum = 1000;
            this.nudPlaceHeightA.Value = 20;
            this.nudPlaceHeightA.DecimalPlaces = 1;

            this.lblPlaceHeightB.Text = "B区高度:";
            this.lblPlaceHeightB.Location = new System.Drawing.Point(290, y);
            this.lblPlaceHeightB.Size = new System.Drawing.Size(70, 20);
            this.nudPlaceHeightB = new System.Windows.Forms.NumericUpDown();
            this.nudPlaceHeightB.Location = new System.Drawing.Point(360, y);
            this.nudPlaceHeightB.Size = new System.Drawing.Size(60, 23);
            this.nudPlaceHeightB.Minimum = 0;
            this.nudPlaceHeightB.Maximum = 1000;
            this.nudPlaceHeightB.Value = 20;
            this.nudPlaceHeightB.DecimalPlaces = 1;

            y += 35;
            this.lblAreaX.Text = "A区X:";
            this.lblAreaX.Location = new System.Drawing.Point(10, y);
            this.lblAreaX.Size = new System.Drawing.Size(50, 20);
            this.nudAreaX = new System.Windows.Forms.NumericUpDown();
            this.nudAreaX.Location = new System.Drawing.Point(60, y);
            this.nudAreaX.Size = new System.Drawing.Size(60, 23);
            this.nudAreaX.Minimum = 0;
            this.nudAreaX.Maximum = 10000;
            this.nudAreaX.Value = 400;
            this.nudAreaX.DecimalPlaces = 1;

            this.lblAreaY.Text = "A区Y:";
            this.lblAreaY.Location = new System.Drawing.Point(130, y);
            this.lblAreaY.Size = new System.Drawing.Size(50, 20);
            this.nudAreaY = new System.Windows.Forms.NumericUpDown();
            this.nudAreaY.Location = new System.Drawing.Point(180, y);
            this.nudAreaY.Size = new System.Drawing.Size(60, 23);
            this.nudAreaY.Minimum = 0;
            this.nudAreaY.Maximum = 10000;
            this.nudAreaY.Value = 100;
            this.nudAreaY.DecimalPlaces = 1;

            this.lblAreaBX.Text = "B区X:";
            this.lblAreaBX.Location = new System.Drawing.Point(250, y);
            this.lblAreaBX.Size = new System.Drawing.Size(50, 20);
            this.nudAreaBX = new System.Windows.Forms.NumericUpDown();
            this.nudAreaBX.Location = new System.Drawing.Point(300, y);
            this.nudAreaBX.Size = new System.Drawing.Size(60, 23);
            this.nudAreaBX.Minimum = 0;
            this.nudAreaBX.Maximum = 10000;
            this.nudAreaBX.Value = 400;
            this.nudAreaBX.DecimalPlaces = 1;

            this.lblAreaBY.Text = "B区Y:";
            this.lblAreaBY.Location = new System.Drawing.Point(370, y);
            this.lblAreaBY.Size = new System.Drawing.Size(50, 20);
            this.nudAreaBY = new System.Windows.Forms.NumericUpDown();
            this.nudAreaBY.Location = new System.Drawing.Point(420, y);
            this.nudAreaBY.Size = new System.Drawing.Size(60, 23);
            this.nudAreaBY.Minimum = 0;
            this.nudAreaBY.Maximum = 10000;
            this.nudAreaBY.Value = 400;
            this.nudAreaBY.DecimalPlaces = 1;

            //
            // btnSave
            //
            this.btnSave.Text = "保存";
            this.btnSave.Location = new System.Drawing.Point(380, 470);
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            //
            // btnCancel
            //
            this.btnCancel.Text = "取消";
            this.btnCancel.Location = new System.Drawing.Point(470, 470);
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            //
            // btnReset
            //
            this.btnReset.Text = "恢复默认";
            this.btnReset.Location = new System.Drawing.Point(20, 470);
            this.btnReset.Size = new System.Drawing.Size(90, 30);
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);

            //
            // ConfigForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 520);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnReset);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "参数配置";
            this.tabControl.ResumeLayout(false);
            this.tabPageSystem.ResumeLayout(false);
            this.tabPageVision.ResumeLayout(false);
            this.tabPageMotion.ResumeLayout(false);
            this.grpSystem.ResumeLayout(false);
            this.grpSystem.PerformLayout();
            this.grpVisionCamera.ResumeLayout(false);
            this.grpVisionCamera.PerformLayout();
            this.grpVisionDetection.ResumeLayout(false);
            this.grpVisionDetection.PerformLayout();
            this.grpMotionSpeed.ResumeLayout(false);
            this.grpMotionSpeed.PerformLayout();
            this.grpMotionLimit.ResumeLayout(false);
            this.grpMotionLimit.PerformLayout();
            this.grpMotionPosition.ResumeLayout(false);
            this.grpMotionPosition.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCameraIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudExposure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFrameRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidthUpper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidthLower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeightUpper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeightLower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDefectSensitivity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPixelEquivalent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpeedX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpeedY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpeedZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAccelX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAccelY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAccelZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLimitXPos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLimitXNeg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLimitYPos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLimitYNeg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLimitZPos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLimitZNeg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrabHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPlaceHeightA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPlaceHeightB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAreaX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAreaY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAreaBX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAreaBY)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageSystem;
        private System.Windows.Forms.TabPage tabPageVision;
        private System.Windows.Forms.TabPage tabPageMotion;
        private System.Windows.Forms.GroupBox grpSystem;
        private System.Windows.Forms.CheckBox chkUseSimulatedCamera;
        private System.Windows.Forms.CheckBox chkUseSimulatedMotion;
        private System.Windows.Forms.Label lblCameraIndex;
        private System.Windows.Forms.NumericUpDown nudCameraIndex;
        private System.Windows.Forms.GroupBox grpVisionCamera;
        private System.Windows.Forms.Label lblExposure;
        private System.Windows.Forms.NumericUpDown nudExposure;
        private System.Windows.Forms.Label lblGain;
        private System.Windows.Forms.NumericUpDown nudGain;
        private System.Windows.Forms.Label lblFrameRate;
        private System.Windows.Forms.NumericUpDown nudFrameRate;
        private System.Windows.Forms.GroupBox grpVisionDetection;
        private System.Windows.Forms.Label lblWidthUpper;
        private System.Windows.Forms.NumericUpDown nudWidthUpper;
        private System.Windows.Forms.Label lblWidthLower;
        private System.Windows.Forms.NumericUpDown nudWidthLower;
        private System.Windows.Forms.Label lblHeightUpper;
        private System.Windows.Forms.NumericUpDown nudHeightUpper;
        private System.Windows.Forms.Label lblHeightLower;
        private System.Windows.Forms.NumericUpDown nudHeightLower;
        private System.Windows.Forms.Label lblDefectSensitivity;
        private System.Windows.Forms.NumericUpDown nudDefectSensitivity;
        private System.Windows.Forms.Label lblPixelEquivalent;
        private System.Windows.Forms.NumericUpDown nudPixelEquivalent;
        private System.Windows.Forms.GroupBox grpMotionSpeed;
        private System.Windows.Forms.Label lblSpeedX;
        private System.Windows.Forms.NumericUpDown nudSpeedX;
        private System.Windows.Forms.Label lblSpeedY;
        private System.Windows.Forms.NumericUpDown nudSpeedY;
        private System.Windows.Forms.Label lblSpeedZ;
        private System.Windows.Forms.NumericUpDown nudSpeedZ;
        private System.Windows.Forms.Label lblAccelX;
        private System.Windows.Forms.NumericUpDown nudAccelX;
        private System.Windows.Forms.Label lblAccelY;
        private System.Windows.Forms.NumericUpDown nudAccelY;
        private System.Windows.Forms.Label lblAccelZ;
        private System.Windows.Forms.NumericUpDown nudAccelZ;
        private System.Windows.Forms.GroupBox grpMotionLimit;
        private System.Windows.Forms.Label lblLimitXPos;
        private System.Windows.Forms.NumericUpDown nudLimitXPos;
        private System.Windows.Forms.Label lblLimitXNeg;
        private System.Windows.Forms.NumericUpDown nudLimitXNeg;
        private System.Windows.Forms.Label lblLimitYPos;
        private System.Windows.Forms.NumericUpDown nudLimitYPos;
        private System.Windows.Forms.Label lblLimitYNeg;
        private System.Windows.Forms.NumericUpDown nudLimitYNeg;
        private System.Windows.Forms.Label lblLimitZPos;
        private System.Windows.Forms.NumericUpDown nudLimitZPos;
        private System.Windows.Forms.Label lblLimitZNeg;
        private System.Windows.Forms.NumericUpDown nudLimitZNeg;
        private System.Windows.Forms.GroupBox grpMotionPosition;
        private System.Windows.Forms.Label lblGrabHeight;
        private System.Windows.Forms.NumericUpDown nudGrabHeight;
        private System.Windows.Forms.Label lblPlaceHeightA;
        private System.Windows.Forms.NumericUpDown nudPlaceHeightA;
        private System.Windows.Forms.Label lblPlaceHeightB;
        private System.Windows.Forms.NumericUpDown nudPlaceHeightB;
        private System.Windows.Forms.Label lblAreaX;
        private System.Windows.Forms.NumericUpDown nudAreaX;
        private System.Windows.Forms.Label lblAreaY;
        private System.Windows.Forms.NumericUpDown nudAreaY;
        private System.Windows.Forms.Label lblAreaBX;
        private System.Windows.Forms.NumericUpDown nudAreaBX;
        private System.Windows.Forms.Label lblAreaBY;
        private System.Windows.Forms.NumericUpDown nudAreaBY;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnReset;
    }
}
