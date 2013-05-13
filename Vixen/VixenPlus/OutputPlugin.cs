namespace VixenPlus
{
    public class OutputPlugin
    {
        private readonly int _from;
        private readonly int _id;
        private readonly bool _isEnabled;
        private readonly string _name;
        private readonly int _to;

        public OutputPlugin(string name, int id, bool enabled, int from, int to)
        {
            _name = name;
            _id = id;
            _isEnabled = enabled;
            _from = from;
            _to = to;
        }

        public int ChannelFrom
        {
            get { return _from; }
        }

        public int ChannelTo
        {
            get { return _to; }
        }

        public bool Enabled
        {
            get { return _isEnabled; }
        }

        public int Id
        {
            get { return _id; }
        }

        public string Name
        {
            get { return _name; }
        }
    }
}