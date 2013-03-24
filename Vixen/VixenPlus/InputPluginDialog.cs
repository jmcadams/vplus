using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Vixen
{
	internal partial class InputPluginDialog : Form
	{
		//TODO: Figure out what the code below does. It was translated to the designer, but doesn't seem to work there.
		//ComponentResourceManager manager = new ComponentResourceManager(typeof(InputPluginDialog));
		//this.imageList.ImageStream = (ImageListStreamer)manager.GetObject("imageList.ImageStream"); 

		private readonly MappingSets m_editingMappingSets;
		private readonly Dictionary<string, Channel> m_idChannel;
		private readonly bool m_init;
		private readonly InputPlugin m_plugin;
		private readonly EventSequence m_sequence;
		private bool m_internal;

		public InputPluginDialog(InputPlugin plugin, EventSequence sequence)
		{
			InitializeComponent();
			m_idChannel = new Dictionary<string, Channel>();
			m_plugin = plugin;
			m_sequence = sequence;
			plugin.SetupDataToPlugin();
			m_init = true;
			listBoxInputs.DisplayMember = "Name";
			listBoxInputs.ValueMember = "OutputChannelId";
			listBoxInputs.DataSource = m_plugin.Inputs;
			foreach (Channel channel in m_sequence.Channels)
			{
				m_idChannel[channel.Id.ToString()] = channel;
			}
			listBoxChannels.Items.AddRange(m_sequence.Channels.ToArray());
			m_editingMappingSets = (MappingSets) m_plugin.MappingSets.Clone();
			m_init = false;
			if (listBoxInputs.SelectedItem != null)
			{
				ReflectInput((Input) listBoxInputs.SelectedItem);
			}
			checkBoxLiveUpdate.Checked = m_plugin.LiveUpdate;
			checkBoxRecord.Checked = m_plugin.Record;
			foreach (MappingSet set in m_editingMappingSets)
			{
				AddMappingSetListViewItem(set);
			}
			Input[] iterators = m_plugin.GetIterators();
			comboBoxSingleIteratorInput.Items.AddRange(iterators);
			listBoxIteratorInputs.Items.AddRange(iterators);
			if (m_plugin.MappingIteratorType == InputPlugin.MappingIterator.SingleInput)
			{
				radioButtonSingleIterator.Checked = true;
				comboBoxSingleIteratorInput.SelectedItem = m_plugin.SingleIterator;
			}
			else if (m_plugin.MappingIteratorType == InputPlugin.MappingIterator.MultiInput)
			{
				radioButtonMultipleIterators.Checked = true;
				tabControlIterators.SelectedTab = tabPageMultipleIterators;
			}
			else
			{
				radioButtonNoIterator.Checked = true;
			}
		}

		private void AddMappingSetListViewItem(MappingSet mappingSet)
		{
			listViewMappingSets.Items.Add(mappingSet.Name);
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
			AddMappingSetListViewItem(m_editingMappingSets.AddMapping());
		}

		private void buttonClearInputChannels_Click(object sender, EventArgs e)
		{
			if (listBoxInputs.SelectedItem != null)
			{
				var selectedItem = (Input) listBoxInputs.SelectedItem;
				m_editingMappingSets.GetOutputChannelIdList(selectedItem).Clear();
				ReflectInput(selectedItem);
			}
		}

		private void buttonMoveDown_Click(object sender, EventArgs e)
		{
			int oldIndex = listViewMappingSets.SelectedIndices[0];
			m_editingMappingSets.MoveMappingTo(oldIndex, oldIndex + 1);
			ListViewItem item = listViewMappingSets.Items[oldIndex];
			listViewMappingSets.Items.RemoveAt(oldIndex);
			listViewMappingSets.Items.Insert(oldIndex + 1, item);
			listViewMappingSets.Items[oldIndex].Selected = false;
			listViewMappingSets.Items[oldIndex + 1].Selected = true;
		}

		private void buttonMoveUp_Click(object sender, EventArgs e)
		{
			int oldIndex = listViewMappingSets.SelectedIndices[0];
			m_editingMappingSets.MoveMappingTo(oldIndex, oldIndex - 1);
			ListViewItem item = listViewMappingSets.Items[oldIndex];
			listViewMappingSets.Items.RemoveAt(oldIndex);
			listViewMappingSets.Items.Insert(oldIndex - 1, item);
			listViewMappingSets.Items[oldIndex].Selected = false;
			listViewMappingSets.Items[oldIndex - 1].Selected = true;
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (radioButtonSingleIterator.Checked && (comboBoxSingleIteratorInput.SelectedItem == null))
			{
				if (m_plugin.GetIterators().Length > 0)
				{
					MessageBox.Show(
						"You have selected to use a single input to iterate the mapping sets, but have not chosen an input.",
						Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
					tabControlPlugin.SelectedTab = tabPageMappingIteration;
				}
				else
				{
					MessageBox.Show(
						"You have selected to use a single input to iterate the mapping sets, but do not have any inputs sets to be iterators and therefore have not chosen an input to iterate with.",
						Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				base.DialogResult = DialogResult.None;
			}
			else
			{
				m_plugin.MappingSets = m_editingMappingSets;
				m_plugin.LiveUpdate = checkBoxLiveUpdate.Checked;
				m_plugin.Record = checkBoxRecord.Checked;
				if (radioButtonSingleIterator.Checked)
				{
					m_plugin.MappingIteratorType = InputPlugin.MappingIterator.SingleInput;
				}
				else if (radioButtonMultipleIterators.Checked)
				{
					m_plugin.MappingIteratorType = InputPlugin.MappingIterator.MultiInput;
				}
				else
				{
					m_plugin.MappingIteratorType = InputPlugin.MappingIterator.None;
				}
				if (m_plugin.MappingIteratorType == InputPlugin.MappingIterator.SingleInput)
				{
					m_plugin.SingleIterator = (Input) comboBoxSingleIteratorInput.SelectedItem;
				}
				else
				{
					m_plugin.SingleIterator = null;
				}
				m_plugin.PluginToSetupData();
			}
		}

		private void buttonRemoveMappingSet_Click(object sender, EventArgs e)
		{
			m_editingMappingSets.RemoveMappingAt(listViewMappingSets.SelectedIndices[0]);
			listViewMappingSets.Items.RemoveAt(listViewMappingSets.SelectedIndices[0]);
			buttonRemoveMappingSet.Enabled = buttonMoveUp.Enabled = buttonMoveDown.Enabled = false;
			UpdateIteratorType();
		}

		private void buttonRenameMappingSet_Click(object sender, EventArgs e)
		{
		}

		private void checkBoxEnabled_CheckedChanged(object sender, EventArgs e)
		{
			checkBoxEnabled.Text = checkBoxEnabled.Checked ? "Input is enabled and is mapped to:" : "Input is disabled";
			listBoxChannels.Enabled = checkBoxEnabled.Checked;
		}

		private void checkBoxEnabled_Click(object sender, EventArgs e)
		{
			if (listBoxInputs.SelectedItem != null)
			{
				((Input) listBoxInputs.SelectedItem).Enabled = checkBoxEnabled.Checked;
			}
		}

		private void comboBoxMappingSet_SelectedIndexChanged(object sender, EventArgs e)
		{
			m_editingMappingSets.CurrentMappingSet = (MappingSet) comboBoxMappingSet.SelectedItem;
			listBoxInputs.SelectedIndex = -1;
			listBoxMappedChannels.Items.Clear();
			listBoxChannels.ClearSelected();
			groupBoxIOMapping.Enabled = comboBoxMappingSet.SelectedIndex != -1;
		}


		private void listBoxChannels_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!m_internal)
			{
				var list = new List<Channel>();
				foreach (Channel channel in listBoxMappedChannels.Items)
				{
					list.Add(channel);
				}
				foreach (Channel channel in listBoxMappedChannels.Items)
				{
					if (!listBoxChannels.SelectedItems.Contains(channel))
					{
						list.Remove(channel);
					}
				}
				foreach (Channel channel in listBoxChannels.SelectedItems)
				{
					if (!list.Contains(channel))
					{
						list.Add(channel);
					}
				}
				if (listBoxInputs.SelectedItem != null)
				{
					List<string> outputChannelIdList = m_editingMappingSets.GetOutputChannelIdList((Input) listBoxInputs.SelectedItem);
					outputChannelIdList.Clear();
					foreach (Channel channel in list)
					{
						outputChannelIdList.Add(channel.Id.ToString());
					}
				}
				listBoxMappedChannels.BeginUpdate();
				listBoxMappedChannels.Items.Clear();
				listBoxMappedChannels.Items.AddRange(list.ToArray());
				listBoxMappedChannels.EndUpdate();
			}
		}

		private void listBoxInputs_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!m_init && (listBoxInputs.SelectedItem != null))
			{
				ReflectInput((Input) listBoxInputs.SelectedItem);
			}
		}

		private void listBoxIteratorInputs_SelectedIndexChanged(object sender, EventArgs e)
		{
			var selectedItem = (Input) listBoxIteratorInputs.SelectedItem;
			if (selectedItem.AssignedMappingSet == null)
			{
				listBoxMappingSets.SelectedIndex = 0;
			}
			else
			{
				listBoxMappingSets.SelectedItem = selectedItem.AssignedMappingSet;
			}
		}

		private void listBoxMappingSets_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!(listBoxMappingSets.SelectedItem is MappingSet))
			{
				AssignMappingSetToInput(null, (Input) listBoxIteratorInputs.SelectedItem);
			}
			else
			{
				AssignMappingSetToInput((MappingSet) listBoxMappingSets.SelectedItem, (Input) listBoxIteratorInputs.SelectedItem);
			}
		}

		private void listViewMappingSets_AfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			if ((e.Label != null) && (e.Label.Trim().Length > 0))
			{
				m_editingMappingSets[listViewMappingSets.SelectedIndices[0]].Name = e.Label;
			}
		}

		private void listViewMappingSets_SelectedIndexChanged(object sender, EventArgs e)
		{
			buttonRemoveMappingSet.Enabled = listViewMappingSets.SelectedItems.Count > 0;
			buttonMoveUp.Enabled = (listViewMappingSets.SelectedItems.Count > 0) && (listViewMappingSets.SelectedIndices[0] != 0);
			buttonMoveDown.Enabled = (listViewMappingSets.SelectedItems.Count > 0) &&
			                         (listViewMappingSets.SelectedIndices[0] < (listViewMappingSets.Items.Count - 1));
		}

		private void radioButtonMultipleIterators_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButtonMultipleIterators.Checked)
			{
				tabControlIterators.SelectedTab = tabPageMultipleIterators;
			}
		}

		private void radioButtonSingleIterator_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButtonSingleIterator.Checked)
			{
				tabControlIterators.SelectedTab = tabPageSingleIterator;
			}
		}

		private void ReflectInput(Input input)
		{
			if (groupBoxIOMapping.Enabled)
			{
				m_internal = true;
				listBoxChannels.ClearSelected();
				m_internal = false;
				listBoxMappedChannels.Items.Clear();
				if (listBoxInputs.SelectedValue != null)
				{
					listBoxChannels.BeginUpdate();
					listBoxMappedChannels.BeginUpdate();
					m_internal = true;
					foreach (string str in m_editingMappingSets.GetOutputChannelIdList(input))
					{
						Channel channel;
						if (m_idChannel.TryGetValue(str, out channel))
						{
							listBoxMappedChannels.Items.Add(channel);
							listBoxChannels.SetSelected(listBoxChannels.Items.IndexOf(channel), true);
						}
					}
					m_internal = false;
					listBoxMappedChannels.EndUpdate();
					listBoxChannels.EndUpdate();
					checkBoxEnabled.Checked = input.Enabled;
				}
			}
		}

		private void tabControlMappingSets_Selecting(object sender, TabControlCancelEventArgs e)
		{
			if (e.TabPage == tabPageInputOutputMapping)
			{
				MappingSet selectedItem = null;
				if (comboBoxMappingSet.SelectedItem != null)
				{
					selectedItem = (MappingSet) comboBoxMappingSet.SelectedItem;
				}
				comboBoxMappingSet.BeginUpdate();
				comboBoxMappingSet.Items.Clear();
				comboBoxMappingSet.Items.AddRange(m_editingMappingSets.AllSets);
				comboBoxMappingSet.EndUpdate();
				if ((selectedItem != null) && comboBoxMappingSet.Items.Contains(selectedItem))
				{
					comboBoxMappingSet.SelectedItem = selectedItem;
				}
				else if (comboBoxMappingSet.Items.Count > 0)
				{
					comboBoxMappingSet.SelectedIndex = 0;
				}
				else
				{
					groupBoxIOMapping.Enabled = false;
				}
			}
		}

		private void tabControlPlugin_Selecting(object sender, TabControlCancelEventArgs e)
		{
			if (e.TabPage == tabPageMappingIteration)
			{
				int selectedIndex = comboBoxSingleIteratorInput.SelectedIndex;
				comboBoxSingleIteratorInput.Items.Clear();
				comboBoxSingleIteratorInput.Items.AddRange(m_plugin.GetIterators());
				listBoxIteratorInputs.Items.Clear();
				listBoxIteratorInputs.Items.AddRange(m_plugin.GetIterators());
				comboBoxSingleIteratorInput.SelectedIndex = selectedIndex;
				listBoxMappingSets.Items.Clear();
				listBoxMappingSets.Items.Add("(none)");
				listBoxMappingSets.Items.AddRange(m_editingMappingSets.AllSets);
			}
		}

		private void UpdateIteratorType()
		{
			if (listViewMappingSets.Items.Count <= 1)
			{
				radioButtonNoIterator.Checked = true;
			}
			else if (radioButtonNoIterator.Checked)
			{
				radioButtonSingleIterator.Checked = true;
			}
		}
	}
}