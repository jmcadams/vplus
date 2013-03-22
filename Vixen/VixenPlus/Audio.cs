namespace Vixen
{
    using System;
    using System.Xml;

    public class Audio
    {
        private int m_duration;
        private string m_filename;
        private string m_name;

        public Audio()
        {
        }

        public Audio(XmlNode node)
        {
            this.m_name = node.Attributes["name"].Value;
            this.m_filename = node.InnerText;
            this.m_duration = Convert.ToInt32(node.Attributes["duration"].Value);
        }

        public Audio(string name, string filename, int duration)
        {
            this.m_name = name;
            this.m_filename = filename;
            this.m_duration = duration;
        }

        public XmlNode SaveToXml(XmlDocument doc)
        {
            XmlNode node = doc.CreateElement("Audio");
            node.InnerText = this.m_filename;
            Xml.SetAttribute(node, "name", this.m_name);
            Xml.SetAttribute(node, "duration", this.m_duration.ToString());
            return node;
        }

        public override string ToString()
        {
            return this.Name;
        }

        public int Duration
        {
            get
            {
                return this.m_duration;
            }
            set
            {
                this.m_duration = value;
            }
        }

        public string FileName
        {
            get
            {
                return this.m_filename;
            }
            set
            {
                this.m_filename = value;
            }
        }

        public string Name
        {
            get
            {
                return this.m_name;
            }
            set
            {
                this.m_name = value;
            }
        }
    }
}

