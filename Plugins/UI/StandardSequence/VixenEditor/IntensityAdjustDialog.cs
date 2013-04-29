using System;
using System.Globalization;
using System.Windows.Forms;

namespace VixenEditor {
    internal partial class IntensityAdjustDialog : Form {
        private bool _actualLevels;
        private int _delta;
        private int _largeDelta = 5;
        private int _maxValue;


        public IntensityAdjustDialog(bool actualLevels) {
            InitializeComponent();
            ActualLevels = actualLevels;
        }


        private void IntensityAdjustDialog_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Down:
                    Delta = e.Alt ? -1 : -_largeDelta;
                    break;
                case Keys.Up:
                    Delta = e.Alt ? 1 : _largeDelta;
                    break;
            }
        }


        private void IntensityAdjustDialog_KeyUp(object sender, KeyEventArgs e) {
            if (!e.Control) {
                Hide();
            }
        }


        private void IntensityAdjustDialog_VisibleChanged(object sender, EventArgs e) {
            if (Visible) {
                _delta = 0;
            }
        }


        public bool ActualLevels {
            set {
                _actualLevels = value;
                _maxValue = value ? 255 : 100;
            }
        }

        public int Delta {
            get { return _delta; }
            set {
                if ((_delta + value) < -_maxValue) {
                    value = -_maxValue - _delta;
                }
                else if ((_delta + value) > _maxValue) {
                    value = _maxValue - _delta;
                }
                _delta += value;
                var str = (_delta > 0) ? string.Format("+{0}", _delta) : _delta.ToString(CultureInfo.InvariantCulture);
                if (!_actualLevels) {
                    str = str + "%";
                }
                lblDelta.Text = str;
            }
        }

        public int LargeDelta {
            set { _largeDelta = value; }
        }
    }
}