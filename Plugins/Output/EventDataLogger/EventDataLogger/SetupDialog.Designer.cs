namespace EventDataLogger {
	using System;
	using System.Drawing;
	using System.Windows.Forms;

	public partial class SetupDialog {
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCancel;
		private Button buttonClearLog;
		private Button buttonOK;
		private Button buttonViewLog;
		private GroupBox groupBox1;

		private void InitializeComponent() {
			this.groupBox1 = new GroupBox();
			this.buttonClearLog = new Button();
			this.buttonViewLog = new Button();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.groupBox1.Controls.Add(this.buttonClearLog);
			this.groupBox1.Controls.Add(this.buttonViewLog);
			this.groupBox1.Location = new Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(0xe3, 0x52);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Logging Setup";
			this.buttonClearLog.Enabled = false;
			this.buttonClearLog.Location = new Point(0x81, 0x24);
			this.buttonClearLog.Name = "buttonClearLog";
			this.buttonClearLog.Size = new Size(0x4b, 23);
			this.buttonClearLog.TabIndex = 1;
			this.buttonClearLog.Text = "Clear Log";
			this.buttonClearLog.UseVisualStyleBackColor = true;
			this.buttonClearLog.Click += new EventHandler(this.buttonClearLog_Click);
			this.buttonViewLog.Enabled = false;
			this.buttonViewLog.Location = new Point(23, 0x24);
			this.buttonViewLog.Name = "buttonViewLog";
			this.buttonViewLog.Size = new Size(0x4b, 23);
			this.buttonViewLog.TabIndex = 0;
			this.buttonViewLog.Text = "View Log";
			this.buttonViewLog.UseVisualStyleBackColor = true;
			this.buttonViewLog.Click += new EventHandler(this.buttonViewLog_Click);
			this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new Point(0x53, 100);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(0x4b, 23);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(0xa4, 100);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 23);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			base.AcceptButton = this.buttonOK;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(0xfb, 0x87);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.groupBox1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Name = "SetupDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Setup";
			this.groupBox1.ResumeLayout(false);
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