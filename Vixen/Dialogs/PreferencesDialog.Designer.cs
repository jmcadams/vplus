using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using TabControl = VixenPlusCommon.TabControl;

namespace VixenPlus.Dialogs {
    internal partial class PreferencesDialog {
        private IContainer components;

        #region Windows Form Designer generated code

        private Button buttonCancel;
        private Button buttonOK;
        private FolderBrowserDialog folderBrowserDialog;
        private OpenFileDialog openFileDialog;
        private ToolTip toolTip;
        private TreeView treeView;


        private void InitializeComponent() {
            this.components = new Container();
            TreeNode treeNode1 = new TreeNode("General");
            TreeNode treeNode2 = new TreeNode("Screen and Colors");
            TreeNode treeNode3 = new TreeNode("New Sequence Settings");
            TreeNode treeNode4 = new TreeNode("Sequence Editing");
            TreeNode treeNode5 = new TreeNode("Sequence Execution");
            this.treeView = new TreeView();
            this.toolTip = new ToolTip(this.components);
            this.checkBoxAutoScrolling = new CheckBox();
            this.checkBoxSavePlugInDialogPositions = new CheckBox();
            this.checkBoxShowPositionMarker = new CheckBox();
            this.checkBoxClearAtEndOfSequence = new CheckBox();
            this.label6 = new Label();
            this.textBoxMaxColumnWidth = new TextBox();
            this.label7 = new Label();
            this.textBoxMaxRowHeight = new TextBox();
            this.checkBoxEventSequenceAutoSize = new CheckBox();
            this.checkBoxSaveZoomLevels = new CheckBox();
            this.checkBoxShowSaveConfirmation = new CheckBox();
            this.label5 = new Label();
            this.textBoxIntensityLargeDelta = new TextBox();
            this.checkBoxShowNaturalChannelNumber = new CheckBox();
            this.checkBoxFlipMouseScroll = new CheckBox();
            this.textBoxDefaultSequenceSaveDirectory = new TextBox();
            this.checkBoxWizardForNewSequences = new CheckBox();
            this.label12 = new Label();
            this.textBoxEventPeriod = new TextBox();
            this.label14 = new Label();
            this.numericUpDownMinimumLevel = new NumericUpDown();
            this.label17 = new Label();
            this.numericUpDownMaximumLevel = new NumericUpDown();
            this.label22 = new Label();
            this.comboBoxDefaultProfile = new ComboBox();
            this.label27 = new Label();
            this.comboBoxDefaultAudioDevice = new ComboBox();
            this.groupBox2 = new GroupBox();
            this.textBoxMouseWheelHorizontal = new TextBox();
            this.label4 = new Label();
            this.textBoxMouseWheelVertical = new TextBox();
            this.label3 = new Label();
            this.labelAutoShutdownTime = new Label();
            this.dateTimePickerAutoShutdownTime = new DateTimePicker();
            this.label26 = new Label();
            this.label36 = new Label();
            this.cbToolbarAutoSave = new CheckBox();
            this.cbUpdateFrequency = new ComboBox();
            this.cbProfileErrors = new CheckBox();
            this.cbChannelHighlight = new CheckBox();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.openFileDialog = new OpenFileDialog();
            this.folderBrowserDialog = new FolderBrowserDialog();
            this.colorDialog1 = new ColorDialog();
            this.btnSetDataFolder = new Button();
            this.sequenceExecutionTab = new TabPage();
            this.groupBox4 = new GroupBox();
            this.sequenceEditingTab = new TabPage();
            this.groupBox5 = new GroupBox();
            this.cbWavefromZeroLine = new CheckBox();
            this.label34 = new Label();
            this.newSequenceSettingsTab = new TabPage();
            this.groupBox12 = new GroupBox();
            this.groupBox8 = new GroupBox();
            this.label13 = new Label();
            this.screenTab = new TabPage();
            this.cbUseCheckmark = new CheckBox();
            this.gbColors = new GroupBox();
            this.lblClickToUpdate = new Label();
            this.lblCurrentColor = new Label();
            this.pbColor = new PictureBox();
            this.lbColorizableItems = new ListBox();
            this.lblPrimaryScreen = new Label();
            this.cbScreens = new ComboBox();
            this.generalTab = new TabPage();
            this.groupBox3 = new GroupBox();
            this.nudRecentFiles = new NumericUpDown();
            this.label35 = new Label();
            this.numericUpDownHistoryImages = new NumericUpDown();
            this.label28 = new Label();
            this.tabControl = new TabControl(this.components);
            ((ISupportInitialize)(this.numericUpDownMinimumLevel)).BeginInit();
            ((ISupportInitialize)(this.numericUpDownMaximumLevel)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.sequenceExecutionTab.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.sequenceEditingTab.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.newSequenceSettingsTab.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.screenTab.SuspendLayout();
            this.gbColors.SuspendLayout();
            ((ISupportInitialize)(this.pbColor)).BeginInit();
            this.generalTab.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((ISupportInitialize)(this.nudRecentFiles)).BeginInit();
            ((ISupportInitialize)(this.numericUpDownHistoryImages)).BeginInit();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Bottom)
                        | AnchorStyles.Left)));
            this.treeView.Location = new Point(12, 12);
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
            this.treeView.Nodes.AddRange(new TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5});
            this.treeView.Size = new Size(161, 310);
            this.treeView.TabIndex = 0;
            this.treeView.AfterSelect += new TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // checkBoxAutoScrolling
            // 
            this.checkBoxAutoScrolling.AutoSize = true;
            this.checkBoxAutoScrolling.Location = new Point(26, 50);
            this.checkBoxAutoScrolling.Name = "checkBoxAutoScrolling";
            this.checkBoxAutoScrolling.Size = new Size(89, 17);
            this.checkBoxAutoScrolling.TabIndex = 1;
            this.checkBoxAutoScrolling.Text = "Auto scrolling";
            this.toolTip.SetToolTip(this.checkBoxAutoScrolling, "Automatically scroll the editing display during execution so that the current poi" +
                    "nt of execution is always visible");
            this.checkBoxAutoScrolling.UseVisualStyleBackColor = true;
            // 
            // checkBoxSavePlugInDialogPositions
            // 
            this.checkBoxSavePlugInDialogPositions.AutoSize = true;
            this.checkBoxSavePlugInDialogPositions.Location = new Point(26, 73);
            this.checkBoxSavePlugInDialogPositions.Name = "checkBoxSavePlugInDialogPositions";
            this.checkBoxSavePlugInDialogPositions.Size = new Size(157, 17);
            this.checkBoxSavePlugInDialogPositions.TabIndex = 2;
            this.checkBoxSavePlugInDialogPositions.Text = "Save plugin dialog positions";
            this.toolTip.SetToolTip(this.checkBoxSavePlugInDialogPositions, "Save the positions of any windows created and displayed by plugins during executi" +
                    "on");
            this.checkBoxSavePlugInDialogPositions.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowPositionMarker
            // 
            this.checkBoxShowPositionMarker.AutoSize = true;
            this.checkBoxShowPositionMarker.Location = new Point(26, 27);
            this.checkBoxShowPositionMarker.Name = "checkBoxShowPositionMarker";
            this.checkBoxShowPositionMarker.Size = new Size(127, 17);
            this.checkBoxShowPositionMarker.TabIndex = 0;
            this.checkBoxShowPositionMarker.Text = "Show position marker";
            this.toolTip.SetToolTip(this.checkBoxShowPositionMarker, "Show the current point of exeuction with a mark in the time panel");
            this.checkBoxShowPositionMarker.UseVisualStyleBackColor = true;
            // 
            // checkBoxClearAtEndOfSequence
            // 
            this.checkBoxClearAtEndOfSequence.AutoSize = true;
            this.checkBoxClearAtEndOfSequence.Location = new Point(26, 96);
            this.checkBoxClearAtEndOfSequence.Name = "checkBoxClearAtEndOfSequence";
            this.checkBoxClearAtEndOfSequence.Size = new Size(200, 17);
            this.checkBoxClearAtEndOfSequence.TabIndex = 3;
            this.checkBoxClearAtEndOfSequence.Text = "Reset controller at end of sequences";
            this.toolTip.SetToolTip(this.checkBoxClearAtEndOfSequence, "Sends a blank event to the plugins at the end of a sequence.\r\nUseful for parallel" +
                    " port-based controllers.  Does not affect every\r\ncontroller type.");
            this.checkBoxClearAtEndOfSequence.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new Point(6, 16);
            this.label6.Name = "label6";
            this.label6.Size = new Size(92, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Max column width";
            this.toolTip.SetToolTip(this.label6, "The width of an event period in the editing grid at 100%");
            // 
            // textBoxMaxColumnWidth
            // 
            this.textBoxMaxColumnWidth.Location = new Point(104, 13);
            this.textBoxMaxColumnWidth.Name = "textBoxMaxColumnWidth";
            this.textBoxMaxColumnWidth.Size = new Size(50, 20);
            this.textBoxMaxColumnWidth.TabIndex = 1;
            this.toolTip.SetToolTip(this.textBoxMaxColumnWidth, "The width of an event period in the editing grid at 100%");
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new Point(201, 16);
            this.label7.Name = "label7";
            this.label7.Size = new Size(79, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Max row height";
            this.toolTip.SetToolTip(this.label7, "The height of a channel in the editing grid at 100%");
            // 
            // textBoxMaxRowHeight
            // 
            this.textBoxMaxRowHeight.Location = new Point(286, 13);
            this.textBoxMaxRowHeight.Name = "textBoxMaxRowHeight";
            this.textBoxMaxRowHeight.Size = new Size(50, 20);
            this.textBoxMaxRowHeight.TabIndex = 3;
            this.toolTip.SetToolTip(this.textBoxMaxRowHeight, "The height of a channel in the editing grid at 100%");
            // 
            // checkBoxEventSequenceAutoSize
            // 
            this.checkBoxEventSequenceAutoSize.AutoSize = true;
            this.checkBoxEventSequenceAutoSize.Location = new Point(9, 64);
            this.checkBoxEventSequenceAutoSize.Name = "checkBoxEventSequenceAutoSize";
            this.checkBoxEventSequenceAutoSize.Size = new Size(195, 17);
            this.checkBoxEventSequenceAutoSize.TabIndex = 6;
            this.checkBoxEventSequenceAutoSize.Text = "Auto size sequence to audio length.";
            this.toolTip.SetToolTip(this.checkBoxEventSequenceAutoSize, "Automatically resize an event sequence to the length of the selected audio");
            this.checkBoxEventSequenceAutoSize.UseVisualStyleBackColor = true;
            // 
            // checkBoxSaveZoomLevels
            // 
            this.checkBoxSaveZoomLevels.AutoSize = true;
            this.checkBoxSaveZoomLevels.Location = new Point(9, 87);
            this.checkBoxSaveZoomLevels.Name = "checkBoxSaveZoomLevels";
            this.checkBoxSaveZoomLevels.Size = new Size(109, 17);
            this.checkBoxSaveZoomLevels.TabIndex = 7;
            this.checkBoxSaveZoomLevels.Text = "Save zoom levels";
            this.toolTip.SetToolTip(this.checkBoxSaveZoomLevels, "Save the row and height zoom levels");
            this.checkBoxSaveZoomLevels.UseVisualStyleBackColor = true;
            // 
            // checkBoxShowSaveConfirmation
            // 
            this.checkBoxShowSaveConfirmation.AutoSize = true;
            this.checkBoxShowSaveConfirmation.Location = new Point(9, 110);
            this.checkBoxShowSaveConfirmation.Name = "checkBoxShowSaveConfirmation";
            this.checkBoxShowSaveConfirmation.Size = new Size(139, 17);
            this.checkBoxShowSaveConfirmation.TabIndex = 8;
            this.checkBoxShowSaveConfirmation.Text = "Show save confirmation";
            this.toolTip.SetToolTip(this.checkBoxShowSaveConfirmation, "Show a confirmation message after saving a sequence");
            this.checkBoxShowSaveConfirmation.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new Point(6, 36);
            this.label5.Name = "label5";
            this.label5.Size = new Size(190, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Intensity adjust large change (% points)";
            this.toolTip.SetToolTip(this.label5, "When using Ctrl-Up or Ctrl-Down to adjust the intensity");
            // 
            // textBoxIntensityLargeDelta
            // 
            this.textBoxIntensityLargeDelta.Location = new Point(202, 33);
            this.textBoxIntensityLargeDelta.Name = "textBoxIntensityLargeDelta";
            this.textBoxIntensityLargeDelta.Size = new Size(50, 20);
            this.textBoxIntensityLargeDelta.TabIndex = 5;
            this.toolTip.SetToolTip(this.textBoxIntensityLargeDelta, "When using Ctrl-Up or Ctrl-Down to adjust the intensity\r\nof a selection.");
            // 
            // checkBoxShowNaturalChannelNumber
            // 
            this.checkBoxShowNaturalChannelNumber.AutoSize = true;
            this.checkBoxShowNaturalChannelNumber.Location = new Point(9, 133);
            this.checkBoxShowNaturalChannelNumber.Name = "checkBoxShowNaturalChannelNumber";
            this.checkBoxShowNaturalChannelNumber.Size = new Size(167, 17);
            this.checkBoxShowNaturalChannelNumber.TabIndex = 9;
            this.checkBoxShowNaturalChannelNumber.Text = "Show natural channel number";
            this.toolTip.SetToolTip(this.checkBoxShowNaturalChannelNumber, "Show the channel numbers according to the order they were created in");
            this.checkBoxShowNaturalChannelNumber.UseVisualStyleBackColor = true;
            // 
            // checkBoxFlipMouseScroll
            // 
            this.checkBoxFlipMouseScroll.AutoSize = true;
            this.checkBoxFlipMouseScroll.Location = new Point(9, 179);
            this.checkBoxFlipMouseScroll.Name = "checkBoxFlipMouseScroll";
            this.checkBoxFlipMouseScroll.Size = new Size(180, 17);
            this.checkBoxFlipMouseScroll.TabIndex = 10;
            this.checkBoxFlipMouseScroll.Text = "Flip mouse scroll + Shift behavior";
            this.toolTip.SetToolTip(this.checkBoxFlipMouseScroll, "The default behavior scrolls horizontally when Shift is down.\r\nSelect this to mak" +
                    "e it scroll vertically when Shift is down.");
            this.checkBoxFlipMouseScroll.UseVisualStyleBackColor = true;
            // 
            // textBoxDefaultSequenceSaveDirectory
            // 
            this.textBoxDefaultSequenceSaveDirectory.Location = new Point(6, 226);
            this.textBoxDefaultSequenceSaveDirectory.Name = "textBoxDefaultSequenceSaveDirectory";
            this.textBoxDefaultSequenceSaveDirectory.Size = new Size(414, 20);
            this.textBoxDefaultSequenceSaveDirectory.TabIndex = 12;
            this.toolTip.SetToolTip(this.textBoxDefaultSequenceSaveDirectory, "Application default is My Documents\\Vixen\\Sequences");
            // 
            // checkBoxWizardForNewSequences
            // 
            this.checkBoxWizardForNewSequences.AutoSize = true;
            this.checkBoxWizardForNewSequences.Location = new Point(9, 206);
            this.checkBoxWizardForNewSequences.Name = "checkBoxWizardForNewSequences";
            this.checkBoxWizardForNewSequences.Size = new Size(316, 17);
            this.checkBoxWizardForNewSequences.TabIndex = 2;
            this.checkBoxWizardForNewSequences.Text = "Use the sequence wizard for new sequences, when available";
            this.toolTip.SetToolTip(this.checkBoxWizardForNewSequences, "Use the sequence wizard for new sequences, when the editor allows for a wizard to" +
                    " be used");
            this.checkBoxWizardForNewSequences.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new Point(17, 25);
            this.label12.Name = "label12";
            this.label12.Size = new Size(99, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "Event period length";
            this.toolTip.SetToolTip(this.label12, "The length of a single event period (in milliseconds)");
            // 
            // textBoxEventPeriod
            // 
            this.textBoxEventPeriod.Location = new Point(215, 22);
            this.textBoxEventPeriod.Name = "textBoxEventPeriod";
            this.textBoxEventPeriod.Size = new Size(50, 20);
            this.textBoxEventPeriod.TabIndex = 1;
            this.toolTip.SetToolTip(this.textBoxEventPeriod, "The length of a single event period (in milliseconds)");
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new Point(17, 50);
            this.label14.Name = "label14";
            this.label14.Size = new Size(163, 13);
            this.label14.TabIndex = 3;
            this.label14.Text = "Minimum illumination level (0-255)";
            this.toolTip.SetToolTip(this.label14, "Minimum illumination level allowed by a sequence");
            // 
            // numericUpDownMinimumLevel
            // 
            this.numericUpDownMinimumLevel.Location = new Point(215, 48);
            this.numericUpDownMinimumLevel.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownMinimumLevel.Name = "numericUpDownMinimumLevel";
            this.numericUpDownMinimumLevel.Size = new Size(50, 20);
            this.numericUpDownMinimumLevel.TabIndex = 4;
            this.toolTip.SetToolTip(this.numericUpDownMinimumLevel, "Minimum illumination level allowed by a sequence");
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new Point(17, 76);
            this.label17.Name = "label17";
            this.label17.Size = new Size(166, 13);
            this.label17.TabIndex = 6;
            this.label17.Text = "Maximum illumination level (0-255)";
            this.toolTip.SetToolTip(this.label17, "Maximum illumination level allowed by a sequence");
            // 
            // numericUpDownMaximumLevel
            // 
            this.numericUpDownMaximumLevel.Location = new Point(215, 74);
            this.numericUpDownMaximumLevel.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownMaximumLevel.Name = "numericUpDownMaximumLevel";
            this.numericUpDownMaximumLevel.Size = new Size(50, 20);
            this.numericUpDownMaximumLevel.TabIndex = 7;
            this.toolTip.SetToolTip(this.numericUpDownMaximumLevel, "Maximum illumination level allowed by a sequence");
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new Point(6, 25);
            this.label22.Name = "label22";
            this.label22.Size = new Size(72, 13);
            this.label22.TabIndex = 15;
            this.label22.Text = "Default profile";
            this.toolTip.SetToolTip(this.label22, "Profile that will be used for new sequences and\r\nfor external clients.");
            // 
            // comboBoxDefaultProfile
            // 
            this.comboBoxDefaultProfile.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxDefaultProfile.DropDownWidth = 150;
            this.comboBoxDefaultProfile.FormattingEnabled = true;
            this.comboBoxDefaultProfile.Location = new Point(84, 22);
            this.comboBoxDefaultProfile.Name = "comboBoxDefaultProfile";
            this.comboBoxDefaultProfile.Size = new Size(192, 21);
            this.comboBoxDefaultProfile.TabIndex = 16;
            this.toolTip.SetToolTip(this.comboBoxDefaultProfile, "Profile that will be used for new sequences and");
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new Point(6, 49);
            this.label27.Name = "label27";
            this.label27.Size = new Size(155, 13);
            this.label27.TabIndex = 18;
            this.label27.Text = "Default sequence audio device";
            this.toolTip.SetToolTip(this.label27, "Audio device that will be selected for a new sequence");
            // 
            // comboBoxDefaultAudioDevice
            // 
            this.comboBoxDefaultAudioDevice.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxDefaultAudioDevice.FormattingEnabled = true;
            this.comboBoxDefaultAudioDevice.Location = new Point(167, 46);
            this.comboBoxDefaultAudioDevice.Name = "comboBoxDefaultAudioDevice";
            this.comboBoxDefaultAudioDevice.Size = new Size(204, 21);
            this.comboBoxDefaultAudioDevice.TabIndex = 19;
            this.toolTip.SetToolTip(this.comboBoxDefaultAudioDevice, "Audio device that will be selected for a new sequence");
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxMouseWheelHorizontal);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBoxMouseWheelVertical);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(432, 45);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mouse Wheel";
            this.toolTip.SetToolTip(this.groupBox2, "Mouse wheel increments");
            // 
            // textBoxMouseWheelHorizontal
            // 
            this.textBoxMouseWheelHorizontal.Location = new Point(296, 13);
            this.textBoxMouseWheelHorizontal.Name = "textBoxMouseWheelHorizontal";
            this.textBoxMouseWheelHorizontal.Size = new Size(50, 20);
            this.textBoxMouseWheelHorizontal.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new Point(212, 16);
            this.label4.Name = "label4";
            this.label4.Size = new Size(84, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Event increment";
            this.toolTip.SetToolTip(this.label4, "How many event periods a single mouse wheel movement scrolls through");
            // 
            // textBoxMouseWheelVertical
            // 
            this.textBoxMouseWheelVertical.Location = new Point(118, 13);
            this.textBoxMouseWheelVertical.Name = "textBoxMouseWheelVertical";
            this.textBoxMouseWheelVertical.Size = new Size(50, 20);
            this.textBoxMouseWheelVertical.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new Point(17, 16);
            this.label3.Name = "label3";
            this.label3.Size = new Size(95, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Channel increment";
            this.toolTip.SetToolTip(this.label3, "How many channels a single mouse wheel movement scrolls through");
            // 
            // labelAutoShutdownTime
            // 
            this.labelAutoShutdownTime.AutoSize = true;
            this.labelAutoShutdownTime.Location = new Point(17, 158);
            this.labelAutoShutdownTime.Name = "labelAutoShutdownTime";
            this.labelAutoShutdownTime.Size = new Size(78, 13);
            this.labelAutoShutdownTime.TabIndex = 69;
            this.labelAutoShutdownTime.Text = "Auto shutdown";
            this.toolTip.SetToolTip(this.labelAutoShutdownTime, "If the application is running, it can shut down your computer at a time you speci" +
                    "fy");
            // 
            // dateTimePickerAutoShutdownTime
            // 
            this.dateTimePickerAutoShutdownTime.Checked = false;
            this.dateTimePickerAutoShutdownTime.CustomFormat = "  hh:mm tt";
            this.dateTimePickerAutoShutdownTime.Format = DateTimePickerFormat.Custom;
            this.dateTimePickerAutoShutdownTime.Location = new Point(179, 151);
            this.dateTimePickerAutoShutdownTime.Name = "dateTimePickerAutoShutdownTime";
            this.dateTimePickerAutoShutdownTime.ShowCheckBox = true;
            this.dateTimePickerAutoShutdownTime.ShowUpDown = true;
            this.dateTimePickerAutoShutdownTime.Size = new Size(117, 20);
            this.dateTimePickerAutoShutdownTime.TabIndex = 71;
            this.toolTip.SetToolTip(this.dateTimePickerAutoShutdownTime, "If the application is running, it can shut down your computer at a time you speci" +
                    "fy");
            this.dateTimePickerAutoShutdownTime.Value = new DateTime(2014, 2, 2, 23, 0, 0, 0);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new Point(17, 22);
            this.label26.Name = "label26";
            this.label26.Size = new Size(129, 13);
            this.label26.TabIndex = 72;
            this.label26.Text = "Update Check Frequency";
            this.toolTip.SetToolTip(this.label26, "Stops the application from trying to update itself over the internet.");
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new Point(17, 124);
            this.label36.Name = "label36";
            this.label36.Size = new Size(95, 13);
            this.label36.TabIndex = 78;
            this.label36.Text = "Auto save toolbars";
            this.toolTip.SetToolTip(this.label36, "Sends a blank event to the plugins in the default plugin setup.\r\nUseful for paral" +
                    "lel port-based controllers.  Does not affect every\r\ncontroller type.");
            // 
            // cbToolbarAutoSave
            // 
            this.cbToolbarAutoSave.AutoSize = true;
            this.cbToolbarAutoSave.Location = new Point(179, 124);
            this.cbToolbarAutoSave.Name = "cbToolbarAutoSave";
            this.cbToolbarAutoSave.Size = new Size(15, 14);
            this.cbToolbarAutoSave.TabIndex = 79;
            this.toolTip.SetToolTip(this.cbToolbarAutoSave, "Sends a blank event to the plugins in the default plugin setup.\r\nUseful for paral" +
                    "lel port-based controllers.  Does not affect every\r\ncontroller type.");
            this.cbToolbarAutoSave.UseVisualStyleBackColor = true;
            // 
            // cbUpdateFrequency
            // 
            this.cbUpdateFrequency.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbUpdateFrequency.FormattingEnabled = true;
            this.cbUpdateFrequency.Items.AddRange(new object[] {
            "On Startup",
            "Daily",
            "Weekly",
            "Monthly",
            "Quarterly",
            "Annually",
            "Never"});
            this.cbUpdateFrequency.Location = new Point(179, 19);
            this.cbUpdateFrequency.Name = "cbUpdateFrequency";
            this.cbUpdateFrequency.Size = new Size(117, 21);
            this.cbUpdateFrequency.TabIndex = 80;
            this.toolTip.SetToolTip(this.cbUpdateFrequency, "How often will Vixen+ check for updates.");
            // 
            // cbProfileErrors
            // 
            this.cbProfileErrors.AutoSize = true;
            this.cbProfileErrors.Location = new Point(240, 64);
            this.cbProfileErrors.Name = "cbProfileErrors";
            this.cbProfileErrors.Size = new Size(150, 17);
            this.cbProfileErrors.TabIndex = 14;
            this.cbProfileErrors.Text = "Silence profile editor errors";
            this.toolTip.SetToolTip(this.cbProfileErrors, "The default behavior scrolls horizontally when Shift is down.\r\nSelect this to mak" +
                    "e it scroll vertically when Shift is down.");
            this.cbProfileErrors.UseVisualStyleBackColor = true;
            // 
            // cbChannelHighlight
            // 
            this.cbChannelHighlight.AutoSize = true;
            this.cbChannelHighlight.Location = new Point(26, 119);
            this.cbChannelHighlight.Name = "cbChannelHighlight";
            this.cbChannelHighlight.Size = new Size(145, 17);
            this.cbChannelHighlight.TabIndex = 4;
            this.cbChannelHighlight.Text = "Enable Channel Highlight";
            this.toolTip.SetToolTip(this.cbChannelHighlight, "Channels are highlighted upon playback in the editor.");
            this.cbChannelHighlight.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(465, 328);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(75, 23);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(546, 328);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
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
            this.btnSetDataFolder.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Left)));
            this.btnSetDataFolder.Location = new Point(12, 328);
            this.btnSetDataFolder.Name = "btnSetDataFolder";
            this.btnSetDataFolder.Size = new Size(161, 23);
            this.btnSetDataFolder.TabIndex = 5;
            this.btnSetDataFolder.Text = "Set Data Folder";
            this.btnSetDataFolder.UseVisualStyleBackColor = true;
            this.btnSetDataFolder.Click += new EventHandler(this.btnSetDataFolder_Click);
            // 
            // sequenceExecutionTab
            // 
            this.sequenceExecutionTab.BackColor = Color.Transparent;
            this.sequenceExecutionTab.Controls.Add(this.groupBox4);
            this.sequenceExecutionTab.Location = new Point(0, 0);
            this.sequenceExecutionTab.Name = "sequenceExecutionTab";
            this.sequenceExecutionTab.Size = new Size(446, 310);
            this.sequenceExecutionTab.TabIndex = 2;
            this.sequenceExecutionTab.Text = "sequenceExecutionTab";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cbChannelHighlight);
            this.groupBox4.Controls.Add(this.checkBoxClearAtEndOfSequence);
            this.groupBox4.Controls.Add(this.checkBoxShowPositionMarker);
            this.groupBox4.Controls.Add(this.checkBoxSavePlugInDialogPositions);
            this.groupBox4.Controls.Add(this.checkBoxAutoScrolling);
            this.groupBox4.Location = new Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new Size(432, 147);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Sequence Execution";
            // 
            // sequenceEditingTab
            // 
            this.sequenceEditingTab.BackColor = Color.Transparent;
            this.sequenceEditingTab.Controls.Add(this.groupBox5);
            this.sequenceEditingTab.Location = new Point(0, 0);
            this.sequenceEditingTab.Name = "sequenceEditingTab";
            this.sequenceEditingTab.Size = new Size(446, 310);
            this.sequenceEditingTab.TabIndex = 1;
            this.sequenceEditingTab.Text = "sequenceEditingTab";
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left)
                        | AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.cbProfileErrors);
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
            this.groupBox5.Location = new Point(3, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new Size(432, 256);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Sequence Editing";
            // 
            // cbWavefromZeroLine
            // 
            this.cbWavefromZeroLine.AutoSize = true;
            this.cbWavefromZeroLine.Location = new Point(9, 156);
            this.cbWavefromZeroLine.Name = "cbWavefromZeroLine";
            this.cbWavefromZeroLine.Size = new Size(147, 17);
            this.cbWavefromZeroLine.TabIndex = 13;
            this.cbWavefromZeroLine.Text = "Show Wavform Zero Line";
            this.cbWavefromZeroLine.UseVisualStyleBackColor = true;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new Point(6, 210);
            this.label34.Name = "label34";
            this.label34.Size = new Size(302, 13);
            this.label34.TabIndex = 11;
            this.label34.Text = "Default sequence save directory: (blank for application default)";
            // 
            // newSequenceSettingsTab
            // 
            this.newSequenceSettingsTab.BackColor = Color.Transparent;
            this.newSequenceSettingsTab.Controls.Add(this.groupBox12);
            this.newSequenceSettingsTab.Controls.Add(this.groupBox8);
            this.newSequenceSettingsTab.Controls.Add(this.checkBoxWizardForNewSequences);
            this.newSequenceSettingsTab.Location = new Point(0, 0);
            this.newSequenceSettingsTab.Name = "newSequenceSettingsTab";
            this.newSequenceSettingsTab.Size = new Size(446, 310);
            this.newSequenceSettingsTab.TabIndex = 0;
            this.newSequenceSettingsTab.Text = "newSequenceSettingsTab";
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.comboBoxDefaultAudioDevice);
            this.groupBox12.Controls.Add(this.label27);
            this.groupBox12.Controls.Add(this.comboBoxDefaultProfile);
            this.groupBox12.Controls.Add(this.label22);
            this.groupBox12.Location = new Point(3, 118);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new Size(432, 85);
            this.groupBox12.TabIndex = 1;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Defaults";
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
            this.groupBox8.Location = new Point(3, 3);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new Size(432, 109);
            this.groupBox8.TabIndex = 0;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Editing Grid";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new Point(271, 25);
            this.label13.Name = "label13";
            this.label13.Size = new Size(63, 13);
            this.label13.TabIndex = 2;
            this.label13.Text = "milliseconds";
            // 
            // screenTab
            // 
            this.screenTab.Controls.Add(this.cbUseCheckmark);
            this.screenTab.Controls.Add(this.gbColors);
            this.screenTab.Controls.Add(this.lblPrimaryScreen);
            this.screenTab.Controls.Add(this.cbScreens);
            this.screenTab.Location = new Point(0, 0);
            this.screenTab.Name = "screenTab";
            this.screenTab.Size = new Size(446, 310);
            this.screenTab.TabIndex = 7;
            this.screenTab.Text = "screenTab";
            this.screenTab.UseVisualStyleBackColor = true;
            // 
            // cbUseCheckmark
            // 
            this.cbUseCheckmark.AutoSize = true;
            this.cbUseCheckmark.Checked = true;
            this.cbUseCheckmark.CheckState = CheckState.Checked;
            this.cbUseCheckmark.Location = new Point(6, 147);
            this.cbUseCheckmark.Name = "cbUseCheckmark";
            this.cbUseCheckmark.Size = new Size(193, 17);
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
            this.gbColors.Location = new Point(6, 32);
            this.gbColors.Name = "gbColors";
            this.gbColors.Size = new Size(429, 109);
            this.gbColors.TabIndex = 3;
            this.gbColors.TabStop = false;
            this.gbColors.Text = "Customizable Colors";
            // 
            // lblClickToUpdate
            // 
            this.lblClickToUpdate.AutoSize = true;
            this.lblClickToUpdate.Location = new Point(281, 88);
            this.lblClickToUpdate.Name = "lblClickToUpdate";
            this.lblClickToUpdate.Size = new Size(104, 13);
            this.lblClickToUpdate.TabIndex = 5;
            this.lblClickToUpdate.Text = "Click color to update";
            // 
            // lblCurrentColor
            // 
            this.lblCurrentColor.AutoSize = true;
            this.lblCurrentColor.Location = new Point(281, 19);
            this.lblCurrentColor.Name = "lblCurrentColor";
            this.lblCurrentColor.Size = new Size(71, 13);
            this.lblCurrentColor.TabIndex = 4;
            this.lblCurrentColor.Text = "Current Color:";
            // 
            // pbColor
            // 
            this.pbColor.BorderStyle = BorderStyle.FixedSingle;
            this.pbColor.Location = new Point(281, 35);
            this.pbColor.Name = "pbColor";
            this.pbColor.Size = new Size(100, 50);
            this.pbColor.TabIndex = 3;
            this.pbColor.TabStop = false;
            this.pbColor.Click += new EventHandler(this.pbColor_Click);
            // 
            // lbColorizableItems
            // 
            this.lbColorizableItems.FormattingEnabled = true;
            this.lbColorizableItems.Location = new Point(6, 19);
            this.lbColorizableItems.Name = "lbColorizableItems";
            this.lbColorizableItems.Size = new Size(269, 82);
            this.lbColorizableItems.TabIndex = 2;
            this.lbColorizableItems.SelectedIndexChanged += new EventHandler(this.lbColorizableItems_SelectedIndexChanged);
            this.lbColorizableItems.DoubleClick += new EventHandler(this.pbColor_Click);
            // 
            // lblPrimaryScreen
            // 
            this.lblPrimaryScreen.AutoSize = true;
            this.lblPrimaryScreen.Location = new Point(3, 11);
            this.lblPrimaryScreen.Name = "lblPrimaryScreen";
            this.lblPrimaryScreen.Size = new Size(81, 13);
            this.lblPrimaryScreen.TabIndex = 1;
            this.lblPrimaryScreen.Text = "Primary Screen:";
            // 
            // cbScreens
            // 
            this.cbScreens.FormattingEnabled = true;
            this.cbScreens.Location = new Point(90, 3);
            this.cbScreens.Name = "cbScreens";
            this.cbScreens.Size = new Size(345, 21);
            this.cbScreens.TabIndex = 0;
            // 
            // generalTab
            // 
            this.generalTab.BackColor = Color.Transparent;
            this.generalTab.Controls.Add(this.groupBox3);
            this.generalTab.Controls.Add(this.groupBox2);
            this.generalTab.Location = new Point(0, 0);
            this.generalTab.Name = "generalTab";
            this.generalTab.Size = new Size(446, 310);
            this.generalTab.TabIndex = 3;
            this.generalTab.Text = "generalTab";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbUpdateFrequency);
            this.groupBox3.Controls.Add(this.cbToolbarAutoSave);
            this.groupBox3.Controls.Add(this.label36);
            this.groupBox3.Controls.Add(this.nudRecentFiles);
            this.groupBox3.Controls.Add(this.label35);
            this.groupBox3.Controls.Add(this.numericUpDownHistoryImages);
            this.groupBox3.Controls.Add(this.label28);
            this.groupBox3.Controls.Add(this.label26);
            this.groupBox3.Controls.Add(this.dateTimePickerAutoShutdownTime);
            this.groupBox3.Controls.Add(this.labelAutoShutdownTime);
            this.groupBox3.Location = new Point(3, 54);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(432, 211);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Miscellaneous";
            // 
            // nudRecentFiles
            // 
            this.nudRecentFiles.Location = new Point(179, 88);
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
            this.nudRecentFiles.Size = new Size(117, 20);
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
            this.label35.Location = new Point(17, 90);
            this.label35.Name = "label35";
            this.label35.Size = new Size(91, 13);
            this.label35.TabIndex = 76;
            this.label35.Text = "Recent file history";
            // 
            // numericUpDownHistoryImages
            // 
            this.numericUpDownHistoryImages.Location = new Point(179, 54);
            this.numericUpDownHistoryImages.Name = "numericUpDownHistoryImages";
            this.numericUpDownHistoryImages.Size = new Size(117, 20);
            this.numericUpDownHistoryImages.TabIndex = 75;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new Point(17, 56);
            this.label28.Name = "label28";
            this.label28.Size = new Size(113, 13);
            this.label28.TabIndex = 74;
            this.label28.Text = "Backup history images";
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
                        | AnchorStyles.Left)
                        | AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.generalTab);
            this.tabControl.Controls.Add(this.screenTab);
            this.tabControl.Controls.Add(this.newSequenceSettingsTab);
            this.tabControl.Controls.Add(this.sequenceEditingTab);
            this.tabControl.Controls.Add(this.sequenceExecutionTab);
            this.tabControl.HideTabs = true;
            this.tabControl.Location = new Point(179, 12);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.OurMultiline = true;
            this.tabControl.SelectedIndex = 0;
            this.tabControl.ShowToolTips = true;
            this.tabControl.Size = new Size(446, 310);
            this.tabControl.TabIndex = 1;
            this.tabControl.Selected += new TabControlEventHandler(this.tabControl_Selected);
            this.tabControl.Deselecting += new TabControlCancelEventHandler(this.tabControl_Deselecting);
            // 
            // PreferencesDialog
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(637, 358);
            this.Controls.Add(this.btnSetDataFolder);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.treeView);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PreferencesDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Preferences";
            ((ISupportInitialize)(this.numericUpDownMinimumLevel)).EndInit();
            ((ISupportInitialize)(this.numericUpDownMaximumLevel)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.sequenceExecutionTab.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.sequenceEditingTab.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.newSequenceSettingsTab.ResumeLayout(false);
            this.newSequenceSettingsTab.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.screenTab.ResumeLayout(false);
            this.screenTab.PerformLayout();
            this.gbColors.ResumeLayout(false);
            this.gbColors.PerformLayout();
            ((ISupportInitialize)(this.pbColor)).EndInit();
            this.generalTab.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((ISupportInitialize)(this.nudRecentFiles)).EndInit();
            ((ISupportInitialize)(this.numericUpDownHistoryImages)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }
        private ColorDialog colorDialog1;
        private Button btnSetDataFolder;
        private TabPage sequenceExecutionTab;
        private GroupBox groupBox4;
        private CheckBox checkBoxClearAtEndOfSequence;
        private CheckBox checkBoxShowPositionMarker;
        private CheckBox checkBoxSavePlugInDialogPositions;
        private CheckBox checkBoxAutoScrolling;
        private TabPage sequenceEditingTab;
        private GroupBox groupBox5;
        private CheckBox cbWavefromZeroLine;
        private TextBox textBoxDefaultSequenceSaveDirectory;
        private Label label34;
        private CheckBox checkBoxFlipMouseScroll;
        private CheckBox checkBoxShowNaturalChannelNumber;
        private TextBox textBoxIntensityLargeDelta;
        private Label label5;
        private CheckBox checkBoxShowSaveConfirmation;
        private CheckBox checkBoxSaveZoomLevels;
        private CheckBox checkBoxEventSequenceAutoSize;
        private TextBox textBoxMaxRowHeight;
        private Label label7;
        private TextBox textBoxMaxColumnWidth;
        private Label label6;
        private TabPage newSequenceSettingsTab;
        private GroupBox groupBox12;
        private ComboBox comboBoxDefaultAudioDevice;
        private Label label27;
        private ComboBox comboBoxDefaultProfile;
        private Label label22;
        private GroupBox groupBox8;
        private NumericUpDown numericUpDownMaximumLevel;
        private Label label17;
        private NumericUpDown numericUpDownMinimumLevel;
        private Label label14;
        private Label label13;
        private TextBox textBoxEventPeriod;
        private Label label12;
        private CheckBox checkBoxWizardForNewSequences;
        private TabPage screenTab;
        private CheckBox cbUseCheckmark;
        private GroupBox gbColors;
        private Label lblClickToUpdate;
        private Label lblCurrentColor;
        private PictureBox pbColor;
        private ListBox lbColorizableItems;
        private Label lblPrimaryScreen;
        private ComboBox cbScreens;
        private TabPage generalTab;
        private GroupBox groupBox3;
        private CheckBox cbToolbarAutoSave;
        private Label label36;
        private NumericUpDown nudRecentFiles;
        private Label label35;
        private NumericUpDown numericUpDownHistoryImages;
        private Label label28;
        private Label label26;
        private DateTimePicker dateTimePickerAutoShutdownTime;
        private Label labelAutoShutdownTime;
        private GroupBox groupBox2;
        private TextBox textBoxMouseWheelHorizontal;
        private Label label4;
        private TextBox textBoxMouseWheelVertical;
        private Label label3;
        private TabControl tabControl;
        private ComboBox cbUpdateFrequency;
        private CheckBox cbProfileErrors;
        private CheckBox cbChannelHighlight;
    }
}
