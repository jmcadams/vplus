using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;

using Properties;

namespace VixenPlus {
    public class EventSequence : IScheduledObject {
        private Audio _audio;
        private int _audioDeviceVolume;
        private int _channelWidth;
        private List<Channel> _channels;
        private EngineType _engineType;
        private int _eventPeriod;
        private byte[,] _eventValues;
        private SetupData _plugInData;
        private Profile _profile;
        private SortOrders _sortOrders;


        public EventSequence(string fileName) {
            _eventValues = null;
            _eventPeriod = 100;
            MinimumLevel = 0;
            MaximumLevel = 255;
            _audio = null;
            TotalEventPeriods = 0;
            WindowWidth = 0;
            WindowHeight = 0;
            _channelWidth = 0;
            _engineType = EngineType.Standard;
            _profile = null;
            TreatAsLocal = false;
            AudioDeviceIndex = -1;
            _audioDeviceVolume = 0;
            UserData = null;
            Key = Host.GetUniqueKey();
            var contextNode = new XmlDocument();
            contextNode.Load(fileName);
            FileName = fileName;
            LoadFromXml(contextNode);
        }


        public EventSequence(Preference2 preferences) {
            _eventValues = null;
            _eventPeriod = 100;
            MinimumLevel = 0;
            MaximumLevel = 255;
            _audio = null;
            TotalEventPeriods = 0;
            WindowWidth = 0;
            WindowHeight = 0;
            _channelWidth = 0;
            _engineType = EngineType.Standard;
            _profile = null;
            TreatAsLocal = false;
            AudioDeviceIndex = -1;
            _audioDeviceVolume = 0;
            UserData = null;
            Key = Host.GetUniqueKey();
            _channels = new List<Channel>();
            _plugInData = new SetupData();
            LoadableData = new LoadableData();
            _sortOrders = new SortOrders();
            Extensions = new SequenceExtensions();
            if (preferences != null) {
                _eventPeriod = preferences.GetInteger("EventPeriod");
                MinimumLevel = (byte) preferences.GetInteger("MinimumLevel");
                MaximumLevel = (byte) preferences.GetInteger("MaximumLevel");
                string profileName = preferences.GetString("DefaultProfile");
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


        public Audio Audio {
            get { return _audio; }
            set { _audio = value; }
        }

        public int ChannelCount {
            get {
                return _profile == null ? _channels.Count : _profile.Channels.Count;
            }
            set {
                while (_channels.Count > value) {
                    _channels.RemoveAt(value);
                }
                for (int i = _channels.Count + 1; _channels.Count < value; i++) {
                    _channels.Add(new Channel("Channel " + i.ToString(CultureInfo.InvariantCulture), i - 1, true));
                }
                UpdateEventValueArray();
                _sortOrders.UpdateChannelCounts(value);
            }
        }

        public int ChannelWidth {
            get { return _channelWidth; }
            set { _channelWidth = value; }
        }

        public EngineType EngineType {
            get { return _engineType; }
            set { _engineType = value; }
        }

        public int EventPeriod {
            get { return _eventPeriod; }
            set {
                _eventPeriod = value;
                UpdateEventValueArray(true);
            }
        }

        public double EventsPerSecond {
            get { return (_eventPeriod != 0) ? 1000 / _eventPeriod : 0; }
        }

        public byte[,] EventValues {
            get { return _eventValues; }
            set { _eventValues = value; }
        }

        public SequenceExtensions Extensions { get; private set; }

        public int LastSort {
            get {
                return _profile == null ? _sortOrders.LastSort : _profile.Sorts.LastSort;
            }
            set {
                if (_profile == null) {
                    _sortOrders.LastSort = value;
                }
                else {
                    _profile.Sorts.LastSort = value;
                }
            }
        }

        public LoadableData LoadableData { get; private set; }

        public byte MaximumLevel { get; set; }

        public byte MinimumLevel { get; set; }

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

        public SortOrders Sorts {
            get {
                return _profile == null ? _sortOrders : _profile.Sorts;
            }
        }

        public int Time {
            get { return Length; }
            set { SetTime(value); }
        }

        public int TotalEventPeriods { get; private set; }

        public int WindowHeight { get; set; }

        public int WindowWidth { get; set; }


        public void Dispose() {
            GC.SuppressFinalize(this);
        }


        public int AudioDeviceIndex { get; set; }

        public int AudioDeviceVolume {
            get { return _audioDeviceVolume; }
            set { _audioDeviceVolume = value; }
        }

        public bool CanBePlayed {
            get { return true; }
        }

        public List<Channel> Channels {
            get {
                return _profile == null ? _channels : _profile.Channels;
            }
            set { AssignChannelArray(value); }
        }

        public string FileName { get; set; }

        public ulong Key { get; private set; }

        public int Length { get; private set; }

        public byte[][] Mask {
            get {
                if (_profile == null) {
                    var buffer = new byte[_channels.Count];
                    for (int i = 0; i < _channels.Count; i++) {
                        buffer[i] = _channels[i].Enabled ? ((byte) 255) : ((byte) 0);
                    }
                    return new[] {buffer};
                }
                return _profile.Mask;
            }
            set {
                if (_profile == null) {
                    for (int i = 0; i < _channels.Count; i++) {
                        _channels[i].Enabled = value[0][i] == 255;
                    }
                }
            }
        }

        public string Name {
            get { return Path.GetFileNameWithoutExtension(FileName); }
            set {
                string extension = ".vix";
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
                    string str2 = string.IsNullOrEmpty(FileName) ? null : Path.GetDirectoryName(FileName);
                    FileName = Path.Combine(!string.IsNullOrEmpty(str2) ? str2 : Paths.SequencePath, value);
                }
            }
        }

        public List<Channel> OutputChannels {
            get {
                var list = new List<Channel>(_channels);
                foreach (Channel channel in _channels) {
                    list[channel.OutputChannel] = channel;
                }
                return list;
            }
        }

        public SetupData PlugInData {
            get { return _plugInData; }
            set { _plugInData = value; }
        }

        public bool TreatAsLocal { get; set; }

        public object UserData { get; set; }


        private void AssignChannelArray(List<Channel> channels) {
            _channels = channels;
            if (_channels.Count != _eventValues.GetLength(0)) {
                UpdateEventValueArray(true);
            }
            _sortOrders.UpdateChannelCounts(_channels.Count);
        }


        private void AttachToProfile(string profileName) {
            string path = Path.Combine(Paths.ProfilePath, profileName + ".pro");
            if (File.Exists(path)) {
                AttachToProfile(new Profile(path));
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
            int index = _channels.IndexOf(source);
            int num2 = _channels.IndexOf(dest);
            for (int i = 0; i < TotalEventPeriods; i++) {
                _eventValues[num2, i] = _eventValues[index, i];
            }
        }


        //TODO This is not working either.
        public void DeleteChannel(ulong channelId) {
            int index = Channels.IndexOf(FindChannel(channelId));
            Channels.RemoveAt(index);
            var buffer = new byte[ChannelCount,TotalEventPeriods];
            int num3 = 0;
            for (int i = 0; i < _eventValues.GetLength(0); i++) {
                if (i != index) {
                    for (int j = 0; j < TotalEventPeriods; j++) {
                        buffer[num3, j] = _eventValues[i, j];
                    }
                    num3++;
                }
            }
            _eventValues = buffer;
            _sortOrders.DeleteChannel(index);
        }


        private void DetachFromProfile() {
            LoadEmbeddedData(FileName);
            if (((_profile.Channels.Count > _channels.Count) && HasData()) &&
                (MessageBox.Show(Resources.IncreaseChannelCount, Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                 DialogResult.Yes)) {
                while (_channels.Count < _profile.Channels.Count) {
                    _channels.Add(_profile.Channels[_channels.Count]);
                }
            }
            _profile = null;
            UpdateEventValueArray();
        }


        private int ExtentOfAudio() {
            if (_audio != null) {
                return _audio.Duration;
            }
            return -2147483648;
        }


        public Channel FindChannel(ulong id) {
            return Channels.Find(c => c.Id == id);
        }


        private bool HasData() {
            for (int i = 0; i < _eventValues.GetLength(0); i++) {
                for (int j = 0; j < _eventValues.GetLength(1); j++) {
                    if (_eventValues[i, j] != 0) {
                        return true;
                    }
                }
            }
            return false;
        }


        //todo this is broken since there are not any channels showing up here...
        public int InsertChannel(int sortedIndex) {
            int count = LastSort >= 0 ? _channels.Count : sortedIndex;
            if (count > _channels.Count) {
                count = _channels.Count;
            }
            if (sortedIndex > _channels.Count) {
                sortedIndex = _channels.Count;
            }
            int outputChannel = count;
            foreach (Channel channel in _channels) {
                if (channel.OutputChannel >= outputChannel) {
                    channel.OutputChannel++;
                }
            }
            int num7 = sortedIndex + 1;
            _channels.Insert(count, new Channel("Channel " + num7.ToString(CultureInfo.InvariantCulture), outputChannel, true));
            var buffer = new byte[_channels.Count,TotalEventPeriods];
            for (int i = 0; i < _eventValues.GetLength(0); i++) {
                int num4 = (i >= count) ? (i + 1) : i;
                for (int j = 0; j < TotalEventPeriods; j++) {
                    buffer[num4, j] = _eventValues[i, j];
                }
            }
            _eventValues = buffer;
            _sortOrders.InsertChannel(count, sortedIndex);
            return count;
        }


        private void LoadEmbeddedData(string fileName) {
            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName)) {
                var document = new XmlDocument();
                document.Load(fileName);
                LoadEmbeddedData(document.SelectSingleNode("//Program"));
            }
            else {
                _plugInData = new SetupData();
            }
        }


        private void LoadEmbeddedData(XmlNode contextNode) {
            _channels.Clear();
            XmlNodeList xmlNodeList = contextNode.SelectNodes("Channels/Channel");
            if (xmlNodeList != null) {
                foreach (XmlNode node in xmlNodeList) {
                    _channels.Add(new Channel(node));
                }
            }
            _plugInData = new SetupData();
            _plugInData.LoadFromXml(contextNode);
            _sortOrders = new SortOrders();
            _sortOrders.LoadFromXml(contextNode);
        }


        private void LoadFromProfile() {
            _plugInData = _profile.PlugInData;
            UpdateEventValueArray();
        }


        private void LoadFromXml(XmlNode contextNode) {
            XmlNode requiredNode = Xml.GetRequiredNode(contextNode, "Program");
            _channels = new List<Channel>();
            _plugInData = new SetupData();
            LoadableData = new LoadableData();
            Extensions = new SequenceExtensions();
            _sortOrders = new SortOrders();
            XmlNode timeNode = requiredNode.SelectSingleNode("Time");
            if (timeNode != null) {
                Time = Convert.ToInt32(timeNode.InnerText);
            }
            XmlNode eventPeriodNode = requiredNode.SelectSingleNode("EventPeriodInMilliseconds");
            if (eventPeriodNode != null) {
                _eventPeriod = Convert.ToInt32(eventPeriodNode.InnerText);
            }
            XmlNode minLevelNode = requiredNode.SelectSingleNode("MinimumLevel");
            if (minLevelNode != null) {
                MinimumLevel = (byte) Convert.ToInt32(minLevelNode.InnerText);
            }
            XmlNode mnaxLevelNode = requiredNode.SelectSingleNode("MaximumLevel");
            if (mnaxLevelNode != null) {
                MaximumLevel = (byte) Convert.ToInt32(mnaxLevelNode.InnerText);
            }
            XmlNode audioDeviceNode = requiredNode.SelectSingleNode("AudioDevice");
            if (audioDeviceNode != null) {
                AudioDeviceIndex = int.Parse(audioDeviceNode.InnerText);
            }
            _audioDeviceVolume = int.Parse(Xml.GetNodeAlways(requiredNode, "AudioVolume", "100").InnerText);
            XmlNode node2 = requiredNode.SelectSingleNode("Profile");
            if (node2 == null) {
                LoadEmbeddedData(requiredNode);
            }
            else {
                AttachToProfile(node2.InnerText);
            }
            UpdateEventValueArray();
            XmlNode audioFileNode = requiredNode.SelectSingleNode("Audio");
            if (audioFileNode != null) {
                if (audioFileNode.Attributes != null) {
                    _audio = new Audio(audioFileNode.InnerText, audioFileNode.Attributes["filename"].Value,
                                       Convert.ToInt32(audioFileNode.Attributes["duration"].Value));
                }
            }
            int count = Channels.Count;
            XmlNode node4 = requiredNode.SelectSingleNode("EventValues");
            if (node4 != null) {
                byte[] buffer = Convert.FromBase64String(node4.InnerText);
                int num3 = 0;
                for (int i = 0; (i < count) && (num3 < buffer.Length); i++) {
                    for (int j = 0; (j < TotalEventPeriods) && (num3 < buffer.Length); j++) {
                        _eventValues[i, j] = buffer[num3++];
                    }
                }
            }
            XmlNode node5 = requiredNode.SelectSingleNode("WindowSize");
            if (node5 != null) {
                string[] strArray = node5.InnerText.Split(new[] {','});
                try {
                    WindowWidth = Convert.ToInt32(strArray[0]);
                }
                catch {
                    WindowWidth = 0;
                }
                try {
                    WindowHeight = Convert.ToInt32(strArray[1]);
                }
                catch {
                    WindowHeight = 0;
                }
            }
            node5 = requiredNode.SelectSingleNode("ChannelWidth");
            if (node5 != null) {
                try {
                    _channelWidth = Convert.ToInt32(node5.InnerText);
                }
                catch {
                    _channelWidth = 0;
                }
            }
            XmlNode node6 = requiredNode.SelectSingleNode("EngineType");
            if (node6 != null) {
                try {
                    _engineType = (EngineType) Enum.Parse(typeof (EngineType), node6.InnerText);
                }
                    // ReSharper disable EmptyGeneralCatchClause
                catch
                    // ReSharper restore EmptyGeneralCatchClause
                {}
            }
            LoadableData.LoadFromXml(requiredNode);
            Extensions.LoadFromXml(requiredNode);
        }


        public void ReloadProfile() {
            if (_profile != null) {
                _profile.Reload();
                LoadFromProfile();
            }
        }


        public void Save() {
            // ReSharper disable AssignNullToNotNullAttribute
            if (!Directory.Exists(Path.GetDirectoryName(FileName))) {
                throw new Exception(Resources.InvalidPath + FileName);
            }
            // ReSharper restore AssignNullToNotNullAttribute
            SaveTo(FileName);
        }


        public void SaveTo(string fileName) {
            SaveTo(fileName, true);
        }


        public void SaveTo(string fileName, bool setSequenceFileName) {
            XmlDocument contextNode = Xml.CreateXmlDocument();
            SaveToXml(contextNode);
            if (setSequenceFileName) {
                FileName = fileName;
            }
            contextNode.Save(fileName);
        }


        private void SaveToXml(XmlNode contextNode) {
            XmlDocument doc = contextNode.OwnerDocument ?? ((XmlDocument) contextNode);
            XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(contextNode, "Program");
            Xml.SetValue(emptyNodeAlways, "Time", Length.ToString(CultureInfo.InvariantCulture));
            Xml.SetValue(emptyNodeAlways, "EventPeriodInMilliseconds", _eventPeriod.ToString(CultureInfo.InvariantCulture));
            Xml.SetValue(emptyNodeAlways, "MinimumLevel", MinimumLevel.ToString(CultureInfo.InvariantCulture));
            Xml.SetValue(emptyNodeAlways, "MaximumLevel", MaximumLevel.ToString(CultureInfo.InvariantCulture));
            Xml.SetValue(emptyNodeAlways, "AudioDevice", AudioDeviceIndex.ToString(CultureInfo.InvariantCulture));
            Xml.SetValue(emptyNodeAlways, "AudioVolume", _audioDeviceVolume.ToString(CultureInfo.InvariantCulture));
            XmlNode node2 = Xml.GetEmptyNodeAlways(emptyNodeAlways, "Channels");
            foreach (Channel channel in _channels) {
                node2.AppendChild(channel.SaveToXml(doc));
            }
            if (emptyNodeAlways.OwnerDocument != null) {
                emptyNodeAlways.AppendChild(emptyNodeAlways.OwnerDocument.ImportNode(_plugInData.RootNode, true));
            }
            _sortOrders.SaveToXml(emptyNodeAlways);
            if (_profile != null) {
                Xml.SetValue(emptyNodeAlways, "Profile", _profile.Name);
            }
            if (_audio != null) {
                XmlNode node = Xml.SetNewValue(emptyNodeAlways, "Audio", _audio.Name);
                Xml.SetAttribute(node, "filename", _audio.FileName);
                Xml.SetAttribute(node, "duration", _audio.Duration.ToString(CultureInfo.InvariantCulture));
            }
            int count = Channels.Count;
            int totalEventPeriods = TotalEventPeriods;
            var inArray = new byte[count * totalEventPeriods];
            int num4 = 0;
            for (int i = 0; i < count; i++) {
                for (int j = 0; j < totalEventPeriods; j++) {
                    inArray[num4++] = _eventValues[i, j];
                }
            }
            Xml.GetNodeAlways(emptyNodeAlways, "EventValues").InnerText = Convert.ToBase64String(inArray);
            if (emptyNodeAlways.OwnerDocument != null) {
                emptyNodeAlways.AppendChild(emptyNodeAlways.OwnerDocument.ImportNode(LoadableData.RootNode, true));
            }
            Xml.SetValue(emptyNodeAlways, "EngineType", _engineType.ToString());
            if (emptyNodeAlways.OwnerDocument != null) {
                emptyNodeAlways.AppendChild(emptyNodeAlways.OwnerDocument.ImportNode(Extensions.RootNode, true));
            }
        }


        private void SetTime(int milliseconds) {
            if (milliseconds < ExtentOfAudio()) {
                throw new Exception(Resources.InvalidSequenceLength);
            }
            if ((_eventValues == null) || (milliseconds != (_eventValues.GetLength(1) * _eventPeriod))) {
                Length = milliseconds;
                UpdateEventValueArray();
            }
        }


        public override string ToString() {
            return Name;
        }


        private void UpdateEventValueArray(bool dataExtrapolation = false) {
            int length = 0;
            List<Channel> list = (_profile == null) ? _channels : _profile.Channels;
            if (_eventValues != null) {
                length = _eventValues.GetLength(0);
            }
            if (!dataExtrapolation) {
                byte[,] eventValues = _eventValues;
                _eventValues = new byte[list.Count,(int) Math.Ceiling(((Length) / ((float) _eventPeriod)))];
                if (eventValues != null) {
                    int num2 = Math.Min(eventValues.GetLength(1), _eventValues.GetLength(1));
                    int num3 = Math.Min(eventValues.GetLength(0), _eventValues.GetLength(0));
                    for (int i = 0; i < num3; i++) {
                        for (int j = 0; j < num2; j++) {
                            _eventValues[i, j] = eventValues[i, j];
                        }
                    }
                }
            }
            else {
                var buffer2 = new byte[list.Count,(int) Math.Ceiling(((Length) / ((float) _eventPeriod)))];
                if (((_eventValues != null) && (_eventValues.GetLength(0) != 0)) && (_eventValues.GetLength(1) != 0)) {
                    double num6 = (buffer2.GetLength(1)) / ((double) _eventValues.GetLength(1));
                    var num7 = (float) (1000.0 / (_eventPeriod * num6));
                    float num8 = 1000f / (_eventPeriod);
                    int num9 = buffer2.Length / list.Count;
                    int num10 = _eventValues.Length / list.Count;
                    float num12 = Math.Min(num7, num8);
                    float num13 = num7 / num12;
                    float num14 = num8 / num12;
                    var num15 = (int) Math.Min(((num10) / num13), ((num9) / num14));
                    int num19 = Math.Min(list.Count, _eventValues.GetLength(0));
                    for (int k = 0; k < num19; k++) {
                        for (float m = 0f; m < num15; m++) {
                            byte num18 = 0;
                            for (float n = 0f; n < num13; n++) {
                                num18 = Math.Max(num18, _eventValues[k, (int) ((m * num13) + n)]);
                            }
                            for (float num17 = 0f; num17 < num14; num17++) {
                                buffer2[k, (int) ((m * num14) + num17)] = num18;
                            }
                        }
                    }
                }
                _eventValues = buffer2;
            }
            TotalEventPeriods = _eventValues.GetLength(1);
            var allPlugIns = _plugInData.GetAllPluginData(SetupData.PluginType.Output);

            foreach (XmlNode node in allPlugIns) {
                if (node.Attributes != null && int.Parse(node.Attributes["from"].Value) > list.Count) {
                    node.Attributes["from"].Value = list.Count.ToString(CultureInfo.InvariantCulture);
                }
                if (node.Attributes != null) {
                    int num21 = int.Parse(node.Attributes["to"].Value);
                    if ((num21 == length) || (num21 > list.Count)) {
                        node.Attributes["to"].Value = list.Count.ToString(CultureInfo.InvariantCulture);
                    }
                }
            }
        }


        public void UpdateMetrics(int windowWidth, int windowHeight, int channelWidth) {
            var document = new XmlDocument();
            if (File.Exists(FileName) && ((File.GetAttributes(FileName) & FileAttributes.ReadOnly) == 0)) {
                document.Load(FileName);
                XmlNode contextNode = document.SelectSingleNode("//Program");
                Xml.SetValue(contextNode, "WindowSize", string.Format("{0},{1}", windowWidth, windowHeight));
                Xml.SetValue(contextNode, "ChannelWidth", channelWidth.ToString(CultureInfo.InvariantCulture));
                document.Save(FileName);
            }
        }
    }
}
