namespace RC_4
{
    using System;
    using System.IO.Ports;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class RC_4 : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private byte m_addr;
        private byte[] m_packet = new byte[] { 0x21, 0x52, 0x43, 0x34, 0, 0x53, 0 };
        private SerialPort m_serialPort;
        private SetupData m_setupData;
        private XmlNode m_setupNode;
        private byte m_threshold;
        private const int MAX_CHANNELS = 4;

        public void Event(byte[] channelValues)
        {
            if (!this.m_serialPort.IsOpen)
            {
                this.m_serialPort.Open();
            }
            if (channelValues.Length < 4)
            {
                Array.Resize<byte>(ref channelValues, 4);
            }
            byte num = 0;
            for (int i = 0; i < 4; i++)
            {
                num = (byte) (num << 1);
                num = (byte) (num | ((channelValues[i] >= this.m_threshold) ? 1 : 0));
            }
            this.m_packet[6] = num;
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
            this.m_threshold = (byte) ((this.m_setupData.GetInteger(this.m_setupNode, "threshold", 50) * 0xff) / 100);
        }

        public void Setup()
        {
            frmEfxSetup setup = new frmEfxSetup(this.m_setupNode, false);
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
                return "Vixen Developers";
            }
        }

        public string Description
        {
            get
            {
                return "EFX-TEK RC-4";
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
                return "EFX-TEK RC-4";
            }
        }
    }
}

