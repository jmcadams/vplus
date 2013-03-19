using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace StandardScript {
	internal partial class TextListDialog {
		private IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCancel;
		private Button buttonOK;
		private TextBox textBoxItems;

		private void InitializeComponent() {
			this.buttonOK = new Button();
			this.textBoxItems = new TextBox();
			this.buttonCancel = new Button();
			base.SuspendLayout();
			this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new Point(0x76, 0x11b);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(0x4b, 0x17);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.textBoxItems.AcceptsReturn = true;
			this.textBoxItems.Location = new Point(12, 12);
			this.textBoxItems.Multiline = true;
			this.textBoxItems.Name = "textBoxItems";
			this.textBoxItems.ScrollBars = ScrollBars.Vertical;
			this.textBoxItems.Size = new Size(0x106, 0x102);
			this.textBoxItems.TabIndex = 0;
			this.textBoxItems.WordWrap = false;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(0xc7, 0x11b);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 0x17);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(0x11e, 0x13e);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.textBoxItems);
			base.Controls.Add(this.buttonOK);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "TextListDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "TextListDialog";
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