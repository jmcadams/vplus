namespace VixenEditor {
	using System;
    using System.Windows.Forms;

	public partial class AudioSpeedDialog : Form {
		public AudioSpeedDialog() {
			InitializeComponent();
		}

		private void AudioSpeedDialog_KeyPress(object sender, KeyPressEventArgs e) {
			e.Handled = true;
			if (e.KeyChar == '\r') {
				DialogResult = DialogResult.OK;
			}
			else if (e.KeyChar == '\x001b') {
				DialogResult = DialogResult.Cancel;
			}
		}

		private void trackBar_Scroll(object sender, EventArgs e) {
			labelValue.Text = trackBar.Value + @"%";
		}

		public float Rate {
			get {
				return (trackBar.Value / 100f);
			}
			set {
				if ((value > 0f) && (value <= 1f)) {
					trackBar.Value = (int)(value * 100f);
					labelValue.Text = trackBar.Value + @"%";
				}
			}
		}
	}
}