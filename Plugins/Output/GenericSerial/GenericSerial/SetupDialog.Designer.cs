namespace GenericSerial {
	using System;
	using System.Drawing;
	using System.Windows.Forms;

	internal partial class SetupDialog {
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCancel;
		private Button buttonOK;
		private CheckBox checkBoxFooter;
		private CheckBox checkBoxHeader;
		private ComboBox comboBoxBaud;
		private GroupBox groupBox1;
		private GroupBox groupBox2;
		private Label label1;
		private Label label2;
		private NumericUpDown numericUpDownPort;
		private TextBox textBoxFooter;
		private TextBox textBoxHeader;

		private void InitializeComponent() {
			this.groupBox1 = new GroupBox();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.groupBox2 = new GroupBox();
			this.label1 = new Label();
			this.numericUpDownPort = new NumericUpDown();
			this.label2 = new Label();
			this.comboBoxBaud = new ComboBox();
			this.checkBoxHeader = new CheckBox();
			this.textBoxHeader = new TextBox();
			this.checkBoxFooter = new CheckBox();
			this.textBoxFooter = new TextBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.numericUpDownPort.BeginInit();
			base.SuspendLayout();
			this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
			this.groupBox1.Controls.Add(this.comboBoxBaud);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.numericUpDownPort);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(0x103, 0x45);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Serial Port";
			this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new Point(0x73, 0xe7);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(0x4b, 0x17);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(0xc4, 0xe7);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 0x17);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.groupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
			this.groupBox2.Controls.Add(this.textBoxFooter);
			this.groupBox2.Controls.Add(this.checkBoxFooter);
			this.groupBox2.Controls.Add(this.textBoxHeader);
			this.groupBox2.Controls.Add(this.checkBoxHeader);
			this.groupBox2.Location = new Point(12, 0x57);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(0x103, 0x8a);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Packet Data";
			this.label1.AutoSize = true;
			this.label1.Location = new Point(15, 0x19);
			this.label1.Name = "label1";
			this.label1.Size = new Size(0x1f, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "COM";
			this.numericUpDownPort.Location = new Point(0x34, 0x17);
			int[] bitsMax = new int[4];
			bitsMax[0] = 4;
			this.numericUpDownPort.Maximum = new decimal(bitsMax);
			int[] bitsMin = new int[4];
			bitsMin[0] = 1;
			this.numericUpDownPort.Minimum = new decimal(bitsMin);
			this.numericUpDownPort.Name = "numericUpDownPort";
			this.numericUpDownPort.Size = new Size(0x2e, 20);
			this.numericUpDownPort.TabIndex = 1;
			int[] bitsPort = new int[4];
			bitsPort[0] = 1;
			this.numericUpDownPort.Value = new decimal(bitsPort);
			this.label2.AutoSize = true;
			this.label2.Location = new Point(0x77, 0x19);
			this.label2.Name = "label2";
			this.label2.Size = new Size(0x20, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Baud";
			this.comboBoxBaud.FormattingEnabled = true;
			this.comboBoxBaud.Items.AddRange(new object[] { "2400", "9600", "19200", "38400", "57600" });
			this.comboBoxBaud.Location = new Point(0x9d, 0x16);
			this.comboBoxBaud.Name = "comboBoxBaud";
			this.comboBoxBaud.Size = new Size(0x4f, 0x15);
			this.comboBoxBaud.TabIndex = 3;
			this.checkBoxHeader.AutoSize = true;
			this.checkBoxHeader.Location = new Point(13, 0x1a);
			this.checkBoxHeader.Name = "checkBoxHeader";
			this.checkBoxHeader.Size = new Size(0x74, 0x11);
			this.checkBoxHeader.TabIndex = 0;
			this.checkBoxHeader.Text = "Send a text header";
			this.checkBoxHeader.UseVisualStyleBackColor = true;
			this.textBoxHeader.Location = new Point(0x20, 0x31);
			this.textBoxHeader.Name = "textBoxHeader";
			this.textBoxHeader.Size = new Size(0xcc, 20);
			this.textBoxHeader.TabIndex = 1;
			this.checkBoxFooter.AutoSize = true;
			this.checkBoxFooter.Location = new Point(13, 0x54);
			this.checkBoxFooter.Name = "checkBoxFooter";
			this.checkBoxFooter.Size = new Size(110, 0x11);
			this.checkBoxFooter.TabIndex = 2;
			this.checkBoxFooter.Text = "Send a text footer";
			this.checkBoxFooter.UseVisualStyleBackColor = true;
			this.textBoxFooter.Location = new Point(0x20, 0x6b);
			this.textBoxFooter.Name = "textBoxFooter";
			this.textBoxFooter.Size = new Size(0xcc, 20);
			this.textBoxFooter.TabIndex = 3;
			base.AcceptButton = this.buttonOK;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(0x11b, 0x10a);
			base.ControlBox = false;
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.groupBox1);
			base.Name = "SetupDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Setup";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.numericUpDownPort.EndInit();
			base.ResumeLayout(false);
		}
		#endregion

		protected override void Dispose(bool disposing) {
			if (disposing && (this.components != null)) {
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}