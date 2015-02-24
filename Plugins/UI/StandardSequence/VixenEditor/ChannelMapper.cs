using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using VixenPlus;
using VixenPlus.Properties;

using VixenPlusCommon;

namespace VixenEditor {
    public partial class ChannelMapper : Form {
        
        #region Class properties, variables and constants.

        private const int MaxChannels = 23;
        private const int YOffset = 0;
        private const int VscrollOffset = 2;
        private const int WheelDelta = 120; // This is defined by MS, but it is a private const in the Control class.
        private const string TextboxNamePrefix = "tb";
        private const string EditButtonTooltip = "Return to editing your destination map";
        private const string EditButtonText = "Edit Map";

        private readonly Label[] _labels = new Label[MaxChannels];
        private readonly string _previewButtonText;
        private readonly string _previewButtonToolTip;
        private readonly int _sourceChannelCount;
        private readonly ChannelMapperProfile _sourceProfile;
        private readonly char[] _splitChar = { ',' };
        private readonly TextBox[] _textBoxes = new TextBox[MaxChannels];
        private readonly bool _useCheckmark;

        private ChannelMapperProfile _destinationProfile;
        private ChannelMapperProfile _destinationNatural;
        private int _currentTopChannelIndex;
        private string[] _destinationTextBoxText;
        private bool _isPreview;
        private int _maxDestinationCount;
        private int _totalChannelsDisplayed;
        private bool _isDirty;
        private int _selectedTextBox = -1;
        private readonly EventSequence _sourceSequence;
        public bool IsMapValid { get; private set; }

        public string GetMappedSequenceFile {
            get { return TransformSequence(); }
        }

        #endregion


        public ChannelMapper(EventSequence sequence) {
            _sourceSequence = sequence;
            _useCheckmark = Preference2.GetInstance().GetBoolean("UseCheckmark");

            InitializeComponent();

            _sourceProfile = new ChannelMapperProfile(_sourceSequence.Profile.FileName);
            //_sourceNatural = new ChannelMapperProfile(sourceSequence.Profile.FileName);
            _sourceChannelCount = _sourceProfile.GetChannelCount();
            InitializeDropDownList();
            InitializeScrollbar();

            GetDestinationProfile();

            BuildDynamicComponents();
            InitializeDynamicComponents();
            IsMapValid = false;
            MouseWheel += MapperMouseWheel;
            _previewButtonToolTip = toolTips.GetToolTip(btnPreviewEdit);
            _previewButtonText = btnPreviewEdit.Text;
            TogglePreviewElements(true);
        }

        #region new stuff
        private void InitializeDropDownList() {
            cbSortSrc.Items.Add("Original Order");
            cbSortSrc.Items.Add("Name only");
            cbSortSrc.Items.Add("Color only");
            cbSortSrc.Items.Add("Name, then Color");
            cbSortSrc.Items.Add("Color, then Name");

            cbSortSrc.SelectedIndex = 0;

            cbSortDest.Items.Add("Original Order");
            cbSortDest.Items.Add("Name only");
            cbSortDest.Items.Add("Color only");
            cbSortDest.Items.Add("Name, then Color");
            cbSortDest.Items.Add("Color, then Name");

            cbSortDest.SelectedIndex = 0;
        }

        private void MapperMouseWheel(object sender, MouseEventArgs e) {
            var delta = vsb.Value - (e.Delta / WheelDelta);

            if (delta < vsb.Minimum) {
                delta = vsb.Minimum;
            }
            else if (delta > vsb.Maximum - MaxChannels + VscrollOffset) {
                delta = vsb.Maximum - MaxChannels + VscrollOffset;
            }

            vsb.Value = delta;
        }

        private void BuildDynamicComponents() {
            for (var i = 0; i < _totalChannelsDisplayed; i++) {
                _labels[i] = NewDefaultLabel(i * (lblEx.Size.Height + YOffset));
                panel1.Controls.Add(_labels[i]);

                _textBoxes[i] = NewDefaultTextBox(i * (tbEx.Size.Height + YOffset));
                _textBoxes[i].Name = TextboxNamePrefix + i;
                panel1.Controls.Add(_textBoxes[i]);
            }
        }

