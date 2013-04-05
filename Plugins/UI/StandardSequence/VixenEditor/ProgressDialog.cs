namespace VixenEditor {
    using System.Windows.Forms;

	internal partial class ProgressDialog : Form {

		public ProgressDialog() {
			InitializeComponent();
		}

		public string Message {
			set {
				labelMessage.Text = value;
				labelMessage.Refresh();
			}
		}
	}
}