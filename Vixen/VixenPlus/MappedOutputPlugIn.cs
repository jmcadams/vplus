using System.Xml;

namespace Vixen
{
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
			PlugIn = plugin;
			From = from;
			To = to;
			Enabled = enabled;
			Buffer = new byte[(to - from) + 1];
			SetupDataNode = setupDataNode;
		}
	}
}