using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using NutcrackerEffectsControl;

using VixenPlus;

namespace VixenEditor
{
    public partial class NutcrackerControlDialog: Form
    {
        private readonly Stopwatch _sw = new Stopwatch();
        private int[] _eventToRender = new[] {0, 0};
        private bool[] _isRendering = new[] {false, false};
        private readonly NutcrackerEffectControl[] _effectControls;
        private int _rows;
        private int _cols;
        private Color[][,] _buffers;

        private const int MaxEffects = 2;

        private enum Layers { Effect1, Effect2, Mask1, Mask2, Unmask1, Unmask2, Layered, Average }

        public NutcrackerControlDialog()
        {   
            InitializeComponent();
            _effectControls = new[] {nutcrackerEffectControl1, nutcrackerEffectControl2};
        }


        private void cbRender_CheckedChanged(object sender, EventArgs e) {
            if (cbRender.Checked) {
                _rows = (int)nudRows.Value;
                _cols = (int)nudColumns.Value;
                if (_rows > 0 && _cols > 0) {
                    nudRows.Enabled = false;
                    nudColumns.Enabled = false;
                    _buffers = new[] {new Color[_rows,_cols], new Color[_rows,_cols]};
                    _eventToRender = new[] {0,0};
                    _isRendering = new[] {false, false};
                    timerRender.Start();
                }
                else {
                    cbRender.Checked = false;
                }
            }
            else {
                nudRows.Enabled = true;
                nudColumns.Enabled = true;
                timerRender.Stop();
            }

        }


        private void timerRender_Tick(object sender, EventArgs e) {
            _sw.Start();

            var effects = GetEffects();
            var isDataUpdated = false;
            for (var i = 0; i < MaxEffects; i++) {
                // Initialize the buffer, if it is not rendering, since the rendering routine depends on it.
                if (!_isRendering[i]) {
                    _isRendering[i] = true;
                    InitializeBuffer(i);
                }

                if (effects[i] == null) {
                    _isRendering[i] = false;
                    continue;
                }

                isDataUpdated = true; 
                _buffers[i] = effects[i].RenderEffect(_buffers[i], _effectControls[i].GetPalette(), _eventToRender[i]);
                
                _eventToRender[i] += _effectControls[i].GetSpeed();
                _isRendering[i] = false;
            }
            
            if (isDataUpdated) {
                Render();
            } 
            
            tbInfo.Text = _sw.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture);
            _sw.Reset();
        }

        private List<INutcrackerEffect> GetEffects() {
            var effects = new List<INutcrackerEffect>(MaxEffects);
            for (var i = 0; i < MaxEffects; i++) {
                if (_isRendering[i]) {
                    effects.Add(null);
                    continue;
                }
                effects.Add((INutcrackerEffect)_effectControls[i].GetCurrentEffectControl());
            }
            return effects;
        }


        private void InitializeBuffer(int bufferNum) {
            var buffer = _buffers[bufferNum];
            for (var row = 0; row < _rows; row++) {
                for (var col = 0; col < _cols; col++) {
                    buffer[row, col] = Color.Transparent;
                }
            }
        }

        private void Render() {
            using (var g = pbPreview.CreateGraphics()) {
                // Bitmap is width (col) then height (row), we pass data like Vixen+, hight (row) then width (col)
                var bitmap = new Bitmap(_cols, _rows, g);
                for (var row = 0; row < _rows; row++) {
                    for (var column = 0; column < _cols; column++) {
                        var color = GetLayerColor(row, column);
                        bitmap.SetPixel(column, _rows - 1 - row, color == Color.Transparent ? Color.Black : color);
                    }
                }
                g.DrawImage(bitmap, new Point(10, 10));
            }
        }


        private Color GetLayerColor(int row, int column) {
            var returnValue = Color.Transparent;

            var effect1 = _buffers[0][row, column];
            var effect2 = _buffers[1][row, column];

            switch (GetEffectLayer()) {
                case Layers.Effect1:
                    returnValue = effect1;
                    break;
                case Layers.Effect2:
                    returnValue = effect2;
                    break;
                case Layers.Mask1:
                    returnValue = (IsBlackOrTransparent(effect1)) ? effect2 : Color.Black;
                    break;
                case Layers.Mask2:
                    returnValue = (IsBlackOrTransparent(effect2)) ? effect1 : Color.Black;
                    break;
                case Layers.Unmask1:
                    returnValue = (!IsBlackOrTransparent(effect1)) ? effect2 : Color.Black;
                    break;
                case Layers.Unmask2:
                    returnValue = (!IsBlackOrTransparent(effect2)) ? effect1 : Color.Black;
                    break;
                case Layers.Layered:
                    returnValue = (IsBlackOrTransparent(effect2)) ? effect1 : effect2;
                    break;
                case Layers.Average:
                    returnValue = (IsBlackOrTransparent(effect1))
                                      ? effect2 : (IsBlackOrTransparent(effect2)) ? effect1 : GetAverageColor(effect1, effect2);
                    break;
            }

            return returnValue;
        }


        private Layers GetEffectLayer() {
            var effectLayer = Layers.Average;
            if (rbEffect1.Checked) effectLayer = Layers.Effect1;
            if (rbEffect2.Checked) effectLayer = Layers.Effect2;
            if (rbMask1.Checked) effectLayer = Layers.Mask1;
            if (rbMask2.Checked) effectLayer = Layers.Mask2;
            if (rbUnmask1.Checked) effectLayer = Layers.Unmask1;
            if (rbUnmask2.Checked) effectLayer = Layers.Unmask2;
            if (rbLayer.Checked) effectLayer = Layers.Layered;

            return effectLayer;
        }

        /// <summary>
        /// Determine if a color is Black, regardless of transparency, or Transparent, regardless of RGB
        /// </summary>
        /// <param name="color">The Color to check</param>
        /// <returns>If the underlying color is black (?,0,0,0) in ARGB or Transparent (0,?,?,?) in ARGB</returns>
        private static bool IsBlackOrTransparent(Color color) {
            var argb = color.ToArgb();
            return (argb & 0xFFFFFF) == 0 || (argb & 0xFF000000) == 0;
        }


        private static Color GetAverageColor(Color color1, Color color2) {
            return Color.FromArgb((color1.A + color2.A) / 255, 
                                  (color1.R + color2.R) / 255,
                                  (color1.G + color2.G) / 255,
                                  (color1.B + color2.B) / 255);
        }

        private void NutcrackerControlDialog_FormClosing(object sender, FormClosingEventArgs e) {
            timerRender.Stop();
            timerRender.Dispose();
        }
    }
}
