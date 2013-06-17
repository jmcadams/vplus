using System;
using System.Drawing;
using System.Windows.Forms;
using CommonUtils;
using VixenPlus;

namespace Fire {
    public partial class Fire : UserControl, INutcrackerEffect {
        public Fire() {
            InitializeComponent();
            InitFirePalette();
        }
        
        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Fire"; }
        }

        private readonly Color[] _firePalette = new Color[200];

        private void InitFirePalette() {
            var hsv = new HSVUtils();

            int i;
            hsv.Hue = 0.0f;
            hsv.Saturation = 1.0f;
            for (i = 0; i < 100; i++) {
                hsv.Value = i / 100.0f;
                _firePalette[i] = HSVUtils.HSVtoColor(hsv); 
            }

            hsv.Value = 1.0f;
            for (i = 0; i < 100; i++) {
                _firePalette[i + 100] = HSVUtils.HSVtoColor(hsv);
                hsv.Hue += 0.00166666f;
            }
        }

        private int[] _fireBuffer = new int[1];
        private readonly Random _random = new Random();
        private int _width;
        private int _height;

        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            var bufferHeight = buffer.GetLength(Utils.IndexRowsOrHeight);
            var bufferWidth = buffer.GetLength(Utils.IndexColsOrWidth);

            if (eventToRender == 0) {
                _fireBuffer = new int[bufferHeight * bufferWidth];
                _width = bufferWidth;
                _height = bufferHeight;
                for (var i = 0; i < _fireBuffer.Length; i++) {
                    _fireBuffer[i] = 0;
                }
            }
            for (var col = 0; col < bufferWidth; col++) {
                var r = col % 2 == 0 ? 190 + (_random.Next() % 10) : 100 + (_random.Next() % 50);
                _fireBuffer[col] = r;
            }
            var step = 255 * 100 / bufferHeight / tbHeight.Value;
            for (var row = 1; row < bufferHeight; row++) {
                for (var col = 0; col < bufferWidth; col++) {
                    var v1 = GetFireBuffer(col - 1, row - 1);
                    var v2 = GetFireBuffer(col + 1, row - 1);
                    var v3 = GetFireBuffer(col, row - 1);
                    var v4 = GetFireBuffer(col, row - 1);
                    var n = 0;
                    var sum = 0;
                    if (v1 >= 0) {
                        sum += v1;
                        n++;
                    }
                    if (v2 >= 0) {
                        sum += v2;
                        n++;
                    }
                    if (v3 >= 0) {
                        sum += v3;
                        n++;
                    }
                    if (v4 >= 0) {
                        sum += v4;
                        n++;
                    }
                    var  newIndex = n > 0 ? sum / n : 0;
                    if (newIndex > 0) {
                        newIndex += (_random.Next() % 100 < 20) ? step : -step;
                        if (newIndex < 0) newIndex = 0;
                        if (newIndex >= _firePalette.Length) newIndex = _firePalette.Length - 1;
                    }
                    _fireBuffer[row * bufferWidth + col] = newIndex;
                }
            }
            for (var row = 0; row < bufferHeight; row++) {
                for (var col = 0; col < bufferWidth; col++) {
                    buffer[row, col] = _firePalette[_fireBuffer[row*bufferWidth + col]];
                }
            }
            return buffer;
        }

        private int GetFireBuffer(int x, int y) {
            return x >= 0 && x < _width && y >= 0 && y < _height ? _fireBuffer[y*_width + x] : -1;
        }


        private void Fire_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, new EventArgs());
        }
    }
}
