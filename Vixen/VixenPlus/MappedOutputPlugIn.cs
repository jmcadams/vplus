namespace Vixen
{
    using System;
    using System.Xml;

    internal class MappedOutputPlugIn
    {
        public byte[] Buffer;
        public bool ContextInitialized = false;
        public bool Enabled;
        public int From;
        public IOutputPlugIn PlugIn;
        public XmlNode SetupDataNode;
        public int To;
        public object UserData;

        public MappedOutputPlugIn(IOutputPlugIn plugin, int from, int to, bool enabled, XmlNode setupDataNode)
        {
            this.PlugIn = plugin;
            this.From = from;
            this.To = to;
            this.Enabled = enabled;
            this.Buffer = new byte[(to - from) + 1];
            this.SetupDataNode = setupDataNode;
        }
    }
}

