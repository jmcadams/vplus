using System;
using System.Globalization;
using System.Timers;
using System.Windows.Forms;

using VixenPlusCommon;

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
            

            
            _drawTimer = new Timer(100.0);
            _drawTimer.Elapsed += drawTimer_Elapsed;
            _drawTimer.Start();

            udMin.Minimum = udMax.Minimum = udMin.Value = actualLevels ? minimum : minimum.ToPercentage();
            udMin.Maximum = udMax.Maximum = actualLevels ? maximum : maximum.ToPercentage();
            udMax.Value = actualLevels ? _max : _max.ToPercentage();

            lblDecay.Text = trackBarDecay.Value.ToString(CultureInfo.InvariantCulture);
            lblFreq.Text = trackBarFrequency.Value.ToString(CultureInfo.InvariantCulture);
        }


        private void drawTimer_Elapsed(object sender, ElapsedEventArgs e) {
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

            SparkleTree.DrawTrees(e.Graphics, _effectValues, _tickCount);
        }


        private void Regenerate() {
            _effectGenerator(_effectValues, _frequency, _decay, _min, _max);
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
