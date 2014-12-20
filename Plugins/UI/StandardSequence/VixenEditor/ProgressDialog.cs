using System.Windows.Forms;

namespace VixenEditor {
    internal partial class ProgressDialog : Form {

        public ProgressDialog() {
            InitializeComponent();
        }

        public string Message {
            set {
                lblMessage.Text = value;
                lblMessage.Refresh();
            }
        }
    }
}