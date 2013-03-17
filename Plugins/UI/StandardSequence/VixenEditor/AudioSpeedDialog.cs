namespace VixenEditor {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;

	public partial class AudioSpeedDialog : Form {
		public AudioSpeedDialog() {
			this.InitializeComponent();
		}

		private void AudioSpeedDialog_KeyPress(object sender, KeyPressEventArgs e) {
			e.Handled = true;
			if (e.KeyChar == '\r') {
				base.DialogResult = System.Windows.Forms.DialogResult.OK;
			}
			else if (e.KeyChar == '\x001b') {
				base.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			}
		}

		private void trackBar_Scroll(object sender, EventArgs e) {
			this.labelValue.Text = this.trackBar.Value + "%";
		}

		public float Rate {
			get {
				return (((float)this.trackBar.Value) / 100f);
			}
			set {
				if ((value > 0f) && (value <= 1f)) {
					this.trackBar.Value = (int)(value * 100f);
					this.labelValue.Text = this.trackBar.Value + "%";
				}
			}
		}
	}
}