using System;
using System.Drawing;
using System.Globalization;
using System.Timers;
using System.Windows.Forms;

using CommonControls;

using CommonUtils;

using Timer = System.Timers.Timer;

namespace VixenEditor {


    internal partial class SparkleParamsDialog : Form {
        private int _decay;
        private int _frequency;
        private int _min;
        private int _max = 100;
        private int _tickCount;

        private readonly bool _actualLevels;
        private readonly Timer _drawTimer;
        private readonly FrequencyEffectGenerator _effectGenerator;
        private readonly byte[,] _effectValues;
        private readonly int _maxColumn;
        private readonly Point[][] _treePoints = new Point[4][];


        public SparkleParamsDialog(int maxFrequency, FrequencyEffectGenerator effectGenerator, byte minimum, byte maximum,
                                   byte currentDrawingIntensity, bool actualLevels) {
            InitializeComponent();
            _actualLevels = actualLevels;
            trackBarFrequency.Maximum = maxFrequency;
            _effectGenerator = effectGenerator;
            m_refreshInvoker = pictureBoxExample.Refresh;
            _effectValues = new byte[4,maxFrequency * 5];
            _maxColumn = _effectValues.GetLength(1);
            _min = minimum;
            _max = currentDrawingIntensity;
            var effectParameters = new int[4];
            effectParameters[2] = _min;
            effectParameters[3] = _max;
            effectGenerator(_effectValues, effectParameters);
            _tickCount = 0;
            
            _treePoints[0] = new[] {new Point(22, 36), new Point(37, 91), new Point(7, 91)};
            _treePoints[1] = new[] {new Point(67, 36), new Point(82, 91), new Point(52, 91)};
            _treePoints[2] = new[] {new Point(112, 36), new Point(127, 91), new Point(97, 91)};
            _treePoints[3] = new[] {new Point(157, 36), new Point(172, 91), new Point(142, 91)};
            
            _drawTimer = new Timer(100.0);
            _drawTimer.Elapsed += m_drawTimer_Elapsed;
            _drawTimer.Start();

            udMin.Minimum = udMax.Minimum = udMin.Value = actualLevels ? minimum : minimum.ToPercentage();
            udMin.Maximum = udMax.Maximum = actualLevels ? maximum : maximum.ToPercentage();
            udMax.Value = actualLevels ? _max : _max.ToPercentage();

            lblDecay.Text = trackBarDecay.Value.ToString(CultureInfo.InvariantCulture);
            lblFreq.Text = trackBarFrequency.Value.ToString(CultureInfo.InvariantCulture);
        }


        private void m_drawTimer_Elapsed(object sender, ElapsedEventArgs e) {
            BeginInvoke(m_refreshInvoker);
            if (++_tickCount == _maxColumn) {
                Regenerate();
            }
        }


        private void numericUpDownMax_ValueChanged(object sender, EventArgs e) {
            _max = (int) (_actualLevels ? udMax.Value : ((int) udMax.Value).ToValue());
            Regenerate();
        }


        private void numericUpDownMin_ValueChanged(object sender, EventArgs e) {
            _min = (int) (_actualLevels ? udMin.Value : ((int) udMin.Value).ToValue());
            Regenerate();
        }


        private void pictureBoxExample_Paint(object sender, PaintEventArgs e) {
            if (!_drawTimer.Enabled) {
                return;
            }

            using (var solidBrush = new SolidBrush(Color.Black)) {
                solidBrush.Color = Color.FromArgb(_effectValues[0, _tickCount], Color.Red);
                e.Graphics.FillPolygon(solidBrush, _treePoints[0]);
                solidBrush.Color = Color.FromArgb(_effectValues[1, _tickCount], Color.Green);
                e.Graphics.FillPolygon(solidBrush, _treePoints[1]);
                solidBrush.Color = Color.FromArgb(_effectValues[2, _tickCount], Color.Blue);
                e.Graphics.FillPolygon(solidBrush, _treePoints[2]);
                solidBrush.Color = Color.FromArgb(_effectValues[3, _tickCount], Color.White);
                e.Graphics.FillPolygon(solidBrush, _treePoints[3]);
            }
        }


        private void Regenerate() {
            _effectGenerator(_effectValues, new[] {_frequency, _decay, _min, _max});
            _tickCount = 0;
        }


        private void SparkleParamsDialog_FormClosing(object sender, FormClosingEventArgs e) {
            _drawTimer.Stop();
        }


        private void trackBarDecay_Scroll(object sender, EventArgs e) {
            _decay = trackBarDecay.Value;
            lblDecay.Text = trackBarDecay.Value.ToString(CultureInfo.InvariantCulture);
            Regenerate();
        }


        private void trackBarFrequency_Scroll(object sender, EventArgs e) {
            _frequency = trackBarFrequency.Value;
            lblFreq.Text = trackBarFrequency.Value.ToString(CultureInfo.InvariantCulture);
            Regenerate();
        }


        public int DecayTime {
            get { return trackBarDecay.Value; }
        }

        public int Frequency {
            get { return trackBarFrequency.Value; }
        }

        public byte MaximumIntensity {
            get { return (byte) _max; }
        }

        public byte MinimumIntensity {
            get { return (byte) _min; }
        }
    }
}
