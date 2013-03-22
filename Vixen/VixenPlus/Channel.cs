namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Xml;

    public class Channel : IDisposable, IComparable<Channel>
    {
        private SolidBrush m_brush;
        private System.Drawing.Color m_color;
        private byte[] m_dimmingCurve;
        private bool m_enabled;
        private ulong m_id;
        private string m_name;
        private int m_outputChannel;

        public Channel(XmlNode channelNode)
        {
            this.m_brush = null;
            this.m_outputChannel = 0;
            this.m_enabled = true;
            this.m_dimmingCurve = null;
            this.m_name = channelNode.Attributes["name"].Value;
            this.Color = System.Drawing.Color.FromArgb(Convert.ToInt32(channelNode.Attributes["color"].Value));
            this.m_outputChannel = Convert.ToInt32(channelNode.Attributes["output"].Value);
            this.m_id = ulong.Parse(channelNode.Attributes["id"].Value);
            this.m_enabled = bool.Parse(channelNode.Attributes["enabled"].Value);
            if (channelNode["Curve"] != null)
            {
                this.m_dimmingCurve = new byte[0x100];
                string[] strArray = channelNode["Curve"].InnerText.Split(new char[] { ',' });
                int num = Math.Min(strArray.Length, 0x100);
                for (int i = 0; i < num; i++)
                {
                    byte num2;
                    if (byte.TryParse(strArray[i], out num2))
                    {
                        this.m_dimmingCurve[i] = num2;
                    }
                    else
                    {
                        this.m_dimmingCurve[i] = (byte) i;
                    }
                }
            }
        }

        public Channel(string name, int outputChannel)
        {
            this.m_brush = null;
            this.m_outputChannel = 0;
            this.m_enabled = true;
            this.m_dimmingCurve = null;
            this.m_name = name;
            this.Color = System.Drawing.Color.FromArgb(-1);
            this.m_id = Host.GetUniqueKey();
            this.m_outputChannel = outputChannel;
        }

        public Channel(string name, System.Drawing.Color color, int outputChannel)
        {
            this.m_brush = null;
            this.m_outputChannel = 0;
            this.m_enabled = true;
            this.m_dimmingCurve = null;
            this.m_name = name;
            this.Color = color;
            this.m_id = Host.GetUniqueKey();
            this.m_outputChannel = outputChannel;
        }

        public Channel(string name, int outputChannel, bool ensureUniqueID)
        {
            this.m_brush = null;
            this.m_outputChannel = 0;
            this.m_enabled = true;
            this.m_dimmingCurve = null;
            this.m_name = name;
            this.Color = System.Drawing.Color.FromArgb(-1);
            if (ensureUniqueID)
            {
                long ticks = DateTime.Now.Ticks;
                while (ticks == DateTime.Now.Ticks)
                {
                }
            }
            this.m_id = Host.GetUniqueKey();
            this.m_outputChannel = outputChannel;
        }

        public Channel(string name, System.Drawing.Color color, int outputChannel, bool ensureUniqueID)
        {
            this.m_brush = null;
            this.m_outputChannel = 0;
            this.m_enabled = true;
            this.m_dimmingCurve = null;
            this.m_name = name;
            this.Color = color;
            if (ensureUniqueID)
            {
                long ticks = DateTime.Now.Ticks;
                while (ticks == DateTime.Now.Ticks)
                {
                }
            }
            this.m_id = Host.GetUniqueKey();
            this.m_outputChannel = outputChannel;
        }

        public Channel Clone()
        {
            Channel channel = (Channel) base.MemberwiseClone();
            channel.m_brush = new SolidBrush(this.m_brush.Color);
            if (this.m_dimmingCurve != null)
            {
                channel.m_dimmingCurve = new byte[this.m_dimmingCurve.Length];
                this.m_dimmingCurve.CopyTo(channel.m_dimmingCurve, 0);
            }
            return channel;
        }

        public int CompareTo(Channel other)
        {
            return this.ID.CompareTo(other.ID);
        }

        public void Dispose()
        {
            if (this.m_brush != null)
            {
                this.m_brush.Dispose();
                this.m_brush = null;
            }
            GC.SuppressFinalize(this);
        }

        ~Channel()
        {
            this.Dispose();
        }

        public XmlNode SaveToXml(XmlDocument doc)
        {
            XmlNode node = doc.CreateElement("Channel");
            Xml.SetAttribute(node, "name", this.m_name);
            Xml.SetAttribute(node, "color", this.m_color.ToArgb().ToString());
            Xml.SetAttribute(node, "output", this.m_outputChannel.ToString());
            Xml.SetAttribute(node, "id", this.m_id.ToString());
            Xml.SetAttribute(node, "enabled", this.m_enabled.ToString());
            if (this.m_dimmingCurve != null)
            {
                List<string> list = new List<string>();
                foreach (byte num in this.m_dimmingCurve)
                {
                    list.Add(num.ToString());
                }
                XmlNode node2 = Xml.SetValue(node, "Curve", string.Join(",", list.ToArray()));
            }
            return node;
        }

        public override string ToString()
        {
            return this.m_name;
        }

        public SolidBrush Brush
        {
            get
            {
                return this.m_brush;
            }
        }

        public System.Drawing.Color Color
        {
            get
            {
                return this.m_color;
            }
            set
            {
                if (this.m_color.ToArgb() != value.ToArgb())
                {
                    this.m_color = value;
                    if (this.m_brush != null)
                    {
                        this.m_brush.Dispose();
                    }
                    this.m_brush = new SolidBrush(value);
                }
            }
        }

        public byte[] DimmingCurve
        {
            get
            {
                return this.m_dimmingCurve;
            }
            set
            {
                this.m_dimmingCurve = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return this.m_enabled;
            }
            set
            {
                this.m_enabled = value;
            }
        }

        public ulong ID
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

        public int OutputChannel
        {
            get
            {
                return this.m_outputChannel;
            }
            set
            {
                this.m_outputChannel = value;
            }
        }
    }
}

