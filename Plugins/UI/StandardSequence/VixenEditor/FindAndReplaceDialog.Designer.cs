using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace VixenEditor {
	public partial class FindAndReplaceDialog {
		private IContainer components;

		#region Windows Form Designer generated code
		private Button buttonCancel;
		private Button buttonOK;
		private GroupBox groupBox1;
		private Label label1;
		private Label label2;
		private Label label3;
		private NumericUpDown numericUpDownFind;
		private NumericUpDown numericUpDownReplaceWith;

		private void InitializeComponent() {
			this.groupBox1 = new GroupBox();
			this.numericUpDownReplaceWith = new NumericUpDown();
			this.label3 = new Label();
			this.numericUpDownFind = new NumericUpDown();
			this.label2 = new Label();
			this.label1 = new Label();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.groupBox1.SuspendLayout();
			this.numericUpDownReplaceWith.BeginInit();
			this.numericUpDownFind.BeginInit();
			base.SuspendLayout();
			this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.groupBox1.Controls.Add(this.numericUpDownReplaceWith);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.numericUpDownFind);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(230, 0xac);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Find and replace by value";
			this.numericUpDownReplaceWith.Location = new Point(150, 120);
			this.numericUpDownReplaceWith.Name = "numericUpDownReplaceWith";
			this.numericUpDownReplaceWith.Size = new Size(0x34, 20);
			this.numericUpDownReplaceWith.TabIndex = 4;
			this.numericUpDownReplaceWith.Enter += new EventHandler(this.numericUpDownReplaceWith_Enter);
			this.label3.AutoSize = true;
			this.label3.Location = new Point(0x1d, 0x7a);
			this.label3.Name = "label3";
			this.label3.Size = new Size(0x45, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "Replace with";
			this.numericUpDownFind.Location = new Point(150, 0x52);
			this.numericUpDownFind.Name = "numericUpDownFind";
			this.numericUpDownFind.Size = new Size(0x34, 20);
			this.numericUpDownFind.TabIndex = 2;
			this.numericUpDownFind.Enter += new EventHandler(this.numericUpDownFind_Enter);
			this.label2.AutoSize = true;
			this.label2.Location = new Point(0x1d, 0x54);
			this.label2.Name = "label2";
			this.label2.Size = new Size(0x1b, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Find";
			this.label1.AutoSize = true;
			this.label1.Location = new Point(0x15, 0x1d);
			this.label1.Name = "label1";
			this.label1.Size = new Size(180, 0x1a);
			this.label1.TabIndex = 0;
			this.label1.Text = "This allows you to replace selected\r\nvalues in the currently-selected cells.";
			this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new Point(0x56, 190);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(0x4b, 0x17);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(0xa7, 190);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 0x17);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			base.AcceptButton = this.buttonOK;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(0xfe, 0xe1);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.groupBox1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Name = "FindAndReplaceDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Find and Replace";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.numericUpDownReplaceWith.EndInit();
			this.numericUpDownFind.EndInit();
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