namespace DMX_512
{
    using IDMX;
    using System;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class DMX_512 : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private IDMX m_dmxInterface = null;
        private bool m_dmxRunning = false;
        private SetupData m_setupData = null;
        private XmlNode m_setupNode = null;

        public DMX_512()
        {
            this.m_dmxInterface = new IDMX();
        }

        public void Event(byte[] channelValues)
        {
            this.m_dmxInterface.SendData(channelValues);
        }

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            this.m_setupData = setupData;
            this.m_setupNode = setupNode;
        }

        public void Setup()
        {
            MessageBox.Show("This plugin only supports a single universe.\nNothing to setup.", "DMX-512", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        public void Shutdown()
        {
            if (this.m_dmxRunning && (this.m_dmxInterface != null))
            {
                this.m_dmxInterface.Close();
                this.m_dmxRunning = false;
            }
        }

        public void Startup()
        {
            this.m_dmxInterface.Init();
            this.m_dmxRunning = true;
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
                return "Enttec Open DMX output plugin";
            }
        }

        public Vixen.HardwareMap[] HardwareMap
        {
            get
            {
                return new Vixen.HardwareMap[0];
            }
        }

        public string Name
        {
            get
            {
                return "Enttec Open DMX";
            }
        }
    }
}

