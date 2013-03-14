namespace MIDIReader
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using Vixen;

    internal class KeyChannelMapping
    {
        private List<int> m_channelList;
        private int m_keyID;

        public KeyChannelMapping(int keyID)
        {
            this.m_keyID = keyID;
            this.m_channelList = new List<int>();
        }

        public KeyChannelMapping(XmlNode node)
        {
            this.m_keyID = int.Parse(node.Attributes["id"].Value);
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
            XmlNode node = Xml.SetNewValue(contextNode, "Key", string.Join(",", list.ToArray()));
            Xml.SetAttribute(node, "id", this.m_keyID.ToString());
            return node;
        }

        public List<int> ChannelList
        {
            get
            {
                return this.m_channelList;
            }
        }

        public int KeyID
        {
            get
            {
                return this.m_keyID;
            }
        }
    }
}

