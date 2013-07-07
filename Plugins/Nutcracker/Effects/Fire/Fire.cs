using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

using CommonUtils;
using VixenPlus;

namespace Fire {
    public partial class Fire : UserControl, INutcrackerEffect {
        public Fire() {
            InitializeComponent();
        }


        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Fire"; }
        }

        public string Notes {
            get { return "Does not use speed control."; }
        }

        public bool UsesPalette {
            get { return true; }
        }

        public bool UsesSpeed {
            get { return false; }
        }

        public XmlElement Settings {
            get { return GetCurrentSettings(); }
            set { Setup(value); }
        }


        private static XmlElement GetCurrentSettings() {
            return Xml.CreateXmlDocument().DocumentElement;
        }


        private static void Setup(XmlElement settings) {
            System.Diagnostics.Debug.Print(settings.ToString());
        }


        private Color[] _firePalette;


        private void InitFirePalette(float hue) {
            _firePalette = new Color[200];
            var hsv = new HSVUtils {Hue = hue, Saturation = 1.0f};

            for (var i = 0; i < 100; i++) {
                hsv.Value = i / 100.0f;
                _firePalette[i] = HSVUtils.HSVtoColor(hsv);
            }

            hsv.Value = 1.0f;
            for (var i = 0; i < 100; i++) {
                _firePalette[i + 100] = HSVUtils.HSVtoColor(hsv);
                hsv.Hue = (hsv.Hue + 0.00166666f) % 1f;
            }
        }


        private int[] _fireBuffer = new int[1];
        private readonly Random _random = new Random();
        private int _width;
        private int _height;
        private Color[] _palette;


        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            var bufferHeight = buffer.GetLength(Utils.IndexRowsOrHeight);
            var bufferWidth = buffer.GetLength(Utils.IndexColsOrWidth);
            _palette = palette;

            if (eventToRender == 0 || _fireBuffer.Length == 1 || _firePalette == null) {
                InitFirePalette(chkBoxUsePalette.Checked ? _palette[0].GetHue() / 360f : 0.0f);
                _fireBuffer = new int[bufferHeight * bufferWidth];
                _width = bufferWidth;
                _height = bufferHeight;
                for (var i = 0; i < _fireBuffer.Length; i++) {
                    _fireBuffer[i] = 0;
                }
            }
            for (var col = 0; col < bufferWidth; col++) {
                var r = col % 2 == 0 ? 190 + (_random.Next() % 10) : 100 + (_random.Next() % 50);
                SetFireBuffer(col, r);
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
                    var newIndex = n > 0 ? sum / n : 0;
                    if (newIndex > 0) {
                        newIndex += (_random.Next() % 100 < 20) ? step : -step;
                        if (newIndex < 0) newIndex = 0;
                        // ReSharper disable PossibleNullReferenceException
                        if (newIndex >= _firePalette.Length) newIndex = _firePalette.Length - 1;
                        // ReSharper restore PossibleNullReferenceException
                    }
                    SetFireBuffer(row * bufferWidth + col, newIndex);
                }
            }
            for (var row = 0; row < bufferHeight; row++) {
                for (var col = 0; col < bufferWidth; col++) {
                    // ReSharper disable PossibleNullReferenceException
                    buffer[row, col] = _firePalette[GetFireBuffer(col, row)];
                    // ReSharper restore PossibleNullReferenceException
                }
            }
            return buffer;
        }


        private void SetFireBuffer(int cell, int value) {
            if (cell >= 0 && cell < _fireBuffer.Length) {
                _fireBuffer[cell] = value;
            }
        }


        private int GetFireBuffer(int x, int y) {
            var cell = y * _width + x;
            return x >= 0 && x < _width && y >= 0 && y < _height && cell < _fireBuffer.Length ? _fireBuffer[cell] : -1;
        }


        private void Fire_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, e);
        }
    }
}
