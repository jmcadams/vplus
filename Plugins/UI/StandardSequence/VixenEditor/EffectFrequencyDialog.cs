using System.Globalization;

using Properties;

namespace VixenEditor {
    using System;
    using System.Drawing;
    using System.Timers;
    using System.Windows.Forms;

    internal partial class EffectFrequencyDialog : Form {

        private readonly System.Timers.Timer _drawTimer;
        private readonly FrequencyEffectGenerator _effectGenerator;
        private readonly byte[,] _effectValues;
        private int _frequency;
        private readonly int _maxColumn;
        private int _tickCount;
        private readonly Point[][] _treePoints = new Point[4][];


        public EffectFrequencyDialog(string effectName, int maxFrequency, FrequencyEffectGenerator effectGenerator) {
            InitializeComponent();
            Text = effectName;
            groupBox1.Text = effectName + @" " + Resources.Frequency;
            trackBarFrequency.Maximum = maxFrequency;
            _effectGenerator = effectGenerator;
            m_refreshInvoker = pictureBoxExample.Refresh;
            _effectValues = new byte[4,maxFrequency * 5];
            _maxColumn = _effectValues.GetLength(1);
            effectGenerator(_effectValues, new[] {1});
            _tickCount = 0;
            _treePoints[0] = new[] {new Point(22, 36), new Point(37, 91), new Point(7, 91)};
            _treePoints[1] = new[] {new Point(67, 36), new Point(82, 91), new Point(52, 91)};
            _treePoints[2] = new[] {new Point(112, 36), new Point(127, 91), new Point(97, 91)};
            _treePoints[3] = new[] {new Point(157, 36), new Point(172, 91), new Point(142, 91)};
            _drawTimer = new System.Timers.Timer(100.0);
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
            if (_frequency == 0) {
                return;
            }

            _drawTimer.Stop();
            _effectGenerator(_effectValues, new[] {_frequency});
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
