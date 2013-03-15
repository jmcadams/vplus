namespace Vixen
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    internal partial class CopySequenceDialog : Form
    {
        private EventSequence m_destSequence = null;
        private bool m_internalUpdate = false;
        private int m_itemsShowing;
        private int m_knownTop = -1;
        private EventSequence m_sourceSequence = null;

        public CopySequenceDialog()
        {
            this.InitializeComponent();
            this.Cursor = Cursors.WaitCursor;
            try
            {
                foreach (string str in Directory.GetFiles(Paths.SequencePath, "*.vix"))
                {
                    try
                    {
                        EventSequence item = new EventSequence(str);
                        this.comboBoxSourceSequence.Items.Add(item);
                        this.comboBoxDestSequence.Items.Add(item);
                    }
                    catch
                    {
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            int num = (this.listViewMapping.Width - 0x16) >> 1;
            this.listViewMapping.Columns[0].Width = num;
            this.listViewMapping.Columns[1].Width = num;
            this.comboBoxDestChannels.Width = num;
            this.comboBoxDestChannels.Left = num + this.listViewMapping.Left;
            this.m_itemsShowing = this.listViewMapping.Height / 0x11;
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            this.m_sourceSequence = (EventSequence) this.comboBoxSourceSequence.SelectedItem;
            this.m_destSequence = (EventSequence) this.comboBoxDestSequence.SelectedItem;
            if (this.m_sourceSequence == null)
            {
                MessageBox.Show("Source program must be selected", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (this.m_destSequence == null)
            {
                MessageBox.Show("Destination program must be selected", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                this.Cursor = Cursors.WaitCursor;
                this.listViewMapping.BeginUpdate();
                this.listViewMapping.Items.Clear();
                foreach (Channel channel in this.m_sourceSequence.Channels)
                {
                    this.listViewMapping.Items.Add(channel.Name).SubItems.Add("none");
                }
                this.listViewMapping.EndUpdate();
                this.comboBoxDestChannels.Items.Clear();
                this.comboBoxDestChannels.Items.Add("none");
                this.comboBoxDestChannels.Items.AddRange(this.m_destSequence.Channels.ToArray());
                CaseInsensitiveComparer comparer = new CaseInsensitiveComparer();
                foreach (ListViewItem item in this.listViewMapping.Items)
                {
                    foreach (object obj2 in this.comboBoxDestChannels.Items)
                    {
                        if ((obj2 is Channel) && (comparer.Compare(item.Text, ((Channel) obj2).Name) == 0))
                        {
                            item.Tag = obj2;
                            item.SubItems[1].Text = ((Channel) obj2).Name;
                        }
                    }
                }
                this.Cursor = Cursors.Default;
                this.buttonOK.Enabled = true;
            }
        }

        private void buttonAutoMap_Click(object sender, EventArgs e)
        {
            if (this.CheckSequences())
            {
                int num = Math.Min(this.m_sourceSequence.ChannelCount, this.m_destSequence.ChannelCount);
                this.listViewMapping.BeginUpdate();
                for (int i = 0; i < num; i++)
                {
                    this.listViewMapping.Items[i].Tag = this.m_destSequence.Channels[i];
                    this.listViewMapping.Items[i].SubItems[1].Text = this.m_destSequence.Channels[i].Name;
                }
                this.listViewMapping.EndUpdate();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (this.checkBoxSequenceLength.Checked && (this.m_destSequence.Time < this.m_sourceSequence.Time))
                {
                    this.m_destSequence.Time = this.m_sourceSequence.Time;
                }
                int num4 = (this.m_sourceSequence.EventValues.GetLength(1) > this.m_destSequence.EventValues.GetLength(1)) ? this.m_destSequence.EventValues.GetLength(1) : this.m_sourceSequence.EventValues.GetLength(1);
                foreach (ListViewItem item in this.listViewMapping.Items)
                {
                    if (item.Tag != null)
                    {
                        int index = item.Index;
                        int num2 = this.m_destSequence.Channels.IndexOf((Channel) item.Tag);
                        if ((num2 != -1) && (num2 < this.m_destSequence.EventValues.GetLength(0)))
                        {
                            for (int i = 0; i < num4; i++)
                            {
                                this.m_destSequence.EventValues[num2, i] = this.m_sourceSequence.EventValues[index, i];
                            }
                        }
                    }
                }
                if (this.checkBoxChannelDefs.Checked)
                {
                    foreach (ListViewItem item in this.listViewMapping.Items)
                    {
                        if (item.Tag != null)
                        {
                            Channel channel = this.m_sourceSequence.Channels[item.Index];
                            Channel tag = (Channel) item.Tag;
                            tag.Color = channel.Color;
                            tag.Name = channel.Name;
                        }
                    }
                }
                this.m_destSequence.Save();
                MessageBox.Show("Channels have been copied.\nDestination sequence has been saved.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            if (this.CheckSequences())
            {
                this.listViewMapping.BeginUpdate();
                foreach (ListViewItem item in this.listViewMapping.Items)
                {
                    item.Tag = null;
                    item.SubItems[1].Text = "none";
                }
                this.comboBoxDestChannels.SelectedIndex = 0;
                this.listViewMapping.EndUpdate();
            }
        }

        private bool CheckSequences()
        {
            if ((this.m_sourceSequence == null) || (this.m_destSequence == null))
            {
                MessageBox.Show("Please select both source and destination sequences first and apply your selection.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
            }
            return true;
        }

        private void comboBoxDestChannels_Leave(object sender, EventArgs e)
        {
            this.comboBoxDestChannels.Visible = false;
        }

        private void comboBoxDestChannels_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.m_internalUpdate && (this.listViewMapping.SelectedItems.Count != 0))
            {
                ListViewItem item = this.listViewMapping.SelectedItems[0];
                if (this.comboBoxDestChannels.SelectedIndex == 0)
                {
                    item.Tag = null;
                    item.SubItems[1].Text = "none";
                }
                else
                {
                    item.Tag = this.comboBoxDestChannels.SelectedItem;
                    item.SubItems[1].Text = ((Channel) item.Tag).Name;
                }
            }
        }

        private void comboBoxSourceSequence_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.buttonOK.Enabled = false;
        }

        private void CopySequenceDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (EventSequence sequence in this.comboBoxSourceSequence.Items)
            {
                sequence.Dispose();
            }
        }

        

        private void groupBox2_Leave(object sender, EventArgs e)
        {
            this.comboBoxDestChannels.Visible = false;
        }

        

        private void listViewMapping_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listViewMapping_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
            if ((this.listViewMapping.SelectedItems.Count > 0) && (this.listViewMapping.TopItem.Index != this.m_knownTop))
            {
                int index = this.listViewMapping.SelectedItems[0].Index;
                if ((index < this.listViewMapping.TopItem.Index) || (index > (this.listViewMapping.TopItem.Index + this.m_itemsShowing)))
                {
                    this.comboBoxDestChannels.Visible = false;
                }
                else
                {
                    this.comboBoxDestChannels.Top = this.listViewMapping.SelectedItems[0].Position.Y + this.listViewMapping.Top;
                    this.comboBoxDestChannels.Visible = true;
                }
            }
            this.m_knownTop = this.listViewMapping.TopItem.Index;
        }

        private void listViewMapping_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listViewMapping_Enter(object sender, EventArgs e)
        {
            this.comboBoxDestChannels.Visible = this.listViewMapping.SelectedItems.Count > 0;
        }

        private void listViewMapping_Leave(object sender, EventArgs e)
        {
            this.comboBoxDestChannels.Visible = base.ActiveControl == this.comboBoxDestChannels;
        }

        private void listViewMapping_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listViewMapping.SelectedItems.Count > 0)
            {
                this.comboBoxDestChannels.Top = this.listViewMapping.SelectedItems[0].Position.Y + this.listViewMapping.Top;
                Channel tag = (Channel) this.listViewMapping.SelectedItems[0].Tag;
                this.m_internalUpdate = true;
                if (tag == null)
                {
                    this.comboBoxDestChannels.SelectedIndex = 0;
                }
                else
                {
                    this.comboBoxDestChannels.SelectedItem = tag;
                }
                this.m_internalUpdate = false;
                this.comboBoxDestChannels.Visible = true;
            }
            else
            {
                this.comboBoxDestChannels.Visible = false;
            }
        }
    }
}

