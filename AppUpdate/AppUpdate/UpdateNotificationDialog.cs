namespace AppUpdate {
	using System;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Drawing;
	using System.IO;
	using System.Windows.Forms;

	internal partial class UpdateNotificationDialog : Form {

		public UpdateNotificationDialog(bool restartRequired) {
			this.InitializeComponent();
			string str = "There are updates available.  Would you like to download and install them now?";
			this.labelMessage.Text = str + (restartRequired ? "\n\n(This will cause the application to be restarted)" : string.Empty);
			this.Text = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.ModuleName) + " Update";
		}

		//ComponentResourceManager manager = new ComponentResourceManager(typeof(UpdateNotificationDialog));
		//this.pictureBox1.Image = (Image) manager.GetObject("pictureBox1.Image");
	}
}