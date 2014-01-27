using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

using CommonUtils;

using VixenPlus.Properties;

public class EventSequence : IScheduledObject {
    private List<Channel> _fullChannels;
    private int _eventPeriod;
    private Profile _profile;
    private SortOrders _sortOrders;
    private string _currentGroup = "";

    public Dictionary<string, GroupData> Groups { get; set; }

    #region Constructors

    public EventSequence(string fileName) {
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
        UserData = null;
        Key = Host.GetUniqueKey();
        var contextNode = new XmlDocument();
        contextNode.Load(fileName);
        FileName = fileName;
        LoadFromXml(contextNode);
    }


    public EventSequence(Preference2 preferences) {
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
        UserData = null;
        Key = Host.GetUniqueKey();
        _fullChannels = new List<Channel>();
        Channels = new List<Channel>();
        PlugInData = new SetupData();
        LoadableData = new LoadableData();
        _sortOrders = new SortOrders();
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
        if (Audio != null) {
            return Audio.Duration;
        }
        return -2147483648;
    }



    public int ChannelWidth { get; private set; }
    public Audio Audio { get; set; }
    public EngineType EngineType { get; private set; }

    public int EventPeriod {
        get { return _eventPeriod; }
        set {
            _eventPeriod = value;
            UpdateEventValueArray(true);
        }
    }

    public double EventsPerSecond {
        get { return (_eventPeriod != 0) ? Utils.MillsPerSecond / _eventPeriod : 0; }
    }

    public byte[,] EventValues { get; private set; }

    public int Rows {
        get { return EventValues.GetLength(Utils.IndexRowsOrHeight); }
    }

    public int Cols {
        get { return EventValues.GetLength(Utils.IndexColsOrWidth); }
    }

    private SequenceExtensions Extensions { get; set; }
    private LoadableData LoadableData { get; set; }

    public byte MaximumLevel { get; set; }

    public byte MinimumLevel { get; set; }

    public int Time {
        get { return Length; }
        set { SetTime(value); }
    }

    public int TotalEventPeriods { get; private set; }

    public int WindowHeight { get; private set; }

    public int WindowWidth { get; private set; }


    public void Dispose() {
        GC.SuppressFinalize(this);
    }


    public int AudioDeviceIndex { get; set; }

    public int AudioDeviceVolume { get; private set; }

    public bool CanBePlayed {
        get { return true; }
    }

    public string FileName { get; private set; }

    public ulong Key { get; private set; }

    private int Length { get; set; }

