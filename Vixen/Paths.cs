using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

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

    public static string DataFolder {
        get { return @"\" + "Data"; }
    }

    public static string ImportExportPath {
        get { return Path.Combine(DataPath, "Imports and Exports"); }
    }

    public static string InputPluginPath {
        get { return Path.Combine(PluginBasePath, "Input"); }
    }

    public static string LibraryPath {
        get { return Path.Combine(DataPath, "Libraries"); }
    }

    public static string MapperPath {
        get { return Path.Combine(ProfilePath, "Mapper"); }
    }

    public static string MyDocutments {
        get { return Environment.GetFolderPath(Environment.SpecialFolder.Personal); }
    }

    public static string NutcrackerEffectsPath {
        get { return Path.Combine(PluginBasePath, @"Nutcracker\Effects"); }
    }

    public static string NutcrackerModelsPath {
        get { return Path.Combine(PluginBasePath, @"Nutcracker\Models"); }
    }

    public static string NutcrackerDataPath {
        get { return Path.Combine(DataPath, "Nutcracker"); }
    }

    public static string NutcrackerDataFile {
        get { return Path.Combine(NutcrackerDataPath, "xlights_rgbeffects.xml"); }
    }

    public static string OutputPluginPath {
        get { return Path.Combine(PluginBasePath, "Output"); }
    }

    public static string PluginBasePath {
        get { return Path.Combine(BinaryPath, "Plugins"); }
    }

    public static string ProfilePath {
        get { return Path.Combine(DataPath, "Profiles"); }
    }

    public static string ProgramPath {
        get { return Path.Combine(DataPath, "Programs"); }
    }

    public static string RoutinePath {
        get { return Path.Combine(DataPath, "Routines"); }
    }

    public static string SequencePath {
        get {
            var path = Host.Preferences.GetString("DefaultSequenceDirectory");
            if ((path.Length > 0) && Directory.Exists(path)) {
                return path;
            }
            return Path.Combine(DataPath, "Sequences");
        }
    }

    public static string TimerTraceFilePath {
        get { return Path.Combine(DataPath, "timers.trace"); }
    }

    public static string UIPluginPath {
        get { return Path.Combine(PluginBasePath, "UI"); }
    }
}