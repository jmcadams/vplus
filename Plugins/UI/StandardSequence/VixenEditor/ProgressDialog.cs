namespace VixenEditor {
    using System.Windows.Forms;

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