namespace Vixen
{
    using System;
    using System.IO;

    internal class EventSequenceStub : IDisposable
    {
        private string m_audioFileName;
        private string m_audioName;
        private EventSequence m_eventSequence;
        private string m_filename;
        private int m_length;
        private string m_lengthString;
        private byte[][] m_mask;

        public EventSequenceStub(EventSequence sequence)
        {
            this.m_filename = string.Empty;
            this.m_length = 0;
            this.m_lengthString = string.Empty;
            this.m_audioName = string.Empty;
            this.m_audioFileName = string.Empty;
            this.m_eventSequence = null;
            this.m_filename = sequence.FileName;
            this.Length = sequence.Time;
            if (sequence.Audio != null)
            {
                this.m_audioName = sequence.Audio.Name;
                this.m_audioFileName = sequence.Audio.FileName;
            }
            this.m_eventSequence = sequence;
            this.m_mask = this.m_eventSequence.Mask;
        }

        public EventSequenceStub(string fileName, bool referenceSequence)
        {
            this.m_filename = string.Empty;
            this.m_length = 0;
            this.m_lengthString = string.Empty;
            this.m_audioName = string.Empty;
            this.m_audioFileName = string.Empty;
            this.m_eventSequence = null;
            EventSequence sequence = new EventSequence(fileName);
            this.m_filename = sequence.FileName;
            this.Length = sequence.Time;
            if (sequence.Audio != null)
            {
                this.m_audioName = sequence.Audio.Name;
                this.m_audioFileName = sequence.Audio.FileName;
            }
            this.m_mask = sequence.Mask;
            if (referenceSequence)
            {
                this.m_eventSequence = sequence;
            }
            else
            {
                sequence.Dispose();
                sequence = null;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public void Dispose(bool disposing)
        {
            if (this.m_eventSequence != null)
            {
                this.m_eventSequence.Dispose();
                this.m_eventSequence = null;
            }
            GC.SuppressFinalize(this);
        }

        ~EventSequenceStub()
        {
            this.Dispose(false);
        }

        public EventSequence RetrieveSequence()
        {
            if (this.m_eventSequence == null)
            {
                this.m_eventSequence = new EventSequence(this.m_filename);
            }
            return this.m_eventSequence;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", this.Name, this.m_lengthString);
        }

        public string AudioFileName
        {
            get
            {
                return this.m_audioFileName;
            }
            set
            {
                this.m_audioFileName = value;
            }
        }

        public string AudioName
        {
            get
            {
                return this.m_audioName;
            }
            set
            {
                this.m_audioName = value;
            }
        }

        public string FileName
        {
            get
            {
                return this.m_filename;
            }
        }

        public int Length
        {
            get
            {
                return this.m_length;
            }
            set
            {
                this.m_length = value;
                this.m_lengthString = string.Format("{0}:{1:d2}", this.m_length / 0xea60, (this.m_length % 0xea60) / 0x3e8);
            }
        }

        public string LengthString
        {
            get
            {
                return this.m_lengthString;
            }
        }

        public byte[][] Mask
        {
            get
            {
                return this.m_mask;
            }
            set
            {
                this.m_mask = value;
            }
        }

        public string Name
        {
            get
            {
                return Path.GetFileNameWithoutExtension(this.m_filename);
            }
            set
            {
                this.m_filename = Path.ChangeExtension(value, ".vpr");
            }
        }

        public EventSequence Sequence
        {
            get
            {
                return this.m_eventSequence;
            }
        }
    }
}

