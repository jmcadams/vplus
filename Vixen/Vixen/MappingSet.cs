namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    public class MappingSet : ICloneable
    {
        private const string ATTRIBUTE_ID = "id";
        private const string ATTRIBUTE_NAME = "name";
        private const string ELEMENT_INPUT = "Input";
        private const string ELEMENT_INPUTS = "Inputs";
        private ulong m_id;
        private Dictionary<ulong, List<string>> m_inputMappings;
        private Dictionary<ulong, List<int>> m_inputOutputMappings;
        private string m_name;

        public MappingSet(string name)
        {
            this.m_name = name;
            this.m_inputMappings = new Dictionary<ulong, List<string>>();
            this.m_inputOutputMappings = new Dictionary<ulong, List<int>>();
            this.m_id = Host.GetUniqueKey();
        }

        public MappingSet(XmlNode dataNode)
        {
            this.m_inputMappings = new Dictionary<ulong, List<string>>();
            this.m_inputOutputMappings = new Dictionary<ulong, List<int>>();
            this.ReadData(dataNode);
        }

        public object Clone()
        {
            MappingSet set = new MappingSet(this.Name);
            set.m_id = this.m_id;
            foreach (ulong num in this.m_inputMappings.Keys)
            {
                List<string> list;
                set.m_inputMappings[num] = list = new List<string>();
                list.AddRange(this.m_inputMappings[num]);
            }
            return set;
        }

        public List<string> GetOutputChannelIdList(ulong inputId)
        {
            List<string> list;
            if (!this.m_inputMappings.TryGetValue(inputId, out list))
            {
                this.m_inputMappings[inputId] = list = new List<string>();
            }
            return list;
        }

        public List<string> GetOutputChannelIdList(Input input)
        {
            return this.GetOutputChannelIdList(input.Id);
        }

        internal List<int> GetOutputChannelIndexList(Input input)
        {
            return this.m_inputOutputMappings[input.Id];
        }

        public bool HasMappingFor(Input input)
        {
            return this.m_inputMappings.ContainsKey(input.Id);
        }

        public void ReadData(XmlNode dataNode)
        {
            this.Name = dataNode.Attributes["name"].Value;
            this.m_id = ulong.Parse(dataNode.Attributes["id"].Value);
            XmlNode node = dataNode["Inputs"];
            foreach (XmlNode node2 in node.SelectNodes("Input"))
            {
                if (node2.InnerText.Trim().Length > 0)
                {
                    this.m_inputMappings[ulong.Parse(node2.Attributes["id"].Value)] = new List<string>(node2.InnerText.Split(new char[] { ',' }));
                }
            }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public void WriteData(XmlNode dataNode)
        {
            Xml.SetAttribute(dataNode, "name", this.Name);
            Xml.SetAttribute(dataNode, "id", this.Id.ToString());
            XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(dataNode, "Inputs");
            foreach (ulong num in this.m_inputMappings.Keys)
            {
                if (this.m_inputMappings[num].Count > 0)
                {
                    Xml.SetAttribute(Xml.SetNewValue(emptyNodeAlways, "Input", string.Join(",", this.m_inputMappings[num].ToArray())), "id", num.ToString());
                }
            }
        }

        public ulong Id
        {
            get
            {
                return this.m_id;
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

