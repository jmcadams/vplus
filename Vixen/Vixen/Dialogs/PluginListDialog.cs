namespace Vixen.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class PluginListDialog : Form
    {
        private Button buttonCancel;
        private Button buttonInput;
        private Button buttonOK;
        private Button buttonPluginSetup;
        private Button buttonRemove;
        private Button buttonUse;
        private CheckedListBox checkedListBoxSequencePlugins;
        private ColumnHeader columnHeaderExpandButton;
        private ColumnHeader columnHeaderPluginName;
        private ColumnHeader columnHeaderPortTypeIndex;
        private ColumnHeader columnHeaderPortTypeName;
        private IContainer components = null;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private ListView listViewOutputPorts;
        private ListView listViewPlugins;
        private List<Channel> m_channels;
        private Rectangle m_collapsedRelativeBounds;
        private IExecutable m_executableObject = null;
        private Rectangle m_expandedRelativeBounds;
        private bool m_internalUpdate = false;
        private int m_itemAffectedIndex;
        private int m_lastIndex = -1;
        private Dictionary<string, Dictionary<int, OutputPort>> m_outputPorts;
        private List<IHardwarePlugin> m_sequencePlugins;
        private SetupData m_setupData = null;
        private PictureBox pictureBoxMinus;
        private PictureBox pictureBoxPlus;
        private ColumnHeader pluginName;
        private TextBox textBoxChannelFrom;
        private TextBox textBoxChannelTo;

        public PluginListDialog(IExecutable executableObject)
        {
            this.m_setupData = executableObject.PlugInData;
            this.m_executableObject = executableObject;
            this.m_channels = executableObject.Channels;
            this.InitializeComponent();
            this.m_sequencePlugins = new List<IHardwarePlugin>();
            this.m_outputPorts = new Dictionary<string, Dictionary<int, OutputPort>>();
            this.Cursor = Cursors.WaitCursor;
            try
            {
                ListViewItem item;
                this.listViewPlugins.Columns[0].Width = this.listViewPlugins.Width - 0x19;
                ListViewGroup group = this.listViewPlugins.Groups["listViewGroupOutput"];
                ListViewGroup group2 = this.listViewPlugins.Groups["listViewGroupInput"];
                List<IHardwarePlugin> list = OutputPlugins.LoadPlugins();
                if (list != null)
                {
                    foreach (IHardwarePlugin plugin in list)
                    {
                        item = new ListViewItem(plugin.Name, group);
                        item.Tag = plugin;
                        this.listViewPlugins.Items.Add(item);
                    }
                }
                list = InputPlugins.LoadPlugins();
                if (list != null)
                {
                    foreach (IHardwarePlugin plugin in list)
                    {
                        item = new ListViewItem(plugin.Name, group2);
                        item.Tag = plugin;
                        this.listViewPlugins.Items.Add(item);
                    }
                }
                this.listViewPlugins.Enabled = this.listViewPlugins.Items.Count > 0;
                OutputPlugins.VerifyPlugIns(this.m_executableObject);
                InputPlugins.VerifyPlugIns(this.m_executableObject);
                this.m_collapsedRelativeBounds = new Rectangle(this.listViewOutputPorts.Columns[2].Width - (this.pictureBoxPlus.Width * 2), (14 - this.pictureBoxPlus.Height) / 2, this.pictureBoxPlus.Width, this.pictureBoxPlus.Height);
                this.m_expandedRelativeBounds = new Rectangle(this.listViewOutputPorts.Columns[2].Width - (this.pictureBoxMinus.Width * 2), (14 - this.pictureBoxMinus.Height) / 2, this.pictureBoxMinus.Width, this.pictureBoxMinus.Height);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void buttonInput_Click(object sender, EventArgs e)
        {
            InputPlugin plugin = (InputPlugin) this.m_sequencePlugins[this.checkedListBoxSequencePlugins.SelectedIndex];
            this.InitializePlugin(plugin, this.m_setupData.GetPlugInData(this.checkedListBoxSequencePlugins.SelectedIndex.ToString()));
            InputPluginDialog dialog = new InputPluginDialog(plugin, (EventSequence) this.m_executableObject);
            dialog.ShowDialog();
            dialog.Dispose();
        }

        private void buttonPluginSetup_Click(object sender, EventArgs e)
        {
            this.PluginSetup();
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            this.RemoveSelectedPlugIn();
        }

        private void buttonUse_Click(object sender, EventArgs e)
        {
            this.UsePlugin();
        }

        private void checkedListBoxSequencePlugins_DoubleClick(object sender, EventArgs e)
        {
            if (this.checkedListBoxSequencePlugins.SelectedIndex != -1)
            {
                this.PluginSetup();
            }
        }

        private void checkedListBoxSequencePlugins_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.Index != -1)
            {
                this.m_setupData.GetPlugInData(e.Index.ToString()).Attributes["enabled"].Value = (e.NewValue == CheckState.Checked).ToString();
                this.UpdateDictionary();
            }
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(PluginListDialog));
            ListViewGroup group = new ListViewGroup("Output", HorizontalAlignment.Left);
            ListViewGroup group2 = new ListViewGroup("Input", HorizontalAlignment.Left);
            this.buttonUse = new Button();
            this.label1 = new Label();
            this.textBoxChannelFrom = new TextBox();
            this.label2 = new Label();
            this.textBoxChannelTo = new TextBox();
            this.buttonPluginSetup = new Button();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.checkedListBoxSequencePlugins = new CheckedListBox();
            this.groupBox1 = new GroupBox();
            this.listViewOutputPorts = new ListView();
            this.columnHeaderPortTypeName = new ColumnHeader();
            this.columnHeaderPortTypeIndex = new ColumnHeader();
            this.columnHeaderExpandButton = new ColumnHeader();
            this.columnHeaderPluginName = new ColumnHeader();
            this.buttonRemove = new Button();
            this.pictureBoxPlus = new PictureBox();
            this.pictureBoxMinus = new PictureBox();
            this.label3 = new Label();
            this.label4 = new Label();
            this.buttonInput = new Button();
            this.listViewPlugins = new ListView();
            this.pluginName = new ColumnHeader();
            this.groupBox1.SuspendLayout();
            ((ISupportInitialize) this.pictureBoxPlus).BeginInit();
            ((ISupportInitialize) this.pictureBoxMinus).BeginInit();
            base.SuspendLayout();
            this.buttonUse.Enabled = false;
            this.buttonUse.Location = new Point(0xd6, 0x79);
            this.buttonUse.Name = "buttonUse";
            this.buttonUse.Size = new Size(0x4b, 0x17);
            this.buttonUse.TabIndex = 2;
            this.buttonUse.Text = "--- Use -->";
            this.buttonUse.UseVisualStyleBackColor = true;
            this.buttonUse.Click += new EventHandler(this.buttonUse_Click);
            this.label1.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x201, 0x20);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x33, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Channels";
            this.textBoxChannelFrom.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.textBoxChannelFrom.Location = new Point(490, 0x3a);
            this.textBoxChannelFrom.MaxLength = 4;
            this.textBoxChannelFrom.Name = "textBoxChannelFrom";
            this.textBoxChannelFrom.Size = new Size(0x22, 20);
            this.textBoxChannelFrom.TabIndex = 7;
            this.label2.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(530, 0x3d);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x10, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "to";
            this.textBoxChannelTo.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.textBoxChannelTo.Location = new Point(0x228, 0x3a);
            this.textBoxChannelTo.MaxLength = 4;
            this.textBoxChannelTo.Name = "textBoxChannelTo";
            this.textBoxChannelTo.Size = new Size(0x22, 20);
            this.textBoxChannelTo.TabIndex = 9;
            this.buttonPluginSetup.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.buttonPluginSetup.Enabled = false;
            this.buttonPluginSetup.Location = new Point(0x1f6, 0x54);
            this.buttonPluginSetup.Name = "buttonPluginSetup";
            this.buttonPluginSetup.Size = new Size(0x4b, 0x17);
            this.buttonPluginSetup.TabIndex = 10;
            this.buttonPluginSetup.Text = "Plugin Setup";
            this.buttonPluginSetup.UseVisualStyleBackColor = true;
            this.buttonPluginSetup.Click += new EventHandler(this.buttonPluginSetup_Click);
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(0x1fb, 0x111);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 14;
            this.buttonOK.Text = "Done";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x1fb, 0xf4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 13;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Visible = false;
            this.checkedListBoxSequencePlugins.FormattingEnabled = true;
            this.checkedListBoxSequencePlugins.Location = new Point(0x12a, 0x21);
            this.checkedListBoxSequencePlugins.Name = "checkedListBoxSequencePlugins";
            this.checkedListBoxSequencePlugins.Size = new Size(0xab, 0x8b);
            this.checkedListBoxSequencePlugins.TabIndex = 5;
            this.checkedListBoxSequencePlugins.SelectedIndexChanged += new EventHandler(this.listBoxSequencePlugins_SelectedIndexChanged);
            this.checkedListBoxSequencePlugins.ItemCheck += new ItemCheckEventHandler(this.checkedListBoxSequencePlugins_ItemCheck);
            this.checkedListBoxSequencePlugins.DoubleClick += new EventHandler(this.checkedListBoxSequencePlugins_DoubleClick);
            this.checkedListBoxSequencePlugins.KeyDown += new KeyEventHandler(this.listBoxSequencePlugins_KeyDown);
            this.groupBox1.Controls.Add(this.listViewOutputPorts);
            this.groupBox1.Location = new Point(12, 0xb2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x1c9, 0x76);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Port Configurations";
            this.listViewOutputPorts.Columns.AddRange(new ColumnHeader[] { this.columnHeaderPortTypeName, this.columnHeaderPortTypeIndex, this.columnHeaderExpandButton, this.columnHeaderPluginName });
            this.listViewOutputPorts.HeaderStyle = ColumnHeaderStyle.None;
            this.listViewOutputPorts.Location = new Point(6, 0x13);
            this.listViewOutputPorts.Name = "listViewOutputPorts";
            this.listViewOutputPorts.OwnerDraw = true;
            this.listViewOutputPorts.Size = new Size(0x1bd, 0x5d);
            this.listViewOutputPorts.TabIndex = 0;
            this.listViewOutputPorts.UseCompatibleStateImageBehavior = false;
            this.listViewOutputPorts.View = View.Details;
            this.listViewOutputPorts.DrawItem += new DrawListViewItemEventHandler(this.listViewOutputPorts_DrawItem);
            this.listViewOutputPorts.MouseDown += new MouseEventHandler(this.listViewOutputPorts_MouseDown);
            this.listViewOutputPorts.DrawSubItem += new DrawListViewSubItemEventHandler(this.listViewOutputPorts_DrawSubItem);
            this.columnHeaderPortTypeName.Text = "PortTypeName";
            this.columnHeaderPortTypeName.Width = 0x37;
            this.columnHeaderPortTypeIndex.Text = "PortTypeIndex";
            this.columnHeaderPortTypeIndex.Width = 0x29;
            this.columnHeaderExpandButton.Text = "";
            this.columnHeaderExpandButton.Width = 0x29;
            this.columnHeaderPluginName.Text = "PluginName";
            this.columnHeaderPluginName.Width = 0xfb;
            this.buttonRemove.Enabled = false;
            this.buttonRemove.Location = new Point(0xd6, 150);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new Size(0x4b, 0x17);
            this.buttonRemove.TabIndex = 3;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new EventHandler(this.buttonRemove_Click);
            this.pictureBoxPlus.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.pictureBoxPlus.Image = (Image) manager.GetObject("pictureBoxPlus.Image");
            this.pictureBoxPlus.Location = new Point(0x1f2, 0x7d);
            this.pictureBoxPlus.Name = "pictureBoxPlus";
            this.pictureBoxPlus.Size = new Size(9, 9);
            this.pictureBoxPlus.SizeMode = PictureBoxSizeMode.AutoSize;
            this.pictureBoxPlus.TabIndex = 12;
            this.pictureBoxPlus.TabStop = false;
            this.pictureBoxPlus.Visible = false;
            this.pictureBoxMinus.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.pictureBoxMinus.Image = (Image) manager.GetObject("pictureBoxMinus.Image");
            this.pictureBoxMinus.Location = new Point(0x201, 0x7d);
            this.pictureBoxMinus.Name = "pictureBoxMinus";
            this.pictureBoxMinus.Size = new Size(9, 9);
            this.pictureBoxMinus.SizeMode = PictureBoxSizeMode.AutoSize;
            this.pictureBoxMinus.TabIndex = 13;
            this.pictureBoxMinus.TabStop = false;
            this.pictureBoxMinus.Visible = false;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(12, 11);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x57, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Available Plugins";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x127, 12);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x4a, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Plugins in Use";
            this.buttonInput.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.buttonInput.Enabled = false;
            this.buttonInput.Location = new Point(0x1f6, 0x95);
            this.buttonInput.Name = "buttonInput";
            this.buttonInput.Size = new Size(0x4b, 0x17);
            this.buttonInput.TabIndex = 11;
            this.buttonInput.Text = "Inputs";
            this.buttonInput.UseVisualStyleBackColor = true;
            this.buttonInput.Click += new EventHandler(this.buttonInput_Click);
            this.listViewPlugins.Columns.AddRange(new ColumnHeader[] { this.pluginName });
            group.Header = "Output";
            group.Name = "listViewGroupOutput";
            group2.Header = "Input";
            group2.Name = "listViewGroupInput";
            this.listViewPlugins.Groups.AddRange(new ListViewGroup[] { group, group2 });
            this.listViewPlugins.HeaderStyle = ColumnHeaderStyle.None;
            this.listViewPlugins.HideSelection = false;
            this.listViewPlugins.Location = new Point(12, 0x20);
            this.listViewPlugins.MultiSelect = false;
            this.listViewPlugins.Name = "listViewPlugins";
            this.listViewPlugins.Size = new Size(0xc4, 0x8b);
            this.listViewPlugins.TabIndex = 15;
            this.listViewPlugins.UseCompatibleStateImageBehavior = false;
            this.listViewPlugins.View = View.Details;
            this.listViewPlugins.SelectedIndexChanged += new EventHandler(this.listViewPlugins_SelectedIndexChanged);
            this.listViewPlugins.DoubleClick += new EventHandler(this.listViewPlugins_DoubleClick);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x252, 0x134);
            base.Controls.Add(this.listViewPlugins);
            base.Controls.Add(this.buttonInput);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.pictureBoxMinus);
            base.Controls.Add(this.pictureBoxPlus);
            base.Controls.Add(this.buttonRemove);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.checkedListBoxSequencePlugins);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.buttonPluginSetup);
            base.Controls.Add(this.textBoxChannelTo);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.textBoxChannelFrom);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.buttonUse);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "PluginListDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Sequence Plugin Mapping";
            base.Load += new EventHandler(this.PluginListDialog_Load);
            base.FormClosing += new FormClosingEventHandler(this.PluginListDialog_FormClosing);
            this.groupBox1.ResumeLayout(false);
            ((ISupportInitialize) this.pictureBoxPlus).EndInit();
            ((ISupportInitialize) this.pictureBoxMinus).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void InitializePlugin(IHardwarePlugin plugin, XmlNode setupNode)
        {
            if (plugin is IEventDrivenOutputPlugIn)
            {
                ((IEventDrivenOutputPlugIn) plugin).Initialize(this.m_executableObject, this.m_setupData, setupNode);
            }
            else if (plugin is IEventlessOutputPlugIn)
            {
                ((IEventlessOutputPlugIn) plugin).Initialize(this.m_executableObject, this.m_setupData, setupNode, null);
            }
            else if (plugin is IInputPlugin)
            {
                ((InputPlugin) plugin).InitializeInternal(this.m_setupData, setupNode);
            }
        }

        private void listBoxAllPlugins_DoubleClick(object sender, EventArgs e)
        {
        }

        private void listBoxAllPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void listBoxSequencePlugins_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                this.RemoveSelectedPlugIn();
            }
        }

        private void listBoxSequencePlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((this.m_lastIndex != -1) && (this.checkedListBoxSequencePlugins.SelectedIndex != -1))
            {
                this.UpdatePlugInNodeChannelRanges(this.m_lastIndex.ToString());
            }
            int selectedIndex = this.checkedListBoxSequencePlugins.SelectedIndex;
            this.buttonPluginSetup.Enabled = selectedIndex != -1;
            this.buttonRemove.Enabled = selectedIndex != -1;
            if (selectedIndex != -1)
            {
                XmlNode plugInData = this.m_setupData.GetPlugInData(selectedIndex.ToString());
                this.textBoxChannelFrom.Text = plugInData.Attributes["from"].Value;
                this.textBoxChannelTo.Text = plugInData.Attributes["to"].Value;
            }
            this.buttonInput.Enabled = ((this.checkedListBoxSequencePlugins.SelectedIndex != -1) && (this.m_executableObject is EventSequence)) && (this.m_sequencePlugins[this.checkedListBoxSequencePlugins.SelectedIndex] is IInputPlugin);
            this.m_lastIndex = selectedIndex;
        }

        private void listViewOutputPorts_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = false;
        }

        private void listViewOutputPorts_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if ((e.ColumnIndex == 2) && (e.Item.Tag != null))
            {
                OutputPort tag = (OutputPort) e.Item.Tag;
                if (tag.ReferencingPlugins.Count > 1)
                {
                    Image image = tag.IsExpanded ? this.pictureBoxMinus.Image : this.pictureBoxPlus.Image;
                    Point point = new Point(e.Bounds.Location.X, e.Bounds.Location.Y);
                    point.Offset(tag.IsExpanded ? this.m_expandedRelativeBounds.Location : this.m_collapsedRelativeBounds.Location);
                    e.Graphics.DrawImage(image, point);
                }
            }
            else if (e.ColumnIndex != 0)
            {
                e.DrawDefault = true;
            }
        }

        private void listViewOutputPorts_MouseDown(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = this.listViewOutputPorts.HitTest(e.Location);
            if ((info.Item != null) && (info.Item.Tag != null))
            {
                OutputPort tag = (OutputPort) info.Item.Tag;
                if ((tag.ReferencingPlugins.Count > 1) && (info.Item.SubItems.IndexOf(info.SubItem) == 2))
                {
                    Point pt = new Point(e.Location.X, e.Location.Y);
                    pt.Offset(-info.SubItem.Bounds.Location.X, -info.SubItem.Bounds.Location.Y);
                    this.m_itemAffectedIndex = info.Item.Index;
                    if (tag.IsExpanded)
                    {
                        if (this.m_expandedRelativeBounds.Contains(pt))
                        {
                            tag.IsExpanded = false;
                            this.UpdateConfigDisplay();
                        }
                    }
                    else if (this.m_collapsedRelativeBounds.Contains(pt))
                    {
                        tag.IsExpanded = true;
                        this.UpdateConfigDisplay();
                    }
                }
            }
        }

        private void listViewPlugins_DoubleClick(object sender, EventArgs e)
        {
            if (this.listViewPlugins.SelectedItems.Count > 0)
            {
                this.UsePlugin();
            }
        }

        private void listViewPlugins_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.buttonUse.Enabled = this.listViewPlugins.SelectedItems.Count > 0;
        }

        private void PluginListDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.listBoxSequencePlugins_SelectedIndexChanged(null, null);
        }

        private void PluginListDialog_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                this.m_internalUpdate = true;
                foreach (XmlNode node in this.m_setupData.GetAllPluginData())
                {
                    IHardwarePlugin plugin;
                    if ((node.Attributes["type"] != null) && (node.Attributes["type"].Value == SetupData.PluginType.Input.ToString()))
                    {
                        plugin = InputPlugins.FindPlugin(node.Attributes["name"].Value, true);
                    }
                    else
                    {
                        plugin = OutputPlugins.FindPlugin(node.Attributes["name"].Value, true);
                    }
                    if (plugin != null)
                    {
                        this.InitializePlugin(plugin, node);
                        this.checkedListBoxSequencePlugins.Items.Add(plugin.Name, bool.Parse(node.Attributes["enabled"].Value));
                        this.m_sequencePlugins.Add(plugin);
                    }
                }
                this.m_internalUpdate = false;
                this.UpdateDictionary();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void PluginSetup()
        {
            if (this.checkedListBoxSequencePlugins.SelectedItem != null)
            {
                this.UpdatePlugInNodeChannelRanges(this.checkedListBoxSequencePlugins.SelectedIndex.ToString());
                try
                {
                    this.m_sequencePlugins[this.checkedListBoxSequencePlugins.SelectedIndex].Setup();
                    this.UpdateDictionary();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("An exception occurred when trying to initialize the plugin for setup.\n\nError:\n" + exception.Message, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void RemoveSelectedPlugIn()
        {
            if ((this.m_sequencePlugins[this.checkedListBoxSequencePlugins.SelectedIndex] != null) && (this.checkedListBoxSequencePlugins.SelectedIndex != -1))
            {
                this.m_setupData.RemovePlugInData(this.checkedListBoxSequencePlugins.SelectedIndex.ToString());
                this.m_sequencePlugins.RemoveAt(this.checkedListBoxSequencePlugins.SelectedIndex);
                this.checkedListBoxSequencePlugins.Items.RemoveAt(this.checkedListBoxSequencePlugins.SelectedIndex);
                this.buttonRemove.Enabled = this.checkedListBoxSequencePlugins.SelectedIndex != -1;
                this.UpdateDictionary();
            }
        }

        private void UpdateConfigDisplay()
        {
            this.listViewOutputPorts.BeginUpdate();
            this.listViewOutputPorts.Items.Clear();
            List<int> list = new List<int>();
            foreach (string str in this.m_outputPorts.Keys)
            {
                ListViewGroup group = this.listViewOutputPorts.Groups.Add(str, str);
                Dictionary<int, OutputPort> dictionary = this.m_outputPorts[str];
                list.Clear();
                list.AddRange(dictionary.Keys);
                list.Sort();
                foreach (int num in list)
                {
                    ListViewItem item;
                    OutputPort port = dictionary[num];
                    if (port.ReferencingPlugins.Count == 1)
                    {
                        item = new ListViewItem(new string[] { string.Empty, port.Index.ToString(port.StringFormat), string.Empty, port.ReferencingPlugins[0].Name }, group);
                    }
                    else if (port.IsExpanded)
                    {
                        item = new ListViewItem(new string[] { string.Empty, port.Index.ToString(port.StringFormat), string.Empty, "Multiple" }, group);
                        if (port.Shared)
                        {
                            item.SubItems[3].ForeColor = Color.LightSteelBlue;
                        }
                        else
                        {
                            item.SubItems[3].ForeColor = Color.Pink;
                        }
                    }
                    else
                    {
                        item = new ListViewItem(new string[] { string.Empty, port.Index.ToString(port.StringFormat), string.Empty, "Multiple" }, group);
                        if (port.Shared)
                        {
                            item.SubItems[3].ForeColor = Color.SteelBlue;
                        }
                        else
                        {
                            item.SubItems[3].ForeColor = Color.Red;
                        }
                    }
                    item.Tag = port;
                    this.listViewOutputPorts.Items.Add(item);
                    if (port.IsExpanded)
                    {
                        foreach (IPlugIn @in in port.ReferencingPlugins)
                        {
                            this.listViewOutputPorts.Items.Add(new ListViewItem(new string[] { string.Empty, string.Empty, string.Empty, @in.Name }, group));
                        }
                    }
                }
            }
            this.listViewOutputPorts.EndUpdate();
            if (this.listViewOutputPorts.Items.Count > 0)
            {
                this.listViewOutputPorts.EnsureVisible(this.listViewOutputPorts.Items.Count - 1);
                this.listViewOutputPorts.EnsureVisible(this.m_itemAffectedIndex);
            }
        }

        private void UpdateDictionary()
        {
            if (!this.m_internalUpdate)
            {
                this.m_outputPorts.Clear();
                int num = 0;
                foreach (IHardwarePlugin plugin in this.m_sequencePlugins)
                {
                    if (bool.Parse(this.m_setupData.GetPlugInData(num.ToString()).Attributes["enabled"].Value))
                    {
                        foreach (HardwareMap map in plugin.HardwareMap)
                        {
                            Dictionary<int, OutputPort> dictionary;
                            OutputPort port;
                            string key = map.PortTypeName.ToLower().Trim();
                            key = char.ToUpper(key[0]) + key.Substring(1);
                            if (!this.m_outputPorts.TryGetValue(key, out dictionary))
                            {
                                this.m_outputPorts[key] = dictionary = new Dictionary<int, OutputPort>();
                            }
                            if (!dictionary.TryGetValue(map.PortTypeIndex, out port))
                            {
                                dictionary[map.PortTypeIndex] = port = new OutputPort(key, map.PortTypeIndex, map.Shared, map.StringFormat);
                            }
                            else
                            {
                                port.Shared |= map.Shared;
                            }
                            port.ReferencingPlugins.Add(plugin);
                        }
                    }
                    num++;
                }
                this.m_itemAffectedIndex = 0;
                this.UpdateConfigDisplay();
            }
        }

        private void UpdatePlugInNodeChannelRanges(string pluginID)
        {
            int count;
            XmlNode plugInData = this.m_setupData.GetPlugInData(pluginID);
            try
            {
                count = Convert.ToInt32(this.textBoxChannelFrom.Text);
            }
            catch
            {
                count = 1;
            }
            plugInData.Attributes["from"].Value = count.ToString();
            try
            {
                count = Convert.ToInt32(this.textBoxChannelTo.Text);
            }
            catch
            {
                count = this.m_channels.Count;
            }
            plugInData.Attributes["to"].Value = count.ToString();
        }

        private void UsePlugin()
        {
            if (this.listViewPlugins.SelectedItems.Count != 0)
            {
                IHardwarePlugin plugIn = OutputPlugins.FindPlugin(((IHardwarePlugin) this.listViewPlugins.SelectedItems[0].Tag).Name, true);
                XmlNode node = this.m_setupData.CreatePlugInData(plugIn);
                Xml.SetAttribute(node, "from", "1");
                Xml.SetAttribute(node, "to", this.m_channels.Count.ToString());
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    this.InitializePlugin(plugIn, node);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(string.Format("Error during plugin initialization:\n\n{0}\n\nThe plugin's setup data may be invalid or inaccurate.", exception.Message), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
                this.checkedListBoxSequencePlugins.Items.Add(plugIn.Name, true);
                this.m_sequencePlugins.Add(plugIn);
            }
        }

        public object[] MappedPluginList
        {
            get
            {
                List<object[]> list = new List<object[]>();
                foreach (XmlNode node in this.m_setupData.GetAllPluginData())
                {
                    list.Add(new object[] { string.Format("{0} ({1}-{2})", node.Attributes["name"].Value, node.Attributes["from"].Value, node.Attributes["to"].Value), bool.Parse(node.Attributes["enabled"].Value), Convert.ToInt32(node.Attributes["id"].Value) });
                }
                return list.ToArray();
            }
        }
    }
}

