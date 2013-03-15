namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Windows.Forms;

    internal partial class ShutdownDialog : Form
    {
        //TODO: Load these images the proper way.
        //ComponentResourceManager manager = new ComponentResourceManager(typeof(ShutdownDialog));
        //this.pictureBox1.Image = (Image) manager.GetObject("pictureBox1.Image");

        public ShutdownDialog()
        {
            this.InitializeComponent();
            this.labelShutdownMessage.Text = string.Format("{0} is shutting down your computer in 30 seconds.\n\nYou can stop this by clicking the Abort button below.", Vendor.ProductName);
        }

        private void buttonAbort_Click(object sender, EventArgs e)
        {
            Process.Start("shutdown", "/a");
            base.Close();
        }

    }
}

