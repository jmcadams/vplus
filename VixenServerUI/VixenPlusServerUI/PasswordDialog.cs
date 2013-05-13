using System;
using System.Windows.Forms;

namespace VixenPlusServerUI {
    public partial class PasswordDialog : Form {
        public PasswordDialog(string password) {
            InitializeComponent();
            textBoxPassword.Text = password;
        }

        private void buttonReset_Click(object sender, EventArgs e) {
            textBoxPassword.Text = string.Empty;
        }


        //ComponentResourceManager manager = new ComponentResourceManager(typeof(PasswordDialog));
        //label1.Text = manager.GetString("label1.Text");



        public string Password {
            get {
                return textBoxPassword.Text;
            }
        }
    }
}

