namespace FanslerDimmer {
	using System;
	using System.Drawing;
	using System.Windows.Forms;

	public partial class DimmingDialog {
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonAll;
		private Button buttonCancel;
		private Button buttonNone;
		private Button buttonOK;
		private Button buttonRange;
		private CheckedListBox checkedListBoxChannels;
		private Label label1;

		private void InitializeComponent() {
			this.label1 = new Label();
			this.checkedListBoxChannels = new CheckedListBox();
			this.buttonCancel = new Button();
			this.buttonAll = new Button();
			this.buttonNone = new Button();
			this.buttonRange = new Button();
			this.buttonOK = new Button();
			base.SuspendLayout();
			this.label1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
			this.label1.Location = new Point(15, 14);
			this.label1.Name = "label1";
			this.label1.Size = new Size(0x15a, 28);
			this.label1.TabIndex = 0;
			this.label1.Text = "Select the channels that are dimmable according to the hardware configuration.";
			this.checkedListBoxChannels.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.checkedListBoxChannels.CheckOnClick = true;
			this.checkedListBoxChannels.FormattingEnabled = true;
			this.checkedListBoxChannels.Location = new Point(15, 0x33);
			this.checkedListBoxChannels.Name = "checkedListBoxChannels";
			this.checkedListBoxChannels.Size = new Size(0x159, 0xb8);
			this.checkedListBoxChannels.TabIndex = 1;
			this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(0x11d, 0x111);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 23);
			this.buttonCancel.TabIndex = 6;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonAll.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
			this.buttonAll.Location = new Point(0x10, 240);
			this.buttonAll.Name = "buttonAll";
			this.buttonAll.Size = new Size(0x4b, 23);
			this.buttonAll.TabIndex = 2;
			this.buttonAll.Text = "All";
			this.buttonAll.UseVisualStyleBackColor = true;
			this.buttonAll.Click += new EventHandler(this.buttonAll_Click);
			this.buttonNone.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
			this.buttonNone.Location = new Point(0x61, 240);
			this.buttonNone.Name = "buttonNone";
			this.buttonNone.Size = new Size(0x4b, 23);
			this.buttonNone.TabIndex = 3;
			this.buttonNone.Text = "None";
			this.buttonNone.UseVisualStyleBackColor = true;
			this.buttonNone.Click += new EventHandler(this.buttonNone_Click);
			this.buttonRange.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
			this.buttonRange.Location = new Point(0xb2, 240);
			this.buttonRange.Name = "buttonRange";
			this.buttonRange.Size = new Size(0x4b, 23);
			this.buttonRange.TabIndex = 4;
			this.buttonRange.Text = "Range";
			this.buttonRange.UseVisualStyleBackColor = true;
			this.buttonRange.Click += new EventHandler(this.buttonRange_Click);
			this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new Point(0xcc, 0x111);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(0x4b, 23);
			this.buttonOK.TabIndex = 5;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			base.AcceptButton = this.buttonOK;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(0x175, 0x134);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.buttonRange);
			base.Controls.Add(this.buttonNone);
			base.Controls.Add(this.buttonAll);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.checkedListBoxChannels);
			base.Controls.Add(this.label1);
			base.Name = "DimmingDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Dimming Configuration";
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