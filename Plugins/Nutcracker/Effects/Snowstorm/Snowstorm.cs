using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using CommonUtils;

namespace Snowstorm {
    public partial class Snowstorm : UserControl, INutcrackerEffect {

        private const string SnowstormCount = "ID_SLIDER_Snowstorm{0}_Count";
        private const string SnowstormLength = "ID_SLIDER_Snowstorm{0}_Length";

        public Snowstorm() {
            InitializeComponent();
        }


        public event EventHandler OnControlChanged;
        private readonly Random _random = new Random();

        public string EffectName {
            get { return "Snowstorm"; }
        }

        public string Notes {
            get { return String.Empty; }
        }

        public bool UsesPalette {
            get { return true; }
        }

        public bool UsesSpeed {
            get { return true; }
        }

        public List<string> Settings {
            get { return GetCurrentSettings(); }
            set { Setup(value); }
        }


        private List<string> GetCurrentSettings() {
            return new List<string> {
                SnowstormCount + "=" + tbMaxFlakes.Value,
                SnowstormLength + "=" + tbTailLength.Value
            };
        }


        private void Setup(IList<string> settings) {
            var effectNum = settings[0];
            var snowstormCount = string.Format(SnowstormCount, effectNum);
            var snowstormLength = string.Format(SnowstormLength, effectNum);

            foreach (var keyValue in settings.Select(s => s.Split(new[] { '=' }))) {
                if (keyValue[0].Equals(snowstormCount)) {
                    tbMaxFlakes.Value = keyValue[1].ToInt();
                }
                else if (keyValue[0].Equals(snowstormLength)) {
                    tbTailLength.Value = keyValue[1].ToInt();
                }
            }
        }

        private class SnowstormClass {
            public readonly List<Point> Points = new List<Point>();
            public HSV HSV;
            public int Idx, SsDecay;


            public SnowstormClass() {
                Points.Clear();
            }
        }

        private readonly List<SnowstormClass> _snowstormItems = new List<SnowstormClass>();
        private int _bufferHeight;
        private int _bufferWidth;
        private Color[] _palette;


        private void InitializeSnowstorm() {
            if (_palette == null) return;

            var hsv0 = _palette[0].ToHSV();
            var hsv1 = (_palette.Length > 1 ? _palette[1] : _palette[0]).ToHSV();

            var count = Convert.ToInt32(_bufferWidth * _bufferHeight * tbMaxFlakes.Value / 2000) + 1;
            var tailLength = _bufferWidth * _bufferHeight * tbTailLength.Value / 2000 + 2;
            var xy = new Point();
            // create snowstorm elements
            _snowstormItems.Clear();
            for (var i = 0; i < count; i++) {
                var ssItem = new SnowstormClass {Idx = i, SsDecay = 0};
                ssItem.Points.Clear();
                ssItem.HSV = hsv0.CreateRangeTo(hsv1);
                // start in a random state
                var r = Rand() % (2 * tailLength);
                if (r > 0) {
                    xy.X = Rand() % _bufferWidth;
                    xy.Y = Rand() % _bufferHeight;
                    //ssItem.points.push_back(xy);
                    ssItem.Points.Add(xy);
                }
                if (r >= tailLength) {
                    ssItem.SsDecay = r - tailLength;
                    r = tailLength;
                }
                for (var j = 1; j < r; j++) {
                    SnowstormAdvance(ssItem);
                }
                //SnowstormItems.push_back(ssItem);
                _snowstormItems.Add(ssItem);
            }
        }


        private int _lastRenderedEvent = -1;
        
        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            _bufferHeight = buffer.GetLength(Utils.IndexRowsOrHeight);
            _bufferWidth = buffer.GetLength(Utils.IndexColsOrWidth);
            _palette = palette;
            var speed = eventToRender - _lastRenderedEvent;
            _lastRenderedEvent = eventToRender;
            var tailLength = _bufferWidth * _bufferHeight * tbTailLength.Value / 2000 + 2;
            var xy = new Point();
            if (eventToRender == 0 || _snowstormItems.Count == 0) {
                InitializeSnowstorm();
            }

            foreach (var it in _snowstormItems) {
                if (it.Points.Count > tailLength) {
                    if (it.SsDecay > tailLength) {
                        it.Points.Clear(); // start over
                        it.SsDecay = 0;
                    }
                    else if (Rand() % 20 < speed) {
                        it.SsDecay++;
                    }
                }
                if (it.Points.Count == 0) {
                    xy.X = Rand() % _bufferWidth;
                    xy.Y = Rand() % _bufferHeight;
                    //it.points.push_back(xy);
                    it.Points.Add(xy);
                }
                else if (Rand() % 20 < speed) {
                    SnowstormAdvance(it);
                }
                var sz = it.Points.Count;
                for (var pt = 0; pt < sz; pt++) {
                    var hsv = it.HSV;
                    hsv.Value = (float) (1.0 - (double) (sz - pt + it.SsDecay) / tailLength);
                    if (hsv.Value < 0.0) {
                        hsv.Value = 0.0f;
                    }
                    buffer[it.Points[pt].Y, it.Points[pt].X] = hsv.ToColor();
                }
                //cnt++;
            }

            return buffer;
        }


        private int Rand() {
            return _random.Next();
        }


        private static Point SnowstormVector(int idx) {
            var xy = new Point();
            switch (idx) {
                case 0:
                    xy.X = -1;
                    xy.Y = 0;
                    break;
                case 1:
                    xy.X = -1;
                    xy.Y = -1;
                    break;
                case 2:
                    xy.X = 0;
                    xy.Y = -1;
                    break;
                case 3:
                    xy.X = 1;
                    xy.Y = -1;
                    break;
                case 4:
                    xy.X = 1;
                    xy.Y = 0;
                    break;
                case 5:
                    xy.X = 1;
                    xy.Y = 1;
                    break;
                case 6:
                    xy.X = 0;
                    xy.Y = 1;
                    break;
                default:
                    xy.X = -1;
                    xy.Y = 1;
                    break;
            }
            return xy;
        }


        private void SnowstormAdvance(SnowstormClass ssItem) {
            const int cnt = 8; // # of integers in each set in arr[]
            int[] arr = {30, 20, 10, 5, 0, 5, 10, 20, 20, 15, 10, 10, 10, 10, 10, 15}; // 2 sets of 8 numbers, each of which add up to 100
            var adv = SnowstormVector(7);
            var i0 = ssItem.Idx % 7 <= 4 ? 0 : cnt;
            var r = Rand() % 100;
            for (int i = 0, val = 0; i < cnt; i++) {
                val += arr[i0 + i];
                if (r >= val) {
                    continue;
                }
                adv = SnowstormVector(i);
                break;
            }
            if (ssItem.Idx % 3 == 0) {
                adv.X *= 2;
                adv.Y *= 2;
            }
            //Point xy = ssItem.points.back() + adv;
            var xy = ssItem.Points[ssItem.Points.Count - 1];
            xy.X += adv.X;
            xy.Y += adv.Y;

            xy.X %= _bufferWidth;
            xy.Y %= _bufferHeight;
            if (xy.X < 0) {
                xy.X += _bufferWidth;
            }
            if (xy.Y < 0) {
                xy.Y += _bufferHeight;
            }
            //ssItem.points.push_back(xy);
            ssItem.Points.Add(xy);
        }


        private void SnowStorm_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, e);
        }
    }
}
