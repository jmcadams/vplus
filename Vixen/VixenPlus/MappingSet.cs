using System;
using System.Collections.Generic;
using System.Xml;

namespace VixenPlus
{
	public class MappingSet : ICloneable
	{
		private const string ATTRIBUTE_ID = "id";
		private const string ATTRIBUTE_NAME = "name";
		private const string ELEMENT_INPUT = "Input";
		private const string ELEMENT_INPUTS = "Inputs";
		private readonly Dictionary<ulong, List<string>> m_inputMappings;
		private readonly Dictionary<ulong, List<int>> m_inputOutputMappings;
		private ulong m_id;

		public MappingSet(string name)
		{
			Name = name;
			m_inputMappings = new Dictionary<ulong, List<string>>();
			m_inputOutputMappings = new Dictionary<ulong, List<int>>();
			m_id = Host.GetUniqueKey();
		}

		public MappingSet(XmlNode dataNode)
		{
			m_inputMappings = new Dictionary<ulong, List<string>>();
			m_inputOutputMappings = new Dictionary<ulong, List<int>>();
			ReadData(dataNode);
		}

		public ulong Id
		{
			get { return m_id; }
		}

		public string Name { get; set; }

		public object Clone()
		{
			var set = new MappingSet(Name);
			set.m_id = m_id;
			foreach (ulong num in m_inputMappings.Keys)
			{
				List<string> list;
				set.m_inputMappings[num] = list = new List<string>();
				list.AddRange(m_inputMappings[num]);
			}
			return set;
		}

		public List<string> GetOutputChannelIdList(ulong inputId)
		{
			List<string> list;
			if (!m_inputMappings.TryGetValue(inputId, out list))
			{
				m_inputMappings[inputId] = list = new List<string>();
			}
			return list;
		}

		public List<string> GetOutputChannelIdList(Input input)
		{
			return GetOutputChannelIdList(input.Id);
		}

		internal List<int> GetOutputChannelIndexList(Input input)
		{
			return m_inputOutputMappings[input.Id];
		}

		public bool HasMappingFor(Input input)
		{
			return m_inputMappings.ContainsKey(input.Id);
		}

		public void ReadData(XmlNode dataNode)
		{
			Name = dataNode.Attributes["name"].Value;
			m_id = ulong.Parse(dataNode.Attributes["id"].Value);
			XmlNode node = dataNode["Inputs"];
			foreach (XmlNode node2 in node.SelectNodes("Input"))
			{
				if (node2.InnerText.Trim().Length > 0)
				{
					m_inputMappings[ulong.Parse(node2.Attributes["id"].Value)] = new List<string>(node2.InnerText.Split(new[] {','}));
				}
			}
		}

		public override string ToString()
		{
			return Name;
		}

		public void WriteData(XmlNode dataNode)
		{
			Xml.SetAttribute(dataNode, "name", Name);
			Xml.SetAttribute(dataNode, "id", Id.ToString());
			XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(dataNode, "Inputs");
			foreach (ulong num in m_inputMappings.Keys)
			{
				if (m_inputMappings[num].Count > 0)
				{
					Xml.SetAttribute(Xml.SetNewValue(emptyNodeAlways, "Input", string.Join(",", m_inputMappings[num].ToArray())), "id",
					                 num.ToString());
				}
			}
		}
	}
}