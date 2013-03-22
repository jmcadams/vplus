namespace RGBLED
{
    using System;
    using System.Collections.Generic;
    using System.IO.Ports;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class RGBLED : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private List<Channel> m_channels;
        private int[,] m_controllerChannelRanges;
        private byte[] m_hexTable = new byte[] { 0x30, 0x31, 50, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x41, 0x42, 0x43, 0x44, 0x45, 70 };
        private byte[] m_packet = new byte[] { 0x23, 0, 0, 0x30, 50, 0, 0, 0, 0, 0, 0, 0, 0, 13 };
        private SerialPort m_selectedPort = null;
        private SetupData m_setupData;
        private XmlNode m_setupNode;

        public void Event(byte[] channelValues)
        {
            if (this.m_selectedPort.IsOpen)
            {
                int num = 0;
                int num3 = 0;
                int num4 = this.m_controllerChannelRanges[0, 1];
                int num5 = channelValues.Length - (channelValues.Length % 3);
                for (int i = 0; i < num5; i += 3)
                {
                    this.m_packet[1] = this.m_hexTable[num >> 4];
                    this.m_packet[2] = this.m_hexTable[num & 15];
                    int num2 = (i / 3) - num3;
                    this.m_packet[5] = this.m_hexTable[num2 >> 4];
                    this.m_packet[6] = this.m_hexTable[num2 & 15];
                    byte num6 = channelValues[i];
                    byte num7 = channelValues[i + 1];
                    byte num8 = channelValues[i + 2];
                    this.m_packet[7] = this.m_hexTable[num6 >> 4];
                    this.m_packet[8] = this.m_hexTable[num6 & 15];
                    this.m_packet[9] = this.m_hexTable[num7 >> 4];
                    this.m_packet[10] = this.m_hexTable[num7 & 15];
                    this.m_packet[11] = this.m_hexTable[num8 >> 4];
                    this.m_packet[12] = this.m_hexTable[num8 & 15];
                    this.m_selectedPort.Write(this.m_packet, 0, this.m_packet.Length);
                    if ((i / 3) == num4)
                    {
                        num++;
                        if (num < this.m_controllerChannelRanges.GetLength(0))
                        {
                            num3 = num4 + 1;
                            num4 = this.m_controllerChannelRanges[num4, 1];
                        }
                    }
                }
            }
        }

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            this.m_setupData = setupData;
            this.m_setupNode = setupNode;
            this.m_channels = executableObject.Channels;
            this.m_selectedPort = new SerialPort(this.m_setupData.GetString(this.m_setupNode, "name", "COM1"), this.m_setupData.GetInteger(this.m_setupNode, "baud", 0x4b00), (Parity) Enum.Parse(typeof(Parity), this.m_setupData.GetString(this.m_setupNode, "parity", Parity.None.ToString())), this.m_setupData.GetInteger(this.m_setupNode, "data", 8), (StopBits) Enum.Parse(typeof(StopBits), this.m_setupData.GetString(this.m_setupNode, "stop", StopBits.One.ToString())));
            this.m_selectedPort.Handshake = Handshake.None;
            this.m_selectedPort.Encoding = Encoding.UTF8;
            this.m_setupData.GetString(this.m_setupNode, "Controllers", string.Empty);
        }

        public void Setup()
        {
            SetupDialog dialog = new SetupDialog(this.m_selectedPort, this.m_setupNode);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.m_selectedPort = dialog.SelectedPort;
                this.m_setupData.SetString(this.m_setupNode, "name", this.m_selectedPort.PortName);
                this.m_setupData.SetInteger(this.m_setupNode, "baud", this.m_selectedPort.BaudRate);
                this.m_setupData.SetString(this.m_setupNode, "parity", this.m_selectedPort.Parity.ToString());
                this.m_setupData.SetInteger(this.m_setupNode, "data", this.m_selectedPort.DataBits);
                this.m_setupData.SetString(this.m_setupNode, "stop", this.m_selectedPort.StopBits.ToString());
            }
            XmlNode contextNode = this.m_setupNode.SelectSingleNode("Controllers");
            contextNode.RemoveAll();
            int num = 0;
            foreach (string str in dialog.Controllers)
            {
                XmlAttribute attribute;
                XmlNode nodeAlways = Xml.GetNodeAlways(contextNode, "Controller");
                nodeAlways.Attributes.Append(attribute = nodeAlways.OwnerDocument.CreateAttribute("id"));
                attribute.InnerText = num.ToString();
                num++;
                nodeAlways.Attributes.Append(attribute = nodeAlways.OwnerDocument.CreateAttribute("config"));
                attribute.InnerText = str;
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
            XmlNodeList list = this.m_setupNode.SelectNodes("Controllers/Controller");
            this.m_controllerChannelRanges = new int[list.Count, 2];
            int num = 0;
            for (int i = 0; i < list.Count; i++)
            {
                int num2;
                this.m_controllerChannelRanges[i, 0] = num;
                if (list[i].Attributes["config"].Value == "hardware")
                {
                    num2 = 13;
                }
                else
                {
                    num2 = 3;
                }
                this.m_controllerChannelRanges[i, 1] = (num + num2) - 1;
                num += num2;
            }
            this.m_selectedPort.Open();
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
                return "RGBLED plugin for Vixen\n(http://www.rgbled.org/RGBLED/index.html)";
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
                return "RGBLED";
            }
        }
    }
}

