namespace EventDataLogger
{
    using System;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml;
    using VixenPlus;

    public class EventDataLogger : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private string m_filePath;
        private SetupData m_setupData;
        private XmlNode m_setupNode;

        public void Event(byte[] channelValues)
        {
            StringBuilder builder = new StringBuilder();
            foreach (byte num in channelValues)
            {
                builder.AppendFormat("{0:X2} ", num);
            }
            File.AppendAllText(this.m_filePath, string.Format("{0:MM-dd-yyyy HH:mm:ss}.{1:D3} [{2,3}] {3}\n", new object[] { DateTime.Now, DateTime.Now.TimeOfDay.Milliseconds, channelValues.Length, builder.ToString() }));
        }

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            this.m_setupData = setupData;
            this.m_setupNode = setupNode;
            this.m_filePath = Xml.GetNodeAlways(this.m_setupNode, "LastFilePath").InnerText;
            if (this.m_filePath == string.Empty)
            {
                this.m_filePath = Path.GetTempFileName();
                Xml.GetNodeAlways(this.m_setupNode, "LastFilePath").InnerText = this.m_filePath;
            }
        }

        public void Setup()
        {
            SetupDialog dialog = new SetupDialog(this.m_filePath);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
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
                return "Logs event data routed to it.";
            }
        }

        public VixenPlus.HardwareMap[] HardwareMap
        {
            get
            {
                return new VixenPlus.HardwareMap[0];
            }
        }

        public string Name
        {
            get
            {
                return "Event data logger";
            }
        }
    }
}

