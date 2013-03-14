namespace JWLC
{
    using System;
    using System.IO.Ports;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class JWLC : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private byte[] m_packet;
        private SerialPort m_serialPort;
        private SetupData m_setupData;
        private XmlNode m_setupNode;

        public void Event(byte[] channelValues)
        {
            if (this.m_serialPort.IsOpen)
            {
                channelValues.CopyTo(this.m_packet, 0);
                this.m_serialPort.Write(this.m_packet, 0, this.m_packet.Length);
            }
        }

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            if ((executableObject.Channels.Count % 8) != 0)
            {
                this.m_packet = new byte[((executableObject.Channels.Count / 8) + 1) * 8];
            }
            else
            {
                this.m_packet = new byte[executableObject.Channels.Count];
            }
            this.m_setupData = setupData;
            this.m_setupNode = setupNode;
            this.SetPort();
        }

        private void SetPort()
        {
            this.m_serialPort = new SerialPort(this.m_setupData.GetString(this.m_setupNode, "Name", "COM1"), this.m_setupData.GetInteger(this.m_setupNode, "Baud", 0x9600), (Parity) Enum.Parse(typeof(Parity), this.m_setupData.GetString(this.m_setupNode, "Parity", Parity.None.ToString())), this.m_setupData.GetInteger(this.m_setupNode, "Data", 8), (StopBits) Enum.Parse(typeof(StopBits), this.m_setupData.GetString(this.m_setupNode, "Stop", StopBits.One.ToString())));
            this.m_serialPort.Handshake = Handshake.None;
            this.m_serialPort.Encoding = Encoding.UTF8;
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
            this.m_serialPort.RtsEnable = false;
            this.m_serialPort.DtrEnable = true;
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
                return "Jon Williams Lighting Controller.";
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
                return "Jon Williams Lighting Controller";
            }
        }
    }
}

