using System;
using System.Collections.Generic;
using System.Xml;

namespace Preview {
    internal class Xml {
        public static XmlNode CloneNode(XmlDocument doc, XmlNode finalNode, bool deep) {
            var stack = new Stack<XmlNode>();
            while (finalNode is XmlElement) {
                stack.Push(finalNode);
                finalNode = finalNode.ParentNode;
            }
            var node = stack.Pop();
            var childNode = doc.SelectSingleNode("//" + node.Name) ?? doc.AppendChild(doc.ImportNode(node, false));
            while (stack.Count > 0) {
                node = stack.Pop();
                var attrNode = node.Attributes != null && node.Attributes["name"] != null
                                   ? childNode.SelectSingleNode(node.Name + string.Format("[@name = \"{0}\"]", node.Attributes["name"].Value))
                                   : childNode.SelectSingleNode(node.Name);
                childNode = attrNode ?? childNode.AppendChild(stack.Count == 0 ? doc.ImportNode(node, deep) : doc.ImportNode(node, false));
            }
            return childNode;
        }


        public static XmlDocument CreateXmlDocument() {
            var document = new XmlDocument();
            var newChild = document.CreateXmlDeclaration("1.0", "utf-8", string.Empty);
            document.AppendChild(newChild);
            return document;
        }


        public static XmlDocument CreateXmlDocument(string rootNodeName) {
            var document = new XmlDocument();
            var newChild = document.CreateXmlDeclaration("1.0", string.Empty, string.Empty);
            document.AppendChild(newChild);
            document.AppendChild(document.CreateElement(rootNodeName));
            return document;
        }


        public static XmlNode GetEmptyNodeAlways(XmlNode contextNode, string nodeName) {
            var nodeAlways = GetNodeAlways(contextNode, nodeName);
            nodeAlways.RemoveAll();
            return nodeAlways;
        }


        public static XmlNode GetNodeAlways(XmlNode contextNode, string nodeName) {
            var newChild = contextNode.SelectSingleNode(nodeName);
            if (newChild != null) {
                return newChild;
            }
            newChild = (contextNode.OwnerDocument ?? ((XmlDocument) contextNode)).CreateElement(nodeName);
            contextNode.AppendChild(newChild);
            return newChild;
        }


        public static string GetOptionalNodeValue(XmlNode contextNode, string nodeName) {
            var node = contextNode.SelectSingleNode(nodeName);
            return node == null ? string.Empty : node.InnerText;
        }


        public static XmlNode GetRequiredNode(XmlNode contextNode, string nodeName) {
            var node = contextNode.SelectSingleNode(nodeName);
            if (node == null) {
                throw new Exception(string.Format("Required element \"{0}\" not found in \"{1}\".", nodeName, contextNode.Name));
            }
            return node;
        }


        public static XmlDocument LoadDocument(string filename) {
            var document = new XmlDocument();
            document.Load(filename);
            return document;
        }


        public static XmlNode SetAttribute(XmlNode node, string attributeName, string attributeValue) {
            if (node.Attributes == null) {
                return node;
            }
            var attribute = node.Attributes[attributeName];
            if (attribute == null) {
                attribute = (node.OwnerDocument ?? ((XmlDocument) node)).CreateAttribute(attributeName);
                node.Attributes.Append(attribute);
            }
            attribute.Value = attributeValue;
            return node;
        }


        public static XmlNode SetAttribute(XmlNode contextNode, string nodeName, string attributeName, string attributeValue) {
            var singleNode = contextNode.SelectSingleNode(nodeName);
            var document = contextNode.OwnerDocument ?? ((XmlDocument) contextNode);
            if (singleNode == null) {
                singleNode = document.CreateElement(nodeName);
                contextNode.AppendChild(singleNode);
            }
            if (singleNode.Attributes == null) {
                return singleNode;
            }
            var node = singleNode.Attributes[attributeName];
            if (node == null) {
                node = document.CreateAttribute(attributeName);
                singleNode.Attributes.Append(node);
            }
            node.Value = attributeValue;
            return singleNode;
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
