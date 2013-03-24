using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Vixen
{
	internal class HardwarePlugins
	{
		private static readonly Dictionary<string, IHardwarePlugin> m_pluginCache = new Dictionary<string, IHardwarePlugin>();

		public static IHardwarePlugin FindPlugin(string pluginName, string directory, string interfaceName)
		{
			return FindPlugin(pluginName, false, directory, interfaceName);
		}

		public static IHardwarePlugin FindPlugin(string pluginName, bool uniqueInstance, string directory,
		                                         string interfaceName)
		{
			foreach (IHardwarePlugin plugin2 in m_pluginCache.Values)
			{
				if (plugin2.Name == pluginName)
				{
					if (uniqueInstance)
					{
						return (IHardwarePlugin) Activator.CreateInstance(plugin2.GetType());
					}
					return plugin2;
				}
			}
			foreach (string str in Directory.GetFiles(directory, "*.dll"))
			{
				try
				{
					Assembly assembly = Assembly.LoadFile(str);
					foreach (Type type in assembly.GetExportedTypes())
					{
						foreach (Type type2 in type.GetInterfaces())
						{
							if (type2.Name == interfaceName)
							{
								var plugin = (IHardwarePlugin) Activator.CreateInstance(type);
								if (!m_pluginCache.ContainsKey(str))
								{
									m_pluginCache[str] = plugin;
								}
								if (plugin.Name == pluginName)
								{
									return plugin;
								}
/*
								plugin = null;
*/
							}
						}
					}
				}
				catch
				{
				}
			}
			return null;
		}

		public static List<string> LoadPluginNames(string directory, string interfaceName)
		{
			var list = new List<string>();
			foreach (string str in Directory.GetFiles(directory, "*.dll"))
			{
				IHardwarePlugin plugin;
				if (!m_pluginCache.TryGetValue(str, out plugin))
				{
					try
					{
						Assembly assembly = Assembly.LoadFile(str);
						foreach (Type type in assembly.GetExportedTypes())
						{
							foreach (Type type2 in type.GetInterfaces())
							{
								if (type2.Name == interfaceName)
								{
									plugin = (IHardwarePlugin) Activator.CreateInstance(type);
									m_pluginCache[str] = plugin;
								}
							}
						}
					}
					catch
					{
					}
				}
				if (plugin != null)
				{
					list.Add(plugin.Name);
				}
			}
			return list;
		}

		public static List<IHardwarePlugin> LoadPlugins(string directory, string interfaceName)
		{
			var list = new List<IHardwarePlugin>();
			var dialog = new ProgressDialog();
			dialog.Show();
			foreach (string str in Directory.GetFiles(directory, "*.dll", SearchOption.TopDirectoryOnly))
			{
				IHardwarePlugin plugin;
				if (!m_pluginCache.TryGetValue(str, out plugin))
				{
					try
					{
						Assembly assembly = Assembly.LoadFile(str);
						foreach (Type type in assembly.GetExportedTypes())
						{
							foreach (Type type2 in type.GetInterfaces())
							{
								if (type2.Name == interfaceName)
								{
									dialog.Message = "Loading " + Path.GetFileName(str);
									plugin = (IHardwarePlugin) Activator.CreateInstance(type);
									m_pluginCache[str] = plugin;
								}
							}
						}
					}
					catch
					{
					}
				}
				if (plugin != null)
				{
					list.Add(plugin);
				}
			}
			dialog.Hide();
			dialog.Dispose();
			return list;
		}
	}
}