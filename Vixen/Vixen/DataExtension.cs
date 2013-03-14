namespace Vixen
{
    using System;
    using System.Xml;

    public class DataExtension
    {
        protected XmlDocument m_doc;
        private string m_extensionName;
        protected XmlNode m_rootNode;

        public DataExtension(string extensionName)
        {
            this.m_extensionName = extensionName;
            this.m_doc = Xml.CreateXmlDocument(extensionName);
            this.m_rootNode = this.m_doc.DocumentElement;
        }

        public DataExtension Clone()
        {
            DataExtension extension = new DataExtension(this.m_extensionName);
            Xml.CloneNode(extension.m_doc, this.m_doc.DocumentElement, true);
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
            this.SetBoolean(setupDataNode, childNode, defaultValue);
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
            this.SetBytes(setupDataNode, childNode, defaultValue);
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
            this.SetInteger(setupDataNode, childNode, defaultValue);
            return defaultValue;
        }

        public string GetString(XmlNode setupDataNode, string childNode, string defaultValue)
        {
            XmlNode node = setupDataNode.SelectSingleNode(childNode);
            if (node != null)
            {
                return node.InnerText;
            }
            this.SetString(setupDataNode, childNode, defaultValue);
            return defaultValue;
        }

        public void LoadFromXml(XmlNode contextNode)
        {
            this.m_rootNode = Xml.GetNodeAlways(contextNode, this.m_extensionName);
            this.m_doc = this.m_rootNode.OwnerDocument;
        }

        public void SetBoolean(XmlNode setupDataNode, string childNode, bool value)
        {
            XmlNode newChild = setupDataNode.SelectSingleNode(childNode);
            if (newChild == null)
            {
                newChild = this.m_doc.CreateElement(childNode);
                setupDataNode.AppendChild(newChild);
            }
            newChild.InnerText = value.ToString();
        }

        public void SetBytes(XmlNode setupDataNode, string childNode, byte[] value)
        {
            XmlNode newChild = setupDataNode.SelectSingleNode(childNode);
            if (newChild == null)
            {
                newChild = this.m_doc.CreateElement(childNode);
                setupDataNode.AppendChild(newChild);
            }
            newChild.InnerText = Convert.ToBase64String(value);
        }

        public void SetInteger(XmlNode setupDataNode, string childNode, int value)
        {
            XmlNode newChild = setupDataNode.SelectSingleNode(childNode);
            if (newChild == null)
            {
                newChild = this.m_doc.CreateElement(childNode);
                setupDataNode.AppendChild(newChild);
            }
            newChild.InnerText = value.ToString();
        }

        public void SetString(XmlNode setupDataNode, string childNode, string value)
        {
            XmlNode newChild = setupDataNode.SelectSingleNode(childNode);
            if (newChild == null)
            {
                newChild = this.m_doc.CreateElement(childNode);
                setupDataNode.AppendChild(newChild);
            }
            newChild.InnerText = value;
        }

        public bool IsEmpty
        {
            get
            {
                return !this.m_rootNode.HasChildNodes;
            }
        }

        public XmlNode RootNode
        {
            get
            {
                return this.m_rootNode;
            }
        }
    }
}

