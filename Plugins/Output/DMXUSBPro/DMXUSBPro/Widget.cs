namespace DMXUSBPro
{
    using System;
    using System.IO;
    using System.IO.Ports;

    internal class Widget : IDisposable
    {
        private string logFile = Path.Combine(Paths.DataPath, "dmxusbpro.log");
        private Message m_dmxPacketMessage;
        private SerialPort m_serialPort = null;
        private byte[] m_statePacket;

        public Widget(SerialPort serialPort)
        {
            this.m_serialPort = serialPort;
            this.m_statePacket = new byte[0x201];
            this.m_dmxPacketMessage = new Message(MessageType.OutputOnlySendDMXPacketRequest);
            this.m_dmxPacketMessage.Data = this.m_statePacket;
        }

        public void Dispose()
        {
            if ((this.m_serialPort != null) && this.m_serialPort.IsOpen)
            {
                this.m_serialPort.Close();
            }
            GC.SuppressFinalize(this);
        }

        ~Widget()
        {
            this.Dispose();
        }

        public void OutputDMXPacket(byte[] channelValues)
        {
            if (this.m_serialPort == null)
            {
                File.AppendAllText(this.logFile, "Port reference is null\n");
            }
            if (this.m_statePacket == null)
            {
                File.AppendAllText(this.logFile, "State packet is null\n");
            }
            if (this.m_dmxPacketMessage == null)
            {
                File.AppendAllText(this.logFile, "Packet message is null\n");
            }
            else if (this.m_dmxPacketMessage.Packet == null)
            {
                File.AppendAllText(this.logFile, "Packet message : packet is null\n");
            }
            if (!this.m_serialPort.IsOpen)
            {
                this.m_serialPort.Open();
            }
            this.m_statePacket[0] = 0;
            Array.Copy(channelValues, 0, this.m_statePacket, 1, Math.Min(0x200, channelValues.Length));
            byte[] packet = this.m_dmxPacketMessage.Packet;
            this.m_serialPort.Write(packet, 0, packet.Length);
        }

        public void Start()
        {
            if (!this.m_serialPort.IsOpen)
            {
                this.m_serialPort.Open();
            }
        }

        public void Stop()
        {
            if (this.m_serialPort.IsOpen)
            {
                this.m_serialPort.Close();
            }
        }
    }
}

