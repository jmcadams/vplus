namespace Renard {
	using System;
	using System.Windows.Forms;
	using System.Drawing;
	using System.ComponentModel;
	using System.Collections;

	public partial class SetupDialog {
		private IContainer components;

		#region Windows Form Designer generated code
		private Button buttonCancel;
		private Button buttonOK;
		private Button buttonSerialSetup;
		private CheckBox checkBoxHoldPort;
		private ComboBox comboBoxProtocolVersion;
		private GroupBox groupBox1;
		private Label label6;

		private void InitializeComponent() {
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.label6 = new Label();
			this.comboBoxProtocolVersion = new ComboBox();
			this.buttonSerialSetup = new Button();
			this.groupBox1 = new GroupBox();
			this.checkBoxHoldPort = new CheckBox();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new Point(0x71, 0xa8);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(0x4b, 0x17);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(0xc2, 0xa8);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 0x17);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.label6.AutoSize = true;
			this.label6.Location = new Point(0x16, 0x1a);
			this.label6.Name = "label6";
			this.label6.Size = new Size(0x57, 13);
			this.label6.TabIndex = 0;
			this.label6.Text = "Protocol Version:";
			this.comboBoxProtocolVersion.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxProtocolVersion.FormattingEnabled = true;
			this.comboBoxProtocolVersion.Items.AddRange(new object[] { "1", "2" });
			this.comboBoxProtocolVersion.Location = new Point(0x73, 0x17);
			this.comboBoxProtocolVersion.Name = "comboBoxProtocolVersion";
			this.comboBoxProtocolVersion.Size = new Size(0x2c, 0x15);
			this.comboBoxProtocolVersion.TabIndex = 1;
			this.buttonSerialSetup.Location = new Point(0x19, 0x6b);
			this.buttonSerialSetup.Name = "buttonSerialSetup";
			this.buttonSerialSetup.Size = new Size(0x4b, 0x17);
			this.buttonSerialSetup.TabIndex = 3;
			this.buttonSerialSetup.Text = "Serial Setup";
			this.buttonSerialSetup.UseVisualStyleBackColor = true;
			this.buttonSerialSetup.Click += new EventHandler(this.buttonSerialSetup_Click);
			this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.groupBox1.Controls.Add(this.checkBoxHoldPort);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.buttonSerialSetup);
			this.groupBox1.Controls.Add(this.comboBoxProtocolVersion);
			this.groupBox1.Location = new Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(0x101, 150);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Setup";
			this.checkBoxHoldPort.Location = new Point(0x19, 0x3a);
			this.checkBoxHoldPort.Name = "checkBoxHoldPort";
			this.checkBoxHoldPort.Size = new Size(0xe2, 0x24);
			this.checkBoxHoldPort.TabIndex = 2;
			this.checkBoxHoldPort.Text = "Hold port open during the duration of the sequence execution.";
			this.checkBoxHoldPort.UseVisualStyleBackColor = true;
			base.AcceptButton = this.buttonOK;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(0x119, 0xcb);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Name = "SetupDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Setup";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
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