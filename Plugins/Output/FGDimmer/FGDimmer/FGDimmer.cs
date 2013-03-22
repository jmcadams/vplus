namespace FGDimmer
{
    using System;
    using System.IO.Ports;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class FGDimmer : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private bool m_acOperation;
        private byte[] m_channelValues;
        private byte[] m_dimmingCurve;
        private Thread m_eventThread;
        private AutoResetEvent m_eventTrigger;
        private bool m_holdPort;
        private Module[] m_modules;
        private byte[][] m_packets;
        private bool m_running;
        private SerialPort m_selectedPort;
        private SetupData m_setupData;
        private XmlNode m_setupNode;
        private int m_startChannel;

        public FGDimmer()
        {
            int num;
            this.m_selectedPort = null;
            this.m_running = false;
            this.m_holdPort = true;
            this.m_acOperation = false;
            this.m_dimmingCurve = new byte[] { 
                0, 0x12, 20, 0x17, 0x18, 0x1b, 0x1c, 0x1f, 0x1f, 0x21, 0x23, 0x25, 0x27, 40, 0x2a, 0x2b, 
                0x2d, 0x2e, 0x30, 0x31, 50, 50, 0x33, 0x34, 0x35, 0x36, 0x36, 0x37, 0x38, 0x38, 0x38, 0x38, 
                0x38, 0x39, 0x39, 0x38, 0x39, 0x3a, 0x3a, 0x3a, 0x3b, 0x3a, 0x3a, 0x3a, 0x3b, 0x3b, 0x3b, 0x3b, 
                60, 0x3d, 60, 0x3d, 0x3d, 0x3d, 0x3d, 0x3e, 0x3e, 0x3e, 0x3e, 0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 
                0x3f, 0x3f, 0x3f, 0x3f, 0x3f, 0x40, 0x40, 0x40, 0x40, 0x41, 0x41, 0x42, 0x42, 0x43, 0x43, 0x44, 
                0x44, 0x44, 0x45, 0x45, 70, 70, 0x47, 0x47, 0x48, 0x49, 0x49, 0x4a, 0x4a, 0x4b, 0x4b, 0x4d, 
                0x4d, 0x4d, 0x4e, 0x4e, 0x4f, 80, 80, 80, 80, 0x51, 0x52, 0x53, 0x53, 0x54, 0x55, 0x55, 
                0x56, 0x57, 0x58, 0x59, 0x5b, 0x5c, 0x5d, 0x5e, 0x60, 0x63, 0x65, 0x68, 0x6b, 0x6d, 0x73, 120, 
                0x87, 140, 0x92, 0x94, 0x97, 0x9a, 0x9c, 0x9f, 0xa1, 0xa2, 0xa3, 0xa4, 0xa6, 0xa7, 0xa8, 0xa9, 
                170, 170, 0xab, 0xac, 0xac, 0xad, 0xae, 0xaf, 0xaf, 0xaf, 0xaf, 0xb0, 0xb1, 0xb1, 0xb2, 0xb2, 
                0xb2, 180, 180, 0xb5, 0xb5, 0xb6, 0xb6, 0xb7, 0xb8, 0xb8, 0xb9, 0xb9, 0xba, 0xba, 0xbb, 0xbb, 
                0xbb, 0xbc, 0xbc, 0xbd, 0xbd, 190, 190, 0xbf, 0xbf, 0xbf, 0xbf, 0xc0, 0xc0, 0xc0, 0xc0, 0xc0, 
                0xc0, 0xc0, 0xc0, 0xc0, 0xc0, 0xc1, 0xc1, 0xc1, 0xc1, 0xc2, 0xc2, 0xc2, 0xc2, 0xc2, 0xc3, 0xc3, 
                0xc4, 0xc4, 0xc4, 0xc4, 0xc4, 0xc5, 0xc5, 0xc5, 0xc5, 0xc5, 0xc5, 0xc6, 0xc6, 0xc6, 0xc7, 0xc7, 
                0xc7, 0xc7, 0xc7, 0xc7, 200, 0xc9, 0xc9, 0xca, 0xcb, 0xcc, 0xcd, 0xcd, 0xce, 0xcf, 0xd1, 210, 
                0xd4, 0xd5, 0xd7, 0xd8, 0xda, 220, 0xde, 0xe0, 0xe0, 0xe3, 0xe4, 0xe7, 0xe8, 0xeb, 0xed, 0xff
             };
            this.m_packets = new byte[][] { new byte[0x22], new byte[0x22], new byte[0x22], new byte[0x22] };
            for (num = 0; num < 4; num++)
            {
                this.m_packets[num][0] = 0x55;
                this.m_packets[num][1] = (byte) (num + 1);
            }
            this.m_modules = new Module[4];
            for (num = 0; num < 4; num++)
            {
                this.m_modules[num] = new Module(num + 1);
            }
            this.m_eventThread = new Thread(new ThreadStart(this.EventThread));
            this.m_eventTrigger = new AutoResetEvent(false);
        }

        public void Event(byte[] channelValues)
        {
            this.m_channelValues = channelValues;
            if (this.m_holdPort)
            {
                this.m_eventTrigger.Set();
            }
            else
            {
                if (!this.m_selectedPort.IsOpen)
                {
                    this.m_selectedPort.Open();
                }
                this.FireEvent();
                this.m_selectedPort.Close();
            }
        }

        private void EventThread()
        {
            while (this.m_running)
            {
                this.m_eventTrigger.WaitOne();
                if (!this.m_running)
                {
                    break;
                }
                this.FireEvent();
            }
        }

        private void FireEvent()
        {
            int num;
            if (!this.m_selectedPort.IsOpen)
            {
                this.m_selectedPort.Open();
            }
            float num2 = 0.3921569f;
            if (this.m_acOperation)
            {
                for (num = 0; num < this.m_channelValues.Length; num++)
                {
                    this.m_channelValues[num] = (byte) ((this.m_dimmingCurve[this.m_channelValues[num]] * num2) + 100f);
                }
            }
            else
            {
                num = 0;
                while (num < this.m_channelValues.Length)
                {
                    this.m_channelValues[num] = (byte) ((this.m_channelValues[num] * num2) + 100f);
                    num++;
                }
            }
            int length = 0;
            Module module = null;
            for (num = 0; num < 4; num++)
            {
                module = this.m_modules[num];
                if (module.Enabled)
                {
                    length = Math.Min(0x20, this.m_channelValues.Length - ((module.StartChannel - 1) - this.m_startChannel));
                    Array.Copy(this.m_channelValues, (module.StartChannel - 1) - this.m_startChannel, this.m_packets[num], 2, length);
                    this.m_selectedPort.Write(this.m_packets[num], 0, this.m_packets[num].Length);
                }
            }
        }

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            this.m_setupData = setupData;
            this.m_setupNode = setupNode;
            this.m_startChannel = int.Parse(this.m_setupNode.Attributes["from"].Value) - 1;
            XmlNode nodeAlways = Xml.GetNodeAlways(setupNode, "Serial");
            this.m_selectedPort = new SerialPort(this.m_setupData.GetString(nodeAlways, "Name", "COM1"), this.m_setupData.GetInteger(nodeAlways, "Baud", 0x1c200), (Parity) Enum.Parse(typeof(Parity), this.m_setupData.GetString(nodeAlways, "Parity", Parity.None.ToString())), this.m_setupData.GetInteger(nodeAlways, "Data", 8), (StopBits) Enum.Parse(typeof(StopBits), this.m_setupData.GetString(nodeAlways, "Stop", StopBits.One.ToString())));
            this.m_selectedPort.Handshake = Handshake.None;
            this.m_selectedPort.Encoding = Encoding.UTF8;
            XmlNode node2 = setupNode.SelectSingleNode("Modules");
            if (node2 != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    XmlNode node3 = node2.SelectSingleNode(string.Format("Module[@id=\"{0}\"]", i + 1));
                    if (node3 != null)
                    {
                        this.m_modules[i].Enabled = bool.Parse(node3.Attributes["enabled"].Value);
                        if (this.m_modules[i].Enabled)
                        {
                            this.m_modules[i].StartChannel = Convert.ToInt32(node3.Attributes["start"].Value);
                        }
                    }
                }
            }
            this.m_holdPort = this.m_setupData.GetBoolean(this.m_setupNode, "HoldPort", false);
            this.m_acOperation = this.m_setupData.GetBoolean(this.m_setupNode, "ACOperation", false);
        }

        public void Setup()
        {
            SetupDialog dialog = new SetupDialog(this.m_selectedPort, this.m_modules, this.m_startChannel + 1, int.Parse(this.m_setupNode.Attributes["to"].Value), this.m_holdPort, this.m_acOperation);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                XmlNode nodeAlways = Xml.GetNodeAlways(this.m_setupNode, "Serial");
                this.m_selectedPort = dialog.SelectedPort;
                this.m_setupData.SetString(nodeAlways, "Name", this.m_selectedPort.PortName);
                this.m_setupData.SetInteger(nodeAlways, "Baud", this.m_selectedPort.BaudRate);
                this.m_setupData.SetString(nodeAlways, "Parity", this.m_selectedPort.Parity.ToString());
                this.m_setupData.SetInteger(nodeAlways, "Data", this.m_selectedPort.DataBits);
                this.m_setupData.SetString(nodeAlways, "Stop", this.m_selectedPort.StopBits.ToString());
                XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(this.m_setupNode, "Modules");
                for (int i = 0; i < 4; i++)
                {
                    XmlNode node = Xml.SetNewValue(emptyNodeAlways, "Module", string.Empty);
                    Xml.SetAttribute(node, "enabled", this.m_modules[i].Enabled.ToString());
                    Xml.SetAttribute(node, "id", this.m_modules[i].ID.ToString());
                    if (this.m_modules[i].Enabled)
                    {
                        Xml.SetAttribute(node, "start", this.m_modules[i].StartChannel.ToString());
                    }
                }
                this.m_holdPort = dialog.HoldPort;
                this.m_acOperation = dialog.ACOperation;
                this.m_setupData.SetBoolean(this.m_setupNode, "HoldPort", this.m_holdPort);
                this.m_setupData.SetBoolean(this.m_setupNode, "ACOperation", this.m_acOperation);
            }
            dialog.Dispose();
        }

        public void Shutdown()
        {
            if (this.m_running)
            {
                this.m_running = false;
                this.m_eventTrigger.Set();
                this.m_eventThread.Join(0x3e8);
            }
            if (this.m_selectedPort.IsOpen)
            {
                this.m_selectedPort.Close();
            }
        }

        public void Startup()
        {
            foreach (byte[] buffer in this.m_packets)
            {
                Array.Clear(buffer, 2, buffer.Length - 2);
            }
            if (this.m_holdPort)
            {
                if (!this.m_selectedPort.IsOpen)
                {
                    this.m_selectedPort.Open();
                }
                this.m_running = true;
                this.m_eventThread.Start();
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
                return "Output plugin for Firegod's 32-channel dimming controller";
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
                return "FG Dimmer";
            }
        }
    }
}

