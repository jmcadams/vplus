using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

using VixenPlus.Properties;

using VixenPlusCommon;

namespace VixenPlus {
    public sealed class EventSequence : ExecutableBase {
        private List<Channel> _fullChannels;
        private int _eventPeriod;
        private Profile _profile;
        private string _currentGroup = "";

        private Dictionary<string, GroupData> _groups;

        public Dictionary<string, GroupData> Groups {
            get { return _groups ?? new Dictionary<string, GroupData>(); }
            set { _groups = value; }
        }

        //public bool IsDirty { get; private set; }

        #region Constructors

        public EventSequence() {
            EventValues = null;
            _eventPeriod = 100;
            MinimumLevel = 0;
            MaximumLevel = 255;
            Audio = null;
            TotalEventPeriods = 0;
            WindowWidth = 0;
            WindowHeight = 0;
            ChannelWidth = 0;
            EngineType = EngineType.Standard;
            _profile = null;
            TreatAsLocal = false;
            AudioDeviceIndex = -1;
            AudioDeviceVolume = 0;
            Key = Host.GetUniqueKey();
        }

        public EventSequence(Preference2 preferences) : this() {
            _fullChannels = new List<Channel>();
            Channels = new List<Channel>();
            PlugInData = new SetupData();
            Extensions = new SequenceExtensions();
            if (preferences != null) {
                _eventPeriod = preferences.GetInteger("EventPeriod");
                MinimumLevel = (byte) preferences.GetInteger("MinimumLevel");
                MaximumLevel = (byte) preferences.GetInteger("MaximumLevel");
                var profileName = preferences.GetString("DefaultProfile");
                if (profileName.Length > 0) {
                    AttachToProfile(profileName);
                }
                AudioDeviceIndex = preferences.GetInteger("DefaultSequenceAudioDevice");
            }
            else {
                _eventPeriod = 100;
                MinimumLevel = 0;
                MaximumLevel = 255;
                AudioDeviceIndex = -1;
            }
            Time = 60000;
        }

        #endregion


        #region nonChannelStuff

        private int ExtentOfAudio() {
            return Audio != null ? Audio.Duration : Int32.MinValue;
        }


        public int ChannelWidth { get; set; }
        public Audio Audio { get; set; }
        public EngineType EngineType { get; set; }

        public int EventPeriod {
            get { return _eventPeriod; }
            set {
                if (_eventPeriod == value) {
                    return;
                }

                _eventPeriod = value;
                UpdateEventValueArray(true);
            }
        }

        public double EventsPerSecond {
            get { return (_eventPeriod != 0) ? Utils.MillsPerSecond / _eventPeriod : 0; }
        }

        public byte[,] EventValues { get; private set; }

        public int Rows {
            get { return  EventValues == null ? 0 : EventValues.GetLength(Utils.IndexRowsOrHeight); }
        }

        public int Cols {
            get { return EventValues == null ? 0 : EventValues.GetLength(Utils.IndexColsOrWidth); }
        }

        public SequenceExtensions Extensions { get; set; }
        public LoadableData LoadableData { get; set; }

        public byte MaximumLevel { get; set; }

        public byte MinimumLevel { get; set; }

        public int Time {
            get { return Length; }
            set { SetTime(value); }
        }

        public int TotalEventPeriods { get; private set; }

        public int WindowHeight { get; set; }

        public int WindowWidth { get; set; }

        public override ulong Key { get; set; }

        public int Length { get; private set; }

        public override byte[][] Mask {
            get {
                if (_profile != null) {
                    return _profile.Mask;
                }
                var buffer = new byte[_fullChannels.Count];
                for (var i = 0; i < _fullChannels.Count; i++) {
                    buffer[i] = _fullChannels[i].Enabled ? ((byte) 255) : ((byte) 0);
                }
                return new[] {buffer};
            }
            set {
                if (_profile != null) {
                    return;
                }
                for (var i = 0; i < _fullChannels.Count; i++) {
                    _fullChannels[i].Enabled = value[0][i] == 255;
                }
            }
        }

