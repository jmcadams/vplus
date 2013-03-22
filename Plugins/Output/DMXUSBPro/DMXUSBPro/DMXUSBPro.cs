namespace DMXUSBPro
{
    using System;
    using System.IO.Ports;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;
    using Vixen.Dialogs;

    public class DMXUSBPro : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private SerialPort m_serialPort = null;
        private SetupData m_setupData;
        private XmlNode m_setupNode;
        private Widget m_widget = null;

        public void Event(byte[] channelValues)
        {
            this.m_widget.OutputDMXPacket(channelValues);
        }

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            this.m_setupNode = setupNode;
            this.m_setupData = setupData;
            this.SetPort();
        }

        private void SetPort()
        {
            if ((this.m_serialPort != null) && this.m_serialPort.IsOpen)
            {
                this.m_serialPort.Close();
            }
            this.m_serialPort = new SerialPort(this.m_setupData.GetString(this.m_setupNode, "Name", "COM1"), this.m_setupData.GetInteger(this.m_setupNode, "Baud", 0xe100), (Parity) Enum.Parse(typeof(Parity), this.m_setupData.GetString(this.m_setupNode, "Parity", "None")), this.m_setupData.GetInteger(this.m_setupNode, "Data", 8), (StopBits) Enum.Parse(typeof(StopBits), this.m_setupData.GetString(this.m_setupNode, "Stop", "One")));
            this.m_serialPort.Handshake = Handshake.None;
            this.m_serialPort.Encoding = Encoding.UTF8;
        }

        public void Setup()
        {
            SerialSetupDialog dialog = new SerialSetupDialog(this.m_serialPort);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.m_serialPort = dialog.SelectedPort;
                this.m_serialPort.Handshake = Handshake.None;
                this.m_serialPort.Encoding = Encoding.UTF8;
                this.m_setupData.SetString(this.m_setupNode, "Name", this.m_serialPort.PortName);
                this.m_setupData.SetInteger(this.m_setupNode, "Baud", this.m_serialPort.BaudRate);
                this.m_setupData.SetString(this.m_setupNode, "Parity", this.m_serialPort.Parity.ToString());
                this.m_setupData.SetInteger(this.m_setupNode, "Data", this.m_serialPort.DataBits);
                this.m_setupData.SetString(this.m_setupNode, "Stop", this.m_serialPort.StopBits.ToString());
            }
        }

        public void Shutdown()
        {
            this.m_widget.Stop();
        }

        public void Startup()
        {
            if (this.m_widget != null)
            {
                this.m_widget.Dispose();
            }
            this.m_widget = new Widget(this.m_serialPort);
            this.m_widget.Start();
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
                return this.Name;
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
                return "Enttec DMX USB Pro";
            }
        }
    }
}

