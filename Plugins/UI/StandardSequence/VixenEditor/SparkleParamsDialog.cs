namespace VixenEditor {
	using System;
    using System.Drawing;
	using System.Timers;
	using System.Windows.Forms;

	internal partial class SparkleParamsDialog : Form {
		private readonly bool _actualLevels;
		private readonly SolidBrush _brush;
		private int _decay;
		private readonly System.Timers.Timer _drawTimer;
		private readonly FrequencyEffectGenerator _effectGenerator;
		private readonly byte[,] _effectValues;
		private int _frequency;
		private int _max = 100;
		private readonly int _maxColumn;
		private int _min;
		private int _tickCount;
		private readonly Point[][] _treePoints;

		public SparkleParamsDialog(int maxFrequency, FrequencyEffectGenerator effectGenerator, byte sequenceMin, byte sequenceMax, byte currentDrawingIntensity, bool actualLevels) {
			InitializeComponent();
			_actualLevels = actualLevels;
			_brush = new SolidBrush(Color.Black);
			trackBarFrequency.Maximum = maxFrequency;
			_effectGenerator = effectGenerator;
			m_refreshInvoker = pictureBoxExample.Refresh;
			_effectValues = new byte[4, maxFrequency * 5];
			_maxColumn = _effectValues.GetLength(1);
			_min = sequenceMin;
			_max = currentDrawingIntensity;
			var effectParameters = new int[4];
			effectParameters[2] = _min;
			effectParameters[3] = _max;
			effectGenerator(_effectValues, effectParameters);
			_tickCount = 0;
			var pointArray = new Point[4][];
			var pointArray2 = new[] { new Point(22, 46), new Point(37, 91), new Point(7, 91) };
			pointArray[0] = pointArray2; // +45,0 - TODO Look at putting this in a loop or function, I've seen it elsewhere
			pointArray2 = new[] { new Point(67, 46), new Point(82, 91), new Point(52, 91) };
			pointArray[1] = pointArray2;
			pointArray2 = new[] { new Point(112, 46), new Point(127, 91), new Point(97, 91) };
			pointArray[2] = pointArray2;
			pointArray2 = new[] { new Point(157, 46), new Point(172, 91), new Point(142, 91) };
			pointArray[3] = pointArray2;
			_treePoints = pointArray;
			_drawTimer = new System.Timers.Timer(100.0);
			_drawTimer.Elapsed += m_drawTimer_Elapsed;
			_drawTimer.Start();
			if (actualLevels) {
				numericUpDownMin.Minimum = numericUpDownMax.Minimum = numericUpDownMin.Value = sequenceMin;
				numericUpDownMin.Maximum = numericUpDownMax.Maximum = sequenceMax;
				numericUpDownMax.Value = _max;
			}
			else {
				numericUpDownMin.Minimum = numericUpDownMax.Minimum = numericUpDownMin.Value = (int)Math.Round(sequenceMin * 100f / 255f, MidpointRounding.AwayFromZero);
				numericUpDownMin.Maximum = numericUpDownMax.Maximum = (int)Math.Round(sequenceMax * 100f / 255f, MidpointRounding.AwayFromZero);
				numericUpDownMax.Value = (int)Math.Round(_max * 100f / 255f, MidpointRounding.AwayFromZero);
			}
		}

		private void m_drawTimer_Elapsed(object sender, ElapsedEventArgs e) {
			BeginInvoke(m_refreshInvoker);
			if (++_tickCount == _maxColumn) {
				Regenerate();
			}
		}

		private void numericUpDownMax_ValueChanged(object sender, EventArgs e) {
			if (_actualLevels) {
				_max = (int)numericUpDownMax.Value;
			}
			else {
				_max = (((int)numericUpDownMax.Value) * 0xff) / 100;
			}
			Regenerate();
		}

		private void numericUpDownMin_ValueChanged(object sender, EventArgs e) {
			if (_actualLevels) {
				_min = (int)numericUpDownMin.Value;
			}
			else {
				_min = (((int)numericUpDownMin.Value) * 0xff) / 100;
			}
			Regenerate();
		}

		private void pictureBoxExample_Paint(object sender, PaintEventArgs e) {
			if (_drawTimer.Enabled) {
				_brush.Color = Color.FromArgb(_effectValues[0, _tickCount], Color.Red);
				e.Graphics.FillPolygon(_brush, _treePoints[0]);
				_brush.Color = Color.FromArgb(_effectValues[1, _tickCount], Color.Green);
				e.Graphics.FillPolygon(_brush, _treePoints[1]);
				_brush.Color = Color.FromArgb(_effectValues[2, _tickCount], Color.Blue);
				e.Graphics.FillPolygon(_brush, _treePoints[2]);
				_brush.Color = Color.FromArgb(_effectValues[3, _tickCount], Color.White);
				e.Graphics.FillPolygon(_brush, _treePoints[3]);
			}
		}

		private void Regenerate() {
			_drawTimer.Stop();
			_effectGenerator(_effectValues, new[] { _frequency, _decay, _min, _max });
			_tickCount = 0;
			_drawTimer.Start();
		}

		private void SparkleParamsDialog_FormClosing(object sender, FormClosingEventArgs e) {
			_drawTimer.Stop();
		}

		private void trackBarDecay_Scroll(object sender, EventArgs e) {
			_decay = trackBarDecay.Value;
			Regenerate();
		}

		private void trackBarFrequency_Scroll(object sender, EventArgs e) {
			_frequency = trackBarFrequency.Value;
			Regenerate();
		}

		public int DecayTime {
			get {
				return trackBarDecay.Value;
			}
		}

		public int Frequency {
			get {
				return trackBarFrequency.Value;
			}
		}

		public byte MaximumIntensity {
			get {
				return (byte)_max;
			}
		}

		public byte MinimumIntensity {
			get {
				return (byte)_min;
			}
		}
	}
}