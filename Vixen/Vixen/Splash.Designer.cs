using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace Vixen
{
	public partial class Splash
    {
        private IContainer components = null;

		#region Windows Form Designer generated code
		
		private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Splash
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(300, 385);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "Splash";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Transparent;
            this.Load += new System.EventHandler(this.Splash_Load);
            this.ResumeLayout(false);

        }
		#endregion

		protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            if (this.m_backgroundPicture != null)
            {
                this.m_backgroundPicture.Dispose();
            }
            if (this.m_bulbOff != null)
            {
                this.m_bulbOff.Dispose();
            }
            if (this.m_bulbOn != null)
            {
                this.m_bulbOn.Dispose();
            }
            if (this.m_bgBrush != null)
            {
                this.m_bgBrush.Dispose();
            }
            if (this.m_borderPen != null)
            {
                this.m_borderPen.Dispose();
            }
            if (this.m_textBrush != null)
            {
                this.m_textBrush.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}
