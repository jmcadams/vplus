namespace DMXUSBPro
{
    using System;

    internal class Message
    {
        private byte[] m_data = null;
        private ushort m_dataLength = 0;
        private MessageType m_messageType;

        public Message(MessageType type)
        {
            this.m_messageType = type;
        }

        public byte[] Data
        {
            set
            {
                this.m_data = value;
                this.m_dataLength = (ushort) Math.Min(value.Length, 600);
            }
        }

        public byte[] Packet
        {
            get
            {
                byte[] array = new byte[5 + this.m_dataLength];
                array[0] = 0x7e;
                array[1] = (byte) this.m_messageType;
                array[2] = (byte) this.m_dataLength;
                array[3] = (byte) (this.m_dataLength >> 8);
                array[4 + this.m_dataLength] = 0xe7;
                if (this.m_data != null)
                {
                    this.m_data.CopyTo(array, 4);
                }
                return array;
            }
        }
    }
}

