using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using CommonControls;

using VixenPlus;

namespace Life {
    public partial class Life : UserControl, INutcrackerEffect {

        private const string LifeCount = "ID_SLIDER_Life{0}_Count";
        private const string LifeSeed = "ID_SLIDER_Life{0}_Seed";

        public Life() {
            _bufferWidth = 0;
            InitializeComponent();
        }


        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Life"; }
        }

        public string Notes {
            get { return "Does not use speed."; }
        }

        public bool UsesPalette {
            get { return true; }
        }

        public bool UsesSpeed {
            get { return false; }
        }

        public List<string> Settings {
            get { return GetCurrentSettings(); }
            set { Setup(value); }
        }

        private List<string> GetCurrentSettings() {
            return new List<string> {
                LifeCount + "=" + tbCellsToStart.Value,
                LifeSeed + "=" + tbType.Value
            };
        }

        private void Setup(IList<string> settings) {
            var effectNum = settings[0];
            var lifeCount = string.Format(LifeCount, effectNum);
            var lifeSeed = string.Format(LifeSeed, effectNum);

            foreach (var keyValue in settings.Select(s => s.Split(new[] { '=' }))) {
                if (keyValue[0].Equals(lifeCount)) {
                    tbCellsToStart.Value = keyValue[1].ToInt();
                }
                else if (keyValue[0].Equals(lifeSeed)) {
                    tbType.Value = keyValue[1].ToInt();
                }
            }
        }

        private readonly Random _random = new Random();
        private int _bufferHeight;
        private int _bufferWidth;
        private Color[,] _buffer;
        private Color[,] _currentGeneration;
        private Color[] _palette;

        private void InitalizeBuffer(int count) {
            if (_palette == null) return;

            for (var col = 0; col < _bufferWidth; col++)
            {
                for (var row = 0; row < _bufferHeight; row++) {
                    _buffer[row, col] = Color.Transparent;
                }
            }
            for (var i = 0; i < count; i++) {
                var col = _random.Next() % _bufferWidth;
                var row = _random.Next() % _bufferHeight;
                var color = GetMultiColor(_palette);
                SetPixel(col, row, color);
            }
            CopyGeneration();
        }


        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            _palette = palette;

            if (eventToRender == 0 || _buffer == null) {
                _bufferWidth = buffer.GetLength(Utils.IndexColsOrWidth);
                _bufferHeight = buffer.GetLength(Utils.IndexRowsOrHeight);
                _buffer = buffer;
                InitalizeBuffer(_bufferWidth * _bufferHeight * tbCellsToStart.Value / 200 + 1);
            }

            for (var x = 0; x < _bufferWidth; x++) {
                for (var y = 0; y < _bufferHeight; y++) {
                    var color = _currentGeneration[y, x];
                    var isLive = (color != Color.Black && color != Color.Transparent);
                    var cnt = CountNeighbors(x, y);
                    _buffer[y, x] = Color.Black; // By Default, kill off the cell
                    switch (tbType.Value) {
                        case 0:
                            if (isLive && cnt >= 2 && cnt <= 3) {
                                SetPixel(x, y, color);
                            }
                            else if (!isLive && cnt == 3) {
                                SetPixel(x, y, GetMultiColor(palette));
                            }
                            break;
                        case 1:
                            if (isLive && (cnt == 2 || cnt == 3 || cnt == 6)) {
                                SetPixel(x, y, color);
                            }
                            else if (!isLive && (cnt == 3 || cnt == 5)) {
                                SetPixel(x, y, GetMultiColor(palette));
                            }
                            break;
                        case 2:
                            if (isLive && (cnt == 1 || cnt == 3 || cnt == 5 || cnt == 8)) {
                                SetPixel(x, y, color);
                            }
                            else if (!isLive && (cnt == 3 || cnt == 5 || cnt == 7)) {
                                SetPixel(x, y, GetMultiColor(palette));
                            }
                            break;
                        case 3:
                            if (isLive && (cnt == 2 || cnt == 3 || cnt >= 5)) {
                                SetPixel(x, y, color);
                            }
                            else if (!isLive && (cnt == 3 || cnt == 7 || cnt == 8)) {
                                SetPixel(x, y, GetMultiColor(palette));
                            }
                            break;
                        case 4:
                            if (isLive && (cnt >= 5)) {
                                SetPixel(x, y, color);
                            }
                            else if (!isLive && (cnt == 2 || cnt >= 5)) {
                                SetPixel(x, y, GetMultiColor(palette));
                            }
                            break;
                    }
                }
            }
            CopyGeneration();
            return _buffer;
        }


        private void SetPixel(int x, int y, Color color) {
            _buffer[y, x] = color;
        }


        private Color GetMultiColor(Color[] palette) {
            return palette.GetMultiColorBlend(_random.Next(100) / 100.0, false);
        }


        private int CountNeighbors(int xHome, int yHome) {
            //  X -1   0  +1
            //                   Y
            //     2   3   4    -1
            //     1   X   5     0
            //     0   7   6    +1
            int[] neighborXOffsets = {-1, -1, -1, 0, 1, 1, 1, 0};
            int[] neighborYOffsets = {1, 0, -1, -1, -1, 0, 1, 1};
            
            var neighbors = 0;
            for (var i = 0; i < 8; i++) {
                var x = (xHome + neighborXOffsets[i]) % _bufferWidth;
                if (x < 0) {
                    x += _bufferWidth;
                }

                var y = (yHome + neighborYOffsets[i]) % _bufferHeight;
                if (y < 0) {
                    y += _bufferHeight;
                }

                if ((_currentGeneration[y, x] != Color.Black) && (_currentGeneration[y, x] != Color.Transparent)) {
                    neighbors++;
                }
            }

            return neighbors;
        }

        private void CopyGeneration() {
            _currentGeneration = new Color[_bufferHeight, _bufferWidth];
            for (var col = 0; col < _bufferWidth; col++) {
                for (var row = 0; row < _bufferHeight; row++) {
                    _currentGeneration[row, col] = _buffer[row, col];
                }
            }
        }

        private void Life_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(this, e);
        }
    }
}
