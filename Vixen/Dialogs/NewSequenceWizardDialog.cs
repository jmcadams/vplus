//TODO This still needs to be cleaned up!  Strike that, rewritten

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;



using VixenPlus.Properties;

using VixenPlusCommon;

namespace VixenPlus.Dialogs {
    public partial class NewSequenceWizardDialog : Form {
        private readonly EventSequence _eventSequence;

        private readonly string[,] _explanations = {
            {string.Empty, string.Empty, string.Empty, string.Empty}, {
                "The event period length is how long a single on/off event lasts.",
                "A sequence is made up of a series of events of all the same length.  At every event, the program updates the controller with new data if a change needs to be made."
                ,
                "The smaller the event period, the finer the control you over the timing of the events.  Generally, you don't want to have the event period be any shorter than you need it to be in order to synchronize to your selected audio."
                , "100 milliseconds, or 10 updates/second, is adequate for synchronizing to most audio."
            }, {
                "A profile contains information about channels and plugins.",
                "To reduce the otherwise repetitive task of setting up channels and plugins for every sequence you create.",
                "By selecting a profile, the channel count, channel names, channel colors, channel outputs, and plugin setup will all be done for you."
                ,
                "Profiles are not required, but can really help you quickly create new sequences.  Profiles can be created by hand or from existing sequences."
            }, {
                "A channel is an independently-controlled circuit of lights.",
                "For every area of your display that you want to control independently, you will want to create a channel.  Sometimes you may need multiple channels assigned to a specific area or structure to adequately meet power limitations."
                , "For every channel you define in your sequence there will be a row defined in the event grid.",
                "The channel count can be changed at any time.  If you try to reduce your channel count, you will be warned about the potential loss of data.  Channel names can be easily imported from a file by going to Sequence/Import, which also affects the channel count."
            },
            {"Names for the channels defined earlier.", "For easier identification of a channel's purpose and location.", string.Empty, string.Empty}, {
                "The output plugins are what Vixen uses to communicate with the controllers.  They translate the data sent from Vixen into a format that the controllers can understand."
                , "Without specifying at least one plugin, Vixen cannot communicate with any controller.", string.Empty,
                "If you select plugins to use with this sequence, they need to be setup before they can be used.  The plugins and their respective setup can be changed at any time.  While Vixen supports using multiple plugins simultaneously, setting up one is adequate in most installations."
            }, {
                "Here you can assign audio and create event patterns based on the music (or without music).  As the music plays, you tap a pattern on your keyboard on a channel-by-channel basis."
                ,
                "The sequence can be authored to be synchronized with audio.  The music would be played by Vixen at the same time the sequence is being executed.\nEvent patterns can help set up the initial event timings which can later be cleaned up with manual editing.  It's much easier to establish the initial synchronized timings this way than by hand."
                , "The event grid will be populated with the created events.  The events are on/off values only.",
                "The audio can be added or removed at any time during editing.  The length of the sequence can be auto-sized to exactly fit the music, or it can be longer.  The sequence length cannot be shorter than any associated music.\nDefining event patterns can also be used to mark events of significance in the music.  The event patterns can be recreated at any time by going to Sequences/Music."
            }, {
                "The length of the sequence in minutes and seconds.", string.Empty, string.Empty,
                "If there is audio assigned to a sequence, the sequence length cannot be shorter than the music length."
            }
        };

        private readonly Stack<int> _history;
        private readonly Preference2 _preferences;
        private bool _back;
        private bool _skip;

        public NewSequenceWizardDialog(Preference2 preferences) {
            InitializeComponent();
            Icon = Resources.VixenPlus;
            openFileDialog.InitialDirectory = Paths.SequencePath;
            tabControl.SelectedIndex = 0;
            _preferences = preferences;
            _eventSequence = new EventSequence(preferences);
            PopulateProfileList();
            string str;
            if ((str = preferences.GetString("DefaultProfile")).Length > 0) {
                comboBoxProfiles.SelectedIndex = comboBoxProfiles.Items.IndexOf(str);
            }
            _history = new Stack<int>();
            UpdateExplanations(0);
        }


