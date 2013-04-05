namespace VixenEditor {
	using System;
    using System.Drawing;
	using System.Timers;
	using System.Windows.Forms;

	internal partial class EffectFrequencyDialog : Form {
		private readonly SolidBrush _solidBrush;
		private readonly System.Timers.Timer _drawTimer;
		private readonly FrequencyEffectGenerator _effectGenerator;
		private readonly byte[,] _effectValues;
		private int _frequency;
		private readonly int _maxColumn;
		private int _tickCount;
		private readonly Point[][] _treePoints;

		public EffectFrequencyDialog(string effectName, int maxFrequency, FrequencyEffectGenerator effectGenerator) {
			InitializeComponent();
			_solidBrush = new SolidBrush(Color.Black);
			Text = effectName;
			groupBox1.Text = effectName + " Frequency";
			trackBarFrequency.Maximum = maxFrequency;
			_effectGenerator = effectGenerator;
			m_refreshInvoker = pictureBoxExample.Refresh;
			_effectValues = new byte[4, maxFrequency * 5];
			_maxColumn = _effectValues.GetLength(1);
			effectGenerator(_effectValues, new[] { 1 });
			_tickCount = 0;
			var pointArray = new Point[4][];
            var pointArray2 = new[] { new Point(22, 0x2e), new Point(0x25, 0x5b), new Point(7, 0x5b) };
			pointArray[0] = pointArray2;
            pointArray2 = new[] { new Point(0x43, 0x2e), new Point(0x52, 0x5b), new Point(0x34, 0x5b) };
			pointArray[1] = pointArray2;
            pointArray2 = new[] { new Point(0x70, 0x2e), new Point(0x7f, 0x5b), new Point(0x61, 0x5b) };
			pointArray[2] = pointArray2;
			pointArray2 = new[] { new Point(0x9d, 0x2e), new Point(0xac, 0x5b), new Point(0x8e, 0x5b) };
			pointArray[3] = pointArray2;
			_treePoints = pointArray;
			_drawTimer = new System.Timers.Timer(100.0);
			_drawTimer.Elapsed += DrawTimerElapsed;
			_drawTimer.Start();
		}

		private void EffectFrequencyDialog_FormClosing(object sender, FormClosingEventArgs e) {
			_drawTimer.Stop();
		}

		private void DrawTimerElapsed(object sender, ElapsedEventArgs e) {
			if (_frequency != 0) {
				BeginInvoke(m_refreshInvoker);
				if (++_tickCount == _maxColumn) {
					Regenerate();
				}
			}
		}

		private void pictureBoxExample_Paint(object sender, PaintEventArgs e) {
			if (_drawTimer.Enabled) {
				_solidBrush.Color = Color.FromArgb(_effectValues[0, _tickCount], Color.Red);
				e.Graphics.FillPolygon(_solidBrush, _treePoints[0]);
				_solidBrush.Color = Color.FromArgb(_effectValues[1, _tickCount], Color.Green);
				e.Graphics.FillPolygon(_solidBrush, _treePoints[1]);
				_solidBrush.Color = Color.FromArgb(_effectValues[2, _tickCount], Color.Blue);
				e.Graphics.FillPolygon(_solidBrush, _treePoints[2]);
				_solidBrush.Color = Color.FromArgb(_effectValues[3, _tickCount], Color.White);
				e.Graphics.FillPolygon(_solidBrush, _treePoints[3]);
			}
		}

		private void Regenerate() {
			if (_frequency != 0) {
				_drawTimer.Stop();
				_effectGenerator(_effectValues, new[] { _frequency });
				_tickCount = 0;
				_drawTimer.Start();
			}
		}

		private void trackBarFrequency_Scroll(object sender, EventArgs e) {
			_frequency = trackBarFrequency.Value;
			Regenerate();
		}

		public int Frequency {
			get {
				return trackBarFrequency.Value;
			}
		}
	}
}