        public override string Name {
            get { return Path.GetFileNameWithoutExtension(FileName); }
            set {
                var extension = Vendor.SequenceExtension;
                if (!string.IsNullOrEmpty(FileName)) {
                    extension = Path.GetExtension(FileName);
                }
                else if (Path.HasExtension(value)) {
                    extension = Path.GetExtension(value);
                }
                if (extension != null) {
                    value = Path.ChangeExtension(value, extension.ToLower());
                }
                if (Path.IsPathRooted(value)) {
                    FileName = value;
                }
                else {
                    var str2 = string.IsNullOrEmpty(FileName) ? null : Path.GetDirectoryName(FileName);
                    FileName = Path.Combine(!string.IsNullOrEmpty(str2) ? str2 : Paths.SequencePath, value);
                }
            }
        }

        public override SetupData PlugInData { get; set; }

        private bool HasData() {
            for (var row = 0; row < Rows; row++) {
                for (var column = 0; column < Cols; column++) {
                    if (EventValues[row, column] != 0) {
                        return true;
                    }
                }
            }
            return false;
        }


        public void InsertChannel() {
            var count = _fullChannels.Count;
            var outputChannel = count;
            foreach (var channel in _fullChannels.Where(channel => channel.OutputChannel >= outputChannel)) {
                channel.OutputChannel++;
            }
            _fullChannels.Insert(count, new Channel(Resources.Channel + " " + (_fullChannels.Count + 1), outputChannel));
            var newEventValues = new byte[_fullChannels.Count, TotalEventPeriods];
            for (var row = 0; row < Rows; row++) {
                var rowOffset = (row >= count) ? (row + 1) : row;
                for (var column = 0; column < TotalEventPeriods; column++) {
                    newEventValues[rowOffset, column] = EventValues[row, column];
                }
            }
            EventValues = newEventValues;

            if (Groups == null) {
                return;
            }

            foreach (var g in Groups) {
                var newChannels = new List<string>();
                foreach (var channel in g.Value.GroupChannels.Split(',')) {
                    var newChannel = channel;
                    int res;
                    if (int.TryParse(channel, out res)) {
                        if (res >= count) res++;
                        newChannel = res.ToString(CultureInfo.InvariantCulture);
                    }
                    newChannels.Add(newChannel);
                }
                g.Value.GroupChannels = string.Join(",", newChannels.ToArray());
            }
        }


