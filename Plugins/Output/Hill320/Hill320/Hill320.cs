namespace Hill320
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;
    using Vixen.Dialogs;

    public class Hill320 : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private PortMapping m_port = null;
        private int m_portAddress;
        private SetupData m_setupData;
        private XmlNode m_setupNode;

        public void Event(byte[] channelValues)
        {
            int num = 0;
            int num6 = channelValues.Length >> 3;
            for (int i = 0; i < num6; i++)
            {
                byte data = 0;
                for (int j = 0; j < 8; j++)
                {
                    data = (byte) (data >> 1);
                    data = (byte) (data | ((channelValues[num++] > 0) ? 0x80 : 0));
                }
                byte num5 = (byte) (((int) 8) << (i >> 3));
                byte num4 = (byte) (i % 8);
                Out(this.m_port.DataPort, data);
                Out(this.m_port.ControlPort, 0);
                Out(this.m_port.ControlPort, 1);
                Out(this.m_port.DataPort, (ushort) (num5 | num4));
                Out(this.m_port.ControlPort, 3);
                Out(this.m_port.ControlPort, 1);
            }
        }

        [DllImport("inpout32", EntryPoint="Inp32")]
        private static extern short In(ushort port);
        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            this.m_setupData = setupData;
            this.m_setupNode = setupNode;
            this.m_portAddress = this.m_setupData.GetInteger(this.m_setupNode, "port", 0);
            this.m_port = new PortMapping(this.m_portAddress);
        }

        [DllImport("inpout32", EntryPoint="Out32")]
        private static extern void Out(ushort port, ushort data);
        public void Setup()
        {
            ParallelSetupDialog dialog = new ParallelSetupDialog(this.m_portAddress);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.m_portAddress = dialog.PortAddress;
                this.m_setupData.SetInteger(this.m_setupNode, "port", this.m_portAddress);
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
                return "Vixen and VixenPlus Developers";
            }
        }

        public string Description
        {
            get
            {
                return "Hill 320 plugin for Vixen";
            }
        }

        public Vixen.HardwareMap[] HardwareMap
        {
            get
            {
                return new Vixen.HardwareMap[] { new Vixen.HardwareMap("Parallel", this.m_portAddress, "X") };
            }
        }

        public string Name
        {
            get
            {
                return "Hill 320";
            }
        }
    }
}