    public byte[][] Mask {
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

    public string Name {
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

    public SetupData PlugInData { get; private set; }

    public bool TreatAsLocal { get; private set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    private object UserData { get; set; }


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


    public int InsertChannel(int sortedIndex) {
        var count = LastSort >= 0 ? _fullChannels.Count : sortedIndex;
        if (count > _fullChannels.Count) {
            count = _fullChannels.Count;
        }
        if (sortedIndex > _fullChannels.Count) {
            sortedIndex = _fullChannels.Count;
        }
        var outputChannel = count;
        foreach (var channel in _fullChannels.Where(channel => channel.OutputChannel >= outputChannel)) {
            channel.OutputChannel++;
        }
        _fullChannels.Insert(count, new Channel(Resources.Channel + " " + (_fullChannels.Count + 1), outputChannel, true));
        var newEventValues = new byte[_fullChannels.Count,TotalEventPeriods];
        for (var row = 0; row < Rows; row++) {
            var rowOffset = (row >= count) ? (row + 1) : row;
            for (var column = 0; column < TotalEventPeriods; column++) {
                newEventValues[rowOffset, column] = EventValues[row, column];
            }
        }
        EventValues = newEventValues;

        if (Groups != null) {
            foreach (var group in Groups) {
                var newChannels = new List<string>();
                foreach (var channel in group.Value.GroupChannels.Split(new[] {','})) {
                    var newChannel = channel;
                    int res;
                    if (int.TryParse(channel, out res)) {
                        if (res >= count) res++;
                        newChannel = res.ToString(CultureInfo.InvariantCulture);
                    }
                    newChannels.Add(newChannel);
                }
                group.Value.GroupChannels = string.Join(",", newChannels.ToArray());
            }
        }

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
        _profile.Reload();
        LoadFromProfile();
    }


/*
    public void Save() {
        // ReSharper disable AssignNullToNotNullAttribute
        if (!Directory.Exists(Path.GetDirectoryName(FileName))) {
            throw new Exception(Resources.InvalidPath + FileName);
        }
        // ReSharper restore AssignNullToNotNullAttribute
        SaveTo(FileName);
        if (Groups != null) {
            Group.SaveGroups(Groups, Profile != null ? Profile.FileName : FileName);
        }
    }
*/


    //TODO Need to ask if this is a 2.1 or 2.5 format before saving.
    public void SaveTo(string fileName, bool setSequenceFileName = true) {
        var contextNode = Xml.CreateXmlDocument();
        SaveToXml(contextNode);
        if (setSequenceFileName) {
            FileName = fileName;
        }
        contextNode.Save(fileName);
    }


    private void SaveToXml(XmlNode contextNode) {
        var doc = contextNode.OwnerDocument ?? ((XmlDocument) contextNode);
        var emptyNodeAlways = Xml.GetEmptyNodeAlways(contextNode, "Program");
        Xml.SetValue(emptyNodeAlways, "Time", Length.ToString(CultureInfo.InvariantCulture));
        Xml.SetValue(emptyNodeAlways, "EventPeriodInMilliseconds", _eventPeriod.ToString(CultureInfo.InvariantCulture));
        Xml.SetValue(emptyNodeAlways, "MinimumLevel", MinimumLevel.ToString(CultureInfo.InvariantCulture));
        Xml.SetValue(emptyNodeAlways, "MaximumLevel", MaximumLevel.ToString(CultureInfo.InvariantCulture));
        Xml.SetValue(emptyNodeAlways, "AudioDevice", AudioDeviceIndex.ToString(CultureInfo.InvariantCulture));
        Xml.SetValue(emptyNodeAlways, "AudioVolume", AudioDeviceVolume.ToString(CultureInfo.InvariantCulture));
        var node2 = Xml.GetEmptyNodeAlways(emptyNodeAlways, "Channels");
        foreach (var channel in _fullChannels) {
            node2.AppendChild(channel.SaveToXml(doc));
        }
        if (emptyNodeAlways.OwnerDocument != null) {
            emptyNodeAlways.AppendChild(emptyNodeAlways.OwnerDocument.ImportNode(PlugInData.RootNode, true));
        }
        _sortOrders.SaveToXml(emptyNodeAlways);
        if (_profile != null) {
            Xml.SetValue(emptyNodeAlways, "Profile", _profile.Name);
        }
        if (Audio != null) {
            var node = Xml.SetNewValue(emptyNodeAlways, "Audio", Audio.Name);
            Xml.SetAttribute(node, "filename", Audio.FileName);
            Xml.SetAttribute(node, "duration", Audio.Duration.ToString(CultureInfo.InvariantCulture));
        }
        var count = FullChannels.Count;
        var totalEventPeriods = TotalEventPeriods;
        var inArray = new byte[count * totalEventPeriods];
        var num4 = 0;
        for (var i = 0; i < count; i++) {
            for (var j = 0; j < totalEventPeriods; j++) {
                inArray[num4++] = EventValues[i, j];
            }
        }
        Xml.GetNodeAlways(emptyNodeAlways, "EventValues").InnerText = Convert.ToBase64String(inArray);
        if (emptyNodeAlways.OwnerDocument != null) {
            emptyNodeAlways.AppendChild(emptyNodeAlways.OwnerDocument.ImportNode(LoadableData.RootNode, true));
        }
        Xml.SetValue(emptyNodeAlways, "EngineType", EngineType.ToString());
        if (emptyNodeAlways.OwnerDocument != null) {
            emptyNodeAlways.AppendChild(emptyNodeAlways.OwnerDocument.ImportNode(Extensions.RootNode, true));
        }
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


    private void UpdateEventValueArray(bool dataExtrapolation = false) {
        var height = 0;
        var channels = (_profile == null) ? _fullChannels : _profile.Channels;
        if (EventValues != null) {
            height = Rows;
        }
        if (!dataExtrapolation) {
            var originalEvents = EventValues;
            EventValues = new byte[channels.Count,(int) Math.Ceiling(((Length) / ((float) _eventPeriod)))];
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
            var newEventValues = new byte[channels.Count,(int) Math.Ceiling(((Length) / ((float) _eventPeriod)))];
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
                        //for (var oldColumn = 0f; oldColumn < oldColumns; oldColumn++) {
                        newValue = Math.Max(newValue, EventValues[row, (int) (column * oldColumns)]);
                        //}
                        //for (var newColumn = 0f; newColumn < newColumns; newColumn++) {
                        newEventValues[row, (int) (column * newColumns)] = newValue;
                        //}
                    }
                }
            }
            EventValues = newEventValues;
        }

        TotalEventPeriods = Cols;
        ResetOutputPlugins(channels, height);
        ApplyGroupAndSort();
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

    private void LoadFromXml(XmlNode contextNode) {
        var requiredNode = Xml.GetRequiredNode(contextNode, "Program");
        _fullChannels = new List<Channel>();
        Channels = new List<Channel>();
        PlugInData = new SetupData();
        LoadableData = new LoadableData();
        Extensions = new SequenceExtensions();
        _sortOrders = new SortOrders();
        var timeNode = requiredNode.SelectSingleNode("Time");
        if (timeNode != null) {
            Time = Convert.ToInt32(timeNode.InnerText);
        }
        var eventPeriodNode = requiredNode.SelectSingleNode("EventPeriodInMilliseconds");
        if (eventPeriodNode != null) {
            _eventPeriod = Convert.ToInt32(eventPeriodNode.InnerText);
        }
        var minLevelNode = requiredNode.SelectSingleNode("MinimumLevel");
        if (minLevelNode != null) {
            MinimumLevel = (byte) Convert.ToInt32(minLevelNode.InnerText);
        }
        var mnaxLevelNode = requiredNode.SelectSingleNode("MaximumLevel");
        if (mnaxLevelNode != null) {
            MaximumLevel = (byte) Convert.ToInt32(mnaxLevelNode.InnerText);
        }
        var audioDeviceNode = requiredNode.SelectSingleNode("AudioDevice");
        if (audioDeviceNode != null) {
            AudioDeviceIndex = int.Parse(audioDeviceNode.InnerText);
        }
        AudioDeviceVolume = int.Parse(Xml.GetNodeAlways(requiredNode, "AudioVolume", "100").InnerText);
        var node2 = requiredNode.SelectSingleNode("Profile");
        if (node2 == null) {
            LoadEmbeddedData(requiredNode);
        }
        else {
            AttachToProfile(node2.InnerText);
        }

        UpdateEventValueArray();
        var audioFileNode = requiredNode.SelectSingleNode("Audio");
        if (audioFileNode != null) {
            if (audioFileNode.Attributes != null) {
                Audio = new Audio(audioFileNode.InnerText, audioFileNode.Attributes["filename"].Value,
                    Convert.ToInt32(audioFileNode.Attributes["duration"].Value));
            }
        }
        var count = FullChannels.Count;

        var node4 = requiredNode.SelectSingleNode("EventValues");
        if (node4 != null) {
            var buffer = Convert.FromBase64String(node4.InnerText);
            var index = 0;
            for (var row = 0; (row < count) && (index < buffer.Length); row++) {
                for (var column = 0; (column < TotalEventPeriods) && (index < buffer.Length); column++) {
                    EventValues[row, column] = buffer[index++];
                }
            }
        }
        var node5 = requiredNode.SelectSingleNode("WindowSize");
        if (node5 != null) {
            var strArray = node5.InnerText.Split(new[] {','});
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
                ChannelWidth = Convert.ToInt32(node5.InnerText);
            }
            catch {
                ChannelWidth = 0;
            }
        }
        var node6 = requiredNode.SelectSingleNode("EngineType");
        if (node6 != null) {
            try {
                EngineType = (EngineType) Enum.Parse(typeof (EngineType), node6.InnerText);
            }
                // ReSharper disable EmptyGeneralCatchClause
            catch
                // ReSharper restore EmptyGeneralCatchClause
            {}
        }
        LoadableData.LoadFromXml(requiredNode);
        Extensions.LoadFromXml(requiredNode);

        ApplyGroupAndSort();
    }


    private void LoadEmbeddedData(XmlNode contextNode) {
        _fullChannels.Clear();
        var xmlNodeList = contextNode.SelectNodes("Channels/Channel");
        if (xmlNodeList != null) {
            foreach (XmlNode node in xmlNodeList) {
                _fullChannels.Add(new Channel(node));
            }
        }
        PlugInData = new SetupData();
        PlugInData.LoadFromXml(contextNode);
        _sortOrders = new SortOrders();
        _sortOrders.LoadFromXml(contextNode);
        var groupFile = Path.Combine(Paths.SequencePath, Path.GetFileNameWithoutExtension(FileName) + Vendor.GroupExtension);
        if (File.Exists(groupFile)) {
            Groups = Group.LoadGroups(groupFile);
        }
    }


    public int ChannelCount {
        get { return Channels.Count == 0 ? _fullChannels.Count : Channels.Count; }
/*
        set {
            while (_groupedAndSortedChannels.Count > value) {
                _groupedAndSortedChannels.RemoveAt(value);
            }
            for (var i = _groupedAndSortedChannels.Count + 1; _groupedAndSortedChannels.Count < value; i++) {
                _groupedAndSortedChannels.Add(new Channel(Resources.Channel + @" " + i.ToString(CultureInfo.InvariantCulture), i - 1, true));
            }
            UpdateEventValueArray();
            _sortOrders.UpdateChannelCounts(value);
        }
*/
    }


    public int FullChannelCount {
        get { return _profile == null ? _fullChannels.Count : _profile.Channels.Count; }
        set {
            while (_fullChannels.Count > value) {
                _fullChannels.RemoveAt(value);
            }
            for (var i = _fullChannels.Count + 1; _fullChannels.Count < value; i++) {
                _fullChannels.Add(new Channel(Resources.Channel + @" " + i.ToString(CultureInfo.InvariantCulture), i - 1, true));
            }
            UpdateEventValueArray();
            _sortOrders.UpdateChannelCounts(value);
        }
    }


    public int LastSort {
        get { return _profile == null ? _sortOrders.LastSort : _profile.Sorts.LastSort; }
        set {
            if (_profile == null) {
                _sortOrders.LastSort = value;
            }
            else {
                _profile.Sorts.LastSort = value;
            }
            ApplyGroupAndSort();
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


    public SortOrders Sorts {
        get { return _profile == null ? _sortOrders : _profile.Sorts; }
    }


    public string CurrentGroup {
        get { return _currentGroup; }
        set {
            if (_currentGroup == value) return;
            _currentGroup = value;
            ApplyGroupAndSort();
        }
    }


    public void ApplyGroupAndSort() {
        Channels = new List<Channel>();

        if (_currentGroup != Group.AllChannels && _currentGroup != "") {
            Channels = new Group().GetGroupChannels(_currentGroup, Groups, FullChannels);
        }
        else {
            Channels = FullChannels;
        }

        if (LastSort == -1) {
            return;
        }

        var currentOrder = Sorts.CurrentOrder;

        if (currentOrder == null || FullChannelCount !=currentOrder.ChannelIndexes.Count) {
            var msg = currentOrder == null
                ? "The sort order referenced does not exist.\n" +
                  "Please edit your sequnce or profile to make sure you have the correct number of sort orders defined."
                : "The selected channel order channel count does not match the sequence channel count and cannot be used.\n" +
                  "Your sequence channels will not be sorted using this channel order.";
            MessageBox.Show(msg, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        Channels = (from channel in currentOrder.ChannelIndexes
            where Channels.Contains(FullChannels[channel])
            select FullChannels[channel]).ToList();
    }


    public List<Channel> Channels { get; private set; }


    public List<Channel> FullChannels {
        get { return _profile == null ? _fullChannels : _profile.Channels; }
        set {
            AssignChannelArray(value);
            ApplyGroupAndSort();
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
        _sortOrders.UpdateChannelCounts(_fullChannels.Count);
    }


    private void AttachToProfile(string profileName) {
        var path = Path.Combine(Paths.ProfilePath, profileName + Vendor.ProfilExtension);
        if (File.Exists(path)) {
            AttachToProfile(new Profile(path));
            var groupFile = Path.Combine(Paths.ProfilePath, Path.GetFileNameWithoutExtension(_profile.FileName) + Vendor.GroupExtension);
            if (File.Exists(groupFile)) {
                Groups = Group.LoadGroups(groupFile);
            }
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

        var buffer = new byte[FullChannelCount,TotalEventPeriods];
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

        if (Groups != null) {
            foreach (var group in Groups) {
                var newChannels = new List<string>();
                foreach (var channel in group.Value.GroupChannels.Split(new[] { ',' })) {
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
                group.Value.GroupChannels = string.Join(",", newChannels.ToArray());
            }
        }
        _sortOrders.DeleteChannel(index);
    }


    private void DetachFromProfile() {
        LoadEmbeddedData(FileName);
        if (((_profile.Channels.Count > _fullChannels.Count) && HasData()) &&
            (MessageBox.Show(Resources.IncreaseChannelCount, Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
             DialogResult.Yes)) {
            while (_fullChannels.Count < _profile.Channels.Count) {
                _fullChannels.Add(_profile.Channels[_fullChannels.Count]);
            }
        }
        _profile = null;
        UpdateEventValueArray();
    }
}