        private void LoadEmbeddedData(string fileName) {
            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName)) {
                var document = new XmlDocument();
                document.Load(fileName);
                LoadEmbeddedData(document.SelectSingleNode("//Program"));
            }
            else {
                PlugInData = new SetupData();
            }
        }


        private void LoadFromProfile() {
            PlugInData = _profile.PlugInData;
            UpdateEventValueArray();
        }


        public void ReloadProfile() {
            if (_profile == null) {
                return;
            }
            _profile = FileIOHandler.OpenProfile(_profile.FileName);
            LoadFromProfile();
        }


        private void SetTime(int milliseconds) {
            if (milliseconds < ExtentOfAudio()) {
                throw new Exception(Resources.InvalidSequenceLength);
            }
            if ((EventValues != null) && (milliseconds == (EventValues.GetLength(1) * _eventPeriod))) {
                return;
            }
            Length = milliseconds;
            UpdateEventValueArray();
        }


        public override string ToString() {
            return Name;
        }


        public void UpdateEventValueArray(bool dataExtrapolation = false) {
            var height = 0;
            var channels = (_profile == null) ? _fullChannels : _profile.Channels;
            if (EventValues != null) {
                height = Rows;
            }
            if (!dataExtrapolation) {
                var originalEvents = EventValues;
                EventValues = new byte[channels.Count, (int) Math.Ceiling(((Length) / ((float) _eventPeriod)))];
                if (originalEvents != null) {
                    var columns = Math.Min(originalEvents.GetLength(Utils.IndexColsOrWidth), Cols);
                    var rows = Math.Min(originalEvents.GetLength(Utils.IndexRowsOrHeight), Rows);
                    for (var row = 0; row < rows; row++) {
                        for (var column = 0; column < columns; column++) {
                            EventValues[row, column] = originalEvents[row, column];
                        }
                    }
                }
            }
            else {
                var newEventValues = new byte[channels.Count, (int) Math.Ceiling(((Length) / ((float) _eventPeriod)))];
                if (((EventValues != null) && (Rows != 0)) && (Cols != 0)) {
                    var eventCountRatio = (newEventValues.GetLength(Utils.IndexColsOrWidth)) / ((double) Cols);

                    var oldEventsPerSecond = (float) (Utils.MillsPerSecond / (_eventPeriod * eventCountRatio));
                    var newEventsPerSecond = (float) Utils.MillsPerSecond / (_eventPeriod);
                    var eventPerSecond = Math.Min(oldEventsPerSecond, newEventsPerSecond);

                    var oldEventCount = EventValues.Length / channels.Count;
                    var newEventCount = newEventValues.Length / channels.Count;

                    var oldColumns = oldEventsPerSecond / eventPerSecond;
                    var newColumns = newEventsPerSecond / eventPerSecond;

                    var columns = (int) Math.Min(((oldEventCount) / oldColumns), ((newEventCount) / newColumns));
                    var rows = Math.Min(channels.Count, Rows);
                    for (var row = 0; row < rows; row++) {
                        for (var column = 0f; column < columns; column++) {
                            byte newValue = 0;
                            if (oldEventCount < newEventCount) {
                                for (var oldColumn = 0f; oldColumn < oldColumns; oldColumn++) {
                                    newValue = Math.Max(newValue, EventValues[row, (int) ((column * oldColumns) + oldColumn)]);
                                }
                                for (var newColumn = 0f; newColumn < newColumns; newColumn++) {
                                    newEventValues[row, (int) ((column * newColumns) + newColumn)] = newValue;
                                }
                            }
                            else {
                                newEventValues[row, (int) (column * newColumns)] = Math.Max(newValue, EventValues[row, (int) (column * oldColumns)]);
                            }
                        }
                    }
                }
                EventValues = newEventValues;
            }

            TotalEventPeriods = Cols;
            ResetOutputPlugins(channels, height);
            ApplyGroup();
        }


        private void ResetOutputPlugins(ICollection channels, int height) {
            var outputPlugins = PlugInData.GetAllPluginData(SetupData.PluginType.Output);

            foreach (XmlNode node in outputPlugins) {
                if (node.Attributes != null && int.Parse(node.Attributes["from"].Value) > channels.Count) {
                    node.Attributes["from"].Value = channels.Count.ToString(CultureInfo.InvariantCulture);
                }
                if (node.Attributes == null) {
                    continue;
                }
                var lastChannel = int.Parse(node.Attributes["to"].Value);
                if ((lastChannel == height) || (lastChannel > channels.Count)) {
                    node.Attributes["to"].Value = channels.Count.ToString(CultureInfo.InvariantCulture);
                }
            }
        }


        public void UpdateMetrics(int windowWidth, int windowHeight, int channelWidth) {
            var document = new XmlDocument();
            if (!File.Exists(FileName) || ((File.GetAttributes(FileName) & FileAttributes.ReadOnly) != 0)) {
                return;
            }
            document.Load(FileName);
            var contextNode = document.SelectSingleNode("//Program");
            Xml.SetValue(contextNode, "WindowSize", string.Format("{0},{1}", windowWidth, windowHeight));
            Xml.SetValue(contextNode, "ChannelWidth", channelWidth.ToString(CultureInfo.InvariantCulture));
            document.Save(FileName);
        }

        #endregion

        public void LoadEmbeddedData(XmlNode contextNode) {
            _fullChannels.Clear();
            var xmlNodeList = contextNode.SelectNodes("Channels/Channel");
            if (xmlNodeList != null) {
                foreach (XmlNode node in xmlNodeList) {
                    _fullChannels.Add(new Channel(node));
                }
            }
            PlugInData = new SetupData();
            PlugInData.LoadFromXml(contextNode);
            Groups = Group.LoadFromXml(contextNode) ?? new Dictionary<string, GroupData>();
            Group.LoadFromFile(contextNode, Groups);
        }


        public int ChannelCount {
            get { return Channels.Count == 0 ? _fullChannels.Count : Channels.Count; }
        }


        public int FullChannelCount {
            get { return _profile == null ? _fullChannels.Count : _profile.Channels.Count; }
            set {
                while (_fullChannels.Count > value) {
                    _fullChannels.RemoveAt(value);
                }
                for (var i = _fullChannels.Count + 1; _fullChannels.Count < value; i++) {
                    _fullChannels.Add(new Channel(Resources.Channel + @" " + i.ToString(CultureInfo.InvariantCulture), i - 1));
                }
                UpdateEventValueArray();
            }
        }


        public Profile Profile {
            get { return _profile; }
            set {
                if ((value == null) && (_profile != null)) {
                    DetachFromProfile();
                }
                else if (_profile != value) {
                    AttachToProfile(value);
                }
            }
        }


        public string CurrentGroup {
            get { return _currentGroup; }
            set {
                _currentGroup = value;
                ApplyGroup();
            }
        }


        public void ApplyGroup() {
            Channels = new List<Channel>();

            if (_currentGroup != Group.AllChannels && _currentGroup != "") {
                Channels = new Group().GetGroupChannels(_currentGroup, Groups, FullChannels);
            }
            else {
                Channels = FullChannels;
            }
        }


        public override List<Channel> Channels { get; set; }


        public override List<Channel> FullChannels {
            get { return _profile == null ? _fullChannels : _profile.Channels; }
            set {
                AssignChannelArray(value);
                ApplyGroup();
            }
        }


        public List<Channel> OutputChannels {
            get {
                var list = new List<Channel>(_fullChannels);
                foreach (var channel in _fullChannels) {
                    list[channel.OutputChannel] = channel;
                }
                return list;
            }
        }


        private void AssignChannelArray(List<Channel> channels) {
            _fullChannels = channels;
            if (_fullChannels.Count != Rows) {
                UpdateEventValueArray(true);
            }
        }


        public void AttachToProfile(string profileName) {
            var path = Path.Combine(Paths.ProfilePath, profileName + Vendor.ProfileExtension);
            if (File.Exists(path)) {
                AttachToProfile(FileIOHandler.OpenProfile(path));
                Groups = _profile.Groups;
            }
            else {
                LoadEmbeddedData(FileName);
            }
        }


        private void AttachToProfile(Profile profile) {
            _profile = profile;
            _profile.Freeze();
            LoadFromProfile();
        }


        public void CopyChannel(Channel source, Channel dest) {
            var index = _fullChannels.IndexOf(source);
            var num2 = _fullChannels.IndexOf(dest);
            for (var i = 0; i < TotalEventPeriods; i++) {
                EventValues[num2, i] = EventValues[index, i];
            }
        }


        public void DeleteChannel(int index) {
            var currOutputChannel = Channels[index].OutputChannel;
            Channels.RemoveAt(index);

            foreach (var channel in _fullChannels.Where(channel => channel.OutputChannel >= currOutputChannel)) {
                channel.OutputChannel--;
            }

            var buffer = new byte[FullChannelCount, TotalEventPeriods];
            var newRow = 0;
            for (var row = 0; row < Rows; row++) {
                if (row == index) {
                    continue;
                }
                for (var col = 0; col < Cols; col++) {
                    buffer[newRow, col] = EventValues[row, col];
                }
                newRow++;
            }
            EventValues = buffer;

            if (Groups == null) {
                return;
            }

            foreach (var g in Groups) {
                var newChannels = new List<string>();
                foreach (var channel in g.Value.GroupChannels.Split(',')) {
                    int res;
                    if (int.TryParse(channel, out res)) {
                        if (res == index) {
                            continue;
                        }
                        if (res >= index) res--;
                        newChannels.Add(res.ToString(CultureInfo.InvariantCulture));
                    }
                    else {
                        newChannels.Add(channel);
                    }
                }
                g.Value.GroupChannels = string.Join(",", newChannels.ToArray());
            }
        }


        private void DetachFromProfile() {
            LoadEmbeddedData(FileName);
            _fullChannels.Clear();
            _fullChannels.AddRange(_profile.FullChannels);
            _profile = null;
            UpdateEventValueArray();
        }
    }
}
