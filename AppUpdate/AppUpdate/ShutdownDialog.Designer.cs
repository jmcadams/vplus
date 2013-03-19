namespace AppUpdate {
	using System;
	using System.Windows.Forms;
	using System.Drawing;
	using System.ComponentModel;
	using System.Collections;

	internal partial class ShutdownDialog {
		private IContainer components;

		#region Windows Form Designer generated code
		private Button buttonAbort;
		private Button buttonKill;
		private Button buttonRetry;
		private Label label1;
		private Label labelMessage;
		private PictureBox pictureBox1;

		private void InitializeComponent() {
			this.pictureBox1 = new PictureBox();
			this.labelMessage = new Label();
			this.label1 = new Label();
			this.buttonRetry = new Button();
			this.buttonAbort = new Button();
			this.buttonKill = new Button();
			((ISupportInitialize)this.pictureBox1).BeginInit();
			base.SuspendLayout();
			this.pictureBox1.Location = new Point(13, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new Size(0x23, 0x2a);
			this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 3;
			this.pictureBox1.TabStop = false;
			this.labelMessage.AutoSize = true;
			this.labelMessage.Location = new Point(0x41, 15);
			this.labelMessage.Name = "labelMessage";
			this.labelMessage.Size = new Size(0x148, 0x4e);
			this.labelMessage.TabIndex = 0;
			this.label1.AutoSize = true;
			this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.label1.Location = new Point(0x41, 110);
			this.label1.Name = "label1";
			this.label1.Size = new Size(0x142, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Aborting may leave the application in an impaired state.";
			this.buttonRetry.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
			this.buttonRetry.DialogResult = System.Windows.Forms.DialogResult.Retry;
			this.buttonRetry.Location = new Point(0x61, 0x9a);
			this.buttonRetry.Name = "buttonRetry";
			this.buttonRetry.Size = new Size(0x4b, 0x17);
			this.buttonRetry.TabIndex = 2;
			this.buttonRetry.Text = "Retry";
			this.buttonRetry.UseVisualStyleBackColor = true;
			this.buttonAbort.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
			this.buttonAbort.DialogResult = System.Windows.Forms.DialogResult.Abort;
			this.buttonAbort.Location = new Point(0x10f, 0x9a);
			this.buttonAbort.Name = "buttonAbort";
			this.buttonAbort.Size = new Size(0x4b, 0x17);
			this.buttonAbort.TabIndex = 4;
			this.buttonAbort.Text = "Abort";
			this.buttonAbort.UseVisualStyleBackColor = true;
			this.buttonKill.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
			this.buttonKill.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonKill.Location = new Point(0xb8, 0x9a);
			this.buttonKill.Name = "buttonKill";
			this.buttonKill.Size = new Size(0x4b, 0x17);
			this.buttonKill.TabIndex = 3;
			this.buttonKill.Text = "Kill";
			this.buttonKill.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new Size(0x1ba, 0xc6);
			base.ControlBox = false;
			base.Controls.Add(this.buttonKill);
			base.Controls.Add(this.buttonAbort);
			base.Controls.Add(this.buttonRetry);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.pictureBox1);
			base.Controls.Add(this.labelMessage);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Name = "ShutdownDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Application Shutdown";
			base.TopMost = true;
			((ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
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