using System;
using System.Globalization;
using System.Xml;

namespace VixenPlus
{
	public class Audio
	{
		private int _duration;
		private string _filename;
		private string _name;

		public Audio()
		{
		}

		public Audio(XmlNode node)
		{
			if (node.Attributes != null)
			{
				_name = node.Attributes["name"].Value;
				_filename = node.InnerText;
				_duration = Convert.ToInt32(node.Attributes["duration"].Value);
			}
		}

		public Audio(string name, string filename, int duration)
		{
			_name = name;
			_filename = filename;
			_duration = duration;
		}

		public int Duration
		{
			get { return _duration; }
			set { _duration = value; }
		}

		public string FileName
		{
			get { return _filename; }
			set { _filename = value; }
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public XmlNode SaveToXml(XmlDocument doc)
		{
			XmlNode node = doc.CreateElement("Audio");
			node.InnerText = _filename;
			Xml.SetAttribute(node, "name", _name);
			Xml.SetAttribute(node, "duration", _duration.ToString(CultureInfo.InvariantCulture));
			return node;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}