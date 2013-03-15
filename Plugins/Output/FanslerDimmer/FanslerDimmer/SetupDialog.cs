namespace FanslerDimmer
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Xml;

    public class SetupDialog : Form
    {
        private Button buttonCancel;
        private Button buttonOK;
        private Button buttonParallel1Dimming;
        private Button buttonParallel2Dimming;
        private Button buttonParallel3Dimming;
        private CheckBox checkBoxParallel1;
        private CheckBox checkBoxParallel1Reverse;
        private CheckBox checkBoxParallel2;
        private CheckBox checkBoxParallel2Reverse;
        private CheckBox checkBoxParallel3;
        private CheckBox checkBoxParallel3Reverse;
        private IContainer components = null;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private DimmingDialog m_dimmingDialog;
        private XmlNode m_setupNode;
        private TextBox textBoxParallel1From;
        private TextBox textBoxParallel1To;
        private TextBox textBoxParallel2From;
        private TextBox textBoxParallel2To;
        private TextBox textBoxParallel3From;
        private TextBox textBoxParallel3To;

        public SetupDialog(XmlNode setupNode)
        {
            this.InitializeComponent();
            this.m_setupNode = setupNode;
            this.m_dimmingDialog = new DimmingDialog();
            XmlNode node = this.m_setupNode.SelectSingleNode("Parallel1");
            this.textBoxParallel1From.Text = node.Attributes["from"].Value;
            this.textBoxParallel1To.Text = node.Attributes["to"].Value;
            this.checkBoxParallel1Reverse.Checked = node.Attributes["reversed"].Value[0] == '1';
            node = this.m_setupNode.SelectSingleNode("Parallel2");
            this.textBoxParallel2From.Text = node.Attributes["from"].Value;
            this.textBoxParallel2To.Text = node.Attributes["to"].Value;
            this.checkBoxParallel2Reverse.Checked = node.Attributes["reversed"].Value[0] == '1';
            node = this.m_setupNode.SelectSingleNode("Parallel3");
            this.textBoxParallel3From.Text = node.Attributes["from"].Value;
            this.textBoxParallel3To.Text = node.Attributes["to"].Value;
            this.checkBoxParallel3Reverse.Checked = node.Attributes["reversed"].Value[0] == '1';
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            XmlNode node = this.m_setupNode.SelectSingleNode("Parallel1");
            node.Attributes["from"].Value = this.textBoxParallel1From.Text;
            node.Attributes["to"].Value = this.textBoxParallel1To.Text;
            node.Attributes["reversed"].Value = this.checkBoxParallel1Reverse.Checked ? "1" : "0";
            node = this.m_setupNode.SelectSingleNode("Parallel2");
            node.Attributes["from"].Value = this.textBoxParallel2From.Text;
            node.Attributes["to"].Value = this.textBoxParallel2To.Text;
            node.Attributes["reversed"].Value = this.checkBoxParallel2Reverse.Checked ? "1" : "0";
            node = this.m_setupNode.SelectSingleNode("Parallel3");
            node.Attributes["from"].Value = this.textBoxParallel3From.Text;
            node.Attributes["to"].Value = this.textBoxParallel3To.Text;
            node.Attributes["reversed"].Value = this.checkBoxParallel3Reverse.Checked ? "1" : "0";
        }

        private void buttonParallel1Dimming_Click(object sender, EventArgs e)
        {
            XmlNode node = this.m_setupNode.SelectSingleNode("Parallel1");
            node.Attributes["from"].Value = this.textBoxParallel1From.Text;
            node.Attributes["to"].Value = this.textBoxParallel1To.Text;
            this.m_dimmingDialog.PortElement = (XmlElement) node;
            this.m_dimmingDialog.ShowDialog();
        }

        private void buttonParallel2Dimming_Click(object sender, EventArgs e)
        {
            XmlNode node = this.m_setupNode.SelectSingleNode("Parallel2");
            node.Attributes["from"].Value = this.textBoxParallel2From.Text;
            node.Attributes["to"].Value = this.textBoxParallel2To.Text;
            this.m_dimmingDialog.PortElement = (XmlElement) node;
            this.m_dimmingDialog.ShowDialog();
        }

        private void buttonParallel3Dimming_Click(object sender, EventArgs e)
        {
            XmlNode node = this.m_setupNode.SelectSingleNode("Parallel3");
            node.Attributes["from"].Value = this.textBoxParallel3From.Text;
            node.Attributes["to"].Value = this.textBoxParallel3To.Text;
            this.m_dimmingDialog.PortElement = (XmlElement) node;
            this.m_dimmingDialog.ShowDialog();
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
            this.checkBoxParallel3Reverse = new CheckBox();
            this.checkBoxParallel2Reverse = new CheckBox();
            this.checkBoxParallel1Reverse = new CheckBox();
            this.buttonParallel3Dimming = new Button();
            this.buttonParallel2Dimming = new Button();
            this.buttonParallel1Dimming = new Button();
            this.textBoxParallel3To = new TextBox();
            this.label4 = new Label();
            this.textBoxParallel3From = new TextBox();
            this.checkBoxParallel3 = new CheckBox();
            this.textBoxParallel2To = new TextBox();
            this.label3 = new Label();
            this.textBoxParallel2From = new TextBox();
            this.checkBoxParallel2 = new CheckBox();
            this.textBoxParallel1To = new TextBox();
            this.label2 = new Label();
            this.textBoxParallel1From = new TextBox();
            this.label1 = new Label();
            this.checkBoxParallel1 = new CheckBox();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.label5 = new Label();
            this.label6 = new Label();
            this.label7 = new Label();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.checkBoxParallel3Reverse);
            this.groupBox1.Controls.Add(this.checkBoxParallel2Reverse);
            this.groupBox1.Controls.Add(this.checkBoxParallel1Reverse);
            this.groupBox1.Controls.Add(this.buttonParallel3Dimming);
            this.groupBox1.Controls.Add(this.buttonParallel2Dimming);
            this.groupBox1.Controls.Add(this.buttonParallel1Dimming);
            this.groupBox1.Controls.Add(this.textBoxParallel3To);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBoxParallel3From);
            this.groupBox1.Controls.Add(this.checkBoxParallel3);
            this.groupBox1.Controls.Add(this.textBoxParallel2To);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxParallel2From);
            this.groupBox1.Controls.Add(this.checkBoxParallel2);
            this.groupBox1.Controls.Add(this.textBoxParallel1To);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxParallel1From);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.checkBoxParallel1);
            this.groupBox1.Location = new Point(14, 0x12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x191, 0x100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Port mappings";
            this.checkBoxParallel3Reverse.AutoSize = true;
            this.checkBoxParallel3Reverse.Location = new Point(0x90, 0xd7);
            this.checkBoxParallel3Reverse.Name = "checkBoxParallel3Reverse";
            this.checkBoxParallel3Reverse.Size = new Size(0xaf, 0x11);
            this.checkBoxParallel3Reverse.TabIndex = 0x12;
            this.checkBoxParallel3Reverse.Text = "Full intensity is represented by 0";
            this.checkBoxParallel3Reverse.UseVisualStyleBackColor = true;
            this.checkBoxParallel2Reverse.AutoSize = true;
            this.checkBoxParallel2Reverse.Location = new Point(0x90, 0x92);
            this.checkBoxParallel2Reverse.Name = "checkBoxParallel2Reverse";
            this.checkBoxParallel2Reverse.Size = new Size(0xaf, 0x11);
            this.checkBoxParallel2Reverse.TabIndex = 12;
            this.checkBoxParallel2Reverse.Text = "Full intensity is represented by 0";
            this.checkBoxParallel2Reverse.UseVisualStyleBackColor = true;
            this.checkBoxParallel1Reverse.AutoSize = true;
            this.checkBoxParallel1Reverse.Location = new Point(0x90, 0x4d);
            this.checkBoxParallel1Reverse.Name = "checkBoxParallel1Reverse";
            this.checkBoxParallel1Reverse.Size = new Size(0xaf, 0x11);
            this.checkBoxParallel1Reverse.TabIndex = 6;
            this.checkBoxParallel1Reverse.Text = "Full intensity is represented by 0";
            this.checkBoxParallel1Reverse.UseVisualStyleBackColor = true;
            this.buttonParallel3Dimming.Location = new Point(0x114, 0xba);
            this.buttonParallel3Dimming.Name = "buttonParallel3Dimming";
            this.buttonParallel3Dimming.Size = new Size(110, 0x17);
            this.buttonParallel3Dimming.TabIndex = 0x11;
            this.buttonParallel3Dimming.Text = "Configure dimming";
            this.buttonParallel3Dimming.UseVisualStyleBackColor = true;
            this.buttonParallel3Dimming.Click += new EventHandler(this.buttonParallel3Dimming_Click);
            this.buttonParallel2Dimming.Location = new Point(0x114, 0x75);
            this.buttonParallel2Dimming.Name = "buttonParallel2Dimming";
            this.buttonParallel2Dimming.Size = new Size(110, 0x17);
            this.buttonParallel2Dimming.TabIndex = 11;
            this.buttonParallel2Dimming.Text = "Configure dimming";
            this.buttonParallel2Dimming.UseVisualStyleBackColor = true;
            this.buttonParallel2Dimming.Click += new EventHandler(this.buttonParallel2Dimming_Click);
            this.buttonParallel1Dimming.Location = new Point(0x114, 0x30);
            this.buttonParallel1Dimming.Name = "buttonParallel1Dimming";
            this.buttonParallel1Dimming.Size = new Size(110, 0x17);
            this.buttonParallel1Dimming.TabIndex = 5;
            this.buttonParallel1Dimming.Text = "Configure dimming";
            this.buttonParallel1Dimming.UseVisualStyleBackColor = true;
            this.buttonParallel1Dimming.Click += new EventHandler(this.buttonParallel1Dimming_Click);
            this.textBoxParallel3To.Location = new Point(0xd4, 0xbd);
            this.textBoxParallel3To.Name = "textBoxParallel3To";
            this.textBoxParallel3To.Size = new Size(40, 20);
            this.textBoxParallel3To.TabIndex = 0x10;
            this.label4.AutoSize = true;
            this.label4.Location = new Point(190, 0xc0);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x10, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "to";
            this.textBoxParallel3From.Location = new Point(0x90, 0xbd);
            this.textBoxParallel3From.Name = "textBoxParallel3From";
            this.textBoxParallel3From.Size = new Size(40, 20);
            this.textBoxParallel3From.TabIndex = 14;
            this.checkBoxParallel3.AutoSize = true;
            this.checkBoxParallel3.Location = new Point(13, 0x9d);
            this.checkBoxParallel3.Name = "checkBoxParallel3";
            this.checkBoxParallel3.Size = new Size(0x5b, 0x11);
            this.checkBoxParallel3.TabIndex = 13;
            this.checkBoxParallel3.Text = "Use Parallel 3";
            this.checkBoxParallel3.UseVisualStyleBackColor = true;
            this.checkBoxParallel3.Visible = false;
            this.textBoxParallel2To.Location = new Point(0xd4, 120);
            this.textBoxParallel2To.Name = "textBoxParallel2To";
            this.textBoxParallel2To.Size = new Size(40, 20);
            this.textBoxParallel2To.TabIndex = 10;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(190, 0x7b);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x10, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "to";
            this.textBoxParallel2From.Location = new Point(0x90, 120);
            this.textBoxParallel2From.Name = "textBoxParallel2From";
            this.textBoxParallel2From.Size = new Size(40, 20);
            this.textBoxParallel2From.TabIndex = 8;
            this.checkBoxParallel2.AutoSize = true;
            this.checkBoxParallel2.Location = new Point(13, 0x58);
            this.checkBoxParallel2.Name = "checkBoxParallel2";
            this.checkBoxParallel2.Size = new Size(0x5b, 0x11);
            this.checkBoxParallel2.TabIndex = 7;
            this.checkBoxParallel2.Text = "Use Parallel 2";
            this.checkBoxParallel2.UseVisualStyleBackColor = true;
            this.checkBoxParallel2.Visible = false;
            this.textBoxParallel1To.Location = new Point(0xd4, 0x33);
            this.textBoxParallel1To.Name = "textBoxParallel1To";
            this.textBoxParallel1To.Size = new Size(40, 20);
            this.textBoxParallel1To.TabIndex = 4;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(190, 0x36);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x10, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "to";
            this.textBoxParallel1From.Location = new Point(0x90, 0x33);
            this.textBoxParallel1From.Name = "textBoxParallel1From";
            this.textBoxParallel1From.Size = new Size(40, 20);
            this.textBoxParallel1From.TabIndex = 2;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0xa1, 0x10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x51, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Channel Range";
            this.checkBoxParallel1.AutoSize = true;
            this.checkBoxParallel1.Location = new Point(13, 0x13);
            this.checkBoxParallel1.Name = "checkBoxParallel1";
            this.checkBoxParallel1.Size = new Size(0x5b, 0x11);
            this.checkBoxParallel1.TabIndex = 1;
            this.checkBoxParallel1.Text = "Use Parallel 1";
            this.checkBoxParallel1.UseVisualStyleBackColor = true;
            this.checkBoxParallel1.Visible = false;
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(0x103, 280);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(340, 280);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.label5.AutoSize = true;
            this.label5.Location = new Point(10, 0x36);
            this.label5.Name = "label5";
            this.label5.Size = new Size(50, 13);
            this.label5.TabIndex = 0x13;
            this.label5.Text = "Parallel 1";
            this.label6.AutoSize = true;
            this.label6.Location = new Point(10, 0x7b);
            this.label6.Name = "label6";
            this.label6.Size = new Size(50, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Parallel 2";
            this.label7.AutoSize = true;
            this.label7.Location = new Point(10, 0xc0);
            this.label7.Name = "label7";
            this.label7.Size = new Size(50, 13);
            this.label7.TabIndex = 0x15;
            this.label7.Text = "Parallel 3";
            base.AcceptButton = this.buttonOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x1a9, 0x13b);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "SetupDialog";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Setup";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
        }
    }
}

