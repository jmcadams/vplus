using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

using Nutcracker.Effects;

using VixenPlus;

using VixenPlusCommon;

namespace Nutcracker {
    public enum RenderTo {
        Routine,
        CurrentSelection,
        SpecificPoint,
        Clipboard
    }

    public sealed partial class NutcrackerControlDialog : Form {

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
        private string _colorSpace;
        private const string StopText = "Stop";

        //private readonly Random _random = new Random();
        private readonly Rectangle _selectedRange;
        private readonly EventSequence _sequence;
        private readonly Stopwatch _sw = new Stopwatch();
        private Rectangle _previewRectangle;
        private const int MaxEffects = 2;
        private readonly List<Channel> _channels; 

        private Layers EffectLayer { get; set; }

        //public enum RenderTo {
        //    Routine,
        //    CurrentSelection,
        //    SpecificPoint,
        //    Clipboard
        //}

        public RenderTo RenderType { get; private set; }
        public byte[,] RenderEventData { get; private set; }
        public int RenderEventCount { get; private set; }
        public int RenderRows { get; private set; }
        public int RenderCols { get; private set; }

        public int RenderChannel {
            get { return cbChannels.SelectedIndex; }
        }

        public int RenderStartEvent {
            get { return (int) nudStartEvent.Value; }
        }

        #endregion

        #region initialization

        public NutcrackerControlDialog(EventSequence sequence, Rectangle selectedRange, bool constrainToGroup) {
            _sequence = sequence;
            _selectedRange = selectedRange;

            _channels = constrainToGroup ? _sequence.Channels : _sequence.FullChannels;
            InitializeComponent();
            MaximumSize = Size;
            MinimumSize = Size;
            InitializeControls();
        }


