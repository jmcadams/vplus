using System.Xml;

namespace Preview {
    internal static class Xml {
        public static XmlNode GetEmptyNodeAlways(XmlNode contextNode, string nodeName) {
            var nodeAlways = GetNodeAlways(contextNode, nodeName);
            nodeAlways.RemoveAll();
            return nodeAlways;
        }


        private static XmlNode GetNodeAlways(XmlNode contextNode, string nodeName) {
            var newChild = contextNode.SelectSingleNode(nodeName);
            if (newChild != null) {
                return newChild;
            }
            newChild = (contextNode.OwnerDocument ?? ((XmlDocument) contextNode)).CreateElement(nodeName);
            contextNode.AppendChild(newChild);
            return newChild;
        }


        public static void SetAttribute(XmlNode node, string attributeName, string attributeValue) {
            // Duplicated in Vixen.Xml.cs
            if (node.Attributes == null) {
                return;
            }
            var attribute = node.Attributes[attributeName];
            if (attribute == null) {
                attribute = (node.OwnerDocument ?? ((XmlDocument) node)).CreateAttribute(attributeName);
                node.Attributes.Append(attribute);
            }
            attribute.Value = attributeValue;
        }


        public static XmlNode SetNewValue(XmlNode contextNode, string nodeName, string nodeValue) {
            XmlNode node = (contextNode.OwnerDocument ?? ((XmlDocument) contextNode)).CreateElement(nodeName);
            contextNode.AppendChild(node);
            node.InnerText = nodeValue;
            return node;
        }


        public static XmlNode SetValue(XmlNode contextNode, string nodeName, string nodeValue) {
            var node = contextNode.SelectSingleNode(nodeName);
            if (node == null) {
                node = (contextNode.OwnerDocument ?? ((XmlDocument) contextNode)).CreateElement(nodeName);
                contextNode.AppendChild(node);
            }
            node.InnerText = nodeValue;
            return node;
        }
    }
}
