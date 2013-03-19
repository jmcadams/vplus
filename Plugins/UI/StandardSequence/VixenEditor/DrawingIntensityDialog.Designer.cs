using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace VixenEditor {
	internal partial class DrawingIntensityDialog {
		private IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCancel;
		private Button buttonOK;
		private Button buttonReset;
		private GroupBox groupBox1;
		private Label label1;
		private NumericUpDown numericUpDownLevel;

		private void InitializeComponent() {
			this.label1 = new Label();
			this.numericUpDownLevel = new NumericUpDown();
			this.buttonReset = new Button();
			this.groupBox1 = new GroupBox();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.numericUpDownLevel.BeginInit();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.label1.Location = new Point(6, 0x10);
			this.label1.Name = "label1";
			this.label1.Size = new Size(0x10f, 90);
			this.label1.TabIndex = 0;
			this.numericUpDownLevel.Location = new Point(0x6f, 0x74);
			this.numericUpDownLevel.Name = "numericUpDownLevel";
			this.numericUpDownLevel.Size = new Size(60, 20);
			this.numericUpDownLevel.TabIndex = 1;
			this.buttonReset.Location = new Point(0x49, 0x8e);
			this.buttonReset.Name = "buttonReset";
			this.buttonReset.Size = new Size(0x89, 0x17);
			this.buttonReset.TabIndex = 2;
			this.buttonReset.Text = "Reset to sequence's max";
			this.buttonReset.UseVisualStyleBackColor = true;
			this.buttonReset.Click += new EventHandler(this.buttonReset_Click);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.buttonReset);
			this.groupBox1.Controls.Add(this.numericUpDownLevel);
			this.groupBox1.Location = new Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(0x11b, 0xb2);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Intensity Level";
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new Point(140, 0xc4);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(0x4b, 0x17);
			this.buttonOK.TabIndex = 4;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(0xdd, 0xc4);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 0x17);
			this.buttonCancel.TabIndex = 5;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			base.AcceptButton = this.buttonOK;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(0x134, 0xe5);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.groupBox1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Name = "DrawingIntensityDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Drawing Intensity";
			this.numericUpDownLevel.EndInit();
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