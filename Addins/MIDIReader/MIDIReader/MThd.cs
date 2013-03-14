namespace MIDIReader
{
    using System;
    using System.IO;

    internal class MThd : Chunk
    {
        private MIDIFormat m_format;
        private int m_fps;
        private int m_framesPerFrames;
        private int m_ppqn;
        private int m_trackCount;
        private bool m_usesSMPTE;

        public MThd(Stream fileStream) : base("MThd", fileStream)
        {
            byte[] data = base.Data;
            this.m_format = (MIDIFormat) base.BigToLittleEndian16(data);
            this.m_trackCount = base.BigToLittleEndian16(data, 2);
            if ((data[4] & 0x80) > 0)
            {
                this.m_usesSMPTE = true;
                this.m_fps = -((sbyte) data[4]);
                this.m_framesPerFrames = data[5];
            }
            else
            {
                this.m_usesSMPTE = false;
                this.m_ppqn = base.BigToLittleEndian16(data, 4);
            }
        }

        public MIDIFormat Format
        {
            get
            {
                return this.m_format;
            }
        }

        public int FramesPerSecond
        {
            get
            {
                return this.m_fps;
            }
        }

        public int PulsesPerQuarterNote
        {
            get
            {
                return this.m_ppqn;
            }
        }

        public int SubFramesPerFrame
        {
            get
            {
                return this.m_framesPerFrames;
            }
        }

        public int TrackCount
        {
            get
            {
                return this.m_trackCount;
            }
        }

        public bool UsesSMPTETiming
        {
            get
            {
                return this.m_usesSMPTE;
            }
        }
    }
}

