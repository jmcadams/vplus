
using VixenPlusCommon;

namespace VixenPlus.Dialogs
{
    partial class VixenPlusRoadie
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Output", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Input", System.Windows.Forms.HorizontalAlignment.Left);
            this.cbProfiles = new System.Windows.Forms.ComboBox();
            this.gbProfiles = new System.Windows.Forms.GroupBox();
            this.btnProfileSave = new System.Windows.Forms.Button();
            this.btnProfileDelete = new System.Windows.Forms.Button();
            this.btnProfileRename = new System.Windows.Forms.Button();
            this.btnProfileCopy = new System.Windows.Forms.Button();
            this.btnProfileAdd = new System.Windows.Forms.Button();
            this.tcProfile = new System.Windows.Forms.TabControl();
            this.tpChannels = new System.Windows.Forms.TabPage();
            this.dgvChannels = new System.Windows.Forms.DataGridView();
            this.ChannelEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ChannelNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChannelName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OutputChannel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChannelColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tcControlArea = new VixenPlusCommon.TabControl(this.components);
            this.tpChannelControl = new System.Windows.Forms.TabPage();
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
            this.tpMultiChannel = new System.Windows.Forms.TabPage();
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
            this.tpMultiColor = new System.Windows.Forms.TabPage();
            this.btnMultiColorOk = new System.Windows.Forms.Button();
            this.btnMultiColorCancel = new System.Windows.Forms.Button();
            this.colorPaletteColor = new VixenPlusCommon.ColorPalette();
            this.tpPlugins = new System.Windows.Forms.TabPage();
            this.gbSetup = new System.Windows.Forms.GroupBox();
            this.listViewPlugins = new System.Windows.Forms.ListView();
            this.pluginName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonUse = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listViewOutputPorts = new System.Windows.Forms.ListView();
            this.columnHeaderPortTypeName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPortTypeIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderExpandButton = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPluginName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.checkedListBoxSequencePlugins = new System.Windows.Forms.CheckedListBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonPluginSetup = new System.Windows.Forms.Button();
            this.textBoxChannelTo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxChannelFrom = new System.Windows.Forms.TextBox();
            this.tpSortOrders = new System.Windows.Forms.TabPage();
            this.btnSrtDelete = new System.Windows.Forms.Button();
            this.btnSrtSave = new System.Windows.Forms.Button();
            this.cbSrtOrders = new System.Windows.Forms.ComboBox();
            this.tpGroups = new System.Windows.Forms.TabPage();
            this.btnGraButton = new System.Windows.Forms.Button();
            this.tpNutcracker = new System.Windows.Forms.TabPage();
            this.btnNcaButton = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ttRoadie = new System.Windows.Forms.ToolTip(this.components);
            this.previewTimer = new System.Windows.Forms.Timer(this.components);
            this.gbProfiles.SuspendLayout();
            this.tcProfile.SuspendLayout();
            this.tpChannels.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChannels)).BeginInit();
            this.tcControlArea.SuspendLayout();
            this.tpChannelControl.SuspendLayout();
            this.gbExportImport.SuspendLayout();
            this.gbChannels.SuspendLayout();
            this.gbEnable.SuspendLayout();
            this.gbColors.SuspendLayout();
            this.tpMultiChannel.SuspendLayout();
            this.gbRules.SuspendLayout();
            this.panelRuleEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRuleIncr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRuleEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRuleStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudChGenChannels)).BeginInit();
            this.tpMultiColor.SuspendLayout();
            this.tpPlugins.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tpSortOrders.SuspendLayout();
            this.tpGroups.SuspendLayout();
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
            this.gbProfiles.Controls.Add(this.btnProfileSave);
            this.gbProfiles.Controls.Add(this.btnProfileDelete);
            this.gbProfiles.Controls.Add(this.btnProfileRename);
            this.gbProfiles.Controls.Add(this.btnProfileCopy);
            this.gbProfiles.Controls.Add(this.btnProfileAdd);
            this.gbProfiles.Controls.Add(this.cbProfiles);
            this.gbProfiles.Location = new System.Drawing.Point(12, 12);
            this.gbProfiles.Name = "gbProfiles";
            this.gbProfiles.Size = new System.Drawing.Size(704, 52);
            this.gbProfiles.TabIndex = 0;
            this.gbProfiles.TabStop = false;
            this.gbProfiles.Text = "Profile";
            // 
            // btnProfileSave
            // 
            this.btnProfileSave.Location = new System.Drawing.Point(621, 19);
            this.btnProfileSave.Name = "btnProfileSave";
            this.btnProfileSave.Size = new System.Drawing.Size(75, 23);
            this.btnProfileSave.TabIndex = 5;
            this.btnProfileSave.Text = "&Save";
            this.ttRoadie.SetToolTip(this.btnProfileSave, "Delete current profile");
            this.btnProfileSave.UseVisualStyleBackColor = true;
            this.btnProfileSave.Click += new System.EventHandler(this.btnProfileSave_Click);
            // 
            // btnProfileDelete
            // 
            this.btnProfileDelete.Location = new System.Drawing.Point(540, 19);
            this.btnProfileDelete.Name = "btnProfileDelete";
            this.btnProfileDelete.Size = new System.Drawing.Size(75, 23);
            this.btnProfileDelete.TabIndex = 4;
            this.btnProfileDelete.Text = "Delete";
            this.ttRoadie.SetToolTip(this.btnProfileDelete, "Delete current profile");
            this.btnProfileDelete.UseVisualStyleBackColor = true;
            this.btnProfileDelete.Click += new System.EventHandler(this.btnProfileDelete_Click);
            // 
            // btnProfileRename
            // 
            this.btnProfileRename.Location = new System.Drawing.Point(378, 19);
            this.btnProfileRename.Name = "btnProfileRename";
            this.btnProfileRename.Size = new System.Drawing.Size(75, 23);
            this.btnProfileRename.TabIndex = 2;
            this.btnProfileRename.Text = "Rename";
            this.ttRoadie.SetToolTip(this.btnProfileRename, "Rename current profile");
            this.btnProfileRename.UseVisualStyleBackColor = true;
            this.btnProfileRename.Click += new System.EventHandler(this.btnProfileRename_Click);
            // 
            // btnProfileCopy
            // 
            this.btnProfileCopy.Location = new System.Drawing.Point(459, 19);
            this.btnProfileCopy.Name = "btnProfileCopy";
            this.btnProfileCopy.Size = new System.Drawing.Size(75, 23);
            this.btnProfileCopy.TabIndex = 3;
            this.btnProfileCopy.Text = "Copy";
            this.ttRoadie.SetToolTip(this.btnProfileCopy, "Copy current profile");
            this.btnProfileCopy.UseVisualStyleBackColor = true;
            this.btnProfileCopy.Click += new System.EventHandler(this.btnProfileCopy_Click);
            // 
            // btnProfileAdd
            // 
            this.btnProfileAdd.Location = new System.Drawing.Point(297, 19);
            this.btnProfileAdd.Name = "btnProfileAdd";
            this.btnProfileAdd.Size = new System.Drawing.Size(75, 23);
            this.btnProfileAdd.TabIndex = 1;
            this.btnProfileAdd.Text = "Add";
            this.ttRoadie.SetToolTip(this.btnProfileAdd, "Add a new profile");
            this.btnProfileAdd.UseVisualStyleBackColor = true;
            this.btnProfileAdd.Click += new System.EventHandler(this.btnProfileAdd_Click);
            // 
            // tcProfile
            // 
            this.tcProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcProfile.Controls.Add(this.tpChannels);
            this.tcProfile.Controls.Add(this.tpPlugins);
            this.tcProfile.Controls.Add(this.tpSortOrders);
            this.tcProfile.Controls.Add(this.tpGroups);
            this.tcProfile.Controls.Add(this.tpNutcracker);
            this.tcProfile.HotTrack = true;
            this.tcProfile.Location = new System.Drawing.Point(12, 117);
            this.tcProfile.Name = "tcProfile";
            this.tcProfile.SelectedIndex = 0;
            this.tcProfile.Size = new System.Drawing.Size(984, 606);
            this.tcProfile.TabIndex = 3;
            this.ttRoadie.SetToolTip(this.tcProfile, "Manage profile channels");
            this.tcProfile.SelectedIndexChanged += new System.EventHandler(this.tcProfile_SelectedIndexChanged);
            // 
            // tpChannels
            // 
            this.tpChannels.Controls.Add(this.dgvChannels);
            this.tpChannels.Controls.Add(this.tcControlArea);
            this.tpChannels.Location = new System.Drawing.Point(4, 22);
            this.tpChannels.Name = "tpChannels";
            this.tpChannels.Padding = new System.Windows.Forms.Padding(3);
            this.tpChannels.Size = new System.Drawing.Size(976, 580);
            this.tpChannels.TabIndex = 0;
            this.tpChannels.Text = "Channels";
            this.tpChannels.UseVisualStyleBackColor = true;
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
            this.dgvChannels.Size = new System.Drawing.Size(700, 580);
            this.dgvChannels.TabIndex = 0;
            this.dgvChannels.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChannels_CellContentDoubleClick);
            this.dgvChannels.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChannels_CellValueChanged);
            this.dgvChannels.SelectionChanged += new System.EventHandler(this.dgvChannels_SelectionChanged);
            this.dgvChannels.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridView1_DragDrop);
            this.dgvChannels.DragOver += new System.Windows.Forms.DragEventHandler(this.dataGridView1_DragOver);
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
            // tcControlArea
            // 
            this.tcControlArea.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tcControlArea.Controls.Add(this.tpChannelControl);
            this.tcControlArea.Controls.Add(this.tpMultiChannel);
            this.tcControlArea.Controls.Add(this.tpMultiColor);
            this.tcControlArea.HideTabs = true;
            this.tcControlArea.Location = new System.Drawing.Point(703, 0);
            this.tcControlArea.Multiline = true;
            this.tcControlArea.Name = "tcControlArea";
            this.tcControlArea.OurMultiline = true;
            this.tcControlArea.SelectedIndex = 0;
            this.tcControlArea.Size = new System.Drawing.Size(273, 580);
            this.tcControlArea.TabIndex = 18;
            // 
            // tpChannelControl
            // 
            this.tpChannelControl.BackColor = System.Drawing.Color.Transparent;
            this.tpChannelControl.Controls.Add(this.gbExportImport);
            this.tpChannelControl.Controls.Add(this.gbChannels);
            this.tpChannelControl.Controls.Add(this.gbEnable);
            this.tpChannelControl.Controls.Add(this.gbColors);
            this.tpChannelControl.Location = new System.Drawing.Point(0, 0);
            this.tpChannelControl.Name = "tpChannelControl";
            this.tpChannelControl.Padding = new System.Windows.Forms.Padding(3);
            this.tpChannelControl.Size = new System.Drawing.Size(273, 580);
            this.tpChannelControl.TabIndex = 0;
            this.tpChannelControl.Text = "Normal";
            this.tpChannelControl.UseVisualStyleBackColor = true;
            // 
            // gbExportImport
            // 
            this.gbExportImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbExportImport.Controls.Add(this.btnChExport);
            this.gbExportImport.Controls.Add(this.btnChImport);
            this.gbExportImport.Location = new System.Drawing.Point(6, 168);
            this.gbExportImport.Name = "gbExportImport";
            this.gbExportImport.Size = new System.Drawing.Size(250, 48);
            this.gbExportImport.TabIndex = 3;
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
            this.ttRoadie.SetToolTip(this.btnChExport, "Export channels to a CSV file");
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
            this.ttRoadie.SetToolTip(this.btnChImport, "Import channels from a CSV file");
            this.btnChImport.UseVisualStyleBackColor = true;
            this.btnChImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // gbChannels
            // 
            this.gbChannels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbChannels.Controls.Add(this.btnChDelete);
            this.gbChannels.Controls.Add(this.btnChAddMulti);
            this.gbChannels.Controls.Add(this.btnChAddOne);
            this.gbChannels.Location = new System.Drawing.Point(6, 6);
            this.gbChannels.Name = "gbChannels";
            this.gbChannels.Size = new System.Drawing.Size(250, 48);
            this.gbChannels.TabIndex = 0;
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
            this.ttRoadie.SetToolTip(this.btnChDelete, "Delete selected channels");
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
            this.ttRoadie.SetToolTip(this.btnChAddMulti, "Add multiple channels using a rule templete");
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
            this.ttRoadie.SetToolTip(this.btnChAddOne, "Add a single white channel");
            this.btnChAddOne.UseVisualStyleBackColor = true;
            this.btnChAddOne.Click += new System.EventHandler(this.btnChAddOne_Click);
            // 
            // gbEnable
            // 
            this.gbEnable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbEnable.Controls.Add(this.btnChEnable);
            this.gbEnable.Controls.Add(this.btnChDisable);
            this.gbEnable.Location = new System.Drawing.Point(6, 114);
            this.gbEnable.Name = "gbEnable";
            this.gbEnable.Size = new System.Drawing.Size(250, 48);
            this.gbEnable.TabIndex = 2;
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
            this.ttRoadie.SetToolTip(this.btnChEnable, "Enable selected channels");
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
            this.ttRoadie.SetToolTip(this.btnChDisable, "Disable selected channels");
            this.btnChDisable.UseVisualStyleBackColor = true;
            this.btnChDisable.Click += new System.EventHandler(this.btnEnableDisable_Click);
            // 
            // gbColors
            // 
            this.gbColors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbColors.Controls.Add(this.btnChColorMulti);
            this.gbColors.Controls.Add(this.btnChColorOne);
            this.gbColors.Location = new System.Drawing.Point(6, 60);
            this.gbColors.Name = "gbColors";
            this.gbColors.Size = new System.Drawing.Size(250, 48);
            this.gbColors.TabIndex = 1;
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
            this.ttRoadie.SetToolTip(this.btnChColorMulti, "Color selected channels with a palette");
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
            this.ttRoadie.SetToolTip(this.btnChColorOne, "Color selected channels with a single color");
            this.btnChColorOne.UseVisualStyleBackColor = true;
            this.btnChColorOne.Click += new System.EventHandler(this.btnChColorOne_Click);
            // 
            // tpMultiChannel
            // 
            this.tpMultiChannel.Controls.Add(this.btnUpdatePreview);
            this.tpMultiChannel.Controls.Add(this.btnMultiChannelCancel);
            this.tpMultiChannel.Controls.Add(this.gbRules);
            this.tpMultiChannel.Controls.Add(this.btnMultiChannelOk);
            this.tpMultiChannel.Controls.Add(this.btnChGenSaveTemplate);
            this.tpMultiChannel.Controls.Add(this.tbChGenNameFormat);
            this.tpMultiChannel.Controls.Add(this.nudChGenChannels);
            this.tpMultiChannel.Controls.Add(this.lblChGenNameFormat);
            this.tpMultiChannel.Controls.Add(this.cbPreview);
            this.tpMultiChannel.Controls.Add(this.lblChGenCount);
            this.tpMultiChannel.Controls.Add(this.lblChGenTemplate);
            this.tpMultiChannel.Controls.Add(this.cbChGenTemplate);
            this.tpMultiChannel.Location = new System.Drawing.Point(0, 0);
            this.tpMultiChannel.Name = "tpMultiChannel";
            this.tpMultiChannel.Padding = new System.Windows.Forms.Padding(3);
            this.tpMultiChannel.Size = new System.Drawing.Size(273, 580);
            this.tpMultiChannel.TabIndex = 1;
            this.tpMultiChannel.Text = "MultiChannel";
            this.tpMultiChannel.UseVisualStyleBackColor = true;
            // 
            // btnUpdatePreview
            // 
            this.btnUpdatePreview.Enabled = false;
            this.btnUpdatePreview.Location = new System.Drawing.Point(160, 6);
            this.btnUpdatePreview.Name = "btnUpdatePreview";
            this.btnUpdatePreview.Size = new System.Drawing.Size(99, 23);
            this.btnUpdatePreview.TabIndex = 10;
            this.btnUpdatePreview.Text = "&Update Preview";
            this.btnUpdatePreview.UseVisualStyleBackColor = true;
            this.btnUpdatePreview.Click += new System.EventHandler(this.btnUpdatePreview_Click);
            // 
            // btnMultiChannelCancel
            // 
            this.btnMultiChannelCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMultiChannelCancel.Location = new System.Drawing.Point(192, 551);
            this.btnMultiChannelCancel.Name = "btnMultiChannelCancel";
            this.btnMultiChannelCancel.Size = new System.Drawing.Size(75, 23);
            this.btnMultiChannelCancel.TabIndex = 4;
            this.btnMultiChannelCancel.Text = "&Cancel";
            this.btnMultiChannelCancel.UseVisualStyleBackColor = true;
            this.btnMultiChannelCancel.Click += new System.EventHandler(this.btnMultiChannelButton_Click);
            // 
            // gbRules
            // 
            this.gbRules.Controls.Add(this.btnRuleDelete);
            this.gbRules.Controls.Add(this.btnRuleAdd);
            this.gbRules.Controls.Add(this.cbRuleRules);
            this.gbRules.Controls.Add(this.btnRuleDown);
            this.gbRules.Controls.Add(this.btnRuleUp);
            this.gbRules.Controls.Add(this.lbRules);
            this.gbRules.Controls.Add(this.panelRuleEditor);
            this.gbRules.Location = new System.Drawing.Point(6, 165);
            this.gbRules.Name = "gbRules";
            this.gbRules.Size = new System.Drawing.Size(261, 303);
            this.gbRules.TabIndex = 8;
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
            this.ttRoadie.SetToolTip(this.btnRuleDelete, "Remove selected rule");
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
            this.ttRoadie.SetToolTip(this.btnRuleAdd, "Add selected rule");
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
            this.ttRoadie.SetToolTip(this.btnRuleDown, "Move Rule Down");
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
            this.ttRoadie.SetToolTip(this.btnRuleUp, "Move rule up");
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
            this.ttRoadie.SetToolTip(this.lbRules, "Channel naming rules");
            this.lbRules.SelectedIndexChanged += new System.EventHandler(this.lbRules_SelectedIndexChanged);
            this.lbRules.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbRules_KeyDown);
            // 
            // panelRuleEditor
            // 
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
            this.ttRoadie.SetToolTip(this.cbRuleEndNum, "Generation numbering is limited");
            this.cbRuleEndNum.UseVisualStyleBackColor = true;
            this.cbRuleEndNum.CheckedChanged += new System.EventHandler(this.cbRuleEndNum_CheckedChanged);
            // 
            // nudRuleIncr
            // 
            this.nudRuleIncr.Location = new System.Drawing.Point(185, 72);
            this.nudRuleIncr.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRuleIncr.Name = "nudRuleIncr";
            this.nudRuleIncr.Size = new System.Drawing.Size(65, 20);
            this.nudRuleIncr.TabIndex = 3;
            this.nudRuleIncr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ttRoadie.SetToolTip(this.nudRuleIncr, "Generation numbering increment");
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
            this.ttRoadie.SetToolTip(this.nudRuleEnd, "Generation end number");
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
            this.ttRoadie.SetToolTip(this.nudRuleStart, "Generation start number");
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
            this.ttRoadie.SetToolTip(this.cbRuleColors, "Use color palette");
            this.cbRuleColors.UseVisualStyleBackColor = true;
            this.cbRuleColors.CheckedChanged += new System.EventHandler(this.cbRuleColors_CheckedChanged);
            // 
            // btnMultiChannelOk
            // 
            this.btnMultiChannelOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMultiChannelOk.Location = new System.Drawing.Point(6, 551);
            this.btnMultiChannelOk.Name = "btnMultiChannelOk";
            this.btnMultiChannelOk.Size = new System.Drawing.Size(141, 23);
            this.btnMultiChannelOk.TabIndex = 3;
            this.btnMultiChannelOk.Text = "&Add Channels to Profile";
            this.btnMultiChannelOk.UseVisualStyleBackColor = true;
            this.btnMultiChannelOk.Click += new System.EventHandler(this.btnMultiChannelButton_Click);
            // 
            // btnChGenSaveTemplate
            // 
            this.btnChGenSaveTemplate.Image = global::VixenPlus.Properties.Resources.saveSm;
            this.btnChGenSaveTemplate.Location = new System.Drawing.Point(237, 40);
            this.btnChGenSaveTemplate.Name = "btnChGenSaveTemplate";
            this.btnChGenSaveTemplate.Size = new System.Drawing.Size(24, 24);
            this.btnChGenSaveTemplate.TabIndex = 1;
            this.ttRoadie.SetToolTip(this.btnChGenSaveTemplate, "Save current template settings");
            this.btnChGenSaveTemplate.UseVisualStyleBackColor = true;
            this.btnChGenSaveTemplate.Click += new System.EventHandler(this.btnChGenSaveTemplate_Click);
            // 
            // tbChGenNameFormat
            // 
            this.tbChGenNameFormat.Location = new System.Drawing.Point(6, 112);
            this.tbChGenNameFormat.MaxLength = 200;
            this.tbChGenNameFormat.Multiline = true;
            this.tbChGenNameFormat.Name = "tbChGenNameFormat";
            this.tbChGenNameFormat.Size = new System.Drawing.Size(253, 47);
            this.tbChGenNameFormat.TabIndex = 2;
            this.ttRoadie.SetToolTip(this.tbChGenNameFormat, "How to format the generated channel names");
            this.tbChGenNameFormat.TextChanged += new System.EventHandler(this.PreviewChannelEvent);
            this.tbChGenNameFormat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbChGenNameFormat_KeyDown);
            // 
            // nudChGenChannels
            // 
            this.nudChGenChannels.Location = new System.Drawing.Point(180, 70);
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
            this.nudChGenChannels.TabIndex = 7;
            this.nudChGenChannels.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ttRoadie.SetToolTip(this.nudChGenChannels, "Set maximum # of channels to generate");
            this.nudChGenChannels.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudChGenChannels.ValueChanged += new System.EventHandler(this.PreviewChannelEvent);
            // 
            // lblChGenNameFormat
            // 
            this.lblChGenNameFormat.AutoSize = true;
            this.lblChGenNameFormat.Location = new System.Drawing.Point(6, 96);
            this.lblChGenNameFormat.Name = "lblChGenNameFormat";
            this.lblChGenNameFormat.Size = new System.Drawing.Size(115, 13);
            this.lblChGenNameFormat.TabIndex = 4;
            this.lblChGenNameFormat.Text = "Channel Name Format:";
            // 
            // cbPreview
            // 
            this.cbPreview.AutoSize = true;
            this.cbPreview.Checked = true;
            this.cbPreview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPreview.Location = new System.Drawing.Point(31, 10);
            this.cbPreview.Name = "cbPreview";
            this.cbPreview.Size = new System.Drawing.Size(123, 17);
            this.cbPreview.TabIndex = 9;
            this.cbPreview.Text = "Enable Live Preview";
            this.cbPreview.UseVisualStyleBackColor = true;
            this.cbPreview.CheckedChanged += new System.EventHandler(this.PreviewChannelEvent);
            // 
            // lblChGenCount
            // 
            this.lblChGenCount.AutoSize = true;
            this.lblChGenCount.Location = new System.Drawing.Point(96, 72);
            this.lblChGenCount.Name = "lblChGenCount";
            this.lblChGenCount.Size = new System.Drawing.Size(76, 13);
            this.lblChGenCount.TabIndex = 6;
            this.lblChGenCount.Text = "# of Channels:";
            // 
            // lblChGenTemplate
            // 
            this.lblChGenTemplate.AutoSize = true;
            this.lblChGenTemplate.Location = new System.Drawing.Point(6, 26);
            this.lblChGenTemplate.Name = "lblChGenTemplate";
            this.lblChGenTemplate.Size = new System.Drawing.Size(54, 13);
            this.lblChGenTemplate.TabIndex = 2;
            this.lblChGenTemplate.Text = "Template:";
            // 
            // cbChGenTemplate
            // 
            this.cbChGenTemplate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbChGenTemplate.FormattingEnabled = true;
            this.cbChGenTemplate.Items.AddRange(new object[] {
            "RGB Channels",
            "Numbered Elements"});
            this.cbChGenTemplate.Location = new System.Drawing.Point(9, 42);
            this.cbChGenTemplate.Name = "cbChGenTemplate";
            this.cbChGenTemplate.Size = new System.Drawing.Size(222, 21);
            this.cbChGenTemplate.TabIndex = 0;
            this.cbChGenTemplate.SelectedIndexChanged += new System.EventHandler(this.cbChGenTemplate_SelectedIndexChanged);
            // 
            // tpMultiColor
            // 
            this.tpMultiColor.Controls.Add(this.btnMultiColorOk);
            this.tpMultiColor.Controls.Add(this.btnMultiColorCancel);
            this.tpMultiColor.Controls.Add(this.colorPaletteColor);
            this.tpMultiColor.Location = new System.Drawing.Point(0, 0);
            this.tpMultiColor.Name = "tpMultiColor";
            this.tpMultiColor.Size = new System.Drawing.Size(273, 580);
            this.tpMultiColor.TabIndex = 2;
            this.tpMultiColor.Text = "MutliColor";
            this.tpMultiColor.UseVisualStyleBackColor = true;
            // 
            // btnMultiColorOk
            // 
            this.btnMultiColorOk.Location = new System.Drawing.Point(6, 63);
            this.btnMultiColorOk.Name = "btnMultiColorOk";
            this.btnMultiColorOk.Size = new System.Drawing.Size(75, 23);
            this.btnMultiColorOk.TabIndex = 2;
            this.btnMultiColorOk.Text = "OK";
            this.btnMultiColorOk.UseVisualStyleBackColor = true;
            this.btnMultiColorOk.Click += new System.EventHandler(this.btnMultiColor_Click);
            // 
            // btnMultiColorCancel
            // 
            this.btnMultiColorCancel.Location = new System.Drawing.Point(192, 63);
            this.btnMultiColorCancel.Name = "btnMultiColorCancel";
            this.btnMultiColorCancel.Size = new System.Drawing.Size(75, 23);
            this.btnMultiColorCancel.TabIndex = 1;
            this.btnMultiColorCancel.Text = "Cancel";
            this.btnMultiColorCancel.UseVisualStyleBackColor = true;
            this.btnMultiColorCancel.Click += new System.EventHandler(this.btnMultiColor_Click);
            // 
            // colorPaletteColor
            // 
            this.colorPaletteColor.Location = new System.Drawing.Point(84, 6);
            this.colorPaletteColor.Name = "colorPaletteColor";
            this.colorPaletteColor.Size = new System.Drawing.Size(104, 50);
            this.colorPaletteColor.TabIndex = 0;
            // 
            // tpPlugins
            // 
            this.tpPlugins.Controls.Add(this.gbSetup);
            this.tpPlugins.Controls.Add(this.listViewPlugins);
            this.tpPlugins.Controls.Add(this.label4);
            this.tpPlugins.Controls.Add(this.label3);
            this.tpPlugins.Controls.Add(this.buttonRemove);
            this.tpPlugins.Controls.Add(this.buttonUse);
            this.tpPlugins.Controls.Add(this.groupBox1);
            this.tpPlugins.Controls.Add(this.checkedListBoxSequencePlugins);
            this.tpPlugins.Controls.Add(this.buttonCancel);
            this.tpPlugins.Controls.Add(this.buttonOK);
            this.tpPlugins.Controls.Add(this.buttonPluginSetup);
            this.tpPlugins.Controls.Add(this.textBoxChannelTo);
            this.tpPlugins.Controls.Add(this.label2);
            this.tpPlugins.Controls.Add(this.label1);
            this.tpPlugins.Controls.Add(this.textBoxChannelFrom);
            this.tpPlugins.Location = new System.Drawing.Point(4, 22);
            this.tpPlugins.Name = "tpPlugins";
            this.tpPlugins.Padding = new System.Windows.Forms.Padding(3);
            this.tpPlugins.Size = new System.Drawing.Size(976, 580);
            this.tpPlugins.TabIndex = 1;
            this.tpPlugins.Text = "Plugins";
            this.tpPlugins.UseVisualStyleBackColor = true;
            this.tpPlugins.Enter += new System.EventHandler(this.PluginListDialog_Load);
            this.tpPlugins.Leave += new System.EventHandler(this.PluginListDialog_FormClosing);
            // 
            // gbSetup
            // 
            this.gbSetup.Location = new System.Drawing.Point(6, 169);
            this.gbSetup.Name = "gbSetup";
            this.gbSetup.Size = new System.Drawing.Size(958, 225);
            this.gbSetup.TabIndex = 33;
            this.gbSetup.TabStop = false;
            this.gbSetup.Text = "Plugin Setup";
            // 
            // listViewPlugins
            // 
            this.listViewPlugins.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.pluginName});
            listViewGroup1.Header = "Output";
            listViewGroup1.Name = "listViewGroupOutput";
            listViewGroup2.Header = "Input";
            listViewGroup2.Name = "listViewGroupInput";
            this.listViewPlugins.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            this.listViewPlugins.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewPlugins.HideSelection = false;
            this.listViewPlugins.Location = new System.Drawing.Point(6, 24);
            this.listViewPlugins.MultiSelect = false;
            this.listViewPlugins.Name = "listViewPlugins";
            this.listViewPlugins.Size = new System.Drawing.Size(196, 139);
            this.listViewPlugins.TabIndex = 32;
            this.listViewPlugins.UseCompatibleStateImageBehavior = false;
            this.listViewPlugins.View = System.Windows.Forms.View.Details;
            this.listViewPlugins.SelectedIndexChanged += new System.EventHandler(this.listViewPlugins_SelectedIndexChanged);
            this.listViewPlugins.DoubleClick += new System.EventHandler(this.listViewPlugins_DoubleClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(289, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Plugins in Use";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Available Plugins";
            // 
            // buttonRemove
            // 
            this.buttonRemove.Enabled = false;
            this.buttonRemove.Location = new System.Drawing.Point(208, 142);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(75, 23);
            this.buttonRemove.TabIndex = 18;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonUse
            // 
            this.buttonUse.Enabled = false;
            this.buttonUse.Location = new System.Drawing.Point(208, 113);
            this.buttonUse.Name = "buttonUse";
            this.buttonUse.Size = new System.Drawing.Size(75, 23);
            this.buttonUse.TabIndex = 17;
            this.buttonUse.Text = "--- Use -->";
            this.buttonUse.UseVisualStyleBackColor = true;
            this.buttonUse.Click += new System.EventHandler(this.buttonUse_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listViewOutputPorts);
            this.groupBox1.Location = new System.Drawing.Point(6, 400);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(964, 145);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Port Configurations";
            // 
            // listViewOutputPorts
            // 
            this.listViewOutputPorts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderPortTypeName,
            this.columnHeaderPortTypeIndex,
            this.columnHeaderExpandButton,
            this.columnHeaderPluginName});
            this.listViewOutputPorts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewOutputPorts.Location = new System.Drawing.Point(6, 19);
            this.listViewOutputPorts.Name = "listViewOutputPorts";
            this.listViewOutputPorts.OwnerDraw = true;
            this.listViewOutputPorts.Size = new System.Drawing.Size(952, 120);
            this.listViewOutputPorts.TabIndex = 0;
            this.listViewOutputPorts.UseCompatibleStateImageBehavior = false;
            this.listViewOutputPorts.View = System.Windows.Forms.View.Details;
            this.listViewOutputPorts.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.listViewOutputPorts_DrawItem);
            this.listViewOutputPorts.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.listViewOutputPorts_DrawSubItem);
            this.listViewOutputPorts.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewOutputPorts_MouseDown);
            // 
            // columnHeaderPortTypeName
            // 
            this.columnHeaderPortTypeName.Text = "PortTypeName";
            this.columnHeaderPortTypeName.Width = 55;
            // 
            // columnHeaderPortTypeIndex
            // 
            this.columnHeaderPortTypeIndex.Text = "PortTypeIndex";
            this.columnHeaderPortTypeIndex.Width = 41;
            // 
            // columnHeaderExpandButton
            // 
            this.columnHeaderExpandButton.Text = "";
            this.columnHeaderExpandButton.Width = 41;
            // 
            // columnHeaderPluginName
            // 
            this.columnHeaderPluginName.Text = "PluginName";
            this.columnHeaderPluginName.Width = 251;
            // 
            // checkedListBoxSequencePlugins
            // 
            this.checkedListBoxSequencePlugins.FormattingEnabled = true;
            this.checkedListBoxSequencePlugins.Location = new System.Drawing.Point(292, 25);
            this.checkedListBoxSequencePlugins.Name = "checkedListBoxSequencePlugins";
            this.checkedListBoxSequencePlugins.Size = new System.Drawing.Size(171, 139);
            this.checkedListBoxSequencePlugins.TabIndex = 20;
            this.checkedListBoxSequencePlugins.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxSequencePlugins_ItemCheck);
            this.checkedListBoxSequencePlugins.SelectedIndexChanged += new System.EventHandler(this.listBoxSequencePlugins_SelectedIndexChanged);
            this.checkedListBoxSequencePlugins.DoubleClick += new System.EventHandler(this.checkedListBoxSequencePlugins_DoubleClick);
            this.checkedListBoxSequencePlugins.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxSequencePlugins_KeyDown);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(814, 551);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 29;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Visible = false;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(895, 551);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 31;
            this.buttonOK.Text = "Done";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonPluginSetup
            // 
            this.buttonPluginSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPluginSetup.Enabled = false;
            this.buttonPluginSetup.Location = new System.Drawing.Point(886, 60);
            this.buttonPluginSetup.Name = "buttonPluginSetup";
            this.buttonPluginSetup.Size = new System.Drawing.Size(75, 23);
            this.buttonPluginSetup.TabIndex = 25;
            this.buttonPluginSetup.Text = "Plugin Setup";
            this.buttonPluginSetup.UseVisualStyleBackColor = true;
            this.buttonPluginSetup.Click += new System.EventHandler(this.buttonPluginSetup_Click);
            // 
            // textBoxChannelTo
            // 
            this.textBoxChannelTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxChannelTo.Location = new System.Drawing.Point(936, 34);
            this.textBoxChannelTo.MaxLength = 4;
            this.textBoxChannelTo.Name = "textBoxChannelTo";
            this.textBoxChannelTo.Size = new System.Drawing.Size(34, 20);
            this.textBoxChannelTo.TabIndex = 24;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(914, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "to";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(897, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Channels";
            // 
            // textBoxChannelFrom
            // 
            this.textBoxChannelFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxChannelFrom.Location = new System.Drawing.Point(874, 34);
            this.textBoxChannelFrom.MaxLength = 4;
            this.textBoxChannelFrom.Name = "textBoxChannelFrom";
            this.textBoxChannelFrom.Size = new System.Drawing.Size(34, 20);
            this.textBoxChannelFrom.TabIndex = 22;
            // 
            // tpSortOrders
            // 
            this.tpSortOrders.Controls.Add(this.btnSrtDelete);
            this.tpSortOrders.Controls.Add(this.btnSrtSave);
            this.tpSortOrders.Controls.Add(this.cbSrtOrders);
            this.tpSortOrders.Location = new System.Drawing.Point(4, 22);
            this.tpSortOrders.Name = "tpSortOrders";
            this.tpSortOrders.Size = new System.Drawing.Size(976, 580);
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
            // tpGroups
            // 
            this.tpGroups.Controls.Add(this.btnGraButton);
            this.tpGroups.Location = new System.Drawing.Point(4, 22);
            this.tpGroups.Name = "tpGroups";
            this.tpGroups.Size = new System.Drawing.Size(976, 580);
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
            // tpNutcracker
            // 
            this.tpNutcracker.Controls.Add(this.btnNcaButton);
            this.tpNutcracker.Location = new System.Drawing.Point(4, 22);
            this.tpNutcracker.Name = "tpNutcracker";
            this.tpNutcracker.Size = new System.Drawing.Size(976, 580);
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
            this.btnOkay.Location = new System.Drawing.Point(830, 31);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 1;
            this.btnOkay.Text = "OK";
            this.ttRoadie.SetToolTip(this.btnOkay, "Save all changes");
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(911, 31);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.ttRoadie.SetToolTip(this.btnCancel, "Cancel all changes");
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // previewTimer
            // 
            this.previewTimer.Interval = 200;
            this.previewTimer.Tick += new System.EventHandler(this.previewTimer_Tick);
            // 
            // VixenPlusRoadie
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.tcProfile);
            this.Controls.Add(this.gbProfiles);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "VixenPlusRoadie";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Vixen+ {Beta} - ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VixenPlusRoadie_FormClosing);
            this.Shown += new System.EventHandler(this.FrmProfileManager_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmProfileManager_KeyDown);
            this.Resize += new System.EventHandler(this.frmProfileManager_Resize);
            this.gbProfiles.ResumeLayout(false);
            this.tcProfile.ResumeLayout(false);
            this.tpChannels.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChannels)).EndInit();
            this.tcControlArea.ResumeLayout(false);
            this.tpChannelControl.ResumeLayout(false);
            this.gbExportImport.ResumeLayout(false);
            this.gbChannels.ResumeLayout(false);
            this.gbEnable.ResumeLayout(false);
            this.gbColors.ResumeLayout(false);
            this.tpMultiChannel.ResumeLayout(false);
            this.tpMultiChannel.PerformLayout();
            this.gbRules.ResumeLayout(false);
            this.panelRuleEditor.ResumeLayout(false);
            this.panelRuleEditor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRuleIncr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRuleEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRuleStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudChGenChannels)).EndInit();
            this.tpMultiColor.ResumeLayout(false);
            this.tpPlugins.ResumeLayout(false);
            this.tpPlugins.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tpSortOrders.ResumeLayout(false);
            this.tpGroups.ResumeLayout(false);
            this.tpNutcracker.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbProfiles;
        private System.Windows.Forms.GroupBox gbProfiles;
        private System.Windows.Forms.Button btnProfileDelete;
        private System.Windows.Forms.Button btnProfileRename;
        private System.Windows.Forms.Button btnProfileCopy;
        private System.Windows.Forms.Button btnProfileAdd;
        private System.Windows.Forms.TabControl tcProfile;
        private System.Windows.Forms.TabPage tpChannels;
        private System.Windows.Forms.TabPage tpPlugins;
        private System.Windows.Forms.DataGridView dgvChannels;
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
        private System.Windows.Forms.GroupBox gbEnable;
        private System.Windows.Forms.Button btnChEnable;
        private System.Windows.Forms.Button btnChDisable;
        private System.Windows.Forms.GroupBox gbColors;
        private System.Windows.Forms.Button btnChColorMulti;
        private System.Windows.Forms.Button btnChColorOne;
        private System.Windows.Forms.GroupBox gbChannels;
        private System.Windows.Forms.Button btnChAddMulti;
        private System.Windows.Forms.Button btnChAddOne;
        private System.Windows.Forms.Button btnGraButton;
        private System.Windows.Forms.Button btnNcaButton;
        private System.Windows.Forms.Button btnChDelete;
        private System.Windows.Forms.Button btnMultiChannelCancel;
        private System.Windows.Forms.Button btnMultiChannelOk;
        private System.Windows.Forms.GroupBox gbRules;
        private System.Windows.Forms.Button btnRuleAdd;
        private System.Windows.Forms.ComboBox cbRuleRules;
        private System.Windows.Forms.Button btnRuleDown;
        private System.Windows.Forms.Button btnRuleUp;
        private System.Windows.Forms.ListBox lbRules;
        private System.Windows.Forms.NumericUpDown nudChGenChannels;
        private System.Windows.Forms.Label lblChGenCount;
        private System.Windows.Forms.TextBox tbChGenNameFormat;
        private System.Windows.Forms.Label lblChGenNameFormat;
        private System.Windows.Forms.ComboBox cbChGenTemplate;
        private System.Windows.Forms.Label lblChGenTemplate;
        private System.Windows.Forms.Panel panelRuleEditor;
        private System.Windows.Forms.Button btnRuleDelete;
        private System.Windows.Forms.ToolTip ttRoadie;
        private System.Windows.Forms.TextBox tbRuleWords;
        private System.Windows.Forms.Label lblRulePrompt;
        private System.Windows.Forms.CheckBox cbRuleColors;
        private System.Windows.Forms.Button btnChGenSaveTemplate;
        private System.Windows.Forms.CheckBox cbRuleEndNum;
        private System.Windows.Forms.NumericUpDown nudRuleIncr;
        private System.Windows.Forms.NumericUpDown nudRuleEnd;
        private System.Windows.Forms.NumericUpDown nudRuleStart;
        private System.Windows.Forms.Label lblRuleIncr;
        private System.Windows.Forms.Label lblRuleStartNum;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ChannelEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChannelNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChannelName;
        private System.Windows.Forms.DataGridViewTextBoxColumn OutputChannel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChannelColor;
        private TabControl tcControlArea;
        private System.Windows.Forms.TabPage tpMultiChannel;
        private System.Windows.Forms.TabPage tpChannelControl;
        private System.Windows.Forms.TabPage tpMultiColor;
        private ColorPalette colorPaletteChannel;
        private ColorPalette colorPaletteColor;
        private System.Windows.Forms.Button btnMultiColorOk;
        private System.Windows.Forms.Button btnMultiColorCancel;
        private System.Windows.Forms.CheckBox cbPreview;
        private System.Windows.Forms.Timer previewTimer;
        private System.Windows.Forms.Button btnUpdatePreview;
        private System.Windows.Forms.Button btnProfileSave;
        private System.Windows.Forms.ListView listViewPlugins;
        private System.Windows.Forms.ColumnHeader pluginName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonUse;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView listViewOutputPorts;
        private System.Windows.Forms.ColumnHeader columnHeaderPortTypeName;
        private System.Windows.Forms.ColumnHeader columnHeaderPortTypeIndex;
        private System.Windows.Forms.ColumnHeader columnHeaderExpandButton;
        private System.Windows.Forms.ColumnHeader columnHeaderPluginName;
        private System.Windows.Forms.CheckedListBox checkedListBoxSequencePlugins;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonPluginSetup;
        private System.Windows.Forms.TextBox textBoxChannelTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxChannelFrom;
        private System.Windows.Forms.GroupBox gbSetup;
    }
}