using VixenPlus;

namespace DMX_512
{
    using System.Windows.Forms;
    using System.Xml;
    using IDMX;

    public class Dmx512 : IEventDrivenOutputPlugIn
    {
        private readonly Idmx _dmxInterface;
        private bool _dmxRunning;

        public Dmx512()
        {
            _dmxInterface = new Idmx();
        }

        public void Event(byte[] channelValues)
        {
            _dmxInterface.SendData(channelValues);
        }

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
        }

        public void Setup()
        {
            MessageBox.Show(@"This plugin only supports a single universe.\nNothing to setup.", @"DMX-512", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        public void Shutdown()
        {
            if (!_dmxRunning || (_dmxInterface == null)) {
                return;
            }
            _dmxInterface.Close();
            _dmxRunning = false;
        }

        public void Startup()
        {
            _dmxInterface.Init();
            _dmxRunning = true;
        }

        public override string ToString()
        {
            return Name;
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
                return "Enttec Open DMX output plugin";
            }
        }

        public HardwareMap[] HardwareMap
        {
            get
            {
                return new HardwareMap[0];
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