        private void MapperKeyDown(object sender, KeyEventArgs e) {
            var isSuppressed = true;
            var k = e.KeyCode;

            if (((ModifierKeys & Keys.Shift) != Keys.Shift)
                && ((k >= Keys.D0 && k <= Keys.D9)
                    || (k >= Keys.NumPad0 && k <= Keys.NumPad9)
                    || k == Keys.Oemcomma
                    || k == Keys.Left
                    || k == Keys.Right
                    || k == Keys.Delete
                    || k == Keys.Back)
                ) {
                isSuppressed = false;
                if (k != Keys.Right && k != Keys.Left) {
                    _isDirty = true;
                }
            }
            // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
            else if ((sender is TextBox) && (k == Keys.Tab || k == Keys.Enter)) {
                HandleEnterTabKeys(((TextBox)sender).Name);
            }

            e.SuppressKeyPress = isSuppressed;
        }

        private void HandleEnterTabKeys(string textBoxName) {
            var textBoxIndex = int.Parse(textBoxName.Remove(0, TextboxNamePrefix.Length));

            var isShiftPressed = ((ModifierKeys & Keys.Shift) == Keys.Shift);

            var isAtTopTextBox = textBoxIndex == 0;
            var isAtTopOfList = isAtTopTextBox && _currentTopChannelIndex == 0;

            var isAtBottomTextBox = textBoxIndex == _totalChannelsDisplayed - 1;
            var isAtBottomOfList = isAtBottomTextBox &&
                                   _sourceChannelCount - _currentTopChannelIndex == _totalChannelsDisplayed;

            if (isShiftPressed) {
                if (isAtTopOfList) {
                    // Make last text box have focus and move to show bottom of list
                    textBoxIndex = _totalChannelsDisplayed - 1;
                    ShowComponents(_sourceChannelCount - _totalChannelsDisplayed);
                }
                else if (isAtTopTextBox) {
                    // Move list down one and stay in same text box
                    ShowComponents(_currentTopChannelIndex - 1);
                }
                else {
                    // Make previous text box have focus
                    textBoxIndex--;
                }
            }
            else {
                if (isAtBottomOfList) {
                    // Make first text box have focus and move to show top of list
                    textBoxIndex = 0;
                    ShowComponents(0);
                }
                else if (isAtBottomTextBox) {
                    //Move list up one and stay in same text box
                    ShowComponents(_currentTopChannelIndex + 1);
                }
                else {
                    // Make next text box have focus
                    textBoxIndex++;
                }
            }

            // Find the control and set the focus to the first matching item.
            var c = (TextBox)Controls.Find(TextboxNamePrefix + textBoxIndex, true)[0];

            c.Focus();
            c.SelectionStart = 0;
            c.SelectionLength = c.Text.Length;
            _selectedTextBox = textBoxIndex;
        }

        private static void MapperPreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
            // In order to consume the tab key we have to set IsInputKey to true.
            e.IsInputKey = (e.KeyCode == Keys.Tab);
        }

        private void InitializeDynamicComponents() {
            vsb.Height = (lblEx.Height + YOffset) * MaxChannels;
            _destinationTextBoxText = new string[_sourceChannelCount];
            for (var i = 0; i < _sourceChannelCount; i++) {
                _destinationTextBoxText[i] = "";
            }
            ShowComponents(0);
        }

        private void InitializeScrollbar() {
            if (_sourceChannelCount > MaxChannels) {
                _totalChannelsDisplayed = MaxChannels;
                vsb.Enabled = true;
                vsb.Maximum = _sourceChannelCount - VscrollOffset;
                vsb.LargeChange = MaxChannels - 1;
            }
            else {
                _totalChannelsDisplayed = _sourceChannelCount;
                vsb.Enabled = false;
            }
        }

