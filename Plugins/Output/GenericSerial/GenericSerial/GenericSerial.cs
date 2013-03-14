namespace GenericSerial
{
    using System;
    using System.IO.Ports;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class GenericSerial : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private byte[] m_footer;
        private byte[] m_header;
        private byte[] m_packet;
        private SerialPort m_serialPort;
        private SetupData m_setupData;
        private XmlNode m_setupNode;

        public void Event(byte[] channelValues)
        {
            channelValues.CopyTo(this.m_packet, this.m_header.Length);
            this.m_serialPort.Write(this.m_packet, 0, this.m_packet.Length);
        }

        private byte[] GetBytes(string nodeName)
        {
            XmlNode nodeAlways = Xml.GetNodeAlways(this.m_setupNode, nodeName);
            if ((nodeAlways.Attributes["checked"] != null) && (nodeAlways.Attributes["checked"].Value == bool.TrueString))
            {
                return Encoding.ASCII.GetBytes(nodeAlways.InnerText);
            }
            return new byte[0];
        }

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            this.m_setupData = setupData;
            this.m_setupNode = setupNode;
            this.SetPort();
            this.m_packet = new byte[(this.m_header.Length + executableObject.Channels.Count) + this.m_footer.Length];
            this.m_header.CopyTo(this.m_packet, 0);
            this.m_footer.CopyTo(this.m_packet, (int) (this.m_packet.Length - this.m_footer.Length));
        }

        private void SetPort()
        {
            this.m_serialPort = new SerialPort(this.m_setupData.GetString(this.m_setupNode, "Name", "COM1"), this.m_setupData.GetInteger(this.m_setupNode, "Baud", 0x9600), (Parity) Enum.Parse(typeof(Parity), this.m_setupData.GetString(this.m_setupNode, "Parity", Parity.None.ToString())), this.m_setupData.GetInteger(this.m_setupNode, "Data", 8), (StopBits) Enum.Parse(typeof(StopBits), this.m_setupData.GetString(this.m_setupNode, "Stop", StopBits.One.ToString())));
            this.m_serialPort.Handshake = Handshake.None;
            this.m_serialPort.Encoding = Encoding.UTF8;
            this.m_header = this.GetBytes("Header");
            this.m_footer = this.GetBytes("Footer");
        }

        public void Setup()
        {
            SetupDialog dialog = new SetupDialog(this.m_setupNode);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.SetPort();
            }
        }

        public void Shutdown()
        {
            if (this.m_serialPort.IsOpen)
            {
                this.m_serialPort.Close();
            }
        }

        public void Startup()
        {
            if (this.m_serialPort.IsOpen)
            {
                this.m_serialPort.Close();
            }
            this.m_serialPort.Open();
        }

        public override string ToString()
        {
            return this.Name;
        }

        public string Author
        {
            get
            {
                return "K.C. Oaks";
            }
        }

        public string Description
        {
            get
            {
                return "Generic serial output plugin";
            }
        }

        public Vixen.HardwareMap[] HardwareMap
        {
            get
            {
                return new Vixen.HardwareMap[] { new Vixen.HardwareMap("Serial", int.Parse(this.m_serialPort.PortName.Substring(3))) };
            }
        }

        public string Name
        {
            get
            {
                return "Generic serial";
            }
        }
    }
}

