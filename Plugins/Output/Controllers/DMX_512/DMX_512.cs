using System.Windows.Forms;
using System.Xml;

using Controllers.IDMX;

using VixenPlus;
using VixenPlus.Annotations;

namespace Controllers.DMX_512
{
    [UsedImplicitly]
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

        public Control Setup()
        {
            //MessageBox.Show(@"This plugin only supports a single universe.\nNothing to setup.", @"DMX-512", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            
            return null;
        }


        public void GetSetup() {
        }


        public void CloseSetup() {
        }


        public bool SupportsLiveSetup() {
            return true;
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


        [UsedImplicitly]
        public string Author
        {
            get
            {
                return "Vixen and VixenPlus Developers";
            }
        }

        [UsedImplicitly]
        public string Description
        {
            get
            {
                return "Enttec Open DMX output plugin";
            }
        }

        public string HardwareMap
        {
            get { return "Only supports a single universe. Nothing to setup."; }
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

