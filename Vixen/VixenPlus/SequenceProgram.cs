﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;

using Properties;

namespace VixenPlus {
    public class SequenceProgram : IScheduledObject {
        private readonly ulong _key;
        private int _crossFadeLength;
        private byte[][] _mask;
        private Profile _profile;
        private SetupData _setupData;
        private bool _useSequencePluginData;


        public SequenceProgram() {
            Loop = false;
            _profile = null;
            _useSequencePluginData = false;
            TreatAsLocal = false;
            UserData = null;
            _crossFadeLength = 0;
            _key = Host.GetUniqueKey();
            _mask = null;
            FileName = string.Empty;
            ConstructUsing();
            _setupData = new SetupData();
        }


        public SequenceProgram(string fileName) {
            Loop = false;
            _profile = null;
            _useSequencePluginData = false;
            TreatAsLocal = false;
            UserData = null;
            _crossFadeLength = 0;
            _key = Host.GetUniqueKey();
            _mask = null;
            FileName = fileName;
            ConstructUsing();
            _setupData = new SetupData();
            LoadFromXml(Xml.LoadDocument(Path.Combine(Paths.ProgramPath, fileName)));
        }


        public SequenceProgram(EventSequence sequence) {
            Loop = false;
            _profile = null;
            _useSequencePluginData = false;
            TreatAsLocal = false;
            UserData = null;
            _crossFadeLength = 0;
            _key = Host.GetUniqueKey();
            _mask = null;
            FileName = sequence.FileName;
            ConstructUsing();
            _setupData = sequence.PlugInData;
            EventSequences.Add(new EventSequenceStub(sequence));
        }


        public int CrossFadeLength {
            get { return _crossFadeLength; }
            set { _crossFadeLength = value; }
        }

        public List<string> EventSequenceFileNames {
            get {
                var list = new List<string>();
                foreach (EventSequenceStub stub in EventSequences) {
                    list.Add(Path.GetFileName(stub.FileName));
                }
                return list;
            }
        }

        internal List<EventSequenceStub> EventSequences { get; private set; }

        public bool Loop { get; set; }

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

        public SetupData SetupData {
            get { return _setupData; }
            set { _setupData = value; }
        }

        public bool UseSequencePluginData {
            get { return _useSequencePluginData; }
            set { _useSequencePluginData = value; }
        }


        public void Dispose() {
            Dispose(true);
        }


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

        public string FileName { get; private set; }

        public ulong Key {
            get { return _key; }
        }

        public int Length {
            get {
                int num = 0;
                foreach (EventSequenceStub stub in EventSequences) {
                    num += stub.Length;
                }
                return num;
            }
        }

        public byte[][] Mask {
            get {
                if (_profile == null) {
                    if (_mask == null) {
                        _mask = new byte[EventSequences.Count][];
                        for (int i = 0; i < EventSequences.Count; i++) {
                            _mask[i] = EventSequences[i].Mask[0];
                        }
                    }
                    return _mask;
                }
                return _profile.Mask;
            }
            set {
                if (_profile == null) {
                    for (int i = 0; i < EventSequences.Count; i++) {
                        EventSequences[i].Mask[0] = value[0];
                    }
                }
            }
        }

        public string Name {
            get { return Path.GetFileNameWithoutExtension(FileName); }
            set { FileName = Path.ChangeExtension(value, ".vpr"); }
        }

        public List<Channel> OutputChannels {
            get {
                return _profile == null ? new List<Channel>() : _profile.OutputChannels;
            }
        }

        public SetupData PlugInData {
            get {
                return _profile == null ? _setupData : _profile.PlugInData;
            }
        }

        public bool TreatAsLocal { get; set; }

        public object UserData { get; set; }


        public void AddSequence(string sequenceFileName) {
            // ReSharper disable AssignNullToNotNullAttribute
            EventSequences.Add(new EventSequenceStub(Path.Combine(Paths.SequencePath, Path.GetFileName(sequenceFileName)), true));
            // ReSharper restore AssignNullToNotNullAttribute
        }


