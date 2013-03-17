namespace AppUpdate {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;

	internal partial class ShutdownDialog : Form {

		public ShutdownDialog() {
			this.InitializeComponent();
		}

		//ComponentResourceManager manager = new ComponentResourceManager(typeof(ShutdownDialog));
		//this.pictureBox1.Image = (Image)manager.GetObject("pictureBox1.Image");
		//this.labelMessage.Text = manager.GetString("labelMessage.Text");
	}
}