namespace DC_16
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    internal class frmSetupDialog : Form
    {
        private Button btnCancel;
        private Button btnOK;
        private ComboBox cboGrp1Addr;
        private ComboBox cboGrp2Addr;
        private ComboBox cboGrp3Addr;
        private ComboBox cboGrp4Addr;
        private IContainer components;
        private GroupBox grpChannels;
        private GroupBox grpPort;
        private GroupBox grpThreshold;
        private Label lblBaudNote;
        private Label lblChGrp1;
        private Label lblChGrp2;
        private Label lblChGrp3;
        private Label lblChGrp4;
        private Label lblPort;
        private Label lblThresh;
        private XmlNode m_setupNode;
        private NumericUpDown nudPort;
        private NumericUpDown nudThreshold;

        public frmSetupDialog(XmlNode setupNode)
        {
            int num;
            this.components = null;
            this.InitializeComponent();
            this.m_setupNode = setupNode;
            string innerText = Xml.GetNodeAlways(this.m_setupNode, "Name").InnerText;
            this.nudPort.Value = (innerText.Length > 3) ? int.Parse(innerText.Substring(3)) : 1;
            if (int.TryParse(Xml.GetNodeAlways(this.m_setupNode, "Threshold").InnerText, out num))
            {
                this.nudThreshold.Value = num;
            }
            if (int.TryParse(Xml.GetNodeAlways(this.m_setupNode, "Group1").InnerText, out num))
            {
                this.cboGrp1Addr.SelectedIndex = num;
            }
            if (int.TryParse(Xml.GetNodeAlways(this.m_setupNode, "Group2").InnerText, out num))
            {
                this.cboGrp2Addr.SelectedIndex = num;
            }
            if (int.TryParse(Xml.GetNodeAlways(this.m_setupNode, "Group3").InnerText, out num))
            {
                this.cboGrp3Addr.SelectedIndex = num;
            }
            if (int.TryParse(Xml.GetNodeAlways(this.m_setupNode, "Group4").InnerText, out num))
            {
                this.cboGrp4Addr.SelectedIndex = num;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (((((this.cboGrp1Addr.SelectedIndex == -1) && (this.cboGrp2Addr.SelectedIndex == -1)) && (this.cboGrp3Addr.SelectedIndex == -1)) && (this.cboGrp4Addr.SelectedIndex == -1)) && (MessageBox.Show("No controllers have been selected to receive any of channel groupings.\nAre you sure that this is what you want to do?", "Vixen", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes))
            {
                base.DialogResult = DialogResult.None;
            }
            else
            {
                int num = (int) this.nudPort.Value;
                Xml.SetValue(this.m_setupNode, "Name", "COM" + num.ToString());
                Xml.SetValue(this.m_setupNode, "Threshold", ((int) this.nudThreshold.Value).ToString());
                Xml.SetValue(this.m_setupNode, "Group1", this.cboGrp1Addr.SelectedIndex.ToString());
                Xml.SetValue(this.m_setupNode, "Group2", this.cboGrp2Addr.SelectedIndex.ToString());
                Xml.SetValue(this.m_setupNode, "Group3", this.cboGrp3Addr.SelectedIndex.ToString());
                Xml.SetValue(this.m_setupNode, "Group4", this.cboGrp4Addr.SelectedIndex.ToString());
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
            this.grpPort = new GroupBox();
            this.lblBaudNote = new Label();
            this.nudPort = new NumericUpDown();
            this.lblPort = new Label();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.grpThreshold = new GroupBox();
            this.nudThreshold = new NumericUpDown();
            this.lblThresh = new Label();
            this.grpChannels = new GroupBox();
            this.cboGrp4Addr = new ComboBox();
            this.lblChGrp4 = new Label();
            this.cboGrp3Addr = new ComboBox();
            this.lblChGrp3 = new Label();
            this.cboGrp2Addr = new ComboBox();
            this.lblChGrp2 = new Label();
            this.cboGrp1Addr = new ComboBox();
            this.lblChGrp1 = new Label();
            this.grpPort.SuspendLayout();
            this.nudPort.BeginInit();
            this.grpThreshold.SuspendLayout();
            this.nudThreshold.BeginInit();
            this.grpChannels.SuspendLayout();
            base.SuspendLayout();
            this.grpPort.Controls.Add(this.lblBaudNote);
            this.grpPort.Controls.Add(this.nudPort);
            this.grpPort.Controls.Add(this.lblPort);
            this.grpPort.Location = new Point(12, 12);
            this.grpPort.Name = "grpPort";
            this.grpPort.Size = new Size(0xea, 50);
            this.grpPort.TabIndex = 0;
            this.grpPort.TabStop = false;
            this.grpPort.Text = "Serial Port";
            this.lblBaudNote.AutoSize = true;
            this.lblBaudNote.Location = new Point(0x51, 20);
            this.lblBaudNote.Name = "lblBaudNote";
            this.lblBaudNote.Size = new Size(0x7f, 13);
            this.lblBaudNote.TabIndex = 2;
            this.lblBaudNote.Text = "38.4k - Install B/R jumper";
            this.nudPort.Location = new Point(40, 0x12);
            int[] bits = new int[4];
            bits[0] = 0x63;
            this.nudPort.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.nudPort.Minimum = new decimal(bits);
            this.nudPort.Name = "nudPort";
            this.nudPort.Size = new Size(0x23, 20);
            this.nudPort.TabIndex = 1;
            bits = new int[4];
            bits[0] = 1;
            this.nudPort.Value = new decimal(bits);
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new Point(6, 20);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new Size(0x1f, 13);
            this.lblPort.TabIndex = 0;
            this.lblPort.Text = "COM";
            this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(170, 0x131);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(0x59, 0x131);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.grpThreshold.Controls.Add(this.nudThreshold);
            this.grpThreshold.Controls.Add(this.lblThresh);
            this.grpThreshold.Enabled = false;
            this.grpThreshold.Location = new Point(12, 0x44);
            this.grpThreshold.Name = "grpThreshold";
            this.grpThreshold.Size = new Size(0xea, 0x58);
            this.grpThreshold.TabIndex = 6;
            this.grpThreshold.TabStop = false;
            this.grpThreshold.Text = "Threshold";
            this.nudThreshold.Location = new Point(0x67, 0x35);
            bits = new int[4];
            bits[0] = 1;
            this.nudThreshold.Minimum = new decimal(bits);
            this.nudThreshold.Name = "nudThreshold";
            this.nudThreshold.Size = new Size(0x27, 20);
            this.nudThreshold.TabIndex = 2;
            bits = new int[4];
            bits[0] = 50;
            this.nudThreshold.Value = new decimal(bits);
            this.lblThresh.Location = new Point(6, 0x10);
            this.lblThresh.Name = "lblThresh";
            this.lblThresh.Size = new Size(0xde, 0x22);
            this.lblThresh.TabIndex = 0;
            this.lblThresh.Text = "Select intensity (1-100%) at which a DC-16 channel will be considered \"on.\"";
            this.grpChannels.Controls.Add(this.cboGrp4Addr);
            this.grpChannels.Controls.Add(this.lblChGrp4);
            this.grpChannels.Controls.Add(this.cboGrp3Addr);
            this.grpChannels.Controls.Add(this.lblChGrp3);
            this.grpChannels.Controls.Add(this.cboGrp2Addr);
            this.grpChannels.Controls.Add(this.lblChGrp2);
            this.grpChannels.Controls.Add(this.cboGrp1Addr);
            this.grpChannels.Controls.Add(this.lblChGrp1);
            this.grpChannels.Location = new Point(12, 0xa2);
            this.grpChannels.Name = "grpChannels";
            this.grpChannels.Size = new Size(0xea, 0x89);
            this.grpChannels.TabIndex = 7;
            this.grpChannels.TabStop = false;
            this.grpChannels.Text = "Channel Groups";
            this.cboGrp4Addr.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboGrp4Addr.FormattingEnabled = true;
            this.cboGrp4Addr.Items.AddRange(new object[] { "%00 (0)", "%01 (1)", "%10 (2)", "%11 (3)" });
            this.cboGrp4Addr.Location = new Point(0x95, 100);
            this.cboGrp4Addr.Name = "cboGrp4Addr";
            this.cboGrp4Addr.Size = new Size(70, 0x15);
            this.cboGrp4Addr.TabIndex = 0x2b;
            this.lblChGrp4.AutoSize = true;
            this.lblChGrp4.Location = new Point(10, 0x67);
            this.lblChGrp4.Name = "lblChGrp4";
            this.lblChGrp4.Size = new Size(0x57, 13);
            this.lblChGrp4.TabIndex = 0x29;
            this.lblChGrp4.Text = "Channels 49 - 64";
            this.cboGrp3Addr.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboGrp3Addr.FormattingEnabled = true;
            this.cboGrp3Addr.Items.AddRange(new object[] { "%00 (0)", "%01 (1)", "%10 (2)", "%11 (3)" });
            this.cboGrp3Addr.Location = new Point(0x95, 0x49);
            this.cboGrp3Addr.Name = "cboGrp3Addr";
            this.cboGrp3Addr.Size = new Size(70, 0x15);
            this.cboGrp3Addr.TabIndex = 40;
            this.lblChGrp3.AutoSize = true;
            this.lblChGrp3.Location = new Point(10, 0x4c);
            this.lblChGrp3.Name = "lblChGrp3";
            this.lblChGrp3.Size = new Size(0x57, 13);
            this.lblChGrp3.TabIndex = 0x26;
            this.lblChGrp3.Text = "Channels 33 - 48";
            this.cboGrp2Addr.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboGrp2Addr.FormattingEnabled = true;
            this.cboGrp2Addr.Items.AddRange(new object[] { "%00 (0)", "%01 (1)", "%10 (2)", "%11 (3)" });
            this.cboGrp2Addr.Location = new Point(0x95, 0x2e);
            this.cboGrp2Addr.Name = "cboGrp2Addr";
            this.cboGrp2Addr.Size = new Size(70, 0x15);
            this.cboGrp2Addr.TabIndex = 0x25;
            this.lblChGrp2.AutoSize = true;
            this.lblChGrp2.Location = new Point(10, 0x31);
            this.lblChGrp2.Name = "lblChGrp2";
            this.lblChGrp2.Size = new Size(0x57, 13);
            this.lblChGrp2.TabIndex = 0x23;
            this.lblChGrp2.Text = "Channels 17 - 32";
            this.cboGrp1Addr.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboGrp1Addr.FormattingEnabled = true;
            this.cboGrp1Addr.Items.AddRange(new object[] { "%00 (0)", "%01 (1)", "%10 (2)", "%11 (3)" });
            this.cboGrp1Addr.Location = new Point(0x95, 0x13);
            this.cboGrp1Addr.Name = "cboGrp1Addr";
            this.cboGrp1Addr.Size = new Size(70, 0x15);
            this.cboGrp1Addr.TabIndex = 0x22;
            this.lblChGrp1.AutoSize = true;
            this.lblChGrp1.Location = new Point(10, 0x16);
            this.lblChGrp1.Name = "lblChGrp1";
            this.lblChGrp1.Size = new Size(0x51, 13);
            this.lblChGrp1.TabIndex = 0x20;
            this.lblChGrp1.Text = "Channels 1 - 16";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x101, 340);
            base.Controls.Add(this.grpChannels);
            base.Controls.Add(this.grpThreshold);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.grpPort);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmSetupDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Setup";
            this.grpPort.ResumeLayout(false);
            this.grpPort.PerformLayout();
            this.nudPort.EndInit();
            this.grpThreshold.ResumeLayout(false);
            this.nudThreshold.EndInit();
            this.grpChannels.ResumeLayout(false);
            this.grpChannels.PerformLayout();
            base.ResumeLayout(false);
        }
    }
}

