using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace VixenPlus {
    public class Profile : IExecutable {
        private readonly List<int> _channelOutputs;
        private List<Channel> _channelObjects;
        private List<Channel> _frozenChannelList;
        private byte[][] _frozenMask;
        private List<Channel> _frozenOutputChannelList;
        private bool _isFrozen;


        public Profile() {
            FileName = string.Empty;
            _isFrozen = false;
            TreatAsLocal = false;
            UserData = null;
            Key = Host.GetUniqueKey();
            _channelObjects = new List<Channel>();
            _channelOutputs = new List<int>();
            PlugInData = new SetupData();
            Sorts = new SortOrders();
            IsDirty = false;
        }


        public Profile(string fileName) : this() {
            ReloadFrom(fileName);
        }


        public int LastSort {
            get { return Sorts.LastSort; }
            set {
                Sorts.LastSort = value;
                IsDirty = true;
            }
        }

        public SortOrders Sorts { get; private set; }

        public int AudioDeviceIndex {
            get { return -1; }
        }

        public int AudioDeviceVolume {
            get { return 100; }
        }

        public bool CanBePlayed {
            get { return false; }
        }

        public List<Channel> Channels {
            get {
                if (_isFrozen) {
                    return _frozenChannelList;
                }
                var list = new List<Channel>(_channelObjects);
                for (var i = 0; i < list.Count; i++) {
                    list[i].OutputChannel = _channelOutputs[i];
                }
                return list;
            }
        }

        public List<Channel> FullChannels {
            get { return Channels; }
        } 

        public string FileName { get; set; }
        
        public bool IsDirty { get; set; }

        public ulong Key { get; private set; }

        public byte[][] Mask {
            get {
                if (_isFrozen) {
                    return _frozenMask;
                }
                var buffer = new byte[Channels.Count];
                for (var i = 0; i < Channels.Count; i++) {
                    buffer[i] = Channels[i].Enabled ? ((byte) 255) : ((byte) 0);
                }
                return new[] {buffer};
            }
            set { }
        }

        public string Name {
            get { return Path.GetFileNameWithoutExtension(FileName); }
            // ReSharper disable AssignNullToNotNullAttribute
            set {
                FileName = Path.Combine(string.IsNullOrEmpty(FileName) ? Paths.ProfilePath : Path.GetDirectoryName(FileName), value + Vendor.ProfileExtension);
                IsDirty = true;
            }
            // ReSharper restore AssignNullToNotNullAttribute
        }

        public List<Channel> OutputChannels {
            get {
                if (_isFrozen) {
                    return _frozenOutputChannelList;
                }
                var channelOutputs = new List<Channel>(Channels);
                for (var i = 0; i < channelOutputs.Count; i++) {
                    channelOutputs[_channelOutputs[i]] = Channels[i];
                }
                return channelOutputs;
            }
            set {
                for (var i = 0; i < Channels.Count; i++) {
                    _channelOutputs[i] = value.IndexOf(Channels[i]);
                }
                IsDirty = true;
            }
        }

        public SetupData PlugInData { get; private set; }

        public bool TreatAsLocal { get; private set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private object UserData { get; set; }


        public void AddChannelObject(Channel channelObject) {
            _channelObjects.Add(channelObject);
            _channelOutputs.Add(_channelOutputs.Count);
            Sorts.UpdateChannelCounts(Channels.Count);
            IsDirty = true;
        }


        public void Dispose() {
            foreach (var channel in _channelObjects) {
                channel.Dispose();
            }
            GC.SuppressFinalize(this);
        }


        ~Profile() {
            Dispose();
        }


        public void Freeze() {
            if (_isFrozen) {
                return;
            }

            _frozenChannelList = Channels;
            _frozenOutputChannelList = OutputChannels;
            _frozenMask = Mask;
            _isFrozen = true;
        }


        public void InheritChannelsFrom(EventSequence sequence) {
            _channelObjects = sequence.FullChannels;
            _channelOutputs.Clear();
            foreach (var channel in sequence.FullChannels) {
                _channelOutputs.Add(channel.OutputChannel);
            }
            Sorts.UpdateChannelCounts(Channels.Count);
            IsDirty = true;
        }


        public void InheritPlugInDataFrom(EventSequence sequence) {
            PlugInData.LoadFromXml(sequence.PlugInData.RootNode.ParentNode);
        }


        public void InheritSortsFrom(EventSequence sequence) {
            Sorts = (sequence.Sorts == null) ? null : sequence.Sorts.Clone();
            IsDirty = true;
        }

        public void RemoveChannel(Channel channelObject) {
            //Find where the associated channel info is
            var objectIndex = _channelObjects.IndexOf(channelObject);
            var outputIndex = _channelOutputs[objectIndex];

            //remove them
            _channelOutputs.RemoveAt(objectIndex);
            _channelObjects.RemoveAt(objectIndex);

            // Now update the output mapping for the channel
            for (var i = 0; i < _channelOutputs.Count; i++) {
                if (_channelOutputs[i] > outputIndex) {
                    _channelOutputs[i]--;
                }
            }

            Sorts.UpdateChannelCounts(Channels.Count);
            IsDirty = true;
        }


        public void Reload() {
            var document = new XmlDocument();
            document.Load(FileName);
            XmlNode documentElement = document.DocumentElement;
            _channelObjects.Clear();
            _channelOutputs.Clear();
            if (documentElement != null) {
                var channelObjectsNode = documentElement.SelectNodes("ChannelObjects/*");
                if (channelObjectsNode != null) {
                    foreach (XmlNode channelObject in channelObjectsNode) {
                        _channelObjects.Add(new Channel(channelObject));
                    }
                }

                var outputNodes = documentElement.SelectSingleNode("Outputs");
                if (outputNodes != null) {
                    foreach (var outputChannel in outputNodes.InnerText.Split(new[] {','})) {
                        if (outputChannel.Length > 0) {
                            _channelOutputs.Add(Convert.ToInt32(outputChannel));
                        }
                    }
                }
            }
            PlugInData.LoadFromXml(documentElement);
            Sorts.LoadFromXml(documentElement);
            if (documentElement != null) {
                var disabledChannelsNode = documentElement.SelectSingleNode("DisabledChannels");
                if (disabledChannelsNode != null) {
                    foreach (var disabledChannel in disabledChannelsNode.InnerText.Split(new[] {','})) {
                        if (disabledChannel != string.Empty) {
                            Channels[Convert.ToInt32(disabledChannel)].Enabled = false;
                        }
                    }
                }
            }
            IsDirty = false;
            if (!_isFrozen) {
                return;
            }

            _isFrozen = false;
            Freeze();
        }


        private void ReloadFrom(string fileName) {
            FileName = fileName;
            Reload();
        }



        public void SaveToFile() {
            var ownerDocument = SaveToXml(null).OwnerDocument;
            if (ownerDocument != null) {
                ownerDocument.Save(FileName);
            }
            IsDirty = false;
        }


        private XmlNode SaveToXml(XmlDocument doc) {
            XmlNode profile;
            
            if (doc == null) {
                doc = Xml.CreateXmlDocument("Profile");
                profile = doc.DocumentElement;
            }
            else {
                profile = doc.CreateElement("Profile");
            }

            var emptyNodeAlways = Xml.GetEmptyNodeAlways(profile, "ChannelObjects");
            foreach (var channel in _channelObjects) {
                emptyNodeAlways.AppendChild(channel.SaveToXml(doc));
            }
            var builder = new StringBuilder();
            foreach (var num in _channelOutputs) {
                builder.AppendFormat("{0},", num);
            }
            Xml.GetEmptyNodeAlways(profile, "Outputs").InnerText = builder.ToString().TrimEnd(new[] {','});
            
            if (profile != null) {
                profile.AppendChild(doc.ImportNode(PlugInData.RootNode, true));
                Sorts.SaveToXml(profile);
            }
            
            var disabledChannels = new List<string>();
            for (var i = 0; i < Channels.Count; i++) {
                if (!Channels[i].Enabled) {
                    disabledChannels.Add(i.ToString(CultureInfo.InvariantCulture));
                }
            }
            Xml.SetValue(profile, "DisabledChannels", string.Join(",", disabledChannels.ToArray()));

            IsDirty = false;
            return profile;
        }


        public override string ToString() {
            return Name;
        }
    }
}