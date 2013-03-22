namespace Icicles
{
    using System;
    using System.IO.Ports;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class Icicles : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private byte[] m_packet_v1 = new byte[] { 0x41, 100, 0x2b, 0 };
        private byte[] m_packet_v2 = new byte[] { 0x41, 100, 0x2b, 0x2b, 0x54, 0x7e, 0x21, 0 };
        private SerialPort m_selectedPort = null;
        private SetupData m_setupData;
        private XmlNode m_setupNode;
        private int m_version;

        public void Event(byte[] channelValues)
        {
            if (this.m_selectedPort.IsOpen)
            {
                int num;
                byte[] buffer;
                if (this.m_version == 1)
                {
                    num = 3;
                    buffer = this.m_packet_v1;
                }
                else
                {
                    num = 7;
                    buffer = this.m_packet_v2;
                }
                buffer[num] = (byte) ((channelValues.Length - 1) / 8);
                byte[] buffer2 = new byte[buffer[num] + 1];
                for (int i = 0; i < channelValues.Length; i++)
                {
                    buffer2[i >> 3] = (byte) (buffer2[i >> 3] | ((byte) (((channelValues[i] > 0) ? 1 : 0) << (i % 8))));
                }
                this.m_selectedPort.Write(buffer, 0, buffer.Length);
                this.m_selectedPort.Write(buffer2, 0, buffer2.Length);
            }
        }

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            this.m_setupData = setupData;
            this.m_setupNode = setupNode;
            this.m_version = this.m_setupData.GetInteger(this.m_setupNode, "Version", 2);
            this.m_selectedPort = new SerialPort(this.m_setupData.GetString(this.m_setupNode, "name", "COM1"), this.m_setupData.GetInteger(this.m_setupNode, "baud", 0x9600), (Parity) Enum.Parse(typeof(Parity), this.m_setupData.GetString(this.m_setupNode, "parity", Parity.None.ToString())), this.m_setupData.GetInteger(this.m_setupNode, "data", 8), (StopBits) Enum.Parse(typeof(StopBits), this.m_setupData.GetString(this.m_setupNode, "stop", StopBits.One.ToString())));
            this.m_selectedPort.Handshake = Handshake.None;
            this.m_selectedPort.Encoding = Encoding.UTF8;
        }

        public void Setup()
        {
            SetupDialog dialog = new SetupDialog(this.m_selectedPort, this.m_version);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.m_version = dialog.Version;
                this.m_setupData.SetInteger(this.m_setupNode, "Version", this.m_version);
                this.m_selectedPort = dialog.SelectedPort;
                this.m_setupData.SetString(this.m_setupNode, "name", this.m_selectedPort.PortName);
                this.m_setupData.SetInteger(this.m_setupNode, "baud", this.m_selectedPort.BaudRate);
                this.m_setupData.SetString(this.m_setupNode, "parity", this.m_selectedPort.Parity.ToString());
                this.m_setupData.SetInteger(this.m_setupNode, "data", this.m_selectedPort.DataBits);
                this.m_setupData.SetString(this.m_setupNode, "stop", this.m_selectedPort.StopBits.ToString());
            }
        }

        public void Shutdown()
        {
            if (this.m_selectedPort.IsOpen)
            {
                this.m_selectedPort.Close();
            }
        }

        public void Startup()
        {
            if (!this.m_selectedPort.IsOpen)
            {
                this.m_selectedPort.Open();
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
                return "Vixen Developers";
            }
        }

        public string Description
        {
            get
            {
                return "Icicles plugin for Vixen";
            }
        }

        public Vixen.HardwareMap[] HardwareMap
        {
            get
            {
                return new Vixen.HardwareMap[] { new Vixen.HardwareMap("Serial", int.Parse(this.m_selectedPort.PortName.Substring(3))) };
            }
        }

        public string Name
        {
            get
            {
                return "Icicle";
            }
        }
    }
}

