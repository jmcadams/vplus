using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace Vixen
{
	internal partial class TextListDialog
    {
        private IContainer components = null;

		#region Windows Form Designer generated code
		private Label labelCaption;

		private void InitializeComponent()
        {
            this.labelCaption = new Label();
            base.SuspendLayout();
            this.labelCaption.AutoSize = true;
            this.labelCaption.Location = new Point(12, 9);
            this.labelCaption.Name = "labelCaption";
            this.labelCaption.Size = new Size(0x23, 13);
            this.labelCaption.TabIndex = 0;
            this.labelCaption.Text = "label1";
            this.labelCaption.Click += new EventHandler(this.labelCaption_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x83, 0x20);
            base.Controls.Add(this.labelCaption);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            base.Name = "TextListDialog";
            base.Opacity = 0.85;
            base.ShowInTaskbar = false;
            this.Text = "TextListDialog";
            base.Click += new EventHandler(this.TextListDialog_Click);
            base.KeyDown += new KeyEventHandler(this.TextListDialog_KeyDown);
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
            this.m_graphics.Dispose();
            base.Dispose(disposing);
        }
	}
}
