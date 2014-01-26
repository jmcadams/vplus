using VixenPlus.Properties;

namespace VixenEditor {
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class DelayedStartDialog : Form {
        private int _countdown;


        public DelayedStartDialog() {
            InitializeComponent();
        }


        private void buttonStartStop_Click(object sender, EventArgs e) {
            if (!timer.Enabled) {
                buttonCancel.Visible = false;
                _countdown = (int) numericUpDownDelay.Value;
                UpdateCountdown();
                timer.Start();
            }
            else {
                timer.Stop();
                buttonStartStop.ForeColor = Color.Black;
                buttonStartStop.Text = Resources.Start;
                buttonCancel.Visible = true;
            }
        }


        //private void DelayedStartDialog_Load(object sender, EventArgs e) {}


        private void timer_Tick(object sender, EventArgs e) {
            if (--_countdown == 0) {
                timer.Stop();
                DialogResult = DialogResult.OK;
            }
            else {
                UpdateCountdown();
            }
        }


        private void UpdateCountdown() {
            buttonStartStop.ForeColor = _countdown > 10 ? Color.Green : Color.Red;
            buttonStartStop.Text = string.Format(Resources.CountdownStop, _countdown);
        }
    }
}