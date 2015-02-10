using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using VixenPlus.Properties;

using VixenPlusCommon;

using common = VixenPlusCommon.Properties;

namespace VixenPlus.Dialogs {
    public partial class NewSequenceWizardDialog : Form {
        private readonly EventSequence _eventSequence;

        #region Wizard Text and Explanations.

        /// <summary>
        /// What
        /// Why
        /// Effects
        /// Notes
        /// </summary>
        private readonly string[,] _explanations = {
            {
                "The new sequence wizard will walk you through the steps of creating a new " + Vendor.ProductName + " sequence.",
                "Because you want to make the lights blink and bring smiles to children's faces everywhere.",
                "Christmas Light Addiction Problem (CLAP) is a known effect of blinking lights and smiling children.",
                "There is currently no know cure for CLAP. Proceed at your own risk and avoid lutefisk at all costs!"
            }, {
                "The event period length is how long a single on/off event lasts.",
                "A sequence is a collection of sequencial events of the same length.  At every event trigger, " + Vendor.ProductName +
                " updates the controller with new data.",
                "The smaller the event period, the finer the control you over the timing of the events.  Generally, you don't want to have the event period be any shorter than you need it to be in order to synchronize to your selected audio.",
                "50 milliseconds, or 20 updates/second, is adequate for synchronizing to most audio. Also, smaller event period numbers = bigger hardware requirements."
            }, {
                "A profile contains information about channels, plug ins, and groups.",
                "To reduce the otherwise repetitive task of setting up channels, plug ins and groups for every sequence you create.",
                "By selecting a profile, the channel count, channel names, channel colors, channel outputs, plugin setup, and groups will all be defined for you.",
                "Profiles are recommended, but not required. Profiles can really help you quickly create new sequences."
            }, {
                "A channel is the smallest unit of control in an element. Pixels usually have 3 channels per pixel, light strings usually have 1.",
                "Each channel gives you another level of control, sometimes you will want to edit each individually and other times as a group or RGB group.",
                "For every channel you define in your sequence there will be a row defined in the event grid.",
                "The profile editor (" + Vendor.ModuleManager +
                ") allows you to manipulate your channels and groups easily. Doing it here is possible but not the desired method."
            },
            {"Names for the channels defined earlier.", "For easier identification of a channel.", string.Empty, string.Empty}, {
                Vendor.ProductName + " communicates with each controller using a plug in.",
                "You can sequence without a plug in, but nothing will output on your controllers.", string.Empty,
                "If you select plug ins to use with this sequence, they need to be setup before they can be used.  The plug ins and their respective setup can be changed at any time.  VixenPlus supports using multiple plug ins simultaneously."
            }, {
                "Assign your music and create event patterns based on the music (or without music).  As the music plays, you tap a pattern on your keyboard on a channel-by-channel basis.",
                "The sequence can be authored to be synchronized with audio.  The music would be played by " + Vendor.ProductName +
                " at the same time the sequence is being executed.\nEvent patterns are going away though, better things are coming!",
                "The event grid will be populated with the created events.  The events are on/off values only.",
                "The audio can be added or removed at any time during editing.  A sequence must be at least as long as the music attached.\nDefining event patterns can also be used to mark events of significance in the music."
            }, {
                "The length of the sequence in minutes and seconds.", string.Empty, string.Empty,
                "If there is audio assigned to a sequence, the sequence length must be at least as long as the music attached."
            }
        };

        #endregion


        private readonly Stack<int> _history;
        private readonly Preference2 _preferences;
        private bool _back;
        private bool _skip;


        public NewSequenceWizardDialog(Preference2 preferences) {
            InitializeComponent();
            _preferences = preferences;
            _eventSequence = new EventSequence(_preferences);
            _history = new Stack<int>();

            InitializeClass();
        }


        private void InitializeClass() {
            Icon = common.Resources.VixenPlus;
            openFileDialog.InitialDirectory = Paths.SequencePath;
            tabControl.SelectedIndex = 0;
            PopulateProfileList();
            var str = _preferences.GetString("DefaultProfile");
            if (str.Length > 0) {
                comboBoxProfiles.SelectedIndex = comboBoxProfiles.Items.IndexOf(str);
            }
            UpdateExplanations(0);
        }


        public EventSequence Sequence {
            get { return _eventSequence; }
        }


        private void buttonAssignAudio_Click(object sender, EventArgs e) {
            var preferredSoundDevice = _preferences.GetInteger("SoundDevice");

            using (var dialog = new AudioDialog(_eventSequence, _preferences.GetBoolean("EventSequenceAutoSize"), preferredSoundDevice)) {
                if (dialog.ShowDialog() == DialogResult.OK) {
                    SetSequenceTime();
                }
            }
        }


        private void buttonImportChannels_Click(object sender, EventArgs e) {
            if (openFileDialog.ShowDialog() != DialogResult.OK) {
                return;
            }
            var fileIO = FileIOHelper.GetNativeHelper();
            var sequence = fileIO.OpenSequence(openFileDialog.FileName);
            textBoxChannelCount.Text = sequence.FullChannelCount.ToString(CultureInfo.InvariantCulture);
            var builder = new StringBuilder();
            foreach (var channel in sequence.FullChannels) {
                builder.AppendLine(channel.Name);
            }
            textBoxChannelNames.Text = builder.ToString();
            _eventSequence.FullChannels.Clear();
            _eventSequence.FullChannels.AddRange(sequence.FullChannels);
        }


