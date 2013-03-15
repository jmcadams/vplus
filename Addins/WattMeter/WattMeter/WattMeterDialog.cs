namespace WattMeter
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    internal class WattMeterDialog : Form
    {
        private Button buttonCalcTotalDraw;
        private Button buttonClearChecks;
        private Button buttonCreateGroup;
        private Button buttonDone;
        private Button buttonRenameGroup;
        private Button buttonTestGroup;
        private Button buttonUpdateGroup;
        private CheckedListBox checkedListBoxChannels;
        private IContainer components = null;
        private const int GRAPH_AMP_GUTTER = 40;
        private const int GRAPH_TIME_GUTTER = 30;
        private const int GRAPH_TOP_GUTTER = 12;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private HScrollBar hScrollBar;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private ListBox listBoxGroups;
        private string[] m_channelWatts;
        private XmlNode m_currentGroupNode = null;
        private XmlNode m_dataNode;
        private int[] m_graphPoints;
        private float m_maxAmps;
        private EventSequence m_sequence;
        private PictureBox pictureBoxGraph;
        private TextBox textBoxChannelAmps;
        private TextBox textBoxChannelWatts;
        private TextBox textBoxMaxAmps;
        private TextBox textBoxVolts;

        public WattMeterDialog(EventSequence sequence, XmlNode dataNode)
        {
            if (sequence == null)
            {
                throw new Exception("The watt meter requires a sequence to be open.");
            }
            this.InitializeComponent();
            this.m_sequence = sequence;
            this.m_dataNode = dataNode;
            this.checkedListBoxChannels.Items.AddRange(sequence.Channels.ToArray());
            foreach (XmlNode node in this.m_dataNode.SelectNodes("ChannelGroup"))
            {
                this.listBoxGroups.Items.Add(node.Attributes["name"].Value);
            }
            XmlNode newChild = this.m_dataNode.SelectSingleNode("ChannelWatts");
            if (newChild == null)
            {
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < this.m_sequence.Channels.Count; i++)
                {
                    builder.Append("0,");
                }
                newChild = this.m_dataNode.OwnerDocument.CreateElement("ChannelWatts");
                this.m_dataNode.AppendChild(newChild);
                newChild.InnerText = builder.ToString().Substring(0, builder.Length - 1);
            }
            this.m_channelWatts = newChild.InnerText.Split(new char[] { ',' });
            if (this.m_channelWatts.Length < this.m_sequence.ChannelCount)
            {
                int length = this.m_channelWatts.Length;
                Array.Resize<string>(ref this.m_channelWatts, this.m_sequence.ChannelCount);
                while (length < this.m_sequence.ChannelCount)
                {
                    this.m_channelWatts[length++] = "0";
                }
            }
            newChild = this.m_dataNode.SelectSingleNode("Volts");
            if (newChild == null)
            {
                newChild = this.m_dataNode.OwnerDocument.CreateElement("Volts");
                this.m_dataNode.AppendChild(newChild);
                newChild.InnerText = "120";
            }
            this.textBoxVolts.Text = newChild.InnerText;
            this.m_graphPoints = new int[this.m_sequence.EventValues.GetLength(1)];
        }

        private void buttonCalcTotalDraw_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                this.Recalc();
                this.pictureBoxGraph.Refresh();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void buttonClearChecks_Click(object sender, EventArgs e)
        {
            this.checkedListBoxChannels.BeginUpdate();
            int[] dest = new int[this.checkedListBoxChannels.CheckedIndices.Count];
            this.checkedListBoxChannels.CheckedIndices.CopyTo(dest, 0);
            foreach (int num in dest)
            {
                this.checkedListBoxChannels.SetItemChecked(num, false);
            }
            this.checkedListBoxChannels.EndUpdate();
        }

        private void buttonCreateGroup_Click(object sender, EventArgs e)
        {
            if (this.checkedListBoxChannels.CheckedIndices.Count == 0)
            {
                MessageBox.Show("There are no channels selected", "Watt Meter", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                TextQueryDialog dialog = new TextQueryDialog("Watt Meter", "Name of the new group", string.Empty);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.listBoxGroups.Items.Add(dialog.Response);
                    XmlNode newChild = this.m_dataNode.OwnerDocument.CreateElement("ChannelGroup");
                    this.m_dataNode.AppendChild(newChild);
                    XmlAttribute node = newChild.OwnerDocument.CreateAttribute("name");
                    node.Value = dialog.Response;
                    newChild.Attributes.Append(node);
                    node = newChild.OwnerDocument.CreateAttribute("maxAmps");
                    node.Value = "0";
                    newChild.Attributes.Append(node);
                    this.UpdateGroupChannels(newChild);
                    this.listBoxGroups.SelectedIndex = this.listBoxGroups.Items.Count - 1;
                }
                dialog.Dispose();
            }
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            XmlNode node = this.m_dataNode.SelectSingleNode("ChannelWatts");
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < this.m_sequence.Channels.Count; i++)
            {
                builder.Append(this.m_channelWatts[i] + ",");
            }
            node.InnerText = builder.ToString().Substring(0, builder.Length - 1);
        }

        private void buttonRenameGroup_Click(object sender, EventArgs e)
        {
            TextQueryDialog dialog = new TextQueryDialog("Watt Meter", "New name", string.Empty);
            XmlNode currentGroupNode = this.m_currentGroupNode;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.listBoxGroups.BeginUpdate();
                int selectedIndex = this.listBoxGroups.SelectedIndex;
                this.listBoxGroups.Items.RemoveAt(selectedIndex);
                this.listBoxGroups.Items.Insert(selectedIndex, dialog.Response);
                this.listBoxGroups.EndUpdate();
                currentGroupNode.Attributes["name"].Value = dialog.Response;
            }
            dialog.Dispose();
        }

        private void buttonTestGroup_Click(object sender, EventArgs e)
        {
            int num = 0;
            try
            {
                num = Convert.ToInt32(this.m_currentGroupNode.Attributes["maxAmps"].Value);
            }
            catch
            {
                MessageBox.Show("The max amps for this group needs to be an integer (non-decimal) value", "Watt Meter", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            if (num == 0)
            {
                MessageBox.Show("Max of 0 amps?  I won't even try to check for this.", "Watt Meter", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                int volts = this.GetVolts();
                if (volts != 0)
                {
                    float num3 = 0f;
                    List<int> list = null;
                    this.Cursor = Cursors.WaitCursor;
                    try
                    {
                        list = new List<int>();
                        string[] strArray = this.m_currentGroupNode.InnerText.Split(new char[] { ',' });
                        int length = strArray.GetLength(0);
                        int[] numArray = new int[length];
                        float[] numArray2 = new float[length];
                        if (length == 0)
                        {
                            MessageBox.Show("There are no channels in this group", "Watt Meter", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;
                        }
                        int index = 0;
                        foreach (string str in strArray)
                        {
                            int num6 = Convert.ToInt32(str);
                            numArray[index] = num6;
                            numArray2[index] = Convert.ToSingle(this.m_channelWatts[num6]);
                            index++;
                        }
                        int num8 = this.m_graphPoints.Length;
                        float num12 = 0f;
                        for (int i = 0; i < num8; i++)
                        {
                            float num4 = 0f;
                            for (int j = 0; j < length; j++)
                            {
                                int num10 = this.m_sequence.EventValues[numArray[j], i];
                                num4 += (num10 > 0) ? ((((float) num10) / 255f) * numArray2[j]) : 0f;
                            }
                            float num11 = num4 / ((float) volts);
                            if ((num11 > num) && (num12 <= num))
                            {
                                list.Add(i);
                            }
                            num12 = num11;
                            num3 = Math.Max(num11, num3);
                        }
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                    if (list.Count > 0)
                    {
                        StringBuilder builder = new StringBuilder();
                        float num14 = 1000f / ((float) this.m_sequence.EventPeriod);
                        foreach (int num13 in list)
                        {
                            builder.AppendFormat("{0:d2}:{1:d2}, event {2}\n", ((int) (((float) num13) / num14)) / 60, ((int) (((float) num13) / num14)) % 60, ((int) (((float) num13) % num14)) + 1);
                        }
                        MessageBox.Show(string.Format("{0} amps was exceeded starting at the following locations:\n\n{1}", this.m_currentGroupNode.Attributes["maxAmps"].Value, builder.ToString()), "Watt Meter", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        MessageBox.Show(string.Format("{0} never exceeds {1} amps.\n\nPeak draw was {2:f2} amps.", this.m_currentGroupNode.Attributes["name"].Value, this.m_currentGroupNode.Attributes["maxAmps"].Value, num3), "Watt Meter", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void buttonUpdateGroup_Click(object sender, EventArgs e)
        {
            if (this.checkedListBoxChannels.CheckedIndices.Count == 0)
            {
                MessageBox.Show("There are no channels selected", "Watt Meter", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                this.UpdateGroupChannels(this.m_currentGroupNode);
            }
        }

        private void checkedListBoxChannels_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int num = this.checkedListBoxChannels.CheckedItems.Count + ((e.NewValue == CheckState.Checked) ? 1 : -1);
            this.buttonCreateGroup.Enabled = num > 0;
        }

        private void checkedListBoxChannels_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.checkedListBoxChannels.SelectedIndex != -1)
            {
                this.textBoxChannelWatts.Text = this.m_channelWatts[this.checkedListBoxChannels.SelectedIndex];
                try
                {
                    this.textBoxChannelAmps.Text = (Convert.ToSingle(this.textBoxChannelWatts.Text) / ((float) Convert.ToInt32(this.textBoxVolts.Text))).ToString("f2");
                }
                catch
                {
                    MessageBox.Show("Make sure that the amps are a valid decimal value and watts is a valid integer value.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                this.textBoxChannelWatts.Text = string.Empty;
                this.textBoxChannelAmps.Text = string.Empty;
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

        private int GetVolts()
        {
            int num = 0;
            try
            {
                num = Convert.ToInt32(this.textBoxVolts.Text);
            }
            catch
            {
                MessageBox.Show("The listed volts needs to be an integer (non-decimal) value", "Watt Meter", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return num;
        }

        private void hScrollBar_ValueChanged(object sender, EventArgs e)
        {
            this.pictureBoxGraph.Refresh();
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new GroupBox();
            this.textBoxChannelAmps = new TextBox();
            this.label5 = new Label();
            this.buttonClearChecks = new Button();
            this.buttonUpdateGroup = new Button();
            this.textBoxChannelWatts = new TextBox();
            this.label2 = new Label();
            this.checkedListBoxChannels = new CheckedListBox();
            this.groupBox2 = new GroupBox();
            this.label4 = new Label();
            this.textBoxVolts = new TextBox();
            this.label3 = new Label();
            this.buttonTestGroup = new Button();
            this.textBoxMaxAmps = new TextBox();
            this.label1 = new Label();
            this.buttonRenameGroup = new Button();
            this.buttonCreateGroup = new Button();
            this.listBoxGroups = new ListBox();
            this.groupBox3 = new GroupBox();
            this.hScrollBar = new HScrollBar();
            this.pictureBoxGraph = new PictureBox();
            this.buttonCalcTotalDraw = new Button();
            this.buttonDone = new Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((ISupportInitialize) this.pictureBoxGraph).BeginInit();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.textBoxChannelAmps);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.buttonClearChecks);
            this.groupBox1.Controls.Add(this.buttonUpdateGroup);
            this.groupBox1.Controls.Add(this.textBoxChannelWatts);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.checkedListBoxChannels);
            this.groupBox1.Location = new Point(30, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xe1, 310);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Channels";
            this.textBoxChannelAmps.Location = new Point(0xb5, 0xf5);
            this.textBoxChannelAmps.Name = "textBoxChannelAmps";
            this.textBoxChannelAmps.Size = new Size(0x21, 20);
            this.textBoxChannelAmps.TabIndex = 5;
            this.textBoxChannelAmps.Leave += new EventHandler(this.textBoxChannelAmps_Leave);
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x83, 0xf8);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x2c, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "or amps";
            this.buttonClearChecks.Location = new Point(8, 0xd3);
            this.buttonClearChecks.Name = "buttonClearChecks";
            this.buttonClearChecks.Size = new Size(0x59, 0x17);
            this.buttonClearChecks.TabIndex = 1;
            this.buttonClearChecks.Text = "Clear checks";
            this.buttonClearChecks.UseVisualStyleBackColor = true;
            this.buttonClearChecks.Click += new EventHandler(this.buttonClearChecks_Click);
            this.buttonUpdateGroup.Enabled = false;
            this.buttonUpdateGroup.Location = new Point(8, 0x114);
            this.buttonUpdateGroup.Name = "buttonUpdateGroup";
            this.buttonUpdateGroup.Size = new Size(0x59, 0x17);
            this.buttonUpdateGroup.TabIndex = 6;
            this.buttonUpdateGroup.Text = "Update Group";
            this.buttonUpdateGroup.UseVisualStyleBackColor = true;
            this.buttonUpdateGroup.Click += new EventHandler(this.buttonUpdateGroup_Click);
            this.textBoxChannelWatts.Location = new Point(0x55, 0xf5);
            this.textBoxChannelWatts.Name = "textBoxChannelWatts";
            this.textBoxChannelWatts.Size = new Size(40, 20);
            this.textBoxChannelWatts.TabIndex = 3;
            this.textBoxChannelWatts.Leave += new EventHandler(this.textBoxChannelWatts_Leave);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(5, 0xf8);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x4a, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Channel watts";
            this.checkedListBoxChannels.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.checkedListBoxChannels.FormattingEnabled = true;
            this.checkedListBoxChannels.Location = new Point(8, 0x15);
            this.checkedListBoxChannels.Name = "checkedListBoxChannels";
            this.checkedListBoxChannels.Size = new Size(0xce, 0xb8);
            this.checkedListBoxChannels.TabIndex = 0;
            this.checkedListBoxChannels.SelectedIndexChanged += new EventHandler(this.checkedListBoxChannels_SelectedIndexChanged);
            this.checkedListBoxChannels.ItemCheck += new ItemCheckEventHandler(this.checkedListBoxChannels_ItemCheck);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.textBoxVolts);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.buttonTestGroup);
            this.groupBox2.Controls.Add(this.textBoxMaxAmps);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.buttonRenameGroup);
            this.groupBox2.Controls.Add(this.buttonCreateGroup);
            this.groupBox2.Controls.Add(this.listBoxGroups);
            this.groupBox2.Location = new Point(0x10f, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0xdf, 310);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Groups";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0xa5, 0x119);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x1d, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "volts";
            this.textBoxVolts.Location = new Point(0x7e, 0x116);
            this.textBoxVolts.MaxLength = 3;
            this.textBoxVolts.Name = "textBoxVolts";
            this.textBoxVolts.Size = new Size(0x21, 20);
            this.textBoxVolts.TabIndex = 7;
            this.textBoxVolts.Leave += new EventHandler(this.textBoxVolts_Leave);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x5e, 0x119);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x1a, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "with";
            this.buttonTestGroup.Enabled = false;
            this.buttonTestGroup.Location = new Point(13, 0x114);
            this.buttonTestGroup.Name = "buttonTestGroup";
            this.buttonTestGroup.Size = new Size(0x4b, 0x17);
            this.buttonTestGroup.TabIndex = 5;
            this.buttonTestGroup.Text = "Test Group";
            this.buttonTestGroup.UseVisualStyleBackColor = true;
            this.buttonTestGroup.Click += new EventHandler(this.buttonTestGroup_Click);
            this.textBoxMaxAmps.Enabled = false;
            this.textBoxMaxAmps.Location = new Point(0x47, 0xce);
            this.textBoxMaxAmps.Name = "textBoxMaxAmps";
            this.textBoxMaxAmps.Size = new Size(0x36, 20);
            this.textBoxMaxAmps.TabIndex = 2;
            this.textBoxMaxAmps.Leave += new EventHandler(this.textBoxMaxAmps_Leave);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(10, 0xd1);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x37, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Max amps";
            this.buttonRenameGroup.Enabled = false;
            this.buttonRenameGroup.Location = new Point(0x79, 0xf2);
            this.buttonRenameGroup.Name = "buttonRenameGroup";
            this.buttonRenameGroup.Size = new Size(90, 0x17);
            this.buttonRenameGroup.TabIndex = 4;
            this.buttonRenameGroup.Text = "Rename Group";
            this.buttonRenameGroup.UseVisualStyleBackColor = true;
            this.buttonRenameGroup.Click += new EventHandler(this.buttonRenameGroup_Click);
            this.buttonCreateGroup.Enabled = false;
            this.buttonCreateGroup.Location = new Point(13, 0xf2);
            this.buttonCreateGroup.Name = "buttonCreateGroup";
            this.buttonCreateGroup.Size = new Size(0x5c, 0x17);
            this.buttonCreateGroup.TabIndex = 3;
            this.buttonCreateGroup.Text = "Create Group";
            this.buttonCreateGroup.UseVisualStyleBackColor = true;
            this.buttonCreateGroup.Click += new EventHandler(this.buttonCreateGroup_Click);
            this.listBoxGroups.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.listBoxGroups.FormattingEnabled = true;
            this.listBoxGroups.Location = new Point(12, 0x18);
            this.listBoxGroups.Name = "listBoxGroups";
            this.listBoxGroups.Size = new Size(0xc7, 0xad);
            this.listBoxGroups.TabIndex = 0;
            this.listBoxGroups.SelectedIndexChanged += new EventHandler(this.listBoxGroups_SelectedIndexChanged);
            this.listBoxGroups.KeyDown += new KeyEventHandler(this.listBoxGroups_KeyDown);
            this.groupBox3.Controls.Add(this.hScrollBar);
            this.groupBox3.Controls.Add(this.pictureBoxGraph);
            this.groupBox3.Controls.Add(this.buttonCalcTotalDraw);
            this.groupBox3.Location = new Point(30, 350);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(0x1d0, 0xd5);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Sequence Total Current Draw";
            this.hScrollBar.Enabled = false;
            this.hScrollBar.Location = new Point(13, 0x99);
            this.hScrollBar.Name = "hScrollBar";
            this.hScrollBar.Size = new Size(0x1b6, 0x11);
            this.hScrollBar.TabIndex = 2;
            this.hScrollBar.ValueChanged += new EventHandler(this.hScrollBar_ValueChanged);
            this.pictureBoxGraph.BackColor = Color.White;
            this.pictureBoxGraph.Location = new Point(13, 0x1c);
            this.pictureBoxGraph.Name = "pictureBoxGraph";
            this.pictureBoxGraph.Size = new Size(0x1b6, 0x7d);
            this.pictureBoxGraph.TabIndex = 0;
            this.pictureBoxGraph.TabStop = false;
            this.pictureBoxGraph.Paint += new PaintEventHandler(this.pictureBoxGraph_Paint);
            this.buttonCalcTotalDraw.Location = new Point(13, 180);
            this.buttonCalcTotalDraw.Name = "buttonCalcTotalDraw";
            this.buttonCalcTotalDraw.Size = new Size(0x4b, 0x17);
            this.buttonCalcTotalDraw.TabIndex = 1;
            this.buttonCalcTotalDraw.Text = "Calculate";
            this.buttonCalcTotalDraw.UseVisualStyleBackColor = true;
            this.buttonCalcTotalDraw.Click += new EventHandler(this.buttonCalcTotalDraw_Click);
            this.buttonDone.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonDone.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonDone.Location = new Point(0x1bc, 570);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new Size(0x4b, 0x17);
            this.buttonDone.TabIndex = 3;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.buttonDone.Click += new EventHandler(this.buttonDone_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x213, 0x25d);
            base.Controls.Add(this.buttonDone);
            base.Controls.Add(this.groupBox3);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Name = "WattMeterDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Watt Meter";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((ISupportInitialize) this.pictureBoxGraph).EndInit();
            base.ResumeLayout(false);
        }

        private void listBoxGroups_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                this.m_dataNode.RemoveChild(this.m_currentGroupNode);
                this.listBoxGroups.Items.RemoveAt(this.listBoxGroups.SelectedIndex);
            }
        }

        private void listBoxGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.buttonRenameGroup.Enabled = this.listBoxGroups.SelectedIndex != -1;
            this.buttonTestGroup.Enabled = this.buttonRenameGroup.Enabled;
            this.buttonUpdateGroup.Enabled = this.buttonRenameGroup.Enabled;
            this.textBoxMaxAmps.Enabled = this.listBoxGroups.SelectedIndex != -1;
            this.checkedListBoxChannels.BeginUpdate();
            for (int i = 0; i < this.checkedListBoxChannels.Items.Count; i++)
            {
                this.checkedListBoxChannels.SetItemChecked(i, false);
            }
            if (this.listBoxGroups.SelectedIndex != -1)
            {
                this.m_currentGroupNode = this.m_dataNode.SelectSingleNode(string.Format("ChannelGroup[position()={0}]", this.listBoxGroups.SelectedIndex + 1));
                if (this.m_currentGroupNode.InnerText != string.Empty)
                {
                    this.textBoxMaxAmps.Text = this.m_currentGroupNode.Attributes["maxAmps"].Value;
                    foreach (string str in this.m_currentGroupNode.InnerText.Split(new char[] { ',' }))
                    {
                        int index = Convert.ToInt32(str);
                        if (index < this.checkedListBoxChannels.Items.Count)
                        {
                            this.checkedListBoxChannels.SetItemChecked(index, true);
                        }
                    }
                }
                else
                {
                    this.textBoxMaxAmps.Text = string.Empty;
                }
            }
            else
            {
                this.textBoxMaxAmps.Text = string.Empty;
                this.m_currentGroupNode = null;
            }
            this.checkedListBoxChannels.EndUpdate();
        }

        private void pictureBoxGraph_Paint(object sender, PaintEventArgs e)
        {
            if (this.m_maxAmps != 0f)
            {
                e.Graphics.FillRectangle(Brushes.White, 0, 0, this.pictureBoxGraph.Width, this.pictureBoxGraph.Height);
                e.Graphics.DrawRectangle(Pens.Black, 40, 12, (this.pictureBoxGraph.Width - 40) - 1, (this.pictureBoxGraph.Height - 30) - 12);
                float num4 = 1000f / ((float) this.m_sequence.EventPeriod);
                float num5 = 40f + (num4 - (((float) this.hScrollBar.Value) % num4));
                float num6 = this.pictureBoxGraph.Height - 30;
                float num7 = num6 + 5f;
                float num3 = num5;
                int num = ((this.hScrollBar.Value * this.m_sequence.EventPeriod) / 0x3e8) + 1;
                while (num3 < e.ClipRectangle.Right)
                {
                    if ((num % 5) == 0)
                    {
                        e.Graphics.DrawLine(Pens.Red, num3, num6, num3, num7);
                        e.Graphics.DrawString(string.Format("{0}:{1:d2}", num / 60, num % 60), this.Font, Brushes.Black, (float) (num3 - 10f), (float) (num7 + 3f));
                    }
                    else
                    {
                        e.Graphics.DrawLine(Pens.Black, num3, num6, num3, num7);
                    }
                    num++;
                    num3 += num4;
                }
                int num8 = (this.m_maxAmps > 10f) ? ((this.m_maxAmps > 50f) ? 10 : 5) : 1;
                int num9 = (this.pictureBoxGraph.Height - 30) - 12;
                float num10 = ((float) num9) / (this.m_maxAmps / ((float) num8));
                float num2 = (this.pictureBoxGraph.Height - 30) - num10;
                for (num = num8; num < this.m_maxAmps; num += num8)
                {
                    e.Graphics.DrawLine(Pens.Black, 0x23, (int) num2, 40, (int) num2);
                    e.Graphics.DrawLine(Pens.LightGray, 0x29, (int) num2, this.pictureBoxGraph.Width - 2, (int) num2);
                    num2 -= num10;
                }
                e.Graphics.DrawString(num8.ToString("f1"), this.Font, Brushes.Black, (float) 2f, (float) (((this.pictureBoxGraph.Height - 30) - num10) - 6f));
                e.Graphics.DrawString(this.m_maxAmps.ToString("f1"), this.Font, Brushes.Black, 2f, (float) (((int) num2) - 5));
                int index = Math.Max(this.hScrollBar.Value - 1, 0);
                int num12 = Math.Min((this.hScrollBar.Value + this.pictureBoxGraph.Width) - 40, this.m_graphPoints.Length);
                Point point = new Point(40, this.m_graphPoints[index]);
                Point point2 = new Point(40, 0);
                while (index < num12)
                {
                    point2.Y = this.m_graphPoints[index];
                    if ((index != 0) && ((point2.Y != num9) || (point.Y != num9)))
                    {
                        e.Graphics.DrawLine(Pens.Blue, point, point2);
                    }
                    point.X = point2.X;
                    point.Y = point2.Y;
                    index++;
                    point2.X++;
                }
                e.Graphics.DrawRectangle(Pens.Gray, 0, 0, this.pictureBoxGraph.Width - 1, this.pictureBoxGraph.Height - 1);
            }
        }

        private void Recalc()
        {
            this.m_maxAmps = float.MinValue;
            int length = this.m_graphPoints.Length;
            int volts = this.GetVolts();
            if (volts != 0)
            {
                float num2;
                int num4;
                int num6;
                float num7;
                int num9;
                int count = this.m_sequence.Channels.Count;
                float[] numArray = new float[count];
                for (int i = 0; i < count; i++)
                {
                    numArray[i] = Convert.ToSingle(this.m_channelWatts[i]);
                }
                for (num9 = 0; num9 < length; num9++)
                {
                    num2 = 0f;
                    num4 = 0;
                    while (num4 < count)
                    {
                        num6 = this.m_sequence.EventValues[num4, num9];
                        num2 += (num6 > 0) ? ((((float) num6) / 255f) * numArray[num4]) : 0f;
                        num4++;
                    }
                    num7 = num2 / ((float) volts);
                    this.m_maxAmps = Math.Max(this.m_maxAmps, num7);
                }
                if (!((this.m_maxAmps % 5f) == 0f))
                {
                    this.m_maxAmps = (((int) (this.m_maxAmps + 5f)) / 5) * 5;
                }
                int num10 = (this.pictureBoxGraph.Height - 30) - 12;
                for (num9 = 0; num9 < length; num9++)
                {
                    num2 = 0f;
                    for (num4 = 0; num4 < count; num4++)
                    {
                        num6 = this.m_sequence.EventValues[num4, num9];
                        num2 += (num6 > 0) ? ((((float) num6) / 255f) * numArray[num4]) : 0f;
                    }
                    num7 = num2 / ((float) volts);
                    this.m_graphPoints[num9] = (int) ((num10 - ((num7 / this.m_maxAmps) * num10)) + 12f);
                }
                this.hScrollBar.Maximum = this.m_graphPoints.Length;
                this.hScrollBar.LargeChange = this.pictureBoxGraph.Width - 40;
                this.hScrollBar.Enabled = (this.m_graphPoints.Length + 40) > this.pictureBoxGraph.Width;
            }
        }

        private void textBoxChannelAmps_Leave(object sender, EventArgs e)
        {
            this.UpdateChannelRating((Control) sender);
        }

        private void textBoxChannelWatts_Leave(object sender, EventArgs e)
        {
            this.UpdateChannelRating((Control) sender);
        }

        private void textBoxMaxAmps_Leave(object sender, EventArgs e)
        {
            this.m_currentGroupNode.Attributes["maxAmps"].Value = this.textBoxMaxAmps.Text;
        }

        private void textBoxVolts_Leave(object sender, EventArgs e)
        {
            this.m_dataNode.SelectSingleNode("Volts").InnerText = this.textBoxVolts.Text;
        }

        private void UpdateChannelRating(Control updatingControl)
        {
            if (this.checkedListBoxChannels.SelectedIndex != -1)
            {
                string text;
                if (updatingControl == this.textBoxChannelWatts)
                {
                    text = this.textBoxChannelWatts.Text;
                    try
                    {
                        this.textBoxChannelAmps.Text = (Convert.ToSingle(text) / ((float) Convert.ToInt32(this.textBoxVolts.Text))).ToString("f2");
                    }
                    catch
                    {
                        MessageBox.Show("Make sure that the watts and volts are a valid integer value.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        base.ActiveControl = this.textBoxChannelWatts;
                        return;
                    }
                }
                else
                {
                    try
                    {
                        text = ((int) Math.Round((double) (Convert.ToSingle(this.textBoxChannelAmps.Text) * Convert.ToInt32(this.textBoxVolts.Text)), 0)).ToString();
                    }
                    catch
                    {
                        MessageBox.Show("Make sure that the amps are a valid decimal value and volts is a valid integer value.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        base.ActiveControl = this.textBoxChannelAmps;
                        return;
                    }
                    this.textBoxChannelWatts.Text = text;
                }
                this.m_channelWatts[this.checkedListBoxChannels.SelectedIndex] = text;
            }
        }

        private void UpdateGroupChannels(XmlNode groupNode)
        {
            StringBuilder builder = new StringBuilder();
            foreach (int num in this.checkedListBoxChannels.CheckedIndices)
            {
                builder.Append(num.ToString() + ",");
            }
            if (builder.Length > 0)
            {
                groupNode.InnerText = builder.ToString().Substring(0, builder.Length - 1);
            }
            else
            {
                groupNode.InnerText = string.Empty;
            }
        }
    }
}

