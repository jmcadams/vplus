namespace DMXAddIn
{
    using IDMX;
    using System;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;
    using VixenPlus;

    public class DMXAddIn : IAddIn, ILoadable, IPlugIn
    {
        private bool m_autoStart = false;
        private IDMX m_dmxInterface;
        private bool m_DMXRunning = false;

        public bool Execute(EventSequence sequence)
        {
            SetupDialog dialog = new SetupDialog(this.m_autoStart);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.m_autoStart = dialog.AutoStartStream;
                if (!(!this.m_autoStart || this.m_DMXRunning))
                {
                    this.StartStream();
                }
                else if (!(this.m_autoStart || !this.m_DMXRunning))
                {
                    this.m_dmxInterface.Close();
                    this.m_DMXRunning = false;
                }
            }
            dialog.Dispose();
            return false;
        }

        public void Loading(XmlNode dataNode)
        {
            this.m_dmxInterface = new IDMX();
            this.LoadSetup();
            if (this.m_autoStart)
            {
                this.StartStream();
            }
            this.m_DMXRunning = this.m_autoStart;
        }

        private void LoadSetup()
        {
            string path = Path.Combine(Paths.AddinPath, "DMX.data");
            if (File.Exists(path))
            {
                XmlDocument document = new XmlDocument();
                document.Load(path);
                this.m_autoStart = document.SelectSingleNode("//DMX/AutoStart").InnerText == bool.TrueString;
            }
        }

        private void SaveSetup()
        {
            XmlDocument document = new XmlDocument();
            document.AppendChild(document.CreateXmlDeclaration("1.0", "utf-8", string.Empty));
            XmlNode node = document.AppendChild(document.CreateElement("DMX"));
            XmlNode newChild = document.CreateElement("AutoStart");
            newChild.InnerText = this.m_autoStart.ToString();
            node.AppendChild(newChild);
            document.Save(Path.Combine(Paths.AddinPath, "DMX.data"));
        }

        private void StartStream()
        {
            this.m_dmxInterface.Init();
            this.m_dmxInterface.SendData(new byte[0x200]);
            this.m_DMXRunning = true;
        }

        public void Unloading()
        {
            if (this.m_DMXRunning)
            {
                this.m_dmxInterface.Close();
            }
            this.SaveSetup();
        }

        public string Author
        {
            get
            {
                return "Vixen and VixenPlus Developers";
            }
        }

        public LoadableDataLocation DataLocationPreference
        {
            get
            {
                return LoadableDataLocation.Application;
            }
        }

        public string Description
        {
            get
            {
                return "Maintains the DMX data stream";
            }
        }

        public string Name
        {
            get
            {
                return "DMX";
            }
        }
    }
}

