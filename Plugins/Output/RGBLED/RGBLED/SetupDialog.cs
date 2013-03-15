namespace RGBLED
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO.Ports;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen.Dialogs;

    public class SetupDialog : Form
    {
        private Button buttonAdd;
        private Button buttonCancel;
        private Button buttonChangeID;
        private Button buttonOK;
        private Button buttonSerial;
        private IContainer components = null;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private SerialPort m_selectedPort;
        private Panel panelContainer;
        private TextBox textBoxNewID;
        private TextBox textBoxOldID;

        public SetupDialog(SerialPort selectedPort, XmlNode setupNode)
        {
            this.InitializeComponent();
            this.m_selectedPort = selectedPort;
            foreach (XmlNode node in setupNode.SelectSingleNode("Controllers").SelectNodes("Controller"))
            {
                this.AddController(node.Attributes["config"].Value);
            }
        }

        private void AddController(string configuration)
        {
            RGBLEDController controller;
            this.panelContainer.Controls.Add(controller = new RGBLEDController(this.panelContainer.Controls.Count + 1));
            controller.Dock = DockStyle.Top;
            controller.IndexChange += new RGBLEDController.OnIndexChange(this.controller_IndexChange);
            controller.Configuration = configuration;
            this.panelContainer.Controls.SetChildIndex(controller, 0);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            this.AddController("software");
        }

        private void buttonChangeID_Click(object sender, EventArgs e)
        {
            int num;
            int num2;
            try
            {
                num = Convert.ToInt32(this.textBoxOldID.Text);
            }
            catch
            {
                MessageBox.Show("Value for old ID is not a valid number.", "RGBLED", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            try
            {
                num2 = Convert.ToInt32(this.textBoxNewID.Text);
            }
            catch
            {
                MessageBox.Show("Value for new ID is not a valid number.", "RGBLED", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            SerialPort port = new SerialPort(this.m_selectedPort.PortName, this.m_selectedPort.BaudRate, this.m_selectedPort.Parity, this.m_selectedPort.DataBits, this.m_selectedPort.StopBits);
            port.Open();
            try
            {
                port.Write(string.Format("#{0:X2}F0{1:X2}0D{1:X2}F10D", num, num2));
            }
            finally
            {
                port.Close();
            }
            MessageBox.Show("ID has been updated", "RGBLED", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void buttonSerial_Click(object sender, EventArgs e)
        {
            SerialSetupDialog dialog = new SerialSetupDialog(this.m_selectedPort);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.m_selectedPort = dialog.SelectedPort;
            }
        }

        private void controller_IndexChange()
        {
            this.RenumberControls(null, null);
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
            this.buttonSerial = new Button();
            this.groupBox1 = new GroupBox();
            this.buttonChangeID = new Button();
            this.textBoxNewID = new TextBox();
            this.label2 = new Label();
            this.textBoxOldID = new TextBox();
            this.label1 = new Label();
            this.panelContainer = new Panel();
            this.buttonAdd = new Button();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.buttonSerial.Location = new Point(12, 12);
            this.buttonSerial.Name = "buttonSerial";
            this.buttonSerial.Size = new Size(0x4b, 0x17);
            this.buttonSerial.TabIndex = 0;
            this.buttonSerial.Text = "Serial Setup";
            this.buttonSerial.UseVisualStyleBackColor = true;
            this.buttonSerial.Click += new EventHandler(this.buttonSerial_Click);
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.buttonChangeID);
            this.groupBox1.Controls.Add(this.textBoxNewID);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxOldID);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.panelContainer);
            this.groupBox1.Controls.Add(this.buttonAdd);
            this.groupBox1.Location = new Point(12, 0x36);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x14b, 0x10b);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Controllers";
            this.buttonChangeID.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.buttonChangeID.Location = new Point(250, 0xe7);
            this.buttonChangeID.Name = "buttonChangeID";
            this.buttonChangeID.Size = new Size(0x4b, 0x17);
            this.buttonChangeID.TabIndex = 6;
            this.buttonChangeID.Text = "Change ID";
            this.buttonChangeID.UseVisualStyleBackColor = true;
            this.buttonChangeID.Click += new EventHandler(this.buttonChangeID_Click);
            this.textBoxNewID.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.textBoxNewID.Location = new Point(0xc4, 0xe9);
            this.textBoxNewID.Name = "textBoxNewID";
            this.textBoxNewID.Size = new Size(40, 20);
            this.textBoxNewID.TabIndex = 5;
            this.label2.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0xae, 0xec);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x10, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "to";
            this.textBoxOldID.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.textBoxOldID.Location = new Point(0x80, 0xe9);
            this.textBoxOldID.Name = "textBoxOldID";
            this.textBoxOldID.Size = new Size(40, 20);
            this.textBoxOldID.TabIndex = 3;
            this.label1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(6, 0xec);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x74, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Change ID of controller";
            this.panelContainer.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.panelContainer.AutoScroll = true;
            this.panelContainer.Location = new Point(6, 0x30);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new Size(0x13f, 0xac);
            this.panelContainer.TabIndex = 1;
            this.panelContainer.ControlRemoved += new ControlEventHandler(this.RenumberControls);
            this.buttonAdd.Location = new Point(6, 0x13);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new Size(0x4b, 0x17);
            this.buttonAdd.TabIndex = 0;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new EventHandler(this.buttonAdd_Click);
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(0xb6, 0x14b);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x10c, 0x14b);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x163, 0x16e);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.buttonSerial);
            base.Name = "SetupDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Setup";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
        }

        private void RenumberControls(object sender, ControlEventArgs e)
        {
            int count = this.panelContainer.Controls.Count;
            foreach (RGBLEDController controller in this.panelContainer.Controls)
            {
                controller.ID = count--;
            }
        }

        public string[] Controllers
        {
            get
            {
                string[] strArray = new string[this.panelContainer.Controls.Count];
                int index = 0;
                for (int i = this.panelContainer.Controls.Count - 1; i >= 0; i--)
                {
                    if (((RGBLEDController) this.panelContainer.Controls[i]).Configuration.ToLower().Contains("hardware"))
                    {
                        strArray[index] = "hardware";
                    }
                    else
                    {
                        strArray[index] = "software";
                    }
                    index++;
                }
                return strArray;
            }
        }

        public SerialPort SelectedPort
        {
            get
            {
                return this.m_selectedPort;
            }
        }
    }
}

