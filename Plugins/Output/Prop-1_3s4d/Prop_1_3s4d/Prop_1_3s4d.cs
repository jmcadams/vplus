namespace Prop_1_3s4d
{
    using System;
    using System.IO.Ports;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class Prop_1_3s4d : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private int m_analogMax;
        private int m_analogMin;
        private float m_multiplier;
        private byte[] m_packet = new byte[] { 0x56, 0x58, 0x4e, 0, 0, 0, 0 };
        private SerialPort m_serialPort;
        private SetupData m_setupData;
        private XmlNode m_setupNode;
        private int m_threshold;

        public void Event(byte[] channelValues)
        {
            if (this.m_serialPort.IsOpen)
            {
                int num;
                if (channelValues.Length < 7)
                {
                    Array.Resize<byte>(ref channelValues, 7);
                }
                byte num2 = 0;
                for (num = 3; num <= 6; num++)
                {
                    num2 = (byte) (num2 >> 1);
                    num2 = (byte) (num2 | ((channelValues[num] >= this.m_threshold) ? ((byte) 0x80) : ((byte) 0)));
                }
                num2 = (byte) (num2 >> 1);
                this.m_packet[3] = num2;
                num = 0;
                for (int i = 4; num < 3; i++)
                {
                    this.m_packet[i] = (byte) ((channelValues[num] * this.m_multiplier) + this.m_analogMin);
                    num++;
                }
                this.m_serialPort.Write(this.m_packet, 0, this.m_packet.Length);
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
            this.m_serialPort = new SerialPort(this.m_setupData.GetString(this.m_setupNode, "name", "COM1"), this.m_setupData.GetInteger(this.m_setupNode, "baud", 0x960), (Parity) Enum.Parse(typeof(Parity), this.m_setupData.GetString(this.m_setupNode, "parity", Parity.None.ToString())), this.m_setupData.GetInteger(this.m_setupNode, "data", 8), (StopBits) Enum.Parse(typeof(StopBits), this.m_setupData.GetString(this.m_setupNode, "stop", StopBits.One.ToString())));
            this.m_serialPort.Handshake = Handshake.None;
            this.m_serialPort.Encoding = Encoding.UTF8;
            this.m_threshold = (int) ((((float) this.m_setupData.GetInteger(this.m_setupNode, "threshold", 50)) / 100f) * 255f);
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
                return "Vixen Developers";
            }
        }

        public string Description
        {
            get
            {
                return "EFX-TEK 3+4 firmware (Prop-1).";
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
                return "EFX-TEK Prop-1 3+4";
            }
        }
    }
}