        private void buttonNext_Click(object sender, EventArgs e) {
            if (buttonNext.Text == @"&Next  >") {
                _skip = false;
                _back = false;
                if (tabControl.SelectedTab == tabPageProfile) {
                    tabControl.SelectedTab = comboBoxProfiles.SelectedIndex == 0 ? tabPageChannelCount : tabPageAudio;
                }
                else {
                    tabControl.SelectedIndex++;
                }
            }
            else {
                buttonOK_Click(null, null);
                DialogResult = DialogResult.OK;
            }
        }


        private void buttonOK_Click(object sender, EventArgs e) {
            tabControl_Deselecting(null,
                new TabControlCancelEventArgs(tabControl.SelectedTab, tabControl.SelectedIndex, false, TabControlAction.Deselecting));
        }


        private void buttonPrev_Click(object sender, EventArgs e) {
            _back = true;
            tabControl.SelectedIndex = _history.Pop();
            buttonPrev.Enabled = _history.Count > 0;
        }


        private void buttonProfileManager_Click(object sender, EventArgs e) {
            using (var dialog = new VixenPlusRoadie()) {
                if (dialog.ShowDialog() == DialogResult.OK) {
                    PopulateProfileList();
                }
            }
        }


        private void buttonSetupPlugins_Click(object sender, EventArgs e) {
            using (var dialog = new VixenPlusRoadie(_eventSequence, true)) {
                dialog.ShowDialog();
            }
        }


        private void buttonSkip_Click(object sender, EventArgs e) {
            _skip = true;
            _back = false;
            tabControl.SelectedIndex++;
        }


