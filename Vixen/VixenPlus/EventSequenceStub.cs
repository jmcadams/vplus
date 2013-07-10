using System;
using System.IO;

using CommonUtils;

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


        public string AudioFileName { get; set; }

        public string AudioName { get; set; }

        public string FileName { get; private set; }

        public int Length {
            get { return _length; }
            set {
                _length = value;
                LengthString = Length.FormatNoMills(true);
            }
        }

        public string LengthString { get; private set; }

        public byte[][] Mask { get; set; }

        public string Name {
            get { return Path.GetFileNameWithoutExtension(FileName); }
            set { FileName = Path.ChangeExtension(value, ".vpr"); }
        }

        public EventSequence Sequence { get; private set; }


        public void Dispose() {
            Dispose(true);
        }


        public void Dispose(bool disposing) {
            if (Sequence != null) {
                Sequence.Dispose();
                Sequence = null;
            }
            GC.SuppressFinalize(this);
        }


        ~EventSequenceStub() {
            Dispose(false);
        }


        public EventSequence RetrieveSequence() {
            return Sequence ?? (Sequence = new EventSequence(FileName));
        }


        public override string ToString() {
            return string.Format("{0} ({1})", Name, LengthString);
        }
    }
}
