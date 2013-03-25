using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace StandardScript {
	public partial class ChannelNameEditDialog {
		private IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCancel;
		private Button buttonOK;
		private TextBox textBoxChannels;

		private void InitializeComponent() {
			this.textBoxChannels = new TextBox();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			base.SuspendLayout();
			this.textBoxChannels.AcceptsReturn = true;
			this.textBoxChannels.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.textBoxChannels.Location = new Point(12, 12);
			this.textBoxChannels.Multiline = true;
			this.textBoxChannels.Name = "textBoxChannels";
			this.textBoxChannels.ScrollBars = ScrollBars.Both;
			this.textBoxChannels.Size = new Size(0x9a, 0x18f);
			this.textBoxChannels.TabIndex = 0;
			this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new Point(10, 0x1a1);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(0x4b, 23);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(0x5b, 0x1a1);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 23);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(0xb2, 0x1c5);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.textBoxChannels);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ChannelNameEditDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Channel Names";
			base.Activated += new EventHandler(this.ChannelNameEditDialog_Activated);
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