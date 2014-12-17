using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using VixenPlusCommon;

namespace VixenPlus {
    public static class SequenceFileIOHelper {

        private static readonly Dictionary<string, ISeqIOHandler> PluginCache = new Dictionary<string, ISeqIOHandler>();


        static SequenceFileIOHelper() {
            LoadPlugins();
        }


        //public static void RefreshPlugins() {
        //    PluginCache.Clear();
        //    LoadPlugins();
        //}


        public static Dictionary<string, ISeqIOHandler> GetFileIOPlugins() {
            return PluginCache;
        } 


        private static void LoadPlugins() {
            foreach (var dllFile in Directory.GetFiles(Paths.OutputPluginPath, Vendor.SeqFileIO + Vendor.AppExtension, SearchOption.TopDirectoryOnly)) {
                try {
                    var assembly = Assembly.LoadFile(dllFile);
                    foreach (var desiredType in from exportedType in assembly.GetExportedTypes()
                        from Interface in exportedType.GetInterfaces()
                        where Interface.Name == "ISeqIOHandler"
                        select exportedType) {
                        ISeqIOHandler plugin;

                        // If we have a cache hit, we can just skip this one.
                        if (PluginCache.TryGetValue(desiredType.Name, out plugin)) {
                            continue;
                        }

                        PluginCache[desiredType.Name] = (ISeqIOHandler) Activator.CreateInstance(desiredType);
                    }

                }
                catch (Exception e) {
                    e.ToString().CrashLog();
                }
            }
        }

    }
}