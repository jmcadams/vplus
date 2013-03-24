namespace FC_4
{
    using System;
    using System.IO.Ports;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;
    using VixenPlus;

    public class FC_4 : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private byte m_addr;
        private byte[] m_packet = new byte[] { 0x21, 70, 0x43, 0x34, 0, 0x53, 0, 0, 0, 0 };
        private SerialPort m_serialPort;
        private SetupData m_setupData;
        private XmlNode m_setupNode;

        public void Event(byte[] channelValues)
        {
            if (!this.m_serialPort.IsOpen)
            {
                this.m_serialPort.Open();
            }
            int index = 6;
            for (int i = 0; index <= 9; i++)
            {
                this.m_packet[index] = (channelValues.Length > i) ? channelValues[i] : ((byte) 0);
                index++;
            }
            this.m_serialPort.Write(this.m_packet, 0, this.m_packet.Length);
            this.m_serialPort.Close();
            while (this.m_serialPort.IsOpen)
            {
            }
        }

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            this.m_setupData = setupData;
            this.m_setupNode = setupNode;
            this.SetPort();
        }

        private void SetPort()
        {
            this.m_serialPort = new SerialPort(this.m_setupData.GetString(this.m_setupNode, "name", "COM1"), this.m_setupData.GetInteger(this.m_setupNode, "baud", 0x9600), (Parity) Enum.Parse(typeof(Parity), this.m_setupData.GetString(this.m_setupNode, "parity", Parity.None.ToString())), this.m_setupData.GetInteger(this.m_setupNode, "data", 8), (StopBits) Enum.Parse(typeof(StopBits), this.m_setupData.GetString(this.m_setupNode, "stop", StopBits.One.ToString())));
            this.m_serialPort.Handshake = Handshake.None;
            this.m_serialPort.Encoding = Encoding.UTF8;
            this.m_addr = (byte) this.m_setupData.GetInteger(this.m_setupNode, "addr", 0);
        }

        public void Setup()
        {
            frmEfxSetup setup = new frmEfxSetup(this.m_setupNode, true);
            if (setup.ShowDialog() == DialogResult.OK)
            {
                this.SetPort();
            }
        }

        public void Shutdown()
        {
        }

        public void Startup()
        {
            this.m_packet[4] = this.m_addr;
        }

        public override string ToString()
        {
            return this.Name;
        }

        public string Author
        {
            get
            {
                return "Vixen and VixenPlus Developers";
            }
        }

        public string Description
        {
            get
            {
                return "EFX-TEK FC-4 Dimmer";
            }
        }

        public VixenPlus.HardwareMap[] HardwareMap
        {
            get
            {
                return new VixenPlus.HardwareMap[] { new VixenPlus.HardwareMap("Serial", int.Parse(this.m_serialPort.PortName.Substring(3))) };
            }
        }

        public string Name
        {
            get
            {
                return "EFX-TEK FC-4 Dimmer";
            }
        }
    }
}

