using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace VixenPlusCommon {
    public static class Paths {
        private static string _binaryPath = String.Empty;

        private static string _dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), Vendor.ProductName);

        public static string DataDir {
            get { return Path.Combine(Application.StartupPath, "data.dir"); }
        }

        public static string AudioPath {
            get { return Path.Combine(DataPath, "Audio"); }
        }

        public static string BinaryPath {
            get {
                if (_binaryPath != String.Empty) {
                    return _binaryPath;
                }
                using (var process = Process.GetCurrentProcess()) {
                    _binaryPath = Path.GetDirectoryName(process.MainModule.FileName);
                }
                return _binaryPath;
            }
        }

        public static string CurveLibraryPath {
            get { return Path.Combine(LibraryPath, "Curves"); }
        }

        public static string DataFolder {
            get { return @"\" + "Data"; }
        }

        public static string DataPath {
            get { return _dataPath; }
            set {
                _dataPath = !String.IsNullOrEmpty(value)
                    ? value : Path.Combine(MyDocutments, Vendor.ProductName);
                if (!Directory.Exists(_dataPath)) {
                    Directory.CreateDirectory(_dataPath);
                }
            }
        }

        public static string ImportExportPath {
            get { return Path.Combine(SupportingFilesPath, "Imports and Exports"); }
        }

        private static string LibraryPath {
            get { return Path.Combine(SupportingFilesPath, "Libraries"); }
        }

        public static string MapperPath {
            get { return Path.Combine(SupportingFilesPath, "Mapper"); }
        }

        public static string MyDocutments {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.Personal); }
        }

        public static string NutcrackerDataPath {
            get { return Path.Combine(SupportingFilesPath, "Nutcracker"); }
        }

        public static string NutcrackerDataFile {
            get { return Path.Combine(NutcrackerDataPath, "xlights_rgbeffects.xml"); }
        }

        public static string OutputPluginPath {
            get { return Path.Combine(PluginBasePath, "Output"); }
        }

        private static string PluginBasePath {
            get { return Path.Combine(BinaryPath, "Plugins"); }
        }

        public static string ProfilePath {
            get { return Path.Combine(DataPath, "Profiles"); }
        }

        public static string ProfileGeneration {
            get { return Path.Combine(SupportingFilesPath, "Templates"); }
        }

        //public static string ProgramPath {
        //    get { return Path.Combine(DataPath, "Programs"); }
        //}

        public static string RoutinePath {
            get { return Path.Combine(SupportingFilesPath, "Routines"); }
        }

        public static string SequencePath {
            get {
                var path = Preference2.GetInstance().GetString("DefaultSequenceDirectory");
                if ((path.Length > 0) && Directory.Exists(path)) {
                    return path;
                }
                return Path.Combine(DataPath, "Sequences");
            }
        }

        public static string SupportingFilesPath {
            get { return Path.Combine(DataPath, "Support"); }
        }

        public static string UIPluginPath {
            get { return Path.Combine(PluginBasePath, "UI"); }
        }
    }
}