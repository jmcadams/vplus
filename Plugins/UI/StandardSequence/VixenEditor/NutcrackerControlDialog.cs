using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using CommonUtils;

using NutcrackerEffectsControl;

using VixenPlus;

namespace VixenEditor {
    public partial class NutcrackerControlDialog : Form {

        #region Class Members and Accessors

        private enum Layers {
            Effect1,
            Effect2,
            Mask1,
            Mask2,
            Unmask1,
            Unmask2,
            Layered,
            Average
        }

        private bool[] _isRendering = new[] {false, false};
        private NutcrackerNodes[,] _nodes;
        private Color[][,] _effectBuffers;
        private int[] _eventToRender = new[] {0, 0};
        private NutcrackerEffectControl[] _effectControls;

        private string _playText;
        private const string StopText = "Stop";

        private readonly Random _random = new Random();
        private readonly Rectangle _selectedRange;
        private readonly EventSequence _sequence;
        private readonly Stopwatch _sw = new Stopwatch();

        private const int MaxEffects = 2;

        private Layers EffectLayer { get; set; }

        public enum RenderTo {
            Routine,
            CurrentSelection,
            SpecificPoint,
            Clipboard
        }

        public RenderTo RenderType { get; private set; }
        public byte[,] RenderData { get; private set; }
        public int RenderEvents { get; private set; }

        public int RenderRows { get; private set; }

        public int RenderCols { get; private set; }

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
            RenderRows = (int) nudRows.Value;
            RenderCols = (int) nudColumns.Value;
            _nodes = new NutcrackerNodes[RenderRows,RenderCols];
            _effectBuffers = new[] {new Color[RenderRows,RenderCols], new Color[RenderRows,RenderCols]};
            _effectControls = new[] {nutcrackerEffectControl1, nutcrackerEffectControl2};
            InitializeEffectBuffer(0);
            InitializeEffectBuffer(1);
            InitializeNodes();
            InitializeFromSequence();
            InitMatrix();
            InitializeModels();
            SetEffectLayer();
        }


        private void InitializeEffectBuffer(int bufferNum) {
            var effectBuffer = _effectBuffers[bufferNum];
            for (var row = 0; row < RenderRows; row++) {
                for (var col = 0; col < RenderCols; col++) {
                    effectBuffer[row, col] = Color.Black;
                }
            }
        }


