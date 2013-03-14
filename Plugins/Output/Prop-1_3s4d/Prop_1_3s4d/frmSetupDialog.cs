namespace Prop_1_3s4d
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
        private ComboBox comboBoxBaud;
        private IContainer components = null;
        private GroupBox grpAdvanced;
        private GroupBox grpPort;
        private Label lblAnaMax;
        private Label lblAnaMin;
        private Label lblBaud;
        private Label lblPercent;
        private Label lblPort;
        private Label lblThreshold;
        private XmlNode m_setupNode;
        private NumericUpDown nudAnaMax;
        private NumericUpDown nudAnaMin;
        private NumericUpDown nudPort;
        private NumericUpDown numericUpDownThreshold;

        public frmSetupDialog(XmlNode setupNode)
        {
            this.InitializeComponent();
            this.m_setupNode = setupNode;
            string innerText = Xml.GetNodeAlways(this.m_setupNode, "name").InnerText;
            this.nudPort.Value = (innerText.Length > 3) ? int.Parse(innerText.Substring(3)) : 1;
            innerText = Xml.GetNodeAlways(this.m_setupNode, "threshold").InnerText;
            this.numericUpDownThreshold.Value = (innerText.Length > 0) ? int.Parse(innerText) : 50;
            innerText = Xml.GetNodeAlways(this.m_setupNode, "analogMin").InnerText;
            this.nudAnaMin.Value = (innerText.Length > 0) ? int.Parse(innerText) : 100;
            innerText = Xml.GetNodeAlways(this.m_setupNode, "analogMax").InnerText;
            this.nudAnaMax.Value = (innerText.Length > 0) ? int.Parse(innerText) : 200;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.nudAnaMin.Value >= this.nudAnaMax.Value)
            {
                MessageBox.Show("Minimum must be below the maximum.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                base.DialogResult = DialogResult.None;
            }
            else
            {
                int num = (int) this.nudPort.Value;
                Xml.SetValue(this.m_setupNode, "name", "COM" + num.ToString());
                num = (int) this.numericUpDownThreshold.Value;
                Xml.SetValue(this.m_setupNode, "threshold", num.ToString());
                num = (int) this.nudAnaMin.Value;
                Xml.SetValue(this.m_setupNode, "analogMin", num.ToString());
                Xml.SetValue(this.m_setupNode, "analogMax", ((int) this.nudAnaMax.Value).ToString());
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
            this.comboBoxBaud = new ComboBox();
            this.lblBaud = new Label();
            this.nudPort = new NumericUpDown();
            this.lblPort = new Label();
            this.grpAdvanced = new GroupBox();
            this.lblPercent = new Label();
            this.lblAnaMax = new Label();
            this.lblAnaMin = new Label();
            this.nudAnaMax = new NumericUpDown();
            this.nudAnaMin = new NumericUpDown();
            this.numericUpDownThreshold = new NumericUpDown();
            this.lblThreshold = new Label();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.grpPort.SuspendLayout();
            this.nudPort.BeginInit();
            this.grpAdvanced.SuspendLayout();
            this.nudAnaMax.BeginInit();
            this.nudAnaMin.BeginInit();
            this.numericUpDownThreshold.BeginInit();
            base.SuspendLayout();
            this.grpPort.Controls.Add(this.comboBoxBaud);
            this.grpPort.Controls.Add(this.lblBaud);
            this.grpPort.Controls.Add(this.nudPort);
            this.grpPort.Controls.Add(this.lblPort);
            this.grpPort.Location = new Point(12, 12);
            this.grpPort.Name = "grpPort";
            this.grpPort.Size = new Size(0xdf, 50);
            this.grpPort.TabIndex = 0;
            this.grpPort.TabStop = false;
            this.grpPort.Text = "Serial Port";
            this.comboBoxBaud.Enabled = false;
            this.comboBoxBaud.FormattingEnabled = true;
            this.comboBoxBaud.Items.AddRange(new object[] { "38400", "19200", "9600", "2400" });
            this.comboBoxBaud.Location = new Point(0x94, 0x11);
            this.comboBoxBaud.Name = "comboBoxBaud";
            this.comboBoxBaud.Size = new Size(0x3f, 0x15);
            this.comboBoxBaud.TabIndex = 3;
            this.comboBoxBaud.Text = "2400";
            this.lblBaud.AutoSize = true;
            this.lblBaud.Location = new Point(0x6a, 20);
            this.lblBaud.Name = "lblBaud";
            this.lblBaud.Size = new Size(0x20, 13);
            this.lblBaud.TabIndex = 2;
            this.lblBaud.Text = "Baud";
            this.nudPort.Location = new Point(0x2b, 0x12);
            int[] bits = new int[4];
            bits[0] = 4;
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
            this.grpAdvanced.Controls.Add(this.lblPercent);
            this.grpAdvanced.Controls.Add(this.lblAnaMax);
            this.grpAdvanced.Controls.Add(this.lblAnaMin);
            this.grpAdvanced.Controls.Add(this.nudAnaMax);
            this.grpAdvanced.Controls.Add(this.nudAnaMin);
            this.grpAdvanced.Controls.Add(this.numericUpDownThreshold);
            this.grpAdvanced.Controls.Add(this.lblThreshold);
            this.grpAdvanced.Location = new Point(12, 0x44);
            this.grpAdvanced.Name = "grpAdvanced";
            this.grpAdvanced.Size = new Size(0xdf, 0x66);
            this.grpAdvanced.TabIndex = 2;
            this.grpAdvanced.TabStop = false;
            this.grpAdvanced.Text = "Advanced";
            this.lblPercent.AutoSize = true;
            this.lblPercent.Location = new Point(0xc4, 20);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.Size = new Size(15, 13);
            this.lblPercent.TabIndex = 7;
            this.lblPercent.Text = "%";
            this.lblAnaMax.AutoSize = true;
            this.lblAnaMax.Location = new Point(6, 0x48);
            this.lblAnaMax.Name = "lblAnaMax";
            this.lblAnaMax.Size = new Size(0x80, 13);
            this.lblAnaMax.TabIndex = 6;
            this.lblAnaMax.Text = "Servo Max (100%) Output";
            this.lblAnaMin.AutoSize = true;
            this.lblAnaMin.Location = new Point(6, 0x2e);
            this.lblAnaMin.Name = "lblAnaMin";
            this.lblAnaMin.Size = new Size(0x71, 13);
            this.lblAnaMin.TabIndex = 5;
            this.lblAnaMin.Text = "Servo Min (0%) Output";
            this.nudAnaMax.Location = new Point(0x94, 70);
            bits = new int[4];
            bits[0] = 250;
            this.nudAnaMax.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 100;
            this.nudAnaMax.Minimum = new decimal(bits);
            this.nudAnaMax.Name = "nudAnaMax";
            this.nudAnaMax.Size = new Size(0x2a, 20);
            this.nudAnaMax.TabIndex = 4;
            bits = new int[4];
            bits[0] = 200;
            this.nudAnaMax.Value = new decimal(bits);
            this.nudAnaMin.Location = new Point(0x94, 0x2c);
            bits = new int[4];
            bits[0] = 200;
            this.nudAnaMin.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 50;
            this.nudAnaMin.Minimum = new decimal(bits);
            this.nudAnaMin.Name = "nudAnaMin";
            this.nudAnaMin.Size = new Size(0x2a, 20);
            this.nudAnaMin.TabIndex = 3;
            bits = new int[4];
            bits[0] = 100;
            this.nudAnaMin.Value = new decimal(bits);
            this.numericUpDownThreshold.Location = new Point(0x94, 0x12);
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDownThreshold.Minimum = new decimal(bits);
            this.numericUpDownThreshold.Name = "numericUpDownThreshold";
            this.numericUpDownThreshold.Size = new Size(0x2a, 20);
            this.numericUpDownThreshold.TabIndex = 2;
            bits = new int[4];
            bits[0] = 50;
            this.numericUpDownThreshold.Value = new decimal(bits);
            this.lblThreshold.AutoSize = true;
            this.lblThreshold.Location = new Point(6, 20);
            this.lblThreshold.Name = "lblThreshold";
            this.lblThreshold.Size = new Size(0x56, 13);
            this.lblThreshold.TabIndex = 0;
            this.lblThreshold.Text = "Digital Threshold";
            this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0xa1, 0xb1);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(80, 0xb1);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0xf8, 0xd4);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.grpAdvanced);
            base.Controls.Add(this.grpPort);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmSetupDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Setup";
            this.grpPort.ResumeLayout(false);
            this.grpPort.PerformLayout();
            this.nudPort.EndInit();
            this.grpAdvanced.ResumeLayout(false);
            this.grpAdvanced.PerformLayout();
            this.nudAnaMax.EndInit();
            this.nudAnaMin.EndInit();
            this.numericUpDownThreshold.EndInit();
            base.ResumeLayout(false);
        }
    }
}

