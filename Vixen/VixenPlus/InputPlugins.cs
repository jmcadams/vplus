using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Vixen
{
	internal class InputPlugins : HardwarePlugins
	{
		private const string INPUT_PLUGIN_INTERFACE_NAME = "IInputPlugin";

		public static IHardwarePlugin FindPlugin(string pluginName)
		{
			return FindPlugin(pluginName, Paths.InputPluginPath, "IInputPlugin");
		}

		public static IHardwarePlugin FindPlugin(string pluginName, bool uniqueInstance)
		{
			return FindPlugin(pluginName, uniqueInstance, Paths.InputPluginPath, "IInputPlugin");
		}

		public static List<string> LoadPluginNames()
		{
			return LoadPluginNames(Paths.InputPluginPath, "IInputPlugin");
		}

		public static List<IHardwarePlugin> LoadPlugins()
		{
			return LoadPlugins(Paths.InputPluginPath, "IInputPlugin");
		}

		public static void VerifyPlugIns(IExecutable _object)
		{
			XmlNodeList allPluginData = _object.PlugInData.GetAllPluginData(SetupData.PluginType.Input);
			if (allPluginData.Count != 0)
			{
				var builder = new StringBuilder();
				var dialog = new ProgressDialog();
				dialog.Show();
				int num = 0;
				foreach (XmlNode node in allPluginData)
				{
					string pluginName = node.Attributes["name"].Value;
					dialog.Message = "Verifying " + pluginName;
					if (FindPlugin(pluginName) == null)
					{
						XmlDocument targetDoc = Xml.CreateXmlDocument(node.OwnerDocument.DocumentElement.Name);
						Xml.CloneNode(targetDoc, node, true);
						Host.GetUniqueKey();
						string str = string.Format("{0}.{1}.{2}.{3}.vda",
						                           new object[]
							                           {
								                           _object.Name, node.Attributes["name"].Value, DateTime.Today.ToString("MMddyyyy"),
								                           DateTime.Now.ToString("HHmmssfff")
							                           });
						targetDoc.Save(Path.Combine(Paths.ImportExportPath, str));
						builder.Append(str + "\n");
						node.ParentNode.RemoveChild(node);
					}
					else
					{
						num++;
					}
				}
				dialog.Hide();
				dialog.Dispose();
				if (builder.Length != 0)
				{
					string str3 = (_object is EventSequence) ? "sequence" : "program";
					MessageBox.Show(
						string.Format(
							"Input plugins used by this {1} were missing.\nThe following exports were created containing the data for those missing plugins:\n\n{0}\n\nThe data has been removed from the {1}, but the {1} has not been saved.",
							builder, str3), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
		}
	}
}