        private void InitializeNodes() {
            for (var row = 0; row < RenderRows; row++) {
                for (var col = 0; col < RenderCols; col++) {
                    _nodes[row, col] = new NutcrackerNodes
                    {PixelColor = Color.Black, Sparkle = _random.Next() % 5000, Model = new Point(col, RenderRows - 1 - row)};
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
        }


        private void InitializeModels() {
            cbModels.Items.Clear();
            var data = new NutcrackerXmlManager();
            var models = data.GetModels();
            foreach (var nameAttr in models.Select(m => m.Attribute("name")).Where(name => name != null)) {
                cbModels.Items.Add(nameAttr.Value);
            }
            cbModels.Items.Add("Manage Models");

            var degrees = 180;
            if (RenderCols < 2) return;
            var factor = pbPreview.Height / RenderRows;
            var renderWi = pbPreview.Width / 2;
            var radians = 2.0 * Math.PI * degrees / 360.0;
            var radius = renderWi * 0.8;
            var startAngle = -radians / 2.0;
            var angleIncr = radians / (RenderCols - 1);
            for (var row = 0; row < RenderRows; row++) {
                for (var col = 0; col < RenderCols; col++) {
                    var angle = startAngle + _nodes[RenderRows - 1 - row, col].BufX * angleIncr;
                    var x0 = radius * Math.Sin(angle);
                    var x = (int) Math.Floor(x0 * (1.0 - (double) (_nodes[row, col].BufY) / RenderRows) + 0.5) + renderWi;
                    var y = _nodes[RenderRows - 1 - row, col].BufY * factor;
                    _nodes[RenderRows - 1 - row, col].Model = new Point(x, y);
                }
            }
        }


        private void InitMatrix() {
            var stringCount = RenderCols; // 32
            var nodesPerString = RenderRows; // 50
            var strandsPerString = 1;
            var IsLtoR = true;

            var numStrands = stringCount * strandsPerString; // 64
            var pixelsPerStrand = nodesPerString / strandsPerString; // 25
            var index = 0;
            for (var strand = 0; strand < numStrands; strand++) {
                var segmentnum = strand % strandsPerString;
                for (var pixel = 0; pixel < pixelsPerStrand; pixel++) {
                    var y = index % RenderRows;
                    var x = index / RenderRows;
                    _nodes[y, x].BufX = IsLtoR ? strand : numStrands - strand - 1;
                    _nodes[y, x].BufY = (segmentnum % 2 != 0) ? pixel : pixelsPerStrand - pixel - 1;
                    index++;
                }
            }
        }


        private void SetupForPlaying() {
            RenderRows = (int) nudRows.Value;
            RenderCols = (int) nudColumns.Value;
            if (RenderRows <= 0 || RenderCols <= 0) {
                return;
            }

            ResetPreview();
            nudRows.Enabled = false;
            nudColumns.Enabled = false;
            _effectBuffers = new[] {new Color[RenderRows,RenderCols], new Color[RenderRows,RenderCols]};
            _eventToRender = new[] {0, 0};
            _isRendering = new[] {false, false};
            timerRender.Start();
        }


        private void ResetPreview() {
            using (var g = pbRawPreview.CreateGraphics()) {
                g.Clear(pbRawPreview.BackColor);
            }

            using (var g = pbPreview.CreateGraphics()) {
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

        public void ControlChanged1(object sender, EventArgs e) {
            _eventToRender[0] = 0;
        }


        private void ControlChanged2(object sender, EventArgs e) {
            _eventToRender[1] = 0;
        }


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
            Render();
            _sw.Stop();

            var mills = _sw.ElapsedMilliseconds;
            lblStatsMs.Text = String.Format("{0} ms", mills);
            lblStatsFps.Text = string.Format(@"{0:F2} FPS", mills > 0 ? Utils.MillsPerSecond / (float) mills : 0f);
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


        private void cbModels_SelectedIndexChanged(object sender, EventArgs e) {
            if (cbModels.SelectedIndex != cbModels.Items.Count - 1) {
                return;
            }
            using (var modelDialog = new NutcrackerModelDialog(_sequence)) {
                modelDialog.ShowDialog();
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
                    InitializeEffectBuffer(i);
                }

                if (effects[i] == null) {
                    _isRendering[i] = false;
                    continue;
                }

                isDataUpdated = true;
                _effectBuffers[i] = effects[i].RenderEffect(_effectBuffers[i], _effectControls[i].GetPalette(), _eventToRender[i]);

                _eventToRender[i] += _effectControls[i].GetSpeed();
                _isRendering[i] = false;
            }

            if (!isDataUpdated) {
                return;
            }

            SetPixelColors();
        }


        private List<INutcrackerEffect> GetEffects() {
            var effects = new List<INutcrackerEffect>(MaxEffects);
            for (var i = 0; i < MaxEffects; i++) {
                if (_isRendering[i]) {
                    effects.Add(null);
                    continue;
                }
                effects.Add((INutcrackerEffect) _effectControls[i].GetCurrentEffectControl());
            }
            return effects;
        }


        private void SetPixelColors() {
            for (var row = 0; row < RenderRows; row++) {
                for (var column = 0; column < RenderCols; column++) {
                    _nodes[row, column].PixelColor = GetLayerColor(row, column);
                }
            }
        }


        private void Render() {
            using (var raw = pbRawPreview.CreateGraphics())
            using (var preview = pbPreview.CreateGraphics()) {
                var rawBitmap = new Bitmap(RenderCols, RenderRows, raw);
                var previewBitmap = new Bitmap(pbPreview.Width, pbPreview.Height, preview);
                for (var row = 0; row < RenderRows; row++) {
                    for (var column = 0; column < RenderCols; column++) {
                        var node = _nodes[row, column];
                        rawBitmap.SetPixel(column, RenderRows - 1 - row, node.PixelColor);
                        previewBitmap.SetPixel(node.Model.X, node.Model.Y, node.PixelColor);
                    }
                }
                if (chkBoxEnableRawPreview.Checked) {
                    raw.DrawImage(rawBitmap, new Point((pbRawPreview.Width - RenderCols) / 2, (pbRawPreview.Height - RenderRows) / 2));
                }
                preview.DrawImage(previewBitmap, 0, 0);
            }
        }


        private Color GetLayerColor(int row, int column) {
            var returnValue = Color.Black;

            var effect1 = _effectBuffers[0][row, column];
            var effect2 = _effectBuffers[1][row, column];

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
                switch (_nodes[row, column].Sparkle++ % (tbSparkles.Maximum - tbSparkles.Value + 20)) {
                    case 2:
                    case 6:
                        returnValue = Color.FromArgb(136, 136, 136);
                        break;
                    case 3:
                    case 5:
                        returnValue = Color.FromArgb(187, 187, 187);
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
            return Color.FromArgb((color1.A + color2.A) / 255, (color1.R + color2.R) / 255, (color1.G + color2.G) / 255, (color1.B + color2.B) / 255);
        }


        private void RenderFinalResults() {
            // Trees and matrix render rows, cols the other render as 1 x num pixles x (arch count | 1 if windows)
            RenderEvents = (int) nudEventCount.Value;
            RenderData = new byte[RenderCols * RenderRows * 3,RenderEvents];
            for (var i = 0; i < MaxEffects; i++) {
                _eventToRender[i] = 0;
            }

            progressBar.Minimum = 0;
            progressBar.Maximum = RenderEvents;
            progressBar.Visible = true;

            for (var renderEvent = 0; renderEvent < RenderEvents; renderEvent++) {

                RenderEffects();
                var nodeRow = 0;
                var eventRow = 0;
                for (var row = 0; row < RenderRows * 3; row += 3) {
                    for (var col = 0; col < RenderCols; col++) {
                        RenderData[eventRow, renderEvent] = _nodes[nodeRow, col].PixelColor.R;
                        RenderData[eventRow + 1, renderEvent] = _nodes[nodeRow, col].PixelColor.B;
                        RenderData[eventRow + 2, renderEvent] = _nodes[nodeRow, col].PixelColor.G;
                        eventRow += 3;
                    }
                    nodeRow++;
                }

                for (var i = 0; i < MaxEffects; i++) {
                    _eventToRender[i] += _effectControls[i].GetSpeed();
                }
                progressBar.Value = renderEvent;
                progressBar.Text = string.Format("{0:d3}%", renderEvent / RenderEvents);
                Application.DoEvents();
            }
            progressBar.Visible = false;
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

            switch (RenderType) {
                case RenderTo.Clipboard:
                case RenderTo.Routine:
                    nudStartEvent.Value = 0;
                    eventCountVisible = true;
                    break;
                case RenderTo.CurrentSelection:
                    break;
                case RenderTo.SpecificPoint:
                    startEventVisible = true;
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


        private void UpdateSummary() {
            var eventPeriod = _sequence.EventPeriod;
            var startTime = Utils.TimeFormatWithMills((int) (nudStartEvent.Value * eventPeriod));
            var elapsedTime = Utils.TimeFormatWithMills((int) (nudEventCount.Value * eventPeriod));
            var endTime = Utils.TimeFormatWithMills((int) (nudStartEvent.Value + nudEventCount.Value - 1) * eventPeriod);

            lblStartEventTime.Text = startTime;
            lblEventCountTime.Text = elapsedTime;

            var channelCount = RenderRows * RenderCols * 3;
            var msg = (channelCount > _sequence.FullChannelCount) ? "(Too Large for your current channel count)" : string.Empty;

            tbSummary.Text = String.Format("From {0} thru {1} on {2} channels {3}", startTime, endTime, channelCount, msg);
        }

        #endregion
    }
}
