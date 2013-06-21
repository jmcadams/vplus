using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using CommonUtils;

using NutcrackerEffectsControl;

using VixenPlus;

namespace VixenEditor
{
    public partial class NutcrackerControlDialog: Form {

        #region Class Members and Accessors

        private enum Layers { Effect1, Effect2, Mask1, Mask2, Unmask1, Unmask2, Layered, Average }

        private bool[] _isRendering = new[] {false, false};
        private int[,] _sparkle;
        private Color[][,] _buffers;
        private int[] _eventToRender = new[] { 0, 0 };
        private NutcrackerEffectControl[] _effectControls;

        private int _rows;
        private int _cols;
        private int _lastGroupSelected;
        private int _lastChannelSelected;
        private string _playText;
        private const string StopText = "Stop";

        private readonly Random _random = new Random();
        private readonly Rectangle _selectedRange;
        private readonly EventSequence _sequence;
        private readonly Stopwatch _sw = new Stopwatch();

        private const int MaxEffects = 2;

        private Layers EffectLayer { get; set; }

        public enum RenderTo { Routine, CurrentSelection, SpecificPoint, Clipboard }

        public RenderTo RenderType { get; private set; } 
        public byte[,] RenderData { get; private set; }

        #endregion

        #region initialization

        public NutcrackerControlDialog(EventSequence sequence, Rectangle selectedRange) {
            _sequence = sequence;
            _selectedRange = selectedRange;
            InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls() {
            _playText = btnPlayStop.Text;
            cbColorLayout.SelectedIndex = 0;
            _rows = (int)nudRows.Value;
            _cols = (int)nudColumns.Value;
            _buffers = new[] { new Color[_rows, _cols], new Color[_rows, _cols] };
            _effectControls = new[] { nutcrackerEffectControl1, nutcrackerEffectControl2 };
            InitializeBuffer(0);
            InitializeBuffer(1);
            InitializeSparkle();
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


        private void InitializeSparkle() {
            _sparkle = new int[_rows, _cols];
            for (var row = 0; row < _rows; row++) {
                for (var col = 0; col < _cols; col++) {
                    _sparkle[row, col] = _random.Next() % 5000;
                }
            } 
        }

        
        private void InitializeFromSequence() {
            nudStartEvent.Maximum = _sequence.TotalEventPeriods;
            nudStartEvent.Value = _selectedRange.Left;
            nudEventCount.Maximum = _sequence.TotalEventPeriods - nudStartEvent.Value;
            if (_selectedRange.Width > 0) {
                nudEventCount.Value = _selectedRange.Width;
                rbCurrentSelection.Checked = true;
            }
            else {
                rbCurrentSelection.Enabled = false;
            }
            
            UpdateRenderToControls();
            UpdateSummary();
            LoadGroups();
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
            if (btnPlayStop.Text == _playText) {
                btnPlayStop.Text = StopText;
                SetupForPlaying();
            }
            else {
                btnPlayStop.Text = _playText;
                TearDownPlaying();
            }
        }


        private void timerRender_Tick(object sender, EventArgs e) {
            _sw.Start();
            RenderEffects();
            _sw.Stop();

            var mills = _sw.ElapsedMilliseconds;
            lblStatsMs.Text = String.Format("{0} ms", mills);
            lblStatsFps.Text = string.Format(@"{0:F2} FPS", mills > 0 ? Utils.MillsPerSecond / (float)mills : 0f);
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


        private void nudEventCount_ValueChanged(object sender, EventArgs e) {
            UpdateSummary();
        }


        private void nudStartEvent_ValueChanged(object sender, EventArgs e) {
            nudEventCount.Maximum = _sequence.TotalEventPeriods - nudStartEvent.Value + 1;
            UpdateSummary();
        }


        private void chkBoxUseGroup_CheckedChanged(object sender, EventArgs e) {
            if (chkBoxUseGroup.Checked) {
                LoadGroups();
            }
            else {
                LoadChannels();
            }
        }


        private void cbGroups_SelectedIndexChanged(object sender, EventArgs e) {
            if (chkBoxUseGroup.Checked) {
                _lastGroupSelected = cbGroups.SelectedIndex;
            }
            else {
                _lastChannelSelected = cbGroups.SelectedIndex;
            }
        }


        private void cbGroups_DrawItem(object sender, DrawItemEventArgs e) {
            if (e.Index < 0) return;

            if (chkBoxUseGroup.Checked) {
                var indexedItem = _sequence.Groups[cbGroups.Items[e.Index].ToString()];
                Utils.DrawItem(e, indexedItem.Name, indexedItem.GroupColor, true);
            }
            else {
                var indexedItem = _sequence.FullChannels[e.Index];
                Utils.DrawItem(e, indexedItem.Name, indexedItem.Color, true);
            }
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

            if (tbSparkles.Value > 0 && !IsBlackOrTransparent(returnValue)) {
                switch (_sparkle[row,column]++ % (tbSparkles.Maximum - tbSparkles.Value + 20))
                {
                    case 2:
                    case 6:
                        returnValue = Color.FromArgb(136,136,136);
                        break;
                    case 3:
                    case 5:
                        returnValue = Color.FromArgb(187,187,187);
                        break;
                    case 4:
                        returnValue = Color.White;
                        break;
                }
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
            if (rbCurrentSelection.Checked) RenderType = RenderTo.CurrentSelection;
            if (rbSpecificPoint.Checked) RenderType = RenderTo.SpecificPoint;
            if (rbClipboard.Checked) RenderType = RenderTo.Clipboard;

            UpdateRenderToControls();
        }


        private void UpdateRenderToControls() {
            var startEventVisible = false;
            var eventCountVisible = false;
            var groupsAndChannelsVisbile = false;

            switch (RenderType) {
                case RenderTo.Routine:
                    nudStartEvent.Value = 0;
                    eventCountVisible = true;
                    break;
                case RenderTo.CurrentSelection:
                    break;
                case RenderTo.SpecificPoint:
                    startEventVisible = true;
                    eventCountVisible = true;
                    groupsAndChannelsVisbile = true;
                    break;
                case RenderTo.Clipboard:
                    nudStartEvent.Value = 0;
                    eventCountVisible = true;
                    break;
            }

            lblStartEvent.Visible = startEventVisible;
            lblStartEventTime.Visible = startEventVisible;
            nudStartEvent.Visible = startEventVisible;

            lblEventCount.Visible = eventCountVisible;
            lblEventCountTime.Visible = eventCountVisible;
            nudEventCount.Visible = eventCountVisible;

            chkBoxUseGroup.Visible = groupsAndChannelsVisbile;
            cbGroups.Visible = groupsAndChannelsVisbile;
        }


        private void UpdateSummary() {
            var eventPeriod = _sequence.EventPeriod;
            var startTime = Utils.TimeFormatWithMills((int)(nudStartEvent.Value * eventPeriod));
            var elapsedTime = Utils.TimeFormatWithMills((int)(nudEventCount.Value * eventPeriod));
            var endTime = Utils.TimeFormatWithMills((int)(nudStartEvent.Value + nudEventCount.Value - 1) * eventPeriod);

            lblStartEventTime.Text = startTime;
            lblEventCountTime.Text = elapsedTime;

            tbSummary.Text = String.Format("From {0} thru {1} on {2}", startTime, endTime, String.Empty);
        }


        private void LoadGroups() {
            cbGroups.Items.Clear();
            foreach (var g in _sequence.Groups) {
                cbGroups.Items.Add(g.Key);
            }
            cbGroups.SelectedIndex = _lastGroupSelected;
        }


        private void LoadChannels() {
            cbGroups.Items.Clear();
            foreach (var c in _sequence.FullChannels) {
                cbGroups.Items.Add(c);
            }
            cbGroups.SelectedIndex = _lastChannelSelected;
        }

        #endregion

    }
}