        public void AddSequence(EventSequence sequence) {
            EventSequences.Add(new EventSequenceStub(sequence));
        }


        private void AttachToProfile(string profileName) {
            string path = Path.Combine(Paths.ProfilePath, profileName + ".pro");
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


        public void ClearSequences() {
            EventSequences.Clear();
        }


        private void ConstructUsing() {
            EventSequences = new List<EventSequenceStub>();
        }


        private void DetachFromProfile() {
            _profile = null;
            LoadEmbeddedData(FileName);
        }


        public void Dispose(bool disposing) {
            foreach (EventSequenceStub stub in EventSequences) {
                stub.Dispose();
            }
            GC.SuppressFinalize(this);
        }


        ~SequenceProgram() {
            Dispose(false);
        }


        private void LoadEmbeddedData(string fileName) {
            var document = new XmlDocument();
            document.Load(fileName);
            LoadEmbeddedData(document.SelectSingleNode("//Program"));
        }


        private void LoadEmbeddedData(XmlNode contextNode) {
            _setupData = new SetupData();
            _setupData.LoadFromXml(contextNode);
        }


        private void LoadFromXml(XmlNode contextNode) {
            XmlNode node = contextNode.SelectSingleNode("Program");
            if (node != null && node.Attributes != null && node.Attributes["useSequencePluginData"] != null) {
                _useSequencePluginData = bool.Parse(node.Attributes["useSequencePluginData"].Value);
            }
            EventSequences.Clear();
            if (node != null) {
                XmlNodeList sequenceNode = node.SelectNodes("Sequence");
                if (sequenceNode != null) {
                    foreach (XmlNode node2 in sequenceNode) {
                        string path = Path.Combine(Paths.SequencePath, node2.InnerText);
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
                XmlNode node3 = node.SelectSingleNode("Profile");
                if (node3 == null) {
                    LoadEmbeddedData(node);
                }
                else {
                    AttachToProfile(node3.InnerText);
                }
            }
            _crossFadeLength = int.Parse(Xml.GetNodeAlways(node, "CrossFadeLength", "0").InnerText);
        }


        public void Refresh() {
            foreach (EventSequenceStub stub in EventSequences) {
                if (string.IsNullOrEmpty(stub.FileName)) {
                    throw new Exception(Resources.OneOrMoreSequencesNotSaved);
                }
                using (var sequence = new EventSequence(stub.FileName)) {
                    stub.Name = sequence.Name;
                    stub.Length = sequence.Time;
                }
            }
        }


        public void ReloadProfile() {
            _setupData = _profile.PlugInData;
        }


        public void SaveTo(string filePath) {
            XmlDocument contextNode = Xml.CreateXmlDocument();
            SaveToXml(contextNode);
            contextNode.Save(filePath);
        }


        private void SaveToXml(XmlNode contextNode) {
            XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(contextNode, "Program");
            if (_useSequencePluginData) {
                Xml.SetAttribute(emptyNodeAlways, "useSequencePluginData", _useSequencePluginData.ToString());
            }
            foreach (EventSequenceStub stub in EventSequences) {
                if (string.IsNullOrEmpty(stub.FileName)) {
                    throw new Exception(Resources.SequencesMustBeSavedBeforeSavingProgram);
                }
                Xml.SetNewValue(emptyNodeAlways, "Sequence", Path.GetFileName(stub.FileName));
            }
            if (_profile == null) {
                if (emptyNodeAlways.OwnerDocument != null) {
                    emptyNodeAlways.AppendChild(emptyNodeAlways.OwnerDocument.ImportNode(_setupData.RootNode, true));
                }
            }
            else {
                Xml.SetValue(emptyNodeAlways, "Profile", _profile.Name);
            }
            Xml.SetValue(emptyNodeAlways, "CrossFadeLength", _crossFadeLength.ToString(CultureInfo.InvariantCulture));
        }


        public override string ToString() {
            return Name;
        }
    }
}
