namespace Launcher
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;

    public class Launcher : IEventDrivenOutputPlugIn, IOutputPlugIn, IHardwarePlugin, IPlugIn, ISetup
    {
        private SetupData m_setupData;
        private XmlNode m_setupNode;
        private Dictionary<byte, string[]> m_targets = new Dictionary<byte, string[]>();

        public void Event(byte[] channelValues)
        {
            foreach (byte num in channelValues)
            {
                string[] strArray;
                if (this.m_targets.TryGetValue(num, out strArray))
                {
                    Process process = new Process();
                    process.StartInfo = new ProcessStartInfo(strArray[0], strArray[1]);
                    process.Start();
                }
            }
        }

        public void Initialize(IExecutable executableObject, SetupData setupData, XmlNode setupNode)
        {
            this.m_setupData = setupData;
            this.m_setupNode = setupNode;
            if (this.m_setupNode.SelectSingleNode("Programs") == null)
            {
                Xml.GetNodeAlways(this.m_setupNode, "Programs");
            }
        }

        public void Setup()
        {
            XmlNodeList list = this.m_setupNode.SelectNodes("Programs/Program");
            string[][] programs = new string[list.Count][];
            int num = 0;
            foreach (XmlNode node in list)
            {
                programs[num++] = new string[] { node.InnerText, node.Attributes["params"].Value, node.Attributes["trigger"].Value };
            }
            SetupDialog dialog = new SetupDialog(programs);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                XmlNode contextNode = this.m_setupNode.SelectSingleNode("Programs");
                contextNode.RemoveAll();
                foreach (string[] strArray2 in dialog.Programs)
                {
                    XmlNode node3 = Xml.SetNewValue(contextNode, "Program", strArray2[0]);
                    Xml.SetAttribute(node3, "params", strArray2[1]);
                    Xml.SetAttribute(node3, "trigger", strArray2[2]);
                }
            }
        }

        public void Shutdown()
        {
        }

        public void Startup()
        {
            this.m_targets.Clear();
            foreach (XmlNode node in this.m_setupNode.SelectNodes("Programs/Program"))
            {
                byte num = (byte) Math.Round((double) ((Convert.ToSingle(node.Attributes["trigger"].Value) * 255f) / 100f), MidpointRounding.AwayFromZero);
                if (File.Exists(node.InnerText))
                {
                    this.m_targets[num] = new string[] { node.InnerText, node.Attributes["params"].Value };
                }
            }
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
                return "External program launcher";
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
                return "Launcher";
            }
        }
    }
}