        public EventSequence Sequence {
            get { return _eventSequence; }
        }


        private void buttonAssignAudio_Click(object sender, EventArgs e) {
            var integer = _preferences.GetInteger("SoundDevice");
            var dialog = new AudioDialog(_eventSequence, _preferences.GetBoolean("EventSequenceAutoSize"), integer);
            if (dialog.ShowDialog() == DialogResult.OK) {
                SetSequenceTime();
            }
            dialog.Dispose();
        }


        private void buttonImportChannels_Click(object sender, EventArgs e) {
            if (openFileDialog.ShowDialog() != DialogResult.OK) {
                return;
            }
            var sequence = new EventSequence(openFileDialog.FileName);
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
                buttonOK_Click(null,null);
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
            using (var dialog = new ProfileManagerDialog(null)) {
                if (dialog.ShowDialog() == DialogResult.OK) {
                    PopulateProfileList();
                }
            }
        }


        private void buttonSetupPlugins_Click(object sender, EventArgs e) {
            using (var dialog = new PluginListDialog(_eventSequence)) {
                dialog.ShowDialog();
            }
        }


        private void buttonSkip_Click(object sender, EventArgs e) {
            _skip = true;
            _back = false;
            tabControl.SelectedIndex++;
        }


        private void labelEffect_Click(object sender, EventArgs e) {
            if (labelEffect.Enabled) {
                labelExplanation.Text = _explanations[tabControl.SelectedIndex, 2];
            }
        }


        private void labelNotes_Click(object sender, EventArgs e) {
            if (labelNotes.Enabled) {
                labelExplanation.Text = _explanations[tabControl.SelectedIndex, 3];
            }
        }


        private void labelWhat_Click(object sender, EventArgs e) {
            if (labelWhat.Enabled) {
                labelExplanation.Text = _explanations[tabControl.SelectedIndex, 0];
            }
        }


        private void labelWhy_Click(object sender, EventArgs e) {
            if (labelWhy.Enabled) {
                labelExplanation.Text = _explanations[tabControl.SelectedIndex, 1];
            }
        }


