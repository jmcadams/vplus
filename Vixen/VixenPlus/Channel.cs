using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;

using CommonUtils;

namespace VixenPlus {
    public class Channel : IDisposable, IComparable<Channel> {
        private Color _color;


        public Channel(XmlNode channelNode) {
            Brush = null;
            OutputChannel = 0;
            Enabled = true;
            DimmingCurve = null;
            if (channelNode.Attributes == null) {
                return;
            }

            CanDoDimming = (channelNode.Attributes["name"] != null);
            // ReSharper disable PossibleNullReferenceException
            Name = CanDoDimming ? channelNode.Attributes["name"].Value : channelNode.InnerText;
            // ReSharper restore PossibleNullReferenceException

            Color = Color.FromArgb(Convert.ToInt32(channelNode.Attributes["color"].Value));
            OutputChannel = Convert.ToInt32(channelNode.Attributes["output"].Value);
            Id = ulong.Parse(channelNode.Attributes["id"].Value);
            Enabled = bool.Parse(channelNode.Attributes["enabled"].Value);
            if (channelNode["Curve"] == null) {
                return;
            }

            DimmingCurve = new byte[256];
            var strArray = channelNode["Curve"].InnerText.Split(new[] {','});
            var num = Math.Min(strArray.Length, 256);
            for (var i = 0; i < num; i++) {
                byte num2;
                if (byte.TryParse(strArray[i], out num2)) {
                    DimmingCurve[i] = num2;
                }
                else {
                    DimmingCurve[i] = (byte) i;
                }
            }
        }


        public Channel(string name, int outputChannel) {
            Name = name;
            OutputChannel = outputChannel;

            Brush = null;
            Enabled = true;
            DimmingCurve = null;
            Color = Color.FromArgb(-1);
            Id = Host.GetUniqueKey();
        }


        public Channel(string name, Color color, int outputChannel) : this(name, outputChannel) {
            Color = color;
        }


        public Channel(string name, int outputChannel, bool ensureUniqueId) : this(name, outputChannel) {
            if (ensureUniqueId) {
                var ticks = DateTime.Now.Ticks;
                while (ticks == DateTime.Now.Ticks) {}
            }
            Id = Host.GetUniqueKey();
        }


        public Channel(string name, Color color, int outputChannel, bool ensureUniqueId) : this(name, outputChannel) {
            Color = color;
            if (ensureUniqueId) {
                var ticks = DateTime.Now.Ticks;
                while (ticks == DateTime.Now.Ticks) {}
            }
            Id = Host.GetUniqueKey();
        }


        public SolidBrush Brush { get; private set; }

        public Color Color {
            get { return _color; }
            set {
                if (_color.ToArgb() == value.ToArgb()) {
                    return;
                }

                _color = value;
                if (Brush != null) {
                    Brush.Dispose();
                }
                Brush = new SolidBrush(value);
            }
        }

        public byte[] DimmingCurve { get; set; }

        public bool Enabled { get; set; }

        public ulong Id { get; private set; }

        public bool CanDoDimming { get; private set; }

        public string Name { get; set; }

        public int OutputChannel { get; set; }

        //public int EventIndex { get; set; }

        public int CompareTo(Channel other) {
            return Id.CompareTo(other.Id);
        }


        public void Dispose() {
            if (Brush != null) {
                Brush.Dispose();
                Brush = null;
            }
            GC.SuppressFinalize(this);
        }


        public Channel Clone() {
            var channel = (Channel) MemberwiseClone();
            channel.Brush = new SolidBrush(Brush.Color);
            if (DimmingCurve != null) {
                channel.DimmingCurve = new byte[DimmingCurve.Length];
                DimmingCurve.CopyTo(channel.DimmingCurve, 0);
            }
            return channel;
        }


        ~Channel() {
            Dispose();
        }


        public XmlNode SaveToXml(XmlDocument doc) {
            XmlNode node = doc.CreateElement("Channel");
            if (CanDoDimming) {
                Xml.SetAttribute(node, "name", Name);
            }
            else {
                node.InnerText = Name;
            }
            Xml.SetAttribute(node, "color", Color.ToArgb().ToString(CultureInfo.InvariantCulture));
            Xml.SetAttribute(node, "output", OutputChannel.ToString(CultureInfo.InvariantCulture));
            Xml.SetAttribute(node, "id", Id.ToString(CultureInfo.InvariantCulture));
            Xml.SetAttribute(node, "enabled", Enabled.ToString());
            if (DimmingCurve != null) {
                var list = new List<string>();
                foreach (var num in DimmingCurve) {
                    list.Add(num.ToString(CultureInfo.InvariantCulture));
                }
                Xml.SetValue(node, "Curve", string.Join(",", list.ToArray()));
            }
            return node;
        }


        public override string ToString() {
            return Name;
        }

        // For ComboBoxes
        public static void DrawItem(DrawItemEventArgs e, Channel channels) {
            e.DrawBackground();

            using (var backgroundBrush = new SolidBrush(channels.Color))
            using (var g = e.Graphics) {
                g.FillRectangle(backgroundBrush, e.Bounds);
                var contrastingBrush = Utils.GetTextColor(backgroundBrush.Color);
                g.DrawString(channels.Name, e.Font, contrastingBrush, new RectangleF(e.Bounds.Location, e.Bounds.Size));
            }

            e.DrawFocusRectangle();
        }


        // For List Boxes
        public static void DrawItem(ListBox lb, DrawItemEventArgs e, Channel channels, bool drawFocus = true) {
            e.DrawBackground();

            using (var backgroundBrush = new SolidBrush(channels.Color))
            using (var g = e.Graphics) {
                g.FillRectangle(backgroundBrush, e.Bounds);

                var contrastingBrush = Utils.GetTextColor(backgroundBrush.Color);
                g.DrawString(channels.Name, e.Font, contrastingBrush, lb.GetItemRectangle(e.Index).Location);
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) {
                    g.DrawString("\u2714", e.Font, contrastingBrush, e.Bounds.Width - e.Bounds.Height, e.Bounds.Y);
                }
            }

            if (drawFocus) {
                e.DrawFocusRectangle();
            }
        }


        // For TreeViews
        public static void DrawItem(TreeView treeView, DrawTreeNodeEventArgs e, Color channelColor) {
            e.Graphics.FillRectangle(new SolidBrush(channelColor), e.Node.Bounds);
            e.Graphics.DrawString(e.Node.Text, e.Node.NodeFont ?? treeView.Font, Utils.GetTextColor(channelColor), Rectangle.Inflate(e.Bounds, 2, 0));
        }
    }
}
