namespace RC_4
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class frmEfxSetup : Form
    {
        private Button btnCancel;
        private Button btnOK;
        private ComboBox cboAddress;
        private ComboBox cboBaud;
        private IContainer components = null;
        private GroupBox grpPort;
        private GroupBox grpThresh;
        private ImageList imgLstSetup;
        private Label lblAddr;
        private Label lblBaud;
        private Label lblPort;
        private Label lblThreshNote;
        private XmlNode m_setupNode;
        private NumericUpDown nudPort;
        private NumericUpDown nudThreshold;
        private PictureBox picSetup;

        public frmEfxSetup(XmlNode setupNode, bool disableThreshold)
        {
            this.InitializeComponent();
            this.m_setupNode = setupNode;
            string innerText = Xml.GetNodeAlways(this.m_setupNode, "name").InnerText;
            this.nudPort.Value = (innerText.Length > 3) ? int.Parse(innerText.Substring(3)) : 1;
            innerText = Xml.GetNodeAlways(this.m_setupNode, "baud").InnerText;
            this.cboBaud.SelectedIndex = this.cboBaud.Items.IndexOf(innerText);
            innerText = Xml.GetNodeAlways(this.m_setupNode, "addr").InnerText;
            this.cboAddress.SelectedIndex = int.Parse(innerText);
            if (!disableThreshold)
            {
                innerText = Xml.GetNodeAlways(this.m_setupNode, "threshold").InnerText;
                this.nudThreshold.Value = int.Parse(innerText);
            }
            else
            {
                this.grpThresh.Enabled = false;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if ((this.cboBaud.SelectedIndex == -1) || (this.cboAddress.SelectedIndex == -1))
            {
                MessageBox.Show("Please select both a baud rate and an address.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                base.DialogResult = DialogResult.None;
            }
            else
            {
                int num = (int) this.nudPort.Value;
                Xml.SetValue(this.m_setupNode, "name", "COM" + num.ToString());
                Xml.SetValue(this.m_setupNode, "baud", this.cboBaud.Text.ToString());
                Xml.SetValue(this.m_setupNode, "addr", this.cboAddress.SelectedIndex.ToString());
                if (this.grpThresh.Enabled)
                {
                    Xml.SetValue(this.m_setupNode, "threshold", ((int) this.nudThreshold.Value).ToString());
                }
            }
        }

        private void cboAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateImage();
        }

        private void cboBaud_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateImage();
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
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmEfxSetup));
            this.grpThresh = new GroupBox();
            this.nudThreshold = new NumericUpDown();
            this.lblThreshNote = new Label();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.grpPort = new GroupBox();
            this.picSetup = new PictureBox();
            this.cboAddress = new ComboBox();
            this.lblAddr = new Label();
            this.cboBaud = new ComboBox();
            this.lblBaud = new Label();
            this.nudPort = new NumericUpDown();
            this.lblPort = new Label();
            this.imgLstSetup = new ImageList(this.components);
            this.grpThresh.SuspendLayout();
            this.nudThreshold.BeginInit();
            this.grpPort.SuspendLayout();
            ((ISupportInitialize) this.picSetup).BeginInit();
            this.nudPort.BeginInit();
            base.SuspendLayout();
            this.grpThresh.Controls.Add(this.nudThreshold);
            this.grpThresh.Controls.Add(this.lblThreshNote);
            this.grpThresh.Location = new Point(12, 0x7b);
            this.grpThresh.Name = "grpThresh";
            this.grpThresh.Size = new Size(0xdf, 0x5d);
            this.grpThresh.TabIndex = 1;
            this.grpThresh.TabStop = false;
            this.grpThresh.Text = "Threshold";
            this.nudThreshold.Location = new Point(0x57, 0x3a);
            this.nudThreshold.Name = "nudThreshold";
            this.nudThreshold.Size = new Size(0x30, 20);
            this.nudThreshold.TabIndex = 1;
            this.nudThreshold.TextAlign = HorizontalAlignment.Right;
            int[] bits = new int[4];
            bits[0] = 50;
            this.nudThreshold.Value = new decimal(bits);
            this.lblThreshNote.Location = new Point(6, 0x10);
            this.lblThreshNote.Name = "lblThreshNote";
            this.lblThreshNote.Size = new Size(0xd3, 0x27);
            this.lblThreshNote.TabIndex = 0;
            this.lblThreshNote.Text = "Select the intensity (0-100%) level at which a channel will be considered triggered.";
            this.btnOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(0x4f, 0xe3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(160, 0xe3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.grpPort.Controls.Add(this.picSetup);
            this.grpPort.Controls.Add(this.cboAddress);
            this.grpPort.Controls.Add(this.lblAddr);
            this.grpPort.Controls.Add(this.cboBaud);
            this.grpPort.Controls.Add(this.lblBaud);
            this.grpPort.Controls.Add(this.nudPort);
            this.grpPort.Controls.Add(this.lblPort);
            this.grpPort.Location = new Point(12, 12);
            this.grpPort.Name = "grpPort";
            this.grpPort.Size = new Size(0xdf, 0x69);
            this.grpPort.TabIndex = 0;
            this.grpPort.TabStop = false;
            this.grpPort.Text = "Connection";
            this.picSetup.InitialImage = (Image) manager.GetObject("picSetup.InitialImage");
            this.picSetup.Location = new Point(120, 0x12);
            this.picSetup.Name = "picSetup";
            this.picSetup.Size = new Size(90, 0x4a);
            this.picSetup.TabIndex = 6;
            this.picSetup.TabStop = false;
            this.cboAddress.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboAddress.FormattingEnabled = true;
            this.cboAddress.Items.AddRange(new object[] { "0 (%00)", "1 (%01)", "2 (%10)", "3 (%11)" });
            this.cboAddress.Location = new Point(0x2b, 0x47);
            this.cboAddress.MaxDropDownItems = 4;
            this.cboAddress.Name = "cboAddress";
            this.cboAddress.Size = new Size(0x3f, 0x15);
            this.cboAddress.TabIndex = 5;
            this.cboAddress.SelectedIndexChanged += new EventHandler(this.cboAddress_SelectedIndexChanged);
            this.lblAddr.AutoSize = true;
            this.lblAddr.Location = new Point(6, 0x4a);
            this.lblAddr.Name = "lblAddr";
            this.lblAddr.Size = new Size(0x1d, 13);
            this.lblAddr.TabIndex = 4;
            this.lblAddr.Text = "Addr";
            this.cboBaud.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboBaud.FormattingEnabled = true;
            this.cboBaud.Items.AddRange(new object[] { "38400", "2400" });
            this.cboBaud.Location = new Point(0x2b, 0x2c);
            this.cboBaud.MaxDropDownItems = 2;
            this.cboBaud.Name = "cboBaud";
            this.cboBaud.Size = new Size(0x3f, 0x15);
            this.cboBaud.TabIndex = 3;
            this.cboBaud.SelectedIndexChanged += new EventHandler(this.cboBaud_SelectedIndexChanged);
            this.lblBaud.AutoSize = true;
            this.lblBaud.Location = new Point(6, 0x2f);
            this.lblBaud.Name = "lblBaud";
            this.lblBaud.Size = new Size(0x20, 13);
            this.lblBaud.TabIndex = 2;
            this.lblBaud.Text = "Baud";
            this.nudPort.Location = new Point(0x2b, 0x12);
            bits = new int[4];
            bits[0] = 4;
            this.nudPort.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.nudPort.Minimum = new decimal(bits);
            this.nudPort.Name = "nudPort";
            this.nudPort.Size = new Size(0x3f, 20);
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
            this.imgLstSetup.ImageStream = (ImageListStreamer) manager.GetObject("imgLstSetup.ImageStream");
            this.imgLstSetup.TransparentColor = Color.Transparent;
            this.imgLstSetup.Images.SetKeyName(0, "efx_setup4.png");
            this.imgLstSetup.Images.SetKeyName(1, "efx_setup5.png");
            this.imgLstSetup.Images.SetKeyName(2, "efx_setup6.png");
            this.imgLstSetup.Images.SetKeyName(3, "efx_setup7.png");
            this.imgLstSetup.Images.SetKeyName(4, "efx_setup0.png");
            this.imgLstSetup.Images.SetKeyName(5, "efx_setup1.png");
            this.imgLstSetup.Images.SetKeyName(6, "efx_setup2.png");
            this.imgLstSetup.Images.SetKeyName(7, "efx_setup3.png");
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0xf8, 0x106);
            base.Controls.Add(this.grpThresh);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.grpPort);
            base.Name = "frmEfxSetup";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Setup";
            this.grpThresh.ResumeLayout(false);
            this.nudThreshold.EndInit();
            this.grpPort.ResumeLayout(false);
            this.grpPort.PerformLayout();
            ((ISupportInitialize) this.picSetup).EndInit();
            this.nudPort.EndInit();
            base.ResumeLayout(false);
        }

        private void UpdateImage()
        {
            if ((this.cboBaud.SelectedIndex != -1) && (this.cboAddress.SelectedIndex != -1))
            {
                this.picSetup.Image = this.imgLstSetup.Images[(this.cboBaud.SelectedIndex * 4) + this.cboAddress.SelectedIndex];
            }
        }
    }
}

