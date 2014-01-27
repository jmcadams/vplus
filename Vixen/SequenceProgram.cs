using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;

using VixenPlus.Properties;

public class SequenceProgram : IScheduledObject {
    private readonly ulong _key;
    private byte[][] _mask;
    private Profile _profile;


/*
    public SequenceProgram() {
        Loop = false;
        _profile = null;
        UseSequencePluginData = false;
        TreatAsLocal = false;
        UserData = null;
        _crossFadeLength = 0;
        _key = Host.GetUniqueKey();
        _mask = null;
        FileName = string.Empty;
        ConstructUsing();
        SetupData = new SetupData();
    }
*/


    public SequenceProgram(string fileName) {
        Loop = false;
        _profile = null;
        UseSequencePluginData = false;
        TreatAsLocal = false;
        UserData = null;
        CrossFadeLength = 0;
        _key = Host.GetUniqueKey();
        _mask = null;
        FileName = fileName;
        ConstructUsing();
        SetupData = new SetupData();
        LoadFromXml(Xml.LoadDocument(Path.Combine(Paths.ProgramPath, fileName)));
    }


    public SequenceProgram(EventSequence sequence) {
        Loop = false;
        _profile = null;
        UseSequencePluginData = false;
        TreatAsLocal = false;
        UserData = null;
        CrossFadeLength = 0;
        _key = Host.GetUniqueKey();
        _mask = null;
        FileName = sequence.FileName;
        ConstructUsing();
        SetupData = sequence.PlugInData;
        EventSequences.Add(new EventSequenceStub(sequence));
    }


    public int CrossFadeLength { get; private set; }

    /*
    public List<string> EventSequenceFileNames {
        get {
            return EventSequences.Select(stub => Path.GetFileName(stub.FileName)).ToList();
        }
    }
*/

