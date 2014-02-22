using System;
using System.IO;

using CommonControls;

namespace VixenPlus {
    internal class EventSequenceStub : IDisposable {
        private int _length;


        public EventSequenceStub(EventSequence sequence) {
            FileName = string.Empty;
            _length = 0;
            LengthString = string.Empty;
            AudioName = string.Empty;
            AudioFileName = string.Empty;
            Sequence = null;
            FileName = sequence.FileName;
            Length = sequence.Time;
            if (sequence.Audio != null) {
                AudioName = sequence.Audio.Name;
                AudioFileName = sequence.Audio.FileName;
            }
            Sequence = sequence;
            Mask = Sequence.Mask;
        }


        public EventSequenceStub(string fileName, bool referenceSequence) {
            FileName = string.Empty;
            _length = 0;
            LengthString = string.Empty;
            AudioName = string.Empty;
            AudioFileName = string.Empty;
            Sequence = null;
            var sequence = new EventSequence(fileName);
            FileName = sequence.FileName;
            Length = sequence.Time;
            if (sequence.Audio != null) {
                AudioName = sequence.Audio.Name;
                AudioFileName = sequence.Audio.FileName;
            }
            Mask = sequence.Mask;
            if (referenceSequence) {
                Sequence = sequence;
            }
            else {
                sequence.Dispose();
            }
        }


        // ReSharper disable UnusedAutoPropertyAccessor.Local
        private string AudioFileName { get; set; }

        private string AudioName { get; set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Local

        private string FileName { get; set; }

        public int Length {
            get { return _length; }
            private set {
                _length = value;
                LengthString = Length.FormatNoMills(true);
            }
        }

        private string LengthString { get; set; }

        public byte[][] Mask { get; private set; }

        private string Name {
            get { return Path.GetFileNameWithoutExtension(FileName); }
        }

        public EventSequence Sequence { get; private set; }

        public void Dispose() {
            if (Sequence != null) {
                Sequence.Dispose();
                Sequence = null;
            }
            GC.SuppressFinalize(this);
        }


        ~EventSequenceStub() {
            Dispose();
        }


        public override string ToString() {
            return string.Format("{0} ({1})", Name, LengthString);
        }
    }
}