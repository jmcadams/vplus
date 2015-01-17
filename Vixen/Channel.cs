using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

using VixenPlusCommon;

namespace VixenPlus {
    public class Channel : IDisposable {
        private Color _color;
        private static readonly Boolean UseCheckmark = Preference2.GetInstance().GetBoolean("UseCheckmark");

        public Channel(XmlNode channelNode) {
            OutputChannel = 0;
            Enabled = true;
            DimmingCurve = null;
            if (channelNode.Attributes == null) {
                return;
            }

            if (channelNode.Attributes["name"] == null) {
                SupportsDimmingCurve = false;
                Name = channelNode.InnerText;
            }
            else {
                SupportsDimmingCurve = true;
                Name = channelNode.Attributes["name"].Value;
            }

            var elementName = "color";
            try {
                Color = Color.FromArgb(Convert.ToInt32(channelNode.Attributes[elementName].Value));
                elementName = "output";
                OutputChannel = Convert.ToInt32(channelNode.Attributes[elementName].Value);
                elementName = "id";
                Id =  channelNode.Attributes[elementName] == null ? NextRandom() : ulong.Parse(channelNode.Attributes[elementName].Value);
                elementName = "enabled";
                Enabled = bool.Parse(channelNode.Attributes[elementName].Value);
            }
            catch (NullReferenceException) {
                MessageBox.Show(String.Format("Embedded or attached profile is missing '{0}' elenment on channel node.\n\nExiting {1}", elementName, Vendor.ProductName), "Missing element");
                throw new NullReferenceException(String.Format("Channel XML is missing '{0}' element", elementName));
            }
            
            
            if (channelNode["Curve"] == null) {
                return;
            }

            DimmingCurve = new byte[256];
            var strArray = channelNode["Curve"].InnerText.Split(',');
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


        private Random _rand;
        private ulong NextRandom() {
            if (_rand == null) {
                _rand = new Random();
            }

            return (ulong)(_rand.NextDouble() * ulong.MaxValue);
        }

        public Channel(string name, int outputChannel) {
            Name = name;
            OutputChannel = outputChannel;

            Enabled = true;
            DimmingCurve = null;
            Color = Color.FromArgb(-1);
        }


        public Channel(string name, Color color, int outputChannel) : this(name, outputChannel) {
            Color = color;
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

        public bool SupportsDimmingCurve { get; private set; }

        public string Name { get; set; }

        public int OutputChannel { get; set; }

        public ulong Id { get; private set; }

        public void Dispose() {
            GC.SuppressFinalize(this);
        }


        public Channel Clone() {
            var channel = (Channel) MemberwiseClone();
            if (DimmingCurve == null) {
                return channel;
            }
            channel.DimmingCurve = new byte[DimmingCurve.Length];
            DimmingCurve.CopyTo(channel.DimmingCurve, 0);
            return channel;
        }


        ~Channel() {
            Dispose();
        }


        public override string ToString() {
            return Name;
        }


        public static void DrawItem(DrawItemEventArgs e, Channel c, ListBox lb) {
            e.DrawItem(c.Name, c.Color, lb, UseCheckmark);
        }


        public static void DrawItem(DrawItemEventArgs e, Channel c, bool useCheckmark = false) {
            e.DrawItem(c.Name, c.Color, useCheckmark);
        }
    }
}