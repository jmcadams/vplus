using System;
using System.Xml;

namespace Vixen
{
	public class DataExtension
	{
		private readonly string m_extensionName;
		protected XmlDocument m_doc;
		protected XmlNode m_rootNode;

		public DataExtension(string extensionName)
		{
			m_extensionName = extensionName;
			m_doc = Xml.CreateXmlDocument(extensionName);
			m_rootNode = m_doc.DocumentElement;
		}

		public bool IsEmpty
		{
			get { return !m_rootNode.HasChildNodes; }
		}

		public XmlNode RootNode
		{
			get { return m_rootNode; }
		}

		public DataExtension Clone()
		{
			var extension = new DataExtension(m_extensionName);
			Xml.CloneNode(extension.m_doc, m_doc.DocumentElement, true);
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
			m_rootNode = Xml.GetNodeAlways(contextNode, m_extensionName);
			m_doc = m_rootNode.OwnerDocument;
		}

		public void SetBoolean(XmlNode setupDataNode, string childNode, bool value)
		{
			XmlNode newChild = setupDataNode.SelectSingleNode(childNode);
			if (newChild == null)
			{
				newChild = m_doc.CreateElement(childNode);
				setupDataNode.AppendChild(newChild);
			}
			newChild.InnerText = value.ToString();
		}

		public void SetBytes(XmlNode setupDataNode, string childNode, byte[] value)
		{
			XmlNode newChild = setupDataNode.SelectSingleNode(childNode);
			if (newChild == null)
			{
				newChild = m_doc.CreateElement(childNode);
				setupDataNode.AppendChild(newChild);
			}
			newChild.InnerText = Convert.ToBase64String(value);
		}

		public void SetInteger(XmlNode setupDataNode, string childNode, int value)
		{
			XmlNode newChild = setupDataNode.SelectSingleNode(childNode);
			if (newChild == null)
			{
				newChild = m_doc.CreateElement(childNode);
				setupDataNode.AppendChild(newChild);
			}
			newChild.InnerText = value.ToString();
		}

		public void SetString(XmlNode setupDataNode, string childNode, string value)
		{
			XmlNode newChild = setupDataNode.SelectSingleNode(childNode);
			if (newChild == null)
			{
				newChild = m_doc.CreateElement(childNode);
				setupDataNode.AppendChild(newChild);
			}
			newChild.InnerText = value;
		}
	}
}