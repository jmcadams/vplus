namespace HorningDimmer
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;

    public class DimmingDialog : Form
    {
        private Button buttonAll;
        private Button buttonCancel;
        private Button buttonNone;
        private Button buttonOK;
        private Button buttonRange;
        private CheckedListBox checkedListBoxChannels;
        private IContainer components = null;
        private Label label1;
        private int m_from = 0;
        private XmlElement m_portElement;
        private int m_to = 0;

        public DimmingDialog()
        {
            this.InitializeComponent();
        }

        private void buttonAll_Click(object sender, EventArgs e)
        {
            this.checkedListBoxChannels.BeginUpdate();
            for (int i = 0; i < this.checkedListBoxChannels.Items.Count; i++)
            {
                this.checkedListBoxChannels.SetItemChecked(i, true);
            }
            this.checkedListBoxChannels.EndUpdate();
        }

        private void buttonNone_Click(object sender, EventArgs e)
        {
            this.checkedListBoxChannels.BeginUpdate();
            for (int i = 0; i < this.checkedListBoxChannels.Items.Count; i++)
            {
                this.checkedListBoxChannels.SetItemChecked(i, false);
            }
            this.checkedListBoxChannels.EndUpdate();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            foreach (int num in this.checkedListBoxChannels.CheckedIndices)
            {
                builder.AppendFormat("{0},", num + this.m_from);
            }
            XmlNode node = this.m_portElement.SelectSingleNode("dimming");
            if (builder.Length > 0)
            {
                node.InnerText = builder.ToString().Substring(0, builder.Length - 1);
            }
            else
            {
                node.InnerText = string.Empty;
            }
        }

        private void buttonRange_Click(object sender, EventArgs e)
        {
            RangeDialog dialog = new RangeDialog(this.m_from, this.m_to);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                bool selected = dialog.Selected;
                this.checkedListBoxChannels.BeginUpdate();
                for (int i = dialog.From; i <= dialog.To; i++)
                {
                    this.checkedListBoxChannels.SetItemChecked(i - this.m_from, selected);
                }
                this.checkedListBoxChannels.EndUpdate();
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
            this.checkedListBoxChannels = new CheckedListBox();
            this.buttonCancel = new Button();
            this.buttonAll = new Button();
            this.buttonNone = new Button();
            this.buttonRange = new Button();
            this.buttonOK = new Button();
            base.SuspendLayout();
            this.label1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.label1.Location = new Point(15, 14);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x15a, 0x1c);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select the channels that are dimmable according to the hardware configuration.";
            this.checkedListBoxChannels.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.checkedListBoxChannels.CheckOnClick = true;
            this.checkedListBoxChannels.FormattingEnabled = true;
            this.checkedListBoxChannels.Location = new Point(15, 0x33);
            this.checkedListBoxChannels.Name = "checkedListBoxChannels";
            this.checkedListBoxChannels.Size = new Size(0x159, 0xb8);
            this.checkedListBoxChannels.TabIndex = 1;
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x11d, 0x111);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonAll.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.buttonAll.Location = new Point(0x10, 240);
            this.buttonAll.Name = "buttonAll";
            this.buttonAll.Size = new Size(0x4b, 0x17);
            this.buttonAll.TabIndex = 2;
            this.buttonAll.Text = "All";
            this.buttonAll.UseVisualStyleBackColor = true;
            this.buttonAll.Click += new EventHandler(this.buttonAll_Click);
            this.buttonNone.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.buttonNone.Location = new Point(0x61, 240);
            this.buttonNone.Name = "buttonNone";
            this.buttonNone.Size = new Size(0x4b, 0x17);
            this.buttonNone.TabIndex = 3;
            this.buttonNone.Text = "None";
            this.buttonNone.UseVisualStyleBackColor = true;
            this.buttonNone.Click += new EventHandler(this.buttonNone_Click);
            this.buttonRange.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.buttonRange.Location = new Point(0xb2, 240);
            this.buttonRange.Name = "buttonRange";
            this.buttonRange.Size = new Size(0x4b, 0x17);
            this.buttonRange.TabIndex = 4;
            this.buttonRange.Text = "Range";
            this.buttonRange.UseVisualStyleBackColor = true;
            this.buttonRange.Click += new EventHandler(this.buttonRange_Click);
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(0xcc, 0x111);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            base.AcceptButton = this.buttonOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x175, 0x134);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.buttonRange);
            base.Controls.Add(this.buttonNone);
            base.Controls.Add(this.buttonAll);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.checkedListBoxChannels);
            base.Controls.Add(this.label1);
            base.Name = "DimmingDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Dimming Configuration";
            base.ResumeLayout(false);
        }

        public XmlElement PortElement
        {
            set
            {
                this.m_portElement = value;
                this.m_from = Convert.ToInt32(this.m_portElement.Attributes["from"].Value);
                this.m_to = Convert.ToInt32(this.m_portElement.Attributes["to"].Value);
                this.checkedListBoxChannels.BeginUpdate();
                this.checkedListBoxChannels.Items.Clear();
                for (int i = this.m_from; i <= this.m_to; i++)
                {
                    this.checkedListBoxChannels.Items.Add("Channel " + i.ToString());
                }
                this.checkedListBoxChannels.EndUpdate();
                XmlNode node = this.m_portElement.SelectSingleNode("dimming");
                foreach (string str in node.InnerText.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    int num2 = Convert.ToInt32(str);
                    if (((num2 - this.m_from) >= 0) && ((num2 - this.m_from) < this.checkedListBoxChannels.Items.Count))
                    {
                        this.checkedListBoxChannels.SetItemChecked(num2 - this.m_from, true);
                    }
                }
            }
        }
    }
}

