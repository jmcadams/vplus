namespace WattMeter {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;
    using VixenPlus;

    internal partial class WattMeterDialog : Form {
        private const int GRAPH_AMP_GUTTER = 40;
        private const int GRAPH_TIME_GUTTER = 30;
        private const int GRAPH_TOP_GUTTER = 12;
        private string[] m_channelWatts;
        private XmlNode m_currentGroupNode = null;
        private XmlNode m_dataNode;
        private int[] m_graphPoints;
        private float m_maxAmps;
        private EventSequence m_sequence;

        public WattMeterDialog(EventSequence sequence, XmlNode dataNode) {
            if (sequence == null) {
                throw new Exception("The watt meter requires a sequence to be open.");
            }
            this.InitializeComponent();
            this.m_sequence = sequence;
            this.m_dataNode = dataNode;
            this.checkedListBoxChannels.Items.AddRange(sequence.Channels.ToArray());
            foreach (XmlNode node in this.m_dataNode.SelectNodes("ChannelGroup")) {
                this.listBoxGroups.Items.Add(node.Attributes["name"].Value);
            }
            XmlNode newChild = this.m_dataNode.SelectSingleNode("ChannelWatts");
            if (newChild == null) {
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < this.m_sequence.Channels.Count; i++) {
                    builder.Append("0,");
                }
                newChild = this.m_dataNode.OwnerDocument.CreateElement("ChannelWatts");
                this.m_dataNode.AppendChild(newChild);
                newChild.InnerText = builder.ToString().Substring(0, builder.Length - 1);
            }
            this.m_channelWatts = newChild.InnerText.Split(new char[] { ',' });
            if (this.m_channelWatts.Length < this.m_sequence.ChannelCount) {
                int length = this.m_channelWatts.Length;
                Array.Resize<string>(ref this.m_channelWatts, this.m_sequence.ChannelCount);
                while (length < this.m_sequence.ChannelCount) {
                    this.m_channelWatts[length++] = "0";
                }
            }
            newChild = this.m_dataNode.SelectSingleNode("Volts");
            if (newChild == null) {
                newChild = this.m_dataNode.OwnerDocument.CreateElement("Volts");
                this.m_dataNode.AppendChild(newChild);
                newChild.InnerText = "120";
            }
            this.textBoxVolts.Text = newChild.InnerText;
            this.m_graphPoints = new int[this.m_sequence.EventValues.GetLength(1)];
        }

        private void buttonCalcTotalDraw_Click(object sender, EventArgs e) {
            this.Cursor = Cursors.WaitCursor;
            try {
                this.Recalc();
                this.pictureBoxGraph.Refresh();
            }
            finally {
                this.Cursor = Cursors.Default;
            }
        }

        private void buttonClearChecks_Click(object sender, EventArgs e) {
            this.checkedListBoxChannels.BeginUpdate();
            int[] dest = new int[this.checkedListBoxChannels.CheckedIndices.Count];
            this.checkedListBoxChannels.CheckedIndices.CopyTo(dest, 0);
            foreach (int num in dest) {
                this.checkedListBoxChannels.SetItemChecked(num, false);
            }
            this.checkedListBoxChannels.EndUpdate();
        }

        private void buttonCreateGroup_Click(object sender, EventArgs e) {
            if (this.checkedListBoxChannels.CheckedIndices.Count == 0) {
                MessageBox.Show("There are no channels selected", "Watt Meter", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else {
                TextQueryDialog dialog = new TextQueryDialog("Watt Meter", "Name of the new group", string.Empty);
                if (dialog.ShowDialog() == DialogResult.OK) {
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

        private void buttonDone_Click(object sender, EventArgs e) {
            XmlNode node = this.m_dataNode.SelectSingleNode("ChannelWatts");
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < this.m_sequence.Channels.Count; i++) {
                builder.Append(this.m_channelWatts[i] + ",");
            }
            node.InnerText = builder.ToString().Substring(0, builder.Length - 1);
        }

        private void buttonRenameGroup_Click(object sender, EventArgs e) {
            TextQueryDialog dialog = new TextQueryDialog("Watt Meter", "New name", string.Empty);
            XmlNode currentGroupNode = this.m_currentGroupNode;
            if (dialog.ShowDialog() == DialogResult.OK) {
                this.listBoxGroups.BeginUpdate();
                int selectedIndex = this.listBoxGroups.SelectedIndex;
                this.listBoxGroups.Items.RemoveAt(selectedIndex);
                this.listBoxGroups.Items.Insert(selectedIndex, dialog.Response);
                this.listBoxGroups.EndUpdate();
                currentGroupNode.Attributes["name"].Value = dialog.Response;
            }
            dialog.Dispose();
        }

        private void buttonTestGroup_Click(object sender, EventArgs e) {
            int num = 0;
            try {
                num = Convert.ToInt32(this.m_currentGroupNode.Attributes["maxAmps"].Value);
            }
            catch {
                MessageBox.Show("The max amps for this group needs to be an integer (non-decimal) value", "Watt Meter", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            if (num == 0) {
                MessageBox.Show("Max of 0 amps?  I won't even try to check for this.", "Watt Meter", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else {
                int volts = this.GetVolts();
                if (volts != 0) {
                    float num3 = 0f;
                    List<int> list = null;
                    this.Cursor = Cursors.WaitCursor;
                    try {
                        list = new List<int>();
                        string[] strArray = this.m_currentGroupNode.InnerText.Split(new char[] { ',' });
                        int length = strArray.GetLength(0);
                        int[] numArray = new int[length];
                        float[] numArray2 = new float[length];
                        if (length == 0) {
                            MessageBox.Show("There are no channels in this group", "Watt Meter", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            return;
                        }
                        int index = 0;
                        foreach (string str in strArray) {
                            int num6 = Convert.ToInt32(str);
                            numArray[index] = num6;
                            numArray2[index] = Convert.ToSingle(this.m_channelWatts[num6]);
                            index++;
                        }
                        int num8 = this.m_graphPoints.Length;
                        float num12 = 0f;
                        for (int i = 0; i < num8; i++) {
                            float num4 = 0f;
                            for (int j = 0; j < length; j++) {
                                int num10 = this.m_sequence.EventValues[numArray[j], i];
                                num4 += (num10 > 0) ? ((((float)num10) / 255f) * numArray2[j]) : 0f;
                            }
                            float num11 = num4 / ((float)volts);
                            if ((num11 > num) && (num12 <= num)) {
                                list.Add(i);
                            }
                            num12 = num11;
                            num3 = Math.Max(num11, num3);
                        }
                    }
                    finally {
                        this.Cursor = Cursors.Default;
                    }
                    if (list.Count > 0) {
                        StringBuilder builder = new StringBuilder();
                        float num14 = 1000f / ((float)this.m_sequence.EventPeriod);
                        foreach (int num13 in list) {
                            builder.AppendFormat("{0:d2}:{1:d2}, event {2}\n", ((int)(((float)num13) / num14)) / 60, ((int)(((float)num13) / num14)) % 60, ((int)(((float)num13) % num14)) + 1);
                        }
                        MessageBox.Show(string.Format("{0} amps was exceeded starting at the following locations:\n\n{1}", this.m_currentGroupNode.Attributes["maxAmps"].Value, builder.ToString()), "Watt Meter", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else {
                        MessageBox.Show(string.Format("{0} never exceeds {1} amps.\n\nPeak draw was {2:f2} amps.", this.m_currentGroupNode.Attributes["name"].Value, this.m_currentGroupNode.Attributes["maxAmps"].Value, num3), "Watt Meter", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void buttonUpdateGroup_Click(object sender, EventArgs e) {
            if (this.checkedListBoxChannels.CheckedIndices.Count == 0) {
                MessageBox.Show("There are no channels selected", "Watt Meter", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else {
                this.UpdateGroupChannels(this.m_currentGroupNode);
            }
        }

        private void checkedListBoxChannels_ItemCheck(object sender, ItemCheckEventArgs e) {
            int num = this.checkedListBoxChannels.CheckedItems.Count + ((e.NewValue == CheckState.Checked) ? 1 : -1);
            this.buttonCreateGroup.Enabled = num > 0;
        }

        private void checkedListBoxChannels_SelectedIndexChanged(object sender, EventArgs e) {
            if (this.checkedListBoxChannels.SelectedIndex != -1) {
                this.textBoxChannelWatts.Text = this.m_channelWatts[this.checkedListBoxChannels.SelectedIndex];
                try {
                    this.textBoxChannelAmps.Text = (Convert.ToSingle(this.textBoxChannelWatts.Text) / ((float)Convert.ToInt32(this.textBoxVolts.Text))).ToString("f2");
                }
                catch {
                    MessageBox.Show("Make sure that the amps are a valid decimal value and watts is a valid integer value.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else {
                this.textBoxChannelWatts.Text = string.Empty;
                this.textBoxChannelAmps.Text = string.Empty;
            }
        }



        private int GetVolts() {
            int num = 0;
            try {
                num = Convert.ToInt32(this.textBoxVolts.Text);
            }
            catch {
                MessageBox.Show("The listed volts needs to be an integer (non-decimal) value", "Watt Meter", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            return num;
        }

        private void hScrollBar_ValueChanged(object sender, EventArgs e) {
            this.pictureBoxGraph.Refresh();
        }

        private void listBoxGroups_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Delete) {
                this.m_dataNode.RemoveChild(this.m_currentGroupNode);
                this.listBoxGroups.Items.RemoveAt(this.listBoxGroups.SelectedIndex);
            }
        }

        private void listBoxGroups_SelectedIndexChanged(object sender, EventArgs e) {
            this.buttonRenameGroup.Enabled = this.listBoxGroups.SelectedIndex != -1;
            this.buttonTestGroup.Enabled = this.buttonRenameGroup.Enabled;
            this.buttonUpdateGroup.Enabled = this.buttonRenameGroup.Enabled;
            this.textBoxMaxAmps.Enabled = this.listBoxGroups.SelectedIndex != -1;
            this.checkedListBoxChannels.BeginUpdate();
            for (int i = 0; i < this.checkedListBoxChannels.Items.Count; i++) {
                this.checkedListBoxChannels.SetItemChecked(i, false);
            }
            if (this.listBoxGroups.SelectedIndex != -1) {
                this.m_currentGroupNode = this.m_dataNode.SelectSingleNode(string.Format("ChannelGroup[position()={0}]", this.listBoxGroups.SelectedIndex + 1));
                if (this.m_currentGroupNode.InnerText != string.Empty) {
                    this.textBoxMaxAmps.Text = this.m_currentGroupNode.Attributes["maxAmps"].Value;
                    foreach (string str in this.m_currentGroupNode.InnerText.Split(new char[] { ',' })) {
                        int index = Convert.ToInt32(str);
                        if (index < this.checkedListBoxChannels.Items.Count) {
                            this.checkedListBoxChannels.SetItemChecked(index, true);
                        }
                    }
                }
                else {
                    this.textBoxMaxAmps.Text = string.Empty;
                }
            }
            else {
                this.textBoxMaxAmps.Text = string.Empty;
                this.m_currentGroupNode = null;
            }
            this.checkedListBoxChannels.EndUpdate();
        }

        private void pictureBoxGraph_Paint(object sender, PaintEventArgs e) {
            if (this.m_maxAmps != 0f) {
                e.Graphics.FillRectangle(Brushes.White, 0, 0, this.pictureBoxGraph.Width, this.pictureBoxGraph.Height);
                e.Graphics.DrawRectangle(Pens.Black, 40, 12, (this.pictureBoxGraph.Width - 40) - 1, (this.pictureBoxGraph.Height - 30) - 12);
                float num4 = 1000f / ((float)this.m_sequence.EventPeriod);
                float num5 = 40f + (num4 - (((float)this.hScrollBar.Value) % num4));
                float num6 = this.pictureBoxGraph.Height - 30;
                float num7 = num6 + 5f;
                float num3 = num5;
                int num = ((this.hScrollBar.Value * this.m_sequence.EventPeriod) / 0x3e8) + 1;
                while (num3 < e.ClipRectangle.Right) {
                    if ((num % 5) == 0) {
                        e.Graphics.DrawLine(Pens.Red, num3, num6, num3, num7);
                        e.Graphics.DrawString(string.Format("{0}:{1:d2}", num / 60, num % 60), this.Font, Brushes.Black, (float)(num3 - 10f), (float)(num7 + 3f));
                    }
                    else {
                        e.Graphics.DrawLine(Pens.Black, num3, num6, num3, num7);
                    }
                    num++;
                    num3 += num4;
                }
                int num8 = (this.m_maxAmps > 10f) ? ((this.m_maxAmps > 50f) ? 10 : 5) : 1;
                int num9 = (this.pictureBoxGraph.Height - 30) - 12;
                float num10 = ((float)num9) / (this.m_maxAmps / ((float)num8));
                float num2 = (this.pictureBoxGraph.Height - 30) - num10;
                for (num = num8; num < this.m_maxAmps; num += num8) {
                    e.Graphics.DrawLine(Pens.Black, 0x23, (int)num2, 40, (int)num2);
                    e.Graphics.DrawLine(Pens.LightGray, 0x29, (int)num2, this.pictureBoxGraph.Width - 2, (int)num2);
                    num2 -= num10;
                }
                e.Graphics.DrawString(num8.ToString("f1"), this.Font, Brushes.Black, (float)2f, (float)(((this.pictureBoxGraph.Height - 30) - num10) - 6f));
                e.Graphics.DrawString(this.m_maxAmps.ToString("f1"), this.Font, Brushes.Black, 2f, (float)(((int)num2) - 5));
                int index = Math.Max(this.hScrollBar.Value - 1, 0);
                int num12 = Math.Min((this.hScrollBar.Value + this.pictureBoxGraph.Width) - 40, this.m_graphPoints.Length);
                Point point = new Point(40, this.m_graphPoints[index]);
                Point point2 = new Point(40, 0);
                while (index < num12) {
                    point2.Y = this.m_graphPoints[index];
                    if ((index != 0) && ((point2.Y != num9) || (point.Y != num9))) {
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

        private void Recalc() {
            this.m_maxAmps = float.MinValue;
            int length = this.m_graphPoints.Length;
            int volts = this.GetVolts();
            if (volts != 0) {
                float num2;
                int num4;
                int num6;
                float num7;
                int num9;
                int count = this.m_sequence.Channels.Count;
                float[] numArray = new float[count];
                for (int i = 0; i < count; i++) {
                    numArray[i] = Convert.ToSingle(this.m_channelWatts[i]);
                }
                for (num9 = 0; num9 < length; num9++) {
                    num2 = 0f;
                    num4 = 0;
                    while (num4 < count) {
                        num6 = this.m_sequence.EventValues[num4, num9];
                        num2 += (num6 > 0) ? ((((float)num6) / 255f) * numArray[num4]) : 0f;
                        num4++;
                    }
                    num7 = num2 / ((float)volts);
                    this.m_maxAmps = Math.Max(this.m_maxAmps, num7);
                }
                if (!((this.m_maxAmps % 5f) == 0f)) {
                    this.m_maxAmps = (((int)(this.m_maxAmps + 5f)) / 5) * 5;
                }
                int num10 = (this.pictureBoxGraph.Height - 30) - 12;
                for (num9 = 0; num9 < length; num9++) {
                    num2 = 0f;
                    for (num4 = 0; num4 < count; num4++) {
                        num6 = this.m_sequence.EventValues[num4, num9];
                        num2 += (num6 > 0) ? ((((float)num6) / 255f) * numArray[num4]) : 0f;
                    }
                    num7 = num2 / ((float)volts);
                    this.m_graphPoints[num9] = (int)((num10 - ((num7 / this.m_maxAmps) * num10)) + 12f);
                }
                this.hScrollBar.Maximum = this.m_graphPoints.Length;
                this.hScrollBar.LargeChange = this.pictureBoxGraph.Width - 40;
                this.hScrollBar.Enabled = (this.m_graphPoints.Length + 40) > this.pictureBoxGraph.Width;
            }
        }

        private void textBoxChannelAmps_Leave(object sender, EventArgs e) {
            this.UpdateChannelRating((Control)sender);
        }

        private void textBoxChannelWatts_Leave(object sender, EventArgs e) {
            this.UpdateChannelRating((Control)sender);
        }

        private void textBoxMaxAmps_Leave(object sender, EventArgs e) {
            this.m_currentGroupNode.Attributes["maxAmps"].Value = this.textBoxMaxAmps.Text;
        }

        private void textBoxVolts_Leave(object sender, EventArgs e) {
            this.m_dataNode.SelectSingleNode("Volts").InnerText = this.textBoxVolts.Text;
        }

        private void UpdateChannelRating(Control updatingControl) {
            if (this.checkedListBoxChannels.SelectedIndex != -1) {
                string text;
                if (updatingControl == this.textBoxChannelWatts) {
                    text = this.textBoxChannelWatts.Text;
                    try {
                        this.textBoxChannelAmps.Text = (Convert.ToSingle(text) / ((float)Convert.ToInt32(this.textBoxVolts.Text))).ToString("f2");
                    }
                    catch {
                        MessageBox.Show("Make sure that the watts and volts are a valid integer value.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        base.ActiveControl = this.textBoxChannelWatts;
                        return;
                    }
                }
                else {
                    try {
                        text = ((int)Math.Round((double)(Convert.ToSingle(this.textBoxChannelAmps.Text) * Convert.ToInt32(this.textBoxVolts.Text)), 0)).ToString();
                    }
                    catch {
                        MessageBox.Show("Make sure that the amps are a valid decimal value and volts is a valid integer value.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        base.ActiveControl = this.textBoxChannelAmps;
                        return;
                    }
                    this.textBoxChannelWatts.Text = text;
                }
                this.m_channelWatts[this.checkedListBoxChannels.SelectedIndex] = text;
            }
        }

        private void UpdateGroupChannels(XmlNode groupNode) {
            StringBuilder builder = new StringBuilder();
            foreach (int num in this.checkedListBoxChannels.CheckedIndices) {
                builder.Append(num.ToString() + ",");
            }
            if (builder.Length > 0) {
                groupNode.InnerText = builder.ToString().Substring(0, builder.Length - 1);
            }
            else {
                groupNode.InnerText = string.Empty;
            }
        }
    }
}
