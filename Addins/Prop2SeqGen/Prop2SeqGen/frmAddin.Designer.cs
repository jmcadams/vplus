using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace Prop2SeqGen
{
	internal partial class frmAddin {
		private IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCancel;
private Button buttonOK;
private CheckBox checkBoxOpenFile;
private ComboBox comboBoxAudioDevice;
private GroupBox groupBox1;
private GroupBox grpAudioDevice;
private GroupBox grpTrigger;
private Label lblIntensity;
private Label lblName;
private NumericUpDown numericUpDownThreshold;
private RadioButton radioButtonActiveHigh;
private RadioButton radioButtonActiveLow;
private SaveFileDialog saveFileDialog;
private TextBox textBoxName;

		private void InitializeComponent()
		{
			this.lblName = new Label();
			this.textBoxName = new TextBox();
			this.grpTrigger = new GroupBox();
			this.radioButtonActiveLow = new RadioButton();
			this.radioButtonActiveHigh = new RadioButton();
			this.grpAudioDevice = new GroupBox();
			this.comboBoxAudioDevice = new ComboBox();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.groupBox1 = new GroupBox();
			this.numericUpDownThreshold = new NumericUpDown();
			this.lblIntensity = new Label();
			this.saveFileDialog = new SaveFileDialog();
			this.checkBoxOpenFile = new CheckBox();
			this.grpTrigger.SuspendLayout();
			this.grpAudioDevice.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.numericUpDownThreshold.BeginInit();
			base.SuspendLayout();
			this.lblName.AutoSize = true;
			this.lblName.Location = new Point(12, 12);
			this.lblName.Name = "lblName";
			this.lblName.Size = new Size(0x23, 13);
			this.lblName.TabIndex = 0;
			this.lblName.Text = "Name";
			this.textBoxName.Location = new Point(0x35, 9);
			this.textBoxName.Name = "textBoxName";
			this.textBoxName.Size = new Size(0xa2, 20);
			this.textBoxName.TabIndex = 1;
			this.grpTrigger.Controls.Add(this.radioButtonActiveLow);
			this.grpTrigger.Controls.Add(this.radioButtonActiveHigh);
			this.grpTrigger.Location = new Point(15, 0x23);
			this.grpTrigger.Name = "grpTrigger";
			this.grpTrigger.Size = new Size(200, 0x43);
			this.grpTrigger.TabIndex = 2;
			this.grpTrigger.TabStop = false;
			this.grpTrigger.Text = "Trigger Level";
			this.radioButtonActiveLow.AutoSize = true;
			this.radioButtonActiveLow.Location = new Point(0x15, 0x2a);
			this.radioButtonActiveLow.Name = "radioButtonActiveLow";
			this.radioButtonActiveLow.Size = new Size(0x4e, 0x11);
			this.radioButtonActiveLow.TabIndex = 1;
			this.radioButtonActiveLow.Text = "Active-Low";
			this.radioButtonActiveLow.UseVisualStyleBackColor = true;
			this.radioButtonActiveHigh.AutoSize = true;
			this.radioButtonActiveHigh.Checked = true;
			this.radioButtonActiveHigh.Location = new Point(0x15, 0x13);
			this.radioButtonActiveHigh.Name = "radioButtonActiveHigh";
			this.radioButtonActiveHigh.Size = new Size(80, 0x11);
			this.radioButtonActiveHigh.TabIndex = 0;
			this.radioButtonActiveHigh.TabStop = true;
			this.radioButtonActiveHigh.Text = "Active-High";
			this.radioButtonActiveHigh.UseVisualStyleBackColor = true;
			this.grpAudioDevice.Controls.Add(this.comboBoxAudioDevice);
			this.grpAudioDevice.Location = new Point(15, 0x6c);
			this.grpAudioDevice.Name = "grpAudioDevice";
			this.grpAudioDevice.Size = new Size(200, 80);
			this.grpAudioDevice.TabIndex = 3;
			this.grpAudioDevice.TabStop = false;
			this.grpAudioDevice.Text = "Audio Device";
			this.comboBoxAudioDevice.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxAudioDevice.FormattingEnabled = true;
			this.comboBoxAudioDevice.Location = new Point(14, 0x21);
			this.comboBoxAudioDevice.Name = "comboBoxAudioDevice";
			this.comboBoxAudioDevice.Size = new Size(0xad, 0x15);
			this.comboBoxAudioDevice.TabIndex = 3;
			this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new Point(60, 0x161);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(0x4b, 0x17);
			this.buttonOK.TabIndex = 6;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(0x8d, 0x161);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 0x17);
			this.buttonCancel.TabIndex = 7;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.groupBox1.Controls.Add(this.numericUpDownThreshold);
			this.groupBox1.Controls.Add(this.lblIntensity);
			this.groupBox1.Location = new Point(15, 0xc2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(0xc7, 0x6b);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Threshold";
			this.numericUpDownThreshold.Location = new Point(0x49, 0x4a);
			this.numericUpDownThreshold.Name = "numericUpDownThreshold";
			this.numericUpDownThreshold.Size = new Size(0x34, 20);
			this.numericUpDownThreshold.TabIndex = 1;
			int[] bits = new int[4];
			bits[0] = 50;
			this.numericUpDownThreshold.Value = new decimal(bits);
			this.lblIntensity.AutoSize = true;
			this.lblIntensity.Location = new Point(14, 0x18);
			this.lblIntensity.Name = "label1";
			this.lblIntensity.Size = new Size(0xb1, 0x27);
			this.lblIntensity.TabIndex = 0;
			this.lblIntensity.Text = "Select the intensity (0-100%) level at\r\nwhich a channel will be considered\r\ntriggered.";
			this.saveFileDialog.DefaultExt = "bs2";
			this.saveFileDialog.Filter = "BS2 files|*.bs2";
			this.checkBoxOpenFile.AutoSize = true;
			this.checkBoxOpenFile.Location = new Point(15, 0x13b);
			this.checkBoxOpenFile.Name = "checkBoxOpenFile";
			this.checkBoxOpenFile.Size = new Size(0xaf, 0x11);
			this.checkBoxOpenFile.TabIndex = 5;
			this.checkBoxOpenFile.Text = "Open file in BASIC Stamp editor";
			this.checkBoxOpenFile.UseVisualStyleBackColor = true;
			base.AcceptButton = this.buttonOK;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(0xe4, 0x184);
			base.Controls.Add(this.checkBoxOpenFile);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.grpAudioDevice);
			base.Controls.Add(this.grpTrigger);
			base.Controls.Add(this.textBoxName);
			base.Controls.Add(this.lblName);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Name = "frmAddin";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Prop-2 Sequencer";
			this.grpTrigger.ResumeLayout(false);
			this.grpTrigger.PerformLayout();
			this.grpAudioDevice.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.numericUpDownThreshold.EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
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
