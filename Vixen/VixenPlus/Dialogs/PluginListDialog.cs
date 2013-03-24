using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace Vixen.Dialogs
{
	public partial class PluginListDialog : Form
	{
		private readonly List<Channel> m_channels;
		private readonly IExecutable m_executableObject;
		private readonly Dictionary<string, Dictionary<int, OutputPort>> m_outputPorts;
		private readonly List<IHardwarePlugin> m_sequencePlugins;
		private readonly SetupData m_setupData;
		private Rectangle m_collapsedRelativeBounds;
		private Rectangle m_expandedRelativeBounds;
		private bool m_internalUpdate;
		private int m_itemAffectedIndex;
		private int m_lastIndex = -1;

		public PluginListDialog(IExecutable executableObject)
		{
			m_setupData = executableObject.PlugInData;
			m_executableObject = executableObject;
			m_channels = executableObject.Channels;
			InitializeComponent();
			m_sequencePlugins = new List<IHardwarePlugin>();
			m_outputPorts = new Dictionary<string, Dictionary<int, OutputPort>>();
			Cursor = Cursors.WaitCursor;
			try
			{
				ListViewItem item;
				listViewPlugins.Columns[0].Width = listViewPlugins.Width - 0x19;
				ListViewGroup group = listViewPlugins.Groups["listViewGroupOutput"];
				ListViewGroup group2 = listViewPlugins.Groups["listViewGroupInput"];
				List<IHardwarePlugin> list = OutputPlugins.LoadPlugins();
				if (list != null)
				{
					foreach (IHardwarePlugin plugin in list)
					{
						item = new ListViewItem(plugin.Name, group);
						item.Tag = plugin;
						listViewPlugins.Items.Add(item);
					}
				}
				list = InputPlugins.LoadPlugins();
				if (list != null)
				{
					foreach (IHardwarePlugin plugin in list)
					{
						item = new ListViewItem(plugin.Name, group2);
						item.Tag = plugin;
						listViewPlugins.Items.Add(item);
					}
				}
				listViewPlugins.Enabled = listViewPlugins.Items.Count > 0;
				OutputPlugins.VerifyPlugIns(m_executableObject);
				InputPlugins.VerifyPlugIns(m_executableObject);
				m_collapsedRelativeBounds = new Rectangle(listViewOutputPorts.Columns[2].Width - (pictureBoxPlus.Width*2),
				                                          (14 - pictureBoxPlus.Height)/2, pictureBoxPlus.Width,
				                                          pictureBoxPlus.Height);
				m_expandedRelativeBounds = new Rectangle(listViewOutputPorts.Columns[2].Width - (pictureBoxMinus.Width*2),
				                                         (14 - pictureBoxMinus.Height)/2, pictureBoxMinus.Width,
				                                         pictureBoxMinus.Height);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		public object[] MappedPluginList
		{
			get
			{
				var list = new List<object[]>();
				foreach (XmlNode node in m_setupData.GetAllPluginData())
				{
					list.Add(new object[]
						{
							string.Format("{0} ({1}-{2})", node.Attributes["name"].Value, node.Attributes["from"].Value,
							              node.Attributes["to"].Value),
							bool.Parse(node.Attributes["enabled"].Value),
							Convert.ToInt32(node.Attributes["id"].Value)
						});
				}
				return list.ToArray();
			}
		}

		private void buttonInput_Click(object sender, EventArgs e)
		{
			var plugin = (InputPlugin) m_sequencePlugins[checkedListBoxSequencePlugins.SelectedIndex];
			InitializePlugin(plugin, m_setupData.GetPlugInData(checkedListBoxSequencePlugins.SelectedIndex.ToString()));
			var dialog = new InputPluginDialog(plugin, (EventSequence) m_executableObject);
			dialog.ShowDialog();
			dialog.Dispose();
		}

		private void buttonPluginSetup_Click(object sender, EventArgs e)
		{
			PluginSetup();
		}

		private void buttonRemove_Click(object sender, EventArgs e)
		{
			RemoveSelectedPlugIn();
		}

		private void buttonUse_Click(object sender, EventArgs e)
		{
			UsePlugin();
		}

		private void checkedListBoxSequencePlugins_DoubleClick(object sender, EventArgs e)
		{
			if (checkedListBoxSequencePlugins.SelectedIndex != -1)
			{
				PluginSetup();
			}
		}

		private void checkedListBoxSequencePlugins_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			if (e.Index != -1)
			{
				m_setupData.GetPlugInData(e.Index.ToString()).Attributes["enabled"].Value =
					(e.NewValue == CheckState.Checked).ToString();
				UpdateDictionary();
			}
		}


		//ComponentResourceManager manager = new ComponentResourceManager(typeof(PluginListDialog));
		//this.pictureBoxPlus.Image = (Image)manager.GetObject("pictureBoxPlus.Image");
		//this.pictureBoxMinus.Image = (Image)manager.GetObject("pictureBoxMinus.Image");


		private void InitializePlugin(IHardwarePlugin plugin, XmlNode setupNode)
		{
			if (plugin is IEventDrivenOutputPlugIn)
			{
				((IEventDrivenOutputPlugIn) plugin).Initialize(m_executableObject, m_setupData, setupNode);
			}
			else if (plugin is IEventlessOutputPlugIn)
			{
				((IEventlessOutputPlugIn) plugin).Initialize(m_executableObject, m_setupData, setupNode, null);
			}
			else if (plugin is IInputPlugin)
			{
				((InputPlugin) plugin).InitializeInternal(m_setupData, setupNode);
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
				RemoveSelectedPlugIn();
			}
		}

		private void listBoxSequencePlugins_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ((m_lastIndex != -1) && (checkedListBoxSequencePlugins.SelectedIndex != -1))
			{
				UpdatePlugInNodeChannelRanges(m_lastIndex.ToString());
			}
			int selectedIndex = checkedListBoxSequencePlugins.SelectedIndex;
			buttonPluginSetup.Enabled = selectedIndex != -1;
			buttonRemove.Enabled = selectedIndex != -1;
			if (selectedIndex != -1)
			{
				XmlNode plugInData = m_setupData.GetPlugInData(selectedIndex.ToString());
				textBoxChannelFrom.Text = plugInData.Attributes["from"].Value;
				textBoxChannelTo.Text = plugInData.Attributes["to"].Value;
			}
			buttonInput.Enabled = ((checkedListBoxSequencePlugins.SelectedIndex != -1) && (m_executableObject is EventSequence)) &&
			                      (m_sequencePlugins[checkedListBoxSequencePlugins.SelectedIndex] is IInputPlugin);
			m_lastIndex = selectedIndex;
		}

		private void listViewOutputPorts_DrawItem(object sender, DrawListViewItemEventArgs e)
		{
			e.DrawDefault = false;
		}

		private void listViewOutputPorts_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
		{
			if ((e.ColumnIndex == 2) && (e.Item.Tag != null))
			{
				var tag = (OutputPort) e.Item.Tag;
				if (tag.ReferencingPlugins.Count > 1)
				{
					Image image = tag.IsExpanded ? pictureBoxMinus.Image : pictureBoxPlus.Image;
					var point = new Point(e.Bounds.Location.X, e.Bounds.Location.Y);
					point.Offset(tag.IsExpanded ? m_expandedRelativeBounds.Location : m_collapsedRelativeBounds.Location);
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
			ListViewHitTestInfo info = listViewOutputPorts.HitTest(e.Location);
			if ((info.Item != null) && (info.Item.Tag != null))
			{
				var tag = (OutputPort) info.Item.Tag;
				if ((tag.ReferencingPlugins.Count > 1) && (info.Item.SubItems.IndexOf(info.SubItem) == 2))
				{
					var pt = new Point(e.Location.X, e.Location.Y);
					pt.Offset(-info.SubItem.Bounds.Location.X, -info.SubItem.Bounds.Location.Y);
					m_itemAffectedIndex = info.Item.Index;
					if (tag.IsExpanded)
					{
						if (m_expandedRelativeBounds.Contains(pt))
						{
							tag.IsExpanded = false;
							UpdateConfigDisplay();
						}
					}
					else if (m_collapsedRelativeBounds.Contains(pt))
					{
						tag.IsExpanded = true;
						UpdateConfigDisplay();
					}
				}
			}
		}

		private void listViewPlugins_DoubleClick(object sender, EventArgs e)
		{
			if (listViewPlugins.SelectedItems.Count > 0)
			{
				UsePlugin();
			}
		}

		private void listViewPlugins_SelectedIndexChanged(object sender, EventArgs e)
		{
			buttonUse.Enabled = listViewPlugins.SelectedItems.Count > 0;
		}

		private void PluginListDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			listBoxSequencePlugins_SelectedIndexChanged(null, null);
		}

		private void PluginListDialog_Load(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
			try
			{
				m_internalUpdate = true;
				foreach (XmlNode node in m_setupData.GetAllPluginData())
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
						InitializePlugin(plugin, node);
						checkedListBoxSequencePlugins.Items.Add(plugin.Name, bool.Parse(node.Attributes["enabled"].Value));
						m_sequencePlugins.Add(plugin);
					}
				}
				m_internalUpdate = false;
				UpdateDictionary();
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		private void PluginSetup()
		{
			if (checkedListBoxSequencePlugins.SelectedItem != null)
			{
				UpdatePlugInNodeChannelRanges(checkedListBoxSequencePlugins.SelectedIndex.ToString());
				try
				{
					m_sequencePlugins[checkedListBoxSequencePlugins.SelectedIndex].Setup();
					UpdateDictionary();
				}
				catch (Exception exception)
				{
					MessageBox.Show(
						"An exception occurred when trying to initialize the plugin for setup.\n\nError:\n" + exception.Message,
						Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
		}

		private void RemoveSelectedPlugIn()
		{
			if ((m_sequencePlugins[checkedListBoxSequencePlugins.SelectedIndex] != null) &&
			    (checkedListBoxSequencePlugins.SelectedIndex != -1))
			{
				m_setupData.RemovePlugInData(checkedListBoxSequencePlugins.SelectedIndex.ToString());
				m_sequencePlugins.RemoveAt(checkedListBoxSequencePlugins.SelectedIndex);
				checkedListBoxSequencePlugins.Items.RemoveAt(checkedListBoxSequencePlugins.SelectedIndex);
				buttonRemove.Enabled = checkedListBoxSequencePlugins.SelectedIndex != -1;
				UpdateDictionary();
			}
		}

		private void UpdateConfigDisplay()
		{
			listViewOutputPorts.BeginUpdate();
			listViewOutputPorts.Items.Clear();
			var list = new List<int>();
			foreach (string str in m_outputPorts.Keys)
			{
				ListViewGroup group = listViewOutputPorts.Groups.Add(str, str);
				Dictionary<int, OutputPort> dictionary = m_outputPorts[str];
				list.Clear();
				list.AddRange(dictionary.Keys);
				list.Sort();
				foreach (int num in list)
				{
					ListViewItem item;
					OutputPort port = dictionary[num];
					if (port.ReferencingPlugins.Count == 1)
					{
						item =
							new ListViewItem(
								new[] {string.Empty, port.Index.ToString(port.StringFormat), string.Empty, port.ReferencingPlugins[0].Name},
								group);
					}
					else if (port.IsExpanded)
					{
						item = new ListViewItem(new[] {string.Empty, port.Index.ToString(port.StringFormat), string.Empty, "Multiple"},
						                        group);
						item.SubItems[3].ForeColor = port.Shared ? Color.LightSteelBlue : Color.Pink;
					}
					else
					{
						item = new ListViewItem(new[] {string.Empty, port.Index.ToString(port.StringFormat), string.Empty, "Multiple"},
						                        group);
						item.SubItems[3].ForeColor = port.Shared ? Color.SteelBlue : Color.Red;
					}
					item.Tag = port;
					listViewOutputPorts.Items.Add(item);
					if (port.IsExpanded)
					{
						foreach (IPlugIn @in in port.ReferencingPlugins)
						{
							listViewOutputPorts.Items.Add(new ListViewItem(new[] {string.Empty, string.Empty, string.Empty, @in.Name}, group));
						}
					}
				}
			}
			listViewOutputPorts.EndUpdate();
			if (listViewOutputPorts.Items.Count > 0)
			{
				listViewOutputPorts.EnsureVisible(listViewOutputPorts.Items.Count - 1);
				listViewOutputPorts.EnsureVisible(m_itemAffectedIndex);
			}
		}

		private void UpdateDictionary()
		{
			if (!m_internalUpdate)
			{
				m_outputPorts.Clear();
				int num = 0;
				foreach (IHardwarePlugin plugin in m_sequencePlugins)
				{
					if (bool.Parse(m_setupData.GetPlugInData(num.ToString()).Attributes["enabled"].Value))
					{
						foreach (HardwareMap map in plugin.HardwareMap)
						{
							Dictionary<int, OutputPort> dictionary;
							OutputPort port;
							string key = map.PortTypeName.ToLower().Trim();
							key = char.ToUpper(key[0]) + key.Substring(1);
							if (!m_outputPorts.TryGetValue(key, out dictionary))
							{
								m_outputPorts[key] = dictionary = new Dictionary<int, OutputPort>();
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
				m_itemAffectedIndex = 0;
				UpdateConfigDisplay();
			}
		}

		private void UpdatePlugInNodeChannelRanges(string pluginID)
		{
			int count;
			XmlNode plugInData = m_setupData.GetPlugInData(pluginID);
			try
			{
				count = Convert.ToInt32(textBoxChannelFrom.Text);
			}
			catch
			{
				count = 1;
			}
			plugInData.Attributes["from"].Value = count.ToString();
			try
			{
				count = Convert.ToInt32(textBoxChannelTo.Text);
			}
			catch
			{
				count = m_channels.Count;
			}
			plugInData.Attributes["to"].Value = count.ToString();
		}

		private void UsePlugin()
		{
			if (listViewPlugins.SelectedItems.Count != 0)
			{
				IHardwarePlugin plugIn = OutputPlugins.FindPlugin(((IHardwarePlugin) listViewPlugins.SelectedItems[0].Tag).Name,
				                                                  true);
				XmlNode node = m_setupData.CreatePlugInData(plugIn);
				Xml.SetAttribute(node, "from", "1");
				Xml.SetAttribute(node, "to", m_channels.Count.ToString());
				Cursor = Cursors.WaitCursor;
				try
				{
					InitializePlugin(plugIn, node);
				}
				catch (Exception exception)
				{
					MessageBox.Show(
						string.Format(
							"Error during plugin initialization:\n\n{0}\n\nThe plugin's setup data may be invalid or inaccurate.",
							exception.Message), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				finally
				{
					Cursor = Cursors.Default;
				}
				checkedListBoxSequencePlugins.Items.Add(plugIn.Name, true);
				m_sequencePlugins.Add(plugIn);
			}
		}
	}
}