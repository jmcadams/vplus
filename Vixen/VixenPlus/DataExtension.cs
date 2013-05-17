using System;
using System.Globalization;
using System.Xml;

namespace VixenPlus
{
    public class DataExtension
    {
        private readonly string _extensionName;
        protected XmlDocument Document;


        public DataExtension(string extensionName)
        {
            _extensionName = extensionName;
            Document = Xml.CreateXmlDocument(extensionName);
            RootNode = Document.DocumentElement;
        }

        public bool IsEmpty
        {
            get { return !RootNode.HasChildNodes; }
        }

        public XmlNode RootNode { get; protected set; }


        public DataExtension Clone()
        {
            var extension = new DataExtension(_extensionName);
            Xml.CloneNode(extension.Document, Document.DocumentElement, true);
            return extension;
        }

        public bool GetBoolean(XmlNode setupDataNode, string childNode, bool defaultValue)
        {
            var node = setupDataNode.SelectSingleNode(childNode);
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
            var node = setupDataNode.SelectSingleNode(childNode);
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
            var node = setupDataNode.SelectSingleNode(childNode);
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
            var node = setupDataNode.SelectSingleNode(childNode);
            if (node != null)
            {
                return node.InnerText;
            }
            SetString(setupDataNode, childNode, defaultValue);
            return defaultValue;
        }

        public void LoadFromXml(XmlNode contextNode)
        {
            RootNode = Xml.GetNodeAlways(contextNode, _extensionName);
            Document = RootNode.OwnerDocument;
        }

        public void SetBoolean(XmlNode setupDataNode, string childNode, bool value)
        {
            var newChild = setupDataNode.SelectSingleNode(childNode);
            if (newChild == null)
            {
                newChild = Document.CreateElement(childNode);
                setupDataNode.AppendChild(newChild);
            }
            newChild.InnerText = value.ToString();
        }

        public void SetBytes(XmlNode setupDataNode, string childNode, byte[] value)
        {
            var newChild = setupDataNode.SelectSingleNode(childNode);
            if (newChild == null)
            {
                newChild = Document.CreateElement(childNode);
                setupDataNode.AppendChild(newChild);
            }
            newChild.InnerText = Convert.ToBase64String(value);
        }

        public void SetInteger(XmlNode setupDataNode, string childNode, int value)
        {
            var newChild = setupDataNode.SelectSingleNode(childNode);
            if (newChild == null)
            {
                newChild = Document.CreateElement(childNode);
                setupDataNode.AppendChild(newChild);
            }
            newChild.InnerText = value.ToString(CultureInfo.InvariantCulture);
        }

        public void SetString(XmlNode setupDataNode, string childNode, string value)
        {
            var newChild = setupDataNode.SelectSingleNode(childNode);
            if (newChild == null)
            {
                newChild = Document.CreateElement(childNode);
                setupDataNode.AppendChild(newChild);
            }
            newChild.InnerText = value;
        }
    }
}