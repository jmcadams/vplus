namespace VixenPlus {
    using System.Windows.Forms;
    using System.ComponentModel;

    internal partial class PreferencesDialog {
        private IContainer components;

        #region Windows Form Designer generated code

        private TabPage backgroundItemsTab;
        private Button buttonCancel;
        private Button buttonCreateProfile;
        private Button buttonEngine;
        private Button buttonLogFilePath;
        private Button buttonOK;
        private Button buttonPluginSetup;
        private CheckBox checkBoxAutoScrolling;
        private CheckBox checkBoxClearAtEndOfSequence;
        private CheckBox checkBoxDisableAutoUpdate;
        private CheckBox checkBoxEnableBackgroundMusic;
        private CheckBox checkBoxEnableBackgroundSequence;
        private CheckBox checkBoxEnableMusicFade;
        private CheckBox checkBoxEventSequenceAutoSize;
        private CheckBox checkBoxFlipMouseScroll;
        private CheckBox checkBoxLogManual;
        private CheckBox checkBoxLogMusicPlayer;
        private CheckBox checkBoxLogScheduled;
        private CheckBox checkBoxResetAtStartup;
        private CheckBox checkBoxSavePlugInDialogPositions;
        private CheckBox checkBoxSaveZoomLevels;
        private CheckBox checkBoxShowNaturalChannelNumber;
        private CheckBox checkBoxShowPositionMarker;
        private CheckBox checkBoxShowSaveConfirmation;
        private CheckBox checkBoxUseDefaultPlugInData;
        private CheckBox checkBoxWizardForNewSequences;
        private ComboBox comboBoxAsyncProfile;
        private ComboBox comboBoxDefaultAudioDevice;
        private ComboBox comboBoxDefaultProfile;
        private ComboBox comboBoxSequenceType;
        private ComboBox comboBoxSyncProfile;
        private DateTimePicker dateTimePickerAutoShutdownTime;
        private TabPage engineTab;
        private FolderBrowserDialog folderBrowserDialog;
        private TabPage generalTab;
        private GroupBox groupBox1;
        private GroupBox groupBox10;
        private GroupBox groupBox11;
        private GroupBox groupBox12;
        private GroupBox groupBox13;
        private GroupBox groupBox14;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private GroupBox groupBox6;
        private GroupBox groupBox7;
        private GroupBox groupBox8;
        private GroupBox groupBox9;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label16;
        private Label label17;
        private Label label18;
        private Label label19;
        private Label label2;
        private Label label20;
        private Label label21;
        private Label label22;
        private Label label23;
        private Label label24;
        private Label label25;
        private Label label26;
        private Label label27;
        private Label label28;
        private Label label29;
        private Label label3;
        private Label label30;
        private Label label31;
        private Label label32;
        private Label label33;
        private Label label34;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label labelAutoShutdownTime;
        private TabPage newSequenceSettingsTab;
        private NumericUpDown numericUpDownHistoryImages;
        private NumericUpDown numericUpDownMaximumLevel;
        private NumericUpDown numericUpDownMinimumLevel;
        private OpenFileDialog openFileDialog;
        private RadioButton radioButtonAsyncDefaultProfileData;
        private RadioButton radioButtonAsyncProfileData;
        private RadioButton radioButtonAsyncSyncObject;
        private RadioButton radioButtonSyncDefaultProfileData;
        private RadioButton radioButtonSyncEmbeddedData;
        private RadioButton radioButtonSyncProfileData;
        private TabPage remoteExecutionTab;
        private TabPage sequenceEditingTab;
        private TabPage sequenceExecutionTab;
        private TabControl tabControl;
        private TextBox textBoxBackgroundMusicDelay;
        private TextBox textBoxBackgroundSequenceDelay;
        private TextBox textBoxClientName;
        private TextBox textBoxCurveLibraryFileName;
        private TextBox textBoxCurveLibraryFtpPassword;
        private TextBox textBoxCurveLibraryFtpUrl;
        private TextBox textBoxCurveLibraryFtpUserName;
        private TextBox textBoxCurveLibraryHttpUrl;
        private TextBox textBoxDefaultChannelCount;
        private TextBox textBoxDefaultSequenceSaveDirectory;
        private TextBox textBoxEngine;
        private TextBox textBoxEventPeriod;
        private TextBox textBoxIntensityLargeDelta;
        private TextBox textBoxLogFilePath;
        private TextBox textBoxMaxColumnWidth;
        private TextBox textBoxMaxRowHeight;
        private TextBox textBoxMouseWheelHorizontal;
        private TextBox textBoxMouseWheelVertical;
        private TextBox textBoxMusicFadeDuration;
        private TextBox textBoxTimerCheckFrequency;
        private ToolTip toolTip;
        private TreeView treeView;


        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("General");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Screen and Colors");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("New Sequence Settings");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Sequence Editing");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Sequence Execution");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Background Items");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Remote Execution");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Engine");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Advanced", new System.Windows.Forms.TreeNode[] {
            treeNode7,
            treeNode8});
            this.treeView = new System.Windows.Forms.TreeView();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.btnSetDataFolder = new System.Windows.Forms.Button();
            this.tabControl = new VixenPlus.TabControl(this.components);
            this.generalTab = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbToolbarAutoSave = new System.Windows.Forms.CheckBox();
            this.label36 = new System.Windows.Forms.Label();
            this.nudRecentFiles = new System.Windows.Forms.NumericUpDown();
            this.label35 = new System.Windows.Forms.Label();
            this.numericUpDownHistoryImages = new System.Windows.Forms.NumericUpDown();
            this.label28 = new System.Windows.Forms.Label();
            this.checkBoxDisableAutoUpdate = new System.Windows.Forms.CheckBox();
            this.label26 = new System.Windows.Forms.Label();
            this.dateTimePickerAutoShutdownTime = new System.Windows.Forms.DateTimePicker();
            this.labelAutoShutdownTime = new System.Windows.Forms.Label();
            this.comboBoxSequenceType = new System.Windows.Forms.ComboBox();
            this.label23 = new System.Windows.Forms.Label();
            this.checkBoxResetAtStartup = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.textBoxClientName = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxMouseWheelHorizontal = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxMouseWheelVertical = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxTimerCheckFrequency = new System.Windows.Forms.TextBox();
            this.screenTab = new System.Windows.Forms.TabPage();
            this.cbUseCheckmark = new System.Windows.Forms.CheckBox();
            this.gbColors = new System.Windows.Forms.GroupBox();
            this.lblClickToUpdate = new System.Windows.Forms.Label();
            this.lblCurrentColor = new System.Windows.Forms.Label();
            this.pbColor = new System.Windows.Forms.PictureBox();
            this.lbColorizableItems = new System.Windows.Forms.ListBox();
            this.lblPrimaryScreen = new System.Windows.Forms.Label();
            this.cbScreens = new System.Windows.Forms.ComboBox();
            this.newSequenceSettingsTab = new System.Windows.Forms.TabPage();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.comboBoxDefaultAudioDevice = new System.Windows.Forms.ComboBox();
            this.label27 = new System.Windows.Forms.Label();
            this.checkBoxUseDefaultPlugInData = new System.Windows.Forms.CheckBox();
            this.buttonPluginSetup = new System.Windows.Forms.Button();
            this.textBoxDefaultChannelCount = new System.Windows.Forms.TextBox();
            this.comboBoxDefaultProfile = new System.Windows.Forms.ComboBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.buttonCreateProfile = new System.Windows.Forms.Button();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.numericUpDownMaximumLevel = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.numericUpDownMinimumLevel = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.textBoxEventPeriod = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.checkBoxWizardForNewSequences = new System.Windows.Forms.CheckBox();
            this.sequenceEditingTab = new System.Windows.Forms.TabPage();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.textBoxCurveLibraryFtpPassword = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.textBoxCurveLibraryFtpUserName = new System.Windows.Forms.TextBox();
            this.label32 = new System.Windows.Forms.Label();
            this.textBoxCurveLibraryFileName = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.textBoxCurveLibraryFtpUrl = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.textBoxCurveLibraryHttpUrl = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cbWavefromZeroLine = new System.Windows.Forms.CheckBox();
            this.textBoxDefaultSequenceSaveDirectory = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.checkBoxFlipMouseScroll = new System.Windows.Forms.CheckBox();
            this.checkBoxShowNaturalChannelNumber = new System.Windows.Forms.CheckBox();
            this.textBoxIntensityLargeDelta = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxShowSaveConfirmation = new System.Windows.Forms.CheckBox();
            this.checkBoxSaveZoomLevels = new System.Windows.Forms.CheckBox();
            this.checkBoxEventSequenceAutoSize = new System.Windows.Forms.CheckBox();
            this.textBoxMaxRowHeight = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxMaxColumnWidth = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.sequenceExecutionTab = new System.Windows.Forms.TabPage();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.textBoxLogFilePath = new System.Windows.Forms.TextBox();
            this.buttonLogFilePath = new System.Windows.Forms.Button();
            this.checkBoxLogMusicPlayer = new System.Windows.Forms.CheckBox();
            this.checkBoxLogScheduled = new System.Windows.Forms.CheckBox();
            this.checkBoxLogManual = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBoxClearAtEndOfSequence = new System.Windows.Forms.CheckBox();
            this.checkBoxShowPositionMarker = new System.Windows.Forms.CheckBox();
            this.checkBoxSavePlugInDialogPositions = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoScrolling = new System.Windows.Forms.CheckBox();
            this.backgroundItemsTab = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label20 = new System.Windows.Forms.Label();
            this.textBoxMusicFadeDuration = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.checkBoxEnableMusicFade = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxBackgroundMusicDelay = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.checkBoxEnableBackgroundMusic = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxBackgroundSequenceDelay = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.checkBoxEnableBackgroundSequence = new System.Windows.Forms.CheckBox();
            this.remoteExecutionTab = new System.Windows.Forms.TabPage();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.radioButtonAsyncSyncObject = new System.Windows.Forms.RadioButton();
            this.comboBoxAsyncProfile = new System.Windows.Forms.ComboBox();
            this.radioButtonAsyncDefaultProfileData = new System.Windows.Forms.RadioButton();
            this.radioButtonAsyncProfileData = new System.Windows.Forms.RadioButton();
            this.label25 = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.comboBoxSyncProfile = new System.Windows.Forms.ComboBox();
            this.radioButtonSyncDefaultProfileData = new System.Windows.Forms.RadioButton();
            this.radioButtonSyncProfileData = new System.Windows.Forms.RadioButton();
            this.radioButtonSyncEmbeddedData = new System.Windows.Forms.RadioButton();
            this.label24 = new System.Windows.Forms.Label();
            this.engineTab = new System.Windows.Forms.TabPage();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.buttonEngine = new System.Windows.Forms.Button();
            this.textBoxEngine = new System.Windows.Forms.TextBox();
            this.tabControl.SuspendLayout();
            this.generalTab.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecentFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHistoryImages)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.screenTab.SuspendLayout();
            this.gbColors.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbColor)).BeginInit();
            this.newSequenceSettingsTab.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaximumLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinimumLevel)).BeginInit();
            this.sequenceEditingTab.SuspendLayout();
            this.groupBox14.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.sequenceExecutionTab.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.backgroundItemsTab.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.remoteExecutionTab.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.engineTab.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView.Location = new System.Drawing.Point(12, 12);
            this.treeView.Name = "treeView";
            treeNode1.Name = "nodeGeneral";
            treeNode1.Text = "General";
            treeNode2.Name = "nodeScreen";
            treeNode2.Text = "Screen and Colors";
            treeNode3.Name = "nodeNewSequenceSettings";
            treeNode3.Text = "New Sequence Settings";
            treeNode4.Name = "nodeSequenceEditing";
            treeNode4.Text = "Sequence Editing";
            treeNode5.Name = "nodeSequenceExecution";
            treeNode5.Text = "Sequence Execution";
            treeNode6.Name = "nodeBackgroundItems";
            treeNode6.Text = "Background Items";
            treeNode7.Name = "nodeRemoteExecution";
            treeNode7.Text = "Remote Execution";
            treeNode8.Name = "nodeEngine";
            treeNode8.Text = "Engine";
            treeNode9.Name = "nodeAdvanced";
            treeNode9.Text = "Advanced";
            this.treeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode9});
            this.treeView.Size = new System.Drawing.Size(161, 410);
            this.treeView.TabIndex = 0;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(469, 445);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(550, 445);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.SupportMultiDottedExtensions = true;
            // 
            // btnSetDataFolder
            // 
            this.btnSetDataFolder.Location = new System.Drawing.Point(12, 445);
            this.btnSetDataFolder.Name = "btnSetDataFolder";
            this.btnSetDataFolder.Size = new System.Drawing.Size(161, 23);
            this.btnSetDataFolder.TabIndex = 5;
            this.btnSetDataFolder.Text = "Set Data Folder";
            this.btnSetDataFolder.UseVisualStyleBackColor = true;
            this.btnSetDataFolder.Click += new System.EventHandler(this.btnSetDataFolder_Click);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.generalTab);
            this.tabControl.Controls.Add(this.screenTab);
            this.tabControl.Controls.Add(this.newSequenceSettingsTab);
            this.tabControl.Controls.Add(this.sequenceEditingTab);
            this.tabControl.Controls.Add(this.sequenceExecutionTab);
            this.tabControl.Controls.Add(this.backgroundItemsTab);
            this.tabControl.Controls.Add(this.remoteExecutionTab);
            this.tabControl.Controls.Add(this.engineTab);
            this.tabControl.HideTabs = true;
            this.tabControl.Location = new System.Drawing.Point(179, 12);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.OurMultiline = true;
            this.tabControl.SelectedIndex = 0;
            this.tabControl.ShowToolTips = true;
            this.tabControl.Size = new System.Drawing.Size(446, 413);
            this.tabControl.TabIndex = 1;
            this.tabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl_Selected);
            this.tabControl.Deselecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl_Deselecting);
            // 
            // generalTab
            // 
            this.generalTab.BackColor = System.Drawing.Color.Transparent;
            this.generalTab.Controls.Add(this.groupBox3);
            this.generalTab.Controls.Add(this.groupBox2);
            this.generalTab.Controls.Add(this.groupBox1);
            this.generalTab.Location = new System.Drawing.Point(0, 0);
            this.generalTab.Name = "generalTab";
            this.generalTab.Size = new System.Drawing.Size(446, 413);
            this.generalTab.TabIndex = 3;
            this.generalTab.Text = "generalTab";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbToolbarAutoSave);
            this.groupBox3.Controls.Add(this.label36);
            this.groupBox3.Controls.Add(this.nudRecentFiles);
            this.groupBox3.Controls.Add(this.label35);
            this.groupBox3.Controls.Add(this.numericUpDownHistoryImages);
            this.groupBox3.Controls.Add(this.label28);
            this.groupBox3.Controls.Add(this.checkBoxDisableAutoUpdate);
            this.groupBox3.Controls.Add(this.label26);
            this.groupBox3.Controls.Add(this.dateTimePickerAutoShutdownTime);
            this.groupBox3.Controls.Add(this.labelAutoShutdownTime);
            this.groupBox3.Controls.Add(this.comboBoxSequenceType);
            this.groupBox3.Controls.Add(this.label23);
            this.groupBox3.Controls.Add(this.checkBoxResetAtStartup);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.textBoxClientName);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Location = new System.Drawing.Point(3, 108);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(440, 242);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Miscellaneous";
            // 
            // cbToolbarAutoSave
            // 
            this.cbToolbarAutoSave.AutoSize = true;
            this.cbToolbarAutoSave.Location = new System.Drawing.Point(179, 170);
            this.cbToolbarAutoSave.Name = "cbToolbarAutoSave";
            this.cbToolbarAutoSave.Size = new System.Drawing.Size(15, 14);
            this.cbToolbarAutoSave.TabIndex = 79;
            this.toolTip.SetToolTip(this.cbToolbarAutoSave, "Sends a blank event to the plugins in the default plugin setup.\r\nUseful for paral" +
                    "lel port-based controllers.  Does not affect every\r\ncontroller type.");
            this.cbToolbarAutoSave.UseVisualStyleBackColor = true;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(17, 170);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(95, 13);
            this.label36.TabIndex = 78;
            this.label36.Text = "Auto save toolbars";
            this.toolTip.SetToolTip(this.label36, "Sends a blank event to the plugins in the default plugin setup.\r\nUseful for paral" +
                    "lel port-based controllers.  Does not affect every\r\ncontroller type.");
            // 
            // nudRecentFiles
            // 
            this.nudRecentFiles.Location = new System.Drawing.Point(179, 144);
            this.nudRecentFiles.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudRecentFiles.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudRecentFiles.Name = "nudRecentFiles";
            this.nudRecentFiles.Size = new System.Drawing.Size(117, 20);
            this.nudRecentFiles.TabIndex = 77;
            this.nudRecentFiles.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(17, 146);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(91, 13);
            this.label35.TabIndex = 76;
            this.label35.Text = "Recent file history";
            // 
            // numericUpDownHistoryImages
            // 
            this.numericUpDownHistoryImages.Location = new System.Drawing.Point(179, 118);
            this.numericUpDownHistoryImages.Name = "numericUpDownHistoryImages";
            this.numericUpDownHistoryImages.Size = new System.Drawing.Size(117, 20);
            this.numericUpDownHistoryImages.TabIndex = 75;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(17, 120);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(113, 13);
            this.label28.TabIndex = 74;
            this.label28.Text = "Backup history images";
            // 
            // checkBoxDisableAutoUpdate
            // 
            this.checkBoxDisableAutoUpdate.AutoSize = true;
            this.checkBoxDisableAutoUpdate.Location = new System.Drawing.Point(179, 98);
            this.checkBoxDisableAutoUpdate.Name = "checkBoxDisableAutoUpdate";
            this.checkBoxDisableAutoUpdate.Size = new System.Drawing.Size(15, 14);
            this.checkBoxDisableAutoUpdate.TabIndex = 73;
            this.toolTip.SetToolTip(this.checkBoxDisableAutoUpdate, "Stops the application from trying to update itself over the internet.");
            this.checkBoxDisableAutoUpdate.UseVisualStyleBackColor = true;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(17, 99);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(102, 13);
            this.label26.TabIndex = 72;
            this.label26.Text = "Disable auto-update";
            this.toolTip.SetToolTip(this.label26, "Stops the application from trying to update itself over the internet.");
            // 
            // dateTimePickerAutoShutdownTime
            // 
            this.dateTimePickerAutoShutdownTime.Checked = false;
            this.dateTimePickerAutoShutdownTime.CustomFormat = "  hh:mm tt";
            this.dateTimePickerAutoShutdownTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerAutoShutdownTime.Location = new System.Drawing.Point(179, 72);
            this.dateTimePickerAutoShutdownTime.Name = "dateTimePickerAutoShutdownTime";
            this.dateTimePickerAutoShutdownTime.ShowCheckBox = true;
            this.dateTimePickerAutoShutdownTime.ShowUpDown = true;
            this.dateTimePickerAutoShutdownTime.Size = new System.Drawing.Size(117, 20);
            this.dateTimePickerAutoShutdownTime.TabIndex = 71;
            this.toolTip.SetToolTip(this.dateTimePickerAutoShutdownTime, "If the application is running, it can shut down your computer at a time you speci" +
                    "fy");
            this.dateTimePickerAutoShutdownTime.Value = new System.DateTime(2007, 4, 20, 12, 0, 0, 0);
            // 
            // labelAutoShutdownTime
            // 
            this.labelAutoShutdownTime.AutoSize = true;
            this.labelAutoShutdownTime.Location = new System.Drawing.Point(17, 78);
            this.labelAutoShutdownTime.Name = "labelAutoShutdownTime";
            this.labelAutoShutdownTime.Size = new System.Drawing.Size(78, 13);
            this.labelAutoShutdownTime.TabIndex = 69;
            this.labelAutoShutdownTime.Text = "Auto shutdown";
            this.toolTip.SetToolTip(this.labelAutoShutdownTime, "If the application is running, it can shut down your computer at a time you speci" +
                    "fy");
            // 
            // comboBoxSequenceType
            // 
            this.comboBoxSequenceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSequenceType.FormattingEnabled = true;
            this.comboBoxSequenceType.Location = new System.Drawing.Point(179, 45);
            this.comboBoxSequenceType.Name = "comboBoxSequenceType";
            this.comboBoxSequenceType.Size = new System.Drawing.Size(255, 21);
            this.comboBoxSequenceType.TabIndex = 6;
            this.toolTip.SetToolTip(this.comboBoxSequenceType, "The sequence type you want to initially display when opening a sequence");
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(17, 48);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(126, 13);
            this.label23.TabIndex = 5;
            this.label23.Text = "Preferred sequence  type";
            this.toolTip.SetToolTip(this.label23, "The sequence type you want to initially display when opening a sequence");
            // 
            // checkBoxResetAtStartup
            // 
            this.checkBoxResetAtStartup.AutoSize = true;
            this.checkBoxResetAtStartup.Enabled = false;
            this.checkBoxResetAtStartup.Location = new System.Drawing.Point(179, 190);
            this.checkBoxResetAtStartup.Name = "checkBoxResetAtStartup";
            this.checkBoxResetAtStartup.Size = new System.Drawing.Size(15, 14);
            this.checkBoxResetAtStartup.TabIndex = 68;
            this.toolTip.SetToolTip(this.checkBoxResetAtStartup, "Sends a blank event to the plugins in the default plugin setup.\r\nUseful for paral" +
                    "lel port-based controllers.  Does not affect every\r\ncontroller type.");
            this.checkBoxResetAtStartup.UseVisualStyleBackColor = true;
            this.checkBoxResetAtStartup.Visible = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Enabled = false;
            this.label16.Location = new System.Drawing.Point(17, 190);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(134, 39);
            this.label16.TabIndex = 7;
            this.label16.Text = "Reset controller at startup\r\n(Requires default profiles in\r\nNew Sequence Settings" +
                ")";
            this.toolTip.SetToolTip(this.label16, "Sends a blank event to the plugins in the default plugin setup.\r\nUseful for paral" +
                    "lel port-based controllers.  Does not affect every\r\ncontroller type.");
            this.label16.Visible = false;
            // 
            // textBoxClientName
            // 
            this.textBoxClientName.Location = new System.Drawing.Point(179, 19);
            this.textBoxClientName.Name = "textBoxClientName";
            this.textBoxClientName.Size = new System.Drawing.Size(255, 20);
            this.textBoxClientName.TabIndex = 4;
            this.toolTip.SetToolTip(this.textBoxClientName, "Name used to identify this installation to remote clients and servers");
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(17, 22);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(62, 13);
            this.label15.TabIndex = 3;
            this.label15.Text = "Client name";
            this.toolTip.SetToolTip(this.label15, "Name used to identify this installation to remote clients and servers");
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxMouseWheelHorizontal);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBoxMouseWheelVertical);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(3, 57);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(440, 45);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mouse Wheel";
            this.toolTip.SetToolTip(this.groupBox2, "Mouse wheel increments");
            // 
            // textBoxMouseWheelHorizontal
            // 
            this.textBoxMouseWheelHorizontal.Location = new System.Drawing.Point(296, 13);
            this.textBoxMouseWheelHorizontal.Name = "textBoxMouseWheelHorizontal";
            this.textBoxMouseWheelHorizontal.Size = new System.Drawing.Size(50, 20);
            this.textBoxMouseWheelHorizontal.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(212, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Event increment";
            this.toolTip.SetToolTip(this.label4, "How many event periods a single mouse wheel movement scrolls through");
            // 
            // textBoxMouseWheelVertical
            // 
            this.textBoxMouseWheelVertical.Location = new System.Drawing.Point(118, 13);
            this.textBoxMouseWheelVertical.Name = "textBoxMouseWheelVertical";
            this.textBoxMouseWheelVertical.Size = new System.Drawing.Size(50, 20);
            this.textBoxMouseWheelVertical.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Channel increment";
            this.toolTip.SetToolTip(this.label3, "How many channels a single mouse wheel movement scrolls through");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxTimerCheckFrequency);
            this.groupBox1.Location = new System.Drawing.Point(3, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(440, 45);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Timers";
            this.toolTip.SetToolTip(this.groupBox1, "How often the timer schedule is checked (in seconds)");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(195, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "seconds";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Timer check frequency";
            // 
            // textBoxTimerCheckFrequency
            // 
            this.textBoxTimerCheckFrequency.Location = new System.Drawing.Point(139, 13);
            this.textBoxTimerCheckFrequency.Name = "textBoxTimerCheckFrequency";
            this.textBoxTimerCheckFrequency.Size = new System.Drawing.Size(50, 20);
            this.textBoxTimerCheckFrequency.TabIndex = 1;
            // 
            // screenTab
            // 
            this.screenTab.Controls.Add(this.cbUseCheckmark);
            this.screenTab.Controls.Add(this.gbColors);
            this.screenTab.Controls.Add(this.lblPrimaryScreen);
            this.screenTab.Controls.Add(this.cbScreens);
            this.screenTab.Location = new System.Drawing.Point(0, 0);
            this.screenTab.Name = "screenTab";
            this.screenTab.Size = new System.Drawing.Size(446, 413);
            this.screenTab.TabIndex = 7;
            this.screenTab.Text = "screenTab";
            this.screenTab.UseVisualStyleBackColor = true;
            // 
            // cbUseCheckmark
            // 
            this.cbUseCheckmark.AutoSize = true;
            this.cbUseCheckmark.Checked = true;
            this.cbUseCheckmark.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUseCheckmark.Location = new System.Drawing.Point(12, 148);
            this.cbUseCheckmark.Name = "cbUseCheckmark";
            this.cbUseCheckmark.Size = new System.Drawing.Size(193, 17);
            this.cbUseCheckmark.TabIndex = 4;
            this.cbUseCheckmark.Text = "Use Checkmark Selection Highlight";
            this.cbUseCheckmark.UseVisualStyleBackColor = true;
            // 
            // gbColors
            // 
            this.gbColors.Controls.Add(this.lblClickToUpdate);
            this.gbColors.Controls.Add(this.lblCurrentColor);
            this.gbColors.Controls.Add(this.pbColor);
            this.gbColors.Controls.Add(this.lbColorizableItems);
            this.gbColors.Location = new System.Drawing.Point(6, 32);
            this.gbColors.Name = "gbColors";
            this.gbColors.Size = new System.Drawing.Size(437, 109);
            this.gbColors.TabIndex = 3;
            this.gbColors.TabStop = false;
            this.gbColors.Text = "Customizable Colors";
            // 
            // lblClickToUpdate
            // 
            this.lblClickToUpdate.AutoSize = true;
            this.lblClickToUpdate.Location = new System.Drawing.Point(281, 88);
            this.lblClickToUpdate.Name = "lblClickToUpdate";
            this.lblClickToUpdate.Size = new System.Drawing.Size(104, 13);
            this.lblClickToUpdate.TabIndex = 5;
            this.lblClickToUpdate.Text = "Click color to update";
            // 
            // lblCurrentColor
            // 
            this.lblCurrentColor.AutoSize = true;
            this.lblCurrentColor.Location = new System.Drawing.Point(281, 19);
            this.lblCurrentColor.Name = "lblCurrentColor";
            this.lblCurrentColor.Size = new System.Drawing.Size(71, 13);
            this.lblCurrentColor.TabIndex = 4;
            this.lblCurrentColor.Text = "Current Color:";
            // 
            // pbColor
            // 
            this.pbColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbColor.Location = new System.Drawing.Point(281, 35);
            this.pbColor.Name = "pbColor";
            this.pbColor.Size = new System.Drawing.Size(100, 50);
            this.pbColor.TabIndex = 3;
            this.pbColor.TabStop = false;
            this.pbColor.Click += new System.EventHandler(this.pbColor_Click);
            // 
            // lbColorizableItems
            // 
            this.lbColorizableItems.FormattingEnabled = true;
            this.lbColorizableItems.Location = new System.Drawing.Point(6, 19);
            this.lbColorizableItems.Name = "lbColorizableItems";
            this.lbColorizableItems.Size = new System.Drawing.Size(269, 82);
            this.lbColorizableItems.TabIndex = 2;
            this.lbColorizableItems.SelectedIndexChanged += new System.EventHandler(this.lbColorizableItems_SelectedIndexChanged);
            this.lbColorizableItems.DoubleClick += new System.EventHandler(this.pbColor_Click);
            // 
            // lblPrimaryScreen
            // 
            this.lblPrimaryScreen.AutoSize = true;
            this.lblPrimaryScreen.Location = new System.Drawing.Point(3, 6);
            this.lblPrimaryScreen.Name = "lblPrimaryScreen";
            this.lblPrimaryScreen.Size = new System.Drawing.Size(81, 13);
            this.lblPrimaryScreen.TabIndex = 1;
            this.lblPrimaryScreen.Text = "Primary Screen:";
            // 
            // cbScreens
            // 
            this.cbScreens.FormattingEnabled = true;
            this.cbScreens.Location = new System.Drawing.Point(90, 3);
            this.cbScreens.Name = "cbScreens";
            this.cbScreens.Size = new System.Drawing.Size(353, 21);
            this.cbScreens.TabIndex = 0;
            // 
            // newSequenceSettingsTab
            // 
            this.newSequenceSettingsTab.BackColor = System.Drawing.Color.Transparent;
            this.newSequenceSettingsTab.Controls.Add(this.groupBox12);
            this.newSequenceSettingsTab.Controls.Add(this.groupBox8);
            this.newSequenceSettingsTab.Controls.Add(this.checkBoxWizardForNewSequences);
            this.newSequenceSettingsTab.Location = new System.Drawing.Point(0, 0);
            this.newSequenceSettingsTab.Name = "newSequenceSettingsTab";
            this.newSequenceSettingsTab.Padding = new System.Windows.Forms.Padding(3);
            this.newSequenceSettingsTab.Size = new System.Drawing.Size(446, 413);
            this.newSequenceSettingsTab.TabIndex = 0;
            this.newSequenceSettingsTab.Text = "newSequenceSettingsTab";
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.comboBoxDefaultAudioDevice);
            this.groupBox12.Controls.Add(this.label27);
            this.groupBox12.Controls.Add(this.checkBoxUseDefaultPlugInData);
            this.groupBox12.Controls.Add(this.buttonPluginSetup);
            this.groupBox12.Controls.Add(this.textBoxDefaultChannelCount);
            this.groupBox12.Controls.Add(this.comboBoxDefaultProfile);
            this.groupBox12.Controls.Add(this.label22);
            this.groupBox12.Controls.Add(this.label19);
            this.groupBox12.Controls.Add(this.label18);
            this.groupBox12.Controls.Add(this.buttonCreateProfile);
            this.groupBox12.Location = new System.Drawing.Point(19, 129);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(424, 112);
            this.groupBox12.TabIndex = 1;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Defaults";
            // 
            // comboBoxDefaultAudioDevice
            // 
            this.comboBoxDefaultAudioDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDefaultAudioDevice.FormattingEnabled = true;
            this.comboBoxDefaultAudioDevice.Location = new System.Drawing.Point(215, 78);
            this.comboBoxDefaultAudioDevice.Name = "comboBoxDefaultAudioDevice";
            this.comboBoxDefaultAudioDevice.Size = new System.Drawing.Size(192, 21);
            this.comboBoxDefaultAudioDevice.TabIndex = 19;
            this.toolTip.SetToolTip(this.comboBoxDefaultAudioDevice, "Audio device that will be selected for a new sequence");
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(17, 81);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(155, 13);
            this.label27.TabIndex = 18;
            this.label27.Text = "Default sequence audio device";
            this.toolTip.SetToolTip(this.label27, "Audio device that will be selected for a new sequence");
            // 
            // checkBoxUseDefaultPlugInData
            // 
            this.checkBoxUseDefaultPlugInData.AutoSize = true;
            this.checkBoxUseDefaultPlugInData.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkBoxUseDefaultPlugInData.Location = new System.Drawing.Point(215, 159);
            this.checkBoxUseDefaultPlugInData.Name = "checkBoxUseDefaultPlugInData";
            this.checkBoxUseDefaultPlugInData.Size = new System.Drawing.Size(159, 17);
            this.checkBoxUseDefaultPlugInData.TabIndex = 14;
            this.checkBoxUseDefaultPlugInData.Text = "Copy this to new sequences";
            this.checkBoxUseDefaultPlugInData.UseVisualStyleBackColor = true;
            this.checkBoxUseDefaultPlugInData.Visible = false;
            // 
            // buttonPluginSetup
            // 
            this.buttonPluginSetup.Location = new System.Drawing.Point(215, 130);
            this.buttonPluginSetup.Name = "buttonPluginSetup";
            this.buttonPluginSetup.Size = new System.Drawing.Size(75, 23);
            this.buttonPluginSetup.TabIndex = 13;
            this.buttonPluginSetup.Text = "Plugin Setup";
            this.toolTip.SetToolTip(this.buttonPluginSetup, "Plugin data for new sequences or for circumstances with no sequence available");
            this.buttonPluginSetup.UseVisualStyleBackColor = true;
            this.buttonPluginSetup.Visible = false;
            // 
            // textBoxDefaultChannelCount
            // 
            this.textBoxDefaultChannelCount.Location = new System.Drawing.Point(215, 104);
            this.textBoxDefaultChannelCount.Name = "textBoxDefaultChannelCount";
            this.textBoxDefaultChannelCount.Size = new System.Drawing.Size(50, 20);
            this.textBoxDefaultChannelCount.TabIndex = 11;
            this.toolTip.SetToolTip(this.textBoxDefaultChannelCount, "The number of channels for new sequences or the number of channels to assume for " +
                    "circumstances with no sequence available");
            this.textBoxDefaultChannelCount.Visible = false;
            // 
            // comboBoxDefaultProfile
            // 
            this.comboBoxDefaultProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDefaultProfile.DropDownWidth = 150;
            this.comboBoxDefaultProfile.FormattingEnabled = true;
            this.comboBoxDefaultProfile.Location = new System.Drawing.Point(215, 22);
            this.comboBoxDefaultProfile.Name = "comboBoxDefaultProfile";
            this.comboBoxDefaultProfile.Size = new System.Drawing.Size(192, 21);
            this.comboBoxDefaultProfile.TabIndex = 16;
            this.toolTip.SetToolTip(this.comboBoxDefaultProfile, "Profile that will be used for new sequences and");
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(17, 25);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(72, 13);
            this.label22.TabIndex = 15;
            this.label22.Text = "Default profile";
            this.toolTip.SetToolTip(this.label22, "Profile that will be used for new sequences and\r\nfor external clients.");
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(17, 135);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(101, 13);
            this.label19.TabIndex = 12;
            this.label19.Text = "Default plugin setup";
            this.toolTip.SetToolTip(this.label19, "Plugin data for new sequences or for circumstances with no sequence available");
            this.label19.Visible = false;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(17, 107);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(112, 13);
            this.label18.TabIndex = 10;
            this.label18.Text = "Default channel count";
            this.toolTip.SetToolTip(this.label18, "The number of channels for new sequences or for circumstances with no sequence av" +
                    "ailable");
            this.label18.Visible = false;
            // 
            // buttonCreateProfile
            // 
            this.buttonCreateProfile.Location = new System.Drawing.Point(215, 49);
            this.buttonCreateProfile.Name = "buttonCreateProfile";
            this.buttonCreateProfile.Size = new System.Drawing.Size(75, 23);
            this.buttonCreateProfile.TabIndex = 17;
            this.buttonCreateProfile.Text = "Create new";
            this.toolTip.SetToolTip(this.buttonCreateProfile, "Create new profiles now");
            this.buttonCreateProfile.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.numericUpDownMaximumLevel);
            this.groupBox8.Controls.Add(this.label17);
            this.groupBox8.Controls.Add(this.numericUpDownMinimumLevel);
            this.groupBox8.Controls.Add(this.label14);
            this.groupBox8.Controls.Add(this.label13);
            this.groupBox8.Controls.Add(this.textBoxEventPeriod);
            this.groupBox8.Controls.Add(this.label12);
            this.groupBox8.Location = new System.Drawing.Point(19, 14);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(424, 109);
            this.groupBox8.TabIndex = 0;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Editing Grid";
            // 
            // numericUpDownMaximumLevel
            // 
            this.numericUpDownMaximumLevel.Location = new System.Drawing.Point(215, 74);
            this.numericUpDownMaximumLevel.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownMaximumLevel.Name = "numericUpDownMaximumLevel";
            this.numericUpDownMaximumLevel.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownMaximumLevel.TabIndex = 7;
            this.toolTip.SetToolTip(this.numericUpDownMaximumLevel, "Maximum illumination level allowed by a sequence");
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(17, 76);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(166, 13);
            this.label17.TabIndex = 6;
            this.label17.Text = "Maximum illumination level (0-255)";
            this.toolTip.SetToolTip(this.label17, "Maximum illumination level allowed by a sequence");
            // 
            // numericUpDownMinimumLevel
            // 
            this.numericUpDownMinimumLevel.Location = new System.Drawing.Point(215, 48);
            this.numericUpDownMinimumLevel.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownMinimumLevel.Name = "numericUpDownMinimumLevel";
            this.numericUpDownMinimumLevel.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownMinimumLevel.TabIndex = 4;
            this.toolTip.SetToolTip(this.numericUpDownMinimumLevel, "Minimum illumination level allowed by a sequence");
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(17, 50);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(163, 13);
            this.label14.TabIndex = 3;
            this.label14.Text = "Minimum illumination level (0-255)";
            this.toolTip.SetToolTip(this.label14, "Minimum illumination level allowed by a sequence");
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(271, 25);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(63, 13);
            this.label13.TabIndex = 2;
            this.label13.Text = "milliseconds";
            // 
            // textBoxEventPeriod
            // 
            this.textBoxEventPeriod.Location = new System.Drawing.Point(215, 22);
            this.textBoxEventPeriod.Name = "textBoxEventPeriod";
            this.textBoxEventPeriod.Size = new System.Drawing.Size(50, 20);
            this.textBoxEventPeriod.TabIndex = 1;
            this.toolTip.SetToolTip(this.textBoxEventPeriod, "The length of a single event period (in milliseconds)");
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(17, 25);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(99, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "Event period length";
            this.toolTip.SetToolTip(this.label12, "The length of a single event period (in milliseconds)");
            // 
            // checkBoxWizardForNewSequences
            // 
            this.checkBoxWizardForNewSequences.AutoSize = true;
            this.checkBoxWizardForNewSequences.Location = new System.Drawing.Point(19, 260);
            this.checkBoxWizardForNewSequences.Name = "checkBoxWizardForNewSequences";
            this.checkBoxWizardForNewSequences.Size = new System.Drawing.Size(316, 17);
            this.checkBoxWizardForNewSequences.TabIndex = 2;
            this.checkBoxWizardForNewSequences.Text = "Use the sequence wizard for new sequences, when available";
            this.toolTip.SetToolTip(this.checkBoxWizardForNewSequences, "Use the sequence wizard for new sequences, when the editor allows for a wizard to" +
                    " be used");
            this.checkBoxWizardForNewSequences.UseVisualStyleBackColor = true;
            // 
            // sequenceEditingTab
            // 
            this.sequenceEditingTab.BackColor = System.Drawing.Color.Transparent;
            this.sequenceEditingTab.Controls.Add(this.groupBox14);
            this.sequenceEditingTab.Controls.Add(this.groupBox5);
            this.sequenceEditingTab.Location = new System.Drawing.Point(0, 0);
            this.sequenceEditingTab.Name = "sequenceEditingTab";
            this.sequenceEditingTab.Padding = new System.Windows.Forms.Padding(3);
            this.sequenceEditingTab.Size = new System.Drawing.Size(446, 413);
            this.sequenceEditingTab.TabIndex = 1;
            this.sequenceEditingTab.Text = "sequenceEditingTab";
            // 
            // groupBox14
            // 
            this.groupBox14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox14.Controls.Add(this.textBoxCurveLibraryFtpPassword);
            this.groupBox14.Controls.Add(this.label33);
            this.groupBox14.Controls.Add(this.textBoxCurveLibraryFtpUserName);
            this.groupBox14.Controls.Add(this.label32);
            this.groupBox14.Controls.Add(this.textBoxCurveLibraryFileName);
            this.groupBox14.Controls.Add(this.label31);
            this.groupBox14.Controls.Add(this.textBoxCurveLibraryFtpUrl);
            this.groupBox14.Controls.Add(this.label30);
            this.groupBox14.Controls.Add(this.textBoxCurveLibraryHttpUrl);
            this.groupBox14.Controls.Add(this.label29);
            this.groupBox14.Location = new System.Drawing.Point(19, 305);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(424, 107);
            this.groupBox14.TabIndex = 1;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = "Remote curve library";
            this.groupBox14.Visible = false;
            // 
            // textBoxCurveLibraryFtpPassword
            // 
            this.textBoxCurveLibraryFtpPassword.Location = new System.Drawing.Point(304, 99);
            this.textBoxCurveLibraryFtpPassword.Name = "textBoxCurveLibraryFtpPassword";
            this.textBoxCurveLibraryFtpPassword.Size = new System.Drawing.Size(111, 20);
            this.textBoxCurveLibraryFtpPassword.TabIndex = 9;
            this.toolTip.SetToolTip(this.textBoxCurveLibraryFtpPassword, "FTP password for the FTP server on which the remote curve library is located.  Ma" +
                    "y possibly be blank.");
            this.textBoxCurveLibraryFtpPassword.Visible = false;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(223, 102);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(75, 13);
            this.label33.TabIndex = 8;
            this.label33.Text = "FTP password";
            this.toolTip.SetToolTip(this.label33, "FTP password for the FTP server on which the remote curve library is located.  Ma" +
                    "y possibly be blank.");
            this.label33.Visible = false;
            // 
            // textBoxCurveLibraryFtpUserName
            // 
            this.textBoxCurveLibraryFtpUserName.Location = new System.Drawing.Point(105, 99);
            this.textBoxCurveLibraryFtpUserName.Name = "textBoxCurveLibraryFtpUserName";
            this.textBoxCurveLibraryFtpUserName.Size = new System.Drawing.Size(100, 20);
            this.textBoxCurveLibraryFtpUserName.TabIndex = 7;
            this.toolTip.SetToolTip(this.textBoxCurveLibraryFtpUserName, "FTP user name for the FTP server on which the remote curve library is located.  M" +
                    "ay possibly be blank.");
            this.textBoxCurveLibraryFtpUserName.Visible = false;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(20, 102);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(79, 13);
            this.label32.TabIndex = 6;
            this.label32.Text = "FTP user name";
            this.toolTip.SetToolTip(this.label32, "FTP user name for the FTP server on which the remote curve library is located.  M" +
                    "ay possibly be blank.");
            this.label32.Visible = false;
            // 
            // textBoxCurveLibraryFileName
            // 
            this.textBoxCurveLibraryFileName.Location = new System.Drawing.Point(105, 73);
            this.textBoxCurveLibraryFileName.Name = "textBoxCurveLibraryFileName";
            this.textBoxCurveLibraryFileName.Size = new System.Drawing.Size(142, 20);
            this.textBoxCurveLibraryFileName.TabIndex = 5;
            this.toolTip.SetToolTip(this.textBoxCurveLibraryFileName, "Name of the remote curve library file.");
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(20, 76);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(52, 13);
            this.label31.TabIndex = 4;
            this.label31.Text = "File name";
            this.toolTip.SetToolTip(this.label31, "Name of the remote curve library file.");
            // 
            // textBoxCurveLibraryFtpUrl
            // 
            this.textBoxCurveLibraryFtpUrl.Location = new System.Drawing.Point(105, 47);
            this.textBoxCurveLibraryFtpUrl.Name = "textBoxCurveLibraryFtpUrl";
            this.textBoxCurveLibraryFtpUrl.Size = new System.Drawing.Size(310, 20);
            this.textBoxCurveLibraryFtpUrl.TabIndex = 3;
            this.toolTip.SetToolTip(this.textBoxCurveLibraryFtpUrl, "FTP location of the remote curve library.  May possibly be the same as the HTTP l" +
                    "ocation.");
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(20, 50);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(52, 13);
            this.label30.TabIndex = 2;
            this.label30.Text = "FTP URL";
            this.toolTip.SetToolTip(this.label30, "FTP location of the remote curve library.  May possibly be the same as the HTTP l" +
                    "ocation.");
            // 
            // textBoxCurveLibraryHttpUrl
            // 
            this.textBoxCurveLibraryHttpUrl.Location = new System.Drawing.Point(105, 21);
            this.textBoxCurveLibraryHttpUrl.Name = "textBoxCurveLibraryHttpUrl";
            this.textBoxCurveLibraryHttpUrl.Size = new System.Drawing.Size(310, 20);
            this.textBoxCurveLibraryHttpUrl.TabIndex = 1;
            this.toolTip.SetToolTip(this.textBoxCurveLibraryHttpUrl, "HTTP location of the remote curve library");
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(20, 24);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(61, 13);
            this.label29.TabIndex = 0;
            this.label29.Text = "HTTP URL";
            this.toolTip.SetToolTip(this.label29, "HTTP location of the remote curve library");
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.cbWavefromZeroLine);
            this.groupBox5.Controls.Add(this.textBoxDefaultSequenceSaveDirectory);
            this.groupBox5.Controls.Add(this.label34);
            this.groupBox5.Controls.Add(this.checkBoxFlipMouseScroll);
            this.groupBox5.Controls.Add(this.checkBoxShowNaturalChannelNumber);
            this.groupBox5.Controls.Add(this.textBoxIntensityLargeDelta);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.checkBoxShowSaveConfirmation);
            this.groupBox5.Controls.Add(this.checkBoxSaveZoomLevels);
            this.groupBox5.Controls.Add(this.checkBoxEventSequenceAutoSize);
            this.groupBox5.Controls.Add(this.textBoxMaxRowHeight);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.textBoxMaxColumnWidth);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Location = new System.Drawing.Point(19, 14);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(424, 285);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Sequence Editing";
            // 
            // cbWavefromZeroLine
            // 
            this.cbWavefromZeroLine.AutoSize = true;
            this.cbWavefromZeroLine.Location = new System.Drawing.Point(215, 112);
            this.cbWavefromZeroLine.Name = "cbWavefromZeroLine";
            this.cbWavefromZeroLine.Size = new System.Drawing.Size(147, 17);
            this.cbWavefromZeroLine.TabIndex = 13;
            this.cbWavefromZeroLine.Text = "Show Wavform Zero Line";
            this.cbWavefromZeroLine.UseVisualStyleBackColor = true;
            // 
            // textBoxDefaultSequenceSaveDirectory
            // 
            this.textBoxDefaultSequenceSaveDirectory.Location = new System.Drawing.Point(20, 253);
            this.textBoxDefaultSequenceSaveDirectory.Name = "textBoxDefaultSequenceSaveDirectory";
            this.textBoxDefaultSequenceSaveDirectory.Size = new System.Drawing.Size(395, 20);
            this.textBoxDefaultSequenceSaveDirectory.TabIndex = 12;
            this.toolTip.SetToolTip(this.textBoxDefaultSequenceSaveDirectory, "Application default is My Documents\\Vixen\\Sequences");
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(20, 237);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(302, 13);
            this.label34.TabIndex = 11;
            this.label34.Text = "Default sequence save directory: (blank for application default)";
            // 
            // checkBoxFlipMouseScroll
            // 
            this.checkBoxFlipMouseScroll.AutoSize = true;
            this.checkBoxFlipMouseScroll.Location = new System.Drawing.Point(20, 204);
            this.checkBoxFlipMouseScroll.Name = "checkBoxFlipMouseScroll";
            this.checkBoxFlipMouseScroll.Size = new System.Drawing.Size(180, 17);
            this.checkBoxFlipMouseScroll.TabIndex = 10;
            this.checkBoxFlipMouseScroll.Text = "Flip mouse scroll + Shift behavior";
            this.toolTip.SetToolTip(this.checkBoxFlipMouseScroll, "The default behavior scrolls horizontally when Shift is down.\r\nSelect this to mak" +
                    "e it scroll vertically when Shift is down.");
            this.checkBoxFlipMouseScroll.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowNaturalChannelNumber
            // 
            this.checkBoxShowNaturalChannelNumber.AutoSize = true;
            this.checkBoxShowNaturalChannelNumber.Location = new System.Drawing.Point(20, 181);
            this.checkBoxShowNaturalChannelNumber.Name = "checkBoxShowNaturalChannelNumber";
            this.checkBoxShowNaturalChannelNumber.Size = new System.Drawing.Size(167, 17);
            this.checkBoxShowNaturalChannelNumber.TabIndex = 9;
            this.checkBoxShowNaturalChannelNumber.Text = "Show natural channel number";
            this.toolTip.SetToolTip(this.checkBoxShowNaturalChannelNumber, "Show the channel numbers according to the order they were created in");
            this.checkBoxShowNaturalChannelNumber.UseVisualStyleBackColor = true;
            // 
            // textBoxIntensityLargeDelta
            // 
            this.textBoxIntensityLargeDelta.Location = new System.Drawing.Point(215, 79);
            this.textBoxIntensityLargeDelta.Name = "textBoxIntensityLargeDelta";
            this.textBoxIntensityLargeDelta.Size = new System.Drawing.Size(50, 20);
            this.textBoxIntensityLargeDelta.TabIndex = 5;
            this.toolTip.SetToolTip(this.textBoxIntensityLargeDelta, "When using Ctrl-Up or Ctrl-Down to adjust the intensity\r\nof a selection.");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(190, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Intensity adjust large change (% points)";
            this.toolTip.SetToolTip(this.label5, "When using Ctrl-Up or Ctrl-Down to adjust the intensity");
            // 
            // checkBoxShowSaveConfirmation
            // 
            this.checkBoxShowSaveConfirmation.AutoSize = true;
            this.checkBoxShowSaveConfirmation.Location = new System.Drawing.Point(20, 158);
            this.checkBoxShowSaveConfirmation.Name = "checkBoxShowSaveConfirmation";
            this.checkBoxShowSaveConfirmation.Size = new System.Drawing.Size(139, 17);
            this.checkBoxShowSaveConfirmation.TabIndex = 8;
            this.checkBoxShowSaveConfirmation.Text = "Show save confirmation";
            this.toolTip.SetToolTip(this.checkBoxShowSaveConfirmation, "Show a confirmation message after saving a sequence");
            this.checkBoxShowSaveConfirmation.UseVisualStyleBackColor = true;
            // 
            // checkBoxSaveZoomLevels
            // 
            this.checkBoxSaveZoomLevels.AutoSize = true;
            this.checkBoxSaveZoomLevels.Location = new System.Drawing.Point(20, 135);
            this.checkBoxSaveZoomLevels.Name = "checkBoxSaveZoomLevels";
            this.checkBoxSaveZoomLevels.Size = new System.Drawing.Size(109, 17);
            this.checkBoxSaveZoomLevels.TabIndex = 7;
            this.checkBoxSaveZoomLevels.Text = "Save zoom levels";
            this.toolTip.SetToolTip(this.checkBoxSaveZoomLevels, "Save the row and height zoom levels");
            this.checkBoxSaveZoomLevels.UseVisualStyleBackColor = true;
            // 
            // checkBoxEventSequenceAutoSize
            // 
            this.checkBoxEventSequenceAutoSize.AutoSize = true;
            this.checkBoxEventSequenceAutoSize.Location = new System.Drawing.Point(20, 112);
            this.checkBoxEventSequenceAutoSize.Name = "checkBoxEventSequenceAutoSize";
            this.checkBoxEventSequenceAutoSize.Size = new System.Drawing.Size(154, 17);
            this.checkBoxEventSequenceAutoSize.TabIndex = 6;
            this.checkBoxEventSequenceAutoSize.Text = "Auto size event sequences";
            this.toolTip.SetToolTip(this.checkBoxEventSequenceAutoSize, "Automatically resize an event sequence to the length of the selected audio");
            this.checkBoxEventSequenceAutoSize.UseVisualStyleBackColor = true;
            // 
            // textBoxMaxRowHeight
            // 
            this.textBoxMaxRowHeight.Location = new System.Drawing.Point(215, 48);
            this.textBoxMaxRowHeight.Name = "textBoxMaxRowHeight";
            this.textBoxMaxRowHeight.Size = new System.Drawing.Size(50, 20);
            this.textBoxMaxRowHeight.TabIndex = 3;
            this.toolTip.SetToolTip(this.textBoxMaxRowHeight, "The height of a channel in the editing grid at 100%");
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Max row height";
            this.toolTip.SetToolTip(this.label7, "The height of a channel in the editing grid at 100%");
            // 
            // textBoxMaxColumnWidth
            // 
            this.textBoxMaxColumnWidth.Location = new System.Drawing.Point(215, 27);
            this.textBoxMaxColumnWidth.Name = "textBoxMaxColumnWidth";
            this.textBoxMaxColumnWidth.Size = new System.Drawing.Size(50, 20);
            this.textBoxMaxColumnWidth.TabIndex = 1;
            this.toolTip.SetToolTip(this.textBoxMaxColumnWidth, "The width of an event period in the editing grid at 100%");
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Max column width";
            this.toolTip.SetToolTip(this.label6, "The width of an event period in the editing grid at 100%");
            // 
            // sequenceExecutionTab
            // 
            this.sequenceExecutionTab.BackColor = System.Drawing.Color.Transparent;
            this.sequenceExecutionTab.Controls.Add(this.groupBox13);
            this.sequenceExecutionTab.Controls.Add(this.groupBox4);
            this.sequenceExecutionTab.Location = new System.Drawing.Point(0, 0);
            this.sequenceExecutionTab.Name = "sequenceExecutionTab";
            this.sequenceExecutionTab.Size = new System.Drawing.Size(446, 413);
            this.sequenceExecutionTab.TabIndex = 2;
            this.sequenceExecutionTab.Text = "sequenceExecutionTab";
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.textBoxLogFilePath);
            this.groupBox13.Controls.Add(this.buttonLogFilePath);
            this.groupBox13.Controls.Add(this.checkBoxLogMusicPlayer);
            this.groupBox13.Controls.Add(this.checkBoxLogScheduled);
            this.groupBox13.Controls.Add(this.checkBoxLogManual);
            this.groupBox13.Location = new System.Drawing.Point(19, 166);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(424, 139);
            this.groupBox13.TabIndex = 4;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Audio Log";
            // 
            // textBoxLogFilePath
            // 
            this.textBoxLogFilePath.Location = new System.Drawing.Point(107, 108);
            this.textBoxLogFilePath.Name = "textBoxLogFilePath";
            this.textBoxLogFilePath.Size = new System.Drawing.Size(311, 20);
            this.textBoxLogFilePath.TabIndex = 4;
            this.toolTip.SetToolTip(this.textBoxLogFilePath, "Path of the log file");
            // 
            // buttonLogFilePath
            // 
            this.buttonLogFilePath.Location = new System.Drawing.Point(26, 106);
            this.buttonLogFilePath.Name = "buttonLogFilePath";
            this.buttonLogFilePath.Size = new System.Drawing.Size(75, 23);
            this.buttonLogFilePath.TabIndex = 3;
            this.buttonLogFilePath.Text = "Log path";
            this.toolTip.SetToolTip(this.buttonLogFilePath, "Path of the log file");
            this.buttonLogFilePath.UseVisualStyleBackColor = true;
            this.buttonLogFilePath.Click += new System.EventHandler(this.buttonLogFilePath_Click);
            // 
            // checkBoxLogMusicPlayer
            // 
            this.checkBoxLogMusicPlayer.AutoSize = true;
            this.checkBoxLogMusicPlayer.Location = new System.Drawing.Point(26, 76);
            this.checkBoxLogMusicPlayer.Name = "checkBoxLogMusicPlayer";
            this.checkBoxLogMusicPlayer.Size = new System.Drawing.Size(159, 17);
            this.checkBoxLogMusicPlayer.TabIndex = 2;
            this.checkBoxLogMusicPlayer.Text = "Log music player executions";
            this.toolTip.SetToolTip(this.checkBoxLogMusicPlayer, "Log the audio files that the music player executes");
            this.checkBoxLogMusicPlayer.UseVisualStyleBackColor = true;
            // 
            // checkBoxLogScheduled
            // 
            this.checkBoxLogScheduled.AutoSize = true;
            this.checkBoxLogScheduled.Location = new System.Drawing.Point(26, 53);
            this.checkBoxLogScheduled.Name = "checkBoxLogScheduled";
            this.checkBoxLogScheduled.Size = new System.Drawing.Size(200, 17);
            this.checkBoxLogScheduled.TabIndex = 1;
            this.checkBoxLogScheduled.Text = "Log scheduled sequence executions";
            this.toolTip.SetToolTip(this.checkBoxLogScheduled, "Sequence executions that are started automatically by the scheduler");
            this.checkBoxLogScheduled.UseVisualStyleBackColor = true;
            // 
            // checkBoxLogManual
            // 
            this.checkBoxLogManual.AutoSize = true;
            this.checkBoxLogManual.Location = new System.Drawing.Point(26, 30);
            this.checkBoxLogManual.Name = "checkBoxLogManual";
            this.checkBoxLogManual.Size = new System.Drawing.Size(185, 17);
            this.checkBoxLogManual.TabIndex = 0;
            this.checkBoxLogManual.Text = "Log manual sequence executions";
            this.toolTip.SetToolTip(this.checkBoxLogManual, "Sequence executions that you start manually in an editor");
            this.checkBoxLogManual.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBoxClearAtEndOfSequence);
            this.groupBox4.Controls.Add(this.checkBoxShowPositionMarker);
            this.groupBox4.Controls.Add(this.checkBoxSavePlugInDialogPositions);
            this.groupBox4.Controls.Add(this.checkBoxAutoScrolling);
            this.groupBox4.Location = new System.Drawing.Point(19, 14);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(424, 126);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Sequence Execution";
            // 
            // checkBoxClearAtEndOfSequence
            // 
            this.checkBoxClearAtEndOfSequence.AutoSize = true;
            this.checkBoxClearAtEndOfSequence.Location = new System.Drawing.Point(26, 96);
            this.checkBoxClearAtEndOfSequence.Name = "checkBoxClearAtEndOfSequence";
            this.checkBoxClearAtEndOfSequence.Size = new System.Drawing.Size(200, 17);
            this.checkBoxClearAtEndOfSequence.TabIndex = 3;
            this.checkBoxClearAtEndOfSequence.Text = "Reset controller at end of sequences";
            this.toolTip.SetToolTip(this.checkBoxClearAtEndOfSequence, "Sends a blank event to the plugins at the end of a sequence.\r\nUseful for parallel" +
                    " port-based controllers.  Does not affect every\r\ncontroller type.");
            this.checkBoxClearAtEndOfSequence.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowPositionMarker
            // 
            this.checkBoxShowPositionMarker.AutoSize = true;
            this.checkBoxShowPositionMarker.Location = new System.Drawing.Point(26, 27);
            this.checkBoxShowPositionMarker.Name = "checkBoxShowPositionMarker";
            this.checkBoxShowPositionMarker.Size = new System.Drawing.Size(127, 17);
            this.checkBoxShowPositionMarker.TabIndex = 0;
            this.checkBoxShowPositionMarker.Text = "Show position marker";
            this.toolTip.SetToolTip(this.checkBoxShowPositionMarker, "Show the current point of exeuction with a mark in the time panel");
            this.checkBoxShowPositionMarker.UseVisualStyleBackColor = true;
            // 
            // checkBoxSavePlugInDialogPositions
            // 
            this.checkBoxSavePlugInDialogPositions.AutoSize = true;
            this.checkBoxSavePlugInDialogPositions.Location = new System.Drawing.Point(26, 73);
            this.checkBoxSavePlugInDialogPositions.Name = "checkBoxSavePlugInDialogPositions";
            this.checkBoxSavePlugInDialogPositions.Size = new System.Drawing.Size(157, 17);
            this.checkBoxSavePlugInDialogPositions.TabIndex = 2;
            this.checkBoxSavePlugInDialogPositions.Text = "Save plugin dialog positions";
            this.toolTip.SetToolTip(this.checkBoxSavePlugInDialogPositions, "Save the positions of any windows created and displayed by plugins during executi" +
                    "on");
            this.checkBoxSavePlugInDialogPositions.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoScrolling
            // 
            this.checkBoxAutoScrolling.AutoSize = true;
            this.checkBoxAutoScrolling.Location = new System.Drawing.Point(26, 50);
            this.checkBoxAutoScrolling.Name = "checkBoxAutoScrolling";
            this.checkBoxAutoScrolling.Size = new System.Drawing.Size(89, 17);
            this.checkBoxAutoScrolling.TabIndex = 1;
            this.checkBoxAutoScrolling.Text = "Auto scrolling";
            this.toolTip.SetToolTip(this.checkBoxAutoScrolling, "Automatically scroll the editing display during execution so that the current poi" +
                    "nt of execution is always visible");
            this.checkBoxAutoScrolling.UseVisualStyleBackColor = true;
            // 
            // backgroundItemsTab
            // 
            this.backgroundItemsTab.BackColor = System.Drawing.Color.Transparent;
            this.backgroundItemsTab.Controls.Add(this.groupBox7);
            this.backgroundItemsTab.Controls.Add(this.groupBox6);
            this.backgroundItemsTab.Location = new System.Drawing.Point(0, 0);
            this.backgroundItemsTab.Name = "backgroundItemsTab";
            this.backgroundItemsTab.Size = new System.Drawing.Size(446, 413);
            this.backgroundItemsTab.TabIndex = 4;
            this.backgroundItemsTab.Text = "backgroundItemsTab";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label20);
            this.groupBox7.Controls.Add(this.textBoxMusicFadeDuration);
            this.groupBox7.Controls.Add(this.label21);
            this.groupBox7.Controls.Add(this.checkBoxEnableMusicFade);
            this.groupBox7.Controls.Add(this.label10);
            this.groupBox7.Controls.Add(this.textBoxBackgroundMusicDelay);
            this.groupBox7.Controls.Add(this.label11);
            this.groupBox7.Controls.Add(this.checkBoxEnableBackgroundMusic);
            this.groupBox7.Location = new System.Drawing.Point(19, 147);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(424, 135);
            this.groupBox7.TabIndex = 1;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Music";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(271, 105);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(47, 13);
            this.label20.TabIndex = 11;
            this.label20.Text = "seconds";
            // 
            // textBoxMusicFadeDuration
            // 
            this.textBoxMusicFadeDuration.Location = new System.Drawing.Point(215, 102);
            this.textBoxMusicFadeDuration.Name = "textBoxMusicFadeDuration";
            this.textBoxMusicFadeDuration.Size = new System.Drawing.Size(50, 20);
            this.textBoxMusicFadeDuration.TabIndex = 10;
            this.toolTip.SetToolTip(this.textBoxMusicFadeDuration, "How long (in seconds) it will take the background music to fade out");
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(14, 105);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(100, 13);
            this.label21.TabIndex = 9;
            this.label21.Text = "Music fade duration";
            this.toolTip.SetToolTip(this.label21, "How long (in seconds) it will take the background music to fade out");
            // 
            // checkBoxEnableMusicFade
            // 
            this.checkBoxEnableMusicFade.AutoSize = true;
            this.checkBoxEnableMusicFade.Location = new System.Drawing.Point(17, 80);
            this.checkBoxEnableMusicFade.Name = "checkBoxEnableMusicFade";
            this.checkBoxEnableMusicFade.Size = new System.Drawing.Size(113, 17);
            this.checkBoxEnableMusicFade.TabIndex = 8;
            this.checkBoxEnableMusicFade.Text = "Enable music fade";
            this.toolTip.SetToolTip(this.checkBoxEnableMusicFade, "Enable the fading of the background music when it\'s stopped");
            this.checkBoxEnableMusicFade.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(271, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = "seconds";
            // 
            // textBoxBackgroundMusicDelay
            // 
            this.textBoxBackgroundMusicDelay.Location = new System.Drawing.Point(215, 45);
            this.textBoxBackgroundMusicDelay.Name = "textBoxBackgroundMusicDelay";
            this.textBoxBackgroundMusicDelay.Size = new System.Drawing.Size(50, 20);
            this.textBoxBackgroundMusicDelay.TabIndex = 6;
            this.toolTip.SetToolTip(this.textBoxBackgroundMusicDelay, "How long (in seconds) the background sequence will wait before starting after seq" +
                    "uence or program execution stops");
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 48);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(123, 13);
            this.label11.TabIndex = 5;
            this.label11.Text = "Background music delay";
            this.toolTip.SetToolTip(this.label11, "How long (in seconds) the background music will wait before starting after sequen" +
                    "ce or program execution stops");
            // 
            // checkBoxEnableBackgroundMusic
            // 
            this.checkBoxEnableBackgroundMusic.AutoSize = true;
            this.checkBoxEnableBackgroundMusic.Location = new System.Drawing.Point(17, 23);
            this.checkBoxEnableBackgroundMusic.Name = "checkBoxEnableBackgroundMusic";
            this.checkBoxEnableBackgroundMusic.Size = new System.Drawing.Size(149, 17);
            this.checkBoxEnableBackgroundMusic.TabIndex = 4;
            this.checkBoxEnableBackgroundMusic.Text = "Enable background music";
            this.toolTip.SetToolTip(this.checkBoxEnableBackgroundMusic, "Enable the playing of background music while no sequences or programs are playing" +
                    "");
            this.checkBoxEnableBackgroundMusic.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label9);
            this.groupBox6.Controls.Add(this.textBoxBackgroundSequenceDelay);
            this.groupBox6.Controls.Add(this.label8);
            this.groupBox6.Controls.Add(this.checkBoxEnableBackgroundSequence);
            this.groupBox6.Location = new System.Drawing.Point(19, 14);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(424, 113);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Sequence";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(271, 70);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "seconds";
            // 
            // textBoxBackgroundSequenceDelay
            // 
            this.textBoxBackgroundSequenceDelay.Location = new System.Drawing.Point(215, 67);
            this.textBoxBackgroundSequenceDelay.Name = "textBoxBackgroundSequenceDelay";
            this.textBoxBackgroundSequenceDelay.Size = new System.Drawing.Size(50, 20);
            this.textBoxBackgroundSequenceDelay.TabIndex = 2;
            this.toolTip.SetToolTip(this.textBoxBackgroundSequenceDelay, "How long (in seconds) the background sequence will wait before starting after seq" +
                    "uence or program execution stops");
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 70);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(143, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Background sequence delay";
            this.toolTip.SetToolTip(this.label8, "How long (in seconds) the background sequence will wait before starting after seq" +
                    "uence or program execution stops");
            // 
            // checkBoxEnableBackgroundSequence
            // 
            this.checkBoxEnableBackgroundSequence.AutoSize = true;
            this.checkBoxEnableBackgroundSequence.Location = new System.Drawing.Point(17, 30);
            this.checkBoxEnableBackgroundSequence.Name = "checkBoxEnableBackgroundSequence";
            this.checkBoxEnableBackgroundSequence.Size = new System.Drawing.Size(169, 17);
            this.checkBoxEnableBackgroundSequence.TabIndex = 0;
            this.checkBoxEnableBackgroundSequence.Text = "Enable background sequence";
            this.toolTip.SetToolTip(this.checkBoxEnableBackgroundSequence, "Enable the playing of a scripted background sequence while no sequences or progra" +
                    "ms are playing");
            this.checkBoxEnableBackgroundSequence.UseVisualStyleBackColor = true;
            // 
            // remoteExecutionTab
            // 
            this.remoteExecutionTab.BackColor = System.Drawing.Color.Transparent;
            this.remoteExecutionTab.Controls.Add(this.groupBox10);
            this.remoteExecutionTab.Controls.Add(this.groupBox9);
            this.remoteExecutionTab.Location = new System.Drawing.Point(0, 0);
            this.remoteExecutionTab.Name = "remoteExecutionTab";
            this.remoteExecutionTab.Size = new System.Drawing.Size(446, 413);
            this.remoteExecutionTab.TabIndex = 5;
            this.remoteExecutionTab.Text = "remoteExecutionTab";
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.radioButtonAsyncSyncObject);
            this.groupBox10.Controls.Add(this.comboBoxAsyncProfile);
            this.groupBox10.Controls.Add(this.radioButtonAsyncDefaultProfileData);
            this.groupBox10.Controls.Add(this.radioButtonAsyncProfileData);
            this.groupBox10.Controls.Add(this.label25);
            this.groupBox10.Location = new System.Drawing.Point(19, 178);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(424, 169);
            this.groupBox10.TabIndex = 1;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Channel Control";
            // 
            // radioButtonAsyncSyncObject
            // 
            this.radioButtonAsyncSyncObject.AutoSize = true;
            this.radioButtonAsyncSyncObject.Location = new System.Drawing.Point(100, 96);
            this.radioButtonAsyncSyncObject.Name = "radioButtonAsyncSyncObject";
            this.radioButtonAsyncSyncObject.Size = new System.Drawing.Size(242, 17);
            this.radioButtonAsyncSyncObject.TabIndex = 1;
            this.radioButtonAsyncSyncObject.TabStop = true;
            this.radioButtonAsyncSyncObject.Text = "Use the sequence loaded for execution, if any";
            this.toolTip.SetToolTip(this.radioButtonAsyncSyncObject, "The sequence will use channel masking and plugin setup data from\r\nthe default pro" +
                    "file.");
            this.radioButtonAsyncSyncObject.UseVisualStyleBackColor = true;
            this.radioButtonAsyncSyncObject.CheckedChanged += new System.EventHandler(this.radioButtonAsyncProfileData_CheckedChanged);
            // 
            // comboBoxAsyncProfile
            // 
            this.comboBoxAsyncProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAsyncProfile.Enabled = false;
            this.comboBoxAsyncProfile.FormattingEnabled = true;
            this.comboBoxAsyncProfile.Location = new System.Drawing.Point(244, 118);
            this.comboBoxAsyncProfile.Name = "comboBoxAsyncProfile";
            this.comboBoxAsyncProfile.Size = new System.Drawing.Size(160, 21);
            this.comboBoxAsyncProfile.TabIndex = 3;
            // 
            // radioButtonAsyncDefaultProfileData
            // 
            this.radioButtonAsyncDefaultProfileData.AutoSize = true;
            this.radioButtonAsyncDefaultProfileData.Location = new System.Drawing.Point(100, 142);
            this.radioButtonAsyncDefaultProfileData.Name = "radioButtonAsyncDefaultProfileData";
            this.radioButtonAsyncDefaultProfileData.Size = new System.Drawing.Size(128, 17);
            this.radioButtonAsyncDefaultProfileData.TabIndex = 4;
            this.radioButtonAsyncDefaultProfileData.TabStop = true;
            this.radioButtonAsyncDefaultProfileData.Text = "Use the default profile";
            this.toolTip.SetToolTip(this.radioButtonAsyncDefaultProfileData, "The sequence will use channel masking and plugin setup data from\r\nthe default pro" +
                    "file.");
            this.radioButtonAsyncDefaultProfileData.UseVisualStyleBackColor = true;
            this.radioButtonAsyncDefaultProfileData.CheckedChanged += new System.EventHandler(this.radioButtonAsyncProfileData_CheckedChanged);
            // 
            // radioButtonAsyncProfileData
            // 
            this.radioButtonAsyncProfileData.AutoSize = true;
            this.radioButtonAsyncProfileData.Location = new System.Drawing.Point(100, 119);
            this.radioButtonAsyncProfileData.Name = "radioButtonAsyncProfileData";
            this.radioButtonAsyncProfileData.Size = new System.Drawing.Size(123, 17);
            this.radioButtonAsyncProfileData.TabIndex = 2;
            this.radioButtonAsyncProfileData.TabStop = true;
            this.radioButtonAsyncProfileData.Text = "Use a specific profile";
            this.toolTip.SetToolTip(this.radioButtonAsyncProfileData, "The sequence will use channel masking and plugin setup data from\r\na specified pro" +
                    "file.");
            this.radioButtonAsyncProfileData.UseVisualStyleBackColor = true;
            this.radioButtonAsyncProfileData.CheckedChanged += new System.EventHandler(this.radioButtonAsyncProfileData_CheckedChanged);
            // 
            // label25
            // 
            this.label25.Location = new System.Drawing.Point(18, 23);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(386, 66);
            this.label25.TabIndex = 0;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.comboBoxSyncProfile);
            this.groupBox9.Controls.Add(this.radioButtonSyncDefaultProfileData);
            this.groupBox9.Controls.Add(this.radioButtonSyncProfileData);
            this.groupBox9.Controls.Add(this.radioButtonSyncEmbeddedData);
            this.groupBox9.Controls.Add(this.label24);
            this.groupBox9.Location = new System.Drawing.Point(19, 3);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(424, 169);
            this.groupBox9.TabIndex = 0;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Sequence Execution";
            // 
            // comboBoxSyncProfile
            // 
            this.comboBoxSyncProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSyncProfile.Enabled = false;
            this.comboBoxSyncProfile.FormattingEnabled = true;
            this.comboBoxSyncProfile.Location = new System.Drawing.Point(244, 106);
            this.comboBoxSyncProfile.Name = "comboBoxSyncProfile";
            this.comboBoxSyncProfile.Size = new System.Drawing.Size(160, 21);
            this.comboBoxSyncProfile.TabIndex = 4;
            // 
            // radioButtonSyncDefaultProfileData
            // 
            this.radioButtonSyncDefaultProfileData.AutoSize = true;
            this.radioButtonSyncDefaultProfileData.Location = new System.Drawing.Point(100, 130);
            this.radioButtonSyncDefaultProfileData.Name = "radioButtonSyncDefaultProfileData";
            this.radioButtonSyncDefaultProfileData.Size = new System.Drawing.Size(128, 17);
            this.radioButtonSyncDefaultProfileData.TabIndex = 3;
            this.radioButtonSyncDefaultProfileData.TabStop = true;
            this.radioButtonSyncDefaultProfileData.Text = "Use the default profile";
            this.toolTip.SetToolTip(this.radioButtonSyncDefaultProfileData, "The sequence will use channel masking and plugin setup data from\r\nthe default pro" +
                    "file.");
            this.radioButtonSyncDefaultProfileData.UseVisualStyleBackColor = true;
            this.radioButtonSyncDefaultProfileData.CheckedChanged += new System.EventHandler(this.radioButtonSyncProfileData_CheckedChanged);
            // 
            // radioButtonSyncProfileData
            // 
            this.radioButtonSyncProfileData.AutoSize = true;
            this.radioButtonSyncProfileData.Location = new System.Drawing.Point(100, 107);
            this.radioButtonSyncProfileData.Name = "radioButtonSyncProfileData";
            this.radioButtonSyncProfileData.Size = new System.Drawing.Size(123, 17);
            this.radioButtonSyncProfileData.TabIndex = 2;
            this.radioButtonSyncProfileData.TabStop = true;
            this.radioButtonSyncProfileData.Text = "Use a specific profile";
            this.toolTip.SetToolTip(this.radioButtonSyncProfileData, "The sequence will use channel masking and plugin setup data from\r\na specified pro" +
                    "file.");
            this.radioButtonSyncProfileData.UseVisualStyleBackColor = true;
            this.radioButtonSyncProfileData.CheckedChanged += new System.EventHandler(this.radioButtonSyncProfileData_CheckedChanged);
            // 
            // radioButtonSyncEmbeddedData
            // 
            this.radioButtonSyncEmbeddedData.AutoSize = true;
            this.radioButtonSyncEmbeddedData.Location = new System.Drawing.Point(100, 84);
            this.radioButtonSyncEmbeddedData.Name = "radioButtonSyncEmbeddedData";
            this.radioButtonSyncEmbeddedData.Size = new System.Drawing.Size(114, 17);
            this.radioButtonSyncEmbeddedData.TabIndex = 1;
            this.radioButtonSyncEmbeddedData.TabStop = true;
            this.radioButtonSyncEmbeddedData.Text = "Use their own data";
            this.toolTip.SetToolTip(this.radioButtonSyncEmbeddedData, "The sequence will determine its own channel masking and use\r\nits own plugin setup" +
                    ".");
            this.radioButtonSyncEmbeddedData.UseVisualStyleBackColor = true;
            this.radioButtonSyncEmbeddedData.CheckedChanged += new System.EventHandler(this.radioButtonSyncProfileData_CheckedChanged);
            // 
            // label24
            // 
            this.label24.Location = new System.Drawing.Point(21, 26);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(383, 46);
            this.label24.TabIndex = 0;
            this.label24.Text = "When a sequence is executed remotely, would you like it to use its own data (for " +
                "plugins and channel masking) or would you prefer to have all sequences use the s" +
                "ame profile?";
            // 
            // engineTab
            // 
            this.engineTab.BackColor = System.Drawing.Color.Transparent;
            this.engineTab.Controls.Add(this.groupBox11);
            this.engineTab.Location = new System.Drawing.Point(0, 0);
            this.engineTab.Name = "engineTab";
            this.engineTab.Size = new System.Drawing.Size(446, 413);
            this.engineTab.TabIndex = 6;
            this.engineTab.Text = "engineTab";
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.buttonEngine);
            this.groupBox11.Controls.Add(this.textBoxEngine);
            this.groupBox11.Location = new System.Drawing.Point(19, 14);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(424, 64);
            this.groupBox11.TabIndex = 4;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Secondary Engine";
            // 
            // buttonEngine
            // 
            this.buttonEngine.Location = new System.Drawing.Point(385, 26);
            this.buttonEngine.Name = "buttonEngine";
            this.buttonEngine.Size = new System.Drawing.Size(24, 24);
            this.buttonEngine.TabIndex = 1;
            this.buttonEngine.Text = "...";
            this.buttonEngine.UseVisualStyleBackColor = true;
            this.buttonEngine.Click += new System.EventHandler(this.buttonEngine_Click);
            // 
            // textBoxEngine
            // 
            this.textBoxEngine.Location = new System.Drawing.Point(14, 28);
            this.textBoxEngine.Name = "textBoxEngine";
            this.textBoxEngine.Size = new System.Drawing.Size(365, 20);
            this.textBoxEngine.TabIndex = 0;
            // 
            // PreferencesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 484);
            this.Controls.Add(this.btnSetDataFolder);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.treeView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = global::Properties.Resources.VixenPlus;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PreferencesDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Preferences";
            this.tabControl.ResumeLayout(false);
            this.generalTab.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecentFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHistoryImages)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.screenTab.ResumeLayout(false);
            this.screenTab.PerformLayout();
            this.gbColors.ResumeLayout(false);
            this.gbColors.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbColor)).EndInit();
            this.newSequenceSettingsTab.ResumeLayout(false);
            this.newSequenceSettingsTab.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaximumLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMinimumLevel)).EndInit();
            this.sequenceEditingTab.ResumeLayout(false);
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.sequenceExecutionTab.ResumeLayout(false);
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.backgroundItemsTab.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.remoteExecutionTab.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.engineTab.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }


        private TabPage screenTab;
        private ComboBox cbScreens;
        private GroupBox gbColors;
        private ListBox lbColorizableItems;
        private Label lblPrimaryScreen;
        private CheckBox cbWavefromZeroLine;
        private Label lblClickToUpdate;
        private Label lblCurrentColor;
        private PictureBox pbColor;
        private ColorDialog colorDialog1;
        private Button btnSetDataFolder;
        private CheckBox cbUseCheckmark;
        private CheckBox cbToolbarAutoSave;
        private Label label36;
        private NumericUpDown nudRecentFiles;
        private Label label35;
    }
}
