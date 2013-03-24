using System.Xml;

namespace VixenPlus
{
	public class LoadableData : DataExtension
	{
		public LoadableData() : base("LoadableData")
		{
		}

		public XmlNode GetLoadableData(string loadableType, string loadableName)
		{
			XmlNode nodeAlways = Xml.GetNodeAlways(Node, loadableType + "Data");
			XmlNode newChild =
				nodeAlways.SelectSingleNode(string.Format("{0}[attribute::name=\"{1}\"]", loadableType, loadableName));
			if (newChild == null)
			{
				newChild = Document.CreateElement(loadableType);
				XmlAttribute node = Document.CreateAttribute("name");
				node.Value = loadableName;
				if (newChild.Attributes != null)
				{
					newChild.Attributes.Append(node);
				}
				nodeAlways.AppendChild(newChild);
			}
			return newChild;
		}
	}
}