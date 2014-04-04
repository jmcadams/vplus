using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;



using VixenPlusCommon;

namespace VixenPlus {
    internal static class OutputPlugins {
        private static IHardwarePlugin FindPlugin(string pluginName) {
            return HardwarePlugins.FindPlugin(pluginName, Paths.OutputPluginPath, "IOutputPlugIn");
        }


        public static IHardwarePlugin FindPlugin(string pluginName, bool uniqueInstance) {
            return HardwarePlugins.FindPlugin(pluginName, uniqueInstance, Paths.OutputPluginPath, "IOutputPlugIn");
        }

        // ReSharper disable ReturnTypeCanBeEnumerable.Global
        public static List<IHardwarePlugin> LoadPlugins() {
            // ReSharper restore ReturnTypeCanBeEnumerable.Global
            return HardwarePlugins.LoadPlugins(Paths.OutputPluginPath, "IOutputPlugIn");
        }


        public static void VerifyPlugIns(IExecutable _object) {
            var allPluginData = _object.PlugInData.GetAllPluginData(SetupData.PluginType.Output);

            if (allPluginData.Count == 0) {
                return;
            }
            var builder = new StringBuilder();
            var plugins = new StringBuilder();
            foreach (XmlNode node in allPluginData) {
                if (node.Attributes == null) {
                    continue;
                }
                var pluginName = node.Attributes["name"].Value;
                if (FindPlugin(pluginName) != null) {
                    continue;
                }
                if (node.OwnerDocument != null &&
                    node.OwnerDocument.DocumentElement != null) {
                    var targetDoc = Xml.CreateXmlDocument(node.OwnerDocument.DocumentElement.Name);
                    Xml.CloneNode(targetDoc, node, true);
                    Host.GetUniqueKey();
                    var str = string.Format(
                        "{0}.{1}.{2}.{3}.vda",
                        new object[] {
                            _object.Name, node.Attributes["name"].Value, DateTime.Today.ToString("MMddyyyy"),
                            DateTime.Now.ToString("HHmmssfff")
                        });
                    targetDoc.Save(Path.Combine(Paths.ImportExportPath, str));
                    builder.Append(str + "\n");
                    plugins.Append(pluginName + "\n");
                }
                _object.PlugInData.RemovePlugInData(node.Attributes["id"].Value);
            }
            if (builder.Length == 0) {
                return;
            }
            var msg =
                string.Format("Output plugins used by this {1} were missing or have not been implemented by {2}.\n\n" +
                              "The following exports were created containing the data for those missing plugins:\n\n" +
                              "{0}\nThe data has been removed from the {1}, but it has not been saved.\n\n" +
                              "NOTE: You may need to edit your profile(s) and remove the\n\n{3}\nplugin(s) so that you don't continue to see this message",
                    builder,(_object is EventSequence) ? "sequence" : "program", Vendor.ProductName, plugins);
            MessageBox.Show(msg, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}