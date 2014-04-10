using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;



using VixenPlusCommon;

namespace VixenPlus {
    internal static class HardwarePlugins {
        private static readonly Dictionary<string, IHardwarePlugin> PluginCache =
            new Dictionary<string, IHardwarePlugin>();


        public static IHardwarePlugin FindPlugin(string pluginName, string directory, string interfaceName) {
            return FindPlugin(pluginName, false, directory, interfaceName);
        }


        public static IHardwarePlugin FindPlugin(string pluginName, bool uniqueInstance, string directory,
            string interfaceName) {
            foreach (var plugin2 in PluginCache.Values.Where(plugin2 => plugin2.Name == pluginName)) {
                if (uniqueInstance) {
                    return (IHardwarePlugin) Activator.CreateInstance(plugin2.GetType());
                }
                return plugin2;
            }
            foreach (var str in Directory.GetFiles(directory, Vendor.All + Vendor.AppExtension)) {
                try {
                    var assembly = Assembly.LoadFile(str);
                    foreach (var plugin in from type in assembly.GetExportedTypes()
                        from typeInterface in type.GetInterfaces()
                        where typeInterface.Name == interfaceName
                        select (IHardwarePlugin) Activator.CreateInstance(type)) {
                        if (!PluginCache.ContainsKey(str)) {
                            PluginCache[str] = plugin;
                        }
                        if (plugin.Name == pluginName) {
                            return plugin;
                        }
                    }
                }
                catch (Exception e) {
                    e.ToString().CrashLog();
                }
            }
            return null;
        }


        public static List<IHardwarePlugin> LoadPlugins(string directory, string interfaceName) {
            var list = new List<IHardwarePlugin>();

            if (!Directory.Exists(directory)) {
                return list;
            }

            foreach (var str in
                Directory.GetFiles(directory, Vendor.All + Vendor.AppExtension, SearchOption.TopDirectoryOnly)) {
                try {
                    var assembly = Assembly.LoadFile(str);
                    foreach (var type in from type in assembly.GetExportedTypes()
                        from type2 in type.GetInterfaces()
                        where type2.Name == interfaceName
                        select type) {
                        IHardwarePlugin plugin;
                        if (!PluginCache.TryGetValue(type.Name, out plugin)) {
                            plugin = (IHardwarePlugin) Activator.CreateInstance(type);
                            PluginCache[type.Name] = plugin;
                        }
                        list.Add(plugin);
                    }
                }
                catch (Exception e) {
                    e.ToString().CrashLog();
                }
            }
            return list;
        }
    }
}
