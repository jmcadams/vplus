namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    public class SortOrder
    {
        public List<int> ChannelIndexes;
        public string Name;

        public SortOrder(XmlNode node)
        {
            this.Name = node.Attributes["name"].Value;
            this.ChannelIndexes = new List<int>();
            foreach (string str in node.InnerText.Split(new char[] { ',' }))
            {
                int num;
                if (int.TryParse(str, out num))
                {
                    this.ChannelIndexes.Add(num);
                }
            }
        }

        public SortOrder(string name, List<int> indexes)
        {
            this.Name = name;
            this.ChannelIndexes = new List<int>();
            this.ChannelIndexes.AddRange(indexes);
        }

        internal SortOrder Clone()
        {
            return new SortOrder(this.Name, this.ChannelIndexes);
        }

        public int FindNaturalIndex(int sortedIndex)
        {
            return this.ChannelIndexes.IndexOf(sortedIndex);
        }

        public XmlNode SaveToXml(XmlDocument doc)
        {
            XmlNode node = doc.CreateElement("SortOrder");
            Xml.SetAttribute(node, "name", this.Name);
            StringBuilder builder = new StringBuilder();
            foreach (int num in this.ChannelIndexes)
            {
                builder.AppendFormat("{0},", num);
            }
            node.InnerText = builder.ToString().TrimEnd(new char[] { ',' });
            return node;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}

