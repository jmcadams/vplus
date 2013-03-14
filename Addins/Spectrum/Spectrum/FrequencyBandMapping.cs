namespace Spectrum
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using Vixen;

    internal class FrequencyBandMapping
    {
        private List<int> m_channelList;
        private float m_responseLevel;

        public FrequencyBandMapping(float responseLevel)
        {
            this.m_responseLevel = responseLevel;
            this.m_channelList = new List<int>();
        }

        public FrequencyBandMapping(XmlNode node)
        {
            this.m_responseLevel = float.Parse(node.Attributes["responseLevel"].Value);
            this.m_channelList = new List<int>();
            if (node.InnerText.Length > 0)
            {
                foreach (string str in node.InnerText.Split(new char[] { ',' }))
                {
                    this.m_channelList.Add(int.Parse(str));
                }
            }
        }

        public XmlNode SaveToXml(XmlNode contextNode)
        {
            List<string> list = new List<string>();
            foreach (int num in this.m_channelList)
            {
                list.Add(num.ToString());
            }
            XmlNode node = Xml.SetNewValue(contextNode, "Band", string.Join(",", list.ToArray()));
            Xml.SetAttribute(node, "responseLevel", this.m_responseLevel.ToString());
            return node;
        }

        public List<int> ChannelList
        {
            get
            {
                return this.m_channelList;
            }
        }

        public float ResponseLevel
        {
            get
            {
                return this.m_responseLevel;
            }
        }
    }
}

