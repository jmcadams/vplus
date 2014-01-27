using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

public static class Xml {
    public static void CloneNode(XmlDocument targetDoc, XmlNode finalNode, bool deep) {
        var stack = new Stack<XmlNode>();
        while (finalNode is XmlElement) {
            stack.Push(finalNode);
            finalNode = finalNode.ParentNode;
        }
        var node = stack.Pop();
        var node2 = targetDoc.SelectSingleNode("//" + node.Name) ?? targetDoc.AppendChild(targetDoc.ImportNode(node, false));
        while (stack.Count > 0) {
            node = stack.Pop();
            var node3 = node.Attributes != null && node.Attributes["name"] != null
                ? node2.SelectSingleNode(node.Name + string.Format("[@name = \"{0}\"]", node.Attributes["name"].Value))
                : node2.SelectSingleNode(node.Name);
            node2 = node3 ?? node2.AppendChild(stack.Count == 0 ? targetDoc.ImportNode(node, deep) : targetDoc.ImportNode(node, false));
        }
    }


    public static XmlDocument CreateXmlDocument() {
        var document = new XmlDocument();
        var newChild = document.CreateXmlDeclaration("1.0", "utf-8", string.Empty);
        document.AppendChild(newChild);
        return document;
    }


    public static XmlDocument CreateXmlDocument(string rootNodeName) {
        var document = new XmlDocument();
        var newChild = document.CreateXmlDeclaration("1.0", Encoding.UTF8.WebName, string.Empty);
        document.AppendChild(newChild);
        document.AppendChild(document.CreateElement(rootNodeName));
        return document;
    }


/*
    public static string GetAttribute(XmlNode node, string attributeName) {
        return GetAttribute(node, attributeName, "");
    }
*/


/*
    public static string GetAttribute(XmlNode node, string attributeName, string attributeDefaultValue) {
        var returnValue = attributeDefaultValue;
            
        if (node.Attributes == null) {
            SetAttribute(node, attributeName, attributeDefaultValue);
        }
        else if (node.Attributes[attributeName] == null) {
            SetAttribute(node, attributeName, attributeDefaultValue);
        }
        else {
            returnValue = node.Attributes[attributeName].Value;
        }
            
        return returnValue;
    }
*/


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


    public static XmlNode GetNodeAlways(XmlNode contextNode, string nodeName, string defaultValue) {
        var newChild = contextNode.SelectSingleNode(nodeName);
        if (newChild != null) {
            return newChild;
        }
        newChild = (contextNode.OwnerDocument ?? ((XmlDocument) contextNode)).CreateElement(nodeName);
        contextNode.AppendChild(newChild);
        newChild.InnerText = defaultValue;
        return newChild;
    }


    public static string GetOptionalNodeValue(XmlNode contextNode, string nodeName) {
        var node = contextNode.SelectSingleNode(nodeName);
        return node == null ? string.Empty : node.InnerText;
    }


    public static XmlNode GetRequiredNode(XmlNode contextNode, string nodeName) {
        var node = contextNode.SelectSingleNode(nodeName);
        if (node == null) {
            throw new Exception(string.Format("Required value \"{0}\" was not found in context node \"{1}\".", nodeName, contextNode.Name));
        }
        return node;
    }


    public static XmlDocument LoadDocument(string filename) {
        var document = new XmlDocument();
        document.Load(filename);
        return document;
    }


    public static void SetAttribute(XmlNode node, string attributeName, string attributeValue) {
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


    public static XmlNode SetAttribute(XmlNode contextNode, string nodeName, string attributeName, string attributeValue) {
        var newChild = contextNode.SelectSingleNode(nodeName);
        var document = contextNode.OwnerDocument ?? ((XmlDocument) contextNode);
        if (newChild == null) {
            newChild = document.CreateElement(nodeName);
            contextNode.AppendChild(newChild);
        }
        if (newChild.Attributes == null) {
            return newChild;
        }
        var node = newChild.Attributes[attributeName];
        if (node == null) {
            node = document.CreateAttribute(attributeName);
            newChild.Attributes.Append(node);
        }
        node.Value = attributeValue;
        return newChild;
    }


    public static XmlNode SetNewValue(XmlNode contextNode, string nodeName, string nodeValue) {
        XmlNode newChild = (contextNode.OwnerDocument ?? ((XmlDocument) contextNode)).CreateElement(nodeName);
        contextNode.AppendChild(newChild);
        newChild.InnerText = nodeValue;
        return newChild;
    }


    public static XmlNode SetValue(XmlNode contextNode, string nodeName, string nodeValue) {
        var newChild = contextNode.SelectSingleNode(nodeName);
        if (newChild == null) {
            newChild = (contextNode.OwnerDocument ?? ((XmlDocument) contextNode)).CreateElement(nodeName);
            contextNode.AppendChild(newChild);
        }
        newChild.InnerText = nodeValue;
        return newChild;
    }
}