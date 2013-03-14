namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal class InputPluginDialog : Form
    {
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
        private IContainer components = null;
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
        private MappingSets m_editingMappingSets;
        private Dictionary<string, Channel> m_idChannel;
        private bool m_init = false;
        private bool m_internal = false;
        private InputPlugin m_plugin;
        private EventSequence m_sequence;
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

        public InputPluginDialog(InputPlugin plugin, EventSequence sequence)
        {
            this.InitializeComponent();
            this.m_idChannel = new Dictionary<string, Channel>();
            this.m_plugin = plugin;
            this.m_sequence = sequence;
            plugin.SetupDataToPlugin();
            this.m_init = true;
            this.listBoxInputs.DisplayMember = "Name";
            this.listBoxInputs.ValueMember = "OutputChannelId";
            this.listBoxInputs.DataSource = this.m_plugin.Inputs;
            foreach (Channel channel in this.m_sequence.Channels)
            {
                this.m_idChannel[channel.ID.ToString()] = channel;
            }
            this.listBoxChannels.Items.AddRange(this.m_sequence.Channels.ToArray());
            this.m_editingMappingSets = (MappingSets) this.m_plugin.MappingSets.Clone();
            this.m_init = false;
            if (this.listBoxInputs.SelectedItem != null)
            {
                this.ReflectInput((Input) this.listBoxInputs.SelectedItem);
            }
            this.checkBoxLiveUpdate.Checked = this.m_plugin.LiveUpdate;
            this.checkBoxRecord.Checked = this.m_plugin.Record;
            foreach (MappingSet set in this.m_editingMappingSets)
            {
                this.AddMappingSetListViewItem(set);
            }
            Input[] iterators = this.m_plugin.GetIterators();
            this.comboBoxSingleIteratorInput.Items.AddRange(iterators);
            this.listBoxIteratorInputs.Items.AddRange(iterators);
            if (this.m_plugin.MappingIteratorType == InputPlugin.MappingIterator.SingleInput)
            {
                this.radioButtonSingleIterator.Checked = true;
                this.comboBoxSingleIteratorInput.SelectedItem = this.m_plugin.SingleIterator;
            }
            else if (this.m_plugin.MappingIteratorType == InputPlugin.MappingIterator.MultiInput)
            {
                this.radioButtonMultipleIterators.Checked = true;
                this.tabControlIterators.SelectedTab = this.tabPageMultipleIterators;
            }
            else
            {
                this.radioButtonNoIterator.Checked = true;
            }
        }

        private void AddMappingSetListViewItem(MappingSet mappingSet)
        {
            this.listViewMappingSets.Items.Add(mappingSet.Name);
        }

        private void AssignMappingSetToInput(MappingSet mappingSet, Input input)
        {
            if (input != null)
            {
                input.AssignedMappingSet = mappingSet;
            }
        }

        private void buttonAddMappingSet_Click(object sender, EventArgs e)
        {
            this.AddMappingSetListViewItem(this.m_editingMappingSets.AddMapping());
        }

        private void buttonClearInputChannels_Click(object sender, EventArgs e)
        {
            if (this.listBoxInputs.SelectedItem != null)
            {
                Input selectedItem = (Input) this.listBoxInputs.SelectedItem;
                this.m_editingMappingSets.GetOutputChannelIdList(selectedItem).Clear();
                this.ReflectInput(selectedItem);
            }
        }

        private void buttonMoveDown_Click(object sender, EventArgs e)
        {
            int oldIndex = this.listViewMappingSets.SelectedIndices[0];
            this.m_editingMappingSets.MoveMappingTo(oldIndex, oldIndex + 1);
            ListViewItem item = this.listViewMappingSets.Items[oldIndex];
            this.listViewMappingSets.Items.RemoveAt(oldIndex);
            this.listViewMappingSets.Items.Insert(oldIndex + 1, item);
            this.listViewMappingSets.Items[oldIndex].Selected = false;
            this.listViewMappingSets.Items[oldIndex + 1].Selected = true;
        }

        private void buttonMoveUp_Click(object sender, EventArgs e)
        {
            int oldIndex = this.listViewMappingSets.SelectedIndices[0];
            this.m_editingMappingSets.MoveMappingTo(oldIndex, oldIndex - 1);
            ListViewItem item = this.listViewMappingSets.Items[oldIndex];
            this.listViewMappingSets.Items.RemoveAt(oldIndex);
            this.listViewMappingSets.Items.Insert(oldIndex - 1, item);
            this.listViewMappingSets.Items[oldIndex].Selected = false;
            this.listViewMappingSets.Items[oldIndex - 1].Selected = true;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (this.radioButtonSingleIterator.Checked && (this.comboBoxSingleIteratorInput.SelectedItem == null))
            {
                if (this.m_plugin.GetIterators().Length > 0)
                {
                    MessageBox.Show("You have selected to use a single input to iterate the mapping sets, but have not chosen an input.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.tabControlPlugin.SelectedTab = this.tabPageMappingIteration;
                }
                else
                {
                    MessageBox.Show("You have selected to use a single input to iterate the mapping sets, but do not have any inputs sets to be iterators and therefore have not chosen an input to iterate with.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                base.DialogResult = DialogResult.None;
            }
            else
            {
                this.m_plugin.MappingSets = this.m_editingMappingSets;
                this.m_plugin.LiveUpdate = this.checkBoxLiveUpdate.Checked;
                this.m_plugin.Record = this.checkBoxRecord.Checked;
                if (this.radioButtonSingleIterator.Checked)
                {
                    this.m_plugin.MappingIteratorType = InputPlugin.MappingIterator.SingleInput;
                }
                else if (this.radioButtonMultipleIterators.Checked)
                {
                    this.m_plugin.MappingIteratorType = InputPlugin.MappingIterator.MultiInput;
                }
                else
                {
                    this.m_plugin.MappingIteratorType = InputPlugin.MappingIterator.None;
                }
                if (this.m_plugin.MappingIteratorType == InputPlugin.MappingIterator.SingleInput)
                {
                    this.m_plugin.SingleIterator = (Input) this.comboBoxSingleIteratorInput.SelectedItem;
                }
                else
                {
                    this.m_plugin.SingleIterator = null;
                }
                this.m_plugin.PluginToSetupData();
            }
        }

        private void buttonRemoveMappingSet_Click(object sender, EventArgs e)
        {
            this.m_editingMappingSets.RemoveMappingAt(this.listViewMappingSets.SelectedIndices[0]);
            this.listViewMappingSets.Items.RemoveAt(this.listViewMappingSets.SelectedIndices[0]);
            this.buttonRemoveMappingSet.Enabled = this.buttonMoveUp.Enabled = this.buttonMoveDown.Enabled = false;
            this.UpdateIteratorType();
        }

        private void buttonRenameMappingSet_Click(object sender, EventArgs e)
        {
        }

        private void checkBoxEnabled_CheckedChanged(object sender, EventArgs e)
        {
            this.checkBoxEnabled.Text = this.checkBoxEnabled.Checked ? "Input is enabled and is mapped to:" : "Input is disabled";
            this.listBoxChannels.Enabled = this.checkBoxEnabled.Checked;
        }

        private void checkBoxEnabled_Click(object sender, EventArgs e)
        {
            if (this.listBoxInputs.SelectedItem != null)
            {
                ((Input) this.listBoxInputs.SelectedItem).Enabled = this.checkBoxEnabled.Checked;
            }
        }

        private void comboBoxMappingSet_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.m_editingMappingSets.CurrentMappingSet = (MappingSet) this.comboBoxMappingSet.SelectedItem;
            this.listBoxInputs.SelectedIndex = -1;
            this.listBoxMappedChannels.Items.Clear();
            this.listBoxChannels.ClearSelected();
            this.groupBoxIOMapping.Enabled = this.comboBoxMappingSet.SelectedIndex != -1;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(InputPluginDialog));
            this.groupBoxIOMapping = new GroupBox();
            this.listBoxInputs = new ListBox();
            this.groupBox2 = new GroupBox();
            this.listBoxMappedChannels = new ListBox();
            this.groupBoxChannels = new GroupBox();
            this.checkBoxEnabled = new CheckBox();
            this.buttonClearInputChannels = new Button();
            this.listBoxChannels = new ListBox();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.checkBoxLiveUpdate = new CheckBox();
            this.checkBoxRecord = new CheckBox();
            this.tabControlPlugin = new System.Windows.Forms.TabControl();
            this.tabPageMappingSets = new TabPage();
            this.tabControlMappingSets = new System.Windows.Forms.TabControl();
            this.tabPageSetDefinitions = new TabPage();
            this.buttonRenameMappingSet = new Button();
            this.buttonMoveDown = new Button();
            this.imageList = new ImageList(this.components);
            this.buttonMoveUp = new Button();
            this.buttonRemoveMappingSet = new Button();
            this.buttonAddMappingSet = new Button();
            this.listViewMappingSets = new ListView();
            this.columnHeader1 = new ColumnHeader();
            this.tabPageInputOutputMapping = new TabPage();
            this.comboBoxMappingSet = new ComboBox();
            this.label1 = new Label();
            this.tabPageMappingIteration = new TabPage();
            this.panel1 = new Panel();
            this.tabControlIterators = new System.Windows.Forms.TabControl();
            this.tabPageSingleIterator = new TabPage();
            this.comboBoxSingleIteratorInput = new ComboBox();
            this.label2 = new Label();
            this.tabPageMultipleIterators = new TabPage();
            this.listBoxIteratorInputs = new ListBox();
            this.label4 = new Label();
            this.listBoxMappingSets = new ListBox();
            this.label3 = new Label();
            this.radioButtonMultipleIterators = new RadioButton();
            this.radioButtonSingleIterator = new RadioButton();
            this.radioButtonNoIterator = new RadioButton();
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
            base.SuspendLayout();
            this.groupBoxIOMapping.Controls.Add(this.listBoxInputs);
            this.groupBoxIOMapping.Controls.Add(this.groupBox2);
            this.groupBoxIOMapping.Controls.Add(this.groupBoxChannels);
            this.groupBoxIOMapping.Enabled = false;
            this.groupBoxIOMapping.Location = new Point(13, 0x3a);
            this.groupBoxIOMapping.Name = "groupBoxIOMapping";
            this.groupBoxIOMapping.Size = new Size(0x162, 0x14e);
            this.groupBoxIOMapping.TabIndex = 4;
            this.groupBoxIOMapping.TabStop = false;
            this.groupBoxIOMapping.Text = "Input-Output Mapping";
            this.listBoxInputs.FormattingEnabled = true;
            this.listBoxInputs.Location = new Point(15, 0x1a);
            this.listBoxInputs.Name = "listBoxInputs";
            this.listBoxInputs.Size = new Size(0x98, 0x6c);
            this.listBoxInputs.TabIndex = 6;
            this.listBoxInputs.SelectedIndexChanged += new EventHandler(this.listBoxInputs_SelectedIndexChanged);
            this.groupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox2.Controls.Add(this.listBoxMappedChannels);
            this.groupBox2.Location = new Point(0xb8, 0x13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0xa1, 0x79);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Input is mapped to:";
            this.listBoxMappedChannels.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.listBoxMappedChannels.BackColor = SystemColors.Control;
            this.listBoxMappedChannels.FormattingEnabled = true;
            this.listBoxMappedChannels.Location = new Point(6, 0x13);
            this.listBoxMappedChannels.Name = "listBoxMappedChannels";
            this.listBoxMappedChannels.Size = new Size(0x95, 0x5f);
            this.listBoxMappedChannels.TabIndex = 0;
            this.groupBoxChannels.Controls.Add(this.checkBoxEnabled);
            this.groupBoxChannels.Controls.Add(this.buttonClearInputChannels);
            this.groupBoxChannels.Controls.Add(this.listBoxChannels);
            this.groupBoxChannels.Location = new Point(11, 0x92);
            this.groupBoxChannels.Name = "groupBoxChannels";
            this.groupBoxChannels.Size = new Size(0x14e, 0xad);
            this.groupBoxChannels.TabIndex = 4;
            this.groupBoxChannels.TabStop = false;
            this.checkBoxEnabled.AutoSize = true;
            this.checkBoxEnabled.Location = new Point(13, 0x12);
            this.checkBoxEnabled.Name = "checkBoxEnabled";
            this.checkBoxEnabled.Size = new Size(0x66, 0x11);
            this.checkBoxEnabled.TabIndex = 3;
            this.checkBoxEnabled.Text = "Input is disabled";
            this.checkBoxEnabled.UseVisualStyleBackColor = true;
            this.checkBoxEnabled.Click += new EventHandler(this.checkBoxEnabled_Click);
            this.checkBoxEnabled.CheckedChanged += new EventHandler(this.checkBoxEnabled_CheckedChanged);
            this.buttonClearInputChannels.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonClearInputChannels.Location = new Point(250, 0x8e);
            this.buttonClearInputChannels.Name = "buttonClearInputChannels";
            this.buttonClearInputChannels.Size = new Size(0x4b, 0x17);
            this.buttonClearInputChannels.TabIndex = 5;
            this.buttonClearInputChannels.Text = "Clear";
            this.buttonClearInputChannels.UseVisualStyleBackColor = true;
            this.buttonClearInputChannels.Click += new EventHandler(this.buttonClearInputChannels_Click);
            this.listBoxChannels.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.listBoxChannels.Enabled = false;
            this.listBoxChannels.FormattingEnabled = true;
            this.listBoxChannels.Location = new Point(13, 0x29);
            this.listBoxChannels.Name = "listBoxChannels";
            this.listBoxChannels.SelectionMode = SelectionMode.MultiExtended;
            this.listBoxChannels.Size = new Size(0x13b, 0x5f);
            this.listBoxChannels.TabIndex = 4;
            this.listBoxChannels.SelectedIndexChanged += new EventHandler(this.listBoxChannels_SelectedIndexChanged);
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(0x111, 0x215);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x162, 0x215);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.checkBoxLiveUpdate.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.checkBoxLiveUpdate.AutoSize = true;
            this.checkBoxLiveUpdate.Location = new Point(12, 510);
            this.checkBoxLiveUpdate.Name = "checkBoxLiveUpdate";
            this.checkBoxLiveUpdate.Size = new Size(0x93, 0x11);
            this.checkBoxLiveUpdate.TabIndex = 5;
            this.checkBoxLiveUpdate.Text = "Live-update the hardware";
            this.checkBoxLiveUpdate.UseVisualStyleBackColor = true;
            this.checkBoxRecord.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.checkBoxRecord.AutoSize = true;
            this.checkBoxRecord.Location = new Point(12, 0x215);
            this.checkBoxRecord.Name = "checkBoxRecord";
            this.checkBoxRecord.Size = new Size(0x8d, 0x11);
            this.checkBoxRecord.TabIndex = 7;
            this.checkBoxRecord.Text = "Record to the sequence";
            this.checkBoxRecord.UseVisualStyleBackColor = true;
            this.tabControlPlugin.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.tabControlPlugin.Controls.Add(this.tabPageMappingSets);
            this.tabControlPlugin.Controls.Add(this.tabPageMappingIteration);
            this.tabControlPlugin.Location = new Point(12, 12);
            this.tabControlPlugin.Name = "tabControlPlugin";
            this.tabControlPlugin.SelectedIndex = 0;
            this.tabControlPlugin.Size = new Size(0x1a1, 0x1ec);
            this.tabControlPlugin.TabIndex = 8;
            this.tabControlPlugin.Selecting += new TabControlCancelEventHandler(this.tabControlPlugin_Selecting);
            this.tabPageMappingSets.Controls.Add(this.tabControlMappingSets);
            this.tabPageMappingSets.Location = new Point(4, 0x16);
            this.tabPageMappingSets.Name = "tabPageMappingSets";
            this.tabPageMappingSets.Padding = new Padding(3);
            this.tabPageMappingSets.Size = new Size(0x199, 0x1d2);
            this.tabPageMappingSets.TabIndex = 0;
            this.tabPageMappingSets.Text = "Mapping Sets";
            this.tabPageMappingSets.UseVisualStyleBackColor = true;
            this.tabControlMappingSets.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.tabControlMappingSets.Controls.Add(this.tabPageSetDefinitions);
            this.tabControlMappingSets.Controls.Add(this.tabPageInputOutputMapping);
            this.tabControlMappingSets.Location = new Point(10, 12);
            this.tabControlMappingSets.Name = "tabControlMappingSets";
            this.tabControlMappingSets.SelectedIndex = 0;
            this.tabControlMappingSets.Size = new Size(0x17d, 0x1c0);
            this.tabControlMappingSets.TabIndex = 5;
            this.tabControlMappingSets.Selecting += new TabControlCancelEventHandler(this.tabControlMappingSets_Selecting);
            this.tabPageSetDefinitions.Controls.Add(this.buttonRenameMappingSet);
            this.tabPageSetDefinitions.Controls.Add(this.buttonMoveDown);
            this.tabPageSetDefinitions.Controls.Add(this.buttonMoveUp);
            this.tabPageSetDefinitions.Controls.Add(this.buttonRemoveMappingSet);
            this.tabPageSetDefinitions.Controls.Add(this.buttonAddMappingSet);
            this.tabPageSetDefinitions.Controls.Add(this.listViewMappingSets);
            this.tabPageSetDefinitions.Location = new Point(4, 0x16);
            this.tabPageSetDefinitions.Name = "tabPageSetDefinitions";
            this.tabPageSetDefinitions.Padding = new Padding(3);
            this.tabPageSetDefinitions.Size = new Size(0x175, 0x1a6);
            this.tabPageSetDefinitions.TabIndex = 0;
            this.tabPageSetDefinitions.Text = "Set Definitions";
            this.tabPageSetDefinitions.UseVisualStyleBackColor = true;
            this.buttonRenameMappingSet.Enabled = false;
            this.buttonRenameMappingSet.Location = new Point(0xb9, 0xc1);
            this.buttonRenameMappingSet.Name = "buttonRenameMappingSet";
            this.buttonRenameMappingSet.Size = new Size(0x4b, 0x17);
            this.buttonRenameMappingSet.TabIndex = 5;
            this.buttonRenameMappingSet.Text = "Rename";
            this.buttonRenameMappingSet.UseVisualStyleBackColor = true;
            this.buttonRenameMappingSet.Visible = false;
            this.buttonRenameMappingSet.Click += new EventHandler(this.buttonRenameMappingSet_Click);
            this.buttonMoveDown.Enabled = false;
            this.buttonMoveDown.ImageIndex = 1;
            this.buttonMoveDown.ImageList = this.imageList;
            this.buttonMoveDown.Location = new Point(0x155, 0x3b);
            this.buttonMoveDown.Name = "buttonMoveDown";
            this.buttonMoveDown.Size = new Size(0x1a, 0x17);
            this.buttonMoveDown.TabIndex = 4;
            this.buttonMoveDown.UseVisualStyleBackColor = true;
            this.buttonMoveDown.Click += new EventHandler(this.buttonMoveDown_Click);
            this.imageList.ImageStream = (ImageListStreamer) manager.GetObject("imageList.ImageStream");
            this.imageList.TransparentColor = Color.White;
            this.imageList.Images.SetKeyName(0, "UpArrowBlack.bmp");
            this.imageList.Images.SetKeyName(1, "DownArrowBlack.bmp");
            this.buttonMoveUp.Enabled = false;
            this.buttonMoveUp.ImageIndex = 0;
            this.buttonMoveUp.ImageList = this.imageList;
            this.buttonMoveUp.Location = new Point(0x155, 30);
            this.buttonMoveUp.Name = "buttonMoveUp";
            this.buttonMoveUp.Size = new Size(0x1a, 0x17);
            this.buttonMoveUp.TabIndex = 3;
            this.buttonMoveUp.UseVisualStyleBackColor = true;
            this.buttonMoveUp.Click += new EventHandler(this.buttonMoveUp_Click);
            this.buttonRemoveMappingSet.Enabled = false;
            this.buttonRemoveMappingSet.Location = new Point(0x68, 0xc1);
            this.buttonRemoveMappingSet.Name = "buttonRemoveMappingSet";
            this.buttonRemoveMappingSet.Size = new Size(0x4b, 0x17);
            this.buttonRemoveMappingSet.TabIndex = 2;
            this.buttonRemoveMappingSet.Text = "Remove";
            this.buttonRemoveMappingSet.UseVisualStyleBackColor = true;
            this.buttonRemoveMappingSet.Click += new EventHandler(this.buttonRemoveMappingSet_Click);
            this.buttonAddMappingSet.Location = new Point(0x17, 0xc1);
            this.buttonAddMappingSet.Name = "buttonAddMappingSet";
            this.buttonAddMappingSet.Size = new Size(0x4b, 0x17);
            this.buttonAddMappingSet.TabIndex = 1;
            this.buttonAddMappingSet.Text = "Add New";
            this.buttonAddMappingSet.UseVisualStyleBackColor = true;
            this.buttonAddMappingSet.Click += new EventHandler(this.buttonAddMappingSet_Click);
            this.listViewMappingSets.Columns.AddRange(new ColumnHeader[] { this.columnHeader1 });
            this.listViewMappingSets.HeaderStyle = ColumnHeaderStyle.None;
            this.listViewMappingSets.HideSelection = false;
            this.listViewMappingSets.LabelEdit = true;
            this.listViewMappingSets.Location = new Point(0x17, 30);
            this.listViewMappingSets.Name = "listViewMappingSets";
            this.listViewMappingSets.Size = new Size(0x138, 0x9d);
            this.listViewMappingSets.TabIndex = 0;
            this.listViewMappingSets.UseCompatibleStateImageBehavior = false;
            this.listViewMappingSets.View = View.Details;
            this.listViewMappingSets.AfterLabelEdit += new LabelEditEventHandler(this.listViewMappingSets_AfterLabelEdit);
            this.listViewMappingSets.SelectedIndexChanged += new EventHandler(this.listViewMappingSets_SelectedIndexChanged);
            this.columnHeader1.Width = 0x11b;
            this.tabPageInputOutputMapping.Controls.Add(this.comboBoxMappingSet);
            this.tabPageInputOutputMapping.Controls.Add(this.label1);
            this.tabPageInputOutputMapping.Controls.Add(this.groupBoxIOMapping);
            this.tabPageInputOutputMapping.Location = new Point(4, 0x16);
            this.tabPageInputOutputMapping.Name = "tabPageInputOutputMapping";
            this.tabPageInputOutputMapping.Padding = new Padding(3);
            this.tabPageInputOutputMapping.Size = new Size(0x175, 0x1a6);
            this.tabPageInputOutputMapping.TabIndex = 1;
            this.tabPageInputOutputMapping.Text = "Set Input/Output Mappings";
            this.tabPageInputOutputMapping.UseVisualStyleBackColor = true;
            this.comboBoxMappingSet.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxMappingSet.FormattingEnabled = true;
            this.comboBoxMappingSet.Location = new Point(0x6b, 0x12);
            this.comboBoxMappingSet.Name = "comboBoxMappingSet";
            this.comboBoxMappingSet.Size = new Size(0xf2, 0x15);
            this.comboBoxMappingSet.TabIndex = 6;
            this.comboBoxMappingSet.SelectedIndexChanged += new EventHandler(this.comboBoxMappingSet_SelectedIndexChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x13, 20);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x44, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Mapping set:";
            this.tabPageMappingIteration.Controls.Add(this.radioButtonNoIterator);
            this.tabPageMappingIteration.Controls.Add(this.panel1);
            this.tabPageMappingIteration.Controls.Add(this.radioButtonMultipleIterators);
            this.tabPageMappingIteration.Controls.Add(this.radioButtonSingleIterator);
            this.tabPageMappingIteration.Location = new Point(4, 0x16);
            this.tabPageMappingIteration.Name = "tabPageMappingIteration";
            this.tabPageMappingIteration.Padding = new Padding(3);
            this.tabPageMappingIteration.Size = new Size(0x199, 0x1d2);
            this.tabPageMappingIteration.TabIndex = 1;
            this.tabPageMappingIteration.Text = "Mapping Set Iteration";
            this.tabPageMappingIteration.UseVisualStyleBackColor = true;
            this.panel1.BorderStyle = BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.tabControlIterators);
            this.panel1.Location = new Point(0x19, 0x6a);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x164, 0xf2);
            this.panel1.TabIndex = 2;
            this.tabControlIterators.Controls.Add(this.tabPageSingleIterator);
            this.tabControlIterators.Controls.Add(this.tabPageMultipleIterators);
            this.tabControlIterators.Location = new Point(0, -22);
            this.tabControlIterators.Name = "tabControlIterators";
            this.tabControlIterators.SelectedIndex = 0;
            this.tabControlIterators.Size = new Size(0x164, 0x108);
            this.tabControlIterators.TabIndex = 0;
            this.tabPageSingleIterator.Controls.Add(this.comboBoxSingleIteratorInput);
            this.tabPageSingleIterator.Controls.Add(this.label2);
            this.tabPageSingleIterator.Location = new Point(4, 0x16);
            this.tabPageSingleIterator.Name = "tabPageSingleIterator";
            this.tabPageSingleIterator.Padding = new Padding(3);
            this.tabPageSingleIterator.Size = new Size(0x15c, 0xee);
            this.tabPageSingleIterator.TabIndex = 0;
            this.tabPageSingleIterator.Text = "tabPageSingleIterator";
            this.tabPageSingleIterator.UseVisualStyleBackColor = true;
            this.comboBoxSingleIteratorInput.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxSingleIteratorInput.FormattingEnabled = true;
            this.comboBoxSingleIteratorInput.Location = new Point(0x3a, 0x10);
            this.comboBoxSingleIteratorInput.Name = "comboBoxSingleIteratorInput";
            this.comboBoxSingleIteratorInput.Size = new Size(0x103, 0x15);
            this.comboBoxSingleIteratorInput.TabIndex = 1;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x12, 0x13);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x22, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Input:";
            this.tabPageMultipleIterators.Controls.Add(this.listBoxIteratorInputs);
            this.tabPageMultipleIterators.Controls.Add(this.label4);
            this.tabPageMultipleIterators.Controls.Add(this.listBoxMappingSets);
            this.tabPageMultipleIterators.Controls.Add(this.label3);
            this.tabPageMultipleIterators.Location = new Point(4, 0x16);
            this.tabPageMultipleIterators.Name = "tabPageMultipleIterators";
            this.tabPageMultipleIterators.Padding = new Padding(3);
            this.tabPageMultipleIterators.Size = new Size(0x15c, 0xee);
            this.tabPageMultipleIterators.TabIndex = 1;
            this.tabPageMultipleIterators.Text = "tabPageMultipleIterators";
            this.tabPageMultipleIterators.UseVisualStyleBackColor = true;
            this.listBoxIteratorInputs.FormattingEnabled = true;
            this.listBoxIteratorInputs.Location = new Point(0x1f, 30);
            this.listBoxIteratorInputs.Name = "listBoxIteratorInputs";
            this.listBoxIteratorInputs.Size = new Size(120, 0xad);
            this.listBoxIteratorInputs.TabIndex = 1;
            this.listBoxIteratorInputs.SelectedIndexChanged += new EventHandler(this.listBoxIteratorInputs_SelectedIndexChanged);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x1c, 14);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x22, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Input:";
            this.listBoxMappingSets.FormattingEnabled = true;
            this.listBoxMappingSets.Items.AddRange(new object[] { "(none)" });
            this.listBoxMappingSets.Location = new Point(0xc1, 30);
            this.listBoxMappingSets.Name = "listBoxMappingSets";
            this.listBoxMappingSets.Size = new Size(120, 0xad);
            this.listBoxMappingSets.TabIndex = 3;
            this.listBoxMappingSets.SelectedIndexChanged += new EventHandler(this.listBoxMappingSets_SelectedIndexChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(190, 14);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x44, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Mapping set:";
            this.radioButtonMultipleIterators.AutoSize = true;
            this.radioButtonMultipleIterators.Location = new Point(0x1a, 70);
            this.radioButtonMultipleIterators.Name = "radioButtonMultipleIterators";
            this.radioButtonMultipleIterators.Size = new Size(220, 0x11);
            this.radioButtonMultipleIterators.TabIndex = 2;
            this.radioButtonMultipleIterators.Text = "Assign each mapping set to a single input";
            this.radioButtonMultipleIterators.UseVisualStyleBackColor = true;
            this.radioButtonMultipleIterators.CheckedChanged += new EventHandler(this.radioButtonMultipleIterators_CheckedChanged);
            this.radioButtonSingleIterator.AutoSize = true;
            this.radioButtonSingleIterator.Location = new Point(0x1a, 0x2f);
            this.radioButtonSingleIterator.Name = "radioButtonSingleIterator";
            this.radioButtonSingleIterator.Size = new Size(0x12e, 0x11);
            this.radioButtonSingleIterator.TabIndex = 1;
            this.radioButtonSingleIterator.Text = "Use a single input to iterate through the list of mapping sets";
            this.radioButtonSingleIterator.UseVisualStyleBackColor = true;
            this.radioButtonSingleIterator.CheckedChanged += new EventHandler(this.radioButtonSingleIterator_CheckedChanged);
            this.radioButtonNoIterator.AutoSize = true;
            this.radioButtonNoIterator.Checked = true;
            this.radioButtonNoIterator.Location = new Point(0x1a, 0x18);
            this.radioButtonNoIterator.Name = "radioButtonNoIterator";
            this.radioButtonNoIterator.Size = new Size(170, 0x11);
            this.radioButtonNoIterator.TabIndex = 0;
            this.radioButtonNoIterator.TabStop = true;
            this.radioButtonNoIterator.Text = "No iterator (single mapping set)";
            this.radioButtonNoIterator.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x1b9, 0x234);
            base.Controls.Add(this.tabControlPlugin);
            base.Controls.Add(this.checkBoxRecord);
            base.Controls.Add(this.checkBoxLiveUpdate);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "InputPluginDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
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
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void listBoxChannels_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.m_internal)
            {
                List<Channel> list = new List<Channel>();
                foreach (Channel channel in this.listBoxMappedChannels.Items)
                {
                    list.Add(channel);
                }
                foreach (Channel channel in this.listBoxMappedChannels.Items)
                {
                    if (!this.listBoxChannels.SelectedItems.Contains(channel))
                    {
                        list.Remove(channel);
                    }
                }
                foreach (Channel channel in this.listBoxChannels.SelectedItems)
                {
                    if (!list.Contains(channel))
                    {
                        list.Add(channel);
                    }
                }
                if (this.listBoxInputs.SelectedItem != null)
                {
                    List<string> outputChannelIdList = this.m_editingMappingSets.GetOutputChannelIdList((Input) this.listBoxInputs.SelectedItem);
                    outputChannelIdList.Clear();
                    foreach (Channel channel in list)
                    {
                        outputChannelIdList.Add(channel.ID.ToString());
                    }
                }
                this.listBoxMappedChannels.BeginUpdate();
                this.listBoxMappedChannels.Items.Clear();
                this.listBoxMappedChannels.Items.AddRange(list.ToArray());
                this.listBoxMappedChannels.EndUpdate();
            }
        }

        private void listBoxInputs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.m_init && (this.listBoxInputs.SelectedItem != null))
            {
                this.ReflectInput((Input) this.listBoxInputs.SelectedItem);
            }
        }

        private void listBoxIteratorInputs_SelectedIndexChanged(object sender, EventArgs e)
        {
            Input selectedItem = (Input) this.listBoxIteratorInputs.SelectedItem;
            if (selectedItem.AssignedMappingSet == null)
            {
                this.listBoxMappingSets.SelectedIndex = 0;
            }
            else
            {
                this.listBoxMappingSets.SelectedItem = selectedItem.AssignedMappingSet;
            }
        }

        private void listBoxMappingSets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(this.listBoxMappingSets.SelectedItem is MappingSet))
            {
                this.AssignMappingSetToInput(null, (Input) this.listBoxIteratorInputs.SelectedItem);
            }
            else
            {
                this.AssignMappingSetToInput((MappingSet) this.listBoxMappingSets.SelectedItem, (Input) this.listBoxIteratorInputs.SelectedItem);
            }
        }

        private void listViewMappingSets_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if ((e.Label != null) && (e.Label.Trim().Length > 0))
            {
                this.m_editingMappingSets[this.listViewMappingSets.SelectedIndices[0]].Name = e.Label;
            }
        }

        private void listViewMappingSets_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.buttonRemoveMappingSet.Enabled = this.listViewMappingSets.SelectedItems.Count > 0;
            this.buttonMoveUp.Enabled = (this.listViewMappingSets.SelectedItems.Count > 0) && (this.listViewMappingSets.SelectedIndices[0] != 0);
            this.buttonMoveDown.Enabled = (this.listViewMappingSets.SelectedItems.Count > 0) && (this.listViewMappingSets.SelectedIndices[0] < (this.listViewMappingSets.Items.Count - 1));
        }

        private void radioButtonMultipleIterators_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonMultipleIterators.Checked)
            {
                this.tabControlIterators.SelectedTab = this.tabPageMultipleIterators;
            }
        }

        private void radioButtonSingleIterator_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonSingleIterator.Checked)
            {
                this.tabControlIterators.SelectedTab = this.tabPageSingleIterator;
            }
        }

        private void ReflectInput(Input input)
        {
            if (this.groupBoxIOMapping.Enabled)
            {
                this.m_internal = true;
                this.listBoxChannels.ClearSelected();
                this.m_internal = false;
                this.listBoxMappedChannels.Items.Clear();
                if (this.listBoxInputs.SelectedValue != null)
                {
                    this.listBoxChannels.BeginUpdate();
                    this.listBoxMappedChannels.BeginUpdate();
                    this.m_internal = true;
                    foreach (string str in this.m_editingMappingSets.GetOutputChannelIdList(input))
                    {
                        Channel channel;
                        if (this.m_idChannel.TryGetValue(str, out channel))
                        {
                            this.listBoxMappedChannels.Items.Add(channel);
                            this.listBoxChannels.SetSelected(this.listBoxChannels.Items.IndexOf(channel), true);
                        }
                    }
                    this.m_internal = false;
                    this.listBoxMappedChannels.EndUpdate();
                    this.listBoxChannels.EndUpdate();
                    this.checkBoxEnabled.Checked = input.Enabled;
                }
            }
        }

        private void tabControlMappingSets_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == this.tabPageInputOutputMapping)
            {
                MappingSet selectedItem = null;
                if (this.comboBoxMappingSet.SelectedItem != null)
                {
                    selectedItem = (MappingSet) this.comboBoxMappingSet.SelectedItem;
                }
                this.comboBoxMappingSet.BeginUpdate();
                this.comboBoxMappingSet.Items.Clear();
                this.comboBoxMappingSet.Items.AddRange(this.m_editingMappingSets.AllSets);
                this.comboBoxMappingSet.EndUpdate();
                if ((selectedItem != null) && this.comboBoxMappingSet.Items.Contains(selectedItem))
                {
                    this.comboBoxMappingSet.SelectedItem = selectedItem;
                }
                else if (this.comboBoxMappingSet.Items.Count > 0)
                {
                    this.comboBoxMappingSet.SelectedIndex = 0;
                }
                else
                {
                    this.groupBoxIOMapping.Enabled = false;
                }
            }
        }

        private void tabControlPlugin_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == this.tabPageMappingIteration)
            {
                int selectedIndex = this.comboBoxSingleIteratorInput.SelectedIndex;
                this.comboBoxSingleIteratorInput.Items.Clear();
                this.comboBoxSingleIteratorInput.Items.AddRange(this.m_plugin.GetIterators());
                this.listBoxIteratorInputs.Items.Clear();
                this.listBoxIteratorInputs.Items.AddRange(this.m_plugin.GetIterators());
                this.comboBoxSingleIteratorInput.SelectedIndex = selectedIndex;
                this.listBoxMappingSets.Items.Clear();
                this.listBoxMappingSets.Items.Add("(none)");
                this.listBoxMappingSets.Items.AddRange(this.m_editingMappingSets.AllSets);
            }
        }

        private void UpdateIteratorType()
        {
            if (this.listViewMappingSets.Items.Count <= 1)
            {
                this.radioButtonNoIterator.Checked = true;
            }
            else if (this.radioButtonNoIterator.Checked)
            {
                this.radioButtonSingleIterator.Checked = true;
            }
        }
    }
}

