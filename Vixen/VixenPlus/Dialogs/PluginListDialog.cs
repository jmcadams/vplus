namespace Vixen.Dialogs {
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using System.Xml;
	using Vixen;

	public partial class PluginListDialog : Form {
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

		public PluginListDialog(IExecutable executableObject) {
			this.m_setupData = executableObject.PlugInData;
			this.m_executableObject = executableObject;
			this.m_channels = executableObject.Channels;
			this.InitializeComponent();
			this.m_sequencePlugins = new List<IHardwarePlugin>();
			this.m_outputPorts = new Dictionary<string, Dictionary<int, OutputPort>>();
			this.Cursor = Cursors.WaitCursor;
			try {
				ListViewItem item;
				this.listViewPlugins.Columns[0].Width = this.listViewPlugins.Width - 0x19;
				ListViewGroup group = this.listViewPlugins.Groups["listViewGroupOutput"];
				ListViewGroup group2 = this.listViewPlugins.Groups["listViewGroupInput"];
				List<IHardwarePlugin> list = OutputPlugins.LoadPlugins();
				if (list != null) {
					foreach (IHardwarePlugin plugin in list) {
						item = new ListViewItem(plugin.Name, group);
						item.Tag = plugin;
						this.listViewPlugins.Items.Add(item);
					}
				}
				list = InputPlugins.LoadPlugins();
				if (list != null) {
					foreach (IHardwarePlugin plugin in list) {
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
			finally {
				this.Cursor = Cursors.Default;
			}
		}

		private void buttonInput_Click(object sender, EventArgs e) {
			InputPlugin plugin = (InputPlugin)this.m_sequencePlugins[this.checkedListBoxSequencePlugins.SelectedIndex];
			this.InitializePlugin(plugin, this.m_setupData.GetPlugInData(this.checkedListBoxSequencePlugins.SelectedIndex.ToString()));
			InputPluginDialog dialog = new InputPluginDialog(plugin, (EventSequence)this.m_executableObject);
			dialog.ShowDialog();
			dialog.Dispose();
		}

		private void buttonPluginSetup_Click(object sender, EventArgs e) {
			this.PluginSetup();
		}

		private void buttonRemove_Click(object sender, EventArgs e) {
			this.RemoveSelectedPlugIn();
		}

		private void buttonUse_Click(object sender, EventArgs e) {
			this.UsePlugin();
		}

		private void checkedListBoxSequencePlugins_DoubleClick(object sender, EventArgs e) {
			if (this.checkedListBoxSequencePlugins.SelectedIndex != -1) {
				this.PluginSetup();
			}
		}

		private void checkedListBoxSequencePlugins_ItemCheck(object sender, ItemCheckEventArgs e) {
			if (e.Index != -1) {
				this.m_setupData.GetPlugInData(e.Index.ToString()).Attributes["enabled"].Value = (e.NewValue == CheckState.Checked).ToString();
				this.UpdateDictionary();
			}
		}


		//ComponentResourceManager manager = new ComponentResourceManager(typeof(PluginListDialog));
		//this.pictureBoxPlus.Image = (Image)manager.GetObject("pictureBoxPlus.Image");
		//this.pictureBoxMinus.Image = (Image)manager.GetObject("pictureBoxMinus.Image");



		private void InitializePlugin(IHardwarePlugin plugin, XmlNode setupNode) {
			if (plugin is IEventDrivenOutputPlugIn) {
				((IEventDrivenOutputPlugIn)plugin).Initialize(this.m_executableObject, this.m_setupData, setupNode);
			}
			else if (plugin is IEventlessOutputPlugIn) {
				((IEventlessOutputPlugIn)plugin).Initialize(this.m_executableObject, this.m_setupData, setupNode, null);
			}
			else if (plugin is IInputPlugin) {
				((InputPlugin)plugin).InitializeInternal(this.m_setupData, setupNode);
			}
		}

		private void listBoxAllPlugins_DoubleClick(object sender, EventArgs e) {
		}

		private void listBoxAllPlugins_SelectedIndexChanged(object sender, EventArgs e) {
		}

		private void listBoxSequencePlugins_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Delete) {
				this.RemoveSelectedPlugIn();
			}
		}

		private void listBoxSequencePlugins_SelectedIndexChanged(object sender, EventArgs e) {
			if ((this.m_lastIndex != -1) && (this.checkedListBoxSequencePlugins.SelectedIndex != -1)) {
				this.UpdatePlugInNodeChannelRanges(this.m_lastIndex.ToString());
			}
			int selectedIndex = this.checkedListBoxSequencePlugins.SelectedIndex;
			this.buttonPluginSetup.Enabled = selectedIndex != -1;
			this.buttonRemove.Enabled = selectedIndex != -1;
			if (selectedIndex != -1) {
				XmlNode plugInData = this.m_setupData.GetPlugInData(selectedIndex.ToString());
				this.textBoxChannelFrom.Text = plugInData.Attributes["from"].Value;
				this.textBoxChannelTo.Text = plugInData.Attributes["to"].Value;
			}
			this.buttonInput.Enabled = ((this.checkedListBoxSequencePlugins.SelectedIndex != -1) && (this.m_executableObject is EventSequence)) && (this.m_sequencePlugins[this.checkedListBoxSequencePlugins.SelectedIndex] is IInputPlugin);
			this.m_lastIndex = selectedIndex;
		}

		private void listViewOutputPorts_DrawItem(object sender, DrawListViewItemEventArgs e) {
			e.DrawDefault = false;
		}

		private void listViewOutputPorts_DrawSubItem(object sender, DrawListViewSubItemEventArgs e) {
			if ((e.ColumnIndex == 2) && (e.Item.Tag != null)) {
				OutputPort tag = (OutputPort)e.Item.Tag;
				if (tag.ReferencingPlugins.Count > 1) {
					Image image = tag.IsExpanded ? this.pictureBoxMinus.Image : this.pictureBoxPlus.Image;
					Point point = new Point(e.Bounds.Location.X, e.Bounds.Location.Y);
					point.Offset(tag.IsExpanded ? this.m_expandedRelativeBounds.Location : this.m_collapsedRelativeBounds.Location);
					e.Graphics.DrawImage(image, point);
				}
			}
			else if (e.ColumnIndex != 0) {
				e.DrawDefault = true;
			}
		}

		private void listViewOutputPorts_MouseDown(object sender, MouseEventArgs e) {
			ListViewHitTestInfo info = this.listViewOutputPorts.HitTest(e.Location);
			if ((info.Item != null) && (info.Item.Tag != null)) {
				OutputPort tag = (OutputPort)info.Item.Tag;
				if ((tag.ReferencingPlugins.Count > 1) && (info.Item.SubItems.IndexOf(info.SubItem) == 2)) {
					Point pt = new Point(e.Location.X, e.Location.Y);
					pt.Offset(-info.SubItem.Bounds.Location.X, -info.SubItem.Bounds.Location.Y);
					this.m_itemAffectedIndex = info.Item.Index;
					if (tag.IsExpanded) {
						if (this.m_expandedRelativeBounds.Contains(pt)) {
							tag.IsExpanded = false;
							this.UpdateConfigDisplay();
						}
					}
					else if (this.m_collapsedRelativeBounds.Contains(pt)) {
						tag.IsExpanded = true;
						this.UpdateConfigDisplay();
					}
				}
			}
		}

		private void listViewPlugins_DoubleClick(object sender, EventArgs e) {
			if (this.listViewPlugins.SelectedItems.Count > 0) {
				this.UsePlugin();
			}
		}

		private void listViewPlugins_SelectedIndexChanged(object sender, EventArgs e) {
			this.buttonUse.Enabled = this.listViewPlugins.SelectedItems.Count > 0;
		}

		private void PluginListDialog_FormClosing(object sender, FormClosingEventArgs e) {
			this.listBoxSequencePlugins_SelectedIndexChanged(null, null);
		}

		private void PluginListDialog_Load(object sender, EventArgs e) {
			this.Cursor = Cursors.WaitCursor;
			try {
				this.m_internalUpdate = true;
				foreach (XmlNode node in this.m_setupData.GetAllPluginData()) {
					IHardwarePlugin plugin;
					if ((node.Attributes["type"] != null) && (node.Attributes["type"].Value == SetupData.PluginType.Input.ToString())) {
						plugin = InputPlugins.FindPlugin(node.Attributes["name"].Value, true);
					}
					else {
						plugin = OutputPlugins.FindPlugin(node.Attributes["name"].Value, true);
					}
					if (plugin != null) {
						this.InitializePlugin(plugin, node);
						this.checkedListBoxSequencePlugins.Items.Add(plugin.Name, bool.Parse(node.Attributes["enabled"].Value));
						this.m_sequencePlugins.Add(plugin);
					}
				}
				this.m_internalUpdate = false;
				this.UpdateDictionary();
			}
			finally {
				this.Cursor = Cursors.Default;
			}
		}

		private void PluginSetup() {
			if (this.checkedListBoxSequencePlugins.SelectedItem != null) {
				this.UpdatePlugInNodeChannelRanges(this.checkedListBoxSequencePlugins.SelectedIndex.ToString());
				try {
					this.m_sequencePlugins[this.checkedListBoxSequencePlugins.SelectedIndex].Setup();
					this.UpdateDictionary();
				}
				catch (Exception exception) {
					MessageBox.Show("An exception occurred when trying to initialize the plugin for setup.\n\nError:\n" + exception.Message, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
		}

		private void RemoveSelectedPlugIn() {
			if ((this.m_sequencePlugins[this.checkedListBoxSequencePlugins.SelectedIndex] != null) && (this.checkedListBoxSequencePlugins.SelectedIndex != -1)) {
				this.m_setupData.RemovePlugInData(this.checkedListBoxSequencePlugins.SelectedIndex.ToString());
				this.m_sequencePlugins.RemoveAt(this.checkedListBoxSequencePlugins.SelectedIndex);
				this.checkedListBoxSequencePlugins.Items.RemoveAt(this.checkedListBoxSequencePlugins.SelectedIndex);
				this.buttonRemove.Enabled = this.checkedListBoxSequencePlugins.SelectedIndex != -1;
				this.UpdateDictionary();
			}
		}

		private void UpdateConfigDisplay() {
			this.listViewOutputPorts.BeginUpdate();
			this.listViewOutputPorts.Items.Clear();
			List<int> list = new List<int>();
			foreach (string str in this.m_outputPorts.Keys) {
				ListViewGroup group = this.listViewOutputPorts.Groups.Add(str, str);
				Dictionary<int, OutputPort> dictionary = this.m_outputPorts[str];
				list.Clear();
				list.AddRange(dictionary.Keys);
				list.Sort();
				foreach (int num in list) {
					ListViewItem item;
					OutputPort port = dictionary[num];
					if (port.ReferencingPlugins.Count == 1) {
						item = new ListViewItem(new string[] { string.Empty, port.Index.ToString(port.StringFormat), string.Empty, port.ReferencingPlugins[0].Name }, group);
					}
					else if (port.IsExpanded) {
						item = new ListViewItem(new string[] { string.Empty, port.Index.ToString(port.StringFormat), string.Empty, "Multiple" }, group);
						if (port.Shared) {
							item.SubItems[3].ForeColor = Color.LightSteelBlue;
						}
						else {
							item.SubItems[3].ForeColor = Color.Pink;
						}
					}
					else {
						item = new ListViewItem(new string[] { string.Empty, port.Index.ToString(port.StringFormat), string.Empty, "Multiple" }, group);
						if (port.Shared) {
							item.SubItems[3].ForeColor = Color.SteelBlue;
						}
						else {
							item.SubItems[3].ForeColor = Color.Red;
						}
					}
					item.Tag = port;
					this.listViewOutputPorts.Items.Add(item);
					if (port.IsExpanded) {
						foreach (IPlugIn @in in port.ReferencingPlugins) {
							this.listViewOutputPorts.Items.Add(new ListViewItem(new string[] { string.Empty, string.Empty, string.Empty, @in.Name }, group));
						}
					}
				}
			}
			this.listViewOutputPorts.EndUpdate();
			if (this.listViewOutputPorts.Items.Count > 0) {
				this.listViewOutputPorts.EnsureVisible(this.listViewOutputPorts.Items.Count - 1);
				this.listViewOutputPorts.EnsureVisible(this.m_itemAffectedIndex);
			}
		}

		private void UpdateDictionary() {
			if (!this.m_internalUpdate) {
				this.m_outputPorts.Clear();
				int num = 0;
				foreach (IHardwarePlugin plugin in this.m_sequencePlugins) {
					if (bool.Parse(this.m_setupData.GetPlugInData(num.ToString()).Attributes["enabled"].Value)) {
						foreach (HardwareMap map in plugin.HardwareMap) {
							Dictionary<int, OutputPort> dictionary;
							OutputPort port;
							string key = map.PortTypeName.ToLower().Trim();
							key = char.ToUpper(key[0]) + key.Substring(1);
							if (!this.m_outputPorts.TryGetValue(key, out dictionary)) {
								this.m_outputPorts[key] = dictionary = new Dictionary<int, OutputPort>();
							}
							if (!dictionary.TryGetValue(map.PortTypeIndex, out port)) {
								dictionary[map.PortTypeIndex] = port = new OutputPort(key, map.PortTypeIndex, map.Shared, map.StringFormat);
							}
							else {
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

		private void UpdatePlugInNodeChannelRanges(string pluginID) {
			int count;
			XmlNode plugInData = this.m_setupData.GetPlugInData(pluginID);
			try {
				count = Convert.ToInt32(this.textBoxChannelFrom.Text);
			}
			catch {
				count = 1;
			}
			plugInData.Attributes["from"].Value = count.ToString();
			try {
				count = Convert.ToInt32(this.textBoxChannelTo.Text);
			}
			catch {
				count = this.m_channels.Count;
			}
			plugInData.Attributes["to"].Value = count.ToString();
		}

		private void UsePlugin() {
			if (this.listViewPlugins.SelectedItems.Count != 0) {
				IHardwarePlugin plugIn = OutputPlugins.FindPlugin(((IHardwarePlugin)this.listViewPlugins.SelectedItems[0].Tag).Name, true);
				XmlNode node = this.m_setupData.CreatePlugInData(plugIn);
				Xml.SetAttribute(node, "from", "1");
				Xml.SetAttribute(node, "to", this.m_channels.Count.ToString());
				this.Cursor = Cursors.WaitCursor;
				try {
					this.InitializePlugin(plugIn, node);
				}
				catch (Exception exception) {
					MessageBox.Show(string.Format("Error during plugin initialization:\n\n{0}\n\nThe plugin's setup data may be invalid or inaccurate.", exception.Message), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				finally {
					this.Cursor = Cursors.Default;
				}
				this.checkedListBoxSequencePlugins.Items.Add(plugIn.Name, true);
				this.m_sequencePlugins.Add(plugIn);
			}
		}

		public object[] MappedPluginList {
			get {
				List<object[]> list = new List<object[]>();
				foreach (XmlNode node in this.m_setupData.GetAllPluginData()) {
					list.Add(new object[] { string.Format("{0} ({1}-{2})", node.Attributes["name"].Value, node.Attributes["from"].Value, node.Attributes["to"].Value), bool.Parse(node.Attributes["enabled"].Value), Convert.ToInt32(node.Attributes["id"].Value) });
				}
				return list.ToArray();
			}
		}
	}
}

