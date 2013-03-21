using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace LedTriksUtil
{
	internal partial class VirtualHardwareDisplayDialog
    {
        private IContainer components;

		#region Windows Form Designer generated code
		
		private void InitializeComponent()
        {
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x11c, 0x108);
            base.ControlBox = false;
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Name = "DisplayDialog";
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "LedTriks Virtual Display";
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
