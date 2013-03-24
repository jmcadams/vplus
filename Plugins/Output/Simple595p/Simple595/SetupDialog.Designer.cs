namespace Simple595{
	using System;
	using System.Windows.Forms;
	using System.Drawing;
	using System.Collections;

	public partial class SetupDialog{
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCancel;
private Button buttonOK;
private Button buttonSetupPort;
private GroupBox groupBox1;
private GroupBox groupBox2;
private Label label2;
private Label labelPulses;
private NumericUpDown numericUpDownPulseWidth;

		private void InitializeComponent()
		{
			this.groupBox1 = new GroupBox();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.groupBox2 = new GroupBox();
			this.labelPulses = new Label();
			this.numericUpDownPulseWidth = new NumericUpDown();
			this.label2 = new Label();
			this.buttonSetupPort = new Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.numericUpDownPulseWidth.BeginInit();
			base.SuspendLayout();
			this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
			this.groupBox1.Controls.Add(this.buttonSetupPort);
			this.groupBox1.Location = new Point(14, 0x12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(0xc6, 0x4d);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Parallel port";
			this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new Point(0x26, 0xab);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(0x4b, 0x17);
			this.buttonOK.TabIndex = 2;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(0x77, 0xab);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 0x17);
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.groupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.groupBox2.Controls.Add(this.labelPulses);
			this.groupBox2.Controls.Add(this.numericUpDownPulseWidth);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Location = new Point(14, 0x65);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(0xc6, 0x40);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Clock pulse width";
			this.labelPulses.AutoSize = true;
			this.labelPulses.Location = new Point(0x97, 0x1b);
			this.labelPulses.Name = "labelPulses";
			this.labelPulses.Size = new Size(0x23, 13);
			this.labelPulses.TabIndex = 2;
			this.labelPulses.Text = "pulse.";
			this.numericUpDownPulseWidth.Location = new Point(0x72, 0x19);
			int[] pulseWidthMin = new int[4];
			pulseWidthMin[0] = 1;
			this.numericUpDownPulseWidth.Minimum = new decimal(pulseWidthMin);
			this.numericUpDownPulseWidth.Name = "numericUpDownPulseWidth";
			this.numericUpDownPulseWidth.Size = new Size(0x1f, 20);
			this.numericUpDownPulseWidth.TabIndex = 1;
			int[] pulseWidthValue = new int[4];
			pulseWidthValue[0] = 1;
			this.numericUpDownPulseWidth.Value = new decimal(pulseWidthValue);
			this.numericUpDownPulseWidth.ValueChanged += new EventHandler(this.numericUpDownPulseWidth_ValueChanged);
			this.label2.AutoSize = true;
			this.label2.Location = new Point(12, 0x1b);
			this.label2.Name = "label2";
			this.label2.Size = new Size(0x60, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Hold clock high for";
			this.buttonSetupPort.Location = new Point(60, 0x1d);
			this.buttonSetupPort.Name = "buttonSetupPort";
			this.buttonSetupPort.Size = new Size(0x4b, 0x17);
			this.buttonSetupPort.TabIndex = 0;
			this.buttonSetupPort.Text = "Setup";
			this.buttonSetupPort.UseVisualStyleBackColor = true;
			this.buttonSetupPort.Click += new EventHandler(this.buttonSetupPort_Click);
			base.AcceptButton = this.buttonOK;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(0xdf, 0xce);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.groupBox1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "SetupDialog";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Setup";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.numericUpDownPulseWidth.EndInit();
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