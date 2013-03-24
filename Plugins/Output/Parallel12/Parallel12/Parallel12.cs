namespace Parallel12
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Xml;
    using VixenPlus;
    using VixenPlus.Dialogs;

    public class Parallel12 : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private ushort m_portAddress = 0;
        private SetupData m_setupData;
        private XmlNode m_setupNode;

        public void Event(byte[] channelValues)
        {
            if (this.m_portAddress == 0)
            {
                throw new Exception("No port has been specified");
            }
            short data = 0;
            for (int i = 0; i < 8; i++)
            {
                data = (short) (data >> 1);
                data = (short) (data | ((channelValues[i] > 0) ? (short)0x80 : (short)0));
            }
            Out(this.m_portAddress, data);
            data = 0;
            if (channelValues[8] == 0)
            {
                data = (short) (data | 1);
            }
            if (channelValues[9] == 0)
            {
                data = (short) (data | 2);
            }
            if (channelValues[10] > 0)
            {
                data = (short) (data | 4);
            }
            if (channelValues[11] == 0)
            {
                data = (short) (data | 8);
            }
            Out((ushort) (this.m_portAddress + 2), data);
        }

        [DllImport("inpout32", EntryPoint="Inp32")]
        private static extern short In(ushort port);
        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            this.m_setupData = setupData;
            this.m_setupNode = setupNode;
            this.m_portAddress = (ushort) this.m_setupData.GetInteger(this.m_setupNode, "address", 0);
        }

        [DllImport("inpout32", EntryPoint="Out32")]
        private static extern void Out(ushort port, short data);
        public void Setup()
        {
            ParallelSetupDialog dialog = new ParallelSetupDialog(this.m_portAddress);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.m_portAddress = dialog.PortAddress;
                this.m_setupData.SetInteger(this.m_setupNode, "address", this.m_portAddress);
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
                return "12-channel parallel plugin for Vixen";
            }
        }

        public VixenPlus.HardwareMap[] HardwareMap
        {
            get
            {
                return new VixenPlus.HardwareMap[] { new VixenPlus.HardwareMap("Parallel", this.m_portAddress, "X") };
            }
        }

        public string Name
        {
            get
            {
                return "12-channel Parallel";
            }
        }
    }
}

