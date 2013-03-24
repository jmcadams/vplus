namespace DC_16
{
    using System;
    using System.IO.Ports;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class DC_16 : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private byte m_addr;
        private int[] m_channelGroupControllers = new int[] { -1, -1, -1, -1 };
        private byte[] m_firmwareErrorHighPacket = new byte[] { 0x21, 0x44, 0x43, 0x31, 0x36, 0, 80, 14, 0 };
        private byte[] m_firmwareErrorLowPacket = new byte[] { 0x21, 0x44, 0x43, 0x31, 0x36, 0, 80, 6, 0 };
        private byte[] m_packet = new byte[] { 0x21, 0x44, 0x43, 0x31, 0x36, 0, 0x53, 0, 0 };
        private SerialPort m_serialPort;
        private SetupData m_setupData;
        private XmlNode m_setupNode;
        private byte m_threshold;
        private const int MAX_CHANNELS = 0x40;
        private const int MAX_CONTROLLER_CHANNELS = 0x10;
        private const int MAX_CONTROLLERS = 4;

        public void Event(byte[] channelValues)
        {
            if (channelValues.Length < 0x40)
            {
                Array.Resize<byte>(ref channelValues, 0x40);
            }
            for (int i = 0; i < this.m_channelGroupControllers.Length; i++)
            {
                int num3 = this.m_channelGroupControllers[i];
                if (num3 != -1)
                {
                    ushort num = 0;
                    int num4 = i * 0x10;
                    for (int j = 0; j < 0x10; j++)
                    {
                        num = (ushort) (num >> 1);
                        num = (ushort) (num | ((channelValues[j + num4] >= this.m_threshold) ? 0x8000 : 0));
                    }
                    this.m_packet[7] = (byte) num;
                    this.m_packet[8] = (byte) (num >> 8);
                    this.m_packet[5] = (byte) num3;
                    this.m_firmwareErrorLowPacket[5] = (byte) num3;
                    this.m_firmwareErrorHighPacket[5] = (byte) num3;
                    this.m_serialPort.Write(this.m_packet, 0, this.m_packet.Length);
                    this.m_firmwareErrorLowPacket[8] = (byte) ((this.m_packet[7] & 0x20) >> 5);
                    this.m_firmwareErrorHighPacket[8] = (byte) ((this.m_packet[8] & 0x20) >> 5);
                    this.m_serialPort.Write(this.m_firmwareErrorLowPacket, 0, this.m_firmwareErrorLowPacket.Length);
                    this.m_serialPort.Write(this.m_firmwareErrorHighPacket, 0, this.m_firmwareErrorHighPacket.Length);
                }
            }
        }

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            this.m_setupData = setupData;
            this.m_setupNode = setupNode;
            this.SetPort();
            for (int i = 0; i < this.m_channelGroupControllers.Length; i++)
            {
                int num;
                int num3 = i + 1;
                if (int.TryParse(Xml.GetNodeAlways(this.m_setupNode, "Group" + num3.ToString()).InnerText, out num))
                {
                    this.m_channelGroupControllers[i] = num;
                }
                else
                {
                    this.m_channelGroupControllers[i] = -1;
                }
            }
        }

        private void SetPort()
        {
            this.m_serialPort = new SerialPort(this.m_setupData.GetString(this.m_setupNode, "Name", "COM1"), 0x9600, Parity.None, 8, StopBits.One);
            this.m_serialPort.Handshake = Handshake.None;
            this.m_serialPort.Encoding = Encoding.UTF8;
            this.m_threshold = (byte) ((this.m_setupData.GetInteger(this.m_setupNode, "Threshold", 50) * 0xff) / 100);
        }

        public void Setup()
        {
            frmSetupDialog dialog = new frmSetupDialog(this.m_setupNode);
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
                while (this.m_serialPort.IsOpen)
                {
                }
            }
        }

        public void Startup()
        {
            this.m_packet[5] = this.m_addr;
            this.m_firmwareErrorLowPacket[5] = this.m_addr;
            this.m_firmwareErrorHighPacket[5] = this.m_addr;
            if (!this.m_serialPort.IsOpen)
            {
                this.m_serialPort.Open();
            }
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
                return "EFX-TEK DC-16";
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
                return "EFX-TEK DC-16";
            }
        }
    }
}

