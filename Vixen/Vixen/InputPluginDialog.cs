namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal partial class InputPluginDialog : Form
    {
        //TODO: Figure out what the code below does. It was translated to the designer, but doesn't seem to work there.
        //ComponentResourceManager manager = new ComponentResourceManager(typeof(InputPluginDialog));
        //this.imageList.ImageStream = (ImageListStreamer)manager.GetObject("imageList.ImageStream"); 

        private MappingSets m_editingMappingSets;
        private Dictionary<string, Channel> m_idChannel;
        private bool m_init = false;
        private bool m_internal = false;
        private InputPlugin m_plugin;
        private EventSequence m_sequence;
 
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
                base.DialogResult = System.Windows.Forms.DialogResult.None;
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

