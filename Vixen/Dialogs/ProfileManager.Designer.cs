namespace VixenPlus.Dialogs
{
    partial class FrmProfileManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cbProfiles = new System.Windows.Forms.ComboBox();
            this.gbProfiles = new System.Windows.Forms.GroupBox();
            this.btnDeleteProfile = new System.Windows.Forms.Button();
            this.btnRenameProfile = new System.Windows.Forms.Button();
            this.btnCopyProfile = new System.Windows.Forms.Button();
            this.btnAddProfile = new System.Windows.Forms.Button();
            this.tcProfile = new System.Windows.Forms.TabControl();
            this.tpChannels = new System.Windows.Forms.TabPage();
            this.panelChGenerator = new System.Windows.Forms.Panel();
            this.gbRules = new System.Windows.Forms.GroupBox();
            this.btnRuleSave = new System.Windows.Forms.Button();
            this.panelRuleEditor = new System.Windows.Forms.Panel();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnRuleDelete = new System.Windows.Forms.Button();
            this.btnRuleAdd = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnRuleDown = new System.Windows.Forms.Button();
            this.btnRuleUp = new System.Windows.Forms.Button();
            this.lbRules = new System.Windows.Forms.ListBox();
            this.nudChannels = new System.Windows.Forms.NumericUpDown();
            this.lblChGenCount = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.cbTemplate = new System.Windows.Forms.ComboBox();
            this.lblChFormat = new System.Windows.Forms.Label();
            this.lblTemplate = new System.Windows.Forms.Label();
            this.btnGenClose = new System.Windows.Forms.Button();
            this.btnGenOk = new System.Windows.Forms.Button();
            this.panelChButtons = new System.Windows.Forms.Panel();
            this.gbChannels = new System.Windows.Forms.GroupBox();
            this.btnChRename = new System.Windows.Forms.Button();
            this.btnChDelete = new System.Windows.Forms.Button();
            this.btnChAddMulti = new System.Windows.Forms.Button();
            this.btnChAddOne = new System.Windows.Forms.Button();
            this.btnChannelOutputs = new System.Windows.Forms.Button();
            this.gbExportImport = new System.Windows.Forms.GroupBox();
            this.btnChExport = new System.Windows.Forms.Button();
            this.btnChImport = new System.Windows.Forms.Button();
            this.gbChMapping = new System.Windows.Forms.GroupBox();
            this.btnChMapBelow = new System.Windows.Forms.Button();
            this.btnChMapAbove = new System.Windows.Forms.Button();
            this.gbEnable = new System.Windows.Forms.GroupBox();
            this.btnChEnable = new System.Windows.Forms.Button();
            this.btnChDisable = new System.Windows.Forms.Button();
            this.gbColors = new System.Windows.Forms.GroupBox();
            this.btnChColorMulti = new System.Windows.Forms.Button();
            this.btnChColorOne = new System.Windows.Forms.Button();
            this.dgvChannels = new System.Windows.Forms.DataGridView();
            this.ChannelEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ChannelNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChannelName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OutputChannel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChannelColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpPlugins = new System.Windows.Forms.TabPage();
            this.btnPiaButton = new System.Windows.Forms.Button();
            this.tpGroups = new System.Windows.Forms.TabPage();
            this.btnGraButton = new System.Windows.Forms.Button();
            this.tpSortOrders = new System.Windows.Forms.TabPage();
            this.btnSrtDelete = new System.Windows.Forms.Button();
            this.btnSrtSave = new System.Windows.Forms.Button();
            this.cbSrtOrders = new System.Windows.Forms.ComboBox();
            this.tpNutcracker = new System.Windows.Forms.TabPage();
            this.btnNcaButton = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.gbProfiles.SuspendLayout();
            this.tcProfile.SuspendLayout();
            this.tpChannels.SuspendLayout();
            this.panelChGenerator.SuspendLayout();
            this.gbRules.SuspendLayout();
            this.panelRuleEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudChannels)).BeginInit();
            this.panelChButtons.SuspendLayout();
            this.gbChannels.SuspendLayout();
            this.gbExportImport.SuspendLayout();
            this.gbChMapping.SuspendLayout();
            this.gbEnable.SuspendLayout();
            this.gbColors.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChannels)).BeginInit();
            this.tpPlugins.SuspendLayout();
            this.tpGroups.SuspendLayout();
            this.tpSortOrders.SuspendLayout();
            this.tpNutcracker.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbProfiles
            // 
            this.cbProfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfiles.FormattingEnabled = true;
            this.cbProfiles.Location = new System.Drawing.Point(10, 19);
            this.cbProfiles.Name = "cbProfiles";
            this.cbProfiles.Size = new System.Drawing.Size(281, 21);
            this.cbProfiles.TabIndex = 0;
            this.cbProfiles.SelectedIndexChanged += new System.EventHandler(this.cbProfiles_SelectedIndexChanged);
            // 
            // gbProfiles
            // 
            this.gbProfiles.Controls.Add(this.btnDeleteProfile);
            this.gbProfiles.Controls.Add(this.btnRenameProfile);
            this.gbProfiles.Controls.Add(this.btnCopyProfile);
            this.gbProfiles.Controls.Add(this.btnAddProfile);
            this.gbProfiles.Controls.Add(this.cbProfiles);
            this.gbProfiles.Location = new System.Drawing.Point(12, 12);
            this.gbProfiles.Name = "gbProfiles";
            this.gbProfiles.Size = new System.Drawing.Size(621, 52);
            this.gbProfiles.TabIndex = 1;
            this.gbProfiles.TabStop = false;
            this.gbProfiles.Text = "Profile";
            // 
            // btnDeleteProfile
            // 
            this.btnDeleteProfile.Location = new System.Drawing.Point(540, 19);
            this.btnDeleteProfile.Name = "btnDeleteProfile";
            this.btnDeleteProfile.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteProfile.TabIndex = 4;
            this.btnDeleteProfile.Text = "Delete";
            this.btnDeleteProfile.UseVisualStyleBackColor = true;
            this.btnDeleteProfile.Click += new System.EventHandler(this.btnDeleteProfile_Click);
            // 
            // btnRenameProfile
            // 
            this.btnRenameProfile.Location = new System.Drawing.Point(378, 19);
            this.btnRenameProfile.Name = "btnRenameProfile";
            this.btnRenameProfile.Size = new System.Drawing.Size(75, 23);
            this.btnRenameProfile.TabIndex = 3;
            this.btnRenameProfile.Text = "Rename";
            this.btnRenameProfile.UseVisualStyleBackColor = true;
            this.btnRenameProfile.Click += new System.EventHandler(this.btnRenameProfile_Click);
            // 
            // btnCopyProfile
            // 
            this.btnCopyProfile.Location = new System.Drawing.Point(459, 19);
            this.btnCopyProfile.Name = "btnCopyProfile";
            this.btnCopyProfile.Size = new System.Drawing.Size(75, 23);
            this.btnCopyProfile.TabIndex = 2;
            this.btnCopyProfile.Text = "Copy";
            this.btnCopyProfile.UseVisualStyleBackColor = true;
            this.btnCopyProfile.Click += new System.EventHandler(this.btnCopyProfile_Click);
            // 
            // btnAddProfile
            // 
            this.btnAddProfile.Location = new System.Drawing.Point(297, 19);
            this.btnAddProfile.Name = "btnAddProfile";
            this.btnAddProfile.Size = new System.Drawing.Size(75, 23);
            this.btnAddProfile.TabIndex = 1;
            this.btnAddProfile.Text = "Add";
            this.btnAddProfile.UseVisualStyleBackColor = true;
            this.btnAddProfile.Click += new System.EventHandler(this.btnAddProfile_Click);
            // 
            // tcProfile
            // 
            this.tcProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcProfile.Controls.Add(this.tpChannels);
            this.tcProfile.Controls.Add(this.tpPlugins);
            this.tcProfile.Controls.Add(this.tpGroups);
            this.tcProfile.Controls.Add(this.tpSortOrders);
            this.tcProfile.Controls.Add(this.tpNutcracker);
            this.tcProfile.HotTrack = true;
            this.tcProfile.Location = new System.Drawing.Point(12, 70);
            this.tcProfile.Name = "tcProfile";
            this.tcProfile.SelectedIndex = 0;
            this.tcProfile.Size = new System.Drawing.Size(760, 480);
            this.tcProfile.TabIndex = 2;
            this.tcProfile.SelectedIndexChanged += new System.EventHandler(this.tcProfile_SelectedIndexChanged);
            // 
            // tpChannels
            // 
            this.tpChannels.Controls.Add(this.panelChGenerator);
            this.tpChannels.Controls.Add(this.panelChButtons);
            this.tpChannels.Controls.Add(this.dgvChannels);
            this.tpChannels.Location = new System.Drawing.Point(4, 22);
            this.tpChannels.Name = "tpChannels";
            this.tpChannels.Padding = new System.Windows.Forms.Padding(3);
            this.tpChannels.Size = new System.Drawing.Size(752, 454);
            this.tpChannels.TabIndex = 0;
            this.tpChannels.Text = "Channels";
            this.tpChannels.UseVisualStyleBackColor = true;
            // 
            // panelChGenerator
            // 
            this.panelChGenerator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelChGenerator.Controls.Add(this.btnRuleSave);
            this.panelChGenerator.Controls.Add(this.gbRules);
            this.panelChGenerator.Controls.Add(this.nudChannels);
            this.panelChGenerator.Controls.Add(this.lblChGenCount);
            this.panelChGenerator.Controls.Add(this.textBox1);
            this.panelChGenerator.Controls.Add(this.cbTemplate);
            this.panelChGenerator.Controls.Add(this.lblChFormat);
            this.panelChGenerator.Controls.Add(this.lblTemplate);
            this.panelChGenerator.Controls.Add(this.btnGenClose);
            this.panelChGenerator.Controls.Add(this.btnGenOk);
            this.panelChGenerator.Location = new System.Drawing.Point(578, 0);
            this.panelChGenerator.Name = "panelChGenerator";
            this.panelChGenerator.Size = new System.Drawing.Size(174, 454);
            this.panelChGenerator.TabIndex = 17;
            this.panelChGenerator.Visible = false;
            // 
            // gbRules
            // 
            this.gbRules.Controls.Add(this.panelRuleEditor);
            this.gbRules.Controls.Add(this.btnRuleDelete);
            this.gbRules.Controls.Add(this.btnRuleAdd);
            this.gbRules.Controls.Add(this.comboBox1);
            this.gbRules.Controls.Add(this.btnRuleDown);
            this.gbRules.Controls.Add(this.btnRuleUp);
            this.gbRules.Controls.Add(this.lbRules);
            this.gbRules.Location = new System.Drawing.Point(3, 116);
            this.gbRules.Name = "gbRules";
            this.gbRules.Size = new System.Drawing.Size(168, 303);
            this.gbRules.TabIndex = 8;
            this.gbRules.TabStop = false;
            this.gbRules.Text = "Rules";
            // 
            // btnRuleSave
            // 
            this.btnRuleSave.Image = global::VixenPlus.Properties.Resources.saveSm;
            this.btnRuleSave.Location = new System.Drawing.Point(140, 21);
            this.btnRuleSave.Name = "btnRuleSave";
            this.btnRuleSave.Size = new System.Drawing.Size(24, 24);
            this.btnRuleSave.TabIndex = 7;
            this.toolTip1.SetToolTip(this.btnRuleSave, "Save template");
            this.btnRuleSave.UseVisualStyleBackColor = true;
            // 
            // panelRuleEditor
            // 
            this.panelRuleEditor.Controls.Add(this.textBox2);
            this.panelRuleEditor.Controls.Add(this.label1);
            this.panelRuleEditor.Controls.Add(this.checkBox1);
            this.panelRuleEditor.Controls.Add(this.pictureBox4);
            this.panelRuleEditor.Controls.Add(this.pictureBox3);
            this.panelRuleEditor.Controls.Add(this.pictureBox2);
            this.panelRuleEditor.Controls.Add(this.pictureBox1);
            this.panelRuleEditor.Location = new System.Drawing.Point(0, 152);
            this.panelRuleEditor.Name = "panelRuleEditor";
            this.panelRuleEditor.Size = new System.Drawing.Size(168, 151);
            this.panelRuleEditor.TabIndex = 6;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(6, 21);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(156, 98);
            this.textBox2.TabIndex = 6;
            this.textBox2.Text = "Red,Blue,Green";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Words (separate with comma)";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(3, 128);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(58, 17);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Colors:";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.Red;
            this.pictureBox4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox4.Location = new System.Drawing.Point(65, 125);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(20, 20);
            this.pictureBox4.TabIndex = 3;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Blue;
            this.pictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox3.Location = new System.Drawing.Point(91, 125);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(20, 20);
            this.pictureBox3.TabIndex = 2;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Lime;
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox2.Location = new System.Drawing.Point(117, 125);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(20, 20);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::VixenPlus.Properties.Resources.none1;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Location = new System.Drawing.Point(143, 125);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(20, 20);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnRuleDelete
            // 
            this.btnRuleDelete.Image = global::VixenPlus.Properties.Resources.list_remove;
            this.btnRuleDelete.Location = new System.Drawing.Point(9, 121);
            this.btnRuleDelete.Name = "btnRuleDelete";
            this.btnRuleDelete.Size = new System.Drawing.Size(24, 24);
            this.btnRuleDelete.TabIndex = 5;
            this.toolTip1.SetToolTip(this.btnRuleDelete, "Remove selected rule");
            this.btnRuleDelete.UseVisualStyleBackColor = true;
            // 
            // btnRuleAdd
            // 
            this.btnRuleAdd.Image = global::VixenPlus.Properties.Resources.list_add;
            this.btnRuleAdd.Location = new System.Drawing.Point(138, 17);
            this.btnRuleAdd.Name = "btnRuleAdd";
            this.btnRuleAdd.Size = new System.Drawing.Size(24, 24);
            this.btnRuleAdd.TabIndex = 4;
            this.toolTip1.SetToolTip(this.btnRuleAdd, "Add selected rule.");
            this.btnRuleAdd.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Numbers",
            "Words"});
            this.comboBox1.Location = new System.Drawing.Point(9, 19);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(123, 21);
            this.comboBox1.TabIndex = 3;
            // 
            // btnRuleDown
            // 
            this.btnRuleDown.Image = global::VixenPlus.Properties.Resources.arrow_down;
            this.btnRuleDown.Location = new System.Drawing.Point(138, 121);
            this.btnRuleDown.Name = "btnRuleDown";
            this.btnRuleDown.Size = new System.Drawing.Size(24, 24);
            this.btnRuleDown.TabIndex = 2;
            this.toolTip1.SetToolTip(this.btnRuleDown, "Move Rule Down");
            this.btnRuleDown.UseVisualStyleBackColor = true;
            // 
            // btnRuleUp
            // 
            this.btnRuleUp.Image = global::VixenPlus.Properties.Resources.arrow_up;
            this.btnRuleUp.Location = new System.Drawing.Point(107, 121);
            this.btnRuleUp.Name = "btnRuleUp";
            this.btnRuleUp.Size = new System.Drawing.Size(24, 24);
            this.btnRuleUp.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btnRuleUp, "Move rule up");
            this.btnRuleUp.UseVisualStyleBackColor = true;
            // 
            // lbRules
            // 
            this.lbRules.FormattingEnabled = true;
            this.lbRules.Items.AddRange(new object[] {
            "Numbers = {0}",
            "Words = {1}",
            "2",
            "3"});
            this.lbRules.Location = new System.Drawing.Point(9, 46);
            this.lbRules.Name = "lbRules";
            this.lbRules.Size = new System.Drawing.Size(153, 69);
            this.lbRules.TabIndex = 0;
            // 
            // nudChannels
            // 
            this.nudChannels.Location = new System.Drawing.Point(88, 51);
            this.nudChannels.Maximum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.nudChannels.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudChannels.Name = "nudChannels";
            this.nudChannels.Size = new System.Drawing.Size(79, 20);
            this.nudChannels.TabIndex = 7;
            this.nudChannels.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudChannels.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // lblChGenCount
            // 
            this.lblChGenCount.AutoSize = true;
            this.lblChGenCount.Location = new System.Drawing.Point(6, 53);
            this.lblChGenCount.Name = "lblChGenCount";
            this.lblChGenCount.Size = new System.Drawing.Size(76, 13);
            this.lblChGenCount.TabIndex = 6;
            this.lblChGenCount.Text = "# of Channels:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(9, 90);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(155, 20);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "Mini Tree {0:00}:{1}";
            // 
            // cbTemplate
            // 
            this.cbTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTemplate.FormattingEnabled = true;
            this.cbTemplate.Items.AddRange(new object[] {
            "RGB Channels",
            "Numbered Elements"});
            this.cbTemplate.Location = new System.Drawing.Point(9, 24);
            this.cbTemplate.Name = "cbTemplate";
            this.cbTemplate.Size = new System.Drawing.Size(125, 21);
            this.cbTemplate.TabIndex = 3;
            // 
            // lblChFormat
            // 
            this.lblChFormat.AutoSize = true;
            this.lblChFormat.Location = new System.Drawing.Point(6, 74);
            this.lblChFormat.Name = "lblChFormat";
            this.lblChFormat.Size = new System.Drawing.Size(115, 13);
            this.lblChFormat.TabIndex = 4;
            this.lblChFormat.Text = "Channel Name Format:";
            // 
            // lblTemplate
            // 
            this.lblTemplate.AutoSize = true;
            this.lblTemplate.Location = new System.Drawing.Point(9, 7);
            this.lblTemplate.Name = "lblTemplate";
            this.lblTemplate.Size = new System.Drawing.Size(54, 13);
            this.lblTemplate.TabIndex = 2;
            this.lblTemplate.Text = "Template:";
            // 
            // btnGenClose
            // 
            this.btnGenClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenClose.Location = new System.Drawing.Point(90, 425);
            this.btnGenClose.Name = "btnGenClose";
            this.btnGenClose.Size = new System.Drawing.Size(75, 23);
            this.btnGenClose.TabIndex = 1;
            this.btnGenClose.Text = "Close";
            this.btnGenClose.UseVisualStyleBackColor = true;
            this.btnGenClose.Click += new System.EventHandler(this.btnGenOk_Click);
            // 
            // btnGenOk
            // 
            this.btnGenOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenOk.Location = new System.Drawing.Point(9, 425);
            this.btnGenOk.Name = "btnGenOk";
            this.btnGenOk.Size = new System.Drawing.Size(75, 23);
            this.btnGenOk.TabIndex = 0;
            this.btnGenOk.Text = "Generate";
            this.btnGenOk.UseVisualStyleBackColor = true;
            this.btnGenOk.Click += new System.EventHandler(this.btnGenOk_Click);
            // 
            // panelChButtons
            // 
            this.panelChButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelChButtons.Controls.Add(this.gbChannels);
            this.panelChButtons.Controls.Add(this.btnChannelOutputs);
            this.panelChButtons.Controls.Add(this.gbExportImport);
            this.panelChButtons.Controls.Add(this.gbChMapping);
            this.panelChButtons.Controls.Add(this.gbEnable);
            this.panelChButtons.Controls.Add(this.gbColors);
            this.panelChButtons.Location = new System.Drawing.Point(578, 0);
            this.panelChButtons.Name = "panelChButtons";
            this.panelChButtons.Size = new System.Drawing.Size(174, 454);
            this.panelChButtons.TabIndex = 16;
            // 
            // gbChannels
            // 
            this.gbChannels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbChannels.Controls.Add(this.btnChRename);
            this.gbChannels.Controls.Add(this.btnChDelete);
            this.gbChannels.Controls.Add(this.btnChAddMulti);
            this.gbChannels.Controls.Add(this.btnChAddOne);
            this.gbChannels.Location = new System.Drawing.Point(3, 6);
            this.gbChannels.Name = "gbChannels";
            this.gbChannels.Size = new System.Drawing.Size(168, 77);
            this.gbChannels.TabIndex = 15;
            this.gbChannels.TabStop = false;
            this.gbChannels.Text = "Channels";
            // 
            // btnChRename
            // 
            this.btnChRename.Location = new System.Drawing.Point(87, 48);
            this.btnChRename.Name = "btnChRename";
            this.btnChRename.Size = new System.Drawing.Size(75, 23);
            this.btnChRename.TabIndex = 3;
            this.btnChRename.Text = "Rename";
            this.btnChRename.UseVisualStyleBackColor = true;
            // 
            // btnChDelete
            // 
            this.btnChDelete.Location = new System.Drawing.Point(6, 48);
            this.btnChDelete.Name = "btnChDelete";
            this.btnChDelete.Size = new System.Drawing.Size(75, 23);
            this.btnChDelete.TabIndex = 2;
            this.btnChDelete.Text = "Delete";
            this.btnChDelete.UseVisualStyleBackColor = true;
            // 
            // btnChAddMulti
            // 
            this.btnChAddMulti.Location = new System.Drawing.Point(87, 19);
            this.btnChAddMulti.Name = "btnChAddMulti";
            this.btnChAddMulti.Size = new System.Drawing.Size(75, 23);
            this.btnChAddMulti.TabIndex = 1;
            this.btnChAddMulti.Text = "Add Multiple";
            this.btnChAddMulti.UseVisualStyleBackColor = true;
            this.btnChAddMulti.Click += new System.EventHandler(this.btnChAddMulti_Click);
            // 
            // btnChAddOne
            // 
            this.btnChAddOne.Location = new System.Drawing.Point(6, 19);
            this.btnChAddOne.Name = "btnChAddOne";
            this.btnChAddOne.Size = new System.Drawing.Size(75, 23);
            this.btnChAddOne.TabIndex = 0;
            this.btnChAddOne.Text = "Add One";
            this.btnChAddOne.UseVisualStyleBackColor = true;
            // 
            // btnChannelOutputs
            // 
            this.btnChannelOutputs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChannelOutputs.Location = new System.Drawing.Point(93, 425);
            this.btnChannelOutputs.Name = "btnChannelOutputs";
            this.btnChannelOutputs.Size = new System.Drawing.Size(75, 23);
            this.btnChannelOutputs.TabIndex = 1;
            this.btnChannelOutputs.Text = "Ch. Outputs";
            this.btnChannelOutputs.UseVisualStyleBackColor = true;
            this.btnChannelOutputs.Visible = false;
            this.btnChannelOutputs.Click += new System.EventHandler(this.btnChannelOutputs_Click);
            // 
            // gbExportImport
            // 
            this.gbExportImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbExportImport.Controls.Add(this.btnChExport);
            this.gbExportImport.Controls.Add(this.btnChImport);
            this.gbExportImport.Location = new System.Drawing.Point(3, 227);
            this.gbExportImport.Name = "gbExportImport";
            this.gbExportImport.Size = new System.Drawing.Size(168, 48);
            this.gbExportImport.TabIndex = 11;
            this.gbExportImport.TabStop = false;
            this.gbExportImport.Text = "Export/Import Channel Data";
            // 
            // btnChExport
            // 
            this.btnChExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChExport.Location = new System.Drawing.Point(6, 19);
            this.btnChExport.Name = "btnChExport";
            this.btnChExport.Size = new System.Drawing.Size(75, 23);
            this.btnChExport.TabIndex = 8;
            this.btnChExport.Text = "Export CSV";
            this.btnChExport.UseVisualStyleBackColor = true;
            this.btnChExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnChImport
            // 
            this.btnChImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChImport.Location = new System.Drawing.Point(87, 19);
            this.btnChImport.Name = "btnChImport";
            this.btnChImport.Size = new System.Drawing.Size(75, 23);
            this.btnChImport.TabIndex = 9;
            this.btnChImport.Text = "Import CSV";
            this.btnChImport.UseVisualStyleBackColor = true;
            this.btnChImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // gbChMapping
            // 
            this.gbChMapping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbChMapping.Controls.Add(this.btnChMapBelow);
            this.gbChMapping.Controls.Add(this.btnChMapAbove);
            this.gbChMapping.Location = new System.Drawing.Point(3, 179);
            this.gbChMapping.Name = "gbChMapping";
            this.gbChMapping.Size = new System.Drawing.Size(168, 48);
            this.gbChMapping.TabIndex = 15;
            this.gbChMapping.TabStop = false;
            this.gbChMapping.Text = "Output Mapping";
            // 
            // btnChMapBelow
            // 
            this.btnChMapBelow.Location = new System.Drawing.Point(87, 19);
            this.btnChMapBelow.Name = "btnChMapBelow";
            this.btnChMapBelow.Size = new System.Drawing.Size(75, 23);
            this.btnChMapBelow.TabIndex = 1;
            this.btnChMapBelow.Text = "Below";
            this.btnChMapBelow.UseVisualStyleBackColor = true;
            // 
            // btnChMapAbove
            // 
            this.btnChMapAbove.Location = new System.Drawing.Point(6, 19);
            this.btnChMapAbove.Name = "btnChMapAbove";
            this.btnChMapAbove.Size = new System.Drawing.Size(75, 23);
            this.btnChMapAbove.TabIndex = 0;
            this.btnChMapAbove.Text = "Above";
            this.btnChMapAbove.UseVisualStyleBackColor = true;
            // 
            // gbEnable
            // 
            this.gbEnable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbEnable.Controls.Add(this.btnChEnable);
            this.gbEnable.Controls.Add(this.btnChDisable);
            this.gbEnable.Location = new System.Drawing.Point(3, 131);
            this.gbEnable.Name = "gbEnable";
            this.gbEnable.Size = new System.Drawing.Size(168, 48);
            this.gbEnable.TabIndex = 13;
            this.gbEnable.TabStop = false;
            this.gbEnable.Text = "Channel Enabling";
            // 
            // btnChEnable
            // 
            this.btnChEnable.Location = new System.Drawing.Point(6, 19);
            this.btnChEnable.Name = "btnChEnable";
            this.btnChEnable.Size = new System.Drawing.Size(75, 23);
            this.btnChEnable.TabIndex = 13;
            this.btnChEnable.Text = "Enable";
            this.btnChEnable.UseVisualStyleBackColor = true;
            this.btnChEnable.Click += new System.EventHandler(this.btnEnableDisable_Click);
            // 
            // btnChDisable
            // 
            this.btnChDisable.Location = new System.Drawing.Point(87, 19);
            this.btnChDisable.Name = "btnChDisable";
            this.btnChDisable.Size = new System.Drawing.Size(75, 23);
            this.btnChDisable.TabIndex = 12;
            this.btnChDisable.Text = "Disable";
            this.btnChDisable.UseVisualStyleBackColor = true;
            this.btnChDisable.Click += new System.EventHandler(this.btnEnableDisable_Click);
            // 
            // gbColors
            // 
            this.gbColors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbColors.Controls.Add(this.btnChColorMulti);
            this.gbColors.Controls.Add(this.btnChColorOne);
            this.gbColors.Location = new System.Drawing.Point(3, 83);
            this.gbColors.Name = "gbColors";
            this.gbColors.Size = new System.Drawing.Size(168, 48);
            this.gbColors.TabIndex = 14;
            this.gbColors.TabStop = false;
            this.gbColors.Text = "Channel Colors";
            // 
            // btnChColorMulti
            // 
            this.btnChColorMulti.Location = new System.Drawing.Point(87, 19);
            this.btnChColorMulti.Name = "btnChColorMulti";
            this.btnChColorMulti.Size = new System.Drawing.Size(75, 23);
            this.btnChColorMulti.TabIndex = 1;
            this.btnChColorMulti.Text = "Multi Color";
            this.btnChColorMulti.UseVisualStyleBackColor = true;
            // 
            // btnChColorOne
            // 
            this.btnChColorOne.Location = new System.Drawing.Point(6, 19);
            this.btnChColorOne.Name = "btnChColorOne";
            this.btnChColorOne.Size = new System.Drawing.Size(75, 23);
            this.btnChColorOne.TabIndex = 0;
            this.btnChColorOne.Text = "One Color";
            this.btnChColorOne.UseVisualStyleBackColor = true;
            // 
            // dgvChannels
            // 
            this.dgvChannels.AllowUserToAddRows = false;
            this.dgvChannels.AllowUserToDeleteRows = false;
            this.dgvChannels.AllowUserToOrderColumns = true;
            this.dgvChannels.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgvChannels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChannels.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ChannelEnabled,
            this.ChannelNum,
            this.ChannelName,
            this.OutputChannel,
            this.ChannelColor});
            this.dgvChannels.Location = new System.Drawing.Point(3, 6);
            this.dgvChannels.Name = "dgvChannels";
            this.dgvChannels.RowHeadersWidth = 25;
            this.dgvChannels.Size = new System.Drawing.Size(566, 442);
            this.dgvChannels.TabIndex = 0;
            this.dgvChannels.SelectionChanged += new System.EventHandler(this.dgvChannels_SelectionChanged);
            // 
            // ChannelEnabled
            // 
            this.ChannelEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ChannelEnabled.HeaderText = "Ch Enabled";
            this.ChannelEnabled.Name = "ChannelEnabled";
            this.ChannelEnabled.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ChannelEnabled.Width = 87;
            // 
            // ChannelNum
            // 
            this.ChannelNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ChannelNum.HeaderText = "Ch #";
            this.ChannelNum.Name = "ChannelNum";
            this.ChannelNum.ReadOnly = true;
            this.ChannelNum.Width = 55;
            // 
            // ChannelName
            // 
            this.ChannelName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ChannelName.HeaderText = "Channel Name";
            this.ChannelName.Name = "ChannelName";
            // 
            // OutputChannel
            // 
            this.OutputChannel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.OutputChannel.HeaderText = "Output Ch #";
            this.OutputChannel.Name = "OutputChannel";
            this.OutputChannel.ReadOnly = true;
            this.OutputChannel.Width = 90;
            // 
            // ChannelColor
            // 
            this.ChannelColor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ChannelColor.HeaderText = "Channel Color";
            this.ChannelColor.Name = "ChannelColor";
            this.ChannelColor.ReadOnly = true;
            this.ChannelColor.Width = 98;
            // 
            // tpPlugins
            // 
            this.tpPlugins.Controls.Add(this.btnPiaButton);
            this.tpPlugins.Location = new System.Drawing.Point(4, 22);
            this.tpPlugins.Name = "tpPlugins";
            this.tpPlugins.Padding = new System.Windows.Forms.Padding(3);
            this.tpPlugins.Size = new System.Drawing.Size(752, 454);
            this.tpPlugins.TabIndex = 1;
            this.tpPlugins.Text = "Plugins";
            this.tpPlugins.UseVisualStyleBackColor = true;
            // 
            // btnPiaButton
            // 
            this.btnPiaButton.Enabled = false;
            this.btnPiaButton.Location = new System.Drawing.Point(674, 428);
            this.btnPiaButton.Name = "btnPiaButton";
            this.btnPiaButton.Size = new System.Drawing.Size(75, 23);
            this.btnPiaButton.TabIndex = 0;
            this.btnPiaButton.Text = "A Button";
            this.btnPiaButton.UseVisualStyleBackColor = true;
            // 
            // tpGroups
            // 
            this.tpGroups.Controls.Add(this.btnGraButton);
            this.tpGroups.Location = new System.Drawing.Point(4, 22);
            this.tpGroups.Name = "tpGroups";
            this.tpGroups.Size = new System.Drawing.Size(752, 454);
            this.tpGroups.TabIndex = 2;
            this.tpGroups.Text = "Groups";
            this.tpGroups.UseVisualStyleBackColor = true;
            // 
            // btnGraButton
            // 
            this.btnGraButton.Enabled = false;
            this.btnGraButton.Location = new System.Drawing.Point(674, 428);
            this.btnGraButton.Name = "btnGraButton";
            this.btnGraButton.Size = new System.Drawing.Size(75, 23);
            this.btnGraButton.TabIndex = 1;
            this.btnGraButton.Text = "A Button";
            this.btnGraButton.UseVisualStyleBackColor = true;
            // 
            // tpSortOrders
            // 
            this.tpSortOrders.Controls.Add(this.btnSrtDelete);
            this.tpSortOrders.Controls.Add(this.btnSrtSave);
            this.tpSortOrders.Controls.Add(this.cbSrtOrders);
            this.tpSortOrders.Location = new System.Drawing.Point(4, 22);
            this.tpSortOrders.Name = "tpSortOrders";
            this.tpSortOrders.Size = new System.Drawing.Size(752, 454);
            this.tpSortOrders.TabIndex = 3;
            this.tpSortOrders.Text = "Sort Orders";
            this.tpSortOrders.UseVisualStyleBackColor = true;
            // 
            // btnSrtDelete
            // 
            this.btnSrtDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSrtDelete.Location = new System.Drawing.Point(674, 32);
            this.btnSrtDelete.Name = "btnSrtDelete";
            this.btnSrtDelete.Size = new System.Drawing.Size(75, 23);
            this.btnSrtDelete.TabIndex = 65;
            this.btnSrtDelete.Text = "Delete Order";
            this.btnSrtDelete.UseVisualStyleBackColor = true;
            // 
            // btnSrtSave
            // 
            this.btnSrtSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSrtSave.Location = new System.Drawing.Point(674, 3);
            this.btnSrtSave.Name = "btnSrtSave";
            this.btnSrtSave.Size = new System.Drawing.Size(75, 23);
            this.btnSrtSave.TabIndex = 64;
            this.btnSrtSave.Text = "Save Order";
            this.btnSrtSave.UseVisualStyleBackColor = true;
            // 
            // cbSrtOrders
            // 
            this.cbSrtOrders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSrtOrders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSrtOrders.FormattingEnabled = true;
            this.cbSrtOrders.Items.AddRange(new object[] {
            "Define new order...",
            "Restore natural order..."});
            this.cbSrtOrders.Location = new System.Drawing.Point(547, 5);
            this.cbSrtOrders.Name = "cbSrtOrders";
            this.cbSrtOrders.Size = new System.Drawing.Size(121, 21);
            this.cbSrtOrders.TabIndex = 63;
            // 
            // tpNutcracker
            // 
            this.tpNutcracker.Controls.Add(this.btnNcaButton);
            this.tpNutcracker.Location = new System.Drawing.Point(4, 22);
            this.tpNutcracker.Name = "tpNutcracker";
            this.tpNutcracker.Size = new System.Drawing.Size(752, 454);
            this.tpNutcracker.TabIndex = 4;
            this.tpNutcracker.Text = "Nutcracker Models";
            this.tpNutcracker.UseVisualStyleBackColor = true;
            // 
            // btnNcaButton
            // 
            this.btnNcaButton.Enabled = false;
            this.btnNcaButton.Location = new System.Drawing.Point(674, 428);
            this.btnNcaButton.Name = "btnNcaButton";
            this.btnNcaButton.Size = new System.Drawing.Size(75, 23);
            this.btnNcaButton.TabIndex = 1;
            this.btnNcaButton.Text = "A Button";
            this.btnNcaButton.UseVisualStyleBackColor = true;
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOkay.Location = new System.Drawing.Point(685, 12);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 3;
            this.btnOkay.Text = "OK";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(685, 41);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmProfileManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.tcProfile);
            this.Controls.Add(this.gbProfiles);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "FrmProfileManager";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ProfileManager";
            this.Shown += new System.EventHandler(this.FrmProfileManager_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmProfileManager_KeyDown);
            this.Resize += new System.EventHandler(this.frmProfileManager_Resize);
            this.gbProfiles.ResumeLayout(false);
            this.tcProfile.ResumeLayout(false);
            this.tpChannels.ResumeLayout(false);
            this.panelChGenerator.ResumeLayout(false);
            this.panelChGenerator.PerformLayout();
            this.gbRules.ResumeLayout(false);
            this.panelRuleEditor.ResumeLayout(false);
            this.panelRuleEditor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudChannels)).EndInit();
            this.panelChButtons.ResumeLayout(false);
            this.gbChannels.ResumeLayout(false);
            this.gbExportImport.ResumeLayout(false);
            this.gbChMapping.ResumeLayout(false);
            this.gbEnable.ResumeLayout(false);
            this.gbColors.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChannels)).EndInit();
            this.tpPlugins.ResumeLayout(false);
            this.tpGroups.ResumeLayout(false);
            this.tpSortOrders.ResumeLayout(false);
            this.tpNutcracker.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbProfiles;
        private System.Windows.Forms.GroupBox gbProfiles;
        private System.Windows.Forms.Button btnDeleteProfile;
        private System.Windows.Forms.Button btnRenameProfile;
        private System.Windows.Forms.Button btnCopyProfile;
        private System.Windows.Forms.Button btnAddProfile;
        private System.Windows.Forms.TabControl tcProfile;
        private System.Windows.Forms.TabPage tpChannels;
        private System.Windows.Forms.TabPage tpPlugins;
        private System.Windows.Forms.DataGridView dgvChannels;
        private System.Windows.Forms.Button btnChannelOutputs;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnChImport;
        private System.Windows.Forms.Button btnChExport;
        private System.Windows.Forms.TabPage tpGroups;
        private System.Windows.Forms.TabPage tpSortOrders;
        private System.Windows.Forms.Button btnSrtDelete;
        private System.Windows.Forms.Button btnSrtSave;
        private System.Windows.Forms.ComboBox cbSrtOrders;
        private System.Windows.Forms.GroupBox gbExportImport;
        private System.Windows.Forms.TabPage tpNutcracker;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ChannelEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChannelNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChannelName;
        private System.Windows.Forms.DataGridViewTextBoxColumn OutputChannel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChannelColor;
        private System.Windows.Forms.GroupBox gbEnable;
        private System.Windows.Forms.Button btnChEnable;
        private System.Windows.Forms.Button btnChDisable;
        private System.Windows.Forms.GroupBox gbColors;
        private System.Windows.Forms.Button btnChColorMulti;
        private System.Windows.Forms.Button btnChColorOne;
        private System.Windows.Forms.GroupBox gbChannels;
        private System.Windows.Forms.Button btnChAddMulti;
        private System.Windows.Forms.Button btnChAddOne;
        private System.Windows.Forms.GroupBox gbChMapping;
        private System.Windows.Forms.Button btnChMapBelow;
        private System.Windows.Forms.Button btnChMapAbove;
        private System.Windows.Forms.Button btnPiaButton;
        private System.Windows.Forms.Button btnGraButton;
        private System.Windows.Forms.Button btnNcaButton;
        private System.Windows.Forms.Button btnChRename;
        private System.Windows.Forms.Button btnChDelete;
        private System.Windows.Forms.Panel panelChButtons;
        private System.Windows.Forms.Panel panelChGenerator;
        private System.Windows.Forms.Button btnGenClose;
        private System.Windows.Forms.Button btnGenOk;
        private System.Windows.Forms.GroupBox gbRules;
        private System.Windows.Forms.Button btnRuleAdd;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnRuleDown;
        private System.Windows.Forms.Button btnRuleUp;
        private System.Windows.Forms.ListBox lbRules;
        private System.Windows.Forms.NumericUpDown nudChannels;
        private System.Windows.Forms.Label lblChGenCount;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lblChFormat;
        private System.Windows.Forms.ComboBox cbTemplate;
        private System.Windows.Forms.Label lblTemplate;
        private System.Windows.Forms.Panel panelRuleEditor;
        private System.Windows.Forms.Button btnRuleDelete;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button btnRuleSave;
    }
}