namespace FC_4 {
	using System;
	using System.Drawing;
	using System.Windows.Forms;

	internal partial class frmSetupDialog {
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code
		private Button btnCancel;
		private Button btnOK;
		private ComboBox comboBoxBaud;
		private GroupBox grpPort;
		private Label lblBaud;
		private Label lblPort;
		private NumericUpDown nudPort;

		private void InitializeComponent() {
			this.grpPort = new GroupBox();
			this.comboBoxBaud = new ComboBox();
			this.lblBaud = new Label();
			this.nudPort = new NumericUpDown();
			this.lblPort = new Label();
			this.btnCancel = new Button();
			this.btnOK = new Button();
			this.grpPort.SuspendLayout();
			this.nudPort.BeginInit();
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
			this.comboBoxBaud.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxBaud.FormattingEnabled = true;
			this.comboBoxBaud.Items.AddRange(new object[] { "38400", "2400" });
			this.comboBoxBaud.Location = new Point(0x94, 0x11);
			this.comboBoxBaud.Name = "comboBoxBaud";
			this.comboBoxBaud.Size = new Size(0x3f, 0x15);
			this.comboBoxBaud.TabIndex = 3;
			this.lblBaud.AutoSize = true;
			this.lblBaud.Location = new Point(0x6a, 20);
			this.lblBaud.Name = "lblBaud";
			this.lblBaud.Size = new Size(0x20, 13);
			this.lblBaud.TabIndex = 2;
			this.lblBaud.Text = "Baud";
			this.nudPort.Location = new Point(0x2b, 0x12);
			int[] bitsMax = new int[4];
			bitsMax[0] = 4;
			this.nudPort.Maximum = new decimal(bitsMax);
			int[] bitsMin = new int[4];
			bitsMin[0] = 1;
			this.nudPort.Minimum = new decimal(bitsMin);
			this.nudPort.Name = "nudPort";
			this.nudPort.Size = new Size(0x23, 20);
			this.nudPort.TabIndex = 1;
			int[] bitsPort = new int[4];
			bitsPort[0] = 1;
			this.nudPort.Value = new decimal(bitsPort);
			this.lblPort.AutoSize = true;
			this.lblPort.Location = new Point(6, 20);
			this.lblPort.Name = "lblPort";
			this.lblPort.Size = new Size(0x1f, 13);
			this.lblPort.TabIndex = 0;
			this.lblPort.Text = "COM";
			this.btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new Point(160, 0x49);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new Size(0x4b, 23);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new Point(0x4f, 0x48);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new Size(0x4b, 23);
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new EventHandler(this.btnOK_Click);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.btnCancel;
			base.ClientSize = new Size(0xf8, 0x6c);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.grpPort);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Name = "frmSetupDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Setup";
			this.grpPort.ResumeLayout(false);
			this.grpPort.PerformLayout();
			this.nudPort.EndInit();
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