using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using VixenPlusCommon;

namespace VixenPlus {
    public static class FileIOHelper {

        private static readonly Dictionary<string, IFileIOHandler> PluginCache = new Dictionary<string, IFileIOHandler>();

        static FileIOHelper() {
            LoadPlugins();
        }



        private static void LoadPlugins() {
            foreach (var dllFile in Directory.GetFiles(Paths.OutputPluginPath, Vendor.SeqFileIO + Vendor.AppExtension, SearchOption.TopDirectoryOnly)) {
                try {
                    var assembly = Assembly.LoadFile(dllFile);

                    var validTypes = from exportedType in assembly.GetExportedTypes()
                        from Interface in exportedType.GetInterfaces()
                        where Interface.Name == "IFileIOHandler"
                        select exportedType;

                    foreach (var desiredType in validTypes) {
                        // Cant use abstract types so skip it.
                        if (desiredType.IsAbstract) continue;

                        // If we have a cache hit, we can just skip this one.
                        IFileIOHandler plugin;
                        if (PluginCache.TryGetValue(desiredType.Name, out plugin)) continue;

                        PluginCache[desiredType.Name] = (IFileIOHandler) Activator.CreateInstance(desiredType);
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
            
            return sb.Length > 0 ? sb.Remove(sb.Length - 1, 1).ToString() : String.Empty;
        }


        public static IFileIOHandler GetHelperByName(string s) {
            return PluginCache.First(v => v.Value.DialogFilterList().StartsWith(s)).Value;
        }


        public static IFileIOHandler GetNativeHelper() {
            return PluginCache.First(fio => fio.Value.IsNativeToVixenPlus()).Value;
        }


        public static IFileIOHandler GetByExtension(string s) {
            return PluginCache.Select(v => v.Value).Where(v => v.FileExtension() == s && v.IsNativeToVixenPlus()).OrderBy(v => v.PreferredOrder()).First();
        }
    }
}