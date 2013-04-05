using System.Globalization;

namespace VixenEditor {
	using System;
    using System.Drawing;
	using System.Windows.Forms;

	internal partial class IntensityAdjustDialog : Form {
		private bool _actualLevels;
		private int _delta;
		private readonly Graphics _graphics;
		private int _largeDelta = 5;
		private int _maxValue;

		public IntensityAdjustDialog(bool actualLevels) {
			InitializeComponent();
			_graphics = CreateGraphics();
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
			get {
				return _delta;
			}
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
				labelDelta.Text = str;
				labelDelta.Left = ((int)(Width - _graphics.MeasureString(labelDelta.Text, labelDelta.Font).Width)) >> 1;
			}
		}

		public int LargeDelta {
			set {
				_largeDelta = value;
			}
		}
	}
}