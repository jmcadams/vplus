namespace FC4_RC4
{
    using System;
    using System.Collections.Generic;
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
        private ComboBox cboGrp1Type;
        private ComboBox cboGrp2Addr;
        private ComboBox cboGrp2Type;
        private ComboBox cboGrp3Addr;
        private ComboBox cboGrp3Type;
        private ComboBox cboGrp4Addr;
        private ComboBox cboGrp4Type;
        private ComboBox cboGrp5Addr;
        private ComboBox cboGrp5Type;
        private ComboBox cboGrp6Addr;
        private ComboBox cboGrp6Type;
        private ComboBox cboGrp7Addr;
        private ComboBox cboGrp7Type;
        private ComboBox cboGrp8Addr;
        private ComboBox cboGrp8Type;
        private IContainer components = null;
        private GroupBox grpChannels;
        private GroupBox grpPort;
        private GroupBox grpThreshold;
        private Label lblBaudNote;
        private Label lblChGrp1;
        private Label lblChGrp2;
        private Label lblChGrp3;
        private Label lblChGrp4;
        private Label lblChGrp5;
        private Label lblChGrp6;
        private Label lblChGrp7;
        private Label lblChGrp8;
        private Label lblPort;
        private Label lblThresh;
        private ComboBox[] m_addrCombos;
        private bool m_init = false;
        private XmlNode m_setupNode;
        private ComboBox[] m_typeCombos;
        private const int MAX_MODULES = 4;
        private NumericUpDown nudPort;
        private NumericUpDown nudThreshold;

        public frmSetupDialog(XmlNode setupNode)
        {
            this.InitializeComponent();
            this.m_setupNode = setupNode;
            string innerText = Xml.GetNodeAlways(this.m_setupNode, "name").InnerText;
            this.nudPort.Value = (innerText.Length > 3) ? int.Parse(innerText.Substring(3)) : 1;
            innerText = Xml.GetNodeAlways(this.m_setupNode, "threshold").InnerText;
            if (innerText.Length > 0)
            {
                this.nudThreshold.Value = int.Parse(innerText);
            }
            this.m_typeCombos = new ComboBox[] { this.cboGrp1Type, this.cboGrp2Type, this.cboGrp3Type, this.cboGrp4Type, this.cboGrp5Type, this.cboGrp6Type, this.cboGrp7Type, this.cboGrp8Type };
            this.m_addrCombos = new ComboBox[] { this.cboGrp1Addr, this.cboGrp2Addr, this.cboGrp3Addr, this.cboGrp4Addr, this.cboGrp5Addr, this.cboGrp6Addr, this.cboGrp7Addr, this.cboGrp8Addr };
            this.m_init = true;
            for (int i = 0; i < 8; i++)
            {
                this.m_typeCombos[i].SelectedIndex = this.m_typeCombos[i].Items.IndexOf(Xml.GetNodeAlways(this.m_setupNode, "Type" + i.ToString()).InnerText);
                this.m_addrCombos[i].SelectedIndex = this.m_addrCombos[i].Items.IndexOf(Xml.GetNodeAlways(this.m_setupNode, "Addr" + i.ToString()).InnerText);
            }
            this.m_init = false;
        }

        private void addrComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.m_init)
            {
                ComboBox addrComboBox = (ComboBox) sender;
                this.ModuleAddressCheck(addrComboBox);
                this.MaxModuleCheck((ComboBox) this.grpChannels.GetNextControl(addrComboBox, false));
            }
        }

        private List<int> AddressesOfType(int typeIndex, ComboBox addrComboToIgnore)
        {
            List<int> list = new List<int>();
            if (typeIndex > 0)
            {
                foreach (ComboBox box2 in this.m_typeCombos)
                {
                    ComboBox nextControl = (ComboBox) this.grpChannels.GetNextControl(box2, true);
                    if ((addrComboToIgnore != nextControl) && (box2.SelectedIndex == typeIndex))
                    {
                        list.Add(nextControl.SelectedIndex);
                    }
                }
            }
            return list;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Xml.SetValue(this.m_setupNode, "name", "COM" + ((int) this.nudPort.Value).ToString());
            Xml.SetValue(this.m_setupNode, "threshold", ((int) this.nudThreshold.Value).ToString());
            for (int i = 0; i < 8; i++)
            {
                if (this.m_typeCombos[i].SelectedItem != null)
                {
                    Xml.SetValue(this.m_setupNode, "Type" + i.ToString(), this.m_typeCombos[i].SelectedItem.ToString());
                }
                else
                {
                    Xml.SetValue(this.m_setupNode, "Type" + i.ToString(), string.Empty);
                }
                if (this.m_addrCombos[i].SelectedItem != null)
                {
                    Xml.SetValue(this.m_setupNode, "Addr" + i.ToString(), this.m_addrCombos[i].SelectedItem.ToString());
                }
                else
                {
                    Xml.SetValue(this.m_setupNode, "Addr" + i.ToString(), string.Empty);
                }
            }
        }

        private int CountOf(int typeIndex)
        {
            int num = 0;
            foreach (ComboBox box in this.m_typeCombos)
            {
                if (box.SelectedIndex == typeIndex)
                {
                    num++;
                }
            }
            return num;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmSetupDialog_Load(object sender, EventArgs e)
        {
            foreach (ComboBox box in this.m_typeCombos)
            {
                if (box.SelectedIndex == -1)
                {
                    box.SelectedIndex = 0;
                }
            }
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
            this.cboGrp8Addr = new ComboBox();
            this.cboGrp8Type = new ComboBox();
            this.lblChGrp8 = new Label();
            this.cboGrp7Addr = new ComboBox();
            this.cboGrp7Type = new ComboBox();
            this.lblChGrp7 = new Label();
            this.cboGrp6Addr = new ComboBox();
            this.cboGrp6Type = new ComboBox();
            this.lblChGrp6 = new Label();
            this.cboGrp5Addr = new ComboBox();
            this.cboGrp5Type = new ComboBox();
            this.lblChGrp5 = new Label();
            this.cboGrp4Addr = new ComboBox();
            this.cboGrp4Type = new ComboBox();
            this.lblChGrp4 = new Label();
            this.cboGrp3Addr = new ComboBox();
            this.cboGrp3Type = new ComboBox();
            this.lblChGrp3 = new Label();
            this.cboGrp2Addr = new ComboBox();
            this.cboGrp2Type = new ComboBox();
            this.lblChGrp2 = new Label();
            this.cboGrp1Addr = new ComboBox();
            this.cboGrp1Type = new ComboBox();
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
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new Point(260, 0x182);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x4b, 0x17);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new Point(260, 0x165);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4b, 0x17);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.grpThreshold.Controls.Add(this.nudThreshold);
            this.grpThreshold.Controls.Add(this.lblThresh);
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
            this.lblThresh.Text = "Select intensity (1-100%) at which an RC-4 channel will be considered \"on.\"";
            this.grpChannels.Controls.Add(this.cboGrp8Addr);
            this.grpChannels.Controls.Add(this.cboGrp8Type);
            this.grpChannels.Controls.Add(this.lblChGrp8);
            this.grpChannels.Controls.Add(this.cboGrp7Addr);
            this.grpChannels.Controls.Add(this.cboGrp7Type);
            this.grpChannels.Controls.Add(this.lblChGrp7);
            this.grpChannels.Controls.Add(this.cboGrp6Addr);
            this.grpChannels.Controls.Add(this.cboGrp6Type);
            this.grpChannels.Controls.Add(this.lblChGrp6);
            this.grpChannels.Controls.Add(this.cboGrp5Addr);
            this.grpChannels.Controls.Add(this.cboGrp5Type);
            this.grpChannels.Controls.Add(this.lblChGrp5);
            this.grpChannels.Controls.Add(this.cboGrp4Addr);
            this.grpChannels.Controls.Add(this.cboGrp4Type);
            this.grpChannels.Controls.Add(this.lblChGrp4);
            this.grpChannels.Controls.Add(this.cboGrp3Addr);
            this.grpChannels.Controls.Add(this.cboGrp3Type);
            this.grpChannels.Controls.Add(this.lblChGrp3);
            this.grpChannels.Controls.Add(this.cboGrp2Addr);
            this.grpChannels.Controls.Add(this.cboGrp2Type);
            this.grpChannels.Controls.Add(this.lblChGrp2);
            this.grpChannels.Controls.Add(this.cboGrp1Addr);
            this.grpChannels.Controls.Add(this.cboGrp1Type);
            this.grpChannels.Controls.Add(this.lblChGrp1);
            this.grpChannels.Location = new Point(12, 0xa2);
            this.grpChannels.Name = "grpChannels";
            this.grpChannels.Size = new Size(0xea, 0xf7);
            this.grpChannels.TabIndex = 7;
            this.grpChannels.TabStop = false;
            this.grpChannels.Text = "Channel Groups";
            this.cboGrp8Addr.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboGrp8Addr.FormattingEnabled = true;
            this.cboGrp8Addr.Items.AddRange(new object[] { "%00 (0)", "%01 (1)", "%10 (2)", "%11 (3)" });
            this.cboGrp8Addr.Location = new Point(0x95, 0xd0);
            this.cboGrp8Addr.Name = "cboGrp8Addr";
            this.cboGrp8Addr.Size = new Size(70, 0x15);
            this.cboGrp8Addr.TabIndex = 0x37;
            this.cboGrp8Addr.SelectedIndexChanged += new EventHandler(this.addrComboBox_SelectedIndexChanged);
            this.cboGrp8Type.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboGrp8Type.FormattingEnabled = true;
            this.cboGrp8Type.Items.AddRange(new object[] { "None", "RC-4", "FC-4" });
            this.cboGrp8Type.Location = new Point(0x48, 0xd0);
            this.cboGrp8Type.Name = "cboGrp8Type";
            this.cboGrp8Type.Size = new Size(70, 0x15);
            this.cboGrp8Type.TabIndex = 0x36;
            this.cboGrp8Type.SelectedIndexChanged += new EventHandler(this.typeComboBox_SelectedIndexChanged);
            this.lblChGrp8.AutoSize = true;
            this.lblChGrp8.Location = new Point(10, 0xd3);
            this.lblChGrp8.Name = "lblChGrp8";
            this.lblChGrp8.Size = new Size(0x38, 13);
            this.lblChGrp8.TabIndex = 0x35;
            this.lblChGrp8.Text = "Ch 29 - 32";
            this.cboGrp7Addr.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboGrp7Addr.FormattingEnabled = true;
            this.cboGrp7Addr.Items.AddRange(new object[] { "%00 (0)", "%01 (1)", "%10 (2)", "%11 (3)" });
            this.cboGrp7Addr.Location = new Point(0x95, 0xb5);
            this.cboGrp7Addr.Name = "cboGrp7Addr";
            this.cboGrp7Addr.Size = new Size(70, 0x15);
            this.cboGrp7Addr.TabIndex = 0x34;
            this.cboGrp7Addr.SelectedIndexChanged += new EventHandler(this.addrComboBox_SelectedIndexChanged);
            this.cboGrp7Type.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboGrp7Type.FormattingEnabled = true;
            this.cboGrp7Type.Items.AddRange(new object[] { "None", "RC-4", "FC-4" });
            this.cboGrp7Type.Location = new Point(0x48, 0xb5);
            this.cboGrp7Type.Name = "cboGrp7Type";
            this.cboGrp7Type.Size = new Size(70, 0x15);
            this.cboGrp7Type.TabIndex = 0x33;
            this.cboGrp7Type.SelectedIndexChanged += new EventHandler(this.typeComboBox_SelectedIndexChanged);
            this.lblChGrp7.AutoSize = true;
            this.lblChGrp7.Location = new Point(10, 0xb8);
            this.lblChGrp7.Name = "lblChGrp7";
            this.lblChGrp7.Size = new Size(0x38, 13);
            this.lblChGrp7.TabIndex = 50;
            this.lblChGrp7.Text = "Ch 25 - 28";
            this.cboGrp6Addr.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboGrp6Addr.FormattingEnabled = true;
            this.cboGrp6Addr.Items.AddRange(new object[] { "%00 (0)", "%01 (1)", "%10 (2)", "%11 (3)" });
            this.cboGrp6Addr.Location = new Point(0x95, 0x9a);
            this.cboGrp6Addr.Name = "cboGrp6Addr";
            this.cboGrp6Addr.Size = new Size(70, 0x15);
            this.cboGrp6Addr.TabIndex = 0x31;
            this.cboGrp6Addr.SelectedIndexChanged += new EventHandler(this.addrComboBox_SelectedIndexChanged);
            this.cboGrp6Type.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboGrp6Type.FormattingEnabled = true;
            this.cboGrp6Type.Items.AddRange(new object[] { "None", "RC-4", "FC-4" });
            this.cboGrp6Type.Location = new Point(0x48, 0x9a);
            this.cboGrp6Type.Name = "cboGrp6Type";
            this.cboGrp6Type.Size = new Size(70, 0x15);
            this.cboGrp6Type.TabIndex = 0x30;
            this.cboGrp6Type.SelectedIndexChanged += new EventHandler(this.typeComboBox_SelectedIndexChanged);
            this.lblChGrp6.AutoSize = true;
            this.lblChGrp6.Location = new Point(10, 0x9d);
            this.lblChGrp6.Name = "lblChGrp6";
            this.lblChGrp6.Size = new Size(0x38, 13);
            this.lblChGrp6.TabIndex = 0x2f;
            this.lblChGrp6.Text = "Ch 21 - 24";
            this.cboGrp5Addr.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboGrp5Addr.FormattingEnabled = true;
            this.cboGrp5Addr.Items.AddRange(new object[] { "%00 (0)", "%01 (1)", "%10 (2)", "%11 (3)" });
            this.cboGrp5Addr.Location = new Point(0x95, 0x7f);
            this.cboGrp5Addr.Name = "cboGrp5Addr";
            this.cboGrp5Addr.Size = new Size(70, 0x15);
            this.cboGrp5Addr.TabIndex = 0x2e;
            this.cboGrp5Addr.SelectedIndexChanged += new EventHandler(this.addrComboBox_SelectedIndexChanged);
            this.cboGrp5Type.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboGrp5Type.FormattingEnabled = true;
            this.cboGrp5Type.Items.AddRange(new object[] { "None", "RC-4", "FC-4" });
            this.cboGrp5Type.Location = new Point(0x49, 0x7f);
            this.cboGrp5Type.Name = "cboGrp5Type";
            this.cboGrp5Type.Size = new Size(70, 0x15);
            this.cboGrp5Type.TabIndex = 0x2d;
            this.cboGrp5Type.SelectedIndexChanged += new EventHandler(this.typeComboBox_SelectedIndexChanged);
            this.lblChGrp5.AutoSize = true;
            this.lblChGrp5.Location = new Point(10, 130);
            this.lblChGrp5.Name = "lblChGrp5";
            this.lblChGrp5.Size = new Size(0x38, 13);
            this.lblChGrp5.TabIndex = 0x2c;
            this.lblChGrp5.Text = "Ch 17 - 20";
            this.cboGrp4Addr.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboGrp4Addr.FormattingEnabled = true;
            this.cboGrp4Addr.Items.AddRange(new object[] { "%00 (0)", "%01 (1)", "%10 (2)", "%11 (3)" });
            this.cboGrp4Addr.Location = new Point(0x95, 100);
            this.cboGrp4Addr.Name = "cboGrp4Addr";
            this.cboGrp4Addr.Size = new Size(70, 0x15);
            this.cboGrp4Addr.TabIndex = 0x2b;
            this.cboGrp4Addr.SelectedIndexChanged += new EventHandler(this.addrComboBox_SelectedIndexChanged);
            this.cboGrp4Type.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboGrp4Type.FormattingEnabled = true;
            this.cboGrp4Type.Items.AddRange(new object[] { "None", "RC-4", "FC-4" });
            this.cboGrp4Type.Location = new Point(0x49, 100);
            this.cboGrp4Type.Name = "cboGrp4Type";
            this.cboGrp4Type.Size = new Size(70, 0x15);
            this.cboGrp4Type.TabIndex = 0x2a;
            this.cboGrp4Type.SelectedIndexChanged += new EventHandler(this.typeComboBox_SelectedIndexChanged);
            this.lblChGrp4.AutoSize = true;
            this.lblChGrp4.Location = new Point(10, 0x67);
            this.lblChGrp4.Name = "lblChGrp4";
            this.lblChGrp4.Size = new Size(0x38, 13);
            this.lblChGrp4.TabIndex = 0x29;
            this.lblChGrp4.Text = "Ch 13 - 16";
            this.cboGrp3Addr.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboGrp3Addr.FormattingEnabled = true;
            this.cboGrp3Addr.Items.AddRange(new object[] { "%00 (0)", "%01 (1)", "%10 (2)", "%11 (3)" });
            this.cboGrp3Addr.Location = new Point(0x95, 0x49);
            this.cboGrp3Addr.Name = "cboGrp3Addr";
            this.cboGrp3Addr.Size = new Size(70, 0x15);
            this.cboGrp3Addr.TabIndex = 40;
            this.cboGrp3Addr.SelectedIndexChanged += new EventHandler(this.addrComboBox_SelectedIndexChanged);
            this.cboGrp3Type.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboGrp3Type.FormattingEnabled = true;
            this.cboGrp3Type.Items.AddRange(new object[] { "None", "RC-4", "FC-4" });
            this.cboGrp3Type.Location = new Point(0x48, 0x49);
            this.cboGrp3Type.Name = "cboGrp3Type";
            this.cboGrp3Type.Size = new Size(70, 0x15);
            this.cboGrp3Type.TabIndex = 0x27;
            this.cboGrp3Type.SelectedIndexChanged += new EventHandler(this.typeComboBox_SelectedIndexChanged);
            this.lblChGrp3.AutoSize = true;
            this.lblChGrp3.Location = new Point(10, 0x4c);
            this.lblChGrp3.Name = "lblChGrp3";
            this.lblChGrp3.Size = new Size(50, 13);
            this.lblChGrp3.TabIndex = 0x26;
            this.lblChGrp3.Text = "Ch 9 - 12";
            this.cboGrp2Addr.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboGrp2Addr.FormattingEnabled = true;
            this.cboGrp2Addr.Items.AddRange(new object[] { "%00 (0)", "%01 (1)", "%10 (2)", "%11 (3)" });
            this.cboGrp2Addr.Location = new Point(0x95, 0x2e);
            this.cboGrp2Addr.Name = "cboGrp2Addr";
            this.cboGrp2Addr.Size = new Size(70, 0x15);
            this.cboGrp2Addr.TabIndex = 0x25;
            this.cboGrp2Addr.SelectedIndexChanged += new EventHandler(this.addrComboBox_SelectedIndexChanged);
            this.cboGrp2Type.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboGrp2Type.FormattingEnabled = true;
            this.cboGrp2Type.Items.AddRange(new object[] { "None", "RC-4", "FC-4" });
            this.cboGrp2Type.Location = new Point(0x48, 0x2e);
            this.cboGrp2Type.Name = "cboGrp2Type";
            this.cboGrp2Type.Size = new Size(70, 0x15);
            this.cboGrp2Type.TabIndex = 0x24;
            this.cboGrp2Type.SelectedIndexChanged += new EventHandler(this.typeComboBox_SelectedIndexChanged);
            this.lblChGrp2.AutoSize = true;
            this.lblChGrp2.Location = new Point(10, 0x31);
            this.lblChGrp2.Name = "lblChGrp2";
            this.lblChGrp2.Size = new Size(0x2c, 13);
            this.lblChGrp2.TabIndex = 0x23;
            this.lblChGrp2.Text = "Ch 5 - 8";
            this.cboGrp1Addr.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboGrp1Addr.FormattingEnabled = true;
            this.cboGrp1Addr.Items.AddRange(new object[] { "%00 (0)", "%01 (1)", "%10 (2)", "%11 (3)" });
            this.cboGrp1Addr.Location = new Point(0x95, 0x13);
            this.cboGrp1Addr.Name = "cboGrp1Addr";
            this.cboGrp1Addr.Size = new Size(70, 0x15);
            this.cboGrp1Addr.TabIndex = 0x22;
            this.cboGrp1Addr.SelectedIndexChanged += new EventHandler(this.addrComboBox_SelectedIndexChanged);
            this.cboGrp1Type.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboGrp1Type.FormattingEnabled = true;
            this.cboGrp1Type.Items.AddRange(new object[] { "None", "RC-4", "FC-4" });
            this.cboGrp1Type.Location = new Point(0x48, 0x13);
            this.cboGrp1Type.Name = "cboGrp1Type";
            this.cboGrp1Type.Size = new Size(70, 0x15);
            this.cboGrp1Type.TabIndex = 0x21;
            this.cboGrp1Type.SelectedIndexChanged += new EventHandler(this.typeComboBox_SelectedIndexChanged);
            this.lblChGrp1.AutoSize = true;
            this.lblChGrp1.Location = new Point(10, 0x16);
            this.lblChGrp1.Name = "lblChGrp1";
            this.lblChGrp1.Size = new Size(0x2c, 13);
            this.lblChGrp1.TabIndex = 0x20;
            this.lblChGrp1.Text = "Ch 1 - 4";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x15a, 0x1a9);
            base.Controls.Add(this.grpChannels);
            base.Controls.Add(this.grpThreshold);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.grpPort);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Name = "frmSetupDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Setup";
            base.Load += new EventHandler(this.frmSetupDialog_Load);
            this.grpPort.ResumeLayout(false);
            this.grpPort.PerformLayout();
            this.nudPort.EndInit();
            this.grpThreshold.ResumeLayout(false);
            this.nudThreshold.EndInit();
            this.grpChannels.ResumeLayout(false);
            this.grpChannels.PerformLayout();
            base.ResumeLayout(false);
        }

        private void MaxModuleCheck(ComboBox typeComboBox)
        {
            if (((typeComboBox.SelectedIndex != 0) && (typeComboBox.SelectedIndex != -1)) && (this.CountOf(typeComboBox.SelectedIndex) > 4))
            {
                MessageBox.Show(string.Format("A maximum of {0} modules of the same type may be selected.\nThe selection will be reset to 'None'.", 4), "FC4-RC4", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                typeComboBox.SelectedIndex = 0;
            }
        }

        private void ModuleAddressCheck(ComboBox addrComboBox)
        {
            if (addrComboBox.SelectedIndex != -1)
            {
                int selectedIndex = ((ComboBox) this.grpChannels.GetNextControl(addrComboBox, false)).SelectedIndex;
                if (selectedIndex > 0)
                {
                    if (this.AddressesOfType(selectedIndex, addrComboBox).Contains(addrComboBox.SelectedIndex))
                    {
                        MessageBox.Show("A module of that type is already at that address.\nThe address will be reset.", "FC4 RC4", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        addrComboBox.SelectedIndex = -1;
                    }
                }
                else
                {
                    addrComboBox.SelectedIndex = -1;
                }
            }
        }

        private void typeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.m_init)
            {
                ComboBox typeComboBox = (ComboBox) sender;
                this.MaxModuleCheck(typeComboBox);
                this.ModuleAddressCheck((ComboBox) this.grpChannels.GetNextControl(typeComboBox, true));
            }
        }
    }
}