        private void ShowComponents(int start, bool save = true) {
            if (save) SaveCurrentValues();

            for (var row = 0; row < _totalChannelsDisplayed; row++) {
                _labels[row].Text = _sourceProfile.GetChannelName(start + row);
                _labels[row].BackColor = _sourceProfile.GetChannelColor(start + row);
                _labels[row].ForeColor = _labels[row].BackColor.GetForeColor();

                _textBoxes[row].Text = _destinationTextBoxText[_sourceProfile.GetChannelLocation(start + row)];

                if (!_isPreview) continue;

                var s = _textBoxes[row].Text.Split(_splitChar, StringSplitOptions.RemoveEmptyEntries);
                var col = 0;

                foreach (var channel in s) {
                    var channelNum = int.Parse(channel);
                    var c = Controls.Find("r" + row + "c" + col, true)[0];
                    if (channelNum < _destinationNatural.GetChannelCount()) {
                        c.Text = _destinationNatural.GetChannelName(channelNum);
                        c.BackColor = _destinationNatural.GetChannelColor(channelNum);
                        c.ForeColor = c.BackColor.GetForeColor();
                        c.Visible = true;
                    }
                    col++;
                }

                for (; col < _maxDestinationCount; col++) {
                    Controls.Find("r" + row + "c" + col, true)[0].Visible = false;
                }
            }

            _currentTopChannelIndex = start;
            vsb.Value = start;

        }

        private void SaveCurrentValues() {
            for (var i = 0; i < _totalChannelsDisplayed; i++) {
                _destinationTextBoxText[_sourceProfile.GetChannelLocation(_currentTopChannelIndex + i)] = _textBoxes[i].Text;
            }
        }

        private Label NewDefaultLabel(int y) {
            return new Label {
                Text = lblEx.Text,
                Size = lblEx.Size,
                Enabled = true,
                Visible = true,
                TextAlign = lblEx.TextAlign,
                Location = new Point(lblEx.Location.X, y),
                AutoSize = lblEx.AutoSize
            };
        }

        private TextBox NewDefaultTextBox(int y) {
            var textBox = new TextBox {
                Text = "",
                Size = tbEx.Size,
                Location = new Point(tbEx.Location.X, y),
                Enabled = true,
                Visible = true
            };

            // Need these events so that we can catch the Tab and Enter key to do special list movement.
            textBox.PreviewKeyDown += MapperPreviewKeyDown;
            textBox.KeyDown += MapperKeyDown;
            textBox.Click += MapperClick;
            return textBox;
        }


        private void MapperClick(object sender, EventArgs e) {
            var tb = sender as TextBox;
            if (tb == null) return;

            _selectedTextBox = int.Parse(tb.Name.Substring(TextboxNamePrefix.Length));
        }


        private void VsbScroll(object sender, ScrollEventArgs e) {
            ScrollTo(e.NewValue);
        }

        private void MapperFormClosing(object sender, FormClosingEventArgs e) {
            // Remove all of our event handlers.
            MouseWheel -= MapperMouseWheel;
            for (var i = 0; i < _totalChannelsDisplayed; i++) {
                _textBoxes[i].PreviewKeyDown -= MapperPreviewKeyDown;
                _textBoxes[i].KeyDown -= MapperKeyDown;
                _textBoxes[i].Click -= MapperClick;
            }
        }


        private string GetMap() {
            SaveCurrentValues();

            var srcSortOrder = cbSortSrc.SelectedIndex;
            var destSortOrder = cbSortDest.SelectedIndex;
            _sourceProfile.SetSortOrder(0);
            _destinationProfile.SetSortOrder(0);

            var mappedChannels = new HashSet<int>();
            if (cbKeepUnmapped.Checked) {
                for (var i = 0; i < _sourceChannelCount; i++) {
                    var src = _sourceProfile.GetChannelLocation(i);
                    var entries = _destinationTextBoxText[src].Split(_splitChar, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var entry in entries) {
                        mappedChannels.Add(int.Parse(entry));
                    }
                }
            }

            var theMap = new StringBuilder();

            for (var i = 0; i < _sourceChannelCount; i++) {
                var src = _sourceProfile.GetChannelLocation(i);
                theMap.Append(i + ":");
                if (cbKeepUnmapped.Checked && _destinationTextBoxText[src].Equals("") && !mappedChannels.Contains(i)) {
                    theMap.Append(i + " ");
                }
                else {
                    var values = _destinationTextBoxText[src].Split(_splitChar, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var channel in values) {
                        theMap.Append(channel.Trim() + " ");
                    }
                }
                theMap.Append(';');
            }

            _sourceProfile.SetSortOrder(srcSortOrder);
            _destinationProfile.SetSortOrder(destSortOrder);

            return theMap.ToString();
        }

