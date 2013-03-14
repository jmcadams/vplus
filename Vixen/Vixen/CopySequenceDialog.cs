namespace Vixen
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    internal class CopySequenceDialog : Form
    {
        private Button buttonApply;
        private Button buttonAutoMap;
        private Button buttonCancel;
        private Button buttonOK;
        private Button buttonReset;
        private CheckBox checkBoxChannelDefs;
        private CheckBox checkBoxSequenceLength;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ComboBox comboBoxDestChannels;
        private ComboBox comboBoxDestSequence;
        private ComboBox comboBoxSourceSequence;
        private IContainer components = null;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
        private Label label2;
        private ListView listViewMapping;
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

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void groupBox2_Leave(object sender, EventArgs e)
        {
            this.comboBoxDestChannels.Visible = false;
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new GroupBox();
            this.buttonApply = new Button();
            this.label2 = new Label();
            this.label1 = new Label();
            this.comboBoxDestSequence = new ComboBox();
            this.comboBoxSourceSequence = new ComboBox();
            this.groupBox2 = new GroupBox();
            this.buttonAutoMap = new Button();
            this.comboBoxDestChannels = new ComboBox();
            this.buttonReset = new Button();
            this.listViewMapping = new ListView();
            this.columnHeader1 = new ColumnHeader();
            this.columnHeader2 = new ColumnHeader();
            this.checkBoxChannelDefs = new CheckBox();
            this.checkBoxSequenceLength = new CheckBox();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.buttonApply);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBoxDestSequence);
            this.groupBox1.Controls.Add(this.comboBoxSourceSequence);
            this.groupBox1.Location = new Point(0x10, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x1aa, 0x89);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sequences";
            this.buttonApply.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonApply.Location = new Point(0x13c, 100);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new Size(0x4b, 0x17);
            this.buttonApply.TabIndex = 5;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new EventHandler(this.buttonApply_Click);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x16, 70);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x70, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Destination Sequence";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x16, 0x1d);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x5d, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Source Sequence";
            this.comboBoxDestSequence.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxDestSequence.FormattingEnabled = true;
            this.comboBoxDestSequence.Location = new Point(0x9b, 0x43);
            this.comboBoxDestSequence.Name = "comboBoxDestSequence";
            this.comboBoxDestSequence.Size = new Size(0xec, 0x15);
            this.comboBoxDestSequence.TabIndex = 4;
            this.comboBoxDestSequence.SelectedIndexChanged += new EventHandler(this.comboBoxSourceSequence_SelectedIndexChanged);
            this.comboBoxSourceSequence.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxSourceSequence.FormattingEnabled = true;
            this.comboBoxSourceSequence.Location = new Point(0x9b, 0x1a);
            this.comboBoxSourceSequence.Name = "comboBoxSourceSequence";
            this.comboBoxSourceSequence.Size = new Size(0xec, 0x15);
            this.comboBoxSourceSequence.TabIndex = 2;
            this.comboBoxSourceSequence.SelectedIndexChanged += new EventHandler(this.comboBoxSourceSequence_SelectedIndexChanged);
            this.groupBox2.Controls.Add(this.buttonAutoMap);
            this.groupBox2.Controls.Add(this.comboBoxDestChannels);
            this.groupBox2.Controls.Add(this.buttonReset);
            this.groupBox2.Controls.Add(this.listViewMapping);
            this.groupBox2.Location = new Point(0x10, 0xab);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x1a9, 0x11b);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Channel mappings";
            this.groupBox2.Leave += new EventHandler(this.groupBox2_Leave);
            this.buttonAutoMap.Location = new Point(0x10, 0xf9);
            this.buttonAutoMap.Name = "buttonAutoMap";
            this.buttonAutoMap.Size = new Size(0x4b, 0x17);
            this.buttonAutoMap.TabIndex = 2;
            this.buttonAutoMap.Text = "Auto map";
            this.buttonAutoMap.UseVisualStyleBackColor = true;
            this.buttonAutoMap.Click += new EventHandler(this.buttonAutoMap_Click);
            this.comboBoxDestChannels.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxDestChannels.Items.AddRange(new object[] { "none" });
            this.comboBoxDestChannels.Location = new Point(0xfb, 0xc7);
            this.comboBoxDestChannels.Name = "comboBoxDestChannels";
            this.comboBoxDestChannels.Size = new Size(0x79, 0x15);
            this.comboBoxDestChannels.TabIndex = 1;
            this.comboBoxDestChannels.Visible = false;
            this.comboBoxDestChannels.SelectedIndexChanged += new EventHandler(this.comboBoxDestChannels_SelectedIndexChanged);
            this.comboBoxDestChannels.Leave += new EventHandler(this.comboBoxDestChannels_Leave);
            this.buttonReset.Location = new Point(0x61, 0xf9);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new Size(0x6c, 0x17);
            this.buttonReset.TabIndex = 3;
            this.buttonReset.Text = "Reset all mappings";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new EventHandler(this.buttonReset_Click);
            this.listViewMapping.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.listViewMapping.Columns.AddRange(new ColumnHeader[] { this.columnHeader1, this.columnHeader2 });
            this.listViewMapping.FullRowSelect = true;
            this.listViewMapping.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.listViewMapping.HideSelection = false;
            this.listViewMapping.Location = new Point(0x10, 0x17);
            this.listViewMapping.MultiSelect = false;
            this.listViewMapping.Name = "listViewMapping";
            this.listViewMapping.OwnerDraw = true;
            this.listViewMapping.Size = new Size(390, 0xd7);
            this.listViewMapping.TabIndex = 0;
            this.listViewMapping.UseCompatibleStateImageBehavior = false;
            this.listViewMapping.View = View.Details;
            this.listViewMapping.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(this.listViewMapping_DrawColumnHeader);
            this.listViewMapping.DrawItem += new DrawListViewItemEventHandler(this.listViewMapping_DrawItem);
            this.listViewMapping.SelectedIndexChanged += new EventHandler(this.listViewMapping_SelectedIndexChanged);
            this.listViewMapping.Leave += new EventHandler(this.listViewMapping_Leave);
            this.listViewMapping.Enter += new EventHandler(this.listViewMapping_Enter);
            this.listViewMapping.DrawSubItem += new DrawListViewSubItemEventHandler(this.listViewMapping_DrawSubItem);
            this.columnHeader1.Text = "Source channel";
            this.columnHeader2.Text = "Destination channel";
            this.checkBoxChannelDefs.AutoSize = true;
            this.checkBoxChannelDefs.Location = new Point(20, 0x1da);
            this.checkBoxChannelDefs.Name = "checkBoxChannelDefs";
            this.checkBoxChannelDefs.Size = new Size(0xc9, 0x11);
            this.checkBoxChannelDefs.TabIndex = 2;
            this.checkBoxChannelDefs.Text = "Copy definitions of selected channels";
            this.checkBoxChannelDefs.UseVisualStyleBackColor = true;
            this.checkBoxSequenceLength.AutoSize = true;
            this.checkBoxSequenceLength.Location = new Point(20, 0x1f1);
            this.checkBoxSequenceLength.Name = "checkBoxSequenceLength";
            this.checkBoxSequenceLength.Size = new Size(0x107, 0x11);
            this.checkBoxSequenceLength.TabIndex = 3;
            this.checkBoxSequenceLength.Text = "Ensure destination sequence has adequate length";
            this.checkBoxSequenceLength.UseVisualStyleBackColor = true;
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Enabled = false;
            this.buttonOK.Location = new Point(0x127, 0x20f);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x178, 0x20f);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x1cf, 0x22f);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.checkBoxSequenceLength);
            base.Controls.Add(this.checkBoxChannelDefs);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "CopySequenceDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Copy event sequence data";
            base.FormClosing += new FormClosingEventHandler(this.CopySequenceDialog_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
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

