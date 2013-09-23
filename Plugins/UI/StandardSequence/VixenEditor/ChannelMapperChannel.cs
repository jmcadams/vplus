using System.Drawing;

namespace VixenEditor {
    class ChannelMapperChannel {

        public ChannelMapperChannel(int channel, int location, Color color, string name, bool enabled)
        {
            Number = channel;
            Location = location;
            Color = color.ToArgb();
            Name = name;
            Enabled = enabled;
        }

        public int Location { get; set; }

        public int Color { get; set; }

        public string Name { get; set; }

        public int Number { get; private set; }

        public bool Enabled { get; set; }
    }
}