        private int ParseTimeString(string text) {
            int num2;
            int num3;
            int num4;
            var s = "0";
            var str3 = "0";
            var index = text.IndexOf(':');
            if (index != -1) {
                s = text.Substring(0, index).Trim();
                text = text.Substring(index + 1);
            }
            index = text.IndexOf('.');
            if (index != -1) {
                str3 = text.Substring(index + 1).Trim();
                text = text.Substring(0, index);
            }
            var str2 = text;
            try {
                num2 = int.Parse(s);
            }
            catch {
                num2 = 0;
            }
            try {
                num3 = int.Parse(str2);
            }
            catch {
                num3 = 0;
            }
            try {
                num4 = int.Parse(str3);
            }
            catch {
                num4 = 0;
            }
            num4 = (num4 + (num3 * 1000)) + (num2 * 60000);
            if (num4 != 0) {
                return num4;
            }
            MessageBox.Show(Resources.InvalidTimeFormat, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return 0;
        }


        private void PopulateProfileList() {
            var selectedIndex = comboBoxProfiles.SelectedIndex;
            comboBoxProfiles.BeginUpdate();
            comboBoxProfiles.Items.Clear();
            comboBoxProfiles.Items.Add(Resources.None);
            foreach (var str in Directory.GetFiles(Paths.ProfilePath, @Vendor.All + Vendor.ProfileExtension)) {
                // ReSharper disable AssignNullToNotNullAttribute
                comboBoxProfiles.Items.Add(Path.GetFileNameWithoutExtension(str));
                // ReSharper restore AssignNullToNotNullAttribute
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


        private void tabControl_Deselecting(object sender, TabControlCancelEventArgs e) {
            if (!_back) {
                _history.Push(e.TabPageIndex);
                buttonPrev.Enabled = true;
            }
            if (_skip) {
                return;
            }
            var text = string.Empty;
            switch (e.TabPageIndex) {
                case 1: {
                    int num;
                    try {
                        num = Convert.ToInt32(textBoxEventPeriod.Text);
                        if (num < 25) {
                            text = Resources.EventPeriodTooShort;
                            goto Label_0339;
                        }
                    }
                    catch {
                        text = textBoxChannelCount.Text + Resources.EventPeriodInvalid;
                        goto Label_0339;
                    }
                    _eventSequence.EventPeriod = num;
                    goto Label_0339;
                }
                case 2:
                    _eventSequence.Profile = comboBoxProfiles.SelectedIndex == 0
                                                 ? null : new Profile(Path.Combine(Paths.ProfilePath, comboBoxProfiles.SelectedItem + ".pro"));
                    if (_eventSequence.Profile != null) {
                        var groupFile = Path.Combine(Paths.ProfilePath, comboBoxProfiles.SelectedItem + Vendor.GroupExtension);
                        if (File.Exists(groupFile)) {
                            _eventSequence.Groups = Group.LoadGroups(groupFile);
                        }
                    }
                    goto Label_0339;

                case 3: {
                    int num2;
                    try {
                        num2 = Convert.ToInt32(textBoxChannelCount.Text);
                        if (num2 < 1) {
                            text = Resources.ChannelCountMinimums;
                            goto Label_0339;
                        }
                    }
                    catch {
                        text = textBoxChannelCount.Text + Resources.InvalidChannelCount;
                        goto Label_0339;
                    }
                    if ((num2 > 1024) &&
                        (MessageBox.Show(string.Format(Resources.ConfirmChannelCount, num2), Vendor.ProductName, MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question) != DialogResult.Yes)) {
                        tabControl.TabIndex = 1;
                    }
                    else {
                        Cursor = Cursors.WaitCursor;
                        try {
                            _eventSequence.FullChannelCount = num2;
                        }
                        finally {
                            Cursor = Cursors.Default;
                        }
                    }
                    goto Label_0339;
                }
                case 4:
                    if (textBoxChannelNames.Lines.Length == Convert.ToInt32(textBoxChannelCount.Text)) {
                        if (textBoxChannelNames.Lines.Any(str2 => str2.Trim() == string.Empty)) {
                            text = Resources.ChannelNameCantBeBlank;
                        }
                        break;
                    }
                    text = Resources.ChannelCountAndNameInequal;
                    goto Label_0339;

                case 7: {
                    var num4 = ParseTimeString(textBoxTime.Text);
                    if (num4 != 0) {
                        try {
                            _eventSequence.Time = num4;
                        }
                        catch (Exception exception) {
                            text = exception.Message;
                        }
                    }
                    goto Label_0339;
                }
                default:
                    goto Label_0339;
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
            Label_0339:
            if (text == string.Empty) {
                return;
            }
            e.Cancel = true;
            MessageBox.Show(text, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }


        private void tabControl_Selected(object sender, TabControlEventArgs e) {
            switch (e.TabPageIndex) {
                case 1:
                    textBoxEventPeriod.Text = _eventSequence.EventPeriod.ToString(CultureInfo.InvariantCulture);
                    break;

                case 2:
                    if (_eventSequence.Profile != null) {
                        comboBoxProfiles.SelectedIndex = comboBoxProfiles.Items.IndexOf(_eventSequence.Profile.Name);
                        break;
                    }
                    comboBoxProfiles.SelectedIndex = 0;
                    break;

                case 4:
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

                case 7:
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


        private void UpdateExplanations(int pageIndex) {
            labelWhat.Enabled = _explanations[pageIndex, 0] != string.Empty;
            labelWhy.Enabled = _explanations[pageIndex, 1] != string.Empty;
            labelEffect.Enabled = _explanations[pageIndex, 2] != string.Empty;
            labelNotes.Enabled = _explanations[pageIndex, 3] != string.Empty;
            labelExplanation.Text = _explanations[pageIndex, 0];
        }
    }
}
