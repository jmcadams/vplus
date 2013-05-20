using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace VixenPlus {
    internal partial class InputPluginDialog {
        private IContainer components = null;

        #region Windows Form Designer generated code

        private Button buttonAddMappingSet;
        private Button buttonCancel;
        private Button buttonClearInputChannels;
        private Button buttonMoveDown;
        private Button buttonMoveUp;
        private Button buttonOK;
        private Button buttonRemoveMappingSet;
        private Button buttonRenameMappingSet;
        private CheckBox checkBoxEnabled;
        private CheckBox checkBoxLiveUpdate;
        private CheckBox checkBoxRecord;
        private ColumnHeader columnHeader1;
        private ComboBox comboBoxMappingSet;
        private ComboBox comboBoxSingleIteratorInput;
        private GroupBox groupBox2;
        private GroupBox groupBoxChannels;
        private GroupBox groupBoxIOMapping;
        private ImageList imageList;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private ListBox listBoxChannels;
        private ListBox listBoxInputs;
        private ListBox listBoxIteratorInputs;
        private ListBox listBoxMappedChannels;
        private ListBox listBoxMappingSets;
        private ListView listViewMappingSets;
        private Panel panel1;
        private RadioButton radioButtonMultipleIterators;
        private RadioButton radioButtonNoIterator;
        private RadioButton radioButtonSingleIterator;
        private System.Windows.Forms.TabControl tabControlIterators;
        private System.Windows.Forms.TabControl tabControlMappingSets;
        private System.Windows.Forms.TabControl tabControlPlugin;
        private TabPage tabPageInputOutputMapping;
        private TabPage tabPageMappingIteration;
        private TabPage tabPageMappingSets;
        private TabPage tabPageMultipleIterators;
        private TabPage tabPageSetDefinitions;
        private TabPage tabPageSingleIterator;


        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.groupBoxIOMapping = new System.Windows.Forms.GroupBox();
            this.listBoxInputs = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listBoxMappedChannels = new System.Windows.Forms.ListBox();
            this.groupBoxChannels = new System.Windows.Forms.GroupBox();
            this.checkBoxEnabled = new System.Windows.Forms.CheckBox();
            this.buttonClearInputChannels = new System.Windows.Forms.Button();
            this.listBoxChannels = new System.Windows.Forms.ListBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.checkBoxLiveUpdate = new System.Windows.Forms.CheckBox();
            this.checkBoxRecord = new System.Windows.Forms.CheckBox();
            this.tabControlPlugin = new System.Windows.Forms.TabControl();
            this.tabPageMappingSets = new System.Windows.Forms.TabPage();
            this.tabControlMappingSets = new System.Windows.Forms.TabControl();
            this.tabPageSetDefinitions = new System.Windows.Forms.TabPage();
            this.buttonRenameMappingSet = new System.Windows.Forms.Button();
            this.buttonMoveDown = new System.Windows.Forms.Button();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.buttonMoveUp = new System.Windows.Forms.Button();
            this.buttonRemoveMappingSet = new System.Windows.Forms.Button();
            this.buttonAddMappingSet = new System.Windows.Forms.Button();
            this.listViewMappingSets = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader) (new System.Windows.Forms.ColumnHeader()));
            this.tabPageInputOutputMapping = new System.Windows.Forms.TabPage();
            this.comboBoxMappingSet = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPageMappingIteration = new System.Windows.Forms.TabPage();
            this.radioButtonNoIterator = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControlIterators = new System.Windows.Forms.TabControl();
            this.tabPageSingleIterator = new System.Windows.Forms.TabPage();
            this.comboBoxSingleIteratorInput = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPageMultipleIterators = new System.Windows.Forms.TabPage();
            this.listBoxIteratorInputs = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.listBoxMappingSets = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButtonMultipleIterators = new System.Windows.Forms.RadioButton();
            this.radioButtonSingleIterator = new System.Windows.Forms.RadioButton();
            this.groupBoxIOMapping.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBoxChannels.SuspendLayout();
            this.tabControlPlugin.SuspendLayout();
            this.tabPageMappingSets.SuspendLayout();
            this.tabControlMappingSets.SuspendLayout();
            this.tabPageSetDefinitions.SuspendLayout();
            this.tabPageInputOutputMapping.SuspendLayout();
            this.tabPageMappingIteration.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControlIterators.SuspendLayout();
            this.tabPageSingleIterator.SuspendLayout();
            this.tabPageMultipleIterators.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxIOMapping
            // 
            this.groupBoxIOMapping.Controls.Add(this.listBoxInputs);
            this.groupBoxIOMapping.Controls.Add(this.groupBox2);
            this.groupBoxIOMapping.Controls.Add(this.groupBoxChannels);
            this.groupBoxIOMapping.Enabled = false;
            this.groupBoxIOMapping.Location = new System.Drawing.Point(13, 58);
            this.groupBoxIOMapping.Name = "groupBoxIOMapping";
            this.groupBoxIOMapping.Size = new System.Drawing.Size(354, 334);
            this.groupBoxIOMapping.TabIndex = 4;
            this.groupBoxIOMapping.TabStop = false;
            this.groupBoxIOMapping.Text = "Input-Output Mapping";
            // 
            // listBoxInputs
            // 
            this.listBoxInputs.FormattingEnabled = true;
            this.listBoxInputs.Location = new System.Drawing.Point(15, 26);
            this.listBoxInputs.Name = "listBoxInputs";
            this.listBoxInputs.Size = new System.Drawing.Size(152, 108);
            this.listBoxInputs.TabIndex = 6;
            this.listBoxInputs.SelectedIndexChanged += new System.EventHandler(this.listBoxInputs_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.listBoxMappedChannels);
            this.groupBox2.Location = new System.Drawing.Point(184, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(161, 121);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Input is mapped to:";
            // 
            // listBoxMappedChannels
            // 
            this.listBoxMappedChannels.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) |
                   System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxMappedChannels.BackColor = System.Drawing.SystemColors.Control;
            this.listBoxMappedChannels.FormattingEnabled = true;
            this.listBoxMappedChannels.Location = new System.Drawing.Point(6, 19);
            this.listBoxMappedChannels.Name = "listBoxMappedChannels";
            this.listBoxMappedChannels.Size = new System.Drawing.Size(149, 95);
            this.listBoxMappedChannels.TabIndex = 0;
            // 
            // groupBoxChannels
            // 
            this.groupBoxChannels.Controls.Add(this.checkBoxEnabled);
            this.groupBoxChannels.Controls.Add(this.buttonClearInputChannels);
            this.groupBoxChannels.Controls.Add(this.listBoxChannels);
            this.groupBoxChannels.Location = new System.Drawing.Point(11, 146);
            this.groupBoxChannels.Name = "groupBoxChannels";
            this.groupBoxChannels.Size = new System.Drawing.Size(334, 173);
            this.groupBoxChannels.TabIndex = 4;
            this.groupBoxChannels.TabStop = false;
            // 
            // checkBoxEnabled
            // 
            this.checkBoxEnabled.AutoSize = true;
            this.checkBoxEnabled.Location = new System.Drawing.Point(13, 18);
            this.checkBoxEnabled.Name = "checkBoxEnabled";
            this.checkBoxEnabled.Size = new System.Drawing.Size(102, 17);
            this.checkBoxEnabled.TabIndex = 3;
            this.checkBoxEnabled.Text = "Input is disabled";
            this.checkBoxEnabled.UseVisualStyleBackColor = true;
            this.checkBoxEnabled.CheckedChanged += new System.EventHandler(this.checkBoxEnabled_CheckedChanged);
            this.checkBoxEnabled.Click += new System.EventHandler(this.checkBoxEnabled_Click);
            // 
            // buttonClearInputChannels
            // 
            this.buttonClearInputChannels.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClearInputChannels.Location = new System.Drawing.Point(250, 142);
            this.buttonClearInputChannels.Name = "buttonClearInputChannels";
            this.buttonClearInputChannels.Size = new System.Drawing.Size(75, 23);
            this.buttonClearInputChannels.TabIndex = 5;
            this.buttonClearInputChannels.Text = "Clear";
            this.buttonClearInputChannels.UseVisualStyleBackColor = true;
            this.buttonClearInputChannels.Click += new System.EventHandler(this.buttonClearInputChannels_Click);
            // 
            // listBoxChannels
            // 
            this.listBoxChannels.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) |
                   System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxChannels.Enabled = false;
            this.listBoxChannels.FormattingEnabled = true;
            this.listBoxChannels.Location = new System.Drawing.Point(13, 41);
            this.listBoxChannels.Name = "listBoxChannels";
            this.listBoxChannels.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxChannels.Size = new System.Drawing.Size(315, 95);
            this.listBoxChannels.TabIndex = 4;
            this.listBoxChannels.SelectedIndexChanged += new System.EventHandler(this.listBoxChannels_SelectedIndexChanged);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(273, 533);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(354, 533);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // checkBoxLiveUpdate
            // 
            this.checkBoxLiveUpdate.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxLiveUpdate.AutoSize = true;
            this.checkBoxLiveUpdate.Location = new System.Drawing.Point(12, 510);
            this.checkBoxLiveUpdate.Name = "checkBoxLiveUpdate";
            this.checkBoxLiveUpdate.Size = new System.Drawing.Size(147, 17);
            this.checkBoxLiveUpdate.TabIndex = 5;
            this.checkBoxLiveUpdate.Text = "Live-update the hardware";
            this.checkBoxLiveUpdate.UseVisualStyleBackColor = true;
            // 
            // checkBoxRecord
            // 
            this.checkBoxRecord.Anchor =
                ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxRecord.AutoSize = true;
            this.checkBoxRecord.Location = new System.Drawing.Point(12, 533);
            this.checkBoxRecord.Name = "checkBoxRecord";
            this.checkBoxRecord.Size = new System.Drawing.Size(141, 17);
            this.checkBoxRecord.TabIndex = 7;
            this.checkBoxRecord.Text = "Record to the sequence";
            this.checkBoxRecord.UseVisualStyleBackColor = true;
            // 
            // tabControlPlugin
            // 
            this.tabControlPlugin.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) |
                   System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlPlugin.Controls.Add(this.tabPageMappingSets);
            this.tabControlPlugin.Controls.Add(this.tabPageMappingIteration);
            this.tabControlPlugin.Location = new System.Drawing.Point(12, 12);
            this.tabControlPlugin.Name = "tabControlPlugin";
            this.tabControlPlugin.SelectedIndex = 0;
            this.tabControlPlugin.Size = new System.Drawing.Size(417, 492);
            this.tabControlPlugin.TabIndex = 8;
            this.tabControlPlugin.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControlPlugin_Selecting);
            // 
            // tabPageMappingSets
            // 
            this.tabPageMappingSets.Controls.Add(this.tabControlMappingSets);
            this.tabPageMappingSets.Location = new System.Drawing.Point(4, 22);
            this.tabPageMappingSets.Name = "tabPageMappingSets";
            this.tabPageMappingSets.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMappingSets.Size = new System.Drawing.Size(409, 466);
            this.tabPageMappingSets.TabIndex = 0;
            this.tabPageMappingSets.Text = "Mapping Sets";
            this.tabPageMappingSets.UseVisualStyleBackColor = true;
            // 
            // tabControlMappingSets
            // 
            this.tabControlMappingSets.Anchor =
                ((System.Windows.Forms.AnchorStyles)
                 ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) |
                   System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlMappingSets.Controls.Add(this.tabPageSetDefinitions);
            this.tabControlMappingSets.Controls.Add(this.tabPageInputOutputMapping);
            this.tabControlMappingSets.Location = new System.Drawing.Point(10, 12);
            this.tabControlMappingSets.Name = "tabControlMappingSets";
            this.tabControlMappingSets.SelectedIndex = 0;
            this.tabControlMappingSets.Size = new System.Drawing.Size(381, 448);
            this.tabControlMappingSets.TabIndex = 5;
            this.tabControlMappingSets.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControlMappingSets_Selecting);
            // 
            // tabPageSetDefinitions
            // 
            this.tabPageSetDefinitions.Controls.Add(this.buttonRenameMappingSet);
            this.tabPageSetDefinitions.Controls.Add(this.buttonMoveDown);
            this.tabPageSetDefinitions.Controls.Add(this.buttonMoveUp);
            this.tabPageSetDefinitions.Controls.Add(this.buttonRemoveMappingSet);
            this.tabPageSetDefinitions.Controls.Add(this.buttonAddMappingSet);
            this.tabPageSetDefinitions.Controls.Add(this.listViewMappingSets);
            this.tabPageSetDefinitions.Location = new System.Drawing.Point(4, 22);
            this.tabPageSetDefinitions.Name = "tabPageSetDefinitions";
            this.tabPageSetDefinitions.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSetDefinitions.Size = new System.Drawing.Size(373, 422);
            this.tabPageSetDefinitions.TabIndex = 0;
            this.tabPageSetDefinitions.Text = "Set Definitions";
            this.tabPageSetDefinitions.UseVisualStyleBackColor = true;
            // 
            // buttonRenameMappingSet
            // 
            this.buttonRenameMappingSet.Enabled = false;
            this.buttonRenameMappingSet.Location = new System.Drawing.Point(185, 193);
            this.buttonRenameMappingSet.Name = "buttonRenameMappingSet";
            this.buttonRenameMappingSet.Size = new System.Drawing.Size(75, 23);
            this.buttonRenameMappingSet.TabIndex = 5;
            this.buttonRenameMappingSet.Text = "Rename";
            this.buttonRenameMappingSet.UseVisualStyleBackColor = true;
            this.buttonRenameMappingSet.Visible = false;
            this.buttonRenameMappingSet.Click += new System.EventHandler(this.buttonRenameMappingSet_Click);
            // 
            // buttonMoveDown
            // 
            this.buttonMoveDown.Enabled = false;
            this.buttonMoveDown.ImageList = this.imageList;
            this.buttonMoveDown.Location = new System.Drawing.Point(341, 59);
            this.buttonMoveDown.Name = "buttonMoveDown";
            this.buttonMoveDown.Size = new System.Drawing.Size(26, 23);
            this.buttonMoveDown.TabIndex = 4;
            this.buttonMoveDown.UseVisualStyleBackColor = true;
            this.buttonMoveDown.Click += new System.EventHandler(this.buttonMoveDown_Click);
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.White;
            // 
            // buttonMoveUp
            // 
            this.buttonMoveUp.Enabled = false;
            this.buttonMoveUp.ImageList = this.imageList;
            this.buttonMoveUp.Location = new System.Drawing.Point(341, 30);
            this.buttonMoveUp.Name = "buttonMoveUp";
            this.buttonMoveUp.Size = new System.Drawing.Size(26, 23);
            this.buttonMoveUp.TabIndex = 3;
            this.buttonMoveUp.UseVisualStyleBackColor = true;
            this.buttonMoveUp.Click += new System.EventHandler(this.buttonMoveUp_Click);
            // 
            // buttonRemoveMappingSet
            // 
            this.buttonRemoveMappingSet.Enabled = false;
            this.buttonRemoveMappingSet.Location = new System.Drawing.Point(104, 193);
            this.buttonRemoveMappingSet.Name = "buttonRemoveMappingSet";
            this.buttonRemoveMappingSet.Size = new System.Drawing.Size(75, 23);
            this.buttonRemoveMappingSet.TabIndex = 2;
            this.buttonRemoveMappingSet.Text = "Remove";
            this.buttonRemoveMappingSet.UseVisualStyleBackColor = true;
            this.buttonRemoveMappingSet.Click += new System.EventHandler(this.buttonRemoveMappingSet_Click);
            // 
            // buttonAddMappingSet
            // 
            this.buttonAddMappingSet.Location = new System.Drawing.Point(23, 193);
            this.buttonAddMappingSet.Name = "buttonAddMappingSet";
            this.buttonAddMappingSet.Size = new System.Drawing.Size(75, 23);
            this.buttonAddMappingSet.TabIndex = 1;
            this.buttonAddMappingSet.Text = "Add New";
            this.buttonAddMappingSet.UseVisualStyleBackColor = true;
            this.buttonAddMappingSet.Click += new System.EventHandler(this.buttonAddMappingSet_Click);
            // 
            // listViewMappingSets
            // 
            this.listViewMappingSets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {this.columnHeader1});
            this.listViewMappingSets.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewMappingSets.HideSelection = false;
            this.listViewMappingSets.LabelEdit = true;
            this.listViewMappingSets.Location = new System.Drawing.Point(23, 30);
            this.listViewMappingSets.Name = "listViewMappingSets";
            this.listViewMappingSets.Size = new System.Drawing.Size(312, 157);
            this.listViewMappingSets.TabIndex = 0;
            this.listViewMappingSets.UseCompatibleStateImageBehavior = false;
            this.listViewMappingSets.View = System.Windows.Forms.View.Details;
            this.listViewMappingSets.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listViewMappingSets_AfterLabelEdit);
            this.listViewMappingSets.SelectedIndexChanged += new System.EventHandler(this.listViewMappingSets_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 283;
            // 
            // tabPageInputOutputMapping
            // 
            this.tabPageInputOutputMapping.Controls.Add(this.comboBoxMappingSet);
            this.tabPageInputOutputMapping.Controls.Add(this.label1);
            this.tabPageInputOutputMapping.Controls.Add(this.groupBoxIOMapping);
            this.tabPageInputOutputMapping.Location = new System.Drawing.Point(4, 22);
            this.tabPageInputOutputMapping.Name = "tabPageInputOutputMapping";
            this.tabPageInputOutputMapping.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageInputOutputMapping.Size = new System.Drawing.Size(373, 422);
            this.tabPageInputOutputMapping.TabIndex = 1;
            this.tabPageInputOutputMapping.Text = "Set Input/Output Mappings";
            this.tabPageInputOutputMapping.UseVisualStyleBackColor = true;
            // 
            // comboBoxMappingSet
            // 
            this.comboBoxMappingSet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMappingSet.FormattingEnabled = true;
            this.comboBoxMappingSet.Location = new System.Drawing.Point(107, 18);
            this.comboBoxMappingSet.Name = "comboBoxMappingSet";
            this.comboBoxMappingSet.Size = new System.Drawing.Size(242, 21);
            this.comboBoxMappingSet.TabIndex = 6;
            this.comboBoxMappingSet.SelectedIndexChanged += new System.EventHandler(this.comboBoxMappingSet_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Mapping set:";
            // 
            // tabPageMappingIteration
            // 
            this.tabPageMappingIteration.Controls.Add(this.radioButtonNoIterator);
            this.tabPageMappingIteration.Controls.Add(this.panel1);
            this.tabPageMappingIteration.Controls.Add(this.radioButtonMultipleIterators);
            this.tabPageMappingIteration.Controls.Add(this.radioButtonSingleIterator);
            this.tabPageMappingIteration.Location = new System.Drawing.Point(4, 22);
            this.tabPageMappingIteration.Name = "tabPageMappingIteration";
            this.tabPageMappingIteration.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMappingIteration.Size = new System.Drawing.Size(409, 466);
            this.tabPageMappingIteration.TabIndex = 1;
            this.tabPageMappingIteration.Text = "Mapping Set Iteration";
            this.tabPageMappingIteration.UseVisualStyleBackColor = true;
            // 
            // radioButtonNoIterator
            // 
            this.radioButtonNoIterator.AutoSize = true;
            this.radioButtonNoIterator.Checked = true;
            this.radioButtonNoIterator.Location = new System.Drawing.Point(26, 24);
            this.radioButtonNoIterator.Name = "radioButtonNoIterator";
            this.radioButtonNoIterator.Size = new System.Drawing.Size(170, 17);
            this.radioButtonNoIterator.TabIndex = 0;
            this.radioButtonNoIterator.TabStop = true;
            this.radioButtonNoIterator.Text = "No iterator (single mapping set)";
            this.radioButtonNoIterator.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.tabControlIterators);
            this.panel1.Location = new System.Drawing.Point(25, 106);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(356, 242);
            this.panel1.TabIndex = 2;
            // 
            // tabControlIterators
            // 
            this.tabControlIterators.Controls.Add(this.tabPageSingleIterator);
            this.tabControlIterators.Controls.Add(this.tabPageMultipleIterators);
            this.tabControlIterators.Location = new System.Drawing.Point(0, -22);
            this.tabControlIterators.Name = "tabControlIterators";
            this.tabControlIterators.SelectedIndex = 0;
            this.tabControlIterators.Size = new System.Drawing.Size(356, 264);
            this.tabControlIterators.TabIndex = 0;
            // 
            // tabPageSingleIterator
            // 
            this.tabPageSingleIterator.Controls.Add(this.comboBoxSingleIteratorInput);
            this.tabPageSingleIterator.Controls.Add(this.label2);
            this.tabPageSingleIterator.Location = new System.Drawing.Point(4, 22);
            this.tabPageSingleIterator.Name = "tabPageSingleIterator";
            this.tabPageSingleIterator.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSingleIterator.Size = new System.Drawing.Size(348, 238);
            this.tabPageSingleIterator.TabIndex = 0;
            this.tabPageSingleIterator.Text = "tabPageSingleIterator";
            this.tabPageSingleIterator.UseVisualStyleBackColor = true;
            // 
            // comboBoxSingleIteratorInput
            // 
            this.comboBoxSingleIteratorInput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSingleIteratorInput.FormattingEnabled = true;
            this.comboBoxSingleIteratorInput.Location = new System.Drawing.Point(58, 16);
            this.comboBoxSingleIteratorInput.Name = "comboBoxSingleIteratorInput";
            this.comboBoxSingleIteratorInput.Size = new System.Drawing.Size(259, 21);
            this.comboBoxSingleIteratorInput.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Input:";
            // 
            // tabPageMultipleIterators
            // 
            this.tabPageMultipleIterators.Controls.Add(this.listBoxIteratorInputs);
            this.tabPageMultipleIterators.Controls.Add(this.label4);
            this.tabPageMultipleIterators.Controls.Add(this.listBoxMappingSets);
            this.tabPageMultipleIterators.Controls.Add(this.label3);
            this.tabPageMultipleIterators.Location = new System.Drawing.Point(4, 22);
            this.tabPageMultipleIterators.Name = "tabPageMultipleIterators";
            this.tabPageMultipleIterators.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMultipleIterators.Size = new System.Drawing.Size(348, 238);
            this.tabPageMultipleIterators.TabIndex = 1;
            this.tabPageMultipleIterators.Text = "tabPageMultipleIterators";
            this.tabPageMultipleIterators.UseVisualStyleBackColor = true;
            // 
            // listBoxIteratorInputs
            // 
            this.listBoxIteratorInputs.FormattingEnabled = true;
            this.listBoxIteratorInputs.Location = new System.Drawing.Point(31, 30);
            this.listBoxIteratorInputs.Name = "listBoxIteratorInputs";
            this.listBoxIteratorInputs.Size = new System.Drawing.Size(120, 173);
            this.listBoxIteratorInputs.TabIndex = 1;
            this.listBoxIteratorInputs.SelectedIndexChanged += new System.EventHandler(this.listBoxIteratorInputs_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Input:";
            // 
            // listBoxMappingSets
            // 
            this.listBoxMappingSets.FormattingEnabled = true;
            this.listBoxMappingSets.Items.AddRange(new object[] {"(none)"});
            this.listBoxMappingSets.Location = new System.Drawing.Point(193, 30);
            this.listBoxMappingSets.Name = "listBoxMappingSets";
            this.listBoxMappingSets.Size = new System.Drawing.Size(120, 173);
            this.listBoxMappingSets.TabIndex = 3;
            this.listBoxMappingSets.SelectedIndexChanged += new System.EventHandler(this.listBoxMappingSets_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(190, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Mapping set:";
            // 
            // radioButtonMultipleIterators
            // 
            this.radioButtonMultipleIterators.AutoSize = true;
            this.radioButtonMultipleIterators.Location = new System.Drawing.Point(26, 70);
            this.radioButtonMultipleIterators.Name = "radioButtonMultipleIterators";
            this.radioButtonMultipleIterators.Size = new System.Drawing.Size(220, 17);
            this.radioButtonMultipleIterators.TabIndex = 2;
            this.radioButtonMultipleIterators.Text = "Assign each mapping set to a single input";
            this.radioButtonMultipleIterators.UseVisualStyleBackColor = true;
            this.radioButtonMultipleIterators.CheckedChanged += new System.EventHandler(this.radioButtonMultipleIterators_CheckedChanged);
            // 
            // radioButtonSingleIterator
            // 
            this.radioButtonSingleIterator.AutoSize = true;
            this.radioButtonSingleIterator.Location = new System.Drawing.Point(26, 47);
            this.radioButtonSingleIterator.Name = "radioButtonSingleIterator";
            this.radioButtonSingleIterator.Size = new System.Drawing.Size(302, 17);
            this.radioButtonSingleIterator.TabIndex = 1;
            this.radioButtonSingleIterator.Text = "Use a single input to iterate through the list of mapping sets";
            this.radioButtonSingleIterator.UseVisualStyleBackColor = true;
            this.radioButtonSingleIterator.CheckedChanged += new System.EventHandler(this.radioButtonSingleIterator_CheckedChanged);
            // 
            // InputPluginDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(441, 564);
            this.Controls.Add(this.tabControlPlugin);
            this.Controls.Add(this.checkBoxRecord);
            this.Controls.Add(this.checkBoxLiveUpdate);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Icon = global::Properties.Resources.VixenPlus;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputPluginDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Input Plugin";
            this.groupBoxIOMapping.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBoxChannels.ResumeLayout(false);
            this.groupBoxChannels.PerformLayout();
            this.tabControlPlugin.ResumeLayout(false);
            this.tabPageMappingSets.ResumeLayout(false);
            this.tabControlMappingSets.ResumeLayout(false);
            this.tabPageSetDefinitions.ResumeLayout(false);
            this.tabPageInputOutputMapping.ResumeLayout(false);
            this.tabPageInputOutputMapping.PerformLayout();
            this.tabPageMappingIteration.ResumeLayout(false);
            this.tabPageMappingIteration.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabControlIterators.ResumeLayout(false);
            this.tabPageSingleIterator.ResumeLayout(false);
            this.tabPageSingleIterator.PerformLayout();
            this.tabPageMultipleIterators.ResumeLayout(false);
            this.tabPageMultipleIterators.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
