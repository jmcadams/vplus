using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;

using CommonControls;

using CommonUtils;

namespace VixenPlus {
    public class Channel : IDisposable, IComparable<Channel> {
        private Color _color;
        private static readonly SolidBrush GenericBrush = new SolidBrush(Color.Black);
        private const string Checkmark = "\u2714";

        public Channel(XmlNode channelNode) {
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


        public Color Color {
            get { return _color; }
            set {
                if (_color.ToArgb() == value.ToArgb()) {
                    return;
                }

                _color = value;
            }
        }

        public byte[] DimmingCurve { get; set; }

        public bool Enabled { get; set; }

        public ulong Id { get; private set; }

        public bool CanDoDimming { get; private set; }

        public string Name { get; set; }

        public int OutputChannel { get; set; }

        public int CompareTo(Channel other) {
            return Id.CompareTo(other.Id);
        }


        public void Dispose() {
            GC.SuppressFinalize(this);
        }


        public Channel Clone() {
            var channel = (Channel) MemberwiseClone();
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
        public static void DrawItem(DrawItemEventArgs e, string name, Color color, bool useCheckmark = false) {
            e.DrawBackground();

            var selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            GenericBrush.Color = color;
            e.Graphics.FillRectangle(selected && !useCheckmark ? SystemBrushes.Highlight : GenericBrush, e.Bounds);
            var contrastingBrush = selected && !useCheckmark ? SystemBrushes.HighlightText : Utils.GetTextColor(color);
            e.Graphics.DrawString(name, e.Font, contrastingBrush, new RectangleF(e.Bounds.Location, e.Bounds.Size));
            if (selected && useCheckmark) {
                e.Graphics.DrawString(Checkmark, e.Font, contrastingBrush, e.Bounds.Width - e.Bounds.Height, e.Bounds.Y);
            }
            e.DrawFocusRectangle();
        }


        public static void DrawItem(DrawItemEventArgs e, Channel c, bool useCheckmark = false) {
            DrawItem(e, c.Name, c.Color, useCheckmark);
        }


        // For List Boxes
        public static void DrawItem(ListBox lb, DrawItemEventArgs e, Channel channel) {
            e.DrawBackground();

            GenericBrush.Color = channel.Color;
            e.Graphics.FillRectangle(GenericBrush, e.Bounds);
            var contrastingBrush = Utils.GetTextColor(channel.Color);
            e.Graphics.DrawString(channel.Name, e.Font, contrastingBrush, lb.GetItemRectangle(e.Index).Location);
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) {
                e.Graphics.DrawString(Checkmark, e.Font, contrastingBrush, e.Bounds.Width - e.Bounds.Height, e.Bounds.Y);
            }

            e.DrawFocusRectangle();
        }


        // For TreeViews
        public static void DrawItem(TreeView treeView, DrawTreeNodeEventArgs e, Color channelColor) {
            if (treeView == null) {
                e.DrawDefault = true;
                return;
            }

            var fillRect = new Rectangle(e.Node.Bounds.X, e.Node.Bounds.Y, treeView.Width - e.Node.Bounds.Left, e.Node.Bounds.Height);
            GenericBrush.Color = channelColor;
            e.Graphics.FillRectangle(GenericBrush, fillRect);
            e.Graphics.DrawString(e.Node.Text, treeView.Font,  Utils.GetTextColor(channelColor), e.Bounds.Left, e.Bounds.Top);

            bool selected;
            var view = treeView as MultiSelectTreeview;
            if (view != null) {
                selected = view.SelectedNodes.Contains(e.Node);
            }
            else {
                selected = (e.State & TreeNodeStates.Selected) != 0;
            } 
            if (selected) {
                e.Graphics.DrawString(Checkmark, treeView.Font, Utils.GetTextColor(channelColor), fillRect.Right - 40, e.Bounds.Top);
            }
        }
    }
}
