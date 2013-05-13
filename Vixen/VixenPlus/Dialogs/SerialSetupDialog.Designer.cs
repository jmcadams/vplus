namespace VixenPlus.Dialogs{
    using System;
    using System.Windows.Forms;
    using System.Drawing;
    using System.ComponentModel;
    using System.Collections;

    public partial class SerialSetupDialog{
        private IContainer components;

        #region Windows Form Designer generated code
        private Button buttonCancel;
private Button buttonOK;
private ComboBox comboBoxBaudRate;
private ComboBox comboBoxParity;
private ComboBox comboBoxPortName;
private ComboBox comboBoxStop;
private GroupBox groupBox1;
private Label label2;
private Label label3;
private Label label4;
private Label label5;
private Label label6;
private NumericUpDown numericUpDownPort;
private TextBox textBoxData;

        private void InitializeComponent()
        {
            this.groupBox1 = new GroupBox();
            this.comboBoxBaudRate = new ComboBox();
            this.numericUpDownPort = new NumericUpDown();
            this.comboBoxStop = new ComboBox();
            this.label6 = new Label();
            this.comboBoxParity = new ComboBox();
            this.label2 = new Label();
            this.label5 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.textBoxData = new TextBox();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.comboBoxPortName = new ComboBox();
            this.groupBox1.SuspendLayout();
            this.numericUpDownPort.BeginInit();
            base.SuspendLayout();
            this.groupBox1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.comboBoxPortName);
            this.groupBox1.Controls.Add(this.comboBoxBaudRate);
            this.groupBox1.Controls.Add(this.numericUpDownPort);
            this.groupBox1.Controls.Add(this.comboBoxStop);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.comboBoxParity);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBoxData);
            this.groupBox1.Location = new Point(9, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(305, 125);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Serial Port";
            this.comboBoxBaudRate.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxBaudRate.FormattingEnabled = true;
            this.comboBoxBaudRate.Items.AddRange(new object[] { "2400", "4800", "9600", "19200", "38400", "57600", "115200" });
            this.comboBoxBaudRate.Location = new Point(65, 63);
            this.comboBoxBaudRate.Name = "comboBoxBaudRate";
            this.comboBoxBaudRate.Size = new Size(75, 21);
            this.comboBoxBaudRate.TabIndex = 2;
            this.numericUpDownPort.Location = new Point(45, 14);
            int[] bits = new int[4];
            bits[0] = 99;
            this.numericUpDownPort.Maximum = new decimal(bits);
            int[] bitsPortMin = new int[4];
            bitsPortMin[0] = 1;
            this.numericUpDownPort.Minimum = new decimal(bitsPortMin);
            this.numericUpDownPort.Name = "numericUpDownPort";
            this.numericUpDownPort.Size = new Size(39, 20);
            this.numericUpDownPort.TabIndex = 10;
            int[] bitsPortValue = new int[4];
            bitsPortValue[0] = 1;
            this.numericUpDownPort.Value = new decimal(bitsPortValue);
            this.numericUpDownPort.Visible = false;
            this.comboBoxStop.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxStop.FormattingEnabled = true;
            this.comboBoxStop.Location = new Point(200, 88);
            this.comboBoxStop.Name = "comboBoxStop";
            this.comboBoxStop.Size = new Size(95, 21);
            this.comboBoxStop.TabIndex = 8;
            this.label6.AutoSize = true;
            this.label6.Location = new Point(8, 16);
            this.label6.Name = "label6";
            this.label6.Size = new Size(31, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "COM";
            this.label6.Visible = false;
            this.comboBoxParity.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxParity.FormattingEnabled = true;
            this.comboBoxParity.Location = new Point(65, 88);
            this.comboBoxParity.Name = "comboBoxParity";
            this.comboBoxParity.Size = new Size(75, 21);
            this.comboBoxParity.TabIndex = 4;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(6, 66);
            this.label2.Name = "label2";
            this.label2.Size = new Size(53, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Baud rate";
            this.label5.AutoSize = true;
            this.label5.Location = new Point(6, 92);
            this.label5.Name = "label5";
            this.label5.Size = new Size(33, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Parity";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(146, 66);
            this.label3.Name = "label3";
            this.label3.Size = new Size(49, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Data bits";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(146, 92);
            this.label4.Name = "label4";
            this.label4.Size = new Size(48, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Stop bits";
            this.textBoxData.Location = new Point(201, 63);
            this.textBoxData.MaxLength = 1;
            this.textBoxData.Name = "textBoxData";
            this.textBoxData.Size = new Size(39, 20);
            this.textBoxData.TabIndex = 6;
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(158, 144);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(239, 144);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.comboBoxPortName.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxPortName.FormattingEnabled = true;
            this.comboBoxPortName.Location = new Point(122, 19);
            this.comboBoxPortName.Name = "comboBoxPortName";
            this.comboBoxPortName.Size = new Size(72, 21);
            this.comboBoxPortName.TabIndex = 0;
            base.AcceptButton = this.buttonOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(326, 179);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Name = "SerialSetupDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Setup";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.numericUpDownPort.EndInit();
            base.ResumeLayout(false);
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
