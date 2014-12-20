using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenPlus {
    internal partial class Splash
    {
        private IContainer components = null;

        #region Windows Form Designer generated code
        
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Splash
            // 
            this.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = Color.Gainsboro;
            this.BackgroundImageLayout = ImageLayout.Zoom;
            this.ClientSize = new Size(606, 192);
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Splash";
            this.StartPosition = FormStartPosition.Manual;
            this.TopMost = true;
            this.TransparencyKey = Color.Gainsboro;
            this.ResumeLayout(false);

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