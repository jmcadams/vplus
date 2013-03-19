using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace VixenEditor {
	internal partial class EffectFrequencyDialog {
		private IContainer components = null;

		#region Windows Form Designer generated code
		private Button buttonCancel;
		private Button buttonOK;
		private GroupBox groupBox1;
		private MethodInvoker m_refreshInvoker;
		private PictureBox pictureBoxExample;
		private TrackBar trackBarFrequency;

		private void InitializeComponent() {
			this.groupBox1 = new GroupBox();
			this.pictureBoxExample = new PictureBox();
			this.trackBarFrequency = new TrackBar();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.groupBox1.SuspendLayout();
			((ISupportInitialize)this.pictureBoxExample).BeginInit();
			this.trackBarFrequency.BeginInit();
			base.SuspendLayout();
			this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
			this.groupBox1.Controls.Add(this.pictureBoxExample);
			this.groupBox1.Controls.Add(this.trackBarFrequency);
			this.groupBox1.Location = new Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(0x10c, 0xa8);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "groupBox1";
			this.pictureBoxExample.BackColor = Color.Black;
			this.pictureBoxExample.Location = new Point(0x4a, 0x13);
			this.pictureBoxExample.Name = "pictureBoxExample";
			this.pictureBoxExample.Size = new Size(0xb3, 0x89);
			this.pictureBoxExample.TabIndex = 1;
			this.pictureBoxExample.TabStop = false;
			this.pictureBoxExample.Paint += new PaintEventHandler(this.pictureBoxExample_Paint);
			this.trackBarFrequency.Location = new Point(12, 0x13);
			this.trackBarFrequency.Name = "trackBarFrequency";
			this.trackBarFrequency.Orientation = Orientation.Vertical;
			this.trackBarFrequency.Size = new Size(0x2d, 0x93);
			this.trackBarFrequency.TabIndex = 0;
			this.trackBarFrequency.TickStyle = TickStyle.Both;
			this.trackBarFrequency.Scroll += new EventHandler(this.trackBarFrequency_Scroll);
			this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new Point(0x7c, 0xba);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(0x4b, 0x17);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new Point(0xcd, 0xba);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(0x4b, 0x17);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(0x124, 0xd9);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.groupBox1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Name = "EffectFrequencyDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "EffectFrequencyDialog";
			base.FormClosing += new FormClosingEventHandler(this.EffectFrequencyDialog_FormClosing);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((ISupportInitialize)this.pictureBoxExample).EndInit();
			this.trackBarFrequency.EndInit();
			base.ResumeLayout(false);
		}
		#endregion

		protected override void Dispose(bool disposing) {
			if (disposing && (this.components != null)) {
				this.components.Dispose();
			}
			if (this.m_brush != null) {
				this.m_brush.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}