        private static int ParseTimeString(string text) {
            TimeSpan timeSpan;
            if (TimeSpan.TryParse("00:" + text, out timeSpan)) {
                return (int) timeSpan.TotalMilliseconds;
            }
            MessageBox.Show(Resources.InvalidTimeFormat, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return 0;
        }


        private void PopulateProfileList() {
            var selectedIndex = comboBoxProfiles.SelectedIndex;
            comboBoxProfiles.BeginUpdate();
            comboBoxProfiles.Items.Clear();
            comboBoxProfiles.Items.Add(Resources.None);
            foreach (var str in Directory.GetFiles(Paths.ProfilePath, @Vendor.All + Vendor.ProfileExtension).Where(str => null != str)) {
                comboBoxProfiles.Items.Add(Path.GetFileNameWithoutExtension(str));
            }
            if (selectedIndex < comboBoxProfiles.Items.Count) {
                comboBoxProfiles.SelectedIndex = selectedIndex;
            }
            comboBoxProfiles.EndUpdate();
        }


        private void SetSequenceTime() {
            textBoxTime.Text = string.Format("{0:d2}:{1:d2}.{2:d3}", _eventSequence.Time / 60000, (_eventSequence.Time % 60000) / 1000,
                _eventSequence.Time % 1000);
        }


        private const int TabEventPeriod = 1;
        private const int TabProfile = 2;
        private const int TabChannelCount = 3;
        private const int TabChannelNames = 4;
        private const int TabSequenceTime = 7;


        private void tabControl_Deselecting(object sender, TabControlCancelEventArgs e) {
            if (!_back) {
                _history.Push(e.TabPageIndex);
                buttonPrev.Enabled = true;
            }

            if (_skip) {
                return;
            }

            var errorText = string.Empty;
            switch (e.TabPageIndex) {
                case TabEventPeriod: {
                    int eventPeriod;
                    if (int.TryParse(textBoxEventPeriod.Text, out eventPeriod)) {
                        if (eventPeriod < Vendor.MinimumEventPeriod) {
                            errorText = String.Format(Resources.EventPeriodTooShort, Vendor.MinimumEventPeriod);
                            textBoxEventPeriod.Text = Vendor.MinimumEventPeriod.ToString(CultureInfo.InvariantCulture);
                        }
                        else {
                            _eventSequence.EventPeriod = eventPeriod;
                        }
                    }
                    else {
                        errorText = textBoxChannelCount.Text + Resources.EventPeriodInvalid;
                    }
                    break;
                }
                case TabProfile:
                    if (comboBoxProfiles.SelectedIndex == 0) {
                        _eventSequence.FileIOHandler = FileIOHelper.GetNativeHelper();
                        _eventSequence.Profile = null;
                    }
                    else {
                        var profilePath = Path.Combine(Paths.ProfilePath, comboBoxProfiles.SelectedItem + ".pro");
                        _eventSequence.FileIOHandler = FileIOHelper.GetProfileVersion(profilePath);
                        _eventSequence.Profile = _eventSequence.FileIOHandler.OpenProfile(profilePath);
                    }

                    if (_eventSequence.Profile != null) {
                        _eventSequence.Groups = _eventSequence.Profile.Groups;
                    }
                    break;

                case TabChannelCount: {

                    int channelCount;
                    if (int.TryParse(textBoxChannelCount.Text, out channelCount)) {
                        if (channelCount < 1) {
                            errorText = Resources.ChannelCountMinimums;
                        }
                        else {
                            if ((channelCount > 1024) &&
                                (MessageBox.Show(string.Format(Resources.ConfirmChannelCount, channelCount), Vendor.ProductName,
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)) {
                                tabControl.TabIndex = 1;
                            }
                            else {
                                Cursor = Cursors.WaitCursor;
                                try {
                                    _eventSequence.FullChannelCount = channelCount;
                                }
                                finally {
                                    Cursor = Cursors.Default;
                                }
                            }
                        }
                    }
                    else {
                        errorText = textBoxChannelCount.Text + Resources.InvalidChannelCount;
                    }
                    break;
                }
                case TabChannelNames:
                    if (textBoxChannelNames.Lines.Length == Convert.ToInt32(textBoxChannelCount.Text)) {
                        if (textBoxChannelNames.Lines.Any(str2 => str2.Trim() == string.Empty)) {
                            errorText = Resources.ChannelNameCantBeBlank;
                        }
                        Cursor = Cursors.WaitCursor;
                        try {
                            for (var i = 0; i < _eventSequence.FullChannelCount; i++) {
                                _eventSequence.FullChannels[i].Name = textBoxChannelNames.Lines[i];
                            }
                        }
                        finally {
                            Cursor = Cursors.Default;
                        }
                        break;
                    }
                    errorText = Resources.ChannelCountAndNameInequal;
                    break;

                case TabSequenceTime: {
                    var sequenceTimeInMills = ParseTimeString(textBoxTime.Text);
                    if (sequenceTimeInMills != 0) {
                        _eventSequence.Time = sequenceTimeInMills;
                    }
                    else {
                        errorText = "Bad time format";
                    }
                    break;
                }
            }

            if (errorText == string.Empty) {
                return;
            }
            e.Cancel = true;
            MessageBox.Show(errorText, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }


        private void tabControl_Selected(object sender, TabControlEventArgs e) {
            switch (e.TabPageIndex) {
                case TabEventPeriod:
                    textBoxEventPeriod.Text = _eventSequence.EventPeriod.ToString(CultureInfo.InvariantCulture);
                    break;

                case TabProfile:
                    if (_eventSequence.Profile != null) {
                        comboBoxProfiles.SelectedIndex = comboBoxProfiles.Items.IndexOf(_eventSequence.Profile.Name);
                        break;
                    }
                    comboBoxProfiles.SelectedIndex = 0;
                    break;

                case TabChannelNames:
                    Cursor = Cursors.WaitCursor;
                    try {
                        string[] strArray;
                        var num = Convert.ToInt32(textBoxChannelCount.Text);
                        var length = textBoxChannelNames.Lines.Length;
                        if (length < num) {
                            strArray = new string[num];
                            textBoxChannelNames.Lines.CopyTo(strArray, 0);
                            while (length < num) {
                                strArray[length] = string.Format(Resources.ChannelSpace + "{0}", length + 1);
                                length++;
                            }
                            textBoxChannelNames.Lines = strArray;
                        }
                        else if (length > num) {
                            strArray = new string[num];
                            for (var i = 0; i < num; i++) {
                                strArray[i] = textBoxChannelNames.Lines[i];
                            }
                            textBoxChannelNames.Lines = strArray;
                        }
                    }
                    finally {
                        Cursor = Cursors.Default;
                    }
                    break;

                case TabSequenceTime:
                    SetSequenceTime();
                    break;
            }
            UpdateExplanations(e.TabPageIndex);
            buttonSkip.Enabled = e.TabPageIndex < (tabControl.TabCount - 1);
            buttonNext.Text = e.TabPageIndex < tabControl.TabCount - 1 ? @"&Next  >" : @"Create It";
            AcceptButton = (e.TabPageIndex == 4) ? null : buttonNext;
        }


        private void textBoxChannelNames_TextChanged(object sender, EventArgs e) {
            labelNamesChannels.Text = string.Format("{0} names / {1} channels", textBoxChannelNames.Lines.Length, textBoxChannelCount.Text);
        }


        private const int What = 0;
        private const int Why = 1;
        private const int Effect = 2;
        private const int Notes = 3;


        private void UpdateExplanations(int pageIndex) {
            // Set the explanations
            lblWhatText.Text = _explanations[pageIndex, What];
            lblWhyText.Text = _explanations[pageIndex, Why];
            lblEffectText.Text = _explanations[pageIndex, Effect];
            lblNotesText.Text = _explanations[pageIndex, Notes];

            // Hide what is not showing anything.
            lblWhat.Visible = string.Empty != lblWhatText.Text;
            lblWhy.Visible = string.Empty != lblWhyText.Text;
            lblEffect.Visible = string.Empty != lblEffectText.Text;
            lblNotes.Visible = string.Empty != lblNotesText.Text;
        }
    }
}