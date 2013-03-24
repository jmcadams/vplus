namespace Simple595
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Xml;
    using VixenPlus;
    using VixenPlus.Dialogs;

    public class InvertedSimple595 : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private ushort m_portAddress = 0;
		//private int m_portNumber = 0;
        private SetupData m_setupData;
        private XmlNode m_setupNode;

        public void Event(byte[] channelValues)
        {
            if (this.m_portAddress == 0)
            {
                throw new Exception("No port has been specified.");
            }
            byte[] array = new byte[channelValues.Length];
            channelValues.CopyTo(array, 0);
            Array.Reverse(array);
            ushort port = (ushort) (this.m_portAddress + 2);
            foreach (byte num2 in array)
            {
                Out(this.m_portAddress, (num2 > 0) ? ((short) 0) : ((short) 1));
                Out(port, 2);
                Out(port, 3);
            }
            Out(port, 1);
            Out(port, 3);
        }

        [DllImport("inpout32", EntryPoint="Inp32")]
        private static extern short In(ushort port);
        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            this.m_setupData = setupData;
            this.m_setupNode = setupNode;
            this.m_portAddress = (ushort) this.m_setupData.GetInteger(this.m_setupNode, "address", 0x378);
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
                return "Simple 595 plugin for Vixen";
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
                return "Simple 595";
            }
        }
    }
}

