namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Xml;

    internal class ChannelRangeFixDialog : Form
    {
        private Button buttonCancel;
        private Button buttonOK;
        private IContainer components = null;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label labelChannelCount;
        private ListBox listBoxPlugIns;
        private XmlDocument m_doc;
        private int m_lastIndex = -1;
        private TextBox textBoxFrom;
        private TextBox textBoxTo;

        public ChannelRangeFixDialog(XmlDocument doc)
        {
            this.InitializeComponent();
            this.m_doc = doc;
            this.labelChannelCount.Text = this.m_doc.SelectNodes("//Program/Channels/Channel").Count.ToString();
            foreach (XmlNode node in this.m_doc.SelectNodes("//Program/PlugInData/PlugIn"))
            {
                this.listBoxPlugIns.Items.Add(new PlugInMapping(node));
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

        public int From(int plugInIndex)
        {
            if (plugInIndex < this.listBoxPlugIns.Items.Count)
            {
                return ((PlugInMapping) this.listBoxPlugIns.Items[plugInIndex]).From;
            }
            return 0;
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new GroupBox();
            this.textBoxTo = new TextBox();
            this.label4 = new Label();
            this.textBoxFrom = new TextBox();
            this.label3 = new Label();
            this.listBoxPlugIns = new ListBox();
            this.labelChannelCount = new Label();
            this.label2 = new Label();
            this.label1 = new Label();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.textBoxTo);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBoxFrom);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.listBoxPlugIns);
            this.groupBox1.Controls.Add(this.labelChannelCount);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x10c, 0x111);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Channel Range";
            this.textBoxTo.Location = new Point(0xd1, 0x76);
            this.textBoxTo.MaxLength = 4;
            this.textBoxTo.Name = "textBoxTo";
            this.textBoxTo.Size = new Size(0x30, 20);
            this.textBoxTo.TabIndex = 7;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0xb2, 0x79);
            this.label4.Name = "label4";
            this.label4.Size = new Size(20, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "To";
            this.textBoxFrom.Location = new Point(0xd1, 0x5c);
            this.textBoxFrom.MaxLength = 4;
            this.textBoxFrom.Name = "textBoxFrom";
            this.textBoxFrom.Size = new Size(0x30, 20);
            this.textBoxFrom.TabIndex = 5;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0xb2, 0x5f);
            this.label3.Name = "label3";
            this.label3.Size = new Size(30, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "From";
            this.listBoxPlugIns.FormattingEnabled = true;
            this.listBoxPlugIns.Location = new Point(13, 0x5f);
            this.listBoxPlugIns.Name = "listBoxPlugIns";
            this.listBoxPlugIns.Size = new Size(0x9f, 160);
            this.listBoxPlugIns.TabIndex = 3;
            this.listBoxPlugIns.SelectedIndexChanged += new EventHandler(this.listBoxPlugIns_SelectedIndexChanged);
            this.labelChannelCount.AutoSize = true;
            this.labelChannelCount.Location = new Point(0x92, 0x41);
            this.labelChannelCount.Name = "labelChannelCount";
            this.labelChannelCount.Size = new Size(0, 13);
            this.labelChannelCount.TabIndex = 2;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(10, 0x41);
            this.label2.Name = "label2";
            this.label2.Size = new Size(130, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Sequence channel count:";
            this.label1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.label1.Location = new Point(10, 0x16);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0xf7, 0x2b);
            this.label1.TabIndex = 0;
            this.label1.Text = "The channel range for a plugin should not exceed the number of channels in a sequence.";
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(0x7c, 0x123);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0xcd, 0x123);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            base.AcceptButton = this.buttonOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x124, 0x146);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "ChannelRangeFixDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Channel Range";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
        }

        private void listBoxPlugIns_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.m_lastIndex != -1)
            {
                PlugInMapping mapping = (PlugInMapping) this.listBoxPlugIns.Items[this.m_lastIndex];
                try
                {
                    mapping.From = Convert.ToInt32(this.textBoxFrom.Text);
                }
                catch
                {
                    mapping.From = 0;
                }
                try
                {
                    mapping.To = Convert.ToInt32(this.textBoxTo.Text);
                }
                catch
                {
                    mapping.To = 0;
                }
            }
            if (this.listBoxPlugIns.SelectedItem != null)
            {
                PlugInMapping selectedItem = (PlugInMapping) this.listBoxPlugIns.SelectedItem;
                this.textBoxFrom.Text = selectedItem.From.ToString();
                this.textBoxTo.Text = selectedItem.To.ToString();
            }
        }

        public int To(int plugInIndex)
        {
            if (plugInIndex < this.listBoxPlugIns.Items.Count)
            {
                return ((PlugInMapping) this.listBoxPlugIns.Items[plugInIndex]).To;
            }
            return 0;
        }
    }
}

