using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using NutcrackerEffectsControl;

using VixenPlus;

namespace VixenEditor
{
    public partial class NutcrackerControlDialog: Form {

        #region Class Members and Accessors
        
        private readonly Stopwatch _sw = new Stopwatch();
        private int[] _eventToRender = new[] {0, 0};
        private bool[] _isRendering = new[] {false, false};
        private bool _isPointSelected;
        private NutcrackerEffectControl[] _effectControls;
        private int _rows;
        private int _cols;
        private Color[][,] _buffers;
        private readonly EventSequence _sequence;

        private const int MaxEffects = 2;

        private enum Layers { Effect1, Effect2, Mask1, Mask2, Unmask1, Unmask2, Layered, Average }
        public enum RenderTo { Routine, CurrentPoint, SpecificPoint, Clipboard }

        private Layers EffectLayer { get; set; }

        public RenderTo RenderType { get; private set; } 
        public byte[,] RenderData { get; private set; }

        #endregion

        #region initialization

        public NutcrackerControlDialog(EventSequence sequence) {
            _sequence = sequence;
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
            InitializeFromSequence();
            SetEffectLayer();
        }


        private void InitializeBuffer(int bufferNum) {
            var buffer = _buffers[bufferNum];
            for (var row = 0; row < _rows; row++) {
                for (var col = 0; col < _cols; col++) {
                    buffer[row, col] = Color.Transparent;
                }
            }
        }


        private void InitializeFromSequence() {
            nudStartEvent.Maximum = _sequence.TotalEventPeriods;
            nudEventCount.Maximum = _sequence.TotalEventPeriods - nudStartEvent.Value;
            _isPointSelected = false;
        }


        private void SetupForPlaying() {
            _rows = (int) nudRows.Value;
            _cols = (int) nudColumns.Value;
            if (_rows <= 0 || _cols <= 0) {
                return;
            }

            ResetPreview();
            nudRows.Enabled = false;
            nudColumns.Enabled = false;
            _buffers = new[] {new Color[_rows,_cols], new Color[_rows,_cols]};
            _eventToRender = new[] {0, 0};
            _isRendering = new[] {false, false};
            timerRender.Start();
        }


        private void ResetPreview() {
            using (var g = pbRawPreview.CreateGraphics()) {
                g.Clear(pbRawPreview.BackColor);
            }
        }


        private void TearDownPlaying() {
            timerRender.Stop();
            nudRows.Enabled = true;
            nudColumns.Enabled = true;
        }

        #endregion

        #region Events

        // TODO: Move to resource strings.
        private void btnPlayStop_Click(object sender, EventArgs e) {
            if (btnPlayStop.Text == @"Play") {
                btnPlayStop.Text = @"Stop";
                SetupForPlaying();
            }
            else {
                btnPlayStop.Text = @"Play";
                TearDownPlaying();
            }
        }


        private void timerRender_Tick(object sender, EventArgs e) {
            _sw.Start();
            RenderEffects();
            _sw.Stop();

            var mills = _sw.ElapsedMilliseconds;
            var fps = mills > 0 ? CommonUtils.Utils.MillsPerSecond / (float)mills : 0f;
            lblInfo.Text = string.Format("{0:D3} ms to render, {1:F2} FPS", mills, fps);
            _sw.Reset();
        }


        private void NutcrackerControlDialog_FormClosing(object sender, FormClosingEventArgs e) {
            timerRender.Stop();
            timerRender.Dispose();
        }

        
        private void EffectLayerChanged(object sender, EventArgs e) {
            var rb = sender as RadioButton;
            if (rb != null && rb.Checked) SetEffectLayer();
        }


        private void RenderToChanged(object sender, EventArgs e) {
            var rb = sender as RadioButton;
            if (rb != null && rb.Checked) SetRenderToChanged();
        }

        
        private void RowOrCol_ValueChanged(object sender, EventArgs e) {
            InitializeControls();
        }


        private void btnOK_Click(object sender, EventArgs e) {
            RenderFinalResults();
        }

        #endregion

        #region Rendering Support

        private void RenderEffects() {
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

            if (!isDataUpdated) {
                return;
            }

            if (chkBoxEnableRawPreview.Checked) {
                RenderRawPreview();
            }
            RenderModel();
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


        private void RenderRawPreview() {
            using (var g = pbRawPreview.CreateGraphics()) {
                // Bitmap is width (col) then height (row), we pass data like Vixen+, hight (row) then width (col)
                var bitmap = new Bitmap(_cols, _rows, g);
                for (var row = 0; row < _rows; row++) {
                    for (var column = 0; column < _cols; column++) {
                        var color = GetLayerColor(row, column);
                        bitmap.SetPixel(column, _rows - 1 - row, color == Color.Transparent ? Color.Black : color);
                    }
                }
                g.DrawImage(bitmap, new Point((pbRawPreview.Width - _cols) / 2, (pbRawPreview.Height - _rows) / 2));
            }
        }


        private Color GetLayerColor(int row, int column) {
            var returnValue = Color.Transparent;

            var effect1 = _buffers[0][row, column];
            var effect2 = _buffers[1][row, column];

            switch (EffectLayer) {
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

        private void RenderModel() {

        }

        private void RenderFinalResults() {
            RenderData = new byte[_rows, _cols];
        }



        #endregion

        #region UI Support

        private void SetEffectLayer() {
            if (rbEffect1.Checked) EffectLayer = Layers.Effect1;
            if (rbEffect2.Checked) EffectLayer = Layers.Effect2;
            if (rbMask1.Checked) EffectLayer = Layers.Mask1;
            if (rbMask2.Checked) EffectLayer = Layers.Mask2;
            if (rbUnmask1.Checked) EffectLayer = Layers.Unmask1;
            if (rbUnmask2.Checked) EffectLayer = Layers.Unmask2;
            if (rbLayer.Checked) EffectLayer = Layers.Layered;
            if (rbAverage.Checked) EffectLayer = Layers.Average;
        }


        private void SetRenderToChanged() {
            if (rbRoutine.Checked) RenderType = RenderTo.Routine;
            if (rbCurrentPointOrRange.Checked) RenderType = RenderTo.CurrentPoint;
            if (rbSpecificPoint.Checked) RenderType = RenderTo.SpecificPoint;
            if (rbClipboard.Checked) RenderType = RenderTo.Clipboard;

            UpdateRenderToControls();
        }


        private void UpdateRenderToControls() {
            var startEventVisible = false;
            var eventCountVisible = false;
            
            switch (RenderType) {
                case RenderTo.Routine:
                    eventCountVisible = true;
                    break;
                case RenderTo.CurrentPoint:
                    eventCountVisible = !_isPointSelected;
                    break;
                case RenderTo.SpecificPoint:
                    startEventVisible = true;
                    eventCountVisible = true;
                    break;
                case RenderTo.Clipboard:
                    eventCountVisible = true;
                    break;
            }

            lblStartEvent.Visible = startEventVisible;
            lblStartEventTime.Visible = startEventVisible;
            nudStartEvent.Visible = startEventVisible;

            lblEventCount.Visible = eventCountVisible;
            lblEventCountTime.Visible = eventCountVisible;
            nudEventCount.Visible = eventCountVisible;
        }

        #endregion

    }
}
