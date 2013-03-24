using System;
using System.Collections.Generic;
using System.Xml;

namespace Vixen
{
	public class SetupData : DataExtension
	{
		public enum PluginType
		{
			Output,
			Input,
			Bidirectional
		}

		public SetupData() : base("PlugInData")
		{
		}

		public bool this[string pluginID]
		{
			get { return bool.Parse(GetPlugInData(pluginID).Attributes["enabled"].Value); }
			set { GetPlugInData(pluginID).Attributes["enabled"].Value = value.ToString(); }
		}

		public XmlNode CreatePlugInData(IHardwarePlugin plugIn)
		{
			XmlNode node = Xml.SetNewValue(base.m_rootNode, "PlugIn", string.Empty);
			Xml.SetAttribute(node, "name", plugIn.Name);
			Xml.SetAttribute(node, "key", plugIn.Name.GetHashCode().ToString());
			Xml.SetAttribute(node, "id", (GetAllPluginData().Count - 1).ToString());
			Xml.SetAttribute(node, "enabled", bool.TrueString);
			if (plugIn is IInputPlugin)
			{
				Xml.SetAttribute(node, "type", PluginType.Input.ToString());
				return node;
			}
			if (plugIn is IOutputPlugIn)
			{
				Xml.SetAttribute(node, "type", PluginType.Output.ToString());
				return node;
			}
			if (plugIn is IBidirectionalPlugin)
			{
				Xml.SetAttribute(node, "type", PluginType.Bidirectional.ToString());
			}
			return node;
		}

		public XmlNodeList GetAllPluginData()
		{
			return base.m_rootNode.SelectNodes("PlugIn");
		}

		public XmlNodeList GetAllPluginData(PluginType type)
		{
			return base.m_rootNode.SelectNodes(string.Format("PlugIn[@type='{0}']", type));
		}

		public XmlNodeList GetAllPluginData(PluginType type, bool enabledOnly)
		{
			return base.m_rootNode.SelectNodes(string.Format("PlugIn[@enabled='{0}' and @type='{1}']", enabledOnly, type));
		}

		public int GetHighestChannel(bool enabledOnly)
		{
			XmlNodeList list = enabledOnly ? GetAllPluginData(PluginType.Output, true) : GetAllPluginData();
			int num = 0;
			foreach (XmlNode node in list)
			{
				num = Math.Max(num, Convert.ToInt32(node.Attributes["to"].Value));
			}
			return num;
		}

		public OutputPlugin[] GetOutputPlugins()
		{
			var list = new List<OutputPlugin>();
			foreach (XmlNode node in GetAllPluginData())
			{
				list.Add(new OutputPlugin(node.Attributes["name"].Value, int.Parse(node.Attributes["id"].Value),
				                          bool.Parse(node.Attributes["enabled"].Value), int.Parse(node.Attributes["from"].Value),
				                          int.Parse(node.Attributes["to"].Value)));
			}
			return list.ToArray();
		}

		public XmlNode GetPlugInData(string pluginID)
		{
			return base.m_rootNode.SelectSingleNode(string.Format("PlugIn[@id='{0}']", pluginID));
		}

		public void RemovePlugInData(string pluginID)
		{
			base.m_rootNode.RemoveChild(GetPlugInData(pluginID));
			int num = 0;
			foreach (XmlNode node in GetAllPluginData())
			{
				node.Attributes["id"].Value = num.ToString();
				num++;
			}
		}

		public void ReplaceRoot(XmlNode newBranch)
		{
			XmlNode parentNode = base.m_rootNode.ParentNode;
			parentNode.RemoveChild(base.m_rootNode);
			XmlNode newChild = base.m_doc.ImportNode(newBranch, true);
			parentNode.AppendChild(newChild);
			base.m_rootNode = newChild;
		}
	}
}