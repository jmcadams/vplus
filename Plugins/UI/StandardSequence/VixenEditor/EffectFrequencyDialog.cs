using System;
using System.Globalization;
using System.Timers;
using System.Windows.Forms;

using VixenPlus.Properties;

using VixenPlusCommon;

using Timer = System.Timers.Timer;

namespace VixenEditor {
    internal partial class EffectFrequencyDialog : Form {

        private readonly Timer _drawTimer;
        private readonly FrequencyEffectGenerator _effectGenerator;
        private readonly byte[,] _effectValues;
        private int _frequency;
        private readonly int _maxColumn;
        private int _tickCount;


        public EffectFrequencyDialog(string effectName, int maxFrequency, FrequencyEffectGenerator effectGenerator) {
            InitializeComponent();
            Text = effectName;
            groupBox1.Text = effectName + @" " + Resources.Frequency;
            trackBarFrequency.Maximum = maxFrequency;
            _effectGenerator = effectGenerator;
            m_refreshInvoker = pictureBoxExample.Refresh;
            _effectValues = new byte[4,maxFrequency * 5];
            _maxColumn = _effectValues.GetLength(1);
            effectGenerator(_effectValues, 1);
            _tickCount = 0;
            _drawTimer = new Timer(100.0);
            _drawTimer.Elapsed += DrawTimerElapsed;
            _drawTimer.Start();
            lblValue.Text = trackBarFrequency.Value.ToString(CultureInfo.InvariantCulture);
        }


        public override sealed string Text {
            get { return base.Text; }
            set { base.Text = value; }
        }


        private void EffectFrequencyDialog_FormClosing(object sender, FormClosingEventArgs e) {
            _drawTimer.Stop();
        }


        private void DrawTimerElapsed(object sender, ElapsedEventArgs e) {
            if (_frequency == 0) {
                return;
            }

            BeginInvoke(m_refreshInvoker);
            if (++_tickCount == _maxColumn) {
                Regenerate();
            }
        }


        private void pictureBoxExample_Paint(object sender, PaintEventArgs e) {
            if (!_drawTimer.Enabled) {
                return;
            }

            SparkleTree.DrawTrees(e.Graphics, _effectValues, _tickCount);
        }


        private void Regenerate() {
            if (_frequency == 0) {
                return;
            }

            _drawTimer.Stop();
            _effectGenerator(_effectValues, _frequency);
            _tickCount = 0;
            _drawTimer.Start();
        }


        private void trackBarFrequency_Scroll(object sender, EventArgs e) {
             _frequency = trackBarFrequency.Value;
            lblValue.Text = trackBarFrequency.Value.ToString(CultureInfo.InvariantCulture);
            Regenerate();
        }


        public int Frequency {
            get { return trackBarFrequency.Value; }
        }
    }
}
