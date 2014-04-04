using System.Globalization;
using System.Xml;

namespace VixenPlus {
    public class SetupData : DataExtension {
        public enum PluginType {
            Output,
            Input
        }

        public SetupData() : base("PlugInData") {}


        public XmlNode CreatePlugInData(IHardwarePlugin plugIn) {
            var node = Xml.SetNewValue(RootNode, "PlugIn", string.Empty);
            Xml.SetAttribute(node, "name", plugIn.Name);
            Xml.SetAttribute(node, "key", plugIn.Name.GetHashCode().ToString(CultureInfo.InvariantCulture));
            Xml.SetAttribute(node, "id", (GetAllPluginData().Count - 1).ToString(CultureInfo.InvariantCulture));
            Xml.SetAttribute(node, "enabled", bool.TrueString);
            //if (plugIn is IInputPlugin) {
            //    Xml.SetAttribute(node, "type", PluginType.Input.ToString());
            //    return node;
            //}
            if (!(plugIn is IOutputPlugIn)) {
                return node;
            }
            Xml.SetAttribute(node, "type", PluginType.Output.ToString());
            return node;
            //if (plugIn is IBidirectionalPlugin)
            //{
            //    Xml.SetAttribute(node, "type", PluginType.Bidirectional.ToString());
            //}
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