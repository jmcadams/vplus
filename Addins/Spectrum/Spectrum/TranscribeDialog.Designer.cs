using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace Spectrum {
	internal partial class TranscribeDialog : Form {
		private IContainer components = null;

		#region Windows Form Designer generated code
		private ProgressBar progressBar;

		private void InitializeComponent() {
			this.progressBar = new ProgressBar();
			base.SuspendLayout();
			this.progressBar.Location = new Point(12, 22);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new Size(0x13a, 23);
			this.progressBar.TabIndex = 0;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new Size(0x152, 0x42);
			base.ControlBox = false;
			base.Controls.Add(this.progressBar);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Name = "TranscribeDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Transcribing...";
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