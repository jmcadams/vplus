using System.Xml;

namespace Vixen
{
	public class LoadableData : DataExtension
	{
		public LoadableData() : base("LoadableData")
		{
		}

		public XmlNode GetLoadableData(string loadableType, string loadableName)
		{
			XmlNode nodeAlways = Xml.GetNodeAlways(base.m_rootNode, loadableType + "Data");
			XmlNode newChild =
				nodeAlways.SelectSingleNode(string.Format("{0}[attribute::name=\"{1}\"]", loadableType, loadableName));
			if (newChild == null)
			{
				newChild = base.m_doc.CreateElement(loadableType);
				XmlAttribute node = base.m_doc.CreateAttribute("name");
				node.Value = loadableName;
				newChild.Attributes.Append(node);
				nodeAlways.AppendChild(newChild);
			}
			return newChild;
		}
	}
}