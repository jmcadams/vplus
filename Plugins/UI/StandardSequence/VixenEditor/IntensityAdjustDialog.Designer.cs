using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace VixenEditor {
	internal partial class IntensityAdjustDialog {
		private IContainer components;

		#region Windows Form Designer generated code
		private Label labelDelta;

		private void InitializeComponent() {
			this.labelDelta = new Label();
			base.SuspendLayout();
			this.labelDelta.AutoSize = true;
			this.labelDelta.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.labelDelta.Location = new Point(12, 10);
			this.labelDelta.Name = "labelDelta";
			this.labelDelta.Size = new Size(0x63, 0x1f);
			this.labelDelta.TabIndex = 0;
			this.labelDelta.Text = "+100%";
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new Size(0x7b, 50);
			base.ControlBox = false;
			base.Controls.Add(this.labelDelta);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.KeyPreview = true;
			base.Name = "IntensityAdjustDialog";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Intensity Adjustment";
			base.TopMost = true;
			base.VisibleChanged += new EventHandler(this.IntensityAdjustDialog_VisibleChanged);
			base.KeyUp += new KeyEventHandler(this.IntensityAdjustDialog_KeyUp);
			base.KeyDown += new KeyEventHandler(this.IntensityAdjustDialog_KeyDown);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		#endregion

		protected override void Dispose(bool disposing) {
			if (disposing && (this.components != null)) {
				this.components.Dispose();
			}
			if (this.m_graphics != null) {
				this.m_graphics.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}