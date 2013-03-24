namespace PSC {
	using System;
	using System.Windows.Forms;
	using System.Drawing;
	using System.Collections;

	public partial class SetupDialog {
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCancel;
		private Button buttonOK;
		private Button buttonSerialSetup;
		private CheckBox checkBoxRamp;
		private GroupBox groupBox1;
		private GroupBox groupBox2;

		private void InitializeComponent() {
			this.groupBox1 = new GroupBox();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.buttonSerialSetup = new Button();
			this.groupBox2 = new GroupBox();
			this.checkBoxRamp = new CheckBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			base.SuspendLayout();
			this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
			this.groupBox1.Controls.Add(this.buttonSerialSetup);
			this.groupBox1.Location = new Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(0x131, 0x4f);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Serial";
			this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new Point(0xa1, 0xc3);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(0x4b, 0x17);
			this.buttonOK.TabIndex = 2;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(0xf2, 0xc3);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 0x17);
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonSerialSetup.Location = new Point(0x73, 0x1d);
			this.buttonSerialSetup.Name = "buttonSerialSetup";
			this.buttonSerialSetup.Size = new Size(0x4b, 0x17);
			this.buttonSerialSetup.TabIndex = 0;
			this.buttonSerialSetup.Text = "Serial Setup";
			this.buttonSerialSetup.UseVisualStyleBackColor = true;
			this.buttonSerialSetup.Click += new EventHandler(this.buttonSerialSetup_Click);
			this.groupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.groupBox2.Controls.Add(this.checkBoxRamp);
			this.groupBox2.Location = new Point(12, 0x6b);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(0x131, 0x52);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Servo Ramps";
			this.checkBoxRamp.AutoSize = true;
			this.checkBoxRamp.Location = new Point(0x22, 0x24);
			this.checkBoxRamp.Name = "checkBoxRamp";
			this.checkBoxRamp.Size = new Size(260, 0x11);
			this.checkBoxRamp.TabIndex = 0;
			this.checkBoxRamp.Text = "Attempt to smooth ramp transitions when possible.";
			this.checkBoxRamp.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(0x149, 230);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.groupBox1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "SetupDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Setup";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
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