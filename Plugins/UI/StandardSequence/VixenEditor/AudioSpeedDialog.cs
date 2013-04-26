namespace VixenEditor {
    using System;
    using System.Windows.Forms;

    public partial class AudioSpeedDialog : Form {
        public AudioSpeedDialog() {
            InitializeComponent();
        }

        private void AudioSpeedDialog_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = true;
            switch (e.KeyChar) {
                case (char)Keys.Enter:
                    DialogResult = DialogResult.OK;
                    break;
                case (char)Keys.Escape:
                    DialogResult = DialogResult.Cancel;
                    break;
            }
        }


        private void trackBar_Scroll(object sender, EventArgs e) {
            SetText();
        }


        private void SetText() {
            var text = trackBar.Value + @"%";
            switch (text) {
                case "25%":
                    text = "Quarter Speed";
                    break;
                case "50%":
                    text = "Half Speed";
                    break;
                case "75%":
                    text = "Three-Quarter Speed";
                    break;
                case "100%":
                    text = "Full Speed";
                    break;
            }
            labelValue.Text = text;
        }


        public float Rate {
            get {
                return (trackBar.Value / 100f);
            }
            set {
                if ((!(value > 0f)) || (!(value <= 1f))) {
                    return;
                }
                trackBar.Value = (int)(value * 100f);
                SetText();
            }
        }

        private void trackBar_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Down:
                    e.SuppressKeyPress = true;
                    if (trackBar.Value > 1) {
                        trackBar.Value--;
                    }
                    break;
                case Keys.Up:
                    e.SuppressKeyPress = true;
                    if (trackBar.Value < 100) {
                        trackBar.Value++;
                    }
                    break;
                case Keys.PageDown:
                    e.SuppressKeyPress = true;
                    if (trackBar.Value > 1) {
                        trackBar.Value = Math.Max(1, trackBar.Value - trackBar.LargeChange);
                    }
                    break;
                case Keys.PageUp:
                    e.SuppressKeyPress = true;
                    if (trackBar.Value <100) {
                        trackBar.Value = Math.Min(100, trackBar.Value + trackBar.LargeChange);
                    }
                    break;
            }
            if (e.SuppressKeyPress) {
                SetText();
            }
        }
    }
}