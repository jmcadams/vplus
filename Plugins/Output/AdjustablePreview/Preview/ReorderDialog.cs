namespace Preview
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Vixen;

    internal class ReorderDialog : Form
    {
        private Button buttonCancel;
        private Button buttonClear;
        private Button buttonCopy;
        private Button buttonOK;
        private ComboBox comboBoxFrom;
        private ComboBox comboBoxTo;
        private IContainer components = null;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Dictionary<int, List<uint>> m_channelDictionary = new Dictionary<int, List<uint>>();

        public ReorderDialog(List<Channel> channels, Dictionary<int, List<uint>> channelDictionary)
        {
            this.InitializeComponent();
            foreach (KeyValuePair<int, List<uint>> pair in channelDictionary)
            {
                List<uint> list;
                this.m_channelDictionary[pair.Key] = list = new List<uint>();
                list.AddRange(pair.Value);
            }
            Channel[] items = channels.ToArray();
            this.comboBoxFrom.Items.AddRange(items);
            this.comboBoxTo.Items.AddRange(items);
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            if (this.comboBoxTo.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a channel to clear.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                this.m_channelDictionary.Remove(this.comboBoxTo.SelectedIndex);
                MessageBox.Show(string.Format("Channel '{0}' has been cleared.", (Channel) this.comboBoxTo.SelectedItem), "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            if ((this.comboBoxFrom.SelectedIndex == -1) || (this.comboBoxTo.SelectedIndex == -1))
            {
                MessageBox.Show("Please select channels to copy from and to.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                List<uint> list = null;
                if (!this.m_channelDictionary.TryGetValue(this.comboBoxFrom.SelectedIndex, out list))
                {
                    MessageBox.Show(string.Format("{0} has no cells drawn.", (Channel) this.comboBoxFrom.SelectedItem), "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    List<uint> list2 = null;
                    if (!this.m_channelDictionary.TryGetValue(this.comboBoxTo.SelectedIndex, out list2))
                    {
                        list2 = new List<uint>();
                        this.m_channelDictionary[this.comboBoxTo.SelectedIndex] = list2;
                    }
                    else
                    {
                        list2.Clear();
                    }
                    list2.AddRange(list);
                    MessageBox.Show(string.Format("Channel '{0}' has been copied to channel '{1}'.", (Channel) this.comboBoxFrom.SelectedItem, (Channel) this.comboBoxTo.SelectedItem), "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
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
            this.label1 = new Label();
            this.label2 = new Label();
            this.comboBoxFrom = new ComboBox();
            this.label3 = new Label();
            this.comboBoxTo = new ComboBox();
            this.buttonClear = new Button();
            this.buttonCopy = new Button();
            this.groupBox1 = new GroupBox();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(20, 0x1a);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0xf1, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Copy the cells drawn from one channel to another";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(20, 0x41);
            this.label2.Name = "label2";
            this.label2.Size = new Size(30, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "From";
            this.comboBoxFrom.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxFrom.FormattingEnabled = true;
            this.comboBoxFrom.Location = new Point(0x4c, 0x3e);
            this.comboBoxFrom.Name = "comboBoxFrom";
            this.comboBoxFrom.Size = new Size(0xb9, 0x15);
            this.comboBoxFrom.TabIndex = 2;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x19, 0x67);
            this.label3.Name = "label3";
            this.label3.Size = new Size(20, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "To";
            this.comboBoxTo.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxTo.FormattingEnabled = true;
            this.comboBoxTo.Location = new Point(0x4c, 100);
            this.comboBoxTo.Name = "comboBoxTo";
            this.comboBoxTo.Size = new Size(0xb9, 0x15);
            this.comboBoxTo.TabIndex = 4;
            this.buttonClear.Location = new Point(0x4c, 160);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new Size(0x75, 0x17);
            this.buttonClear.TabIndex = 5;
            this.buttonClear.Text = "Clear this channel";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new EventHandler(this.buttonClear_Click);
            this.buttonCopy.Location = new Point(0x4c, 0x83);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new Size(0x75, 0x17);
            this.buttonCopy.TabIndex = 6;
            this.buttonCopy.Text = "Copy to this channel";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new EventHandler(this.buttonCopy_Click);
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.buttonCopy);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.buttonClear);
            this.groupBox1.Controls.Add(this.comboBoxFrom);
            this.groupBox1.Controls.Add(this.comboBoxTo);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x11d, 0xc9);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Copy Cells";
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(0x8d, 0xe2);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 8;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0xde, 0xe2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 9;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x135, 0x105);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "ReorderDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Copy";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
        }

        public Dictionary<int, List<uint>> ChannelDictionary
        {
            get
            {
                return this.m_channelDictionary;
            }
        }
    }
}

