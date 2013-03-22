namespace Olsen595
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class Olsen595 : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private PortMapping[] m_portMappings = new PortMapping[] { new PortMapping(1), new PortMapping(2), new PortMapping(3) };
        private SetupData m_setupData;
        private XmlNode m_setupNode;

        private void CompilePortMappings()
        {
            PortMapping mapping = this.m_portMappings[0];
            XmlNode node = this.m_setupNode.SelectSingleNode("parallel1");
            mapping.From = Convert.ToInt32(node.Attributes["from"].Value);
            mapping.To = Convert.ToInt32(node.Attributes["to"].Value);
            mapping.Mapped = (mapping.From != 0) && (mapping.To != 0);
            mapping = this.m_portMappings[1];
            node = this.m_setupNode.SelectSingleNode("parallel2");
            mapping.From = Convert.ToInt32(node.Attributes["from"].Value);
            mapping.To = Convert.ToInt32(node.Attributes["to"].Value);
            mapping.Mapped = (mapping.From != 0) && (mapping.To != 0);
            mapping = this.m_portMappings[2];
            node = this.m_setupNode.SelectSingleNode("parallel3");
            mapping.From = Convert.ToInt32(node.Attributes["from"].Value);
            mapping.To = Convert.ToInt32(node.Attributes["to"].Value);
            mapping.Mapped = (mapping.From != 0) && (mapping.To != 0);
        }

        public void Event(byte[] channelValues)
        {
            foreach (PortMapping mapping3 in this.m_portMappings)
            {
                if (mapping3.Mapped)
                {
                    int num2 = mapping3.From - 1;
                    int num3 = Math.Min(channelValues.Length, mapping3.To) - 1;
                    for (int i = num3; i >= num2; i--)
                    {
                        Out(mapping3.DataPort, (channelValues[i] > 0) ? ((short) 1) : ((short) 0));
                        Out(mapping3.ControlPort, 2);
                        Out(mapping3.ControlPort, 3);
                    }
                    Out(mapping3.ControlPort, 1);
                    Out(mapping3.ControlPort, 3);
                }
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
            if (this.m_setupNode.SelectSingleNode("parallel1") == null)
            {
                Xml.SetAttribute(this.m_setupNode, "parallel1", "from", "0");
                Xml.SetAttribute(this.m_setupNode, "parallel1", "to", "0");
            }
            if (this.m_setupNode.SelectSingleNode("parallel2") == null)
            {
                Xml.SetAttribute(this.m_setupNode, "parallel2", "from", "0");
                Xml.SetAttribute(this.m_setupNode, "parallel2", "to", "0");
            }
            if (this.m_setupNode.SelectSingleNode("parallel3") == null)
            {
                Xml.SetAttribute(this.m_setupNode, "parallel3", "from", "0");
                Xml.SetAttribute(this.m_setupNode, "parallel3", "to", "0");
            }
            this.CompilePortMappings();
        }

        [DllImport("inpout32", EntryPoint="Out32")]
        private static extern void Out(short port, short data);
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
                return "Vixen Developers";
            }
        }

        public string Description
        {
            get
            {
                return "Multi-port Olsen 595 plugin for Vixen";
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
                return "Olsen 595";
            }
        }
    }
}

