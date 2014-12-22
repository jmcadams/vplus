using System;
using System.Collections.Generic;
using System.IO;

using VixenPlusCommon;

//TODO We need to refactor this, a profile is a profile, how it is persisted depends on the file IO routine, not the profile.
//TODO What is frozen really do?
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
            Key = Host.GetUniqueKey();
            _channelObjects = new List<Channel>();
            _channelOutputs = new List<int>();
            PlugInData = new SetupData();
            IsDirty = false;
            FileIOHandler = FileIOHelper.GetNativeHelper();
        }

        public IFileIOHandler FileIOHandler { get; set; }

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

        private Dictionary<string, GroupData> _groups;

        public Dictionary<string, GroupData> Groups {
            get { return _groups ?? new Dictionary<string, GroupData>(); }
            set { _groups = value; }
        }

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

        private List<Channel> OutputChannels {
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
        }

        public SetupData PlugInData { get; private set; }

        public bool TreatAsLocal { get; private set; }

        public void AddChannelObject(Channel channelObject,bool addOutput = true) {
            _channelObjects.Add(channelObject);
            if (addOutput) {
                AddChannelOutput(_channelOutputs.Count);
            }

            IsDirty = true;
        }


        public void AddChannelOutput(int output) {
            _channelOutputs.Add(output);
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
            IsDirty = true;
        }


        public void InheritPlugInDataFrom(EventSequence sequence) {
            PlugInData.LoadFromXml(sequence.PlugInData.RootNode.ParentNode);
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

            IsDirty = true;
        }


        public void ClearChannels() {
            _channelObjects.Clear();
            _channelOutputs.Clear();
        }

        public override string ToString() {
            return Name;
        }
    }
}