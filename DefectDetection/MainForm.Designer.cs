namespace DefectDetection
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();

            // 主布局
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.文件FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出日志ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出统计ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.退出XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.参数配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助HToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();

            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageMain = new System.Windows.Forms.TabPage();
            this.tabPageManual = new System.Windows.Forms.TabPage();
            this.tabPageLog = new System.Windows.Forms.TabPage();

            // 主页面控件
            this.grpDeviceStatus = new System.Windows.Forms.GroupBox();
            this.lblDeviceStateLabel = new System.Windows.Forms.Label();
            this.lblDeviceState = new System.Windows.Forms.Label();
            this.lblCameraStatusLabel = new System.Windows.Forms.Label();
            this.lblCameraStatus = new System.Windows.Forms.Label();
            this.lblMotionStatusLabel = new System.Windows.Forms.Label();
            this.lblMotionStatus = new System.Windows.Forms.Label();

            this.grpImageDisplay = new System.Windows.Forms.GroupBox();
            this.picOriginalImage = new System.Windows.Forms.PictureBox();
            this.lblOriginalTitle = new System.Windows.Forms.Label();
            this.picProcessedImage = new System.Windows.Forms.PictureBox();
            this.lblProcessedTitle = new System.Windows.Forms.Label();

            this.grpDetectionResult = new System.Windows.Forms.GroupBox();
            this.lblResultLabel = new System.Windows.Forms.Label();
            this.lblResult = new System.Windows.Forms.Label();
            this.lblDefectTypeLabel = new System.Windows.Forms.Label();
            this.lblDefectType = new System.Windows.Forms.Label();
            this.lblDetectionTimeLabel = new System.Windows.Forms.Label();
            this.lblDetectionTime = new System.Windows.Forms.Label();
            this.lblPartSizeLabel = new System.Windows.Forms.Label();
            this.lblPartSize = new System.Windows.Forms.Label();
            this.lblPartPositionLabel = new System.Windows.Forms.Label();
            this.lblPartPosition = new System.Windows.Forms.Label();

            this.grpStatistics = new System.Windows.Forms.GroupBox();
            this.lblTotalCount = new System.Windows.Forms.Label();
            this.lblQualifiedCount = new System.Windows.Forms.Label();
            this.lblUnqualifiedCount = new System.Windows.Forms.Label();
            this.lblQualifiedRate = new System.Windows.Forms.Label();

            this.grpControl = new System.Windows.Forms.GroupBox();
            this.btnConnectCamera = new System.Windows.Forms.Button();
            this.btnDisconnectCamera = new System.Windows.Forms.Button();
            this.btnStartCapture = new System.Windows.Forms.Button();
            this.btnConnectMotion = new System.Windows.Forms.Button();
            this.btnDisconnectMotion = new System.Windows.Forms.Button();
            this.btnHome = new System.Windows.Forms.Button();
            this.btnSingleCycle = new System.Windows.Forms.Button();
            this.btnStartAuto = new System.Windows.Forms.Button();
            this.btnEmergencyStop = new System.Windows.Forms.Button();
            this.btnClearAlarm = new System.Windows.Forms.Button();

            this.lblStatusMessage = new System.Windows.Forms.Label();
            this.lblSortingStatusLabel = new System.Windows.Forms.Label();
            this.lblSortingStatus = new System.Windows.Forms.Label();
            this.lblCurrentStepLabel = new System.Windows.Forms.Label();
            this.lblCurrentStep = new System.Windows.Forms.Label();
            this.lblCycleCount = new System.Windows.Forms.Label();

            // 手动控制页面
            this.grpManualMotion = new System.Windows.Forms.GroupBox();
            this.lblManualPosX = new System.Windows.Forms.Label();
            this.lblPosX = new System.Windows.Forms.Label();
            this.lblManualPosY = new System.Windows.Forms.Label();
            this.lblPosY = new System.Windows.Forms.Label();
            this.lblManualPosZ = new System.Windows.Forms.Label();
            this.lblPosZ = new System.Windows.Forms.Label();

            // 日志页面
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.btnExportLog = new System.Windows.Forms.Button();
            this.btnExportStatistics = new System.Windows.Forms.Button();

            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageMain.SuspendLayout();
            this.tabPageManual.SuspendLayout();
            this.tabPageLog.SuspendLayout();
            this.grpDeviceStatus.SuspendLayout();
            this.grpImageDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picOriginalImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picProcessedImage)).BeginInit();
            this.grpDetectionResult.SuspendLayout();
            this.grpStatistics.SuspendLayout();
            this.grpControl.SuspendLayout();
            this.grpManualMotion.SuspendLayout();
            this.SuspendLayout();

            //
            // menuStrip
            //
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.文件FToolStripMenuItem,
                this.设置SToolStripMenuItem,
                this.帮助HToolStripMenuItem
            });
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1200, 25);
            this.menuStrip.TabIndex = 0;

            //
            // 文件FToolStripMenuItem
            //
            this.文件FToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.导出日志ToolStripMenuItem,
                this.导出统计ToolStripMenuItem,
                this.toolStripMenuItem1,
                this.退出XToolStripMenuItem
            });
            this.文件FToolStripMenuItem.Name = "文件FToolStripMenuItem";
            this.文件FToolStripMenuItem.Size = new System.Drawing.Size(58, 21);
            this.文件FToolStripMenuItem.Text = "文件(&F)";

            //
            // 导出日志ToolStripMenuItem
            //
            this.导出日志ToolStripMenuItem.Name = "导出日志ToolStripMenuItem";
            this.导出日志ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.导出日志ToolStripMenuItem.Text = "导出日志";
            this.导出日志ToolStripMenuItem.Click += new System.EventHandler(this.btnExportLog_Click);

            //
            // 导出统计ToolStripMenuItem
            //
            this.导出统计ToolStripMenuItem.Name = "导出统计ToolStripMenuItem";
            this.导出统计ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.导出统计ToolStripMenuItem.Text = "导出统计";
            this.导出统计ToolStripMenuItem.Click += new System.EventHandler(this.btnExportStatistics_Click);

            //
            // 退出XToolStripMenuItem
            //
            this.退出XToolStripMenuItem.Name = "退出XToolStripMenuItem";
            this.退出XToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.退出XToolStripMenuItem.Text = "退出(&X)";
            this.退出XToolStripMenuItem.Click += (s, e) => this.Close();

            //
            // 设置SToolStripMenuItem
            //
            this.设置SToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.参数配置ToolStripMenuItem
            });
            this.设置SToolStripMenuItem.Name = "设置SToolStripMenuItem";
            this.设置SToolStripMenuItem.Size = new System.Drawing.Size(59, 21);
            this.设置SToolStripMenuItem.Text = "设置(&S)";

            //
            // 参数配置ToolStripMenuItem
            //
            this.参数配置ToolStripMenuItem.Name = "参数配置ToolStripMenuItem";
            this.参数配置ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.参数配置ToolStripMenuItem.Text = "参数配置";
            this.参数配置ToolStripMenuItem.Click += new System.EventHandler(this.btnConfig_Click);

            //
            // 帮助HToolStripMenuItem
            //
            this.帮助HToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.关于AToolStripMenuItem
            });
            this.帮助HToolStripMenuItem.Name = "帮助HToolStripMenuItem";
            this.帮助HToolStripMenuItem.Size = new System.Drawing.Size(61, 21);
            this.帮助HToolStripMenuItem.Text = "帮助(&H)";

            //
            // 关于AToolStripMenuItem
            //
            this.关于AToolStripMenuItem.Name = "关于AToolStripMenuItem";
            this.关于AToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.关于AToolStripMenuItem.Text = "关于(&A)";
            this.关于AToolStripMenuItem.Click += (s, e) =>
                MessageBox.Show("零件视觉检测与分拣上位机 v1.0\n基于C# WinForms开发", "关于", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //
            // tabControlMain
            //
            this.tabControlMain.Controls.Add(this.tabPageMain);
            this.tabControlMain.Controls.Add(this.tabPageManual);
            this.tabControlMain.Controls.Add(this.tabPageLog);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 25);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1200, 700);
            this.tabControlMain.TabIndex = 1;

            //
            // tabPageMain
            //
            this.tabPageMain.Controls.Add(this.grpDeviceStatus);
            this.tabPageMain.Controls.Add(this.grpImageDisplay);
            this.tabPageMain.Controls.Add(this.grpDetectionResult);
            this.tabPageMain.Controls.Add(this.grpStatistics);
            this.tabPageMain.Controls.Add(this.grpControl);
            this.tabPageMain.Controls.Add(this.lblStatusMessage);
            this.tabPageMain.Controls.Add(this.lblSortingStatusLabel);
            this.tabPageMain.Controls.Add(this.lblSortingStatus);
            this.tabPageMain.Controls.Add(this.lblCurrentStepLabel);
            this.tabPageMain.Controls.Add(this.lblCurrentStep);
            this.tabPageMain.Controls.Add(this.lblCycleCount);
            this.tabPageMain.Location = new System.Drawing.Point(4, 22);
            this.tabPageMain.Name = "tabPageMain";
            this.tabPageMain.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMain.Size = new System.Drawing.Size(1192, 674);
            this.tabPageMain.TabIndex = 0;
            this.tabPageMain.Text = "主界面";
            this.tabPageMain.UseVisualStyleBackColor = true;

            //
            // grpDeviceStatus
            //
            this.grpDeviceStatus.Controls.Add(this.lblDeviceStateLabel);
            this.grpDeviceStatus.Controls.Add(this.lblDeviceState);
            this.grpDeviceStatus.Controls.Add(this.lblCameraStatusLabel);
            this.grpDeviceStatus.Controls.Add(this.lblCameraStatus);
            this.grpDeviceStatus.Controls.Add(this.lblMotionStatusLabel);
            this.grpDeviceStatus.Controls.Add(this.lblMotionStatus);
            this.grpDeviceStatus.Location = new System.Drawing.Point(10, 10);
            this.grpDeviceStatus.Name = "grpDeviceStatus";
            this.grpDeviceStatus.Size = new System.Drawing.Size(250, 120);
            this.grpDeviceStatus.TabIndex = 0;
            this.grpDeviceStatus.TabStop = false;
            this.grpDeviceStatus.Text = "设备状态";

            this.lblDeviceStateLabel.Text = "设备状态:";
            this.lblDeviceStateLabel.Location = new System.Drawing.Point(10, 25);
            this.lblDeviceStateLabel.Size = new System.Drawing.Size(70, 20);
            this.lblDeviceState = new System.Windows.Forms.Label();
            this.lblDeviceState.Text = "未连接";
            this.lblDeviceState.ForeColor = System.Drawing.Color.Gray;
            this.lblDeviceState.Location = new System.Drawing.Point(80, 25);
            this.lblDeviceState.Size = new System.Drawing.Size(150, 20);

            this.lblCameraStatusLabel.Text = "相机状态:";
            this.lblCameraStatusLabel.Location = new System.Drawing.Point(10, 50);
            this.lblCameraStatusLabel.Size = new System.Drawing.Size(70, 20);
            this.lblCameraStatus = new System.Windows.Forms.Label();
            this.lblCameraStatus.Text = "未连接";
            this.lblCameraStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblCameraStatus.Location = new System.Drawing.Point(80, 50);
            this.lblCameraStatus.Size = new System.Drawing.Size(150, 20);

            this.lblMotionStatusLabel.Text = "运动状态:";
            this.lblMotionStatusLabel.Location = new System.Drawing.Point(10, 75);
            this.lblMotionStatusLabel.Size = new System.Drawing.Size(70, 20);
            this.lblMotionStatus = new System.Windows.Forms.Label();
            this.lblMotionStatus.Text = "未连接";
            this.lblMotionStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblMotionStatus.Location = new System.Drawing.Point(80, 75);
            this.lblMotionStatus.Size = new System.Drawing.Size(150, 20);

            this.grpDeviceStatus.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblDeviceStateLabel, this.lblDeviceState,
                this.lblCameraStatusLabel, this.lblCameraStatus,
                this.lblMotionStatusLabel, this.lblMotionStatus
            });

            //
            // lblOriginalTitle
            //
            this.lblOriginalTitle.Text = "原始图像";
            this.lblOriginalTitle.Location = new System.Drawing.Point(10, 20);
            this.lblOriginalTitle.Size = new System.Drawing.Size(100, 20);
            this.lblOriginalTitle.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);

            //
            // picOriginalImage
            //
            this.picOriginalImage = new System.Windows.Forms.PictureBox();
            this.picOriginalImage.Location = new System.Drawing.Point(10, 45);
            this.picOriginalImage.Size = new System.Drawing.Size(310, 340);
            this.picOriginalImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picOriginalImage.BackColor = System.Drawing.Color.Black;
            this.picOriginalImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;

            //
            // lblProcessedTitle
            //
            this.lblProcessedTitle.Text = "处理后图像";
            this.lblProcessedTitle.Location = new System.Drawing.Point(330, 20);
            this.lblProcessedTitle.Size = new System.Drawing.Size(100, 20);
            this.lblProcessedTitle.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);

            //
            // picProcessedImage
            //
            this.picProcessedImage = new System.Windows.Forms.PictureBox();
            this.picProcessedImage.Location = new System.Drawing.Point(330, 45);
            this.picProcessedImage.Size = new System.Drawing.Size(310, 340);
            this.picProcessedImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picProcessedImage.BackColor = System.Drawing.Color.Black;
            this.picProcessedImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;

            //
            // grpImageDisplay
            //
            this.grpImageDisplay.Controls.Add(this.picOriginalImage);
            this.grpImageDisplay.Controls.Add(this.lblOriginalTitle);
            this.grpImageDisplay.Controls.Add(this.picProcessedImage);
            this.grpImageDisplay.Controls.Add(this.lblProcessedTitle);
            this.grpImageDisplay.Location = new System.Drawing.Point(270, 10);
            this.grpImageDisplay.Name = "grpImageDisplay";
            this.grpImageDisplay.Size = new System.Drawing.Size(650, 400);
            this.grpImageDisplay.TabIndex = 1;
            this.grpImageDisplay.TabStop = false;
            this.grpImageDisplay.Text = "图像显示";

            //
            // grpDetectionResult
            //
            this.grpDetectionResult.Controls.Add(this.lblResultLabel);
            this.grpDetectionResult.Controls.Add(this.lblResult);
            this.grpDetectionResult.Controls.Add(this.lblDefectTypeLabel);
            this.grpDetectionResult.Controls.Add(this.lblDefectType);
            this.grpDetectionResult.Controls.Add(this.lblDetectionTimeLabel);
            this.grpDetectionResult.Controls.Add(this.lblDetectionTime);
            this.grpDetectionResult.Controls.Add(this.lblPartSizeLabel);
            this.grpDetectionResult.Controls.Add(this.lblPartSize);
            this.grpDetectionResult.Controls.Add(this.lblPartPositionLabel);
            this.grpDetectionResult.Controls.Add(this.lblPartPosition);
            this.grpDetectionResult.Location = new System.Drawing.Point(930, 10);
            this.grpDetectionResult.Name = "grpDetectionResult";
            this.grpDetectionResult.Size = new System.Drawing.Size(250, 200);
            this.grpDetectionResult.TabIndex = 2;
            this.grpDetectionResult.TabStop = false;
            this.grpDetectionResult.Text = "检测结果";

            this.lblResultLabel.Text = "结果:";
            this.lblResultLabel.Location = new System.Drawing.Point(10, 25);
            this.lblResultLabel.Size = new System.Drawing.Size(60, 20);
            this.lblResult = new System.Windows.Forms.Label();
            this.lblResult.Text = "-";
            this.lblResult.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold);
            this.lblResult.Location = new System.Drawing.Point(70, 25);
            this.lblResult.Size = new System.Drawing.Size(170, 25);

            this.lblDefectTypeLabel.Text = "缺陷类型:";
            this.lblDefectTypeLabel.Location = new System.Drawing.Point(10, 60);
            this.lblDefectTypeLabel.Size = new System.Drawing.Size(70, 20);
            this.lblDefectType = new System.Windows.Forms.Label();
            this.lblDefectType.Text = "-";
            this.lblDefectType.Location = new System.Drawing.Point(80, 60);
            this.lblDefectType.Size = new System.Drawing.Size(160, 20);

            this.lblDetectionTimeLabel.Text = "检测耗时:";
            this.lblDetectionTimeLabel.Location = new System.Drawing.Point(10, 85);
            this.lblDetectionTimeLabel.Size = new System.Drawing.Size(70, 20);
            this.lblDetectionTime = new System.Windows.Forms.Label();
            this.lblDetectionTime.Text = "-";
            this.lblDetectionTime.Location = new System.Drawing.Point(80, 85);
            this.lblDetectionTime.Size = new System.Drawing.Size(160, 20);

            this.lblPartSizeLabel.Text = "零件尺寸:";
            this.lblPartSizeLabel.Location = new System.Drawing.Point(10, 110);
            this.lblPartSizeLabel.Size = new System.Drawing.Size(70, 20);
            this.lblPartSize = new System.Windows.Forms.Label();
            this.lblPartSize.Text = "-";
            this.lblPartSize.Location = new System.Drawing.Point(80, 110);
            this.lblPartSize.Size = new System.Drawing.Size(160, 20);

            this.lblPartPositionLabel.Text = "零件位置:";
            this.lblPartPositionLabel.Location = new System.Drawing.Point(10, 135);
            this.lblPartPositionLabel.Size = new System.Drawing.Size(70, 20);
            this.lblPartPosition = new System.Windows.Forms.Label();
            this.lblPartPosition.Text = "-";
            this.lblPartPosition.Location = new System.Drawing.Point(80, 135);
            this.lblPartPosition.Size = new System.Drawing.Size(160, 20);

            this.grpDetectionResult.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblResultLabel, this.lblResult,
                this.lblDefectTypeLabel, this.lblDefectType,
                this.lblDetectionTimeLabel, this.lblDetectionTime,
                this.lblPartSizeLabel, this.lblPartSize,
                this.lblPartPositionLabel, this.lblPartPosition
            });

            //
            // grpStatistics
            //
            this.grpStatistics.Controls.Add(this.lblTotalCount);
            this.grpStatistics.Controls.Add(this.lblQualifiedCount);
            this.grpStatistics.Controls.Add(this.lblUnqualifiedCount);
            this.grpStatistics.Controls.Add(this.lblQualifiedRate);
            this.grpStatistics.Location = new System.Drawing.Point(930, 220);
            this.grpStatistics.Name = "grpStatistics";
            this.grpStatistics.Size = new System.Drawing.Size(250, 120);
            this.grpStatistics.TabIndex = 3;
            this.grpStatistics.TabStop = false;
            this.grpStatistics.Text = "生产统计";

            this.lblTotalCount.Text = "总数: 0";
            this.lblTotalCount.Location = new System.Drawing.Point(10, 25);
            this.lblTotalCount.Size = new System.Drawing.Size(230, 20);
            this.lblTotalCount.Font = new System.Drawing.Font("Microsoft YaHei", 10F, System.Drawing.FontStyle.Bold);

            this.lblQualifiedCount.Text = "合格: 0";
            this.lblQualifiedCount.Location = new System.Drawing.Point(10, 50);
            this.lblQualifiedCount.Size = new System.Drawing.Size(110, 20);
            this.lblQualifiedCount.ForeColor = System.Drawing.Color.Green;

            this.lblUnqualifiedCount.Text = "不合格: 0";
            this.lblUnqualifiedCount.Location = new System.Drawing.Point(130, 50);
            this.lblUnqualifiedCount.Size = new System.Drawing.Size(110, 20);
            this.lblUnqualifiedCount.ForeColor = System.Drawing.Color.Red;

            this.lblQualifiedRate.Text = "合格率: 0.0%";
            this.lblQualifiedRate.Location = new System.Drawing.Point(10, 80);
            this.lblQualifiedRate.Size = new System.Drawing.Size(230, 25);
            this.lblQualifiedRate.Font = new System.Drawing.Font("Microsoft YaHei", 11F, System.Drawing.FontStyle.Bold);
            this.lblQualifiedRate.ForeColor = System.Drawing.Color.Blue;

            //
            // grpControl
            //
            this.grpControl.Controls.Add(this.btnConnectCamera);
            this.grpControl.Controls.Add(this.btnDisconnectCamera);
            this.grpControl.Controls.Add(this.btnStartCapture);
            this.grpControl.Controls.Add(this.btnConnectMotion);
            this.grpControl.Controls.Add(this.btnDisconnectMotion);
            this.grpControl.Controls.Add(this.btnHome);
            this.grpControl.Controls.Add(this.btnSingleCycle);
            this.grpControl.Controls.Add(this.btnStartAuto);
            this.grpControl.Controls.Add(this.btnEmergencyStop);
            this.grpControl.Controls.Add(this.btnClearAlarm);
            this.grpControl.Location = new System.Drawing.Point(10, 420);
            this.grpControl.Name = "grpControl";
            this.grpControl.Size = new System.Drawing.Size(910, 120);
            this.grpControl.TabIndex = 4;
            this.grpControl.TabStop = false;
            this.grpControl.Text = "控制面板";

            int btnX = 10;
            int btnY = 25;
            int btnWidth = 85;
            int btnHeight = 35;
            int btnGap = 5;

            this.btnConnectCamera.Text = "连接相机";
            this.btnConnectCamera.Location = new System.Drawing.Point(btnX, btnY);
            this.btnConnectCamera.Size = new System.Drawing.Size(btnWidth, btnHeight);
            this.btnConnectCamera.Click += new System.EventHandler(this.btnConnectCamera_Click);
            btnX += btnWidth + btnGap;

            this.btnDisconnectCamera.Text = "断开相机";
            this.btnDisconnectCamera.Location = new System.Drawing.Point(btnX, btnY);
            this.btnDisconnectCamera.Size = new System.Drawing.Size(btnWidth, btnHeight);
            this.btnDisconnectCamera.Click += new System.EventHandler(this.btnDisconnectCamera_Click);
            btnX += btnWidth + btnGap;

            this.btnStartCapture.Text = "开始采集";
            this.btnStartCapture.Location = new System.Drawing.Point(btnX, btnY);
            this.btnStartCapture.Size = new System.Drawing.Size(btnWidth, btnHeight);
            this.btnStartCapture.Click += new System.EventHandler(this.btnStartCapture_Click);
            btnX += btnWidth + btnGap;

            this.btnConnectMotion.Text = "连接运动";
            this.btnConnectMotion.Location = new System.Drawing.Point(btnX, btnY);
            this.btnConnectMotion.Size = new System.Drawing.Size(btnWidth, btnHeight);
            this.btnConnectMotion.Click += new System.EventHandler(this.btnConnectMotion_Click);
            btnX += btnWidth + btnGap;

            this.btnDisconnectMotion.Text = "断开运动";
            this.btnDisconnectMotion.Location = new System.Drawing.Point(btnX, btnY);
            this.btnDisconnectMotion.Size = new System.Drawing.Size(btnWidth, btnHeight);
            this.btnDisconnectMotion.Click += new System.EventHandler(this.btnDisconnectMotion_Click);
            btnX += btnWidth + btnGap;

            this.btnHome.Text = "回零";
            this.btnHome.Location = new System.Drawing.Point(btnX, btnY);
            this.btnHome.Size = new System.Drawing.Size(btnWidth, btnHeight);
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            btnX += btnWidth + btnGap;

            this.btnSingleCycle.Text = "单次流程";
            this.btnSingleCycle.Location = new System.Drawing.Point(btnX, btnY);
            this.btnSingleCycle.Size = new System.Drawing.Size(btnWidth, btnHeight);
            this.btnSingleCycle.Click += new System.EventHandler(this.btnSingleCycle_Click);
            btnX += btnWidth + btnGap;

            this.btnStartAuto.Text = "启动自动";
            this.btnStartAuto.Location = new System.Drawing.Point(btnX, btnY);
            this.btnStartAuto.Size = new System.Drawing.Size(btnWidth, btnHeight);
            this.btnStartAuto.Click += new System.EventHandler(this.btnStartAuto_Click);
            btnX += btnWidth + btnGap;

            this.btnEmergencyStop.Text = "急停";
            this.btnEmergencyStop.Location = new System.Drawing.Point(btnX, btnY);
            this.btnEmergencyStop.Size = new System.Drawing.Size(btnWidth, btnHeight);
            this.btnEmergencyStop.BackColor = System.Drawing.Color.Red;
            this.btnEmergencyStop.ForeColor = System.Drawing.Color.White;
            this.btnEmergencyStop.Click += new System.EventHandler(this.btnEmergencyStop_Click);
            btnX += btnWidth + btnGap;

            this.btnClearAlarm.Text = "清除报警";
            this.btnClearAlarm.Location = new System.Drawing.Point(btnX, btnY);
            this.btnClearAlarm.Size = new System.Drawing.Size(btnWidth, btnHeight);
            this.btnClearAlarm.Click += new System.EventHandler(this.btnClearAlarm_Click);

            //
            // 状态标签
            //
            this.lblStatusMessage.Text = "就绪";
            this.lblStatusMessage.Location = new System.Drawing.Point(10, 550);
            this.lblStatusMessage.Size = new System.Drawing.Size(400, 25);
            this.lblStatusMessage.Font = new System.Drawing.Font("Microsoft YaHei", 10F);

            this.lblSortingStatusLabel.Text = "分拣状态:";
            this.lblSortingStatusLabel.Location = new System.Drawing.Point(420, 550);
            this.lblSortingStatusLabel.Size = new System.Drawing.Size(70, 25);

            this.lblSortingStatus = new System.Windows.Forms.Label();
            this.lblSortingStatus.Text = "待机";
            this.lblSortingStatus.Location = new System.Drawing.Point(490, 550);
            this.lblSortingStatus.Size = new System.Drawing.Size(100, 25);

            this.lblCurrentStepLabel.Text = "当前步骤:";
            this.lblCurrentStepLabel.Location = new System.Drawing.Point(600, 550);
            this.lblCurrentStepLabel.Size = new System.Drawing.Size(70, 25);

            this.lblCurrentStep = new System.Windows.Forms.Label();
            this.lblCurrentStep.Text = "-";
            this.lblCurrentStep.Location = new System.Drawing.Point(670, 550);
            this.lblCurrentStep.Size = new System.Drawing.Size(200, 25);

            this.lblCycleCount.Text = "已完成: 0";
            this.lblCycleCount.Location = new System.Drawing.Point(880, 550);
            this.lblCycleCount.Size = new System.Drawing.Size(150, 25);
            this.lblCycleCount.Font = new System.Drawing.Font("Microsoft YaHei", 10F, System.Drawing.FontStyle.Bold);

            //
            // tabPageManual
            //
            this.tabPageManual.Controls.Add(this.grpManualMotion);
            this.tabPageManual.Location = new System.Drawing.Point(4, 22);
            this.tabPageManual.Name = "tabPageManual";
            this.tabPageManual.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageManual.Size = new System.Drawing.Size(1192, 674);
            this.tabPageManual.TabIndex = 1;
            this.tabPageManual.Text = "手动控制";
            this.tabPageManual.UseVisualStyleBackColor = true;

            //
            // grpManualMotion
            //
            this.grpManualMotion.Controls.Add(this.lblManualPosX);
            this.grpManualMotion.Controls.Add(this.lblPosX);
            this.grpManualMotion.Controls.Add(this.lblManualPosY);
            this.grpManualMotion.Controls.Add(this.lblPosY);
            this.grpManualMotion.Controls.Add(this.lblManualPosZ);
            this.grpManualMotion.Controls.Add(this.lblPosZ);
            this.grpManualMotion.Location = new System.Drawing.Point(10, 10);
            this.grpManualMotion.Name = "grpManualMotion";
            this.grpManualMotion.Size = new System.Drawing.Size(400, 150);
            this.grpManualMotion.TabIndex = 0;
            this.grpManualMotion.TabStop = false;
            this.grpManualMotion.Text = "当前位置";

            this.lblManualPosX.Text = "X轴:";
            this.lblManualPosX.Location = new System.Drawing.Point(10, 30);
            this.lblManualPosX.Size = new System.Drawing.Size(50, 20);
            this.lblPosX = new System.Windows.Forms.Label();
            this.lblPosX.Text = "X: 0.00 mm";
            this.lblPosX.Location = new System.Drawing.Point(60, 30);
            this.lblPosX.Size = new System.Drawing.Size(150, 20);
            this.lblPosX.Font = new System.Drawing.Font("Microsoft YaHei", 10F, System.Drawing.FontStyle.Bold);

            this.lblManualPosY.Text = "Y轴:";
            this.lblManualPosY.Location = new System.Drawing.Point(10, 60);
            this.lblManualPosY.Size = new System.Drawing.Size(50, 20);
            this.lblPosY = new System.Windows.Forms.Label();
            this.lblPosY.Text = "Y: 0.00 mm";
            this.lblPosY.Location = new System.Drawing.Point(60, 60);
            this.lblPosY.Size = new System.Drawing.Size(150, 20);
            this.lblPosY.Font = new System.Drawing.Font("Microsoft YaHei", 10F, System.Drawing.FontStyle.Bold);

            this.lblManualPosZ.Text = "Z轴:";
            this.lblManualPosZ.Location = new System.Drawing.Point(10, 90);
            this.lblManualPosZ.Size = new System.Drawing.Size(50, 20);
            this.lblPosZ = new System.Windows.Forms.Label();
            this.lblPosZ.Text = "Z: 0.00 mm";
            this.lblPosZ.Location = new System.Drawing.Point(60, 90);
            this.lblPosZ.Size = new System.Drawing.Size(150, 20);
            this.lblPosZ.Font = new System.Drawing.Font("Microsoft YaHei", 10F, System.Drawing.FontStyle.Bold);

            this.grpManualMotion.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblManualPosX, this.lblPosX,
                this.lblManualPosY, this.lblPosY,
                this.lblManualPosZ, this.lblPosZ
            });

            //
            // tabPageLog
            //
            this.tabPageLog.Controls.Add(this.txtLog);
            this.tabPageLog.Controls.Add(this.btnExportLog);
            this.tabPageLog.Controls.Add(this.btnExportStatistics);
            this.tabPageLog.Location = new System.Drawing.Point(4, 22);
            this.tabPageLog.Name = "tabPageLog";
            this.tabPageLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLog.Size = new System.Drawing.Size(1192, 674);
            this.tabPageLog.TabIndex = 2;
            this.tabPageLog.Text = "日志";
            this.tabPageLog.UseVisualStyleBackColor = true;

            //
            // txtLog
            //
            this.txtLog.Location = new System.Drawing.Point(10, 10);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(1170, 600);
            this.txtLog.TabIndex = 0;
            this.txtLog.Text = "";
            this.txtLog.BackColor = System.Drawing.Color.White;
            this.txtLog.ReadOnly = true;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);

            //
            // btnExportLog
            //
            this.btnExportLog.Text = "导出日志";
            this.btnExportLog.Location = new System.Drawing.Point(10, 620);
            this.btnExportLog.Size = new System.Drawing.Size(100, 35);
            this.btnExportLog.Click += new System.EventHandler(this.btnExportLog_Click);

            //
            // btnExportStatistics
            //
            this.btnExportStatistics.Text = "导出统计";
            this.btnExportStatistics.Location = new System.Drawing.Point(120, 620);
            this.btnExportStatistics.Size = new System.Drawing.Size(100, 35);
            this.btnExportStatistics.Click += new System.EventHandler(this.btnExportStatistics_Click);

            //
            // statusStrip
            //
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.toolStripStatusLabel1
            });
            this.statusStrip.Location = new System.Drawing.Point(0, 725);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1200, 22);
            this.statusStrip.TabIndex = 2;

            //
            // toolStripStatusLabel1
            //
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(131, 17);
            this.toolStripStatusLabel1.Text = "零件视觉检测与分拣上位机";

            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 750);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "零件视觉检测与分拣上位机 v1.0";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.tabControlMain.ResumeLayout(false);
            this.tabPageMain.ResumeLayout(false);
            this.tabPageMain.PerformLayout();
            this.tabPageManual.ResumeLayout(false);
            this.tabPageLog.ResumeLayout(false);
            this.grpDeviceStatus.ResumeLayout(false);
            this.grpImageDisplay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picOriginalImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picProcessedImage)).EndInit();
            this.grpDetectionResult.ResumeLayout(false);
            this.grpStatistics.ResumeLayout(false);
            this.grpControl.ResumeLayout(false);
            this.grpManualMotion.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem 文件FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导出日志ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导出统计ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 退出XToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 参数配置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助HToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于AToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageMain;
        private System.Windows.Forms.TabPage tabPageManual;
        private System.Windows.Forms.TabPage tabPageLog;
        private System.Windows.Forms.GroupBox grpDeviceStatus;
        private System.Windows.Forms.Label lblDeviceStateLabel;
        private System.Windows.Forms.Label lblDeviceState;
        private System.Windows.Forms.Label lblCameraStatusLabel;
        private System.Windows.Forms.Label lblCameraStatus;
        private System.Windows.Forms.Label lblMotionStatusLabel;
        private System.Windows.Forms.Label lblMotionStatus;
        private System.Windows.Forms.GroupBox grpImageDisplay;
        private System.Windows.Forms.PictureBox picOriginalImage;
        private System.Windows.Forms.Label lblOriginalTitle;
        private System.Windows.Forms.PictureBox picProcessedImage;
        private System.Windows.Forms.Label lblProcessedTitle;
        private System.Windows.Forms.GroupBox grpDetectionResult;
        private System.Windows.Forms.Label lblResultLabel;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Label lblDefectTypeLabel;
        private System.Windows.Forms.Label lblDefectType;
        private System.Windows.Forms.Label lblDetectionTimeLabel;
        private System.Windows.Forms.Label lblDetectionTime;
        private System.Windows.Forms.Label lblPartSizeLabel;
        private System.Windows.Forms.Label lblPartSize;
        private System.Windows.Forms.Label lblPartPositionLabel;
        private System.Windows.Forms.Label lblPartPosition;
        private System.Windows.Forms.GroupBox grpStatistics;
        private System.Windows.Forms.Label lblTotalCount;
        private System.Windows.Forms.Label lblQualifiedCount;
        private System.Windows.Forms.Label lblUnqualifiedCount;
        private System.Windows.Forms.Label lblQualifiedRate;
        private System.Windows.Forms.GroupBox grpControl;
        private System.Windows.Forms.Button btnConnectCamera;
        private System.Windows.Forms.Button btnDisconnectCamera;
        private System.Windows.Forms.Button btnStartCapture;
        private System.Windows.Forms.Button btnConnectMotion;
        private System.Windows.Forms.Button btnDisconnectMotion;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button btnSingleCycle;
        private System.Windows.Forms.Button btnStartAuto;
        private System.Windows.Forms.Button btnEmergencyStop;
        private System.Windows.Forms.Button btnClearAlarm;
        private System.Windows.Forms.Label lblStatusMessage;
        private System.Windows.Forms.Label lblSortingStatusLabel;
        private System.Windows.Forms.Label lblSortingStatus;
        private System.Windows.Forms.Label lblCurrentStepLabel;
        private System.Windows.Forms.Label lblCurrentStep;
        private System.Windows.Forms.Label lblCycleCount;
        private System.Windows.Forms.GroupBox grpManualMotion;
        private System.Windows.Forms.Label lblManualPosX;
        private System.Windows.Forms.Label lblPosX;
        private System.Windows.Forms.Label lblManualPosY;
        private System.Windows.Forms.Label lblPosY;
        private System.Windows.Forms.Label lblManualPosZ;
        private System.Windows.Forms.Label lblPosZ;
        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.Button btnExportLog;
        private System.Windows.Forms.Button btnExportStatistics;
        private System.Windows.Forms.Button btnConfig;
    }
}
