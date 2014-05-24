using System;
using System.Globalization;
using System.Xml;

namespace VixenPlus {
    public class DataExtension
    {
        private readonly string _extensionName;
        protected XmlDocument Document;


        protected DataExtension(string extensionName)
        {
            _extensionName = extensionName;
            Document = Xml.CreateXmlDocument(extensionName);
            RootNode = Document.DocumentElement;
        }

        public XmlNode RootNode { get; protected set; }

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

        public void GetBytes(XmlNode setupDataNode, string childNode, byte[] defaultValue)
        {
            var node = setupDataNode.SelectSingleNode(childNode);
            if (node != null)
            {
                return;
            }
            SetBytes(setupDataNode, childNode, defaultValue);
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


        private void SetBytes(XmlNode setupDataNode, string childNode, byte[] value)
        {
            var newChild = setupDataNode.SelectSingleNode(childNode);
            if (newChild == null)
            {
                newChild = Document.CreateElement(childNode);
                setupDataNode.AppendChild(newChild);
            }
            newChild.InnerText = Convert.ToBase64String(value);
        }


        private void SetInteger(XmlNode setupDataNode, string childNode, int value)
        {
            var newChild = setupDataNode.SelectSingleNode(childNode);
            if (newChild == null)
            {
                newChild = Document.CreateElement(childNode);
                setupDataNode.AppendChild(newChild);
            }
            newChild.InnerText = value.ToString(CultureInfo.InvariantCulture);
        }


        private void SetString(XmlNode setupDataNode, string childNode, string value)
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