using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using VixenPlusCommon;
using VixenPlusCommon.Annotations;

namespace NutcrackerEffects.Effects {
    [UsedImplicitly]
    public partial class Snowflakes : UserControl, INutcrackerEffect {

        private const string SnowflakesCount = "ID_SLIDER_Snowflakes{0}_Count";
        private const string SnowflakesType = "ID_SLIDER_Snowflakes{0}_Type";

        public Snowflakes() {
            InitializeComponent();
        }

        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Snowflakes"; }
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
                SnowflakesCount + "=" + tbMaxFlakes.Value,
                SnowflakesType + "=" + tbType.Value
            };
        }

        private void Setup(IList<string> settings) {
            var effectNum = settings[0];
            var snowflakesCount = string.Format(SnowflakesCount, effectNum);
            var snowflakesType = string.Format(SnowflakesType, effectNum);

            foreach (var keyValue in settings.Select(s => s.Split(new[] { '=' }))) {
                if (keyValue[0].Equals(snowflakesCount)) {
                    tbMaxFlakes.Value = keyValue[1].ToInt();
                }
                else if (keyValue[0].Equals(snowflakesType)) {
                    tbType.Value = keyValue[1].ToInt();
                }
            }
        }

        private readonly Random _random = new Random();
        private Color[,] _tempBuf;
        private Color[] _palette;
        private int _bufferHeight;
        private int _bufferWidth;


        private void InitializeSnowflakes() {
            if (_palette == null) return;
            var x = 0;
            var y = 0;
            var color1 = _palette[0];
            var color2 = _palette.Length > 1 ? _palette[1] : _palette[0];
            ClearTempBuf();
            for (var n = 0; n < tbMaxFlakes.Value; n++) {
               var deltaY = _bufferHeight / 4;
                var y0 = (n % 4) * deltaY;
                if (y0 + deltaY > _bufferHeight) deltaY = _bufferHeight - y0;
                for (var check = 0; check < 20; check++) {
                    x = NextRandom() % _bufferWidth;
                    y = y0 + (NextRandom() % deltaY);
                    if (GetTempPixel(x, y) == Color.Transparent) break;
                }
                switch (tbType.Value == 0 ? NextRandom() % 5 : tbType.Value - 1) {
                    case 0:
                        SetTempPixel(x, y, color1);
                        break;
                    case 1:
                        if (x < 1) x += 1;
                        if (y < 1) y += 1;
                        if (x > _bufferWidth - 2) x -= 1;
                        if (y > _bufferHeight - 2) y -= 1;
                        SetTempPixel(x, y, color1);
                        SetTempPixel(x - 1, y, color2);
                        SetTempPixel(x + 1, y, color2);
                        SetTempPixel(x, y - 1, color2);
                        SetTempPixel(x, y + 1, color2);
                        break;
                    case 2:
                        if (x < 1) x += 1;
                        if (y < 1) y += 1;
                        if (x > _bufferWidth - 2) x -= 1;
                        if (y > _bufferHeight - 2) y -= 1;
                        SetTempPixel(x, y, color1);
                        if (NextRandom() % 100 > 50) {
                            SetTempPixel(x - 1, y, color2);
                            SetTempPixel(x + 1, y, color2);
                        }
                        else {
                            SetTempPixel(x, y - 1, color2);
                            SetTempPixel(x, y + 1, color2);
                        }
                        break;
                    case 3:
                        if (x < 2) x += 2;
                        if (y < 2) y += 2;
                        if (x > _bufferWidth - 3) x -= 2;
                        if (y > _bufferHeight - 3) y -= 2;
                        SetTempPixel(x, y, color1);
                        for (var i = 1; i <= 2; i++) {
                            SetTempPixel(x - i, y, color2);
                            SetTempPixel(x + i, y, color2);
                            SetTempPixel(x, y - i, color2);
                            SetTempPixel(x, y + i, color2);
                        }
                        break;
                    case 4:
                        if (x < 2) x += 2;
                        if (y < 2) y += 2;
                        if (x > _bufferWidth - 3) x -= 2;
                        if (y > _bufferHeight - 3) y -= 2;
                        SetTempPixel(x, y, color1);
                        SetTempPixel(x - 1, y, color2);
                        SetTempPixel(x + 1, y, color2);
                        SetTempPixel(x, y - 1, color2);
                        SetTempPixel(x, y + 1, color2);

                        SetTempPixel(x - 1, y + 2, color2);
                        SetTempPixel(x + 1, y + 2, color2);
                        SetTempPixel(x - 1, y - 2, color2);
                        SetTempPixel(x + 1, y - 2, color2);
                        SetTempPixel(x + 2, y - 1, color2);
                        SetTempPixel(x + 2, y + 1, color2);
                        SetTempPixel(x - 2, y - 1, color2);
                        SetTempPixel(x - 2, y + 1, color2);
                        break;
                }
            }
        }


        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            _palette = palette;
            _bufferHeight = buffer.GetLength(0);
            _bufferWidth = buffer.GetLength(1);


            if (eventToRender == 0 || _tempBuf == null) {
                InitializeSnowflakes();
            }

            // move snowflakes
            for (var x = 0; x < _bufferWidth; x++) {
                var newX = (x + eventToRender / 20) % _bufferWidth; // CW
                var newX2 = (x - eventToRender / 20) % _bufferWidth; // CCW
                if (newX2 < 0) {
                    newX2 += _bufferWidth;
                }
                for (var y = 0; y < _bufferHeight; y++) {
                    var newY = (y + eventToRender / 10) % _bufferHeight;
                    var newY2 = (newY + _bufferHeight / 2) % _bufferHeight;
                    var color1 = GetTempPixel(newX, newY);
                    if (color1 == Color.Transparent) {
                        color1 = GetTempPixel(newX2, newY2);
                    }
                    buffer[y,x] = color1;
                }
            }
            return buffer;
        }


        private void SetTempPixel(int x, int y, Color color) {
            _tempBuf[y, x] = color;
        }

        private Color GetTempPixel(int x, int y) {
            return _tempBuf[y, x];
        }

        private void ClearTempBuf() {
            _tempBuf = new Color[_bufferHeight,_bufferWidth];
            for (var col = 0; col < _bufferWidth; col++) {
                for (var row = 0; row < _bufferHeight; row++) {
                    _tempBuf[row,col] = Color.Black;
                }
            }
        }

        private int NextRandom() {
            return _random.Next();
        }

        private void Snowflakes_ControlChanged(object sender, EventArgs e) {
            InitializeSnowflakes();
            OnControlChanged(this, new EventArgs());
        }
    }
}
