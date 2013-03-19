using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace StandardScript {
	internal partial class ModuleListDialog {
		private IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCancel;
		private Button buttonOK;
		private CheckedListBox checkedListBoxModules;
		private Label label1;

		private void InitializeComponent() {
			this.label1 = new Label();
			this.checkedListBoxModules = new CheckedListBox();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			base.SuspendLayout();
			this.label1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
			this.label1.Location = new Point(13, 14);
			this.label1.Name = "label1";
			this.label1.Size = new Size(0x10b, 0x1d);
			this.label1.TabIndex = 0;
			this.label1.Text = "Select the script modules you will be using.";
			this.checkedListBoxModules.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
			this.checkedListBoxModules.CheckOnClick = true;
			this.checkedListBoxModules.FormattingEnabled = true;
			this.checkedListBoxModules.Location = new Point(0x10, 0x2e);
			this.checkedListBoxModules.Name = "checkedListBoxModules";
			this.checkedListBoxModules.Size = new Size(0x108, 0xa9);
			this.checkedListBoxModules.TabIndex = 1;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new Point(0x7c, 0xe7);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(0x4b, 0x17);
			this.buttonOK.TabIndex = 2;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(0xcd, 0xe7);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 0x17);
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(0x124, 0x10a);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.checkedListBoxModules);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Name = "ModuleListDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Module List";
			base.Load += new EventHandler(this.ModuleListDialog_Load);
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