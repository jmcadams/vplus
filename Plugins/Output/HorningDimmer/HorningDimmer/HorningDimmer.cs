namespace HorningDimmer
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class HorningDimmer : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private byte[] m_channelIndex = null;
        private PortMapping[] m_portMappings = new PortMapping[] { new PortMapping(1), new PortMapping(2), new PortMapping(3) };
        private SetupData m_setupData;
        private XmlNode m_setupNode;

        private void CheckSetup()
        {
            this.EnsurePort("Parallel1");
            this.EnsurePort("Parallel2");
            this.EnsurePort("Parallel3");
        }

        private void CompilePortMappings()
        {
            PortMapping mapping = this.m_portMappings[0];
            XmlNode node = this.m_setupNode.SelectSingleNode("Parallel1");
            mapping.From = Convert.ToInt32(node.Attributes["from"].Value);
            mapping.To = Convert.ToInt32(node.Attributes["to"].Value);
            mapping.Mapped = (mapping.To + mapping.From) != 0;
            mapping.Reversed = node.Attributes["reversed"].Value[0] == '1';
            mapping = this.m_portMappings[1];
            node = this.m_setupNode.SelectSingleNode("Parallel2");
            mapping.From = Convert.ToInt32(node.Attributes["from"].Value);
            mapping.To = Convert.ToInt32(node.Attributes["to"].Value);
            mapping.Mapped = (mapping.To + mapping.From) != 0;
            mapping.Reversed = node.Attributes["reversed"].Value[0] == '1';
            mapping = this.m_portMappings[2];
            node = this.m_setupNode.SelectSingleNode("Parallel3");
            mapping.From = Convert.ToInt32(node.Attributes["from"].Value);
            mapping.To = Convert.ToInt32(node.Attributes["to"].Value);
            mapping.Mapped = (mapping.To + mapping.From) != 0;
            mapping.Reversed = node.Attributes["reversed"].Value[0] == '1';
            int num = 0;
            foreach (PortMapping mapping2 in this.m_portMappings)
            {
                if (mapping2.Mapped)
                {
                    num = Math.Max(num, mapping2.To);
                }
            }
            this.m_channelIndex = new byte[num];
            this.SetDimmedChannels(this.m_setupNode.SelectSingleNode("Parallel1/dimming"));
            this.SetDimmedChannels(this.m_setupNode.SelectSingleNode("Parallel2/dimming"));
            this.SetDimmedChannels(this.m_setupNode.SelectSingleNode("Parallel3/dimming"));
        }

        private void EnsurePort(string portName)
        {
            if (this.m_setupNode.SelectSingleNode(portName) == null)
            {
                XmlNode nodeAlways = Xml.GetNodeAlways(this.m_setupNode, portName);
                Xml.SetAttribute(nodeAlways, "from", "0");
                Xml.SetAttribute(nodeAlways, "to", "0");
                Xml.SetAttribute(nodeAlways, "reversed", "0");
                Xml.GetNodeAlways(nodeAlways, "dimming");
            }
        }

        public void Event(byte[] channelValues)
        {
            PortMapping mapping = null;
            PortMapping mapping2 = null;
            byte[] array = new byte[channelValues.Length];
            channelValues.CopyTo(array, 0);
            Array.Reverse(array);
            int length = array.Length;
            foreach (byte num2 in array)
            {
                mapping = null;
                foreach (PortMapping mapping3 in this.m_portMappings)
                {
                    if ((mapping3.Mapped && (length >= mapping3.From)) && (length <= mapping3.To))
                    {
                        mapping = mapping3;
                        break;
                    }
                }
                if (mapping == null)
                {
                    throw new Exception("Attempt to write to unmapped channel " + length.ToString());
                }
                if (mapping != mapping2)
                {
                    if (mapping2 != null)
                    {
                        Out(mapping2.ControlPort, 1);
                        Out(mapping2.ControlPort, 3);
                    }
                    mapping2 = mapping;
                }
                if (this.m_channelIndex[length - 1] == 0)
                {
                    Out(mapping.DataPort, (num2 > 0) ? ((short) 1) : ((short) 0));
                    Out(mapping.ControlPort, 2);
                    Out(mapping.ControlPort, 3);
                }
                else
                {
                    int num3;
                    if (mapping.Reversed)
                    {
                        num3 = 0xff - num2;
                    }
                    else
                    {
                        num3 = num2;
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        Out(mapping.DataPort, (short) ((num3 & 0x80) >> 7));
                        Out(mapping.ControlPort, 2);
                        Out(mapping.ControlPort, 3);
                        num3 = num3 << 1;
                    }
                }
                length--;
            }
            if (mapping != null)
            {
                Out(mapping.ControlPort, 1);
                Out(mapping.ControlPort, 3);
            }
        }

        [DllImport("inpout32", EntryPoint="Inp32")]
        private static extern short In(short port);
        private int IndexOfPort(int dataPort)
        {
            switch (dataPort)
            {
                case 0x278:
                    return 1;

                case 0x378:
                    return 0;

                case 0x3bc:
                    return 2;
            }
            return -1;
        }

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            this.m_setupData = setupData;
            this.m_setupNode = setupNode;
            this.EnsurePort("Parallel1");
            this.EnsurePort("Parallel2");
            this.EnsurePort("Parallel3");
            this.CompilePortMappings();
        }

        [DllImport("inpout32", EntryPoint="Out32")]
        private static extern void Out(short port, short data);
        private void SetDimmedChannels(XmlNode dimmingNode)
        {
            string[] strArray = dimmingNode.InnerText.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in strArray)
            {
                int num = Convert.ToInt32(str);
                if (((num - 1) >= 0) && ((num - 1) < this.m_channelIndex.Length))
                {
                    this.m_channelIndex[num - 1] = 1;
                }
            }
        }

        public void Setup()
        {
            SetupDialog dialog = new SetupDialog(this.m_setupNode);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.CompilePortMappings();
            }
        }

        public void Shutdown()
        {
        }

        public void Startup()
        {
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
                return "Vixen plugin for Ernie Horning's dimming controller";
            }
        }

        public Vixen.HardwareMap[] HardwareMap
        {
            get
            {
                int num = 0;
                foreach (PortMapping mapping in this.m_portMappings)
                {
                    num += mapping.Mapped ? 1 : 0;
                }
                Vixen.HardwareMap[] mapArray = new Vixen.HardwareMap[num];
                num = 0;
                foreach (PortMapping mapping in this.m_portMappings)
                {
                    if (mapping.Mapped)
                    {
                        mapArray[num++] = new Vixen.HardwareMap("Parallel", mapping.DataPort, "X");
                    }
                }
                return mapArray;
            }
        }

        public string Name
        {
            get
            {
                return "Horning Dimmer";
            }
        }
    }
}

