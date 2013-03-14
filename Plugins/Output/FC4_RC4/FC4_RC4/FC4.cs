namespace FC4_RC4
{
    using System;
    using System.IO.Ports;

    internal class FC4 : IModule
    {
        private byte[] m_packet;
        private int m_startChannel;
        private const int MAX_CHANNELS = 4;

        public FC4(byte address, int startChannel)
        {
            byte[] buffer = new byte[] { 0x21, 70, 0x43, 0x34, 0, 0x53, 0, 0, 0, 0 };
            buffer[4] = address;
            this.m_packet = buffer;
            this.m_startChannel = startChannel;
        }

        public void OutputEvent(SerialPort port, byte[] channelValues)
        {
            if (!port.IsOpen)
            {
                port.Open();
            }
            int num = Math.Min(channelValues.Length, this.m_startChannel + 4);
            int index = 6;
            for (int i = this.m_startChannel; index <= 9; i++)
            {
                this.m_packet[index] = (i < num) ? channelValues[i] : ((byte) 0);
                index++;
            }
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