        private void VsbValueChanged(object sender, EventArgs e) {
            ScrollTo(vsb.Value);
        }

        private void ScrollTo(int newValue) {
            if (newValue + _totalChannelsDisplayed > _sourceChannelCount) {
                ShowComponents(_sourceChannelCount - _totalChannelsDisplayed);
            }
            else {
                ShowComponents(newValue);
            }
        }

        private void BtnTransformClick(object sender, EventArgs e) {
            SaveCurrentValues();
            SaveToFileIfDirty();

            var destCount = new int[_destinationProfile.GetChannelCount()];

            var badChannels = FindBadChannels(destCount);
            var dupeChannels = FindDupeChannels(destCount);

            if (string.IsNullOrEmpty(badChannels) && string.IsNullOrEmpty(dupeChannels)) {
                IsMapValid = true;
                if (ActiveForm == null) {
                    return;
                }

                ActiveForm.DialogResult = DialogResult.OK;
                if (ActiveForm != null) ActiveForm.Close();
            }
            else {
                IsMapValid = false;
                ShowErrors(dupeChannels, badChannels);
            }
        }

        private string FindDupeChannels(IList<int> destCount) {
            var destChannelCount = _destinationProfile.GetChannelCount();

            var dupeChannels = new StringBuilder();
            for (var i = 0; i < destChannelCount; i++) {
                if (destCount[i] <= 1) continue;
                dupeChannels.Append(i).Append(" (" + destCount[i] + @" times), ");
            }

            return dupeChannels.ToString();
        }

        private string FindBadChannels(int[] destCount) {
            var destChannelCount = _destinationProfile.GetChannelCount();

            var currentChannel = 0;

            var badChannels = new StringBuilder();
            for (var i = 0; i < _sourceChannelCount; i++) {
                var process = true;

                var destChannels = _destinationTextBoxText[_sourceProfile.GetChannelLocation(i)].Split(_splitChar);

                foreach (var channel in destChannels) {
                    if (channel.Equals("") && cbKeepUnmapped.Checked) {
                        currentChannel = i;
                    }

                    if (channel.Equals("") && !cbKeepUnmapped.Checked) {
                        process = false;
                    }

                    if (!channel.Equals("")) {
                        currentChannel = int.Parse(channel);
                    }

                    if (!process) continue;

                    if (currentChannel < destChannelCount) {
                        destCount[currentChannel]++;
                    }
                    else {
                        badChannels.Append(channel).Append(" (from ch " + i + "), ");
                    }
                }
            }

            return badChannels.ToString();
        }


