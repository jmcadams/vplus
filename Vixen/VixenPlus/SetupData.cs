using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace VixenPlus
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

		public bool this[string pluginId]
		{
			get { return bool.Parse(GetPlugInData(pluginId).Attributes["enabled"].Value); }
			set { GetPlugInData(pluginId).Attributes["enabled"].Value = value.ToString(); }
		}

		public XmlNode CreatePlugInData(IHardwarePlugin plugIn)
		{
			XmlNode node = Xml.SetNewValue(Node, "PlugIn", string.Empty);
			Xml.SetAttribute(node, "name", plugIn.Name);
			Xml.SetAttribute(node, "key", plugIn.Name.GetHashCode().ToString(CultureInfo.InvariantCulture));
			Xml.SetAttribute(node, "id", (GetAllPluginData().Count - 1).ToString(CultureInfo.InvariantCulture));
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
			//if (plugIn is IBidirectionalPlugin)
			//{
			//    Xml.SetAttribute(node, "type", PluginType.Bidirectional.ToString());
			//}
			return node;
		}

		public XmlNodeList GetAllPluginData()
		{
			return Node.SelectNodes("PlugIn");
		}

        // TODO this is where 2.1 profiles break
		public XmlNodeList GetAllPluginData(PluginType type)
		{
            var node = Node.SelectNodes(string.Format("PlugIn[@type='{0}']", type));
		    if (node != null &&
		        node.Count == 0 &&
		        type == PluginType.Output) { // Hack for 2.1
		        node = GetAllPluginData();
		    }
		    return node;
		}

		public XmlNodeList GetAllPluginData(PluginType type, bool enabledOnly)
		{
            var node = Node.SelectNodes(string.Format("PlugIn[@enabled='{0}' and @type='{1}']", enabledOnly, type));
            if (node != null &&
                node.Count == 0 &&
                type == PluginType.Output) { // Hack for 2.1
                    node = Node.SelectNodes(string.Format("PlugIn[@enabled='{0}']", enabledOnly));
            }

			return node;
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

		public XmlNode GetPlugInData(string pluginId)
		{
			return Node.SelectSingleNode(string.Format("PlugIn[@id='{0}']", pluginId));
		}

		public void RemovePlugInData(string pluginId)
		{
			Node.RemoveChild(GetPlugInData(pluginId));
			int num = 0;
			foreach (XmlNode node in GetAllPluginData())
			{
				node.Attributes["id"].Value = num.ToString(CultureInfo.InvariantCulture);
				num++;
			}
		}

		public void ReplaceRoot(XmlNode newBranch)
		{
			XmlNode parentNode = Node.ParentNode;
			parentNode.RemoveChild(Node);
			XmlNode newChild = Document.ImportNode(newBranch, true);
			parentNode.AppendChild(newChild);
			Node = newChild;
		}
	}
}