using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;

using VixenPlus.Properties;

namespace VixenPlus {
    public class SequenceProgram : IScheduledObject {
        private readonly ulong _key;
        private byte[][] _mask;
        private Profile _profile;


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

        internal List<EventSequenceStub> EventSequences { get; private set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private bool Loop { get; set; }

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
        }

        public SetupData PlugInData {
            get {
                return _profile == null ? SetupData : _profile.PlugInData;
            }
        }

        public bool TreatAsLocal { get; private set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private object UserData { get; set; }


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


        private void ConstructUsing() {
            EventSequences = new List<EventSequenceStub>();
        }

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

        private void ReloadProfile() {
            SetupData = _profile.PlugInData;
        }

        public override string ToString() {
            return Name;
        }
    }
}