    internal List<EventSequenceStub> EventSequences { get; private set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    private bool Loop { get; set; }

/*
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
*/

    public SetupData SetupData { get; private set; }

    public bool UseSequencePluginData { get; private set; }

    public int AudioDeviceIndex {
        get { return -1; }
    }

    public int AudioDeviceVolume {
        get { return 100; }
    }

    public bool CanBePlayed {
        get { return true; }
    }

    public List<Channel> Channels {
        get {
            return _profile == null ? new List<Channel>() : _profile.Channels;
        }
    }

    public List<Channel> FullChannels {
        get { return Channels; }
    } 

    public string FileName { get; private set; }

    public ulong Key {
        get { return _key; }
    }

/*
    public int Length {
        get {
            return EventSequences.Sum(stub => stub.Length);
        }
    }
*/

    public byte[][] Mask {
        get {
            if (_profile != null) {
                return _profile.Mask;
            }
            if (_mask != null) {
                return _mask;
            }
            _mask = new byte[EventSequences.Count][];
            for (var i = 0; i < EventSequences.Count; i++) {
                _mask[i] = EventSequences[i].Mask[0];
            }
            return _mask;
        }
        set {
            if (_profile != null) {
                return;
            }
            foreach (var t in EventSequences) {
                t.Mask[0] = value[0];
            }
        }
    }

    public string Name {
        get { return Path.GetFileNameWithoutExtension(FileName); }
/*
        set { FileName = Path.ChangeExtension(value, ".vpr"); }
*/
    }

/*
    public List<Channel> OutputChannels {
        get {
            return _profile == null ? new List<Channel>() : _profile.OutputChannels;
        }
    }
*/

    public SetupData PlugInData {
        get {
            return _profile == null ? SetupData : _profile.PlugInData;
        }
    }

    public bool TreatAsLocal { get; private set; }

    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    private object UserData { get; set; }


/*
    public void AddSequence(string sequenceFileName) {
        // ReSharper disable AssignNullToNotNullAttribute
        EventSequences.Add(new EventSequenceStub(Path.Combine(Paths.SequencePath, Path.GetFileName(sequenceFileName)), true));
        // ReSharper restore AssignNullToNotNullAttribute
    }
*/


/*
    public void AddSequence(EventSequence sequence) {
        EventSequences.Add(new EventSequenceStub(sequence));
    }
*/


    private void AttachToProfile(string profileName) {
        var path = Path.Combine(Paths.ProfilePath, profileName + ".pro");
        if (File.Exists(path)) {
            AttachToProfile(new Profile(path));
        }
        else {
            MessageBox.Show(Name + Resources.ProfileNotFound, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            LoadEmbeddedData(FileName);
        }
    }


    private void AttachToProfile(Profile profile) {
        _profile = profile;
        ReloadProfile();
    }


/*
    public void ClearSequences() {
        EventSequences.Clear();
    }
*/


    private void ConstructUsing() {
        EventSequences = new List<EventSequenceStub>();
    }


/*
    private void DetachFromProfile() {
        _profile = null;
        LoadEmbeddedData(FileName);
    }
*/


    public void Dispose() {
        foreach (var stub in EventSequences) {
            stub.Dispose();
        }
        GC.SuppressFinalize(this);
    }


    ~SequenceProgram() {
        Dispose();
    }


    private void LoadEmbeddedData(string fileName) {
        var document = new XmlDocument();
        document.Load(fileName);
        LoadEmbeddedData(document.SelectSingleNode("//Program"));
    }


    private void LoadEmbeddedData(XmlNode contextNode) {
        SetupData = new SetupData();
        SetupData.LoadFromXml(contextNode);
    }


    private void LoadFromXml(XmlNode contextNode) {
        var node = contextNode.SelectSingleNode("Program");
        if (node != null && node.Attributes != null && node.Attributes["useSequencePluginData"] != null) {
            UseSequencePluginData = bool.Parse(node.Attributes["useSequencePluginData"].Value);
        }
        EventSequences.Clear();
        if (node != null) {
            var sequenceNode = node.SelectNodes("Sequence");
            if (sequenceNode != null) {
                foreach (XmlNode node2 in sequenceNode) {
                    var path = Path.Combine(Paths.SequencePath, node2.InnerText);
                    if (File.Exists(path)) {
                        EventSequences.Add(new EventSequenceStub(path, true));
                    }
                    else {
                        node.RemoveChild(node2);
                    }
                }
            }
        }
        if (node != null) {
            var node3 = node.SelectSingleNode("Profile");
            if (node3 == null) {
                LoadEmbeddedData(node);
            }
            else {
                AttachToProfile(node3.InnerText);
            }
        }
        CrossFadeLength = int.Parse(Xml.GetNodeAlways(node, "CrossFadeLength", "0").InnerText);
    }


/*
    public void Refresh() {
        foreach (var stub in EventSequences) {
            if (string.IsNullOrEmpty(stub.FileName)) {
                throw new Exception(Resources.OneOrMoreSequencesNotSaved);
            }
            using (var sequence = new EventSequence(stub.FileName)) {
                stub.Name = sequence.Name;
                stub.Length = sequence.Time;
            }
        }
    }
*/


    private void ReloadProfile() {
        SetupData = _profile.PlugInData;
    }


/*
    public void SaveTo(string filePath) {
        var contextNode = Xml.CreateXmlDocument();
        SaveToXml(contextNode);
        contextNode.Save(filePath);
    }
*/


/*
    private void SaveToXml(XmlNode contextNode) {
        var emptyNodeAlways = Xml.GetEmptyNodeAlways(contextNode, "Program");
        if (UseSequencePluginData) {
            Xml.SetAttribute(emptyNodeAlways, "useSequencePluginData", UseSequencePluginData.ToString());
        }
        foreach (var stub in EventSequences) {
            if (string.IsNullOrEmpty(stub.FileName)) {
                throw new Exception(Resources.SequencesMustBeSavedBeforeSavingProgram);
            }
            Xml.SetNewValue(emptyNodeAlways, "Sequence", Path.GetFileName(stub.FileName));
        }
        if (_profile == null) {
            if (emptyNodeAlways.OwnerDocument != null) {
                emptyNodeAlways.AppendChild(emptyNodeAlways.OwnerDocument.ImportNode(SetupData.RootNode, true));
            }
        }
        else {
            Xml.SetValue(emptyNodeAlways, "Profile", _profile.Name);
        }
        Xml.SetValue(emptyNodeAlways, "CrossFadeLength", _crossFadeLength.ToString(CultureInfo.InvariantCulture));
    }
*/


    public override string ToString() {
        return Name;
    }
}