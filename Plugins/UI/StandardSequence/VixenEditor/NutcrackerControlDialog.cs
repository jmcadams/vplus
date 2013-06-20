using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
        private NutcrackerEffectControl[] _effectControls;
        private int _rows;
        private int _cols;
        private Color[][,] _buffers;
        private Layers _effectLayer = Layers.Effect1;
        private const int MaxEffects = 2;

        private enum Layers { Effect1, Effect2, Mask1, Mask2, Unmask1, Unmask2, Layered, Average }

        public NutcrackerControlDialog()
        {   
            InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls() {
            _rows = (int)nudRows.Value;
            _cols = (int)nudColumns.Value;
            _buffers = new[] { new Color[_rows, _cols], new Color[_rows, _cols] };
            _effectControls = new[] { nutcrackerEffectControl1, nutcrackerEffectControl2 };
            InitializeBuffer(0);
            InitializeBuffer(1);
        }


        // TODO: Move to resource strings.
        private void btnPlayStop_Click(object sender, EventArgs e) {
            if (btnPlayStop.Text == @"Play") {
                _rows = (int)nudRows.Value;
                _cols = (int)nudColumns.Value;
                if (_rows > 0 && _cols > 0) {
                    ResetPreview();
                    btnPlayStop.Text = @"Stop";
                    nudRows.Enabled = false;
                    nudColumns.Enabled = false;
                    _buffers = new[] { new Color[_rows, _cols], new Color[_rows, _cols] };
                    _eventToRender = new[] { 0, 0 };
                    _isRendering = new[] { false, false };
                    timerRender.Start();
                }
            }
            else {
                btnPlayStop.Text = @"Play";
                nudRows.Enabled = true;
                nudColumns.Enabled = true;
                timerRender.Stop();
            }
        }


        private void ResetPreview() {
            using (var g = pbRawPreview.CreateGraphics()) {
                g.Clear(pbRawPreview.BackColor);
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
            
            var mills = _sw.ElapsedMilliseconds;
            var fps = mills > 0 ? CommonUtils.Utils.MillsPerSecond / (float)mills : 0f;
            lblInfo.Text = string.Format("{0:D3} ms to render, {1:F2} FPS", mills, fps);
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
            if (!chkBoxEnableRawPreview.Checked) return;

            using (var g = pbRawPreview.CreateGraphics()) {
                // Bitmap is width (col) then height (row), we pass data like Vixen+, hight (row) then width (col)
                var bitmap = new Bitmap(_cols, _rows, g);
                for (var row = 0; row < _rows; row++) {
                    for (var column = 0; column < _cols; column++) {
                        var color = GetLayerColor(row, column);
                        bitmap.SetPixel(column, _rows - 1 - row, color == Color.Transparent ? Color.Black : color);
                    }
                }
                g.DrawImage(bitmap, new Point((pbRawPreview.Width - _cols)/2, (pbRawPreview.Height - _rows) / 2));
            }
        }


        private Color GetLayerColor(int row, int column) {
            var returnValue = Color.Transparent;

            var effect1 = _buffers[0][row, column];
            var effect2 = _buffers[1][row, column];

            switch (_effectLayer) {
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

        private void EffectLayerChanged(object sender, EventArgs e) {
            var rb = sender as RadioButton;
            if (rb != null && rb.Checked) SetEffectLayer();
        }


        private void SetEffectLayer() {
            if (rbEffect1.Checked) _effectLayer = Layers.Effect1;
            if (rbEffect2.Checked) _effectLayer = Layers.Effect2;
            if (rbMask1.Checked) _effectLayer = Layers.Mask1;
            if (rbMask2.Checked) _effectLayer = Layers.Mask2;
            if (rbUnmask1.Checked) _effectLayer = Layers.Unmask1;
            if (rbUnmask2.Checked) _effectLayer = Layers.Unmask2;
            if (rbLayer.Checked) _effectLayer = Layers.Layered;
            if (rbAverage.Checked) _effectLayer = Layers.Average;
        }

        private void RowOrCol_ValueChanged(object sender, EventArgs e) {
            InitializeControls();
        }

    }
}
