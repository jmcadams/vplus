using System;
using System.Collections.Generic;

namespace VixenPlus {
    public class ExecutableBase: IExecutable {
        private IFileIOHandler _fileHandler;
        private int _audioDeviceVolume = -1;

        public virtual byte[][] Mask {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void Dispose() {
            GC.SuppressFinalize(this);
        }


        public virtual int AudioDeviceIndex { get; set; }

        public int AudioDeviceVolume {
            get { return _audioDeviceVolume == -1 ? 100 : _audioDeviceVolume; }
            protected set { _audioDeviceVolume = value; }
        }

        public bool CanBePlayed {
            get { return true; }
        }

        public virtual List<Channel> Channels {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual List<Channel> FullChannels {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string FileName { get; protected set; }

        public virtual ulong Key {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual string Name {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual SetupData PlugInData {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool TreatAsLocal { get; set; }

        public IFileIOHandler FileIOHandler {
            get { return _fileHandler ?? (_fileHandler = FileIOHelper.GetNativeHelper()); }
            set { _fileHandler = value; } }
    }
}