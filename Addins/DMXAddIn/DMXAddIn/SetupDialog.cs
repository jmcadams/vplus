namespace DMXAddIn {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;

	public partial class SetupDialog : Form {
		public SetupDialog(bool autoStart) {
			this.InitializeComponent();
			this.checkBoxStartStream.Checked = autoStart;
		}

		public bool AutoStartStream {
			get {
				return this.checkBoxStartStream.Checked;
			}
		}
	}
}