using System;
using System.Diagnostics;
using System.IO;

namespace Vixen
{
	public static class Paths
	{
		private static string m_binaryPath = string.Empty;

		private static string m_dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
		                                                Vendor.ProductName);

		public static string AddinPath
		{
			get { return Path.Combine(BinaryPath, "AddIns"); }
		}

		public static string AudioPath
		{
			get { return Path.Combine(DataPath, "Audio"); }
		}

		public static string BidirectionalPluginPath
		{
			get { return Path.Combine(PluginBasePath, "Bidirectional"); }
		}

		public static string BinaryPath
		{
			get
			{
				if (m_binaryPath == string.Empty)
				{
					using (Process process = Process.GetCurrentProcess())
					{
						m_binaryPath = Path.GetDirectoryName(process.MainModule.FileName);
					}
				}
				return m_binaryPath;
			}
		}

		public static string CurveLibraryPath
		{
			get { return Path.Combine(LibraryPath, "Curves"); }
		}

		public static string DataPath
		{
			get { return m_dataPath; }
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					m_dataPath = value;
				}
				else
				{
					m_dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), Vendor.ProductName);
				}
				if (!Directory.Exists(m_dataPath))
				{
					Directory.CreateDirectory(m_dataPath);
				}
			}
		}

		public static string DebugDumpFilePath
		{
			get { return Path.Combine(DataPath, "debug.txt"); }
		}

		public static string ImportExportPath
		{
			get { return Path.Combine(DataPath, "Imports and Exports"); }
		}

		public static string InputPluginPath
		{
			get { return Path.Combine(PluginBasePath, "Input"); }
		}

		public static string LibraryPath
		{
			get { return Path.Combine(DataPath, "Libraries"); }
		}

		public static string OutputPluginPath
		{
			get { return Path.Combine(PluginBasePath, "Output"); }
		}

		public static string PluginBasePath
		{
			get { return Path.Combine(BinaryPath, "Plugins"); }
		}

		public static string ProfilePath
		{
			get { return Path.Combine(DataPath, "Profiles"); }
		}

		public static string ProgramPath
		{
			get { return Path.Combine(DataPath, "Programs"); }
		}

		public static string RoutinePath
		{
			get { return Path.Combine(DataPath, "Routines"); }
		}

		public static string ScriptModulePath
		{
			get { return Path.Combine(BinaryPath, "Script Modules"); }
		}

		public static string SequencePath
		{
			get
			{
				string path = Host.Preferences.GetString("DefaultSequenceDirectory");
				if ((path.Length > 0) && Directory.Exists(path))
				{
					return path;
				}
				return Path.Combine(DataPath, "Sequences");
			}
		}

		public static string SourceFilePath
		{
			get { return Path.Combine(DataPath, "Source Files"); }
		}

		public static string TimerTraceFilePath
		{
			get { return Path.Combine(DataPath, "timers.trace"); }
		}

		public static string TriggerPluginPath
		{
			get { return Path.Combine(PluginBasePath, "Trigger"); }
		}

		public static string UIPluginPath
		{
			get { return Path.Combine(PluginBasePath, "UI"); }
		}
	}
}