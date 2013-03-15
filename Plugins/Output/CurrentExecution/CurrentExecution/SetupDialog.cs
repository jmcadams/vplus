namespace CurrentExecution
{
    using LedTriksUtil;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;
    using Vixen.Dialogs;

    public class SetupDialog : Form
    {
        private Button buttonBoardLayout;
        private Button buttonCancel;
        private Button buttonOK;
        private Button buttonPortSetup;
        private Button buttonTextOptions;
        private Button buttonVirtualDisplaySetup;
        private CheckBox checkBoxVirtualHardware;
        private IContainer components = null;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Label label1;
        private Label label2;
        private Label labelPortAddress;
        private Size m_boardLayout;
        private int m_dotPitch;
        private Generator m_generator;
        private Color m_ledColor;
        private int m_ledSize;
        private int m_portAddress;
        private XmlNode m_setupNode;
        private TextBox textBoxMessage;

        public SetupDialog(XmlNode setupNode)
        {
            this.InitializeComponent();
            try
            {
                this.m_setupNode = setupNode;
                this.m_generator = new Generator();
                this.m_boardLayout = new Size(1, 1);
                XmlNode nodeAlways = Xml.GetNodeAlways(this.m_setupNode, "Boards");
                if (nodeAlways.Attributes["width"] != null)
                {
                    this.m_boardLayout.Width = int.Parse(nodeAlways.Attributes["width"].Value);
                }
                if (nodeAlways.Attributes["height"] != null)
                {
                    this.m_boardLayout.Height = int.Parse(nodeAlways.Attributes["height"].Value);
                }
                this.textBoxMessage.Text = Xml.GetNodeAlways(this.m_setupNode, "Message", "[NAME]").InnerText;
                this.m_portAddress = int.Parse(Xml.GetNodeAlways(this.m_setupNode, "Address", "888").InnerText);
                XmlNode parentNode = this.m_setupNode["TextOptions"];
                if (parentNode != null)
                {
                    this.m_generator.LoadFromXml(parentNode);
                }
                XmlNode contextNode = Xml.GetNodeAlways(this.m_setupNode, "Virtual");
                if (contextNode.Attributes["enabled"] != null)
                {
                    this.SetVirtual(bool.Parse(contextNode.Attributes["enabled"].Value));
                }
                else
                {
                    this.SetVirtual(false);
                }
                if (this.checkBoxVirtualHardware.Checked)
                {
                    this.m_ledSize = int.Parse(Xml.GetNodeAlways(contextNode, "LEDSize", "3").InnerText);
                    this.m_ledColor = Color.FromArgb(int.Parse(Xml.GetNodeAlways(contextNode, "LEDColor", "-65536").InnerText));
                    this.m_dotPitch = int.Parse(Xml.GetNodeAlways(contextNode, "DotPitch", "9").InnerText);
                }
                else
                {
                    this.m_ledSize = 3;
                    this.m_ledColor = Color.Red;
                    this.m_dotPitch = 9;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + "\n" + exception.StackTrace);
                throw;
            }
        }

        private void buttonBoardLayout_Click(object sender, EventArgs e)
        {
            BoardLayoutDialog dialog = new BoardLayoutDialog(this.m_boardLayout);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.m_boardLayout = dialog.BoardLayout;
            }
            dialog.Dispose();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            XmlNode nodeAlways = Xml.GetNodeAlways(this.m_setupNode, "Boards");
            Xml.SetAttribute(nodeAlways, "width", this.m_boardLayout.Width.ToString());
            Xml.SetAttribute(nodeAlways, "height", this.m_boardLayout.Height.ToString());
            Xml.SetValue(this.m_setupNode, "Address", this.m_portAddress.ToString());
            Xml.SetValue(this.m_setupNode, "Message", this.textBoxMessage.Text);
            XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(this.m_setupNode, "TextOptions");
            this.m_generator.SaveToXml(emptyNodeAlways);
            XmlNode contextNode = Xml.SetAttribute(this.m_setupNode, "Virtual", "enabled", this.checkBoxVirtualHardware.Checked.ToString());
            if (this.checkBoxVirtualHardware.Checked)
            {
                Xml.SetValue(contextNode, "LEDSize", this.m_ledSize.ToString());
                Xml.SetValue(contextNode, "LEDColor", this.m_ledColor.ToArgb().ToString());
                Xml.SetValue(contextNode, "DotPitch", this.m_dotPitch.ToString());
            }
        }

        private void buttonPortSetup_Click(object sender, EventArgs e)
        {
            ParallelSetupDialog dialog = new ParallelSetupDialog(this.m_portAddress);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.m_portAddress = dialog.PortAddress;
            }
            dialog.Dispose();
        }

        private void buttonTextOptions_Click(object sender, EventArgs e)
        {
            TextOptionsDialog dialog = new TextOptionsDialog(this.m_generator);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
            }
            dialog.Dispose();
        }

        private void buttonVirtualDisplaySetup_Click(object sender, EventArgs e)
        {
            VirtualHardwareSetupDialog dialog = new VirtualHardwareSetupDialog(false);
            dialog.LEDSize = this.m_ledSize;
            dialog.LEDColor = this.m_ledColor;
            dialog.DotPitch = this.m_dotPitch;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.m_ledSize = dialog.LEDSize;
                this.m_ledColor = dialog.LEDColor;
                this.m_dotPitch = dialog.DotPitch;
            }
            dialog.Dispose();
        }

        private void checkBoxVirtualHardware_CheckedChanged(object sender, EventArgs e)
        {
            this.SetVirtual(this.checkBoxVirtualHardware.Checked);
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
            this.groupBox1 = new GroupBox();
            this.buttonBoardLayout = new Button();
            this.label1 = new Label();
            this.groupBox2 = new GroupBox();
            this.buttonTextOptions = new Button();
            this.textBoxMessage = new TextBox();
            this.label2 = new Label();
            this.groupBox3 = new GroupBox();
            this.buttonPortSetup = new Button();
            this.labelPortAddress = new Label();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.checkBoxVirtualHardware = new CheckBox();
            this.buttonVirtualDisplaySetup = new Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.buttonBoardLayout);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new Point(0x10, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x1ab, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Board Layout";
            this.buttonBoardLayout.Location = new Point(0x145, 0x1d);
            this.buttonBoardLayout.Name = "buttonBoardLayout";
            this.buttonBoardLayout.Size = new Size(0x54, 0x17);
            this.buttonBoardLayout.TabIndex = 1;
            this.buttonBoardLayout.Text = "Board layout";
            this.buttonBoardLayout.UseVisualStyleBackColor = true;
            this.buttonBoardLayout.Click += new EventHandler(this.buttonBoardLayout_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x13, 0x1d);
            this.label1.Name = "label1";
            this.label1.Size = new Size(270, 0x27);
            this.label1.TabIndex = 0;
            this.label1.Text = "Up to 4 LedTriks boards are possible in a single display.\r\nSelect how many boards you will be using and how they\r\nwill be laid out.";
            this.groupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox2.Controls.Add(this.buttonTextOptions);
            this.groupBox2.Controls.Add(this.textBoxMessage);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new Point(0x10, 0x79);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x1ab, 0x97);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Message Format";
            this.buttonTextOptions.Location = new Point(0x16, 0x6c);
            this.buttonTextOptions.Name = "buttonTextOptions";
            this.buttonTextOptions.Size = new Size(0x4b, 0x17);
            this.buttonTextOptions.TabIndex = 2;
            this.buttonTextOptions.Text = "Text options";
            this.buttonTextOptions.UseVisualStyleBackColor = true;
            this.buttonTextOptions.Click += new EventHandler(this.buttonTextOptions_Click);
            this.textBoxMessage.Location = new Point(0x16, 0x52);
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new Size(0x17e, 20);
            this.textBoxMessage.TabIndex = 1;
            this.textBoxMessage.Text = "[NAME]";
            this.textBoxMessage.Validating += new CancelEventHandler(this.textBoxMessage_Validating);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x13, 0x20);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x107, 0x27);
            this.label2.TabIndex = 0;
            this.label2.Text = "The name of the currently-executing sequence can be\r\ninserted into a custom message.  Insert [NAME] where\r\nyou want it to appear in your message.";
            this.groupBox3.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox3.Controls.Add(this.buttonVirtualDisplaySetup);
            this.groupBox3.Controls.Add(this.checkBoxVirtualHardware);
            this.groupBox3.Controls.Add(this.buttonPortSetup);
            this.groupBox3.Controls.Add(this.labelPortAddress);
            this.groupBox3.Location = new Point(0x10, 0x116);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(0x1ab, 100);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Port Setup";
            this.buttonPortSetup.Location = new Point(0x145, 0x36);
            this.buttonPortSetup.Name = "buttonPortSetup";
            this.buttonPortSetup.Size = new Size(0x54, 0x17);
            this.buttonPortSetup.TabIndex = 3;
            this.buttonPortSetup.Text = "Port setup";
            this.buttonPortSetup.UseVisualStyleBackColor = true;
            this.buttonPortSetup.Click += new EventHandler(this.buttonPortSetup_Click);
            this.labelPortAddress.AutoSize = true;
            this.labelPortAddress.Location = new Point(0x13, 0x3b);
            this.labelPortAddress.Name = "labelPortAddress";
            this.labelPortAddress.Size = new Size(0xd3, 13);
            this.labelPortAddress.TabIndex = 2;
            this.labelPortAddress.Text = "Select the base address of the parallel port.";
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(0x11f, 0x180);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x170, 0x180);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.checkBoxVirtualHardware.AutoSize = true;
            this.checkBoxVirtualHardware.Location = new Point(0x16, 0x1d);
            this.checkBoxVirtualHardware.Name = "checkBoxVirtualHardware";
            this.checkBoxVirtualHardware.Size = new Size(0xf5, 0x11);
            this.checkBoxVirtualHardware.TabIndex = 0;
            this.checkBoxVirtualHardware.Text = "I'm not using hardware, I want a virtual display.";
            this.checkBoxVirtualHardware.UseVisualStyleBackColor = true;
            this.checkBoxVirtualHardware.CheckedChanged += new EventHandler(this.checkBoxVirtualHardware_CheckedChanged);
            this.buttonVirtualDisplaySetup.Enabled = false;
            this.buttonVirtualDisplaySetup.Location = new Point(0x145, 0x19);
            this.buttonVirtualDisplaySetup.Name = "buttonVirtualDisplaySetup";
            this.buttonVirtualDisplaySetup.Size = new Size(0x54, 0x17);
            this.buttonVirtualDisplaySetup.TabIndex = 1;
            this.buttonVirtualDisplaySetup.Text = "Display setup";
            this.buttonVirtualDisplaySetup.UseVisualStyleBackColor = true;
            this.buttonVirtualDisplaySetup.Click += new EventHandler(this.buttonVirtualDisplaySetup_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x1c7, 0x1a3);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.groupBox3);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Name = "SetupDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Setup";
            base.FormClosing += new FormClosingEventHandler(this.SetupDialog_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            base.ResumeLayout(false);
        }

        private void SetupDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.m_generator.Dispose();
        }

        private void SetVirtual(bool virtualEnabled)
        {
            this.labelPortAddress.Enabled = this.buttonPortSetup.Enabled = !virtualEnabled;
            this.buttonVirtualDisplaySetup.Enabled = virtualEnabled;
            this.checkBoxVirtualHardware.Checked = virtualEnabled;
        }

        private void textBoxMessage_Validating(object sender, CancelEventArgs e)
        {
            if (this.textBoxMessage.Text.Trim().Length == 0)
            {
                this.textBoxMessage.Text = "[NAME]";
            }
            else if (!this.textBoxMessage.Text.Contains("[NAME]"))
            {
                e.Cancel = true;
                MessageBox.Show(string.Format("The message needs to contain {0} to know where to put the sequence name in the message.", "[NAME]"), "Current Execution", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
    }
}

