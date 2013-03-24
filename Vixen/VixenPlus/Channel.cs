using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

namespace Vixen
{
	public class Channel : IDisposable, IComparable<Channel>
	{
		private readonly ulong m_id;
		private SolidBrush m_brush;
		private Color m_color;
		private byte[] m_dimmingCurve;
		private bool m_enabled;
		private string m_name;
		private int m_outputChannel;

		public Channel(XmlNode channelNode)
		{
			m_brush = null;
			m_outputChannel = 0;
			m_enabled = true;
			m_dimmingCurve = null;
			m_name = channelNode.Attributes["name"].Value;
			Color = Color.FromArgb(Convert.ToInt32(channelNode.Attributes["color"].Value));
			m_outputChannel = Convert.ToInt32(channelNode.Attributes["output"].Value);
			m_id = ulong.Parse(channelNode.Attributes["id"].Value);
			m_enabled = bool.Parse(channelNode.Attributes["enabled"].Value);
			if (channelNode["Curve"] != null)
			{
				m_dimmingCurve = new byte[0x100];
				string[] strArray = channelNode["Curve"].InnerText.Split(new[] {','});
				int num = Math.Min(strArray.Length, 0x100);
				for (int i = 0; i < num; i++)
				{
					byte num2;
					if (byte.TryParse(strArray[i], out num2))
					{
						m_dimmingCurve[i] = num2;
					}
					else
					{
						m_dimmingCurve[i] = (byte) i;
					}
				}
			}
		}

		public Channel(string name, int outputChannel)
		{
			m_brush = null;
			m_outputChannel = 0;
			m_enabled = true;
			m_dimmingCurve = null;
			m_name = name;
			Color = Color.FromArgb(-1);
			m_id = Host.GetUniqueKey();
			m_outputChannel = outputChannel;
		}

		public Channel(string name, Color color, int outputChannel)
		{
			m_brush = null;
			m_outputChannel = 0;
			m_enabled = true;
			m_dimmingCurve = null;
			m_name = name;
			Color = color;
			m_id = Host.GetUniqueKey();
			m_outputChannel = outputChannel;
		}

		public Channel(string name, int outputChannel, bool ensureUniqueID)
		{
			m_brush = null;
			m_outputChannel = 0;
			m_enabled = true;
			m_dimmingCurve = null;
			m_name = name;
			Color = Color.FromArgb(-1);
			if (ensureUniqueID)
			{
				long ticks = DateTime.Now.Ticks;
				while (ticks == DateTime.Now.Ticks)
				{
				}
			}
			m_id = Host.GetUniqueKey();
			m_outputChannel = outputChannel;
		}

		public Channel(string name, Color color, int outputChannel, bool ensureUniqueID)
		{
			m_brush = null;
			m_outputChannel = 0;
			m_enabled = true;
			m_dimmingCurve = null;
			m_name = name;
			Color = color;
			if (ensureUniqueID)
			{
				long ticks = DateTime.Now.Ticks;
				while (ticks == DateTime.Now.Ticks)
				{
				}
			}
			m_id = Host.GetUniqueKey();
			m_outputChannel = outputChannel;
		}

		public SolidBrush Brush
		{
			get { return m_brush; }
		}

		public Color Color
		{
			get { return m_color; }
			set
			{
				if (m_color.ToArgb() != value.ToArgb())
				{
					m_color = value;
					if (m_brush != null)
					{
						m_brush.Dispose();
					}
					m_brush = new SolidBrush(value);
				}
			}
		}

		public byte[] DimmingCurve
		{
			get { return m_dimmingCurve; }
			set { m_dimmingCurve = value; }
		}

		public bool Enabled
		{
			get { return m_enabled; }
			set { m_enabled = value; }
		}

		public ulong ID
		{
			get { return m_id; }
		}

		public string Name
		{
			get { return m_name; }
			set { m_name = value; }
		}

		public int OutputChannel
		{
			get { return m_outputChannel; }
			set { m_outputChannel = value; }
		}

		public int CompareTo(Channel other)
		{
			return ID.CompareTo(other.ID);
		}

		public void Dispose()
		{
			if (m_brush != null)
			{
				m_brush.Dispose();
				m_brush = null;
			}
			GC.SuppressFinalize(this);
		}

		public Channel Clone()
		{
			var channel = (Channel) base.MemberwiseClone();
			channel.m_brush = new SolidBrush(m_brush.Color);
			if (m_dimmingCurve != null)
			{
				channel.m_dimmingCurve = new byte[m_dimmingCurve.Length];
				m_dimmingCurve.CopyTo(channel.m_dimmingCurve, 0);
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
			Xml.SetAttribute(node, "name", m_name);
			Xml.SetAttribute(node, "color", m_color.ToArgb().ToString());
			Xml.SetAttribute(node, "output", m_outputChannel.ToString());
			Xml.SetAttribute(node, "id", m_id.ToString());
			Xml.SetAttribute(node, "enabled", m_enabled.ToString());
			if (m_dimmingCurve != null)
			{
				var list = new List<string>();
				foreach (byte num in m_dimmingCurve)
				{
					list.Add(num.ToString());
				}
				XmlNode node2 = Xml.SetValue(node, "Curve", string.Join(",", list.ToArray()));
			}
			return node;
		}

		public override string ToString()
		{
			return m_name;
		}
	}
}