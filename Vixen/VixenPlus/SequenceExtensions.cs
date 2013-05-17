using System.Xml;

namespace VixenPlus
{
    public class SequenceExtensions : DataExtension
    {
        public SequenceExtensions() : base("Extensions")
        {
        }

        public XmlNode this[string extensionKey]
        {
            get
            {
                var node = RootNode.SelectSingleNode(string.Format("Extension[@type = \"{0}\"]", extensionKey)) ??
                               Xml.SetAttribute(RootNode, "Extension", "type", extensionKey);
                return node;
            }
        }
    }
}