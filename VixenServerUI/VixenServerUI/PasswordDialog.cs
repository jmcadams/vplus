namespace VixenServerUI {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;

	public partial class PasswordDialog : Form {
		public PasswordDialog(string password) {
			this.InitializeComponent();
			this.textBoxPassword.Text = password;
		}

		private void buttonReset_Click(object sender, EventArgs e) {
			this.textBoxPassword.Text = string.Empty;
		}


		//ComponentResourceManager manager = new ComponentResourceManager(typeof(PasswordDialog));
		//this.label1.Text = manager.GetString("label1.Text");



		public string Password {
			get {
				return this.textBoxPassword.Text;
			}
		}
	}
}

