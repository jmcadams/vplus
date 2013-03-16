namespace Spectrum {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;

	internal partial class TranscribeDialog : Form {
		public TranscribeDialog(int maximum) {
			this.InitializeComponent();
			this.progressBar.Maximum = maximum;
		}

		public int Progress {
			set {
				this.progressBar.Value = value;
				this.Refresh();
			}
		}
	}
}