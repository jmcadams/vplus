using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;



using NutcrackerEffectsControl;

using VixenPlus;
using VixenPlus.Dialogs;

using VixenPlusCommon;

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

        private bool[] _isRendering = {false, false};
        private NutcrackerNodes[,] _nodes;
        private Color[][,] _effectBuffers;
        private int[] _eventToRender = {0, 0};
        private NutcrackerEffectControl[] _effectControls;
        private NutcrackerXmlManager _nutcrackerData;
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

        public RenderTo RenderType { get; set; }
        public byte[,] RenderData { get; set; }
        public int RenderEvents { get; set; }
        public int RenderRows { get; set; }
        public int RenderCols { get; set; }

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
            _nutcrackerData = new NutcrackerXmlManager();

            InitializeEffectBuffer(0);
            InitializeEffectBuffer(1);
            InitializeNodes();
            InitializeFromSequence();
            InitMatrix();
            InitializeModels();
            InitializePresets();
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
            var models = _nutcrackerData.GetModels();
            foreach (var nameAttr in models.Select(m => m.Attribute("name")).Where(name => name != null)) {
                cbModels.Items.Add(nameAttr.Value);
            }
            cbModels.Items.Add("Manage Models");

            const int degrees = 180;
            if (RenderCols < 2) return;
            var factor = pbPreview.Height / RenderRows;
            var renderWi = pbPreview.Width / 2;
            const double radians = 2.0 * Math.PI * degrees / 360.0;
            var radius = renderWi * 0.8;
            const double startAngle = -radians / 2.0;
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


        private void InitializePresets() {
            cbEffectsPresets.Items.Clear();
            var effects = _nutcrackerData.GetPresets();
            foreach (var nameAttr in effects.Select(m => m.Attribute("name")).Where(name => name != null)) {
                cbEffectsPresets.Items.Add(nameAttr.Value);
            }
            cbEffectsPresets.Items.Add("Save current settings...");
            cbEffectsPresets.Items.Add("Manage Effect Presets...");
        }


        private void InitMatrix() {
            var stringCount = RenderCols; // 32
            var nodesPerString = RenderRows; // 50
            const int strandsPerString = 1;

            var numStrands = stringCount * strandsPerString; // 64
            var pixelsPerStrand = nodesPerString / strandsPerString; // 25
            var index = 0;
            for (var strand = 0; strand < numStrands; strand++) {
                var segmentnum = strand % strandsPerString;
                for (var pixel = 0; pixel < pixelsPerStrand; pixel++) {
                    var y = index % RenderRows;
                    var x = index / RenderRows;
                    _nodes[y, x].BufX = strand;
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

        private void ControlChanged1(object sender, EventArgs e) {
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

                _eventToRender[i] += _effectControls[i].Speed;
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

            if (tbSparkles.Value <= 0 || IsBlackOrTransparent(returnValue)) {
                return returnValue;
            }
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
                    _eventToRender[i] += _effectControls[i].Speed;
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
            var startTime = ((int) (nudStartEvent.Value * eventPeriod)).FormatFull();
            var elapsedTime = ((int) (nudEventCount.Value * eventPeriod)).FormatFull();
            var endTime = ((int) (nudStartEvent.Value + nudEventCount.Value - 1) * eventPeriod).FormatFull();

            lblStartEventTime.Text = startTime;
            lblEventCountTime.Text = elapsedTime;

            var channelCount = RenderRows * RenderCols * 3;
            var msg = (channelCount > _sequence.FullChannelCount) ? "(Too Large for your current channel count)" : string.Empty;

            tbSummary.Text = String.Format("From {0} thru {1} on {2} channels {3}", startTime, endTime, channelCount, msg);
        }

        #endregion

        //private void btnManagePresets_Click(object sender, EventArgs e) {}


        private void SetPreset(string effectData) {
            var settings = effectData.Split(new[] {','}).ToList();

            // Strip out first 4 settings
            var effect1Name = settings[0];
            var effect2Name = settings[1];
            SetLayeringMethod(settings[2]);
            SetSparkleFrequency(settings[3].Split(new[] {'='})[1]);

            for (var i = 0; i < 4; i++) {
                settings.RemoveAt(0);
            }

            // Pass the remainder to the effects, let them take what they need and ignore the rest.
            nutcrackerEffectControl1.SetEffect(effect1Name, settings, true);
            nutcrackerEffectControl2.SetEffect(effect2Name, settings, false);
        }


        private void SetLayeringMethod(string method) {
            switch (method) {
                case "Effect 1":
                    rbEffect1.Checked = true;
                    break;
                case "Effect 2":
                    rbEffect2.Checked = true;
                    break;
                case "1 is Mask":
                    rbMask1.Checked = true;
                    break;
                case "2 is Mask":
                    rbMask2.Checked = true;
                    break;
                case "1 is Unmask":
                    rbUnmask1.Checked = true;
                    break;
                case "2 is Unmask":
                    rbUnmask2.Checked = true;
                    break;
                case "Layered":
                    rbLayer.Checked = true;
                    break;
                case "Average":
                    rbAverage.Checked = true;
                    break;
            }
        }


        private void SetSparkleFrequency(string level) {
            int frequency;
            tbSparkles.Value = (Int32.TryParse(level, out frequency) ? 100 - (frequency / 2) : 0);
        }


        private void cbEffectsPresets_SelectedIndexChanged(object sender, EventArgs e) {
            if (cbEffectsPresets.SelectedIndex < cbEffectsPresets.Items.Count - 2) {
                var effectData = _nutcrackerData.GetDataForEffect(cbEffectsPresets.SelectedItem.ToString());
                if (effectData != null) {
                    SetPreset(effectData);
                }
                return;
            }
            if (cbEffectsPresets.SelectedIndex == cbEffectsPresets.Items.Count - 1) {
                using (var modelDialog = new NutcrackerModelDialog(_sequence)) {
                    modelDialog.ShowDialog();
                }
            }
            else if (cbEffectsPresets.SelectedIndex == cbEffectsPresets.Items.Count - 2) {
                var isValid = false;
                while (!isValid)
                    using (var textDialog = new TextQueryDialog("Preset name", "What do you want to name this preset?", "")) {
                        if (textDialog.ShowDialog() != DialogResult.OK) {
                            return;
                        }
                        var presetName = textDialog.Response;
                        if (cbEffectsPresets.Items.Contains(presetName)) {
                            continue;
                        }
                        isValid = true;
                        SavePreset(presetName);
                    }
            }
        }


        private void SavePreset(string presetName) {
            var settings = new List<string>();
            for (var i = 0; i < MaxEffects; i++) {
                if (_effectControls[i] != null) {
                    settings.Add(_effectControls[i].Name);
                }
            }
            var layerEffects = string.Empty;

            if (rbEffect1.Checked) {
                layerEffects = "Effect 1";
            }
            else if (rbEffect2.Checked) {
                layerEffects = "Effect 2";
            }
            else if (rbMask1.Checked) {
                layerEffects = "1 is Mask";
            }
            else if (rbMask2.Checked) {
                layerEffects = "2 is Mask";
            }
            else if (rbUnmask1.Checked) {
                layerEffects = "1 is Unmask";
            }
            else if (rbUnmask2.Checked) {
                layerEffects = "2 is Unmask";
            }
            if (rbAverage.Checked) {
                layerEffects = "Average";
            }
            else if (rbLayer.Checked) {
                layerEffects = "Layered";
            }

            var sparkleFrequency = "ID_SLIDER_SparkleFrequency=" + tbSparkles.Value * 2;

            settings.Add(layerEffects);
            settings.Add(sparkleFrequency);
            settings.Add(_effectControls[0].GetSpeed(true));
            settings.Add(_effectControls[1].GetSpeed(false));

            settings.Add(_effectControls[0].GetEffect(true));
            settings.Add(_effectControls[1].GetEffect(false));

            settings.Add(_effectControls[0].GetPaletteSettings(true));
            settings.Add(_effectControls[1].GetPaletteSettings(false));

            var settingString = new StringBuilder();
            foreach (var s in settings) {
                settingString.Append(s).Append(",");
            }
            settingString.Remove(settingString.Length - 1, 1);

            _nutcrackerData.AddPreset(new XElement("effect", new XAttribute("name", presetName), new XAttribute("settings", settingString)));
        }
    }
}