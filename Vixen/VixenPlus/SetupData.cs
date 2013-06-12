using System;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace VixenPlus {
    public class SetupData : DataExtension {
        public enum PluginType {
            Output,
            Input,
            Bidirectional
        }

        public SetupData() : base("PlugInData") {}


        public bool this[string pluginId] {
            get {
                var xmlAttributeCollection = GetPlugInData(pluginId).Attributes;
                return xmlAttributeCollection != null && bool.Parse(xmlAttributeCollection["enabled"].Value);
            }
            set {
                var xmlAttributeCollection = GetPlugInData(pluginId).Attributes;
                if (xmlAttributeCollection != null) {
                    xmlAttributeCollection["enabled"].Value = value.ToString();
                }
            }
        }


        public XmlNode CreatePlugInData(IHardwarePlugin plugIn) {
            var node = Xml.SetNewValue(RootNode, "PlugIn", string.Empty);
            Xml.SetAttribute(node, "name", plugIn.Name);
            Xml.SetAttribute(node, "key", plugIn.Name.GetHashCode().ToString(CultureInfo.InvariantCulture));
            Xml.SetAttribute(node, "id", (GetAllPluginData().Count - 1).ToString(CultureInfo.InvariantCulture));
            Xml.SetAttribute(node, "enabled", bool.TrueString);
            if (plugIn is IInputPlugin) {
                Xml.SetAttribute(node, "type", PluginType.Input.ToString());
                return node;
            }
            if (plugIn is IOutputPlugIn) {
                Xml.SetAttribute(node, "type", PluginType.Output.ToString());
                return node;
            }
            //if (plugIn is IBidirectionalPlugin)
            //{
            //    Xml.SetAttribute(node, "type", PluginType.Bidirectional.ToString());
            //}
            return node;
        }


        public XmlNodeList GetAllPluginData() {
            return RootNode.SelectNodes("PlugIn");
        }


        public XmlNodeList GetAllPluginData(PluginType type) {
            var node = RootNode.SelectNodes(string.Format("PlugIn[@type='{0}']", type));
            if (node != null && node.Count == 0 && type == PluginType.Output) { // Hack for 2.1
                node = GetAllPluginData();
            }
            return node;
        }


        public XmlNodeList GetAllPluginData(PluginType type, bool enabledOnly) {
            var node = RootNode.SelectNodes(string.Format("PlugIn[@enabled='{0}' and @type='{1}']", enabledOnly, type));
            if (node != null && node.Count == 0 && type == PluginType.Output) { // Hack for 2.1
                node = RootNode.SelectNodes(string.Format("PlugIn[@enabled='{0}']", enabledOnly));
            }

            return node;
        }


        public int GetHighestChannel(bool enabledOnly) {
            var list = enabledOnly ? GetAllPluginData(PluginType.Output, true) : GetAllPluginData();
            return (from XmlNode node in list
                    let attributes = node.Attributes
                    where attributes != null
                    select Convert.ToInt32(attributes["to"].Value)).Concat(new[] {0}).Max();
        }


        public OutputPlugin[] GetOutputPlugins() {
            return (from XmlNode node in GetAllPluginData()
                    let attributes = node.Attributes
                    where attributes != null
                    select new OutputPlugin(attributes["name"].Value, int.Parse(attributes["id"].Value), bool.Parse(attributes["enabled"].Value), int.Parse(attributes["from"].Value), int.Parse(attributes["to"].Value))).ToArray();
        }


        public XmlNode GetPlugInData(string pluginId) {
            return RootNode.SelectSingleNode(string.Format("PlugIn[@id='{0}']", pluginId));
        }


        public void RemovePlugInData(string pluginId) {
            RootNode.RemoveChild(GetPlugInData(pluginId));
            var num = 0;
            foreach (XmlNode node in GetAllPluginData()) {
                if (node.Attributes == null) {
                    continue;
                }
                node.Attributes["id"].Value = num.ToString(CultureInfo.InvariantCulture);
                num++;
            }
        }


        public void ReplaceRoot(XmlNode newBranch) {
            var parentNode = RootNode.ParentNode;
            if (parentNode != null) {
                parentNode.RemoveChild(RootNode);
            }
            var newChild = Document.ImportNode(newBranch, true);
            if (parentNode != null) {
                parentNode.AppendChild(newChild);
            }
            RootNode = newChild;
        }
    }
}
