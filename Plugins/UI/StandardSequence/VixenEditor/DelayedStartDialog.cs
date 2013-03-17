namespace VixenEditor {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;

	public class DelayedStartDialog : Form {
		private int m_countdown;

		public DelayedStartDialog() {
			this.InitializeComponent();
		}

		private void buttonStartStop_Click(object sender, EventArgs e) {
			if (!this.timer.Enabled) {
				this.m_countdown = (int)this.numericUpDownDelay.Value;
				this.UpdateCountdown();
				this.timer.Start();
			}
			else {
				this.timer.Stop();
				this.buttonStartStop.ForeColor = Color.Black;
				this.buttonStartStop.Text = "Start";
			}
		}

		private void DelayedStartDialog_Load(object sender, EventArgs e) {
		}

		private void timer_Tick(object sender, EventArgs e) {
			if (--this.m_countdown == 0) {
				this.timer.Stop();
				base.DialogResult = System.Windows.Forms.DialogResult.OK;
			}
			else {
				this.UpdateCountdown();
			}
		}

		private void UpdateCountdown() {
			if (this.m_countdown > 10) {
				this.buttonStartStop.ForeColor = Color.Green;
			}
			else {
				this.buttonStartStop.ForeColor = Color.Red;
			}
			this.buttonStartStop.Text = string.Format("Stop ({0})", this.m_countdown);
		}
	}
}