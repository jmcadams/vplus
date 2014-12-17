using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using VixenPlusCommon;

namespace FileHelper {
    public static class FileHelper {

        private static readonly Dictionary<string, ISeqFileIO> PluginCache = new Dictionary<string, ISeqFileIO>();

        static FileHelper() {
            LoadPlugins();
        }


        public static Dictionary<string, ISeqFileIO> GetIoDictionary() {
            return PluginCache;
        }

        private static void LoadPlugins() {
            foreach (var str in Directory.GetFiles(Paths.OutputPluginPath, Vendor.All + Vendor.AppExtension, SearchOption.TopDirectoryOnly)) {
                try {
                    var assembly = Assembly.LoadFile(str);
                    foreach (var desiredType in from exportedTypes in assembly.GetExportedTypes()
                                         from interfaces in exportedTypes.GetInterfaces()
                                         where interfaces.Name == "ISeqFileIO"
                                         select exportedTypes) {
                        ISeqFileIO plugin;
                        
                        // Do we have a cache hit?  Exit early
                        if (PluginCache.TryGetValue(desiredType.Name, out plugin)) {
                            continue;
                        }

                        //Instanciate and add to the cache
                        plugin = (ISeqFileIO)Activator.CreateInstance(desiredType);
                        PluginCache[desiredType.Name] = plugin;
                    }
                }
                catch (Exception e) {
                    e.ToString().CrashLog();
                }
            }
        }
    }
}
