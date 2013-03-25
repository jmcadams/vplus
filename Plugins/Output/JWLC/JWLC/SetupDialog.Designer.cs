namespace JWLC{
	using System;
	using System.Drawing;
	using System.Windows.Forms;

	internal partial class SetupDialog{
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCancel;
private Button buttonOK;
private ComboBox comboBoxBaud;
private GroupBox groupBox1;
private Label label1;
private Label label2;
private NumericUpDown numericUpDownPort;

		private void InitializeComponent()
		{
			this.groupBox1 = new GroupBox();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.label1 = new Label();
			this.numericUpDownPort = new NumericUpDown();
			this.label2 = new Label();
			this.comboBoxBaud = new ComboBox();
			this.groupBox1.SuspendLayout();
			this.numericUpDownPort.BeginInit();
			base.SuspendLayout();
			this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.groupBox1.Controls.Add(this.comboBoxBaud);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.numericUpDownPort);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(240, 0x47);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Serial Port";
			this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new Point(0x60, 0x59);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(0x4b, 23);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(0xb1, 0x59);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 23);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.label1.AutoSize = true;
			this.label1.Location = new Point(14, 29);
			this.label1.Name = "label1";
			this.label1.Size = new Size(0x1f, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "COM";
			this.numericUpDownPort.Location = new Point(0x33, 27);
			int[] bits = new int[4];
			bits[0] = 4;
			this.numericUpDownPort.Maximum = new decimal(bits);
			int[] bitsMin = new int[4];
			bitsMin[0] = 1;
			this.numericUpDownPort.Minimum = new decimal(bitsMin);
			this.numericUpDownPort.Name = "numericUpDownPort";
			this.numericUpDownPort.Size = new Size(0x2f, 20);
			this.numericUpDownPort.TabIndex = 1;
			int[] bitsValue = new int[4];
			bitsValue[0] = 1;
			this.numericUpDownPort.Value = new decimal(bitsValue);
			this.label2.AutoSize = true;
			this.label2.Location = new Point(0x73, 29);
			this.label2.Name = "label2";
			this.label2.Size = new Size(0x20, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Baud";
			this.comboBoxBaud.FormattingEnabled = true;
			this.comboBoxBaud.Items.AddRange(new object[] { "9600", "19200", "38400" });
			this.comboBoxBaud.Location = new Point(0x99, 26);
			this.comboBoxBaud.Name = "comboBoxBaud";
			this.comboBoxBaud.Size = new Size(0x41, 0x15);
			this.comboBoxBaud.TabIndex = 3;
			base.AcceptButton = this.buttonOK;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(0x108, 0x7c);
			base.ControlBox = false;
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.groupBox1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Name = "SetupDialog";
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
