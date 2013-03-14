namespace MIDIReader
{
    using System;
    using System.Drawing;

    internal class Key
    {
        private bool m_enabled;
        private int m_id;
        private char m_note;
        private int m_octave;
        private MIDIReader.Pitch m_pitch;
        private Rectangle m_region;
        private int m_signature;

        public override bool Equals(object obj)
        {
            return (this.m_signature == ((Key) obj).Signature);
        }

        public override int GetHashCode()
        {
            return this.m_signature;
        }

        public override string ToString()
        {
            switch (this.m_pitch)
            {
                case MIDIReader.Pitch.Flat:
                    return string.Format("{0}b{1}", this.m_note, this.m_octave);

                case MIDIReader.Pitch.Sharp:
                    return string.Format("{0}#{1}", this.m_note, this.m_octave);
            }
            return string.Format("{0}{1}", this.m_note, this.m_octave);
        }

        public bool Enabled
        {
            get
            {
                return this.m_enabled;
            }
            set
            {
                this.m_enabled = value;
            }
        }

        public int Id
        {
            get
            {
                return this.m_id;
            }
            set
            {
                this.m_id = value;
            }
        }

        public char Note
        {
            get
            {
                return this.m_note;
            }
            set
            {
                this.m_note = value;
                this.m_signature = ((((int) this.m_pitch) << 0x10) | (this.m_note << 8)) | this.m_octave;
            }
        }

        public int Octave
        {
            get
            {
                return this.m_octave;
            }
            set
            {
                this.m_octave = value;
                this.m_signature = ((((int) this.m_pitch) << 0x10) | (this.m_note << 8)) | this.m_octave;
            }
        }

        public MIDIReader.Pitch Pitch
        {
            get
            {
                return this.m_pitch;
            }
            set
            {
                this.m_pitch = value;
            }
        }

        public Rectangle Region
        {
            get
            {
                return this.m_region;
            }
            set
            {
                this.m_region = value;
            }
        }

        public int Signature
        {
            get
            {
                return this.m_signature;
            }
        }
    }
}

