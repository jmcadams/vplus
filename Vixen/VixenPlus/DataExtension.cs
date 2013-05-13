using System;
using System.Globalization;
using System.Xml;

namespace VixenPlus
{
    public class DataExtension
    {
        private readonly string _extensionName;
        protected XmlDocument Document;
        protected XmlNode Node;

        public DataExtension(string extensionName)
        {
            _extensionName = extensionName;
            Document = Xml.CreateXmlDocument(extensionName);
            Node = Document.DocumentElement;
        }

        public bool IsEmpty
        {
            get { return !Node.HasChildNodes; }
        }

        public XmlNode RootNode
        {
            get { return Node; }
        }

        public DataExtension Clone()
        {
            var extension = new DataExtension(_extensionName);
            Xml.CloneNode(extension.Document, Document.DocumentElement, true);
            return extension;
        }

        public bool GetBoolean(XmlNode setupDataNode, string childNode, bool defaultValue)
        {
            XmlNode node = setupDataNode.SelectSingleNode(childNode);
            if (node != null)
            {
                try
                {
                    return Convert.ToBoolean(node.InnerText);
                }
                catch
                {
                    return false;
                }
            }
            SetBoolean(setupDataNode, childNode, defaultValue);
            return defaultValue;
        }

        public byte[] GetBytes(XmlNode setupDataNode, string childNode, byte[] defaultValue)
        {
            XmlNode node = setupDataNode.SelectSingleNode(childNode);
            if (node != null)
            {
                try
                {
                    return Convert.FromBase64String(node.InnerText);
                }
                catch
                {
                    return new byte[0];
                }
            }
            SetBytes(setupDataNode, childNode, defaultValue);
            return defaultValue;
        }

        public int GetInteger(XmlNode setupDataNode, string childNode, int defaultValue)
        {
            XmlNode node = setupDataNode.SelectSingleNode(childNode);
            if (node != null)
            {
                try
                {
                    return Convert.ToInt32(node.InnerText);
                }
                catch
                {
                    return 0;
                }
            }
            SetInteger(setupDataNode, childNode, defaultValue);
            return defaultValue;
        }

        public string GetString(XmlNode setupDataNode, string childNode, string defaultValue)
        {
            XmlNode node = setupDataNode.SelectSingleNode(childNode);
            if (node != null)
            {
                return node.InnerText;
            }
            SetString(setupDataNode, childNode, defaultValue);
            return defaultValue;
        }

        public void LoadFromXml(XmlNode contextNode)
        {
            Node = Xml.GetNodeAlways(contextNode, _extensionName);
            Document = Node.OwnerDocument;
        }

        public void SetBoolean(XmlNode setupDataNode, string childNode, bool value)
        {
            XmlNode newChild = setupDataNode.SelectSingleNode(childNode);
            if (newChild == null)
            {
                newChild = Document.CreateElement(childNode);
                setupDataNode.AppendChild(newChild);
            }
            newChild.InnerText = value.ToString();
        }

        public void SetBytes(XmlNode setupDataNode, string childNode, byte[] value)
        {
            XmlNode newChild = setupDataNode.SelectSingleNode(childNode);
            if (newChild == null)
            {
                newChild = Document.CreateElement(childNode);
                setupDataNode.AppendChild(newChild);
            }
            newChild.InnerText = Convert.ToBase64String(value);
        }

        public void SetInteger(XmlNode setupDataNode, string childNode, int value)
        {
            XmlNode newChild = setupDataNode.SelectSingleNode(childNode);
            if (newChild == null)
            {
                newChild = Document.CreateElement(childNode);
                setupDataNode.AppendChild(newChild);
            }
            newChild.InnerText = value.ToString(CultureInfo.InvariantCulture);
        }

        public void SetString(XmlNode setupDataNode, string childNode, string value)
        {
            XmlNode newChild = setupDataNode.SelectSingleNode(childNode);
            if (newChild == null)
            {
                newChild = Document.CreateElement(childNode);
                setupDataNode.AppendChild(newChild);
            }
            newChild.InnerText = value;
        }
    }
}