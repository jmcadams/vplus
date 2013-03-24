using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Xml;

namespace Vixen
{
	public class Channel : IDisposable, IComparable<Channel>
	{
		private readonly ulong _id;
		private SolidBrush _solidBrush;
		private Color _color;
		private byte[] _dimmingCurve;
		private bool _enabled;
		private string _name;
		private int _outputChannel;

		public Channel(XmlNode channelNode)
		{
			_solidBrush = null;
			_outputChannel = 0;
			_enabled = true;
			_dimmingCurve = null;
			if (channelNode.Attributes != null)
			{
				_name = channelNode.Attributes["name"].Value;
				Color = Color.FromArgb(Convert.ToInt32(channelNode.Attributes["color"].Value));
				_outputChannel = Convert.ToInt32(channelNode.Attributes["output"].Value);
				_id = ulong.Parse(channelNode.Attributes["id"].Value);
				_enabled = bool.Parse(channelNode.Attributes["enabled"].Value);
				if (channelNode["Curve"] != null)
				{
					_dimmingCurve = new byte[0x100];
					string[] strArray = channelNode["Curve"].InnerText.Split(new[] {','});
					int num = Math.Min(strArray.Length, 0x100);
					for (int i = 0; i < num; i++)
					{
						byte num2;
						if (byte.TryParse(strArray[i], out num2))
						{
							_dimmingCurve[i] = num2;
						}
						else
						{
							_dimmingCurve[i] = (byte) i;
						}
					}
				}
			}
		}

		public Channel(string name, int outputChannel)
		{
			_solidBrush = null;
			_outputChannel = 0;
			_enabled = true;
			_dimmingCurve = null;
			_name = name;
			Color = Color.FromArgb(-1);
			_id = Host.GetUniqueKey();
			_outputChannel = outputChannel;
		}

		public Channel(string name, Color color, int outputChannel)
		{
			_solidBrush = null;
			_outputChannel = 0;
			_enabled = true;
			_dimmingCurve = null;
			_name = name;
			Color = color;
			_id = Host.GetUniqueKey();
			_outputChannel = outputChannel;
		}

		public Channel(string name, int outputChannel, bool ensureUniqueId)
		{
			_solidBrush = null;
			_outputChannel = 0;
			_enabled = true;
			_dimmingCurve = null;
			_name = name;
			Color = Color.FromArgb(-1);
			if (ensureUniqueId)
			{
				long ticks = DateTime.Now.Ticks;
				while (ticks == DateTime.Now.Ticks)
				{
				}
			}
			_id = Host.GetUniqueKey();
			_outputChannel = outputChannel;
		}

		public Channel(string name, Color color, int outputChannel, bool ensureUniqueId)
		{
			_solidBrush = null;
			_outputChannel = 0;
			_enabled = true;
			_dimmingCurve = null;
			_name = name;
			Color = color;
			if (ensureUniqueId)
			{
				long ticks = DateTime.Now.Ticks;
				while (ticks == DateTime.Now.Ticks)
				{
				}
			}
			_id = Host.GetUniqueKey();
			_outputChannel = outputChannel;
		}

		public SolidBrush Brush
		{
			get { return _solidBrush; }
		}

		public Color Color
		{
			get { return _color; }
			set
			{
				if (_color.ToArgb() != value.ToArgb())
				{
					_color = value;
					if (_solidBrush != null)
					{
						_solidBrush.Dispose();
					}
					_solidBrush = new SolidBrush(value);
				}
			}
		}

		public byte[] DimmingCurve
		{
			get { return _dimmingCurve; }
			set { _dimmingCurve = value; }
		}

		public bool Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

		public ulong Id
		{
			get { return _id; }
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public int OutputChannel
		{
			get { return _outputChannel; }
			set { _outputChannel = value; }
		}

		public int CompareTo(Channel other)
		{
			return Id.CompareTo(other.Id);
		}

		public void Dispose()
		{
			if (_solidBrush != null)
			{
				_solidBrush.Dispose();
				_solidBrush = null;
			}
			GC.SuppressFinalize(this);
		}

		public Channel Clone()
		{
			var channel = (Channel) MemberwiseClone();
			channel._solidBrush = new SolidBrush(_solidBrush.Color);
			if (_dimmingCurve != null)
			{
				channel._dimmingCurve = new byte[_dimmingCurve.Length];
				_dimmingCurve.CopyTo(channel._dimmingCurve, 0);
			}
			return channel;
		}

		~Channel()
		{
			Dispose();
		}

		public XmlNode SaveToXml(XmlDocument doc)
		{
			XmlNode node = doc.CreateElement("Channel");
			Xml.SetAttribute(node, "name", _name);
			Xml.SetAttribute(node, "color", _color.ToArgb().ToString(CultureInfo.InvariantCulture));
			Xml.SetAttribute(node, "output", _outputChannel.ToString(CultureInfo.InvariantCulture));
			Xml.SetAttribute(node, "id", _id.ToString(CultureInfo.InvariantCulture));
			Xml.SetAttribute(node, "enabled", _enabled.ToString());
			if (_dimmingCurve != null)
			{
				var list = new List<string>();
				foreach (byte num in _dimmingCurve)
				{
					list.Add(num.ToString(CultureInfo.InvariantCulture));
				}
				Xml.SetValue(node, "Curve", string.Join(",", list.ToArray()));
			}
			return node;
		}

		public override string ToString()
		{
			return _name;
		}
	}
}