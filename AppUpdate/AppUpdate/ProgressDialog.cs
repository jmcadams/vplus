namespace AppUpdate {
    using System.Windows.Forms;

    internal partial class ProgressDialog : Form {

        public ProgressDialog() {
            this.InitializeComponent();
        }

        public string Message {
            set {
                this.lblMessage.Text = value;
                this.lblMessage.Refresh();
            }
        }
    }
}