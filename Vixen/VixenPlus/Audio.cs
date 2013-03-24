using System;
using System.Xml;

namespace Vixen
{
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
			m_name = node.Attributes["name"].Value;
			m_filename = node.InnerText;
			m_duration = Convert.ToInt32(node.Attributes["duration"].Value);
		}

		public Audio(string name, string filename, int duration)
		{
			m_name = name;
			m_filename = filename;
			m_duration = duration;
		}

		public int Duration
		{
			get { return m_duration; }
			set { m_duration = value; }
		}

		public string FileName
		{
			get { return m_filename; }
			set { m_filename = value; }
		}

		public string Name
		{
			get { return m_name; }
			set { m_name = value; }
		}

		public XmlNode SaveToXml(XmlDocument doc)
		{
			XmlNode node = doc.CreateElement("Audio");
			node.InnerText = m_filename;
			Xml.SetAttribute(node, "name", m_name);
			Xml.SetAttribute(node, "duration", m_duration.ToString());
			return node;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}