using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using VixenPlusCommon;

namespace VixenPlus {
    public static class SequenceFileIOHelper {

        private static readonly Dictionary<string, ISeqIOHandler> PluginCache = new Dictionary<string, ISeqIOHandler>();

        static SequenceFileIOHelper() {
            LoadPlugins();
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


        public static string GetOpenFilters() {
            var filter = PluginCache.Select(v => v.Value).Where(v => v.CanOpen()).OrderBy(handler => handler.PreferredOrder()).ToArray();

            var sb = new StringBuilder();
            foreach (var f in filter) {
                sb.Append(f.DialogFilterList()).Append("|");
            }

            return sb.Remove(sb.Length - 1, 1).ToString();
        }


        public static string GetSaveFilters() {
            var filter = PluginCache.Select(v => v.Value).Where(v => v.CanSave()).OrderBy(handler => handler.PreferredOrder()).ToArray();

            var sb = new StringBuilder();
            foreach (var f in filter) {
                sb.Append(f.DialogFilterList()).Append("|");
            }
            
            return sb.Remove(sb.Length - 1, 1).ToString();
        }


        public static ISeqIOHandler GetHelperByName(string s) {
            return PluginCache.First(v => v.Value.DialogFilterList().StartsWith(s)).Value;
        }


        public static ISeqIOHandler GetNativeHelper() {
            return PluginCache.First(fio => fio.Value.IsNativeToVixenPlus()).Value;
        }
    }
}