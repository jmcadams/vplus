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
                XmlNode node = Node.SelectSingleNode(string.Format("Extension[@type = \"{0}\"]", extensionKey)) ??
                               Xml.SetAttribute(Node, "Extension", "type", extensionKey);
                return node;
            }
        }
    }
}