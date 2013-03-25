namespace FanslerDimmer {
	using System;
	using System.Drawing;
	using System.Windows.Forms;

	public partial class RangeDialog {
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCancel;
		private Button buttonOK;
		private Label label1;
		private Label label2;
		private Label label3;
		private RadioButton radioButtonSelected;
		private RadioButton radioButtonUnselected;
		private TextBox textBoxFrom;
		private TextBox textBoxTo;

		private void InitializeComponent() {
			this.label1 = new Label();
			this.textBoxFrom = new TextBox();
			this.label2 = new Label();
			this.textBoxTo = new TextBox();
			this.label3 = new Label();
			this.radioButtonSelected = new RadioButton();
			this.radioButtonUnselected = new RadioButton();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(13, 0x15);
			this.label1.Name = "label1";
			this.label1.Size = new Size(0x42, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Affects ports";
			this.textBoxFrom.Location = new Point(0x55, 0x12);
			this.textBoxFrom.Name = "textBoxFrom";
			this.textBoxFrom.Size = new Size(0x29, 20);
			this.textBoxFrom.TabIndex = 1;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(0x84, 0x15);
			this.label2.Name = "label2";
			this.label2.Size = new Size(0x10, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "to";
			this.textBoxTo.Location = new Point(0x9a, 0x12);
			this.textBoxTo.Name = "textBoxTo";
			this.textBoxTo.Size = new Size(0x29, 20);
			this.textBoxTo.TabIndex = 3;
			this.label3.AutoSize = true;
			this.label3.Location = new Point(13, 70);
			this.label3.Name = "label3";
			this.label3.Size = new Size(0x3d, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Set to state";
			this.radioButtonSelected.AutoSize = true;
			this.radioButtonSelected.Checked = true;
			this.radioButtonSelected.Location = new Point(0x55, 0x44);
			this.radioButtonSelected.Name = "radioButtonSelected";
			this.radioButtonSelected.Size = new Size(0x43, 0x11);
			this.radioButtonSelected.TabIndex = 5;
			this.radioButtonSelected.TabStop = true;
			this.radioButtonSelected.Text = "Selected";
			this.radioButtonSelected.UseVisualStyleBackColor = true;
			this.radioButtonUnselected.AutoSize = true;
			this.radioButtonUnselected.Location = new Point(0x9e, 0x45);
			this.radioButtonUnselected.Name = "radioButtonUnselected";
			this.radioButtonUnselected.Size = new Size(0x4f, 0x11);
			this.radioButtonUnselected.TabIndex = 6;
			this.radioButtonUnselected.Text = "Unselected";
			this.radioButtonUnselected.UseVisualStyleBackColor = true;
			this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new Point(0x59, 0x84);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(0x4b, 23);
			this.buttonOK.TabIndex = 7;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(170, 0x84);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 23);
			this.buttonCancel.TabIndex = 8;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			base.AcceptButton = this.buttonOK;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(0x101, 0xa7);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.radioButtonUnselected);
			base.Controls.Add(this.radioButtonSelected);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.textBoxTo);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.textBoxFrom);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "RangeDialog";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Set a range";
			base.FormClosing += new FormClosingEventHandler(this.RangeDialog_FormClosing);
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