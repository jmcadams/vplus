using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using VixenPlus;

namespace Spectrum
{
	internal class FrequencyBandMapping
	{
		private readonly List<int> _mChannelList;
		private readonly float _mResponseLevelMax;
		private readonly float _mResponseLevelMin;

		public FrequencyBandMapping(XmlNode node)
		{
			if (node.Attributes != null)
			{
				_mResponseLevelMin = float.Parse(node.Attributes["responseLevelMin"].Value);
				_mResponseLevelMax = float.Parse(node.Attributes["responseLevelMax"].Value);
			}
			_mChannelList = new List<int>();
			if (node.InnerText.Length > 0)
			{
				foreach (string str in node.InnerText.Split(new[] {','}))
				{
					_mChannelList.Add(int.Parse(str));
				}
			}
		}

		public FrequencyBandMapping(float responseLevelMin, float responseLevelMax)
		{
			_mResponseLevelMin = responseLevelMin;
			_mResponseLevelMax = responseLevelMax;
			_mChannelList = new List<int>();
		}

		public List<int> ChannelList
		{
			get { return _mChannelList; }
		}

		public float ResponseLevelMax
		{
			get { return _mResponseLevelMax; }
		}

		public float ResponseLevelMin
		{
			get { return _mResponseLevelMin; }
		}

		public XmlNode SaveToXml(XmlNode contextNode)
		{
			var list = new List<string>();
			foreach (int num in _mChannelList)
			{
				list.Add(num.ToString(CultureInfo.InvariantCulture));
			}
			XmlNode node = Xml.SetNewValue(contextNode, "Band", string.Join(",", list.ToArray()));
			Xml.SetAttribute(node, "responseLevelMin", _mResponseLevelMin.ToString(CultureInfo.InvariantCulture));
			Xml.SetAttribute(node, "responseLevelMax", _mResponseLevelMax.ToString(CultureInfo.InvariantCulture));
			return node;
		}
	}
}