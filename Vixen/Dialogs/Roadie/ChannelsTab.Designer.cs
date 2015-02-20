using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using VixenPlus.Properties;

using VixenPlusCommon;

namespace VixenPlus.Dialogs {
    partial class ChannelsTab {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.pMultiAdd = new System.Windows.Forms.Panel();
            this.btnClearSettings = new System.Windows.Forms.Button();
            this.btnUpdatePreview = new System.Windows.Forms.Button();
            this.btnMultiChannelCancel = new System.Windows.Forms.Button();
            this.gbRules = new System.Windows.Forms.GroupBox();
            this.btnRuleDelete = new System.Windows.Forms.Button();
            this.btnRuleAdd = new System.Windows.Forms.Button();
            this.cbRuleRules = new System.Windows.Forms.ComboBox();
            this.btnRuleDown = new System.Windows.Forms.Button();
            this.btnRuleUp = new System.Windows.Forms.Button();
            this.lbRules = new System.Windows.Forms.ListBox();
            this.panelRuleEditor = new System.Windows.Forms.Panel();
            this.colorPaletteChannel = new VixenPlusCommon.ColorPalette();
            this.cbRuleEndNum = new System.Windows.Forms.CheckBox();
            this.nudRuleIncr = new System.Windows.Forms.NumericUpDown();
            this.nudRuleEnd = new System.Windows.Forms.NumericUpDown();
            this.nudRuleStart = new System.Windows.Forms.NumericUpDown();
            this.lblRuleIncr = new System.Windows.Forms.Label();
            this.lblRuleStartNum = new System.Windows.Forms.Label();
            this.tbRuleWords = new System.Windows.Forms.TextBox();
            this.lblRulePrompt = new System.Windows.Forms.Label();
            this.cbRuleColors = new System.Windows.Forms.CheckBox();
            this.btnMultiChannelOk = new System.Windows.Forms.Button();
            this.btnChGenSaveTemplate = new System.Windows.Forms.Button();
            this.tbChGenNameFormat = new System.Windows.Forms.TextBox();
            this.nudChGenChannels = new System.Windows.Forms.NumericUpDown();
            this.lblChGenNameFormat = new System.Windows.Forms.Label();
            this.cbPreview = new System.Windows.Forms.CheckBox();
            this.lblChGenCount = new System.Windows.Forms.Label();
            this.lblChGenTemplate = new System.Windows.Forms.Label();
            this.cbChGenTemplate = new System.Windows.Forms.ComboBox();
            this.dgvChannels = new System.Windows.Forms.DataGridView();
            this.ChannelEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ChannelNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChannelName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OutputChannel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChannelColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pChannels = new System.Windows.Forms.Panel();
            this.gbExportImport = new System.Windows.Forms.GroupBox();
            this.btnChExport = new System.Windows.Forms.Button();
            this.btnChImport = new System.Windows.Forms.Button();
            this.gbChannels = new System.Windows.Forms.GroupBox();
            this.btnChDelete = new System.Windows.Forms.Button();
            this.btnChAddMulti = new System.Windows.Forms.Button();
            this.btnChAddOne = new System.Windows.Forms.Button();
            this.gbEnable = new System.Windows.Forms.GroupBox();
            this.btnChEnable = new System.Windows.Forms.Button();
            this.btnChDisable = new System.Windows.Forms.Button();
            this.gbColors = new System.Windows.Forms.GroupBox();
            this.btnChColorMulti = new System.Windows.Forms.Button();
            this.btnChColorOne = new System.Windows.Forms.Button();
            this.pMultiColor = new System.Windows.Forms.Panel();
            this.btnMultiColorOk = new System.Windows.Forms.Button();
            this.btnMultiColorCancel = new System.Windows.Forms.Button();
            this.colorPaletteColor = new VixenPlusCommon.ColorPalette();
            this.previewTimer = new System.Windows.Forms.Timer(this.components);
            this.cbBounce = new System.Windows.Forms.CheckBox();
            this.cbMatchInFormat = new System.Windows.Forms.CheckBox();
            this.pMultiAdd.SuspendLayout();
            this.gbRules.SuspendLayout();
            this.panelRuleEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRuleIncr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRuleEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRuleStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudChGenChannels)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChannels)).BeginInit();
            this.pChannels.SuspendLayout();
            this.gbExportImport.SuspendLayout();
            this.gbChannels.SuspendLayout();
            this.gbEnable.SuspendLayout();
            this.gbColors.SuspendLayout();
            this.pMultiColor.SuspendLayout();
            this.SuspendLayout();
            // 
            // pMultiAdd
            // 
            this.pMultiAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pMultiAdd.Controls.Add(this.btnClearSettings);
            this.pMultiAdd.Controls.Add(this.btnUpdatePreview);
            this.pMultiAdd.Controls.Add(this.btnMultiChannelCancel);
            this.pMultiAdd.Controls.Add(this.gbRules);
            this.pMultiAdd.Controls.Add(this.btnMultiChannelOk);
            this.pMultiAdd.Controls.Add(this.btnChGenSaveTemplate);
            this.pMultiAdd.Controls.Add(this.tbChGenNameFormat);
            this.pMultiAdd.Controls.Add(this.nudChGenChannels);
            this.pMultiAdd.Controls.Add(this.lblChGenNameFormat);
            this.pMultiAdd.Controls.Add(this.cbPreview);
            this.pMultiAdd.Controls.Add(this.lblChGenCount);
            this.pMultiAdd.Controls.Add(this.lblChGenTemplate);
            this.pMultiAdd.Controls.Add(this.cbChGenTemplate);
            this.pMultiAdd.Location = new System.Drawing.Point(705, 0);
            this.pMultiAdd.Name = "pMultiAdd";
            this.pMultiAdd.Size = new System.Drawing.Size(269, 527);
            this.pMultiAdd.TabIndex = 0;
            // 
            // btnClearSettings
            // 
            this.btnClearSettings.Location = new System.Drawing.Point(81, 468);
            this.btnClearSettings.Name = "btnClearSettings";
            this.btnClearSettings.Size = new System.Drawing.Size(104, 23);
            this.btnClearSettings.TabIndex = 23;
            this.btnClearSettings.Text = "C&lear Settings";
            this.btnClearSettings.UseVisualStyleBackColor = true;
            this.btnClearSettings.Click += new System.EventHandler(this.btnClearSettings_Click);
            // 
            // btnUpdatePreview
            // 
            this.btnUpdatePreview.Enabled = false;
            this.btnUpdatePreview.Location = new System.Drawing.Point(157, 0);
            this.btnUpdatePreview.Name = "btnUpdatePreview";
            this.btnUpdatePreview.Size = new System.Drawing.Size(99, 23);
            this.btnUpdatePreview.TabIndex = 22;
            this.btnUpdatePreview.Text = "&Update Preview";
            this.btnUpdatePreview.UseVisualStyleBackColor = true;
            this.btnUpdatePreview.Click += new System.EventHandler(this.btnUpdatePreview_Click);
            // 
            // btnMultiChannelCancel
            // 
            this.btnMultiChannelCancel.Location = new System.Drawing.Point(182, 504);
            this.btnMultiChannelCancel.Name = "btnMultiChannelCancel";
            this.btnMultiChannelCancel.Size = new System.Drawing.Size(75, 23);
            this.btnMultiChannelCancel.TabIndex = 17;
            this.btnMultiChannelCancel.Text = "&Cancel";
            this.btnMultiChannelCancel.UseVisualStyleBackColor = true;
            this.btnMultiChannelCancel.Click += new System.EventHandler(this.btnMultiChannelButton_Click);
            // 
            // gbRules
            // 
            this.gbRules.Controls.Add(this.cbMatchInFormat);
            this.gbRules.Controls.Add(this.btnRuleDelete);
            this.gbRules.Controls.Add(this.btnRuleAdd);
            this.gbRules.Controls.Add(this.cbRuleRules);
            this.gbRules.Controls.Add(this.btnRuleDown);
            this.gbRules.Controls.Add(this.btnRuleUp);
            this.gbRules.Controls.Add(this.lbRules);
            this.gbRules.Controls.Add(this.panelRuleEditor);
            this.gbRules.Location = new System.Drawing.Point(3, 159);
            this.gbRules.Name = "gbRules";
            this.gbRules.Size = new System.Drawing.Size(261, 303);
            this.gbRules.TabIndex = 20;
            this.gbRules.TabStop = false;
            this.gbRules.Text = "Channel Generation Rules";
            // 
            // btnRuleDelete
            // 
            this.btnRuleDelete.Image = global::VixenPlus.Properties.Resources.list_remove;
            this.btnRuleDelete.Location = new System.Drawing.Point(6, 121);
            this.btnRuleDelete.Name = "btnRuleDelete";
            this.btnRuleDelete.Size = new System.Drawing.Size(24, 24);
            this.btnRuleDelete.TabIndex = 5;
            this.btnRuleDelete.UseVisualStyleBackColor = true;
            this.btnRuleDelete.Click += new System.EventHandler(this.btnRuleDelete_Click);
            // 
            // btnRuleAdd
            // 
            this.btnRuleAdd.Image = global::VixenPlus.Properties.Resources.list_add;
            this.btnRuleAdd.Location = new System.Drawing.Point(231, 17);
            this.btnRuleAdd.Name = "btnRuleAdd";
            this.btnRuleAdd.Size = new System.Drawing.Size(24, 24);
            this.btnRuleAdd.TabIndex = 1;
            this.btnRuleAdd.UseVisualStyleBackColor = true;
            this.btnRuleAdd.Click += new System.EventHandler(this.btnRuleAdd_Click);
            // 
            // cbRuleRules
            // 
            this.cbRuleRules.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRuleRules.FormattingEnabled = true;
            this.cbRuleRules.Items.AddRange(new object[] {
            "Numbers",
            "Words"});
            this.cbRuleRules.Location = new System.Drawing.Point(6, 19);
            this.cbRuleRules.Name = "cbRuleRules";
            this.cbRuleRules.Size = new System.Drawing.Size(219, 21);
            this.cbRuleRules.TabIndex = 0;
            this.cbRuleRules.SelectedIndexChanged += new System.EventHandler(this.cbRuleRules_SelectedIndexChanged);
            // 
            // btnRuleDown
            // 
            this.btnRuleDown.Image = global::VixenPlus.Properties.Resources.arrow_down;
            this.btnRuleDown.Location = new System.Drawing.Point(231, 121);
            this.btnRuleDown.Name = "btnRuleDown";
            this.btnRuleDown.Size = new System.Drawing.Size(24, 24);
            this.btnRuleDown.TabIndex = 2;
            this.btnRuleDown.UseVisualStyleBackColor = true;
            this.btnRuleDown.Click += new System.EventHandler(this.btnRuleDown_Click);
            // 
            // btnRuleUp
            // 
            this.btnRuleUp.Image = global::VixenPlus.Properties.Resources.arrow_up;
            this.btnRuleUp.Location = new System.Drawing.Point(201, 121);
            this.btnRuleUp.Name = "btnRuleUp";
            this.btnRuleUp.Size = new System.Drawing.Size(24, 24);
            this.btnRuleUp.TabIndex = 1;
            this.btnRuleUp.UseVisualStyleBackColor = true;
            this.btnRuleUp.Click += new System.EventHandler(this.btnRuleUp_Click);
            // 
            // lbRules
            // 
            this.lbRules.DisplayMember = "Name";
            this.lbRules.FormattingEnabled = true;
            this.lbRules.Location = new System.Drawing.Point(6, 46);
            this.lbRules.Name = "lbRules";
            this.lbRules.ScrollAlwaysVisible = true;
            this.lbRules.Size = new System.Drawing.Size(247, 69);
            this.lbRules.TabIndex = 0;
            this.lbRules.SelectedIndexChanged += new System.EventHandler(this.lbRules_SelectedIndexChanged);
            this.lbRules.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbRules_KeyDown);
            // 
            // panelRuleEditor
            // 
            this.panelRuleEditor.Controls.Add(this.cbBounce);
            this.panelRuleEditor.Controls.Add(this.colorPaletteChannel);
            this.panelRuleEditor.Controls.Add(this.cbRuleEndNum);
            this.panelRuleEditor.Controls.Add(this.nudRuleIncr);
            this.panelRuleEditor.Controls.Add(this.nudRuleEnd);
            this.panelRuleEditor.Controls.Add(this.nudRuleStart);
            this.panelRuleEditor.Controls.Add(this.lblRuleIncr);
            this.panelRuleEditor.Controls.Add(this.lblRuleStartNum);
            this.panelRuleEditor.Controls.Add(this.tbRuleWords);
            this.panelRuleEditor.Controls.Add(this.lblRulePrompt);
            this.panelRuleEditor.Controls.Add(this.cbRuleColors);
            this.panelRuleEditor.Location = new System.Drawing.Point(0, 152);
            this.panelRuleEditor.Name = "panelRuleEditor";
            this.panelRuleEditor.Size = new System.Drawing.Size(253, 151);
            this.panelRuleEditor.TabIndex = 6;
            // 
            // colorPaletteChannel
            // 
            this.colorPaletteChannel.Location = new System.Drawing.Point(147, 95);
            this.colorPaletteChannel.Name = "colorPaletteChannel";
            this.colorPaletteChannel.Size = new System.Drawing.Size(103, 50);
            this.colorPaletteChannel.TabIndex = 5;
            // 
            // cbRuleEndNum
            // 
            this.cbRuleEndNum.AutoSize = true;
            this.cbRuleEndNum.Location = new System.Drawing.Point(69, 47);
            this.cbRuleEndNum.Name = "cbRuleEndNum";
            this.cbRuleEndNum.Size = new System.Drawing.Size(110, 17);
            this.cbRuleEndNum.TabIndex = 1;
            this.cbRuleEndNum.Text = "Use End Number:";
            this.cbRuleEndNum.UseVisualStyleBackColor = true;
            this.cbRuleEndNum.CheckedChanged += new System.EventHandler(this.cbRuleEndNum_CheckedChanged);
            // 
            // nudRuleIncr
            // 
            this.nudRuleIncr.Location = new System.Drawing.Point(185, 72);
            this.nudRuleIncr.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudRuleIncr.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.nudRuleIncr.Name = "nudRuleIncr";
            this.nudRuleIncr.Size = new System.Drawing.Size(65, 20);
            this.nudRuleIncr.TabIndex = 3;
            this.nudRuleIncr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudRuleIncr.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRuleIncr.ValueChanged += new System.EventHandler(this.nudRuleIncr_ValueChanged);
            // 
            // nudRuleEnd
            // 
            this.nudRuleEnd.Location = new System.Drawing.Point(185, 46);
            this.nudRuleEnd.Maximum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.nudRuleEnd.Name = "nudRuleEnd";
            this.nudRuleEnd.Size = new System.Drawing.Size(65, 20);
            this.nudRuleEnd.TabIndex = 2;
            this.nudRuleEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudRuleEnd.ValueChanged += new System.EventHandler(this.nudRuleEnd_ValueChanged);
            // 
            // nudRuleStart
            // 
            this.nudRuleStart.Location = new System.Drawing.Point(185, 20);
            this.nudRuleStart.Maximum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.nudRuleStart.Name = "nudRuleStart";
            this.nudRuleStart.Size = new System.Drawing.Size(65, 20);
            this.nudRuleStart.TabIndex = 0;
            this.nudRuleStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudRuleStart.ValueChanged += new System.EventHandler(this.nudRuleStart_ValueChanged);
            // 
            // lblRuleIncr
            // 
            this.lblRuleIncr.AutoSize = true;
            this.lblRuleIncr.Location = new System.Drawing.Point(122, 74);
            this.lblRuleIncr.Name = "lblRuleIncr";
            this.lblRuleIncr.Size = new System.Drawing.Size(57, 13);
            this.lblRuleIncr.TabIndex = 9;
            this.lblRuleIncr.Text = "Increment:";
            // 
            // lblRuleStartNum
            // 
            this.lblRuleStartNum.AutoSize = true;
            this.lblRuleStartNum.Location = new System.Drawing.Point(93, 22);
            this.lblRuleStartNum.Name = "lblRuleStartNum";
            this.lblRuleStartNum.Size = new System.Drawing.Size(86, 13);
            this.lblRuleStartNum.TabIndex = 7;
            this.lblRuleStartNum.Text = "Starting Number:";
            // 
            // tbRuleWords
            // 
            this.tbRuleWords.Location = new System.Drawing.Point(6, 21);
            this.tbRuleWords.Multiline = true;
            this.tbRuleWords.Name = "tbRuleWords";
            this.tbRuleWords.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbRuleWords.Size = new System.Drawing.Size(244, 71);
            this.tbRuleWords.TabIndex = 6;
            this.tbRuleWords.TextChanged += new System.EventHandler(this.tbRuleWords_TextChanged);
            // 
            // lblRulePrompt
            // 
            this.lblRulePrompt.AutoSize = true;
            this.lblRulePrompt.Location = new System.Drawing.Point(6, 4);
            this.lblRulePrompt.Name = "lblRulePrompt";
            this.lblRulePrompt.Size = new System.Drawing.Size(109, 13);
            this.lblRulePrompt.TabIndex = 5;
            this.lblRulePrompt.Text = "Words (One Per Line)";
            // 
            // cbRuleColors
            // 
            this.cbRuleColors.AutoSize = true;
            this.cbRuleColors.Checked = true;
            this.cbRuleColors.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRuleColors.Location = new System.Drawing.Point(83, 112);
            this.cbRuleColors.Name = "cbRuleColors";
            this.cbRuleColors.Size = new System.Drawing.Size(58, 17);
            this.cbRuleColors.TabIndex = 4;
            this.cbRuleColors.Text = "Colors:";
            this.cbRuleColors.UseVisualStyleBackColor = true;
            this.cbRuleColors.CheckStateChanged += new System.EventHandler(this.cbRuleColors_CheckedChanged);
            // 
            // btnMultiChannelOk
            // 
            this.btnMultiChannelOk.Location = new System.Drawing.Point(3, 504);
            this.btnMultiChannelOk.Name = "btnMultiChannelOk";
            this.btnMultiChannelOk.Size = new System.Drawing.Size(82, 23);
            this.btnMultiChannelOk.TabIndex = 15;
            this.btnMultiChannelOk.Text = "&Add Channels";
            this.btnMultiChannelOk.UseVisualStyleBackColor = true;
            this.btnMultiChannelOk.Click += new System.EventHandler(this.btnMultiChannelButton_Click);
            // 
            // btnChGenSaveTemplate
            // 
            this.btnChGenSaveTemplate.Image = global::VixenPlus.Properties.Resources.saveSm;
            this.btnChGenSaveTemplate.Location = new System.Drawing.Point(234, 34);
            this.btnChGenSaveTemplate.Name = "btnChGenSaveTemplate";
            this.btnChGenSaveTemplate.Size = new System.Drawing.Size(24, 24);
            this.btnChGenSaveTemplate.TabIndex = 12;
            this.btnChGenSaveTemplate.UseVisualStyleBackColor = true;
            this.btnChGenSaveTemplate.Click += new System.EventHandler(this.btnChGenSaveTemplate_Click);
            // 
            // tbChGenNameFormat
            // 
            this.tbChGenNameFormat.Location = new System.Drawing.Point(3, 106);
            this.tbChGenNameFormat.MaxLength = 200;
            this.tbChGenNameFormat.Multiline = true;
            this.tbChGenNameFormat.Name = "tbChGenNameFormat";
            this.tbChGenNameFormat.Size = new System.Drawing.Size(253, 47);
            this.tbChGenNameFormat.TabIndex = 14;
            this.tbChGenNameFormat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbChGenNameFormat_KeyDown);
            // 
            // nudChGenChannels
            // 
            this.nudChGenChannels.Location = new System.Drawing.Point(177, 64);
            this.nudChGenChannels.Maximum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.nudChGenChannels.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudChGenChannels.Name = "nudChGenChannels";
            this.nudChGenChannels.Size = new System.Drawing.Size(79, 20);
            this.nudChGenChannels.TabIndex = 19;
            this.nudChGenChannels.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudChGenChannels.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblChGenNameFormat
            // 
            this.lblChGenNameFormat.AutoSize = true;
            this.lblChGenNameFormat.Location = new System.Drawing.Point(3, 90);
            this.lblChGenNameFormat.Name = "lblChGenNameFormat";
            this.lblChGenNameFormat.Size = new System.Drawing.Size(115, 13);
            this.lblChGenNameFormat.TabIndex = 16;
            this.lblChGenNameFormat.Text = "Channel Name Format:";
            // 
            // cbPreview
            // 
            this.cbPreview.AutoSize = true;
            this.cbPreview.Checked = true;
            this.cbPreview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPreview.Location = new System.Drawing.Point(28, 4);
            this.cbPreview.Name = "cbPreview";
            this.cbPreview.Size = new System.Drawing.Size(123, 17);
            this.cbPreview.TabIndex = 21;
            this.cbPreview.Text = "Enable Live Preview";
            this.cbPreview.UseVisualStyleBackColor = true;
            this.cbPreview.CheckStateChanged += new System.EventHandler(this.PreviewChannelEvent);
            // 
            // lblChGenCount
            // 
            this.lblChGenCount.AutoSize = true;
            this.lblChGenCount.Location = new System.Drawing.Point(93, 66);
            this.lblChGenCount.Name = "lblChGenCount";
            this.lblChGenCount.Size = new System.Drawing.Size(76, 13);
            this.lblChGenCount.TabIndex = 18;
            this.lblChGenCount.Text = "# of Channels:";
            // 
            // lblChGenTemplate
            // 
            this.lblChGenTemplate.AutoSize = true;
            this.lblChGenTemplate.Location = new System.Drawing.Point(3, 20);
            this.lblChGenTemplate.Name = "lblChGenTemplate";
            this.lblChGenTemplate.Size = new System.Drawing.Size(54, 13);
            this.lblChGenTemplate.TabIndex = 13;
            this.lblChGenTemplate.Text = "Template:";
            // 
            // cbChGenTemplate
            // 
            this.cbChGenTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbChGenTemplate.FormattingEnabled = true;
            this.cbChGenTemplate.Items.AddRange(new object[] {
            "RGB Channels",
            "Numbered Elements"});
            this.cbChGenTemplate.Location = new System.Drawing.Point(6, 36);
            this.cbChGenTemplate.Name = "cbChGenTemplate";
            this.cbChGenTemplate.Size = new System.Drawing.Size(222, 21);
            this.cbChGenTemplate.TabIndex = 11;
            this.cbChGenTemplate.SelectedIndexChanged += new System.EventHandler(this.cbChGenTemplate_SelectedIndexChanged);
            // 
            // dgvChannels
            // 
            this.dgvChannels.AllowDrop = true;
            this.dgvChannels.AllowUserToAddRows = false;
            this.dgvChannels.AllowUserToDeleteRows = false;
            this.dgvChannels.AllowUserToOrderColumns = true;
            this.dgvChannels.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvChannels.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvChannels.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgvChannels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChannels.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ChannelEnabled,
            this.ChannelNum,
            this.ChannelName,
            this.OutputChannel,
            this.ChannelColor});
            this.dgvChannels.Location = new System.Drawing.Point(0, 0);
            this.dgvChannels.Name = "dgvChannels";
            this.dgvChannels.RowHeadersWidth = 25;
            this.dgvChannels.Size = new System.Drawing.Size(700, 512);
            this.dgvChannels.TabIndex = 1;
            this.dgvChannels.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChannels_CellContentDoubleClick);
            this.dgvChannels.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChannels_CellValueChanged);
            this.dgvChannels.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridView1_DragDrop);
            this.dgvChannels.DragOver += new System.Windows.Forms.DragEventHandler(this.dataGridView1_DragOver);
            this.dgvChannels.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.dgvChannels_QueryContinueDrag);
            this.dgvChannels.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvChannels_KeyDown);
            this.dgvChannels.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDown);
            this.dgvChannels.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseMove);
            // 
            // ChannelEnabled
            // 
            this.ChannelEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ChannelEnabled.FillWeight = 1F;
            this.ChannelEnabled.HeaderText = "Ch Enabled";
            this.ChannelEnabled.MinimumWidth = 87;
            this.ChannelEnabled.Name = "ChannelEnabled";
            this.ChannelEnabled.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ChannelEnabled.ToolTipText = "Is the channel outputing data";
            this.ChannelEnabled.Width = 87;
            // 
            // ChannelNum
            // 
            this.ChannelNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ChannelNum.FillWeight = 1F;
            this.ChannelNum.HeaderText = "Ch #";
            this.ChannelNum.MinimumWidth = 80;
            this.ChannelNum.Name = "ChannelNum";
            this.ChannelNum.ReadOnly = true;
            this.ChannelNum.ToolTipText = "Channel Number (Sequence Order)";
            this.ChannelNum.Width = 80;
            // 
            // ChannelName
            // 
            this.ChannelName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ChannelName.FillWeight = 10F;
            this.ChannelName.HeaderText = "Channel Name";
            this.ChannelName.Name = "ChannelName";
            this.ChannelName.ToolTipText = "Human Readable Name of the Channel";
            // 
            // OutputChannel
            // 
            this.OutputChannel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.OutputChannel.FillWeight = 1F;
            this.OutputChannel.HeaderText = "Output Ch #";
            this.OutputChannel.MinimumWidth = 90;
            this.OutputChannel.Name = "OutputChannel";
            this.OutputChannel.ReadOnly = true;
            this.OutputChannel.ToolTipText = "Channel where the output goes (Physical controller)";
            this.OutputChannel.Width = 90;
            // 
            // ChannelColor
            // 
            this.ChannelColor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ChannelColor.FillWeight = 1F;
            this.ChannelColor.HeaderText = "Channel Color";
            this.ChannelColor.MinimumWidth = 98;
            this.ChannelColor.Name = "ChannelColor";
            this.ChannelColor.ReadOnly = true;
            this.ChannelColor.ToolTipText = "Sequencer Color of the Channel";
            this.ChannelColor.Width = 98;
            // 
            // pChannels
            // 
            this.pChannels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pChannels.Controls.Add(this.gbExportImport);
            this.pChannels.Controls.Add(this.gbChannels);
            this.pChannels.Controls.Add(this.gbEnable);
            this.pChannels.Controls.Add(this.gbColors);
            this.pChannels.Location = new System.Drawing.Point(705, 0);
            this.pChannels.Name = "pChannels";
            this.pChannels.Size = new System.Drawing.Size(269, 512);
            this.pChannels.TabIndex = 23;
            // 
            // gbExportImport
            // 
            this.gbExportImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbExportImport.Controls.Add(this.btnChExport);
            this.gbExportImport.Controls.Add(this.btnChImport);
            this.gbExportImport.Location = new System.Drawing.Point(3, 166);
            this.gbExportImport.Name = "gbExportImport";
            this.gbExportImport.Size = new System.Drawing.Size(250, 48);
            this.gbExportImport.TabIndex = 7;
            this.gbExportImport.TabStop = false;
            this.gbExportImport.Text = "Export/Import Channel Data";
            // 
            // btnChExport
            // 
            this.btnChExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChExport.Location = new System.Drawing.Point(7, 19);
            this.btnChExport.Name = "btnChExport";
            this.btnChExport.Size = new System.Drawing.Size(75, 23);
            this.btnChExport.TabIndex = 0;
            this.btnChExport.Text = "E&xport CSV";
            this.btnChExport.UseVisualStyleBackColor = true;
            this.btnChExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnChImport
            // 
            this.btnChImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChImport.Location = new System.Drawing.Point(88, 19);
            this.btnChImport.Name = "btnChImport";
            this.btnChImport.Size = new System.Drawing.Size(75, 23);
            this.btnChImport.TabIndex = 1;
            this.btnChImport.Text = "&Import CSV";
            this.btnChImport.UseVisualStyleBackColor = true;
            this.btnChImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // gbChannels
            // 
            this.gbChannels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbChannels.Controls.Add(this.btnChDelete);
            this.gbChannels.Controls.Add(this.btnChAddMulti);
            this.gbChannels.Controls.Add(this.btnChAddOne);
            this.gbChannels.Location = new System.Drawing.Point(3, 4);
            this.gbChannels.Name = "gbChannels";
            this.gbChannels.Size = new System.Drawing.Size(250, 48);
            this.gbChannels.TabIndex = 4;
            this.gbChannels.TabStop = false;
            this.gbChannels.Text = "Channels";
            // 
            // btnChDelete
            // 
            this.btnChDelete.Location = new System.Drawing.Point(169, 19);
            this.btnChDelete.Name = "btnChDelete";
            this.btnChDelete.Size = new System.Drawing.Size(75, 23);
            this.btnChDelete.TabIndex = 2;
            this.btnChDelete.Text = "Delete";
            this.btnChDelete.UseVisualStyleBackColor = true;
            this.btnChDelete.Click += new System.EventHandler(this.btnChDelete_Click);
            // 
            // btnChAddMulti
            // 
            this.btnChAddMulti.Location = new System.Drawing.Point(88, 19);
            this.btnChAddMulti.Name = "btnChAddMulti";
            this.btnChAddMulti.Size = new System.Drawing.Size(75, 23);
            this.btnChAddMulti.TabIndex = 1;
            this.btnChAddMulti.Text = "Add &Multiple";
            this.btnChAddMulti.UseVisualStyleBackColor = true;
            this.btnChAddMulti.Click += new System.EventHandler(this.btnChAddMulti_Click);
            // 
            // btnChAddOne
            // 
            this.btnChAddOne.Location = new System.Drawing.Point(7, 19);
            this.btnChAddOne.Name = "btnChAddOne";
            this.btnChAddOne.Size = new System.Drawing.Size(75, 23);
            this.btnChAddOne.TabIndex = 0;
            this.btnChAddOne.Text = "Add O&ne";
            this.btnChAddOne.UseVisualStyleBackColor = true;
            this.btnChAddOne.Click += new System.EventHandler(this.btnChAddOne_Click);
            // 
            // gbEnable
            // 
            this.gbEnable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbEnable.Controls.Add(this.btnChEnable);
            this.gbEnable.Controls.Add(this.btnChDisable);
            this.gbEnable.Location = new System.Drawing.Point(3, 112);
            this.gbEnable.Name = "gbEnable";
            this.gbEnable.Size = new System.Drawing.Size(250, 48);
            this.gbEnable.TabIndex = 6;
            this.gbEnable.TabStop = false;
            this.gbEnable.Text = "Channel Enabling";
            // 
            // btnChEnable
            // 
            this.btnChEnable.Location = new System.Drawing.Point(6, 19);
            this.btnChEnable.Name = "btnChEnable";
            this.btnChEnable.Size = new System.Drawing.Size(75, 23);
            this.btnChEnable.TabIndex = 0;
            this.btnChEnable.Text = "&Enable";
            this.btnChEnable.UseVisualStyleBackColor = true;
            this.btnChEnable.Click += new System.EventHandler(this.btnEnableDisable_Click);
            // 
            // btnChDisable
            // 
            this.btnChDisable.Location = new System.Drawing.Point(87, 19);
            this.btnChDisable.Name = "btnChDisable";
            this.btnChDisable.Size = new System.Drawing.Size(75, 23);
            this.btnChDisable.TabIndex = 1;
            this.btnChDisable.Text = "&Disable";
            this.btnChDisable.UseVisualStyleBackColor = true;
            this.btnChDisable.Click += new System.EventHandler(this.btnEnableDisable_Click);
            // 
            // gbColors
            // 
            this.gbColors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbColors.Controls.Add(this.btnChColorMulti);
            this.gbColors.Controls.Add(this.btnChColorOne);
            this.gbColors.Location = new System.Drawing.Point(3, 58);
            this.gbColors.Name = "gbColors";
            this.gbColors.Size = new System.Drawing.Size(250, 48);
            this.gbColors.TabIndex = 5;
            this.gbColors.TabStop = false;
            this.gbColors.Text = "Channel Colors";
            // 
            // btnChColorMulti
            // 
            this.btnChColorMulti.Location = new System.Drawing.Point(88, 19);
            this.btnChColorMulti.Name = "btnChColorMulti";
            this.btnChColorMulti.Size = new System.Drawing.Size(75, 23);
            this.btnChColorMulti.TabIndex = 1;
            this.btnChColorMulti.Text = "Multi &Color";
            this.btnChColorMulti.UseVisualStyleBackColor = true;
            this.btnChColorMulti.Click += new System.EventHandler(this.btnChColorMulti_Click);
            // 
            // btnChColorOne
            // 
            this.btnChColorOne.Location = new System.Drawing.Point(7, 19);
            this.btnChColorOne.Name = "btnChColorOne";
            this.btnChColorOne.Size = new System.Drawing.Size(75, 23);
            this.btnChColorOne.TabIndex = 0;
            this.btnChColorOne.Text = "&One Color";
            this.btnChColorOne.UseVisualStyleBackColor = true;
            this.btnChColorOne.Click += new System.EventHandler(this.btnChColorOne_Click);
            // 
            // pMultiColor
            // 
            this.pMultiColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pMultiColor.Controls.Add(this.btnMultiColorOk);
            this.pMultiColor.Controls.Add(this.btnMultiColorCancel);
            this.pMultiColor.Controls.Add(this.colorPaletteColor);
            this.pMultiColor.Location = new System.Drawing.Point(705, 0);
            this.pMultiColor.Name = "pMultiColor";
            this.pMultiColor.Size = new System.Drawing.Size(269, 512);
            this.pMultiColor.TabIndex = 8;
            // 
            // btnMultiColorOk
            // 
            this.btnMultiColorOk.Location = new System.Drawing.Point(3, 61);
            this.btnMultiColorOk.Name = "btnMultiColorOk";
            this.btnMultiColorOk.Size = new System.Drawing.Size(75, 23);
            this.btnMultiColorOk.TabIndex = 5;
            this.btnMultiColorOk.Text = "OK";
            this.btnMultiColorOk.UseVisualStyleBackColor = true;
            this.btnMultiColorOk.Click += new System.EventHandler(this.btnMultiColor_Click);
            // 
            // btnMultiColorCancel
            // 
            this.btnMultiColorCancel.Location = new System.Drawing.Point(189, 61);
            this.btnMultiColorCancel.Name = "btnMultiColorCancel";
            this.btnMultiColorCancel.Size = new System.Drawing.Size(75, 23);
            this.btnMultiColorCancel.TabIndex = 4;
            this.btnMultiColorCancel.Text = "Cancel";
            this.btnMultiColorCancel.UseVisualStyleBackColor = true;
            this.btnMultiColorCancel.Click += new System.EventHandler(this.btnMultiColor_Click);
            // 
            // colorPaletteColor
            // 
            this.colorPaletteColor.Location = new System.Drawing.Point(81, 4);
            this.colorPaletteColor.Name = "colorPaletteColor";
            this.colorPaletteColor.Size = new System.Drawing.Size(104, 50);
            this.colorPaletteColor.TabIndex = 3;
            // 
            // previewTimer
            // 
            this.previewTimer.Interval = 200;
            this.previewTimer.Tick += new System.EventHandler(this.previewTimer_Tick);
            // 
            // cbBounce
            // 
            this.cbBounce.AutoSize = true;
            this.cbBounce.Location = new System.Drawing.Point(47, 72);
            this.cbBounce.Name = "cbBounce";
            this.cbBounce.Size = new System.Drawing.Size(69, 17);
            this.cbBounce.TabIndex = 10;
            this.cbBounce.Text = "Bounce?";
            this.cbBounce.UseVisualStyleBackColor = true;
            this.cbBounce.CheckedChanged += new System.EventHandler(this.cbBounce_CheckedChanged);
            // 
            // cbMatchInFormat
            // 
            this.cbMatchInFormat.AutoSize = true;
            this.cbMatchInFormat.Checked = true;
            this.cbMatchInFormat.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMatchInFormat.Location = new System.Drawing.Point(67, 126);
            this.cbMatchInFormat.Name = "cbMatchInFormat";
            this.cbMatchInFormat.Size = new System.Drawing.Size(128, 17);
            this.cbMatchInFormat.TabIndex = 7;
            this.cbMatchInFormat.Text = "Update Ch Name Fmt";
            this.cbMatchInFormat.UseVisualStyleBackColor = true;
            // 
            // ChannelsTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvChannels);
            this.Controls.Add(this.pMultiAdd);
            this.Controls.Add(this.pMultiColor);
            this.Controls.Add(this.pChannels);
            this.Name = "ChannelsTab";
            this.Size = new System.Drawing.Size(979, 542);
            this.SizeChanged += new System.EventHandler(this.ChannelsTab_SizeChanged);
            this.pMultiAdd.ResumeLayout(false);
            this.pMultiAdd.PerformLayout();
            this.gbRules.ResumeLayout(false);
            this.gbRules.PerformLayout();
            this.panelRuleEditor.ResumeLayout(false);
            this.panelRuleEditor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRuleIncr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRuleEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRuleStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudChGenChannels)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChannels)).EndInit();
            this.pChannels.ResumeLayout(false);
            this.gbExportImport.ResumeLayout(false);
            this.gbChannels.ResumeLayout(false);
            this.gbEnable.ResumeLayout(false);
            this.gbColors.ResumeLayout(false);
            this.pMultiColor.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pMultiAdd;
        private Button btnUpdatePreview;
        private Button btnMultiChannelCancel;
        private GroupBox gbRules;
        private Button btnRuleDelete;
        private Button btnRuleAdd;
        private ComboBox cbRuleRules;
        private Button btnRuleDown;
        private Button btnRuleUp;
        private ListBox lbRules;
        private Panel panelRuleEditor;
        private ColorPalette colorPaletteChannel;
        private CheckBox cbRuleEndNum;
        private NumericUpDown nudRuleIncr;
        private NumericUpDown nudRuleEnd;
        private NumericUpDown nudRuleStart;
        private Label lblRuleIncr;
        private Label lblRuleStartNum;
        private TextBox tbRuleWords;
        private Label lblRulePrompt;
        private CheckBox cbRuleColors;
        private Button btnMultiChannelOk;
        private Button btnChGenSaveTemplate;
        private TextBox tbChGenNameFormat;
        private NumericUpDown nudChGenChannels;
        private Label lblChGenNameFormat;
        private CheckBox cbPreview;
        private Label lblChGenCount;
        private Label lblChGenTemplate;
        private ComboBox cbChGenTemplate;
        private DataGridView dgvChannels;
        private DataGridViewCheckBoxColumn ChannelEnabled;
        private DataGridViewTextBoxColumn ChannelNum;
        private DataGridViewTextBoxColumn ChannelName;
        private DataGridViewTextBoxColumn OutputChannel;
        private DataGridViewTextBoxColumn ChannelColor;
        private Panel pChannels;
        private GroupBox gbExportImport;
        private Button btnChExport;
        private Button btnChImport;
        private GroupBox gbChannels;
        private Button btnChDelete;
        private Button btnChAddMulti;
        private Button btnChAddOne;
        private GroupBox gbEnable;
        private Button btnChEnable;
        private Button btnChDisable;
        private GroupBox gbColors;
        private Button btnChColorMulti;
        private Button btnChColorOne;
        private Panel pMultiColor;
        private Button btnMultiColorOk;
        private Button btnMultiColorCancel;
        private ColorPalette colorPaletteColor;
        private Timer previewTimer;
        private Button btnClearSettings;
        private CheckBox cbBounce;
        private CheckBox cbMatchInFormat;
    }
}
