
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using VixenPlus.Properties;

using VixenPlusCommon;

using TabControl = System.Windows.Forms.TabControl;

namespace VixenPlus.Dialogs
{
    partial class VixenPlusRoadie
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.components = new Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            this.cbProfiles = new ComboBox();
            this.gbProfiles = new GroupBox();
            this.btnProfileSave = new Button();
            this.btnProfileDelete = new Button();
            this.btnProfileRename = new Button();
            this.btnProfileCopy = new Button();
            this.btnProfileAdd = new Button();
            this.tcProfile = new TabControl();
            this.tpChannels = new TabPage();
            this.dgvChannels = new DataGridView();
            this.ChannelEnabled = new DataGridViewCheckBoxColumn();
            this.ChannelNum = new DataGridViewTextBoxColumn();
            this.ChannelName = new DataGridViewTextBoxColumn();
            this.OutputChannel = new DataGridViewTextBoxColumn();
            this.ChannelColor = new DataGridViewTextBoxColumn();
            this.tpPlugins = new TabPage();
            this.cbAvailablePlugIns = new ComboBox();
            this.btnRemovePlugIn = new Button();
            this.btnAddPlugIn = new Button();
            this.dgvPlugIns = new DataGridView();
            this.colPlugInName = new DataGridViewTextBoxColumn();
            this.colPlugInEnabled = new DataGridViewCheckBoxColumn();
            this.colPlugInStartChannel = new DataGridViewTextBoxColumn();
            this.colPlugInEndChannel = new DataGridViewTextBoxColumn();
            this.colPlugInConfiguration = new DataGridViewTextBoxColumn();
            this.gbSetup = new GroupBox();
            this.pSetup = new Panel();
            this.tpGroups = new TabPage();
            this.pGroups = new Panel();
            this.btnOkay = new Button();
            this.btnCancel = new Button();
            this.ttRoadie = new ToolTip(this.components);
            this.previewTimer = new Timer(this.components);
            this.dataGridViewDisableButtonColumn1 = new DataGridViewDisableButtonColumn();
            this.dataGridViewDisableButtonColumn2 = new DataGridViewDisableButtonColumn();
            this.tcControlArea = new VixenPlusCommon.TabControl(this.components);
            this.tpChannelControl = new TabPage();
            this.gbExportImport = new GroupBox();
            this.btnChExport = new Button();
            this.btnChImport = new Button();
            this.gbChannels = new GroupBox();
            this.btnChDelete = new Button();
            this.btnChAddMulti = new Button();
            this.btnChAddOne = new Button();
            this.gbEnable = new GroupBox();
            this.btnChEnable = new Button();
            this.btnChDisable = new Button();
            this.gbColors = new GroupBox();
            this.btnChColorMulti = new Button();
            this.btnChColorOne = new Button();
            this.tpMultiChannel = new TabPage();
            this.btnUpdatePreview = new Button();
            this.btnMultiChannelCancel = new Button();
            this.gbRules = new GroupBox();
            this.btnRuleDelete = new Button();
            this.btnRuleAdd = new Button();
            this.cbRuleRules = new ComboBox();
            this.btnRuleDown = new Button();
            this.btnRuleUp = new Button();
            this.lbRules = new ListBox();
            this.panelRuleEditor = new Panel();
            this.colorPaletteChannel = new ColorPalette();
            this.cbRuleEndNum = new CheckBox();
            this.nudRuleIncr = new NumericUpDown();
            this.nudRuleEnd = new NumericUpDown();
            this.nudRuleStart = new NumericUpDown();
            this.lblRuleIncr = new Label();
            this.lblRuleStartNum = new Label();
            this.tbRuleWords = new TextBox();
            this.lblRulePrompt = new Label();
            this.cbRuleColors = new CheckBox();
            this.btnMultiChannelOk = new Button();
            this.btnChGenSaveTemplate = new Button();
            this.tbChGenNameFormat = new TextBox();
            this.nudChGenChannels = new NumericUpDown();
            this.lblChGenNameFormat = new Label();
            this.cbPreview = new CheckBox();
            this.lblChGenCount = new Label();
            this.lblChGenTemplate = new Label();
            this.cbChGenTemplate = new ComboBox();
            this.tpMultiColor = new TabPage();
            this.btnMultiColorOk = new Button();
            this.btnMultiColorCancel = new Button();
            this.colorPaletteColor = new ColorPalette();
            this.dataGridViewDisableButtonColumn3 = new DataGridViewDisableButtonColumn();
            this.colPlugInSetup = new DataGridViewDisableButtonColumn();
            this.gbProfiles.SuspendLayout();
            this.tcProfile.SuspendLayout();
            this.tpChannels.SuspendLayout();
            ((ISupportInitialize)(this.dgvChannels)).BeginInit();
            this.tpPlugins.SuspendLayout();
            ((ISupportInitialize)(this.dgvPlugIns)).BeginInit();
            this.gbSetup.SuspendLayout();
            this.tpGroups.SuspendLayout();
            this.tcControlArea.SuspendLayout();
            this.tpChannelControl.SuspendLayout();
            this.gbExportImport.SuspendLayout();
            this.gbChannels.SuspendLayout();
            this.gbEnable.SuspendLayout();
            this.gbColors.SuspendLayout();
            this.tpMultiChannel.SuspendLayout();
            this.gbRules.SuspendLayout();
            this.panelRuleEditor.SuspendLayout();
            ((ISupportInitialize)(this.nudRuleIncr)).BeginInit();
            ((ISupportInitialize)(this.nudRuleEnd)).BeginInit();
            ((ISupportInitialize)(this.nudRuleStart)).BeginInit();
            ((ISupportInitialize)(this.nudChGenChannels)).BeginInit();
            this.tpMultiColor.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbProfiles
            // 
            this.cbProfiles.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbProfiles.FormattingEnabled = true;
            this.cbProfiles.Location = new Point(10, 19);
            this.cbProfiles.Name = "cbProfiles";
            this.cbProfiles.Size = new Size(281, 21);
            this.cbProfiles.TabIndex = 0;
            this.cbProfiles.SelectedIndexChanged += new EventHandler(this.cbProfiles_SelectedIndexChanged);
            // 
            // gbProfiles
            // 
            this.gbProfiles.Controls.Add(this.btnProfileSave);
            this.gbProfiles.Controls.Add(this.btnProfileDelete);
            this.gbProfiles.Controls.Add(this.btnProfileRename);
            this.gbProfiles.Controls.Add(this.btnProfileCopy);
            this.gbProfiles.Controls.Add(this.btnProfileAdd);
            this.gbProfiles.Controls.Add(this.cbProfiles);
            this.gbProfiles.Location = new Point(12, 12);
            this.gbProfiles.Name = "gbProfiles";
            this.gbProfiles.Size = new Size(704, 52);
            this.gbProfiles.TabIndex = 0;
            this.gbProfiles.TabStop = false;
            this.gbProfiles.Text = "Profile";
            // 
            // btnProfileSave
            // 
            this.btnProfileSave.Location = new Point(621, 19);
            this.btnProfileSave.Name = "btnProfileSave";
            this.btnProfileSave.Size = new Size(75, 23);
            this.btnProfileSave.TabIndex = 5;
            this.btnProfileSave.Text = "&Save";
            this.ttRoadie.SetToolTip(this.btnProfileSave, "Delete current profile");
            this.btnProfileSave.UseVisualStyleBackColor = true;
            this.btnProfileSave.Click += new EventHandler(this.btnProfileSave_Click);
            // 
            // btnProfileDelete
            // 
            this.btnProfileDelete.Location = new Point(540, 19);
            this.btnProfileDelete.Name = "btnProfileDelete";
            this.btnProfileDelete.Size = new Size(75, 23);
            this.btnProfileDelete.TabIndex = 4;
            this.btnProfileDelete.Text = "Delete";
            this.ttRoadie.SetToolTip(this.btnProfileDelete, "Delete current profile");
            this.btnProfileDelete.UseVisualStyleBackColor = true;
            this.btnProfileDelete.Click += new EventHandler(this.btnProfileDelete_Click);
            // 
            // btnProfileRename
            // 
            this.btnProfileRename.Location = new Point(378, 19);
            this.btnProfileRename.Name = "btnProfileRename";
            this.btnProfileRename.Size = new Size(75, 23);
            this.btnProfileRename.TabIndex = 2;
            this.btnProfileRename.Text = "Rename";
            this.ttRoadie.SetToolTip(this.btnProfileRename, "Rename current profile");
            this.btnProfileRename.UseVisualStyleBackColor = true;
            this.btnProfileRename.Click += new EventHandler(this.btnProfileRename_Click);
            // 
            // btnProfileCopy
            // 
            this.btnProfileCopy.Location = new Point(459, 19);
            this.btnProfileCopy.Name = "btnProfileCopy";
            this.btnProfileCopy.Size = new Size(75, 23);
            this.btnProfileCopy.TabIndex = 3;
            this.btnProfileCopy.Text = "Copy";
            this.ttRoadie.SetToolTip(this.btnProfileCopy, "Copy current profile");
            this.btnProfileCopy.UseVisualStyleBackColor = true;
            this.btnProfileCopy.Click += new EventHandler(this.btnProfileCopy_Click);
            // 
            // btnProfileAdd
            // 
            this.btnProfileAdd.Location = new Point(297, 19);
            this.btnProfileAdd.Name = "btnProfileAdd";
            this.btnProfileAdd.Size = new Size(75, 23);
            this.btnProfileAdd.TabIndex = 1;
            this.btnProfileAdd.Text = "Add";
            this.ttRoadie.SetToolTip(this.btnProfileAdd, "Add a new profile");
            this.btnProfileAdd.UseVisualStyleBackColor = true;
            this.btnProfileAdd.Click += new EventHandler(this.btnProfileAdd_Click);
            // 
            // tcProfile
            // 
            this.tcProfile.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
                        | AnchorStyles.Left)
                        | AnchorStyles.Right)));
            this.tcProfile.Controls.Add(this.tpChannels);
            this.tcProfile.Controls.Add(this.tpPlugins);
            this.tcProfile.Controls.Add(this.tpGroups);
            this.tcProfile.HotTrack = true;
            this.tcProfile.Location = new Point(12, 70);
            this.tcProfile.Name = "tcProfile";
            this.tcProfile.SelectedIndex = 0;
            this.tcProfile.Size = new Size(984, 561);
            this.tcProfile.TabIndex = 3;
            this.ttRoadie.SetToolTip(this.tcProfile, "Manage profile channels");
            this.tcProfile.Visible = false;
            this.tcProfile.SelectedIndexChanged += new EventHandler(this.tcProfile_SelectedIndexChanged);
            // 
            // tpChannels
            // 
            this.tpChannels.Controls.Add(this.dgvChannels);
            this.tpChannels.Controls.Add(this.tcControlArea);
            this.tpChannels.Location = new Point(4, 22);
            this.tpChannels.Name = "tpChannels";
            this.tpChannels.Padding = new Padding(3);
            this.tpChannels.Size = new Size(976, 535);
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
            this.dgvChannels.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvChannels.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvChannels.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dgvChannels.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChannels.Columns.AddRange(new DataGridViewColumn[] {
            this.ChannelEnabled,
            this.ChannelNum,
            this.ChannelName,
            this.OutputChannel,
            this.ChannelColor});
            this.dgvChannels.Location = new Point(0, 0);
            this.dgvChannels.Name = "dgvChannels";
            this.dgvChannels.RowHeadersWidth = 25;
            this.dgvChannels.Size = new Size(700, 535);
            this.dgvChannels.TabIndex = 0;
            this.dgvChannels.CellContentDoubleClick += new DataGridViewCellEventHandler(this.dgvChannels_CellContentDoubleClick);
            this.dgvChannels.CellValueChanged += new DataGridViewCellEventHandler(this.dgvChannels_CellValueChanged);
            this.dgvChannels.SelectionChanged += new EventHandler(this.dgvChannels_SelectionChanged);
            this.dgvChannels.DragDrop += new DragEventHandler(this.dataGridView1_DragDrop);
            this.dgvChannels.DragOver += new DragEventHandler(this.dataGridView1_DragOver);
            this.dgvChannels.QueryContinueDrag += new QueryContinueDragEventHandler(this.dgvChannels_QueryContinueDrag);
            this.dgvChannels.KeyDown += new KeyEventHandler(this.dgvChannels_KeyDown);
            this.dgvChannels.MouseDown += new MouseEventHandler(this.dataGridView1_MouseDown);
            this.dgvChannels.MouseMove += new MouseEventHandler(this.dataGridView1_MouseMove);
            // 
            // ChannelEnabled
            // 
            this.ChannelEnabled.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            this.ChannelEnabled.FillWeight = 1F;
            this.ChannelEnabled.HeaderText = "Ch Enabled";
            this.ChannelEnabled.MinimumWidth = 87;
            this.ChannelEnabled.Name = "ChannelEnabled";
            this.ChannelEnabled.SortMode = DataGridViewColumnSortMode.Automatic;
            this.ChannelEnabled.ToolTipText = "Is the channel outputing data";
            this.ChannelEnabled.Width = 87;
            // 
            // ChannelNum
            // 
            this.ChannelNum.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
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
            this.ChannelName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.ChannelName.FillWeight = 10F;
            this.ChannelName.HeaderText = "Channel Name";
            this.ChannelName.Name = "ChannelName";
            this.ChannelName.ToolTipText = "Human Readable Name of the Channel";
            // 
            // OutputChannel
            // 
            this.OutputChannel.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
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
            this.ChannelColor.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            this.ChannelColor.FillWeight = 1F;
            this.ChannelColor.HeaderText = "Channel Color";
            this.ChannelColor.MinimumWidth = 98;
            this.ChannelColor.Name = "ChannelColor";
            this.ChannelColor.ReadOnly = true;
            this.ChannelColor.ToolTipText = "Sequencer Color of the Channel";
            this.ChannelColor.Width = 98;
            // 
            // tpPlugins
            // 
            this.tpPlugins.Controls.Add(this.cbAvailablePlugIns);
            this.tpPlugins.Controls.Add(this.btnRemovePlugIn);
            this.tpPlugins.Controls.Add(this.btnAddPlugIn);
            this.tpPlugins.Controls.Add(this.dgvPlugIns);
            this.tpPlugins.Controls.Add(this.gbSetup);
            this.tpPlugins.Location = new Point(4, 22);
            this.tpPlugins.Name = "tpPlugins";
            this.tpPlugins.Padding = new Padding(3);
            this.tpPlugins.Size = new Size(976, 535);
            this.tpPlugins.TabIndex = 1;
            this.tpPlugins.Text = "Plugins";
            this.tpPlugins.UseVisualStyleBackColor = true;
            // 
            // cbAvailablePlugIns
            // 
            this.cbAvailablePlugIns.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbAvailablePlugIns.FormattingEnabled = true;
            this.cbAvailablePlugIns.Location = new Point(7, 6);
            this.cbAvailablePlugIns.Name = "cbAvailablePlugIns";
            this.cbAvailablePlugIns.Size = new Size(280, 21);
            this.cbAvailablePlugIns.TabIndex = 36;
            this.cbAvailablePlugIns.SelectedIndexChanged += new EventHandler(this.cbAvailablePlugIns_SelectedIndexChanged);
            // 
            // btnRemovePlugIn
            // 
            this.btnRemovePlugIn.Enabled = false;
            this.btnRemovePlugIn.Location = new Point(536, 5);
            this.btnRemovePlugIn.Name = "btnRemovePlugIn";
            this.btnRemovePlugIn.Size = new Size(75, 23);
            this.btnRemovePlugIn.TabIndex = 18;
            this.btnRemovePlugIn.Text = "Remove";
            this.btnRemovePlugIn.UseVisualStyleBackColor = true;
            this.btnRemovePlugIn.Click += new EventHandler(this.buttonRemove_Click);
            // 
            // btnAddPlugIn
            // 
            this.btnAddPlugIn.Enabled = false;
            this.btnAddPlugIn.Location = new Point(293, 5);
            this.btnAddPlugIn.Name = "btnAddPlugIn";
            this.btnAddPlugIn.Size = new Size(75, 23);
            this.btnAddPlugIn.TabIndex = 17;
            this.btnAddPlugIn.Text = "Add";
            this.btnAddPlugIn.UseVisualStyleBackColor = true;
            this.btnAddPlugIn.Click += new EventHandler(this.buttonUse_Click);
            // 
            // dgvPlugIns
            // 
            this.dgvPlugIns.AllowUserToAddRows = false;
            this.dgvPlugIns.AllowUserToDeleteRows = false;
            this.dgvPlugIns.AllowUserToOrderColumns = true;
            this.dgvPlugIns.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
                        | AnchorStyles.Left)
                        | AnchorStyles.Right)));
            this.dgvPlugIns.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgvPlugIns.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPlugIns.Columns.AddRange(new DataGridViewColumn[] {
            this.colPlugInName,
            this.colPlugInEnabled,
            this.colPlugInStartChannel,
            this.colPlugInEndChannel,
            this.colPlugInConfiguration,
            this.colPlugInSetup});
            this.dgvPlugIns.Location = new Point(6, 35);
            this.dgvPlugIns.MultiSelect = false;
            this.dgvPlugIns.Name = "dgvPlugIns";
            this.dgvPlugIns.RowHeadersVisible = false;
            this.dgvPlugIns.Size = new Size(964, 228);
            this.dgvPlugIns.TabIndex = 35;
            this.dgvPlugIns.CellClick += new DataGridViewCellEventHandler(this.dgvPlugIns_CellClick);
            this.dgvPlugIns.CellValueChanged += new DataGridViewCellEventHandler(this.dgvPlugIns_CellValueChanged);
            this.dgvPlugIns.RowEnter += new DataGridViewCellEventHandler(this.dgvPlugIns_RowEnter);
            this.dgvPlugIns.RowLeave += new DataGridViewCellEventHandler(this.dgvPlugIns_RowLeave);
            // 
            // colPlugInName
            // 
            this.colPlugInName.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colPlugInName.FillWeight = 150F;
            this.colPlugInName.HeaderText = "Plug In Name";
            this.colPlugInName.Name = "colPlugInName";
            this.colPlugInName.ReadOnly = true;
            this.colPlugInName.Width = 96;
            // 
            // colPlugInEnabled
            // 
            this.colPlugInEnabled.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.colPlugInEnabled.HeaderText = "Enabled";
            this.colPlugInEnabled.Name = "colPlugInEnabled";
            this.colPlugInEnabled.Width = 52;
            // 
            // colPlugInStartChannel
            // 
            this.colPlugInStartChannel.HeaderText = "Start Channel";
            this.colPlugInStartChannel.Name = "colPlugInStartChannel";
            // 
            // colPlugInEndChannel
            // 
            this.colPlugInEndChannel.HeaderText = "End Channel";
            this.colPlugInEndChannel.Name = "colPlugInEndChannel";
            // 
            // colPlugInConfiguration
            // 
            this.colPlugInConfiguration.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            this.colPlugInConfiguration.DefaultCellStyle = dataGridViewCellStyle1;
            this.colPlugInConfiguration.HeaderText = "Current Configuration";
            this.colPlugInConfiguration.Name = "colPlugInConfiguration";
            this.colPlugInConfiguration.ReadOnly = true;
            // 
            // gbSetup
            // 
            this.gbSetup.Anchor = ((AnchorStyles)(((AnchorStyles.Bottom | AnchorStyles.Left)
                        | AnchorStyles.Right)));
            this.gbSetup.Controls.Add(this.pSetup);
            this.gbSetup.Location = new Point(6, 269);
            this.gbSetup.Name = "gbSetup";
            this.gbSetup.Size = new Size(964, 261);
            this.gbSetup.TabIndex = 33;
            this.gbSetup.TabStop = false;
            this.gbSetup.Text = "Inline Setup";
            // 
            // pSetup
            // 
            this.pSetup.Anchor = ((AnchorStyles)(((AnchorStyles.Bottom | AnchorStyles.Left)
                        | AnchorStyles.Right)));
            this.pSetup.Location = new Point(6, 19);
            this.pSetup.Name = "pSetup";
            this.pSetup.Size = new Size(952, 236);
            this.pSetup.TabIndex = 0;
            // 
            // tpGroups
            // 
            this.tpGroups.Controls.Add(this.pGroups);
            this.tpGroups.Location = new Point(4, 22);
            this.tpGroups.Name = "tpGroups";
            this.tpGroups.Size = new Size(976, 535);
            this.tpGroups.TabIndex = 2;
            this.tpGroups.Text = "Groups";
            this.tpGroups.UseVisualStyleBackColor = true;
            // 
            // pGroups
            // 
            this.pGroups.Location = new Point(0, 0);
            this.pGroups.Name = "pGroups";
            this.pGroups.Size = new Size(976, 535);
            this.pGroups.TabIndex = 2;
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnOkay.Location = new Point(830, 31);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new Size(75, 23);
            this.btnOkay.TabIndex = 1;
            this.btnOkay.Text = "OK";
            this.ttRoadie.SetToolTip(this.btnOkay, "Save all changes");
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new EventHandler(this.btnOkay_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnCancel.Location = new Point(911, 31);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.ttRoadie.SetToolTip(this.btnCancel, "Cancel all changes");
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            // 
            // previewTimer
            // 
            this.previewTimer.Interval = 200;
            this.previewTimer.Tick += new EventHandler(this.previewTimer_Tick);
            // 
            // dataGridViewDisableButtonColumn1
            // 
            this.dataGridViewDisableButtonColumn1.HeaderText = "Setup";
            this.dataGridViewDisableButtonColumn1.Name = "dataGridViewDisableButtonColumn1";
            this.dataGridViewDisableButtonColumn1.ReadOnly = true;
            this.dataGridViewDisableButtonColumn1.Text = "Setup";
            // 
            // dataGridViewDisableButtonColumn2
            // 
            this.dataGridViewDisableButtonColumn2.HeaderText = "Setup";
            this.dataGridViewDisableButtonColumn2.Name = "dataGridViewDisableButtonColumn2";
            this.dataGridViewDisableButtonColumn2.ReadOnly = true;
            this.dataGridViewDisableButtonColumn2.Text = "Setup";
            // 
            // tcControlArea
            // 
            this.tcControlArea.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.tcControlArea.Controls.Add(this.tpChannelControl);
            this.tcControlArea.Controls.Add(this.tpMultiChannel);
            this.tcControlArea.Controls.Add(this.tpMultiColor);
            this.tcControlArea.HideTabs = true;
            this.tcControlArea.Location = new Point(703, 0);
            this.tcControlArea.Multiline = true;
            this.tcControlArea.Name = "tcControlArea";
            this.tcControlArea.OurMultiline = true;
            this.tcControlArea.SelectedIndex = 0;
            this.tcControlArea.Size = new Size(273, 535);
            this.tcControlArea.TabIndex = 18;
            // 
            // tpChannelControl
            // 
            this.tpChannelControl.BackColor = Color.Transparent;
            this.tpChannelControl.Controls.Add(this.gbExportImport);
            this.tpChannelControl.Controls.Add(this.gbChannels);
            this.tpChannelControl.Controls.Add(this.gbEnable);
            this.tpChannelControl.Controls.Add(this.gbColors);
            this.tpChannelControl.Location = new Point(0, 0);
            this.tpChannelControl.Name = "tpChannelControl";
            this.tpChannelControl.Padding = new Padding(3);
            this.tpChannelControl.Size = new Size(273, 535);
            this.tpChannelControl.TabIndex = 0;
            this.tpChannelControl.Text = "Normal";
            this.tpChannelControl.UseVisualStyleBackColor = true;
            // 
            // gbExportImport
            // 
            this.gbExportImport.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.gbExportImport.Controls.Add(this.btnChExport);
            this.gbExportImport.Controls.Add(this.btnChImport);
            this.gbExportImport.Location = new Point(6, 168);
            this.gbExportImport.Name = "gbExportImport";
            this.gbExportImport.Size = new Size(250, 48);
            this.gbExportImport.TabIndex = 3;
            this.gbExportImport.TabStop = false;
            this.gbExportImport.Text = "Export/Import Channel Data";
            // 
            // btnChExport
            // 
            this.btnChExport.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnChExport.Location = new Point(7, 19);
            this.btnChExport.Name = "btnChExport";
            this.btnChExport.Size = new Size(75, 23);
            this.btnChExport.TabIndex = 0;
            this.btnChExport.Text = "E&xport CSV";
            this.ttRoadie.SetToolTip(this.btnChExport, "Export channels to a CSV file");
            this.btnChExport.UseVisualStyleBackColor = true;
            this.btnChExport.Click += new EventHandler(this.btnExport_Click);
            // 
            // btnChImport
            // 
            this.btnChImport.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnChImport.Location = new Point(88, 19);
            this.btnChImport.Name = "btnChImport";
            this.btnChImport.Size = new Size(75, 23);
            this.btnChImport.TabIndex = 1;
            this.btnChImport.Text = "&Import CSV";
            this.ttRoadie.SetToolTip(this.btnChImport, "Import channels from a CSV file");
            this.btnChImport.UseVisualStyleBackColor = true;
            this.btnChImport.Click += new EventHandler(this.btnImport_Click);
            // 
            // gbChannels
            // 
            this.gbChannels.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.gbChannels.Controls.Add(this.btnChDelete);
            this.gbChannels.Controls.Add(this.btnChAddMulti);
            this.gbChannels.Controls.Add(this.btnChAddOne);
            this.gbChannels.Location = new Point(6, 6);
            this.gbChannels.Name = "gbChannels";
            this.gbChannels.Size = new Size(250, 48);
            this.gbChannels.TabIndex = 0;
            this.gbChannels.TabStop = false;
            this.gbChannels.Text = "Channels";
            // 
            // btnChDelete
            // 
            this.btnChDelete.Location = new Point(169, 19);
            this.btnChDelete.Name = "btnChDelete";
            this.btnChDelete.Size = new Size(75, 23);
            this.btnChDelete.TabIndex = 2;
            this.btnChDelete.Text = "Delete";
            this.ttRoadie.SetToolTip(this.btnChDelete, "Delete selected channels");
            this.btnChDelete.UseVisualStyleBackColor = true;
            this.btnChDelete.Click += new EventHandler(this.btnChDelete_Click);
            // 
            // btnChAddMulti
            // 
            this.btnChAddMulti.Location = new Point(88, 19);
            this.btnChAddMulti.Name = "btnChAddMulti";
            this.btnChAddMulti.Size = new Size(75, 23);
            this.btnChAddMulti.TabIndex = 1;
            this.btnChAddMulti.Text = "Add &Multiple";
            this.ttRoadie.SetToolTip(this.btnChAddMulti, "Add multiple channels using a rule templete");
            this.btnChAddMulti.UseVisualStyleBackColor = true;
            this.btnChAddMulti.Click += new EventHandler(this.btnChAddMulti_Click);
            // 
            // btnChAddOne
            // 
            this.btnChAddOne.Location = new Point(7, 19);
            this.btnChAddOne.Name = "btnChAddOne";
            this.btnChAddOne.Size = new Size(75, 23);
            this.btnChAddOne.TabIndex = 0;
            this.btnChAddOne.Text = "Add O&ne";
            this.ttRoadie.SetToolTip(this.btnChAddOne, "Add a single white channel");
            this.btnChAddOne.UseVisualStyleBackColor = true;
            this.btnChAddOne.Click += new EventHandler(this.btnChAddOne_Click);
            // 
            // gbEnable
            // 
            this.gbEnable.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.gbEnable.Controls.Add(this.btnChEnable);
            this.gbEnable.Controls.Add(this.btnChDisable);
            this.gbEnable.Location = new Point(6, 114);
            this.gbEnable.Name = "gbEnable";
            this.gbEnable.Size = new Size(250, 48);
            this.gbEnable.TabIndex = 2;
            this.gbEnable.TabStop = false;
            this.gbEnable.Text = "Channel Enabling";
            // 
            // btnChEnable
            // 
            this.btnChEnable.Location = new Point(6, 19);
            this.btnChEnable.Name = "btnChEnable";
            this.btnChEnable.Size = new Size(75, 23);
            this.btnChEnable.TabIndex = 0;
            this.btnChEnable.Text = "&Enable";
            this.ttRoadie.SetToolTip(this.btnChEnable, "Enable selected channels");
            this.btnChEnable.UseVisualStyleBackColor = true;
            this.btnChEnable.Click += new EventHandler(this.btnEnableDisable_Click);
            // 
            // btnChDisable
            // 
            this.btnChDisable.Location = new Point(87, 19);
            this.btnChDisable.Name = "btnChDisable";
            this.btnChDisable.Size = new Size(75, 23);
            this.btnChDisable.TabIndex = 1;
            this.btnChDisable.Text = "&Disable";
            this.ttRoadie.SetToolTip(this.btnChDisable, "Disable selected channels");
            this.btnChDisable.UseVisualStyleBackColor = true;
            this.btnChDisable.Click += new EventHandler(this.btnEnableDisable_Click);
            // 
            // gbColors
            // 
            this.gbColors.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.gbColors.Controls.Add(this.btnChColorMulti);
            this.gbColors.Controls.Add(this.btnChColorOne);
            this.gbColors.Location = new Point(6, 60);
            this.gbColors.Name = "gbColors";
            this.gbColors.Size = new Size(250, 48);
            this.gbColors.TabIndex = 1;
            this.gbColors.TabStop = false;
            this.gbColors.Text = "Channel Colors";
            // 
            // btnChColorMulti
            // 
            this.btnChColorMulti.Location = new Point(88, 19);
            this.btnChColorMulti.Name = "btnChColorMulti";
            this.btnChColorMulti.Size = new Size(75, 23);
            this.btnChColorMulti.TabIndex = 1;
            this.btnChColorMulti.Text = "Multi &Color";
            this.ttRoadie.SetToolTip(this.btnChColorMulti, "Color selected channels with a palette");
            this.btnChColorMulti.UseVisualStyleBackColor = true;
            this.btnChColorMulti.Click += new EventHandler(this.btnChColorMulti_Click);
            // 
            // btnChColorOne
            // 
            this.btnChColorOne.Location = new Point(7, 19);
            this.btnChColorOne.Name = "btnChColorOne";
            this.btnChColorOne.Size = new Size(75, 23);
            this.btnChColorOne.TabIndex = 0;
            this.btnChColorOne.Text = "&One Color";
            this.ttRoadie.SetToolTip(this.btnChColorOne, "Color selected channels with a single color");
            this.btnChColorOne.UseVisualStyleBackColor = true;
            this.btnChColorOne.Click += new EventHandler(this.btnChColorOne_Click);
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
            this.tpMultiChannel.Location = new Point(0, 0);
            this.tpMultiChannel.Name = "tpMultiChannel";
            this.tpMultiChannel.Padding = new Padding(3);
            this.tpMultiChannel.Size = new Size(273, 535);
            this.tpMultiChannel.TabIndex = 1;
            this.tpMultiChannel.Text = "MultiChannel";
            this.tpMultiChannel.UseVisualStyleBackColor = true;
            // 
            // btnUpdatePreview
            // 
            this.btnUpdatePreview.Enabled = false;
            this.btnUpdatePreview.Location = new Point(160, 6);
            this.btnUpdatePreview.Name = "btnUpdatePreview";
            this.btnUpdatePreview.Size = new Size(99, 23);
            this.btnUpdatePreview.TabIndex = 10;
            this.btnUpdatePreview.Text = "&Update Preview";
            this.btnUpdatePreview.UseVisualStyleBackColor = true;
            this.btnUpdatePreview.Click += new EventHandler(this.btnUpdatePreview_Click);
            // 
            // btnMultiChannelCancel
            // 
            this.btnMultiChannelCancel.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnMultiChannelCancel.Location = new Point(192, 474);
            this.btnMultiChannelCancel.Name = "btnMultiChannelCancel";
            this.btnMultiChannelCancel.Size = new Size(75, 23);
            this.btnMultiChannelCancel.TabIndex = 4;
            this.btnMultiChannelCancel.Text = "&Cancel";
            this.btnMultiChannelCancel.UseVisualStyleBackColor = true;
            this.btnMultiChannelCancel.Click += new EventHandler(this.btnMultiChannelButton_Click);
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
            this.gbRules.Location = new Point(6, 165);
            this.gbRules.Name = "gbRules";
            this.gbRules.Size = new Size(261, 303);
            this.gbRules.TabIndex = 8;
            this.gbRules.TabStop = false;
            this.gbRules.Text = "Channel Generation Rules";
            // 
            // btnRuleDelete
            // 
            this.btnRuleDelete.Image = Resources.list_remove;
            this.btnRuleDelete.Location = new Point(6, 121);
            this.btnRuleDelete.Name = "btnRuleDelete";
            this.btnRuleDelete.Size = new Size(24, 24);
            this.btnRuleDelete.TabIndex = 5;
            this.ttRoadie.SetToolTip(this.btnRuleDelete, "Remove selected rule");
            this.btnRuleDelete.UseVisualStyleBackColor = true;
            this.btnRuleDelete.Click += new EventHandler(this.btnRuleDelete_Click);
            // 
            // btnRuleAdd
            // 
            this.btnRuleAdd.Image = Resources.list_add;
            this.btnRuleAdd.Location = new Point(231, 17);
            this.btnRuleAdd.Name = "btnRuleAdd";
            this.btnRuleAdd.Size = new Size(24, 24);
            this.btnRuleAdd.TabIndex = 1;
            this.ttRoadie.SetToolTip(this.btnRuleAdd, "Add selected rule");
            this.btnRuleAdd.UseVisualStyleBackColor = true;
            this.btnRuleAdd.Click += new EventHandler(this.btnRuleAdd_Click);
            // 
            // cbRuleRules
            // 
            this.cbRuleRules.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbRuleRules.FormattingEnabled = true;
            this.cbRuleRules.Items.AddRange(new object[] {
            "Numbers",
            "Words"});
            this.cbRuleRules.Location = new Point(6, 19);
            this.cbRuleRules.Name = "cbRuleRules";
            this.cbRuleRules.Size = new Size(219, 21);
            this.cbRuleRules.TabIndex = 0;
            this.cbRuleRules.SelectedIndexChanged += new EventHandler(this.cbRuleRules_SelectedIndexChanged);
            // 
            // btnRuleDown
            // 
            this.btnRuleDown.Image = Resources.arrow_down;
            this.btnRuleDown.Location = new Point(231, 121);
            this.btnRuleDown.Name = "btnRuleDown";
            this.btnRuleDown.Size = new Size(24, 24);
            this.btnRuleDown.TabIndex = 2;
            this.ttRoadie.SetToolTip(this.btnRuleDown, "Move Rule Down");
            this.btnRuleDown.UseVisualStyleBackColor = true;
            this.btnRuleDown.Click += new EventHandler(this.btnRuleDown_Click);
            // 
            // btnRuleUp
            // 
            this.btnRuleUp.Image = Resources.arrow_up;
            this.btnRuleUp.Location = new Point(201, 121);
            this.btnRuleUp.Name = "btnRuleUp";
            this.btnRuleUp.Size = new Size(24, 24);
            this.btnRuleUp.TabIndex = 1;
            this.ttRoadie.SetToolTip(this.btnRuleUp, "Move rule up");
            this.btnRuleUp.UseVisualStyleBackColor = true;
            this.btnRuleUp.Click += new EventHandler(this.btnRuleUp_Click);
            // 
            // lbRules
            // 
            this.lbRules.DisplayMember = "Name";
            this.lbRules.FormattingEnabled = true;
            this.lbRules.Location = new Point(6, 46);
            this.lbRules.Name = "lbRules";
            this.lbRules.ScrollAlwaysVisible = true;
            this.lbRules.Size = new Size(247, 69);
            this.lbRules.TabIndex = 0;
            this.ttRoadie.SetToolTip(this.lbRules, "Channel naming rules");
            this.lbRules.SelectedIndexChanged += new EventHandler(this.lbRules_SelectedIndexChanged);
            this.lbRules.KeyDown += new KeyEventHandler(this.lbRules_KeyDown);
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
            this.panelRuleEditor.Location = new Point(0, 152);
            this.panelRuleEditor.Name = "panelRuleEditor";
            this.panelRuleEditor.Size = new Size(253, 151);
            this.panelRuleEditor.TabIndex = 6;
            // 
            // colorPaletteChannel
            // 
            this.colorPaletteChannel.Location = new Point(147, 95);
            this.colorPaletteChannel.Name = "colorPaletteChannel";
            this.colorPaletteChannel.Size = new Size(103, 50);
            this.colorPaletteChannel.TabIndex = 5;
            // 
            // cbRuleEndNum
            // 
            this.cbRuleEndNum.AutoSize = true;
            this.cbRuleEndNum.Location = new Point(69, 47);
            this.cbRuleEndNum.Name = "cbRuleEndNum";
            this.cbRuleEndNum.Size = new Size(110, 17);
            this.cbRuleEndNum.TabIndex = 1;
            this.cbRuleEndNum.Text = "Use End Number:";
            this.ttRoadie.SetToolTip(this.cbRuleEndNum, "Generation numbering is limited");
            this.cbRuleEndNum.UseVisualStyleBackColor = true;
            this.cbRuleEndNum.CheckedChanged += new EventHandler(this.cbRuleEndNum_CheckedChanged);
            // 
            // nudRuleIncr
            // 
            this.nudRuleIncr.Location = new Point(185, 72);
            this.nudRuleIncr.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRuleIncr.Name = "nudRuleIncr";
            this.nudRuleIncr.Size = new Size(65, 20);
            this.nudRuleIncr.TabIndex = 3;
            this.nudRuleIncr.TextAlign = HorizontalAlignment.Right;
            this.ttRoadie.SetToolTip(this.nudRuleIncr, "Generation numbering increment");
            this.nudRuleIncr.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRuleIncr.ValueChanged += new EventHandler(this.nudRuleIncr_ValueChanged);
            // 
            // nudRuleEnd
            // 
            this.nudRuleEnd.Location = new Point(185, 46);
            this.nudRuleEnd.Maximum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.nudRuleEnd.Name = "nudRuleEnd";
            this.nudRuleEnd.Size = new Size(65, 20);
            this.nudRuleEnd.TabIndex = 2;
            this.nudRuleEnd.TextAlign = HorizontalAlignment.Right;
            this.ttRoadie.SetToolTip(this.nudRuleEnd, "Generation end number");
            this.nudRuleEnd.ValueChanged += new EventHandler(this.nudRuleEnd_ValueChanged);
            // 
            // nudRuleStart
            // 
            this.nudRuleStart.Location = new Point(185, 20);
            this.nudRuleStart.Maximum = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.nudRuleStart.Name = "nudRuleStart";
            this.nudRuleStart.Size = new Size(65, 20);
            this.nudRuleStart.TabIndex = 0;
            this.nudRuleStart.TextAlign = HorizontalAlignment.Right;
            this.ttRoadie.SetToolTip(this.nudRuleStart, "Generation start number");
            this.nudRuleStart.ValueChanged += new EventHandler(this.nudRuleStart_ValueChanged);
            // 
            // lblRuleIncr
            // 
            this.lblRuleIncr.AutoSize = true;
            this.lblRuleIncr.Location = new Point(122, 74);
            this.lblRuleIncr.Name = "lblRuleIncr";
            this.lblRuleIncr.Size = new Size(57, 13);
            this.lblRuleIncr.TabIndex = 9;
            this.lblRuleIncr.Text = "Increment:";
            // 
            // lblRuleStartNum
            // 
            this.lblRuleStartNum.AutoSize = true;
            this.lblRuleStartNum.Location = new Point(93, 22);
            this.lblRuleStartNum.Name = "lblRuleStartNum";
            this.lblRuleStartNum.Size = new Size(86, 13);
            this.lblRuleStartNum.TabIndex = 7;
            this.lblRuleStartNum.Text = "Starting Number:";
            // 
            // tbRuleWords
            // 
            this.tbRuleWords.Location = new Point(6, 21);
            this.tbRuleWords.Multiline = true;
            this.tbRuleWords.Name = "tbRuleWords";
            this.tbRuleWords.ScrollBars = ScrollBars.Vertical;
            this.tbRuleWords.Size = new Size(244, 71);
            this.tbRuleWords.TabIndex = 6;
            this.tbRuleWords.TextChanged += new EventHandler(this.tbRuleWords_TextChanged);
            // 
            // lblRulePrompt
            // 
            this.lblRulePrompt.AutoSize = true;
            this.lblRulePrompt.Location = new Point(6, 4);
            this.lblRulePrompt.Name = "lblRulePrompt";
            this.lblRulePrompt.Size = new Size(109, 13);
            this.lblRulePrompt.TabIndex = 5;
            this.lblRulePrompt.Text = "Words (One Per Line)";
            // 
            // cbRuleColors
            // 
            this.cbRuleColors.AutoSize = true;
            this.cbRuleColors.Checked = true;
            this.cbRuleColors.CheckState = CheckState.Checked;
            this.cbRuleColors.Location = new Point(83, 112);
            this.cbRuleColors.Name = "cbRuleColors";
            this.cbRuleColors.Size = new Size(58, 17);
            this.cbRuleColors.TabIndex = 4;
            this.cbRuleColors.Text = "Colors:";
            this.ttRoadie.SetToolTip(this.cbRuleColors, "Use color palette");
            this.cbRuleColors.UseVisualStyleBackColor = true;
            this.cbRuleColors.CheckedChanged += new EventHandler(this.cbRuleColors_CheckedChanged);
            // 
            // btnMultiChannelOk
            // 
            this.btnMultiChannelOk.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnMultiChannelOk.Location = new Point(6, 474);
            this.btnMultiChannelOk.Name = "btnMultiChannelOk";
            this.btnMultiChannelOk.Size = new Size(141, 23);
            this.btnMultiChannelOk.TabIndex = 3;
            this.btnMultiChannelOk.Text = "&Add Channels to Profile";
            this.btnMultiChannelOk.UseVisualStyleBackColor = true;
            this.btnMultiChannelOk.Click += new EventHandler(this.btnMultiChannelButton_Click);
            // 
            // btnChGenSaveTemplate
            // 
            this.btnChGenSaveTemplate.Image = Resources.saveSm;
            this.btnChGenSaveTemplate.Location = new Point(237, 40);
            this.btnChGenSaveTemplate.Name = "btnChGenSaveTemplate";
            this.btnChGenSaveTemplate.Size = new Size(24, 24);
            this.btnChGenSaveTemplate.TabIndex = 1;
            this.ttRoadie.SetToolTip(this.btnChGenSaveTemplate, "Save current template settings");
            this.btnChGenSaveTemplate.UseVisualStyleBackColor = true;
            this.btnChGenSaveTemplate.Click += new EventHandler(this.btnChGenSaveTemplate_Click);
            // 
            // tbChGenNameFormat
            // 
            this.tbChGenNameFormat.Location = new Point(6, 112);
            this.tbChGenNameFormat.MaxLength = 200;
            this.tbChGenNameFormat.Multiline = true;
            this.tbChGenNameFormat.Name = "tbChGenNameFormat";
            this.tbChGenNameFormat.Size = new Size(253, 47);
            this.tbChGenNameFormat.TabIndex = 2;
            this.ttRoadie.SetToolTip(this.tbChGenNameFormat, "How to format the generated channel names");
            this.tbChGenNameFormat.TextChanged += new EventHandler(this.PreviewChannelEvent);
            this.tbChGenNameFormat.KeyDown += new KeyEventHandler(this.tbChGenNameFormat_KeyDown);
            // 
            // nudChGenChannels
            // 
            this.nudChGenChannels.Location = new Point(180, 70);
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
            this.nudChGenChannels.Size = new Size(79, 20);
            this.nudChGenChannels.TabIndex = 7;
            this.nudChGenChannels.TextAlign = HorizontalAlignment.Right;
            this.ttRoadie.SetToolTip(this.nudChGenChannels, "Set maximum # of channels to generate");
            this.nudChGenChannels.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudChGenChannels.ValueChanged += new EventHandler(this.PreviewChannelEvent);
            // 
            // lblChGenNameFormat
            // 
            this.lblChGenNameFormat.AutoSize = true;
            this.lblChGenNameFormat.Location = new Point(6, 96);
            this.lblChGenNameFormat.Name = "lblChGenNameFormat";
            this.lblChGenNameFormat.Size = new Size(115, 13);
            this.lblChGenNameFormat.TabIndex = 4;
            this.lblChGenNameFormat.Text = "Channel Name Format:";
            // 
            // cbPreview
            // 
            this.cbPreview.AutoSize = true;
            this.cbPreview.Checked = true;
            this.cbPreview.CheckState = CheckState.Checked;
            this.cbPreview.Location = new Point(31, 10);
            this.cbPreview.Name = "cbPreview";
            this.cbPreview.Size = new Size(123, 17);
            this.cbPreview.TabIndex = 9;
            this.cbPreview.Text = "Enable Live Preview";
            this.cbPreview.UseVisualStyleBackColor = true;
            this.cbPreview.CheckedChanged += new EventHandler(this.PreviewChannelEvent);
            // 
            // lblChGenCount
            // 
            this.lblChGenCount.AutoSize = true;
            this.lblChGenCount.Location = new Point(96, 72);
            this.lblChGenCount.Name = "lblChGenCount";
            this.lblChGenCount.Size = new Size(76, 13);
            this.lblChGenCount.TabIndex = 6;
            this.lblChGenCount.Text = "# of Channels:";
            // 
            // lblChGenTemplate
            // 
            this.lblChGenTemplate.AutoSize = true;
            this.lblChGenTemplate.Location = new Point(6, 26);
            this.lblChGenTemplate.Name = "lblChGenTemplate";
            this.lblChGenTemplate.Size = new Size(54, 13);
            this.lblChGenTemplate.TabIndex = 2;
            this.lblChGenTemplate.Text = "Template:";
            // 
            // cbChGenTemplate
            // 
            this.cbChGenTemplate.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbChGenTemplate.FormattingEnabled = true;
            this.cbChGenTemplate.Items.AddRange(new object[] {
            "RGB Channels",
            "Numbered Elements"});
            this.cbChGenTemplate.Location = new Point(9, 42);
            this.cbChGenTemplate.Name = "cbChGenTemplate";
            this.cbChGenTemplate.Size = new Size(222, 21);
            this.cbChGenTemplate.TabIndex = 0;
            this.cbChGenTemplate.SelectedIndexChanged += new EventHandler(this.cbChGenTemplate_SelectedIndexChanged);
            // 
            // tpMultiColor
            // 
            this.tpMultiColor.Controls.Add(this.btnMultiColorOk);
            this.tpMultiColor.Controls.Add(this.btnMultiColorCancel);
            this.tpMultiColor.Controls.Add(this.colorPaletteColor);
            this.tpMultiColor.Location = new Point(0, 0);
            this.tpMultiColor.Name = "tpMultiColor";
            this.tpMultiColor.Size = new Size(273, 535);
            this.tpMultiColor.TabIndex = 2;
            this.tpMultiColor.Text = "MutliColor";
            this.tpMultiColor.UseVisualStyleBackColor = true;
            // 
            // btnMultiColorOk
            // 
            this.btnMultiColorOk.Location = new Point(6, 63);
            this.btnMultiColorOk.Name = "btnMultiColorOk";
            this.btnMultiColorOk.Size = new Size(75, 23);
            this.btnMultiColorOk.TabIndex = 2;
            this.btnMultiColorOk.Text = "OK";
            this.btnMultiColorOk.UseVisualStyleBackColor = true;
            this.btnMultiColorOk.Click += new EventHandler(this.btnMultiColor_Click);
            // 
            // btnMultiColorCancel
            // 
            this.btnMultiColorCancel.Location = new Point(192, 63);
            this.btnMultiColorCancel.Name = "btnMultiColorCancel";
            this.btnMultiColorCancel.Size = new Size(75, 23);
            this.btnMultiColorCancel.TabIndex = 1;
            this.btnMultiColorCancel.Text = "Cancel";
            this.btnMultiColorCancel.UseVisualStyleBackColor = true;
            this.btnMultiColorCancel.Click += new EventHandler(this.btnMultiColor_Click);
            // 
            // colorPaletteColor
            // 
            this.colorPaletteColor.Location = new Point(84, 6);
            this.colorPaletteColor.Name = "colorPaletteColor";
            this.colorPaletteColor.Size = new Size(104, 50);
            this.colorPaletteColor.TabIndex = 0;
            // 
            // dataGridViewDisableButtonColumn3
            // 
            this.dataGridViewDisableButtonColumn3.HeaderText = "Setup";
            this.dataGridViewDisableButtonColumn3.Name = "dataGridViewDisableButtonColumn3";
            this.dataGridViewDisableButtonColumn3.ReadOnly = true;
            this.dataGridViewDisableButtonColumn3.Text = "Setup";
            // 
            // colPlugInSetup
            // 
            this.colPlugInSetup.HeaderText = "Setup";
            this.colPlugInSetup.Name = "colPlugInSetup";
            this.colPlugInSetup.ReadOnly = true;
            this.colPlugInSetup.Text = "Setup";
            // 
            // VixenPlusRoadie
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1008, 643);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.tcProfile);
            this.Controls.Add(this.gbProfiles);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "VixenPlusRoadie";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Vixen+ {Beta} - ";
            this.FormClosing += new FormClosingEventHandler(this.VixenPlusRoadie_FormClosing);
            this.KeyDown += new KeyEventHandler(this.FrmProfileManager_KeyDown);
            this.Resize += new EventHandler(this.frmProfileManager_Resize);
            this.gbProfiles.ResumeLayout(false);
            this.tcProfile.ResumeLayout(false);
            this.tpChannels.ResumeLayout(false);
            ((ISupportInitialize)(this.dgvChannels)).EndInit();
            this.tpPlugins.ResumeLayout(false);
            ((ISupportInitialize)(this.dgvPlugIns)).EndInit();
            this.gbSetup.ResumeLayout(false);
            this.tpGroups.ResumeLayout(false);
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
            ((ISupportInitialize)(this.nudRuleIncr)).EndInit();
            ((ISupportInitialize)(this.nudRuleEnd)).EndInit();
            ((ISupportInitialize)(this.nudRuleStart)).EndInit();
            ((ISupportInitialize)(this.nudChGenChannels)).EndInit();
            this.tpMultiColor.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComboBox cbProfiles;
        private GroupBox gbProfiles;
        private Button btnProfileDelete;
        private Button btnProfileRename;
        private Button btnProfileCopy;
        private Button btnProfileAdd;
        private TabControl tcProfile;
        private TabPage tpChannels;
        private TabPage tpPlugins;
        private DataGridView dgvChannels;
        private Button btnOkay;
        private Button btnCancel;
        private Button btnChImport;
        private Button btnChExport;
        private TabPage tpGroups;
        private GroupBox gbExportImport;
        private GroupBox gbEnable;
        private Button btnChEnable;
        private Button btnChDisable;
        private GroupBox gbColors;
        private Button btnChColorMulti;
        private Button btnChColorOne;
        private GroupBox gbChannels;
        private Button btnChAddMulti;
        private Button btnChAddOne;
        private Button btnChDelete;
        private Button btnMultiChannelCancel;
        private Button btnMultiChannelOk;
        private GroupBox gbRules;
        private Button btnRuleAdd;
        private ComboBox cbRuleRules;
        private Button btnRuleDown;
        private Button btnRuleUp;
        private ListBox lbRules;
        private NumericUpDown nudChGenChannels;
        private Label lblChGenCount;
        private TextBox tbChGenNameFormat;
        private Label lblChGenNameFormat;
        private ComboBox cbChGenTemplate;
        private Label lblChGenTemplate;
        private Panel panelRuleEditor;
        private Button btnRuleDelete;
        private ToolTip ttRoadie;
        private TextBox tbRuleWords;
        private Label lblRulePrompt;
        private CheckBox cbRuleColors;
        private Button btnChGenSaveTemplate;
        private CheckBox cbRuleEndNum;
        private NumericUpDown nudRuleIncr;
        private NumericUpDown nudRuleEnd;
        private NumericUpDown nudRuleStart;
        private Label lblRuleIncr;
        private Label lblRuleStartNum;
        private DataGridViewCheckBoxColumn ChannelEnabled;
        private DataGridViewTextBoxColumn ChannelNum;
        private DataGridViewTextBoxColumn ChannelName;
        private DataGridViewTextBoxColumn OutputChannel;
        private DataGridViewTextBoxColumn ChannelColor;
        private VixenPlusCommon.TabControl tcControlArea;
        private TabPage tpMultiChannel;
        private TabPage tpChannelControl;
        private TabPage tpMultiColor;
        private ColorPalette colorPaletteChannel;
        private ColorPalette colorPaletteColor;
        private Button btnMultiColorOk;
        private Button btnMultiColorCancel;
        private CheckBox cbPreview;
        private Timer previewTimer;
        private Button btnUpdatePreview;
        private Button btnProfileSave;
        private Button btnRemovePlugIn;
        private Button btnAddPlugIn;
        private GroupBox gbSetup;
        private Panel pSetup;
        private DataGridView dgvPlugIns;
        private ComboBox cbAvailablePlugIns;
        private DataGridViewTextBoxColumn colPlugInName;
        private DataGridViewCheckBoxColumn colPlugInEnabled;
        private DataGridViewTextBoxColumn colPlugInStartChannel;
        private DataGridViewTextBoxColumn colPlugInEndChannel;
        private DataGridViewTextBoxColumn colPlugInConfiguration;
        private DataGridViewDisableButtonColumn colPlugInSetup;
        private DataGridViewDisableButtonColumn dataGridViewDisableButtonColumn1;
        private Panel pGroups;
        private DataGridViewDisableButtonColumn dataGridViewDisableButtonColumn2;
        private DataGridViewDisableButtonColumn dataGridViewDisableButtonColumn3;
    }
}