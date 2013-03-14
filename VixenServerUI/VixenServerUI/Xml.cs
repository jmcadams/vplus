namespace VixenServerUI
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    internal class Xml
    {
        public static XmlNode CloneNode(XmlDocument doc, XmlNode finalNode, bool deep)
        {
            Stack<XmlNode> stack = new Stack<XmlNode>();
            while ((finalNode != null) && (finalNode is XmlElement))
            {
                stack.Push(finalNode);
                finalNode = finalNode.ParentNode;
            }
            XmlNode node = stack.Pop();
            XmlNode node2 = doc.SelectSingleNode("//" + node.Name);
            if (node2 == null)
            {
                node2 = doc.AppendChild(doc.ImportNode(node, false));
            }
            while (stack.Count > 0)
            {
                XmlNode node3;
                node = stack.Pop();
                if (node.Attributes["name"] != null)
                {
                    node3 = node2.SelectSingleNode(node.Name + string.Format("[@name = \"{0}\"]", node.Attributes["name"].Value));
                }
                else
                {
                    node3 = node2.SelectSingleNode(node.Name);
                }
                if (node3 == null)
                {
                    if (stack.Count == 0)
                    {
                        node2 = node2.AppendChild(doc.ImportNode(node, deep));
                    }
                    else
                    {
                        node2 = node2.AppendChild(doc.ImportNode(node, false));
                    }
                }
                else
                {
                    node2 = node3;
                }
            }
            return node2;
        }

        public static XmlDocument CreateXmlDocument()
        {
            XmlDocument document = new XmlDocument();
            XmlDeclaration newChild = document.CreateXmlDeclaration("1.0", "utf-8", string.Empty);
            document.AppendChild(newChild);
            return document;
        }

        public static XmlDocument CreateXmlDocument(string rootNodeName)
        {
            XmlDocument document = new XmlDocument();
            XmlDeclaration newChild = document.CreateXmlDeclaration("1.0", string.Empty, string.Empty);
            document.AppendChild(newChild);
            document.AppendChild(document.CreateElement(rootNodeName));
            return document;
        }

        public static XmlNode GetEmptyNodeAlways(XmlNode contextNode, string nodeName)
        {
            XmlNode nodeAlways = GetNodeAlways(contextNode, nodeName);
            nodeAlways.RemoveAll();
            return nodeAlways;
        }

        public static XmlNode GetNodeAlways(XmlNode contextNode, string nodeName)
        {
            XmlNode newChild = contextNode.SelectSingleNode(nodeName);
            if (newChild == null)
            {
                newChild = ((contextNode.OwnerDocument == null) ? ((XmlDocument) contextNode) : contextNode.OwnerDocument).CreateElement(nodeName);
                contextNode.AppendChild(newChild);
            }
            return newChild;
        }

        public static string GetOptionalNodeValue(XmlNode contextNode, string nodeName)
        {
            XmlNode node = contextNode.SelectSingleNode(nodeName);
            if (node == null)
            {
                return string.Empty;
            }
            return node.InnerText;
        }

        public static XmlNode GetRequiredNode(XmlNode contextNode, string nodeName)
        {
            XmlNode node = contextNode.SelectSingleNode(nodeName);
            if (node == null)
            {
                throw new Exception(string.Format("Required value \"{0}\" was not found in context node \"{1}\".", nodeName, contextNode.Name));
            }
            return node;
        }

        public static XmlDocument LoadDocument(string filename)
        {
            XmlDocument document = new XmlDocument();
            document.Load(filename);
            return document;
        }

        public static XmlNode SetAttribute(XmlNode node, string attributeName, string attributeValue)
        {
            XmlAttribute attribute = node.Attributes[attributeName];
            if (attribute == null)
            {
                attribute = ((node.OwnerDocument == null) ? ((XmlDocument) node) : node.OwnerDocument).CreateAttribute(attributeName);
                node.Attributes.Append(attribute);
            }
            attribute.Value = attributeValue;
            return node;
        }

        public static XmlNode SetAttribute(XmlNode contextNode, string nodeName, string attributeName, string attributeValue)
        {
            XmlNode newChild = contextNode.SelectSingleNode(nodeName);
            XmlDocument document = (contextNode.OwnerDocument == null) ? ((XmlDocument) contextNode) : contextNode.OwnerDocument;
            if (newChild == null)
            {
                newChild = document.CreateElement(nodeName);
                contextNode.AppendChild(newChild);
            }
            XmlAttribute node = newChild.Attributes[attributeName];
            if (node == null)
            {
                node = document.CreateAttribute(attributeName);
                newChild.Attributes.Append(node);
            }
            node.Value = attributeValue;
            return newChild;
        }

        public static XmlNode SetNewValue(XmlNode contextNode, string nodeName, string nodeValue)
        {
            XmlNode newChild = ((contextNode.OwnerDocument == null) ? ((XmlDocument) contextNode) : contextNode.OwnerDocument).CreateElement(nodeName);
            contextNode.AppendChild(newChild);
            newChild.InnerText = nodeValue;
            return newChild;
        }

        public static XmlNode SetValue(XmlNode contextNode, string nodeName, string nodeValue)
        {
            XmlNode newChild = contextNode.SelectSingleNode(nodeName);
            if (newChild == null)
            {
                newChild = ((contextNode.OwnerDocument == null) ? ((XmlDocument) contextNode) : contextNode.OwnerDocument).CreateElement(nodeName);
                contextNode.AppendChild(newChild);
            }
            newChild.InnerText = nodeValue;
            return newChild;
        }
    }
}

