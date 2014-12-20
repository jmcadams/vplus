using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenPlus.Dialogs {
    public partial class AudioDialog {
        private IContainer components;

        #region Windows Form Designer generated code

        private Button buttonCancel;
        private Button buttonClear;
        private Button buttonLoad;
        private Button buttonOK;
        private Button buttonPlayPause;
        private Button buttonRemoveAudio;
        private Button buttonStop;
        private ToolStripComboBox channel0ToolStripMenuItem;
        private ToolStripComboBox channel1ToolStripMenuItem;
        private ToolStripComboBox channel2ToolStripMenuItem;
        private ToolStripComboBox channel3ToolStripMenuItem;
        private ToolStripComboBox channel4ToolStripMenuItem;
        private ToolStripComboBox channel5ToolStripMenuItem;
        private ToolStripComboBox channel6ToolStripMenuItem;
        private ToolStripComboBox channel7ToolStripMenuItem;
        private ToolStripComboBox channel8ToolStripMenuItem;
        private ToolStripComboBox channel9ToolStripMenuItem;
        private CheckBox checkBoxAutoSize;
        private ComboBox comboBoxAudioDevice;
        private ContextMenuStrip contextMenuStrip;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Label label1;
        private Label label2;
        private Label labelAudioFileName;
        private Label labelAudioLength;
        private Label labelAudioName;
        private Label labelTime;
        private Label labelTotalTime;
        private LinkLabel linkLabelAssignedKeys;
        private ListBox listBoxChannels;
        private OpenFileDialog openFileDialog1;
        private PictureBox pictureBoxPause;
        private PictureBox pictureBoxPlay;
        private PictureBox pictureBoxPlayBlue;
        private ProgressBar progressBarCountdown;
        private RadioButton radioButtonMultipleEvents;
        private RadioButton radioButtonSingleEvent;
        private ToolStripMenuItem toolStripMenuItem10;
        private ToolStripMenuItem toolStripMenuItem11;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem toolStripMenuItem5;
        private ToolStripMenuItem toolStripMenuItem6;
        private ToolStripMenuItem toolStripMenuItem7;
        private ToolStripMenuItem toolStripMenuItem8;
        private ToolStripMenuItem toolStripMenuItem9;
        private TrackBar trackBarPosition;


        private void InitializeComponent() {
            this.components = new Container();
            this.openFileDialog1 = new OpenFileDialog();
            this.groupBox1 = new GroupBox();
            this.comboBoxAudioDevice = new ComboBox();
            this.label2 = new Label();
            this.checkBoxAutoSize = new CheckBox();
            this.buttonRemoveAudio = new Button();
            this.labelAudioLength = new Label();
            this.labelAudioFileName = new Label();
            this.labelAudioName = new Label();
            this.buttonLoad = new Button();
            this.groupBox2 = new GroupBox();
            this.trackBarPosition = new TrackBar();
            this.progressBarCountdown = new ProgressBar();
            this.linkLabelAssignedKeys = new LinkLabel();
            this.pictureBoxPlay = new PictureBox();
            this.pictureBoxPlayBlue = new PictureBox();
            this.pictureBoxPause = new PictureBox();
            this.labelTotalTime = new Label();
            this.labelTime = new Label();
            this.buttonClear = new Button();
            this.buttonStop = new Button();
            this.buttonPlayPause = new Button();
            this.label1 = new Label();
            this.listBoxChannels = new ListBox();
            this.contextMenuStrip = new ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new ToolStripMenuItem();
            this.channel1ToolStripMenuItem = new ToolStripComboBox();
            this.toolStripMenuItem3 = new ToolStripMenuItem();
            this.channel2ToolStripMenuItem = new ToolStripComboBox();
            this.toolStripMenuItem4 = new ToolStripMenuItem();
            this.channel3ToolStripMenuItem = new ToolStripComboBox();
            this.toolStripMenuItem5 = new ToolStripMenuItem();
            this.channel4ToolStripMenuItem = new ToolStripComboBox();
            this.toolStripMenuItem6 = new ToolStripMenuItem();
            this.channel5ToolStripMenuItem = new ToolStripComboBox();
            this.toolStripMenuItem7 = new ToolStripMenuItem();
            this.channel6ToolStripMenuItem = new ToolStripComboBox();
            this.toolStripMenuItem8 = new ToolStripMenuItem();
            this.channel7ToolStripMenuItem = new ToolStripComboBox();
            this.toolStripMenuItem9 = new ToolStripMenuItem();
            this.channel8ToolStripMenuItem = new ToolStripComboBox();
            this.toolStripMenuItem10 = new ToolStripMenuItem();
            this.channel9ToolStripMenuItem = new ToolStripComboBox();
            this.toolStripMenuItem11 = new ToolStripMenuItem();
            this.channel0ToolStripMenuItem = new ToolStripComboBox();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.groupBox3 = new GroupBox();
            this.radioButtonMultipleEvents = new RadioButton();
            this.radioButtonSingleEvent = new RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((ISupportInitialize)(this.trackBarPosition)).BeginInit();
            ((ISupportInitialize)(this.pictureBoxPlay)).BeginInit();
            ((ISupportInitialize)(this.pictureBoxPlayBlue)).BeginInit();
            ((ISupportInitialize)(this.pictureBoxPause)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "wma";
            this.openFileDialog1.Filter = "Windows Media Audio | *.wma";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left)
                        | AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.comboBoxAudioDevice);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.checkBoxAutoSize);
            this.groupBox1.Controls.Add(this.buttonRemoveAudio);
            this.groupBox1.Controls.Add(this.labelAudioLength);
            this.groupBox1.Controls.Add(this.labelAudioFileName);
            this.groupBox1.Controls.Add(this.labelAudioName);
            this.groupBox1.Controls.Add(this.buttonLoad);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(420, 171);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select audio";
            // 
            // comboBoxAudioDevice
            // 
            this.comboBoxAudioDevice.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxAudioDevice.FormattingEnabled = true;
            this.comboBoxAudioDevice.Location = new Point(132, 135);
            this.comboBoxAudioDevice.Name = "comboBoxAudioDevice";
            this.comboBoxAudioDevice.Size = new Size(206, 21);
            this.comboBoxAudioDevice.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(57, 138);
            this.label2.Name = "label2";
            this.label2.Size = new Size(69, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Audio device";
            // 
            // checkBoxAutoSize
            // 
            this.checkBoxAutoSize.AutoSize = true;
            this.checkBoxAutoSize.CheckAlign = ContentAlignment.TopLeft;
            this.checkBoxAutoSize.Location = new Point(30, 28);
            this.checkBoxAutoSize.Name = "checkBoxAutoSize";
            this.checkBoxAutoSize.Size = new Size(217, 17);
            this.checkBoxAutoSize.TabIndex = 0;
            this.checkBoxAutoSize.Text = "Resize the sequence to the audio length";
            this.checkBoxAutoSize.UseVisualStyleBackColor = true;
            this.checkBoxAutoSize.CheckedChanged += new EventHandler(this.checkBoxAutoSize_CheckedChanged);
            // 
            // buttonRemoveAudio
            // 
            this.buttonRemoveAudio.Enabled = false;
            this.buttonRemoveAudio.Location = new Point(30, 95);
            this.buttonRemoveAudio.Name = "buttonRemoveAudio";
            this.buttonRemoveAudio.Size = new Size(96, 23);
            this.buttonRemoveAudio.TabIndex = 2;
            this.buttonRemoveAudio.Text = "Remove audio";
            this.buttonRemoveAudio.UseVisualStyleBackColor = true;
            this.buttonRemoveAudio.Click += new EventHandler(this.buttonRemoveAudio_Click);
            // 
            // labelAudioLength
            // 
            this.labelAudioLength.AutoSize = true;
            this.labelAudioLength.Location = new Point(132, 105);
            this.labelAudioLength.Name = "labelAudioLength";
            this.labelAudioLength.Size = new Size(36, 13);
            this.labelAudioLength.TabIndex = 5;
            this.labelAudioLength.Text = "length";
            // 
            // labelAudioFileName
            // 
            this.labelAudioFileName.AutoSize = true;
            this.labelAudioFileName.Location = new Point(132, 66);
            this.labelAudioFileName.Name = "labelAudioFileName";
            this.labelAudioFileName.Size = new Size(46, 13);
            this.labelAudioFileName.TabIndex = 3;
            this.labelAudioFileName.Text = "filename";
            // 
            // labelAudioName
            // 
            this.labelAudioName.AutoSize = true;
            this.labelAudioName.Location = new Point(132, 85);
            this.labelAudioName.Name = "labelAudioName";
            this.labelAudioName.Size = new Size(33, 13);
            this.labelAudioName.TabIndex = 4;
            this.labelAudioName.Text = "name";
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new Point(30, 66);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new Size(96, 23);
            this.buttonLoad.TabIndex = 1;
            this.buttonLoad.Text = "Assign audio";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new EventHandler(this.buttonLoad_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((AnchorStyles)(((AnchorStyles.Bottom | AnchorStyles.Left)
                        | AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.trackBarPosition);
            this.groupBox2.Controls.Add(this.progressBarCountdown);
            this.groupBox2.Controls.Add(this.linkLabelAssignedKeys);
            this.groupBox2.Controls.Add(this.pictureBoxPlay);
            this.groupBox2.Controls.Add(this.pictureBoxPlayBlue);
            this.groupBox2.Controls.Add(this.pictureBoxPause);
            this.groupBox2.Controls.Add(this.labelTotalTime);
            this.groupBox2.Controls.Add(this.labelTime);
            this.groupBox2.Controls.Add(this.buttonClear);
            this.groupBox2.Controls.Add(this.buttonStop);
            this.groupBox2.Controls.Add(this.buttonPlayPause);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.listBoxChannels);
            this.groupBox2.Location = new Point(12, 312);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(420, 225);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Channel events";
            // 
            // trackBarPosition
            // 
            this.trackBarPosition.LargeChange = 30;
            this.trackBarPosition.Location = new Point(131, 177);
            this.trackBarPosition.Name = "trackBarPosition";
            this.trackBarPosition.Size = new Size(275, 45);
            this.trackBarPosition.TabIndex = 19;
            this.trackBarPosition.TickStyle = TickStyle.TopLeft;
            this.trackBarPosition.Scroll += new EventHandler(this.trackBarPosition_Scroll);
            // 
            // progressBarCountdown
            // 
            this.progressBarCountdown.Location = new Point(141, 164);
            this.progressBarCountdown.Name = "progressBarCountdown";
            this.progressBarCountdown.RightToLeft = RightToLeft.Yes;
            this.progressBarCountdown.RightToLeftLayout = true;
            this.progressBarCountdown.Size = new Size(265, 13);
            this.progressBarCountdown.TabIndex = 18;
            this.progressBarCountdown.Visible = false;
            // 
            // linkLabelAssignedKeys
            // 
            this.linkLabelAssignedKeys.LinkArea = new LinkArea(11, 11);
            this.linkLabelAssignedKeys.Location = new Point(141, 75);
            this.linkLabelAssignedKeys.Name = "linkLabelAssignedKeys";
            this.linkLabelAssignedKeys.Size = new Size(242, 29);
            this.linkLabelAssignedKeys.TabIndex = 16;
            this.linkLabelAssignedKeys.TabStop = true;
            this.linkLabelAssignedKeys.Text = "Or use the number keys to create the pattern for specific channels simultaneously" +
                ".";
            this.linkLabelAssignedKeys.UseCompatibleTextRendering = true;
            this.linkLabelAssignedKeys.VisitedLinkColor = Color.Blue;
            this.linkLabelAssignedKeys.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabelAssignedKeys_LinkClicked);
            // 
            // pictureBoxPlay
            // 
            this.pictureBoxPlay.Location = new Point(354, 142);
            this.pictureBoxPlay.Name = "pictureBoxPlay";
            this.pictureBoxPlay.Size = new Size(16, 16);
            this.pictureBoxPlay.TabIndex = 15;
            this.pictureBoxPlay.TabStop = false;
            this.pictureBoxPlay.Visible = false;
            // 
            // pictureBoxPlayBlue
            // 
            this.pictureBoxPlayBlue.Location = new Point(332, 142);
            this.pictureBoxPlayBlue.Name = "pictureBoxPlayBlue";
            this.pictureBoxPlayBlue.Size = new Size(16, 16);
            this.pictureBoxPlayBlue.TabIndex = 14;
            this.pictureBoxPlayBlue.TabStop = false;
            this.pictureBoxPlayBlue.Visible = false;
            // 
            // pictureBoxPause
            // 
            this.pictureBoxPause.Location = new Point(310, 142);
            this.pictureBoxPause.Name = "pictureBoxPause";
            this.pictureBoxPause.Size = new Size(16, 16);
            this.pictureBoxPause.TabIndex = 13;
            this.pictureBoxPause.TabStop = false;
            this.pictureBoxPause.Visible = false;
            // 
            // labelTotalTime
            // 
            this.labelTotalTime.AutoSize = true;
            this.labelTotalTime.Location = new Point(191, 147);
            this.labelTotalTime.Name = "labelTotalTime";
            this.labelTotalTime.Size = new Size(63, 13);
            this.labelTotalTime.TabIndex = 7;
            this.labelTotalTime.Text = "/ 00:00.000";
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new Point(138, 147);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new Size(55, 13);
            this.labelTime.TabIndex = 6;
            this.labelTime.Text = "00:00.000";
            // 
            // buttonClear
            // 
            this.buttonClear.Enabled = false;
            this.buttonClear.Location = new Point(303, 116);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new Size(87, 23);
            this.buttonClear.TabIndex = 4;
            this.buttonClear.Text = "Reset channel";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new EventHandler(this.buttonClear_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.AutoSize = true;
            this.buttonStop.Location = new Point(222, 116);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new Size(75, 23);
            this.buttonStop.TabIndex = 3;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new EventHandler(this.buttonStop_Click);
            // 
            // buttonPlayPause
            // 
            this.buttonPlayPause.AutoSize = true;
            this.buttonPlayPause.Location = new Point(141, 116);
            this.buttonPlayPause.Name = "buttonPlayPause";
            this.buttonPlayPause.Size = new Size(75, 23);
            this.buttonPlayPause.TabIndex = 2;
            this.buttonPlayPause.Text = "Play";
            this.buttonPlayPause.UseVisualStyleBackColor = true;
            this.buttonPlayPause.Click += new EventHandler(this.buttonPlayPause_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(138, 26);
            this.label1.Name = "label1";
            this.label1.Size = new Size(225, 39);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select a channel and use the left and/or right\r\nControl keys (CTRL) to create the" +
                " pattern after\r\nclicking the play button.";
            // 
            // listBoxChannels
            // 
            this.listBoxChannels.FormattingEnabled = true;
            this.listBoxChannels.Location = new Point(11, 26);
            this.listBoxChannels.Name = "listBoxChannels";
            this.listBoxChannels.Size = new Size(115, 186);
            this.listBoxChannels.TabIndex = 0;
            this.listBoxChannels.SelectedIndexChanged += new EventHandler(this.listBoxChannels_SelectedIndexChanged);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.toolStripMenuItem7,
            this.toolStripMenuItem8,
            this.toolStripMenuItem9,
            this.toolStripMenuItem10,
            this.toolStripMenuItem11});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new Size(87, 224);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new ToolStripItem[] {
            this.channel1ToolStripMenuItem});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new Size(86, 22);
            this.toolStripMenuItem2.Text = "\'1\'";
            // 
            // channel1ToolStripMenuItem
            // 
            this.channel1ToolStripMenuItem.DropDownStyle = ComboBoxStyle.DropDownList;
            this.channel1ToolStripMenuItem.Name = "channel1ToolStripMenuItem";
            this.channel1ToolStripMenuItem.Size = new Size(152, 23);
            this.channel1ToolStripMenuItem.Tag = "1";
            this.channel1ToolStripMenuItem.SelectedIndexChanged += new EventHandler(this.channelMapItem_SelectedIndexChanged);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.DropDownItems.AddRange(new ToolStripItem[] {
            this.channel2ToolStripMenuItem});
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new Size(86, 22);
            this.toolStripMenuItem3.Text = "\'2\'";
            // 
            // channel2ToolStripMenuItem
            // 
            this.channel2ToolStripMenuItem.DropDownStyle = ComboBoxStyle.DropDownList;
            this.channel2ToolStripMenuItem.Name = "channel2ToolStripMenuItem";
            this.channel2ToolStripMenuItem.Size = new Size(152, 23);
            this.channel2ToolStripMenuItem.Tag = "2";
            this.channel2ToolStripMenuItem.SelectedIndexChanged += new EventHandler(this.channelMapItem_SelectedIndexChanged);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.DropDownItems.AddRange(new ToolStripItem[] {
            this.channel3ToolStripMenuItem});
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new Size(86, 22);
            this.toolStripMenuItem4.Text = "\'3\'";
            // 
            // channel3ToolStripMenuItem
            // 
            this.channel3ToolStripMenuItem.DropDownStyle = ComboBoxStyle.DropDownList;
            this.channel3ToolStripMenuItem.Name = "channel3ToolStripMenuItem";
            this.channel3ToolStripMenuItem.Size = new Size(152, 23);
            this.channel3ToolStripMenuItem.Tag = "3";
            this.channel3ToolStripMenuItem.SelectedIndexChanged += new EventHandler(this.channelMapItem_SelectedIndexChanged);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.DropDownItems.AddRange(new ToolStripItem[] {
            this.channel4ToolStripMenuItem});
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new Size(86, 22);
            this.toolStripMenuItem5.Text = "\'4\'";
            // 
            // channel4ToolStripMenuItem
            // 
            this.channel4ToolStripMenuItem.DropDownStyle = ComboBoxStyle.DropDownList;
            this.channel4ToolStripMenuItem.Name = "channel4ToolStripMenuItem";
            this.channel4ToolStripMenuItem.Size = new Size(152, 23);
            this.channel4ToolStripMenuItem.Tag = "4";
            this.channel4ToolStripMenuItem.SelectedIndexChanged += new EventHandler(this.channelMapItem_SelectedIndexChanged);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.DropDownItems.AddRange(new ToolStripItem[] {
            this.channel5ToolStripMenuItem});
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new Size(86, 22);
            this.toolStripMenuItem6.Text = "\'5\'";
            // 
            // channel5ToolStripMenuItem
            // 
            this.channel5ToolStripMenuItem.DropDownStyle = ComboBoxStyle.DropDownList;
            this.channel5ToolStripMenuItem.Name = "channel5ToolStripMenuItem";
            this.channel5ToolStripMenuItem.Size = new Size(152, 23);
            this.channel5ToolStripMenuItem.Tag = "5";
            this.channel5ToolStripMenuItem.SelectedIndexChanged += new EventHandler(this.channelMapItem_SelectedIndexChanged);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.DropDownItems.AddRange(new ToolStripItem[] {
            this.channel6ToolStripMenuItem});
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new Size(86, 22);
            this.toolStripMenuItem7.Text = "\'6\'";
            // 
            // channel6ToolStripMenuItem
            // 
            this.channel6ToolStripMenuItem.DropDownStyle = ComboBoxStyle.DropDownList;
            this.channel6ToolStripMenuItem.Name = "channel6ToolStripMenuItem";
            this.channel6ToolStripMenuItem.Size = new Size(152, 23);
            this.channel6ToolStripMenuItem.Tag = "6";
            this.channel6ToolStripMenuItem.SelectedIndexChanged += new EventHandler(this.channelMapItem_SelectedIndexChanged);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.DropDownItems.AddRange(new ToolStripItem[] {
            this.channel7ToolStripMenuItem});
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new Size(86, 22);
            this.toolStripMenuItem8.Text = "\'7\'";
            // 
            // channel7ToolStripMenuItem
            // 
            this.channel7ToolStripMenuItem.DropDownStyle = ComboBoxStyle.DropDownList;
            this.channel7ToolStripMenuItem.Name = "channel7ToolStripMenuItem";
            this.channel7ToolStripMenuItem.Size = new Size(152, 23);
            this.channel7ToolStripMenuItem.Tag = "7";
            this.channel7ToolStripMenuItem.SelectedIndexChanged += new EventHandler(this.channelMapItem_SelectedIndexChanged);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.DropDownItems.AddRange(new ToolStripItem[] {
            this.channel8ToolStripMenuItem});
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new Size(86, 22);
            this.toolStripMenuItem9.Text = "\'8\'";
            // 
            // channel8ToolStripMenuItem
            // 
            this.channel8ToolStripMenuItem.DropDownStyle = ComboBoxStyle.DropDownList;
            this.channel8ToolStripMenuItem.Name = "channel8ToolStripMenuItem";
            this.channel8ToolStripMenuItem.Size = new Size(152, 23);
            this.channel8ToolStripMenuItem.Tag = "8";
            this.channel8ToolStripMenuItem.SelectedIndexChanged += new EventHandler(this.channelMapItem_SelectedIndexChanged);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.DropDownItems.AddRange(new ToolStripItem[] {
            this.channel9ToolStripMenuItem});
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new Size(86, 22);
            this.toolStripMenuItem10.Text = "\'9\'";
            // 
            // channel9ToolStripMenuItem
            // 
            this.channel9ToolStripMenuItem.DropDownStyle = ComboBoxStyle.DropDownList;
            this.channel9ToolStripMenuItem.Name = "channel9ToolStripMenuItem";
            this.channel9ToolStripMenuItem.Size = new Size(152, 23);
            this.channel9ToolStripMenuItem.Tag = "9";
            this.channel9ToolStripMenuItem.SelectedIndexChanged += new EventHandler(this.channelMapItem_SelectedIndexChanged);
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.DropDownItems.AddRange(new ToolStripItem[] {
            this.channel0ToolStripMenuItem});
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new Size(86, 22);
            this.toolStripMenuItem11.Text = "\'0\'";
            // 
            // channel0ToolStripMenuItem
            // 
            this.channel0ToolStripMenuItem.DropDownStyle = ComboBoxStyle.DropDownList;
            this.channel0ToolStripMenuItem.Name = "channel0ToolStripMenuItem";
            this.channel0ToolStripMenuItem.Size = new Size(152, 23);
            this.channel0ToolStripMenuItem.Tag = "10";
            this.channel0ToolStripMenuItem.SelectedIndexChanged += new EventHandler(this.channelMapItem_SelectedIndexChanged);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(276, 543);
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
            this.buttonCancel.Location = new Point(357, 543);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
                        | AnchorStyles.Left)
                        | AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.radioButtonMultipleEvents);
            this.groupBox3.Controls.Add(this.radioButtonSingleEvent);
            this.groupBox3.Location = new Point(12, 198);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(420, 99);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Key behavior";
            // 
            // radioButtonMultipleEvents
            // 
            this.radioButtonMultipleEvents.AutoSize = true;
            this.radioButtonMultipleEvents.Location = new Point(23, 58);
            this.radioButtonMultipleEvents.Name = "radioButtonMultipleEvents";
            this.radioButtonMultipleEvents.Size = new Size(283, 17);
            this.radioButtonMultipleEvents.TabIndex = 1;
            this.radioButtonMultipleEvents.Text = "Create events while a key is pressed, until it is released";
            this.radioButtonMultipleEvents.UseVisualStyleBackColor = true;
            // 
            // radioButtonSingleEvent
            // 
            this.radioButtonSingleEvent.AutoSize = true;
            this.radioButtonSingleEvent.Checked = true;
            this.radioButtonSingleEvent.Location = new Point(23, 29);
            this.radioButtonSingleEvent.Name = "radioButtonSingleEvent";
            this.radioButtonSingleEvent.Size = new Size(233, 17);
            this.radioButtonSingleEvent.TabIndex = 0;
            this.radioButtonSingleEvent.TabStop = true;
            this.radioButtonSingleEvent.Text = "Create a single event when a key is pressed";
            this.radioButtonSingleEvent.UseVisualStyleBackColor = true;
            // 
            // AudioDialog
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new Size(444, 574);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AudioDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Event Sequence Audio";
            this.FormClosing += new FormClosingEventHandler(this.AudioDialog_FormClosing);
            this.KeyDown += new KeyEventHandler(this.AudioDialog_KeyDown);
            this.KeyUp += new KeyEventHandler(this.AudioDialog_KeyUp);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((ISupportInitialize)(this.trackBarPosition)).EndInit();
            ((ISupportInitialize)(this.pictureBoxPlay)).EndInit();
            ((ISupportInitialize)(this.pictureBoxPlayBlue)).EndInit();
            ((ISupportInitialize)(this.pictureBoxPause)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
