using System;
using System.Xml;

namespace Vixen
{
	internal class PlugInMapping
	{
		public int From;
		public XmlNode Node;
		public int To;

		public PlugInMapping(XmlNode node)
		{
			try
			{
				From = Convert.ToInt32(node.Attributes["from"].Value);
			}
			catch
			{
				From = 0;
			}
			try
			{
				To = Convert.ToInt32(node.Attributes["to"].Value);
			}
			catch
			{
				To = 0;
			}
			Node = node;
		}

		public override string ToString()
		{
			return Node.Attributes["name"].Value;
		}
	}
}