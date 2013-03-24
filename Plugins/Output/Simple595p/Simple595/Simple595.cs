namespace Simple595
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Xml;
    using VixenPlus;

    public class Simple595 : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private ushort m_portAddress = 0;
        private int m_pulseWidth = 1;
        private SetupData m_setupData;
        private XmlNode m_setupNode;

        public void Event(byte[] channelValues)
        {
            int num;
            if (this.m_portAddress == 0)
            {
                throw new Exception("No port has been specified.");
            }
            byte[] array = new byte[channelValues.Length];
            channelValues.CopyTo(array, 0);
            Array.Reverse(array);
            ushort port = (ushort) (this.m_portAddress + 2);
            foreach (byte num3 in array)
            {
                Out(this.m_portAddress, (num3 > 0) ? ((short) 1) : ((short) 0));
                num = 0;
                while (num < this.m_pulseWidth)
                {
                    Out(port, 2);
                    num++;
                }
                Out(port, 3);
            }
            for (num = 0; num < this.m_pulseWidth; num++)
            {
                Out(port, 1);
            }
            Out(port, 3);
        }

        [DllImport("inpout32", EntryPoint="Inp32")]
        private static extern short In(ushort port);
        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            this.m_setupData = setupData;
            this.m_setupNode = setupNode;
            this.m_portAddress = (ushort) this.m_setupData.GetInteger(this.m_setupNode, "Address", 0x378);
            this.m_pulseWidth = this.m_setupData.GetInteger(this.m_setupNode, "PulseWidth", 1);
        }

        [DllImport("inpout32", EntryPoint="Out32")]
        private static extern void Out(ushort port, short data);
        public void Setup()
        {
            SetupDialog dialog = new SetupDialog(this.m_portAddress, this.m_pulseWidth);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.m_pulseWidth = dialog.PulseWidth;
                this.m_portAddress = (ushort) dialog.PortAddress;
                this.m_setupData.SetInteger(this.m_setupNode, "Address", this.m_portAddress);
                this.m_setupData.SetInteger(this.m_setupNode, "PulseWidth", this.m_pulseWidth);
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
                return "Simple 595 plugin for Vixen with pulse width";
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
                return "Simple 595 (with pulse width)";
            }
        }
    }
}

