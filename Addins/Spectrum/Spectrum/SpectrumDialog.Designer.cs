using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace Spectrum {
	public partial class SpectrumDialog {
		private IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonAutoMap;
		private Button buttonCancel;
		private Button buttonOK;
		private CheckBox checkBoxLockSliders;
		private Label labelScaleFactor;
		private PictureBox pictureBoxPause;
		private PictureBox pictureBoxPlay;
		private PictureBox pictureBoxScaleDown;
		private PictureBox pictureBoxScaleUp;
		private PictureBox pictureBoxStop;
		private System.Windows.Forms.Timer timer;

		private void InitializeComponent() {
			this.components = new Container();
			this.buttonAutoMap = new Button();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.pictureBoxPlay = new PictureBox();
			this.pictureBoxPause = new PictureBox();
			this.pictureBoxStop = new PictureBox();
			this.pictureBoxScaleUp = new PictureBox();
			this.pictureBoxScaleDown = new PictureBox();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.labelScaleFactor = new Label();
			this.checkBoxLockSliders = new CheckBox();
			((ISupportInitialize)this.pictureBoxPlay).BeginInit();
			((ISupportInitialize)this.pictureBoxPause).BeginInit();
			((ISupportInitialize)this.pictureBoxStop).BeginInit();
			((ISupportInitialize)this.pictureBoxScaleUp).BeginInit();
			((ISupportInitialize)this.pictureBoxScaleDown).BeginInit();
			base.SuspendLayout();
			this.buttonAutoMap.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
			this.buttonAutoMap.Location = new Point(12, 0x143);
			this.buttonAutoMap.Name = "buttonAutoMap";
			this.buttonAutoMap.Size = new Size(0x4b, 0x17);
			this.buttonAutoMap.TabIndex = 1;
			this.buttonAutoMap.Text = "Auto Map";
			this.buttonAutoMap.UseVisualStyleBackColor = true;
			this.buttonAutoMap.Click += new EventHandler(this.buttonAutoMap_Click);
			this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new Point(0x31c, 0x143);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(0x4b, 0x17);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(0x36d, 0x143);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 0x17);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.pictureBoxPlay.Location = new Point(0x1c4, 0x124);
			this.pictureBoxPlay.Name = "pictureBoxPlay";
			this.pictureBoxPlay.Size = new Size(0x10, 0x10);
			this.pictureBoxPlay.SizeMode = PictureBoxSizeMode.AutoSize;
			this.pictureBoxPlay.TabIndex = 3;
			this.pictureBoxPlay.TabStop = false;
			this.pictureBoxPlay.Click += new EventHandler(this.pictureBoxPlay_Click);
			this.pictureBoxPause.Location = new Point(0x1da, 0x124);
			this.pictureBoxPause.Name = "pictureBoxPause";
			this.pictureBoxPause.Size = new Size(0x10, 0x10);
			this.pictureBoxPause.SizeMode = PictureBoxSizeMode.AutoSize;
			this.pictureBoxPause.TabIndex = 4;
			this.pictureBoxPause.TabStop = false;
			this.pictureBoxPause.Click += new EventHandler(this.pictureBoxPause_Click);
			this.pictureBoxStop.Location = new Point(0x1f0, 0x124);
			this.pictureBoxStop.Name = "pictureBoxStop";
			this.pictureBoxStop.Size = new Size(0x10, 0x10);
			this.pictureBoxStop.SizeMode = PictureBoxSizeMode.AutoSize;
			this.pictureBoxStop.TabIndex = 5;
			this.pictureBoxStop.TabStop = false;
			this.pictureBoxStop.Click += new EventHandler(this.pictureBoxStop_Click);
			this.pictureBoxScaleUp.Location = new Point(12, 0x52);
			this.pictureBoxScaleUp.Name = "pictureBoxScaleUp";
			this.pictureBoxScaleUp.Size = new Size(0x10, 0x10);
			this.pictureBoxScaleUp.SizeMode = PictureBoxSizeMode.AutoSize;
			this.pictureBoxScaleUp.TabIndex = 6;
			this.pictureBoxScaleUp.TabStop = false;
			this.pictureBoxScaleUp.MouseDown += new MouseEventHandler(this.pictureBoxScaleUp_MouseDown);
			this.pictureBoxScaleDown.Location = new Point(12, 0x68);
			this.pictureBoxScaleDown.Name = "pictureBoxScaleDown";
			this.pictureBoxScaleDown.Size = new Size(0x10, 0x10);
			this.pictureBoxScaleDown.SizeMode = PictureBoxSizeMode.AutoSize;
			this.pictureBoxScaleDown.TabIndex = 7;
			this.pictureBoxScaleDown.TabStop = false;
			this.pictureBoxScaleDown.MouseDown += new MouseEventHandler(this.pictureBoxScaleDown_MouseDown);
			this.timer.Interval = 50;
			this.timer.Tick += new EventHandler(this.timer_Tick);
			this.labelScaleFactor.AutoSize = true;
			this.labelScaleFactor.Location = new Point(10, 0x80);
			this.labelScaleFactor.Name = "labelScaleFactor";
			this.labelScaleFactor.Size = new Size(0x23, 13);
			this.labelScaleFactor.TabIndex = 8;
			this.labelScaleFactor.Text = "label1";
			this.checkBoxLockSliders.AutoSize = true;
			this.checkBoxLockSliders.Location = new Point(14, 300);
			this.checkBoxLockSliders.Name = "checkBoxLockSliders";
			this.checkBoxLockSliders.Size = new Size(0x10f, 0x11);
			this.checkBoxLockSliders.TabIndex = 0;
			this.checkBoxLockSliders.Text = "Adjust sliders automatically when adjusting the scale";
			this.checkBoxLockSliders.UseVisualStyleBackColor = true;
			base.AcceptButton = this.buttonOK;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = Color.White;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(0x3c4, 0x166);
			base.Controls.Add(this.checkBoxLockSliders);
			base.Controls.Add(this.labelScaleFactor);
			base.Controls.Add(this.pictureBoxScaleDown);
			base.Controls.Add(this.pictureBoxScaleUp);
			base.Controls.Add(this.pictureBoxStop);
			base.Controls.Add(this.pictureBoxPause);
			base.Controls.Add(this.pictureBoxPlay);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.buttonAutoMap);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.HelpButton = true;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "SpectrumDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Frequency Spectrum Analysis";
			base.HelpButtonClicked += new CancelEventHandler(this.SpectrumParamsDialog_HelpButtonClicked);
			base.FormClosing += new FormClosingEventHandler(this.SpectrumDialog_FormClosing);
			base.Load += new EventHandler(this.SpectrumDialog_Load);
			((ISupportInitialize)this.pictureBoxPlay).EndInit();
			((ISupportInitialize)this.pictureBoxPause).EndInit();
			((ISupportInitialize)this.pictureBoxStop).EndInit();
			((ISupportInitialize)this.pictureBoxScaleUp).EndInit();
			((ISupportInitialize)this.pictureBoxScaleDown).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		#endregion

		protected override void Dispose(bool disposing) {
			RESULT result;
			if (this.sound != null) {
				result = this.sound.release();
				this.ERRCHECK(result);
			}
			if (this.system != null) {
				result = this.system.close();
				this.ERRCHECK(result);
				result = this.system.release();
				this.ERRCHECK(result);
			}
			if (disposing && (this.components != null)) {
				this.components.Dispose();
			}
			this.m_textFont.Dispose();
			this.m_bandFont.Dispose();
			base.Dispose(disposing);
		}
	}
}