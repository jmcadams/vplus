namespace Spectrum
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using Vixen;

    internal class FrequencyBandMapping
    {
        private List<int> m_channelList;
        private float m_responseLevelMax;
        private float m_responseLevelMin;

        public FrequencyBandMapping(XmlNode node)
        {
            this.m_responseLevelMin = float.Parse(node.Attributes["responseLevelMin"].Value);
            this.m_responseLevelMax = float.Parse(node.Attributes["responseLevelMax"].Value);
            this.m_channelList = new List<int>();
            if (node.InnerText.Length > 0)
            {
                foreach (string str in node.InnerText.Split(new char[] { ',' }))
                {
                    this.m_channelList.Add(int.Parse(str));
                }
            }
        }

        public FrequencyBandMapping(float responseLevelMin, float responseLevelMax)
        {
            this.m_responseLevelMin = responseLevelMin;
            this.m_responseLevelMax = responseLevelMax;
            this.m_channelList = new List<int>();
        }

        public XmlNode SaveToXml(XmlNode contextNode)
        {
            List<string> list = new List<string>();
            foreach (int num in this.m_channelList)
            {
                list.Add(num.ToString());
            }
            XmlNode node = Xml.SetNewValue(contextNode, "Band", string.Join(",", list.ToArray()));
            Xml.SetAttribute(node, "responseLevelMin", this.m_responseLevelMin.ToString());
            Xml.SetAttribute(node, "responseLevelMax", this.m_responseLevelMax.ToString());
            return node;
        }

        public List<int> ChannelList
        {
            get
            {
                return this.m_channelList;
            }
        }

        public float ResponseLevelMax
        {
            get
            {
                return this.m_responseLevelMax;
            }
        }

        public float ResponseLevelMin
        {
            get
            {
                return this.m_responseLevelMin;
            }
        }
    }
}