        private void SaveToFileIfDirty() {
            if (!_isDirty) {
                return;
            }

            if (MessageBox.Show(@"Would you like to save your mapping data before transforming your sequence?",
                @"Save Changes?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question)
                .Equals(DialogResult.Yes)) {
                BtnSaveClick(this, new EventArgs());
            }
        }


        private static void ShowErrors(string dupes, string bad) {
            var msg = new StringBuilder(@"Transformation cannot take place because of the follow errors:");
            msg.Append(Environment.NewLine + Environment.NewLine);

            if (dupes.Length > 0) {
                msg.AppendLine(@"Destination channels cannot come from more than one source channel: ").AppendLine(
                    dupes.Remove(dupes.Length - 2)).AppendLine();
            }

            if (bad.Length > 0) {
                msg.AppendLine(@"Bad (out of range) destination channels: ").AppendLine(bad.Remove(bad.Length - 2)).
                    AppendLine();
            }

            msg.Append(@"Please correct these errors and try again.");

            MessageBox.Show(msg.ToString(), @"Errors in map. Can't transform.", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
        }


        private void BtnSaveClick(object sender, EventArgs e) {
            using (var saveMap = new SaveFileDialog()) {
                saveMap.InitialDirectory = Paths.MapperPath;
                saveMap.DefaultExt = Vendor.MapperExtension;
                saveMap.CheckPathExists = true;
                saveMap.FileName = _sourceProfile.GetFileName() + " to " + _destinationProfile.GetFileName() + Vendor.MapperExtension;
                saveMap.Filter = @"MapperFile|*" + Vendor.MapperExtension;
                saveMap.AddExtension = true;
                if (!saveMap.ShowDialog().Equals(DialogResult.OK)) return;
                using (TextWriter tw = new StreamWriter(saveMap.FileName)) {
                    tw.WriteLine(GetMap());
                    tw.Close();
                    _isDirty = false;
                }
            }
        }


        private void BtnLoadClick(object sender, EventArgs e) {
            using (var openMap = new OpenFileDialog()) {
                openMap.InitialDirectory = Paths.MapperPath;
                openMap.CheckPathExists = true;
                openMap.DefaultExt = Vendor.MapperExtension;
                openMap.Filter = @"MapperFile | *" + Vendor.MapperExtension; 
                if (!openMap.ShowDialog().Equals(DialogResult.OK)) return;

                using (TextReader tr = new StreamReader(openMap.FileName)) {
                    LoadMap(tr.ReadLine());
                    tr.Close();
                }
            }
        }


        private void LoadMap(string map) {
            var isMapError = false;
            var srcSortOrder = cbSortSrc.SelectedIndex;
            var destSortOrder = cbSortDest.SelectedIndex;
            _sourceProfile.SetSortOrder(0);
            _destinationProfile.SetSortOrder(0);

            var splitChannelChar = new[] { ';' };
            var channels = map.Split(splitChannelChar, StringSplitOptions.RemoveEmptyEntries);
            foreach (var channel in channels) {
                var elements = channel.Split(':');

                var sourceChannel = int.Parse(elements[0]);
                if (sourceChannel < _sourceChannelCount) {
                    _destinationTextBoxText[_sourceProfile.GetChannelLocation(sourceChannel)] = elements.Length > 1
                        ? elements[1].Trim().Replace(' ', _splitChar[0]) : "";
                }
                else {
                    isMapError = true;
                }
            }

            _sourceProfile.SetSortOrder(srcSortOrder);
            _destinationProfile.SetSortOrder(destSortOrder);

            for (var i = 0; i < _totalChannelsDisplayed; i++) {
                _textBoxes[i].Text = _destinationTextBoxText[_sourceProfile.GetChannelLocation(i + _currentTopChannelIndex)];
            }

            if (!isMapError) {
                return;
            }
            var msg = string.Format("The mapping file you selected had more than {0} channels, those channel maps were ignored", _sourceChannelCount);
            MessageBox.Show(msg, @"Mapping file error", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnPreviewClick(object sender, EventArgs e) {
            SaveCurrentValues();
            if (btnPreviewEdit.Text.Equals(EditButtonText)) {
                _isPreview = false;
                btnPreviewEdit.Text = _previewButtonText;
                toolTips.SetToolTip(btnPreviewEdit, _previewButtonToolTip);

                TeardownPreviewGrid();
            }
            else {
                _isPreview = true;
                btnPreviewEdit.Text = EditButtonText;
                toolTips.SetToolTip(btnPreviewEdit, EditButtonTooltip);
                ConstructPreviewGrid();
            }
            TogglePreviewElements(!_isPreview);
            ShowComponents(_currentTopChannelIndex);
        }


        private void ConstructPreviewGrid() {
            if (cbKeepUnmapped.Checked) {
                LoadMap(GetMap());
            }

            SetMaxDestinationCount();

            var width = tbEx.Width / _maxDestinationCount;

            for (var row = 0; row < _totalChannelsDisplayed; row++) {
                for (var col = 0; col < _maxDestinationCount; col++) {
                    panel1.Controls.Add(new Label {
                        Name = "r" + row + "c" + col, Height = lblEx.Size.Height, Width = width,
                        Location = new Point(tbEx.Location.X + width * col, row * (lblEx.Height + YOffset)), Text = Name,
                        TextAlign = ContentAlignment.MiddleLeft, Visible = false
                    });
                }
            }
        }


        private void SetMaxDestinationCount() {
            var previewError = false;
            var errorList = new List<string>();
            var maxChannel = _destinationProfile.GetChannelCount() - 1;
            _maxDestinationCount = 1;
            for (var i = 0; i < _sourceChannelCount; i++) {
                var channelName = _sourceProfile.GetChannelName(i);
                var enteredChannels = _destinationTextBoxText[_sourceProfile.GetChannelLocation(i)];
                if (enteredChannels == "") {
                    continue;
                }

                var channels = enteredChannels.Split(_splitChar);
                var elementCount = channels.Length;
                // ReSharper disable UnusedVariable
                foreach (var c in channels.Where(c => int.Parse(c) > maxChannel)) {
                // ReSharper restore UnusedVariable
                    previewError = true;
                    if (!errorList.Contains(channelName)) {
                        errorList.Add(channelName);
                    }
                    elementCount--;
                }

                if (_maxDestinationCount < elementCount) {
                    _maxDestinationCount = elementCount;
                }
            }
            if (!previewError) {
                return;
            }

            var msg =
                string.Format(
                    "There was one or more invalid channels in your map. The channels in error can not be shown. \n\nThe Maximum channel number you can enter is {0}\n\nPlease Check the following source channel map(s):\n\n",
                    _destinationProfile.GetChannelCount() - 1);

            msg = errorList.Aggregate(msg, (current, s) => current + (s + Environment.NewLine));
            
            MessageBox.Show(msg, @"Error in map", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void TeardownPreviewGrid() {
            for (var row = 0; row < _totalChannelsDisplayed; row++) {
                for (var col = 0; col < _maxDestinationCount; col++) {
                    var cc = panel1.Controls.Find("r" + row + "c" + col, true);
                    foreach (var c in cc) {
                        panel1.Controls.Remove(c);
                    }
                }
            }
        }

        private void TogglePreviewElements(bool isVisible) {
            btnLoadMap.Visible = isVisible && _destinationProfile != null;
            btnSaveMap.Visible = isVisible && _destinationProfile != null;
            btnDestinationProfile.Visible = isVisible;
            btnPreviewEdit.Visible = _destinationProfile != null;
            cbKeepUnmapped.Visible = _destinationProfile != null && _sourceProfile.GetChannelCount() <= _destinationProfile.GetChannelCount() && isVisible;
            btnCancel.Visible = isVisible;
            btnTransform.Visible = isVisible && _destinationProfile != null;
            for (var i = 0; i < _totalChannelsDisplayed; i++) {
                _textBoxes[i].Visible = isVisible;
            }
        }

        private void SortSrcIndexChanged(object sender, EventArgs e) {
            SaveCurrentValues();
            _sourceProfile.SetSortOrder(cbSortSrc.SelectedIndex);
            ShowComponents(_currentTopChannelIndex, false);
        }


        private void SortDestIndexChanged(object sender, EventArgs e) {
            if (_destinationProfile == null) return;

            SaveCurrentValues();
            _destinationProfile.SetSortOrder(cbSortDest.SelectedIndex);
            PopulateDestinationList();
        }

        private void cbKeepEmpty_CheckedChanged(object sender, EventArgs e) {
            _isDirty = true;
        }
        #endregion

        #region old stuff

        private void PopulateDestinationList() {

            var maxSize = lbDestination.Width;
            using (var g = lbDestination.CreateGraphics()) {
                lbDestination.Items.Clear();
                for (var i = 0; i < _destinationProfile.GetChannelCount(); i++) {
                    var name = _destinationProfile.GetChannelName(i);
                    lbDestination.Items.Add(name);
                    var size = g.MeasureString(name, lbDestination.Font);
                    if (size.Width > maxSize) maxSize = (int) size.Width;
                }
            }
            lbDestination.HorizontalExtent = maxSize;
        }


        private void GetDestinationProfile() {
            using (var openProfile = new OpenFileDialog()) {
                openProfile.Filter = Resources.Profile + @"|*" + Vendor.ProfileExtension;
                openProfile.DefaultExt = Vendor.ProfileExtension.Replace(".", "");
                openProfile.CheckFileExists = true;
                openProfile.Title = @"Select a destination profile";
                openProfile.InitialDirectory = Paths.ProfilePath;
                openProfile.FileName = string.Empty;
                if (openProfile.ShowDialog() != DialogResult.OK) {
                    return;
                }

                _destinationProfile = new ChannelMapperProfile(openProfile.FileName);
                _destinationNatural = new ChannelMapperProfile(openProfile.FileName);
                PopulateDestinationList();
            }
        }


        private void btnDestProfile_Click(object sender, EventArgs e) {
            GetDestinationProfile();

            TogglePreviewElements(true);
        }

        private void lbDestination_DrawItem(object sender, DrawItemEventArgs e) {
            var listBox = sender as ListBox;
            if (e.Index == -1 || listBox == null) {
                return;
            }
            var channelNum = e.Index;
            var name = _destinationProfile.GetChannelName(channelNum);
            var color = _destinationProfile.GetChannelColor(channelNum);

            e.DrawItemWide(name, color, _useCheckmark);
        }
        #endregion

        private void lbDestination_DoubleClick(object sender, EventArgs e) {
            SaveCurrentValues();
            var listBox = sender as ListBox;
            if (listBox == null || _selectedTextBox == -1 || !btnTransform.Visible) return;
            
            var srcChannel = _sourceProfile.GetChannelLocation(_selectedTextBox + _currentTopChannelIndex);

            if (_destinationTextBoxText[srcChannel].Length > 0) {
                _destinationTextBoxText[srcChannel] += ",";
            }

            _destinationTextBoxText[srcChannel] += _destinationProfile.GetChannel(lbDestination.SelectedIndex).Location;
            ShowComponents(_currentTopChannelIndex, false);

            var tbControl = (TextBox)Controls.Find(TextboxNamePrefix + _selectedTextBox, true)[0];
            tbControl.Focus();
            tbControl.SelectionStart = tbControl.TextLength;
        }

        private string TransformSequence() {
            var sequence = new ChannelMapperSequence(_sourceSequence.FileName);
            var numOfEvents = sequence.GetEventCount();

            var oldNumOfChannels = _sourceProfile.GetChannelCount();
            var newNumOfChannels = _destinationProfile.GetChannelCount();
            var newEventValues = new byte[newNumOfChannels, numOfEvents];
            var oldEventValues = new byte[oldNumOfChannels, numOfEvents];


            var oldEventData = Convert.FromBase64String(sequence.EventData);
            var oldEventLength = oldEventData.Length;

            var currentEvent = 0;

            for (var chan = 0; chan < oldNumOfChannels; chan++) {
                for (var thisEvent = 0; currentEvent < oldEventLength && thisEvent < numOfEvents; thisEvent++) {
                    oldEventValues[chan, thisEvent] = oldEventData[currentEvent++];
                }
            }

            //writeChannels("oldChannels.txt", oldEventValues, oldNumOfChannels, numOfEvents);

            var mapSplit = new[] { ';' };
            var channelSplit = new[] { ':' };
            var elementSplit = new[] { ' ' };

            var channels = GetMap().Split(mapSplit, StringSplitOptions.RemoveEmptyEntries);
            foreach (var channel in channels) {
                var elements = channel.Split(channelSplit, StringSplitOptions.RemoveEmptyEntries);

                if (elements.Length <= 1) continue;

                var from = int.Parse(elements[0]);
                var toChannels = elements[1].Split(elementSplit, StringSplitOptions.RemoveEmptyEntries);
                foreach (var toChannel in toChannels) {
                    var to = int.Parse(toChannel);
                    for (var i = 0; i < numOfEvents; i++) {
                        var destinationChannel = _destinationProfile.GetChannelLocation(to);
                        var sourceChannel = _sourceProfile.GetChannelLocation(from);
                        var oldEventValue = oldEventValues[sourceChannel, i];
                        newEventValues[destinationChannel, i] = oldEventValue;
                    }
                }
            }

            //writeChannels("newChannels.txt", newEventValues, newNumOfChannels, numOfEvents);

            var newEventData = new byte[newNumOfChannels * numOfEvents];
            var index = 0;
            for (var i = 0; i < newNumOfChannels; i++) {
                for (var j = 0; j < numOfEvents; j++) {
                    newEventData[index++] = newEventValues[i, j];
                }
            }

            sequence.EventData = Convert.ToBase64String(newEventData);

            return sequence.SaveNewData(_destinationProfile.GetFileName());
        }
    }
}
