namespace MiniSSC
{
    using System;
    using System.IO.Ports;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class MiniSSC : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private int m_analogMax;
        private int m_analogMin;
        private float m_multiplier;
        private byte[] m_packet;
        private SerialPort m_serialPort;
        private SetupData m_setupData;
        private XmlNode m_setupNode;
        private byte[] m_shadow;

        public void Event(byte[] channelValues)
        {
            for (byte i = 0; i < channelValues.Length; i = (byte) (i + 1))
            {
                if (channelValues[i] > 0xfe)
                {
                    channelValues[i] = 0xfe;
                }
                if (channelValues[i] != this.m_shadow[i])
                {
                    this.m_packet[1] = i;
                    this.m_packet[2] = (byte) ((channelValues[i] * this.m_multiplier) + this.m_analogMin);
                    this.m_serialPort.Write(this.m_packet, 0, this.m_packet.Length);
                }
            }
            channelValues.CopyTo(this.m_shadow, 0);
        }

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            this.m_setupData = setupData;
            this.m_setupNode = setupNode;
            this.SetPort();
            byte[] buffer = new byte[3];
            buffer[0] = 0xff;
            this.m_packet = buffer;
            this.m_shadow = new byte[executableObject.Channels.Count];
        }

        private void SetPort()
        {
            this.m_serialPort = new SerialPort(this.m_setupData.GetString(this.m_setupNode, "name", "COM1"), this.m_setupData.GetInteger(this.m_setupNode, "baud", 0x9600), (Parity) Enum.Parse(typeof(Parity), this.m_setupData.GetString(this.m_setupNode, "parity", Parity.None.ToString())), this.m_setupData.GetInteger(this.m_setupNode, "data", 8), (StopBits) Enum.Parse(typeof(StopBits), this.m_setupData.GetString(this.m_setupNode, "stop", StopBits.One.ToString())));
            this.m_serialPort.Handshake = Handshake.None;
            this.m_serialPort.Encoding = Encoding.UTF8;
            this.m_analogMin = this.m_setupData.GetInteger(this.m_setupNode, "analogMin", 100);
            this.m_analogMax = this.m_setupData.GetInteger(this.m_setupNode, "analogMax", 200);
            this.m_multiplier = ((float) (this.m_analogMax - this.m_analogMin)) / 255f;
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
                return "MiniSSC";
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
                return "MiniSSC";
            }
        }
    }
}

