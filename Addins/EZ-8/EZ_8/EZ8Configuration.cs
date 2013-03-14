namespace EZ_8
{
    using System;

    public class EZ8Configuration
    {
        private const byte CODE_PROTECT = 0xcf;
        public bool CodeProtect = false;
        public byte FrameTiming = 0x21;
        public const int HEADER_LENGTH = 0x40;
        private ushort[] m_channelEnds = new ushort[] { 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40, 0x40 };
        private ushort m_endOfShow = 0x40;
        private ushort m_startOfShow = 0x40;

        internal byte[] GetConfigurationBytes()
        {
            byte[] buffer = new byte[0x40];
            buffer[0] = this.FrameTiming;
            buffer[1] = this.CodeProtect ? ((byte) 0xcf) : ((byte) 0);
            ushort startOfShowInternal = this.StartOfShowInternal;
            buffer[2] = (byte) (startOfShowInternal & 0xff);
            buffer[3] = (byte) (startOfShowInternal >> 8);
            startOfShowInternal = this.EndOfShowInternal;
            buffer[4] = (byte) (startOfShowInternal & 0xff);
            buffer[5] = (byte) (startOfShowInternal >> 8);
            int channelNumber = 0;
            for (int i = 6; channelNumber < this.m_channelEnds.Length; i += 2)
            {
                startOfShowInternal = this.GetEndOfChannelInternal(channelNumber);
                buffer[i] = (byte) (startOfShowInternal & 0xff);
                buffer[i + 1] = (byte) (startOfShowInternal >> 8);
                channelNumber++;
            }
            return buffer;
        }

        public ushort GetEndOfChannel(int channelNumber)
        {
            return (ushort) (this.GetEndOfChannelInternal(channelNumber) - 0x40);
        }

        private ushort GetEndOfChannelInternal(int channelNumber)
        {
            if ((channelNumber < 0) || (channelNumber >= this.m_channelEnds.Length))
            {
                throw new Exception("Invalid channel number.");
            }
            return this.m_channelEnds[channelNumber];
        }

        internal void ReadConfigurationBytes(byte[] value)
        {
            if (value.Length < 0x40)
            {
                Array.Resize<byte>(ref value, 0x40);
            }
            this.FrameTiming = value[0];
            this.CodeProtect = value[1] == 0xcf;
            this.StartOfShowInternal = BitConverter.ToUInt16(value, 2);
            this.EndOfShowInternal = BitConverter.ToUInt16(value, 4);
            int channelNumber = 0;
            for (int i = 6; channelNumber < this.m_channelEnds.Length; i += 2)
            {
                this.SetEndOfChannelInternal(channelNumber, BitConverter.ToUInt16(value, i));
                channelNumber++;
            }
        }

        public void SetEndOfChannel(int channelNumber, ushort value)
        {
            this.SetEndOfChannelInternal(channelNumber, (ushort) (value + 0x40));
        }

        private void SetEndOfChannelInternal(int channelNumber, ushort value)
        {
            if ((channelNumber < 0) || (channelNumber >= this.m_channelEnds.Length))
            {
                throw new Exception("Invalid channel number.");
            }
            this.m_channelEnds[channelNumber] = value;
            this.EndOfShowInternal = Math.Max(value, this.m_endOfShow);
        }

        public ushort EndOfShow
        {
            get
            {
                return (ushort) (this.EndOfShowInternal - 0x40);
            }
            set
            {
                this.EndOfShowInternal = (ushort) (value + 0x40);
            }
        }

        private ushort EndOfShowInternal
        {
            get
            {
                if (this.m_endOfShow != 0)
                {
                    return this.m_endOfShow;
                }
                ushort num = 0;
                foreach (ushort num2 in this.m_channelEnds)
                {
                    num = Math.Max(num2, num);
                }
                return num;
            }
            set
            {
                this.m_endOfShow = value;
            }
        }

        public ushort StartOfShow
        {
            get
            {
                return (ushort) (this.StartOfShowInternal - 0x40);
            }
            set
            {
                this.StartOfShowInternal = (ushort) (value + 0x40);
            }
        }

        private ushort StartOfShowInternal
        {
            get
            {
                return this.m_startOfShow;
            }
            set
            {
                this.m_startOfShow = value;
            }
        }
    }
}

