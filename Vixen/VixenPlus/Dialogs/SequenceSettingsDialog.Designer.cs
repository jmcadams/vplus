namespace Vixen.Dialogs{
	using System;
	using System.Windows.Forms;
	using System.Drawing;
	using System.Collections;

	public partial class SequenceSettingsDialog{
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCancel;
private Button buttonOK;
private GroupBox groupBox1;
private Label label1;
private Label label2;
private Label label3;
private Label label4;
private NumericUpDown numericUpDownMaximum;
private NumericUpDown numericUpDownMinimum;
private TextBox textBoxEventPeriodLength;
private TextBox textBoxSequenceName;

		private void InitializeComponent()
		{
			this.label1 = new Label();
			this.textBoxSequenceName = new TextBox();
			this.label2 = new Label();
			this.label3 = new Label();
			this.numericUpDownMinimum = new NumericUpDown();
			this.numericUpDownMaximum = new NumericUpDown();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.label4 = new Label();
			this.textBoxEventPeriodLength = new TextBox();
			this.groupBox1 = new GroupBox();
			this.numericUpDownMinimum.BeginInit();
			this.numericUpDownMaximum.BeginInit();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(10, 0x76);
			this.label1.Name = "label1";
			this.label1.Size = new Size(0x55, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Sequence name";
			this.label1.Visible = false;
			this.textBoxSequenceName.Location = new Point(0x65, 0x73);
			this.textBoxSequenceName.Name = "textBoxSequenceName";
			this.textBoxSequenceName.Size = new Size(0xa1, 20);
			this.textBoxSequenceName.TabIndex = 1;
			this.textBoxSequenceName.Visible = false;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(6, 0x1a);
			this.label2.Name = "label2";
			this.label2.Size = new Size(0xa3, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Minimum illumination level (0-255)";
			this.label3.AutoSize = true;
			this.label3.Location = new Point(6, 0x3b);
			this.label3.Name = "label3";
			this.label3.Size = new Size(0xa6, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Maximum illumination level (0-255)";
			this.numericUpDownMinimum.Location = new Point(0xd5, 0x18);
			int[] bits = new int[4];
			bits[0] = 0xff;
			this.numericUpDownMinimum.Maximum = new decimal(bits);
			this.numericUpDownMinimum.Name = "numericUpDownMinimum";
			this.numericUpDownMinimum.Size = new Size(0x2d, 20);
			this.numericUpDownMinimum.TabIndex = 3;
			this.numericUpDownMaximum.Location = new Point(0xd5, 0x39);
			int[] bitsMax = new int[4];
			bitsMax[0] = 0xff;
			this.numericUpDownMaximum.Maximum = new decimal(bitsMax);
			this.numericUpDownMaximum.Name = "numericUpDownMaximum";
			this.numericUpDownMaximum.Size = new Size(0x2d, 20);
			this.numericUpDownMaximum.TabIndex = 5;
			this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new Point(0x7c, 0x90);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(0x4b, 0x17);
			this.buttonOK.TabIndex = 8;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(0xcd, 0x90);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 0x17);
			this.buttonCancel.TabIndex = 9;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.label4.AutoSize = true;
			this.label4.Location = new Point(6, 0x5c);
			this.label4.Name = "label4";
			this.label4.Size = new Size(0x79, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "Event period length (ms)";
			this.textBoxEventPeriodLength.Location = new Point(0xd5, 0x59);
			this.textBoxEventPeriodLength.Name = "textBoxEventPeriodLength";
			this.textBoxEventPeriodLength.Size = new Size(0x2d, 20);
			this.textBoxEventPeriodLength.TabIndex = 7;
			this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.textBoxEventPeriodLength);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.textBoxSequenceName);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.numericUpDownMinimum);
			this.groupBox1.Controls.Add(this.numericUpDownMaximum);
			this.groupBox1.Location = new Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(0x10c, 0x7e);
			this.groupBox1.TabIndex = 10;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Settings";
			base.AcceptButton = this.buttonOK;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(0x124, 0xb3);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "SequenceSettingsDialog";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Sequence Settings";
			this.numericUpDownMinimum.EndInit();
			this.numericUpDownMaximum.EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
		}
		#endregion

		protected override void Dispose(bool disposing)
		{
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