        private void InitializeControls() {
            _playText = btnPlayStop.Text;
            RenderRows = 0;
            RenderCols = 0;
            _previewRectangle = new Rectangle(0, 0, pbPreview.Size.Width / 2, pbPreview.Height / 2);
            _nodes = new NutcrackerNodes[RenderRows,RenderCols];
            _effectBuffers = new[] {new Color[RenderRows,RenderCols], new Color[RenderRows,RenderCols]};
            _effectControls = new[] {nutcrackerEffectControl1, nutcrackerEffectControl2};
            _nutcrackerData = new NutcrackerXmlManager();

            InitializeEffectBuffer(0);
            InitializeEffectBuffer(1);
            InitializeFromSequence();
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


        private void InitializeFromSequence() {
            nudStartEvent.Maximum = _sequence.TotalEventPeriods;
            nudStartEvent.Value = _selectedRange.Left;
            nudEventCount.Maximum = _sequence.TotalEventPeriods - nudStartEvent.Value;

            foreach (var c in _channels) {
                cbChannels.Items.Add(c);
            }
            
            if (_selectedRange.IsEmpty) {
                rbCurrentSelection.Enabled = false;

            }
            else {
                cbChannels.SelectedIndex = cbChannels.FindStringExact(_channels[_selectedRange.Y].Name);
                nudEventCount.Value = _selectedRange.Width;
                rbCurrentSelection.Checked = true;
            }

            UpdateRenderToControls();
            UpdateSummary();
        }


        private void InitializeModels() {
            cbModels.Items.Clear();
            btnModelRemove.Visible = false;
            btnModelEdit.Visible = false;
            foreach (var nameAttr in Directory.GetFiles(Paths.NutcrackerDataPath, Vendor.All + Vendor.NutcrakerModelExtension)) {
                // ReSharper disable AssignNullToNotNullAttribute
                cbModels.Items.Add(Path.GetFileNameWithoutExtension(nameAttr));
                // ReSharper restore AssignNullToNotNullAttribute
            }
            cbModels.Items.Add("Add a New Model");
        }


        private void InitializePresets() {
            cbEffectsPresets.Items.Clear();
            var effects = _nutcrackerData.GetPresets();
            foreach (var nameAttr in effects.Select(m => m.Attribute("name")).Where(name => name != null)) {
                cbEffectsPresets.Items.Add(nameAttr.Value);
            }
            cbEffectsPresets.Items.Add("Save current settings...");
        }

        private void SetupForPlaying() {
            if (RenderRows <= 0 || RenderCols <= 0) {
                return;
            }

            ResetPreview();
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
            try {
                _sw.Start();
                RenderEffects();
                Render();
                _sw.Stop();

                var mills = _sw.ElapsedMilliseconds;
                lblStatsMs.Text = String.Format("{0} ms", mills);
                lblStatsFps.Text = string.Format(@"{0:F2} FPS", mills > 0 ? Utils.MillsPerSecond / (float) mills : 0f);
                _sw.Reset();
            }
            catch (NutcrackerException ex) {
                btnPlayStop_Click(null, null);
                ex.InformException();
            }
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


        private bool _ignoreIndexChange;

        private void cbModels_SelectedIndexChanged(object sender, EventArgs e) {
            if (_ignoreIndexChange) return;

            btnModelRemove.Visible = false;
            btnModelEdit.Visible = true;
            if (cbModels.SelectedIndex == -1) {
                return;
            }

            var selected = string.Empty;
            if (cbModels.SelectedIndex != cbModels.Items.Count - 1) {
                btnModelRemove.Visible = true;
                btnModelEdit.Visible = true;
                EditModel(GetFilePath(), false);
                return;
            }

            EditModel(selected);
        }


        private void EditModel(string selected, bool show = true) {
            using (var modelDialog = new NutcrackerModelDialog(selected)) {
                if (show) {
                    modelDialog.ShowDialog();
                    _ignoreIndexChange = true;
                    InitializeModels();
                    cbModels.SelectedIndex = cbModels.FindStringExact(modelDialog.ModelName);
                    _ignoreIndexChange = false;
                }
                modelDialog.PreviewRectangle = _previewRectangle;
                RenderRows = modelDialog.Rows;
                RenderCols = modelDialog.Cols;
                _effectBuffers = new[] { new Color[RenderRows, RenderCols], new Color[RenderRows, RenderCols] };
                _nodes = modelDialog.Nodes;
                _colorSpace = modelDialog.ColorLayout;
                btnModelEdit.Visible = true;
                btnModelRemove.Visible = true;
            }
            UpdateSummary();
   
        }

        #endregion

        #region Rendering Support

        private void RenderEffects() {
            try {
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
                    _effectBuffers[i] = effects[i].RenderEffect(_effectBuffers[i], _effectControls[i].GetPalette(),
                        _eventToRender[i]);

                    _eventToRender[i] += _effectControls[i].Speed;
                    _isRendering[i] = false;
                }

                if (!isDataUpdated) {
                    return;
                }

                SetPixelColors();
            }
            catch (Exception ex) {
                throw new NutcrackerException("Nutcracker Error in Rendering (see log for details)", ex);
            }
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
                var previewBitmap = new Bitmap(_previewRectangle.Width, _previewRectangle.Height, preview);
                for (var row = 0; row < RenderRows; row++) {
                    for (var column = 0; column < RenderCols; column++) {
                        var node = _nodes[row, column];
                        if (node.Model.X < 0 || node.Model.Y < 0) {
                            continue;
                        }
                        rawBitmap.SetPixel(column, RenderRows - 1 - row, node.PixelColor);
                        previewBitmap.SetPixel(node.Model.X, node.Model.Y, node.PixelColor);
                    }
                }
                if (chkBoxEnableRawPreview.Checked) {
                    raw.DrawImage(rawBitmap, new Point((pbRawPreview.Width - RenderCols) / 2, (pbRawPreview.Height - RenderRows) / 2));
                }
                using (previewBitmap) {
                    var scaled = new Bitmap(pbPreview.Width - 10, pbPreview.Height - 10);
                    using (var g = Graphics.FromImage(scaled)) {
                        g.InterpolationMode = InterpolationMode.High;
                        g.FillRectangle(Brushes.Black, new Rectangle(Point.Empty, scaled.Size));
                        g.DrawImage(previewBitmap, new Rectangle(Point.Empty, scaled.Size));
                        preview.DrawImage(scaled, 5, 5);
                    }
                }
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

        //TODO this should go in a background worker thread.
        private void RenderFinalResults() {
            // Trees and matrix render rows, cols the other render as 1 x num pixles x (arch count | 1 if windows)
            try {
                RenderEventCount = (int) nudEventCount.Value;
                RenderEventData = new byte[RenderCols * RenderRows * 3, RenderEventCount];
                for (var i = 0; i < MaxEffects; i++) {
                    _eventToRender[i] = 0;
                }

                progressBar.Minimum = 0;
                progressBar.Maximum = RenderEventCount;
                progressBar.Visible = true;

                var colorMask = GetColorMaskFromSettings();
                var colorShift = GetColorShiftFromMask(colorMask);
                for (var renderEvent = 0; renderEvent < RenderEventCount; renderEvent++) {

                    RenderEffects();
                    var nodeRow = 0;
                    var eventRow = 0;
                    const int colorSpaceBytes = 3;
                    for (var row = 0; row < RenderRows * colorSpaceBytes; row += colorSpaceBytes) {
                        for (var col = 0; col < RenderCols; col++) {
                            for (var i = 0; i < colorSpaceBytes; i++) {
                                RenderEventData[eventRow + i, renderEvent] = 
                                    (byte)((_nodes[nodeRow, col].PixelColor.ToArgb() & colorMask[i]) >> colorShift[i]);
                            }
                            eventRow += colorSpaceBytes;
                        }
                        nodeRow++;
                    }

                    for (var i = 0; i < MaxEffects; i++) {
                        _eventToRender[i] += _effectControls[i].Speed;
                    }
                    var val = renderEvent;
                    Invoke((MethodInvoker) delegate {
                        progressBar.Value = val;
                    });
                }
                progressBar.Visible = false;
            }
            catch (NutcrackerException ex) {
                ex.InformException();
                DialogResult = DialogResult.Cancel;
            }
        }


        private const int Red = 0xFF0000;
        private const int Green = 0xFF00;
        private const int Blue = 0xFF;

        private int[] GetColorMaskFromSettings() {
            var mask = new int[3];

            for (var i = 0; i < 3; i++) {
                switch (_colorSpace.Substring(i, 1)) {
                    case "R":
                        mask[i] = Red;
                        break;
                    case "G":
                        mask[i] = Green;
                        break;
                    case "B":
                        mask[i] = Blue;
                        break;
                }
            }

            return mask;
        }

        private const int RedShift = 16;
        private const int GreenShift = 8;
        private const int BlueShift = 0;

        private static int[] GetColorShiftFromMask(IList<int> mask) {
            var shift = new int[3];
            
            for (var i = 0; i < 3; i++) {
                switch (mask[i]) {
                    case Red:
                        shift[i] = RedShift;
                        break;
                    case Green:
                        shift[1] = GreenShift;
                        break;
                    case Blue:
                        shift[i] = BlueShift;
                        break;
                }
            }

            return shift;
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
            UpdateSummary();
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

            // Resharper false positive.
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            lblChannels.Visible = (startEventVisible && eventCountVisible);
            cbChannels.Visible = (startEventVisible && eventCountVisible);
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
        }


        private void UpdateSummary() {
            var eventPeriod = _sequence.EventPeriod;
            var startTime = ((int) (nudStartEvent.Value * eventPeriod)).FormatFull();
            var elapsedTime = ((int) (nudEventCount.Value * eventPeriod)).FormatFull();
            var endTime = ((int) (nudStartEvent.Value + nudEventCount.Value - 1) * eventPeriod).FormatFull();

            lblStartEventTime.Text = startTime;
            lblEventCountTime.Text = elapsedTime;

            var channelCount = RenderRows * RenderCols * 3;
            var msg = (channelCount > _channels.Count)
                ? string.Format("Too Large for your current channel count.{0}You may still render to the clipboard or a routine.", Environment.NewLine)
                : string.Empty;

            tbSummary.Text = String.Format("Strings: {4}{6}Nodes per string: {5}{6}Channels: {2}{6}Position: From {0} thru {1}{6}Using {7} color space{6}{6}{3}", startTime, endTime,
                channelCount, msg, RenderRows, RenderCols, Environment.NewLine, _colorSpace);
            btnOK.Enabled = (msg == string.Empty || rbClipboard.Checked || rbRoutine.Checked);
        }

        #endregion


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
            if (_ignoreIndexChange) return;

            var effectsCount = cbEffectsPresets.Items.Count;

            if (cbEffectsPresets.SelectedIndex < effectsCount - 1) {
                var effectData = _nutcrackerData.GetDataForEffect(cbEffectsPresets.SelectedItem.ToString());
                if (effectData != null) {
                    SetPreset(effectData);
                }
                return;
            }

            //todo: there is some work here to do.
            while (true)
                using (var textDialog = new TextQueryDialog("Preset name", "What do you want to name this preset?", "")) {
                    if (textDialog.ShowDialog() != DialogResult.OK) {
                        return;
                    }
                    var presetName = textDialog.Response;
                    cbEffectsPresets.Items.Insert(effectsCount - 1, presetName);
                    _ignoreIndexChange = true;
                    cbEffectsPresets.SelectedIndex = cbEffectsPresets.FindStringExact(presetName);
                    _ignoreIndexChange = false;
                    SavePreset(presetName);
                    return;
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

        private void btnModelRemove_Click(object sender, EventArgs e) {
            if (MessageBox.Show("Are you sure you want to delete this model?", "Delete Model?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) !=
                DialogResult.Yes) {
                return;
            }

            File.Delete(GetFilePath());
            InitializeModels();
        }

        private void btnModelEdit_Click(object sender, EventArgs e) {
            EditModel(GetFilePath());
        }


        private string GetFilePath() {
            return Path.Combine(Paths.NutcrackerDataPath, cbModels.SelectedItem + Vendor.NutcrakerModelExtension);
        }

        private void cbChannels_DrawItem(object sender, DrawItemEventArgs e) {
            if (e.Index < 0 || e.Index > _channels.Count) {
                return;
            }

            Channel.DrawItem(e, (Channel)cbChannels.Items[e.Index]);
        }
    }
}