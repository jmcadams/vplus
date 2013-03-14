namespace FC4_RC4
{
    using System;
    using System.IO.Ports;

    internal class RC4 : IModule
    {
        private byte[] m_packet;
        private int m_startChannel;
        private byte m_threshold;
        private const int MAX_CHANNELS = 4;

        public RC4(byte address, int startChannel, byte threshold)
        {
            byte[] buffer = new byte[] { 0x21, 0x52, 0x43, 0x34, 0, 0x53, 0 };
            buffer[4] = address;
            this.m_packet = buffer;
            this.m_startChannel = startChannel;
            this.m_threshold = threshold;
        }

        public void OutputEvent(SerialPort port, byte[] channelValues)
        {
            if (!port.IsOpen)
            {
                port.Open();
            }
            int num = Math.Min(channelValues.Length, this.m_startChannel + 4);
            byte num2 = 0;
            for (int i = this.m_startChannel; i < num; i++)
            {
                num2 = (byte) (num2 >> 1);
                num2 = (byte) (num2 | ((channelValues[i] >= this.m_threshold) ? 8 : 0));
            }
            this.m_packet[6] = num2;
            port.Write(this.m_packet, 0, this.m_packet.Length);
        }

        public byte Address
        {
            set
            {
                this.m_packet[4] = value;
            }
        }

        public int ChannelCount
        {
            get
            {
                return 4;
            }
        }

        public int StartChannel
        {
            set
            {
                this.m_startChannel = value;
            }
        }
    }
}

