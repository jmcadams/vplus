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
            var nodeAlways = Xml.GetNodeAlways(RootNode, loadableType + "Data");
            var newChild =
                nodeAlways.SelectSingleNode(string.Format("{0}[attribute::name=\"{1}\"]", loadableType, loadableName));
            if (newChild != null) {
                return newChild;
            }
            newChild = Document.CreateElement(loadableType);
            var node = Document.CreateAttribute("name");
            node.Value = loadableName;
            if (newChild.Attributes != null)
            {
                newChild.Attributes.Append(node);
            }
            nodeAlways.AppendChild(newChild);
            return newChild;
        }
    }
}