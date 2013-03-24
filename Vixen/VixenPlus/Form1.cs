using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using FMOD;
using VixenPlus.Dialogs;

namespace VixenPlus
{
	internal sealed partial class Form1 : Form, ISystem
	{
		private const int HistoryMax = 7;
		private readonly EventHandler _historyItemClick;
		private readonly Host _host;
		private readonly TriggerImpl _iTriggerImpl;
		private readonly LoadableData _loadableData;
		private readonly Dictionary<string, List<LoadedObject>> _loadables;
		private readonly EventHandler _newMenuItemClick;
		private readonly Preference2 _preferences;
		private readonly Dictionary<string, IUIPlugIn> _registeredFileTypes;
		private readonly TimerExecutor _timerExecutor;
		private readonly string _timersPath;
		private string[] _audioDevices;
		private List<string> _history;
		private string _knownFileTypesFilter;
		private string _lastWindowsClipboardValue = "";
		private DateTime _shutdownAt;
		private Timers _timers;

		public Form1(string[] args)
		{
			var list = new List<string>();
			list.AddRange(args);
			//this.LoadVendorData();
			SetDataPath();
			Ensure(Paths.DataPath);
			Ensure(Paths.SequencePath);
			Ensure(Paths.ProgramPath);
			Ensure(Paths.ImportExportPath);
			Ensure(Paths.AudioPath);
			Ensure(Paths.ProfilePath);
			Ensure(Paths.RoutinePath);
			Ensure(Paths.SourceFilePath);
			Ensure(Paths.CurveLibraryPath);
			var splash = new Splash();
			splash.Show();
			splash.Refresh();
			InitializeComponent();
			SetVendorData();
			_registeredFileTypes = new Dictionary<string, IUIPlugIn>();
			_timersPath = Path.Combine(Paths.DataPath, "timers");
			helpProvider.HelpNamespace = Path.Combine(Paths.BinaryPath, Vendor.ProductName + ".chm");
			string path = Path.Combine(Paths.BinaryPath, "prepare.exe");
			if (File.Exists(path))
			{
				Process process = Process.Start(path);
				if (process != null)
				{
					process.WaitForExit();
				}
				File.Delete(path);
			}
			_preferences = Preference2.GetInstance();
			_preferences.PreferenceChange += PreferencesPreferenceChange;
			_host = new Host(this);
			_loadables = new Dictionary<string, List<LoadedObject>>();
			Interfaces.Available["ISystem"] = this;
			Interfaces.Available["IExecution"] = new ExecutionImpl(_host);
			Interfaces.Available["ITrigger"] = _iTriggerImpl = new TriggerImpl();
			_newMenuItemClick = NewMenuItemClick;
			_historyItemClick = HistoryItemClick;
			LoadHistory();
			_loadableData = new LoadableData();
			_loadableData.LoadFromXml(_preferences.XmlDoc.DocumentElement);
			splash.Task = "Starting add-ins";
			if (!list.Contains("no_addins"))
			{
				StartAddins();
			}
			splash.Task = "Starting triggers";
			if (!list.Contains("no_triggers"))
			{
				StartTriggers();
			}
			splash.Task = "Loading UI";
			LoadUIPlugins();
			Cursor = Cursors.WaitCursor;
			try
			{
				foreach (string str2 in args)
				{
					if (File.Exists(str2))
					{
						OpenSequence(str2);
					}
				}
			}
			finally
			{
				Cursor = Cursors.Default;
			}
			scheduleTimer.Interval = _preferences.GetInteger("TimerCheckFrequency")*1000;
			_timers = new Timers();
			if (File.Exists(_timersPath))
			{
				_timers.LoadFromXml(Xml.LoadDocument(_timersPath));
			}
			_timerExecutor = new TimerExecutor();
			scheduleTimer.Enabled = !_timers.TimersDisabled;
			if (_preferences.GetBoolean("EnableBackgroundSequence"))
			{
				_host.BackgroundSequenceName = _preferences.GetString("BackgroundSequence");
			}
			_host.StartBackgroundObjects();
			SetShutdownTime(_preferences.GetString("ShutdownTime"));
			splash.Hide();
			splash.Dispose();
			if (!(list.Contains("no_update") || File.Exists(Path.Combine(Paths.DataPath, "no.update"))))
			{
				CheckForUpdates();
			}
		}

		public int GetExecutingTimerExecutionContextHandle(int executingTimerIndex)
		{
			return _timerExecutor.GetExecutingTimerExecutionContextHandle(executingTimerIndex);
		}

		public Form InstantiateForm(ConstructorInfo constructorInfo, params object[] parameters)
		{
			if (InvokeRequired)
			{
				return (Form) Invoke(new InstantiateFormDelegate(InstantiateForm), new object[] {constructorInfo, parameters});
			}
			var child = (Form) constructorInfo.Invoke(parameters);
			if (child == null)
			{
				return null;
			}
			if (!(child is OutputPlugInUIBase))
			{
				return null;
			}
			var base2 = (OutputPlugInUIBase) child;
			var executable = (IExecutable) Host.Communication["CurrentObject"];
			if (executable != null)
			{
				string str = executable.Key.ToString(CultureInfo.InvariantCulture);
				XmlNode node2 = null;
				XmlNode node = ((XmlNode) Host.Communication["SetupNode_" + str]).SelectSingleNode("DialogPositions");
				object obj2;
				if (Host.Communication.TryGetValue("KeyInterceptor_" + str, out obj2))
				{
					base2.ExecutionParent = (IVixenMDI) obj2;
				}
				if (Host.Communication.TryGetValue("SetupNode_" + str, out obj2))
				{
					base2.DataNode = (XmlNode) obj2;
				}
				child.ControlBox = true;
				if (!FormContainsChild(this, child))
				{
					child.MdiParent = this;
				}
				((ExecutionContext) Host.Communication["ExecutionContext_" + str]).OutputPlugInForms.Add(child);
				child.Show();
				if (node != null)
				{
					node2 = node.SelectSingleNode(child.Name);
				}
				if ((node2 != null) && _preferences.GetBoolean("SavePlugInDialogPositions"))
				{
					if (node2.Attributes != null)
					{
						XmlAttribute attribute = node2.Attributes["x"];
						XmlAttribute attribute2 = node2.Attributes["y"];
						if ((attribute != null) && (attribute2 != null))
						{
							child.Location = new Point(Convert.ToInt32(attribute.Value), Convert.ToInt32(attribute2.Value));
						}
					}
				}
			}
			return child;
		}

		public bool InvokeSave(UIBase pluginInstance)
		{
			return Save(pluginInstance);
		}

		public List<ILoadable> LoadableList(string interfaceName)
		{
			var list = new List<ILoadable>();
			if (_loadables.ContainsKey(interfaceName))
			{
				foreach (LoadedObject obj2 in _loadables[interfaceName])
				{
					if (obj2.Instance != null)
					{
						list.Add(obj2.Instance);
					}
				}
			}
			return list;
		}

		public void VerifySequenceHardwarePlugins(EventSequence sequence)
		{
			OutputPlugins.VerifyPlugIns(sequence);
			InputPlugins.VerifyPlugIns(sequence);
		}

		public string[] AudioDevices
		{
			get { return _audioDevices ?? (_audioDevices = fmod.GetSoundDeviceList()); }
		}

		public byte[,] Clipboard
		{
			get
			{
				byte[,] array = null;
				if (System.Windows.Forms.Clipboard.ContainsText())
				{
					try
					{
						if (_lastWindowsClipboardValue == System.Windows.Forms.Clipboard.GetText())
						{
							return Host.Clipboard;
						}
						string[] strArray = System.Windows.Forms.Clipboard.GetText()
						                          .Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
						int maxCols = 0;
						Array.ForEach(strArray, delegate(string s) { maxCols = Math.Max(s.Split(new[] {','}).Length, maxCols); });
						array = new byte[strArray.Length,maxCols];
						Array.Clear(array, 0, array.Length);
						for (int i = 0; i < strArray.Length; i++)
						{
							string[] strArray2 = strArray[i].Split(new[] {','});
							for (int j = 0; j < strArray2.Length; j++)
							{
								array[i, j] = byte.Parse(strArray2[j]);
							}
						}
					}
					catch
					{
						array = null;
					}
				}
				if (array == null)
				{
					return Host.Clipboard;
				}
				Host.Clipboard = array;
				_lastWindowsClipboardValue = System.Windows.Forms.Clipboard.GetText();
				return array;
			}
			set
			{
				if ((value != null) && (value.Length > 0))
				{
					Host.Clipboard = value;
					var builder = new StringBuilder();
					for (int i = 0; i < value.GetLength(0); i++)
					{
						for (int j = 0; j < value.GetLength(1); j++)
						{
							builder.AppendFormat("{0},", value[i, j]);
						}
						builder.Remove(builder.Length - 1, 1);
						builder.AppendLine();
					}
					System.Windows.Forms.Clipboard.SetText(builder.ToString());
				}
			}
		}

		public int ExecutingTimerCount
		{
			get { return _timerExecutor.ExecutingTimerCount; }
		}

		public string KnownFileTypesFilter
		{
			get { return _knownFileTypesFilter; }
		}

		public Preference2 UserPreferences
		{
			get { return _preferences; }
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var dialog = new AboutDialog();
			dialog.ShowDialog();
			dialog.Dispose();
		}

		private void AddInClickHandler(object sender, EventArgs e)
		{
			var item = (ToolStripMenuItem) sender;
			var tag = (LoadedObject) item.Tag;
			if (tag.Instance == null)
			{
				if (!InstantiateObject(tag))
				{
					return;
				}
				item.Checked = true;
			}
			EventSequence sequence = null;
			if (ActiveMdiChild != null && (base.ActiveMdiChild is IVixenMDI))
			{
				sequence = ((IVixenMDI) ActiveMdiChild).Sequence;
			}
			try
			{
				var addIn = (IAddIn) tag.Instance;
				if (addIn != null && (addIn.Execute(sequence) && (sequence != null)))
				{
					((IVixenMDI) ActiveMdiChild).Notify(Notification.SequenceChange, null);
				}
			}
			catch (Exception exception)
			{
				MessageBox.Show("Add-in error:\n\n" + exception.Message, Vendor.ProductName, MessageBoxButtons.OK,
				                MessageBoxIcon.Exclamation);
			}
			finally
			{
				if (tag.Instance != null && tag.Instance.DataLocationPreference == LoadableDataLocation.Sequence)
				{
					tag.Instance.Unloading();
					tag.Instance = null;
				}
			}
		}

		private void AddToFileHistory(string fileName)
		{
			string item = Path.GetFileName(fileName);
			_history.Remove(item);
			_history.Insert(0, item);
			if (_history.Count > HistoryMax)
			{
				_history.RemoveAt(_history.Count - 1);
			}
			FlushHistory();
			LoadHistory();
		}

		private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.Cascade);
		}

		private void ChangeSequenceName(IUIPlugIn pluginInstance, string newName)
		{
			if (!newName.EndsWith(pluginInstance.FileExtension))
			{
				newName = newName + pluginInstance.FileExtension;
			}
			pluginInstance.Sequence.Name = newName;
			saveToolStripMenuItem.Text = string.Format("Save ({0})", pluginInstance.Sequence.Name);
			((Form) pluginInstance).Text = pluginInstance.Sequence.Name;
		}

		private void channelDimmingCurvesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var activeMdiChild = ActiveMdiChild as IUIPlugIn;
			if (activeMdiChild != null)
			{
				var dialog = new DimmingCurveDialog(activeMdiChild.Sequence, null);
				if (dialog.ShowDialog() == DialogResult.OK)
				{
					activeMdiChild.IsDirty = true;
				}
			}
		}

		private DialogResult CheckDirty(IUIPlugIn pluginInstance)
		{
			var none = DialogResult.None;
			if (pluginInstance.IsDirty)
			{
				string str = pluginInstance.Sequence.Name ?? "this unnamed sequence";
				none = MessageBox.Show(string.Format("[{0}]\nSave changes to {1}?", pluginInstance.FileTypeDescription, str),
				                       Vendor.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				if (none == DialogResult.Yes)
				{
					Save((UIBase) pluginInstance);
				}
			}
			return none;
		}

		private void CheckForUpdates()
		{
			string updateServerURI = Vendor.UpdateFile;
			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			string updateRootPath = string.Format("{0}/{1}.{2}", Vendor.UpdateFile, version.Major, version.Minor);
			string path = Path.Combine(Paths.DataPath, "target.update");
			if (File.Exists(path))
			{
				string str4;
				var reader = new StreamReader(path);
				while ((str4 = reader.ReadLine()) != null)
				{
					string[] strArray = str4.Split(new[] {'='});
					string str5 = strArray[0];
					if (str5 != null)
					{
						if (str5 != "server")
						{
							if (str5 == "root")
							{
								goto Label_00B8;
							}
						}
						else
						{
							updateServerURI = strArray[1].Trim();
						}
					}
					goto Label_00C4;
					Label_00B8:
					updateRootPath = strArray[1].Trim();
					Label_00C4:
					;
				}
				reader.Close();
				reader.Dispose();
			}
			new AppUpdate.AppUpdate(updateServerURI, updateRootPath).ExecuteMigration();
		}

		private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CheckForUpdates();
		}

		private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Help.ShowHelp(this, helpProvider.HelpNamespace);
		}

		private void copyASequenceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var dialog = new CopySequenceDialog();
			dialog.ShowDialog();
			dialog.Dispose();
		}

		private void copyChannelColorsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var dialog = new CopyChannelColorsDialog();
			dialog.ShowDialog();
			dialog.Dispose();
		}

		private void copyPluginaddinDataToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var dialog = new CopyDataDialog2();
			dialog.ShowDialog();
			dialog.Dispose();
		}

		private void diagnosticsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new DiagnosticsDialog(_timers).ShowDialog();
		}


		private void Ensure(string path)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}

		private void FlushHistory()
		{
			XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(_preferences.XmlDoc.DocumentElement, "History");
			foreach (string str in _history)
			{
				Xml.SetNewValue(emptyNodeAlways, "Item", str);
			}
			_preferences.Flush();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			foreach (Form form in MdiChildren)
			{
				if ((form is IUIPlugIn) && (CheckDirty((IUIPlugIn) form) == DialogResult.Cancel))
				{
					e.Cancel = true;
					return;
				}
			}
			_host.StopBackgroundObjects();
			_host.BackgroundSequenceName = null;
			foreach (ToolStripItem item in addInsToolStripMenuItem.DropDownItems)
			{
				var loadedObject = item.Tag as LoadedObject;
				if (loadedObject != null)
				{
					LoadedObject tag = loadedObject;
					XmlNode loadableData = _loadableData.GetLoadableData(tag.InterfaceImplemented, item.Text);
					if ((tag.Instance != null) && (tag.Instance.DataLocationPreference == LoadableDataLocation.Application))
					{
						Xml.SetAttribute(loadableData, "enabled", bool.TrueString);
						tag.Instance.Unloading();
					}
					else
					{
						Xml.SetAttribute(loadableData, "enabled", bool.FalseString);
					}
				}
			}
			foreach (ToolStripItem item in triggersToolStripMenuItem.DropDownItems)
			{
				var loadedObject = item.Tag as LoadedObject;
				if (loadedObject != null)
				{
					LoadedObject obj3 = loadedObject;
					XmlNode node = _loadableData.GetLoadableData(obj3.InterfaceImplemented, item.Text);
					if (obj3.Instance != null)
					{
						Xml.SetAttribute(node, "enabled", bool.TrueString);
						obj3.Instance.Unloading();
					}
					else
					{
						Xml.SetAttribute(node, "enabled", bool.FalseString);
					}
				}
			}
			_preferences.Flush();
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			if (ActiveMdiChild != null)
			{
				if ((ActiveMdiChild is OutputPlugInUIBase) &&
				    (((OutputPlugInUIBase) ActiveMdiChild).ExecutionParent != null))
				{
					((OutputPlugInUIBase) ActiveMdiChild).ExecutionParent.Notify(Notification.KeyDown, e);
				}
				else
				{
					var activeMdiChild = ActiveMdiChild as IVixenMDI;
					if (activeMdiChild != null)
					{
						activeMdiChild.Notify(Notification.KeyDown, e);
					}
				}
			}
		}

		private void Form1_MdiChildActivate(object sender, EventArgs e)
		{
			if (ActiveMdiChild is IUIPlugIn)
			{
				saveToolStripMenuItem.Enabled = (ActiveMdiChild as IUIPlugIn).IsDirty;
				saveAsToolStripMenuItem.Enabled = true;
			}
			else
			{
				saveToolStripMenuItem.Enabled = false;
				saveAsToolStripMenuItem.Enabled = false;
			}
		}

		private bool FormContainsChild(Form parent, Form child)
		{
			foreach (Form form in parent.MdiChildren)
			{
				if (form == child)
				{
					return true;
				}
			}
			return false;
		}

		private bool GetNewName(IUIPlugIn pluginInstance)
		{
			saveFileDialog1.Filter = string.Format("{0}|*{1}", pluginInstance.FileTypeDescription, pluginInstance.FileExtension);
			saveFileDialog1.InitialDirectory = Paths.SequencePath;
			saveFileDialog1.FileName = string.Empty;
			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				ChangeSequenceName(pluginInstance, saveFileDialog1.FileName);
				return true;
			}
			return false;
		}

		private void HistoryItemClick(object sender, EventArgs e)
		{
			string text = ((ToolStripItem) sender).Text;
			string path = Path.Combine(Paths.SequencePath, text);
			if (File.Exists(path))
			{
				OpenSequence(path);
			}
			else
			{
				_history.Remove(text);
				FlushHistory();
				LoadHistory();
				MessageBox.Show("File does not exist.\nItem has been removed from the history.", Vendor.ProductName,
				                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		//ComponentResourceManager manager = new ComponentResourceManager(typeof(Form1));
		//this.toolStripStatusLabelMusic.Image = (Image)manager.GetObject("toolStripStatusLabelMusic.Image");
		//base.Icon = (Icon)manager.GetObject("$this.Icon");


		private bool InstantiateObject(LoadedObject loadedObject)
		{
			loadedObject.Instance = (ILoadable) Activator.CreateInstance(loadedObject.ObjectType);
			try
			{
				if ((((loadedObject.Instance.DataLocationPreference != LoadableDataLocation.Sequence) || ActiveMdiChild == null) ||
				     (!(ActiveMdiChild is IVixenMDI))) || (((IVixenMDI) ActiveMdiChild).Sequence == null))
				{
					var activeMdiChild = (IVixenMDI) ActiveMdiChild;
					if (activeMdiChild != null)
					{
						loadedObject.Instance.Loading(
							activeMdiChild.Sequence.LoadableData.GetLoadableData(loadedObject.InterfaceImplemented,
							                                                     loadedObject.Instance.Name));
					}
				}
				else
				{
					loadedObject.Instance.Loading(_loadableData.GetLoadableData(loadedObject.InterfaceImplemented,
					                                                            loadedObject.Instance.Name));
				}
			}
			catch (Exception exception)
			{
				MessageBox.Show(
					string.Format("Error when initializing loadable {0}:\n\n{1}", loadedObject.Instance.Name, exception.Message),
					Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}
			return true;
		}

		private void loadableMenuItem_CheckClick(object sender, EventArgs e)
		{
			var item = (ToolStripMenuItem) sender;
			var tag = (LoadedObject) item.Tag;
			if (item.Checked)
			{
				InstantiateObject(tag);
			}
			else
			{
				try
				{
					tag.Instance.Unloading();
				}
				catch (Exception exception)
				{
					MessageBox.Show(
						string.Format("{0} caused the following error while unloading:\n\n{1}", tag.Instance.Name, exception.Message),
						Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				tag.Instance = null;
			}
		}

		private void loadableToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
		{
			var item = (ToolStripMenuItem) sender;
			foreach (ToolStripItem item2 in item.DropDownItems)
			{
				var loadedObject = item2.Tag as LoadedObject;
				if (loadedObject != null)
				{
					LoadedObject tag = loadedObject;
					((ToolStripMenuItem) item2).Checked = tag.Instance != null;
				}
			}
		}

		private void LoadHistory()
		{
			recentToolStripMenuItem.DropDownItems.Clear();
			_history = new List<string>();
			XmlNodeList list = _preferences.XmlDoc.SelectNodes("//User/History/*");
			if (list != null)
			{
				foreach (XmlNode node in list)
				{
					_history.Add(node.InnerText);
					recentToolStripMenuItem.DropDownItems.Add(node.InnerText, null, _historyItemClick);
				}
			}
			recentToolStripMenuItem.Enabled = recentToolStripMenuItem.DropDownItems.Count > 0;
		}

		private List<LoadedObject> LoadObjects(string directory, string interfaceName)
		{
			var list = new List<LoadedObject>();
			if (Directory.Exists(directory))
			{
				foreach (string str in Directory.GetFiles(directory, "*.dll"))
				{
					Exception exception;
					try
					{
						Assembly assembly = Assembly.LoadFile(str);
						foreach (Type type in assembly.GetExportedTypes())
						{
							foreach (Type type2 in type.GetInterfaces())
							{
								if (type2.Name == interfaceName)
								{
									try
									{
										list.Add(new LoadedObject(type, interfaceName));
									}
									catch (Exception exception1)
									{
										exception = exception1;
										MessageBox.Show(
											string.Format("Error when loading object from {0}:\n{1}", Path.GetFileNameWithoutExtension(str),
											              exception.Message), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
									}
								}
							}
						}
					}
					catch (BadImageFormatException)
					{
					}
					catch (Exception exception3)
					{
						exception = exception3;
						MessageBox.Show(string.Format("{0}:\n{1}", Path.GetFileName(str), exception.Message));
					}
				}
				_loadables[interfaceName] = list;
			}
			return list;
		}

		private void LoadUIPlugins()
		{
			if (Directory.Exists(Paths.UIPluginPath))
			{
				foreach (string str in Directory.GetFiles(Paths.UIPluginPath, "*.dll"))
				{
					Exception exception;
					try
					{
						Assembly assembly = Assembly.LoadFile(str);
						foreach (Type type in assembly.GetExportedTypes())
						{
							foreach (Type type2 in type.GetInterfaces())
							{
								if (type2.Name == "IUIPlugIn")
								{
									try
									{
										Debug.WriteLine("Type: " + type.Name + " Type2: " + type2.Name);
										var inputPlugin = (IUIPlugIn) Activator.CreateInstance(type);
										if (!RegisterFileType(inputPlugin.FileExtension, inputPlugin))
										{
											MessageBox.Show(
												string.Format("Could not register UI plugin {0}.\nFile type is already handled.", inputPlugin.Name),
												Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
										}
									}
									catch (Exception exception1)
									{
										exception = exception1;
										MessageBox.Show(
											string.Format("Error when loading UI plugin from {0}:\n{1}", Path.GetFileNameWithoutExtension(str),
											              exception.StackTrace), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
									}
								}
							}
						}
					}
					catch (BadImageFormatException)
					{
					}
					catch (Exception exception3)
					{
						exception = exception3;
						MessageBox.Show(string.Format("{0}:\n{1}", Path.GetFileName(str), exception.Message));
					}
				}
				foreach (IUIPlugIn in2 in _registeredFileTypes.Values)
				{
					ToolStripItem item = newLightingProgramToolStripMenuItem.DropDownItems.Add(in2.FileTypeDescription);
					item.Tag = in2;
					item.Click += _newMenuItemClick;
				}
				var builder = new StringBuilder();
				foreach (IUIPlugIn in2 in _registeredFileTypes.Values)
				{
					builder.AppendFormat("|{0}|*{1}", in2.FileTypeDescription, in2.FileExtension);
				}
				_knownFileTypesFilter = builder.ToString().Remove(0, 1);
			}
		}

		//private void LoadVendorData() {
		//    string path = Path.Combine(Paths.BinaryPath, "Vendor.dll");
		//    if (File.Exists(path)) {
		//        ResourceManager manager = new ResourceManager("Vixen.Properties.Resources", Assembly.LoadFile(path));
		//        Vendor.ProductName = manager.GetString("VendorProductName");
		//        Vendor.Name = manager.GetString("VendorName");
		//        Vendor.SequenceExtension = manager.GetString("SequenceFileExtension");
		//        Vendor.ProgramExtension = manager.GetString("ProgramFileExtension");
		//        Vendor.DataExtension = manager.GetString("DataFileExtension");
		//        Vendor.UpdateRoot = manager.GetString("UpdateRoot");
		//        Vendor.SupportURL = manager.GetString("SupportURL");
		//        manager.ReleaseAllResources();
		//    }
		//}

		private void PreferencesPreferenceChange(string preferenceName)
		{
			switch (preferenceName)
			{
				case "TimerCheckFrequency":
					scheduleTimer.Interval = _preferences.GetInteger("TimerCheckFrequency")*0x3e8;
					break;

				case "EnableBackgroundSequence":
					if (!_preferences.GetBoolean("EnableBackgroundSequence"))
					{
						_host.StopBackgroundSequence();
						break;
					}
					_host.BackgroundSequenceName = _preferences.GetString("BackgroundSequence");
					_host.StartBackgroundSequence();
					break;

				case "EnableBackgroundMusic":
					if (!_preferences.GetBoolean("EnableBackgroundMusic"))
					{
						_host.StopBackgroundMusic();
						break;
					}
					_host.StartBackgroundMusic();
					break;

				case "EventPeriod":
					if (_preferences.GetInteger("EventPeriod") < 0x19)
					{
						_preferences.SetInteger("EventPeriod", 0x19);
						MessageBox.Show(
							"The event period length cannot be less than 25 milliseconds.\nThe length has been reset to 25 milliseconds.",
							Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
					break;

				case "BackgroundMusicDelay":
					if (_preferences.GetInteger("BackgroundMusicDelay") < 1)
					{
						MessageBox.Show("Delay cannot be less than 1 second.\nResetting to 1 second", Vendor.ProductName,
						                MessageBoxButtons.OK, MessageBoxIcon.Hand);
						_preferences.SetInteger("BackgroundMusicDelay", 1);
					}
					break;

				case "BackgroundSequenceDelay":
					if (_preferences.GetInteger("BackgroundSequenceDelay") < 1)
					{
						MessageBox.Show("Delay cannot be less than 1 second.\nResetting to 1 second", Vendor.ProductName,
						                MessageBoxButtons.OK, MessageBoxIcon.Hand);
						_preferences.SetInteger("BackgroundSequenceDelay", 1);
					}
					break;

				case "ShutdownTime":
					SetShutdownTime(_preferences.GetString("ShutdownTime"));
					break;
			}
		}

		private void manageProgramsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var dialog = new ProgramManagerDialog();
			dialog.ShowDialog();
			dialog.Dispose();
		}

		private void manageToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var dialog = new ProfileManagerDialog(null);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				NotifyAll(Notification.ProfileChange);
			}
			dialog.Dispose();
		}

		private void musicPlayerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_host.MusicPlayer.ShowDialog();
		}

		private void NewMenuItemClick(object sender, EventArgs e)
		{
			var item = (ToolStripItem) sender;
			if (!(item.Tag is IUIPlugIn))
			{
				return;
			}
			var tag = (IUIPlugIn) item.Tag;
			tag = (IUIPlugIn) Activator.CreateInstance(tag.GetType());
			tag.Sequence = null;
			if (_preferences.GetBoolean("WizardForNewSequences"))
			{
				EventSequence resultSequence = null;
				switch (tag.RunWizard(ref resultSequence))
				{
					case DialogResult.None:
						MessageBox.Show("No wizard available.\n\nA blank sequence will be created for you.", Vendor.ProductName,
						                MessageBoxButtons.OK, MessageBoxIcon.Hand);
						tag.Sequence = tag.New();
						goto Label_00DF;

					case DialogResult.OK:
						tag.Sequence = tag.New(resultSequence);
						if (!SaveAs((UIBase) tag))
						{
							DialogResult = DialogResult.None;
						}
						goto Label_00DF;

					case DialogResult.Cancel:
						return;
				}
			}
			else
			{
				tag.Sequence = tag.New();
			}
			Label_00DF:
			if (tag.Sequence != null)
			{
				var uiBase = tag as UIBase;
				if (uiBase != null)
				{
					uiBase.DirtyChanged += plugin_DirtyChanged;
				}
				tag.MdiParent = this;
				tag.Show();
			}
		}

		private void NotifyAll(Notification notification)
		{
			foreach (Form form in MdiChildren)
			{
				var vixenMdi = form as IVixenMDI;
				if (vixenMdi != null)
				{
					vixenMdi.Notify(notification, null);
				}
			}
		}

		private void onlineSupportForumToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var process = new Process {StartInfo = {FileName = Vendor.SupportURL, UseShellExecute = true}};
			process.Start();
		}

		private void openALightingProgramToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int num = 0;
			int num2 = 1;
			string str = _preferences.GetString("PreferredSequenceType");
			foreach (IUIPlugIn @in in _registeredFileTypes.Values)
			{
				if (str == @in.FileExtension)
				{
					num = num2;
					break;
				}
				num2++;
			}
			openFileDialog1.Filter = _knownFileTypesFilter;
			openFileDialog1.InitialDirectory = Paths.SequencePath;
			openFileDialog1.FileName = string.Empty;
			openFileDialog1.FilterIndex = num;
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				Cursor = Cursors.WaitCursor;
				try
				{
					OpenSequence(openFileDialog1.FileName);
				}
				finally
				{
					Cursor = Cursors.Default;
				}
			}
		}

		public void OpenSequence(string fileName)
		{
			IUIPlugIn @in;
			new XmlDocument();
			string extension = Path.GetExtension(fileName);
			if (extension != null && _registeredFileTypes.TryGetValue(extension.ToLower(), out @in))
			{
				AddToFileHistory(fileName);
				@in = (IUIPlugIn) Activator.CreateInstance(@in.GetType());
				@in.Sequence = @in.Open(fileName);
				var uiBase = @in as UIBase;
				if (uiBase != null)
				{
					uiBase.DirtyChanged += plugin_DirtyChanged;
				}
				@in.MdiParent = this;
				@in.Show();
			}
			else
			{
				MessageBox.Show("There is no known editor for that file type.", Vendor.ProductName, MessageBoxButtons.OK,
				                MessageBoxIcon.Hand);
			}
		}

		private void plugin_DirtyChanged(object sender, EventArgs e)
		{
			var uiPlugIn = sender as IUIPlugIn;
			if (uiPlugIn != null)
			{
				saveToolStripMenuItem.Enabled = uiPlugIn.IsDirty;
			}
		}

		private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var array = new IUIPlugIn[_registeredFileTypes.Values.Count];
			_registeredFileTypes.Values.CopyTo(array, 0);
			var dialog = new PreferencesDialog(array);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				_preferences.Reload();
				NotifyAll(Notification.PreferenceChange);
			}
			dialog.Dispose();
		}

		private void programsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
		{
			manageProgramsToolStripMenuItem.Enabled = ((ISystem) Interfaces.Available["ISystem"]).ExecutingTimerCount == 0;
		}

		private void programToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
		{
			var @in = ActiveMdiChild as IUIPlugIn;
			if (@in != null)
			{
				IUIPlugIn activeMdiChild = @in;
				saveToolStripMenuItem.Text = !string.IsNullOrEmpty(activeMdiChild.Sequence.Name)
					                             ? string.Format("Save ({0})", activeMdiChild.Sequence.Name)
					                             : "Save";
				channelDimmingCurvesToolStripMenuItem.Enabled = true;
			}
			else
			{
				saveToolStripMenuItem.Text = "Save";
				channelDimmingCurvesToolStripMenuItem.Enabled = false;
			}
		}

		private bool RegisterFileType(string fileExtension, IUIPlugIn inputPlugin)
		{
			IUIPlugIn @in;
			fileExtension = fileExtension.ToLower();
			if (_registeredFileTypes.TryGetValue(fileExtension, out @in))
			{
				return false;
			}
			_registeredFileTypes[fileExtension] = inputPlugin;
			return true;
		}

		private void restartCurrentTimerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_timers.TimersDisabled)
			{
				MessageBox.Show("Schedule is currently disabled.", Vendor.ProductName, MessageBoxButtons.OK,
				                MessageBoxIcon.Exclamation);
			}
			else
			{
				SetTimerTraceFlag();
				List<Timer> list = _timers.CurrentlyEffectiveTimers();
				if (list.Count == 0)
				{
					MessageBox.Show("There is nothing scheduled to execute at this time.", Vendor.ProductName, MessageBoxButtons.OK,
					                MessageBoxIcon.Asterisk);
				}
				else
				{
					foreach (Timer timer in list)
					{
						_timerExecutor.SpawnExecutorFor(timer);
					}
					if (_timerExecutor.ExecutingTimerCount == 0)
					{
						MessageBox.Show(
							"Timers were not executed.\n\nThe scheduled items may be longer than their remaining time or there may have been an error.",
							Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					}
				}
			}
		}

		private bool Save(UIBase pluginInstance)
		{
			if (pluginInstance == null)
			{
				return false;
			}
			IUIPlugIn @in = pluginInstance;
			if ((@in.IsDirty && string.IsNullOrEmpty(@in.Sequence.Name)) &&
			    !GetNewName(pluginInstance))
			{
				return false;
			}
			UpdateHistoryImages(@in.Sequence.FileName);
			@in.SaveTo(@in.Sequence.FileName);
			AddToFileHistory(@in.Sequence.FileName);
			if (_preferences.GetBoolean("ShowSaveConfirmation"))
			{
				MessageBox.Show("Sequence saved", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			return true;
		}

		private bool SaveAs(UIBase pluginInstance)
		{
			return (GetNewName(pluginInstance) && Save(pluginInstance));
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveAs((UIBase) ActiveMdiChild);
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Save((UIBase) ActiveMdiChild);
		}

		private void setBackgroundSequenceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var dialog = new BackgroundSequenceDialog(_preferences.GetString("BackgroundSequence"), Paths.SequencePath);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				_preferences.SetString("BackgroundSequence", dialog.BackgroundSequenceFileName);
				_host.BackgroundSequenceName = dialog.BackgroundSequenceFileName;
			}
			dialog.Dispose();
		}

		private void SetDataPath()
		{
			if (File.Exists("redirect.data"))
			{
				Paths.DataPath = Path.Combine(Paths.BinaryPath, "Data");
			}
		}

		private void SetShutdownTime(string time)
		{
			if (time == string.Empty)
			{
				shutdownTimer.Stop();
			}
			else if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "shutdown.exe")))
			{
				_shutdownAt = DateTime.Parse(time);
				shutdownTimer.Start();
			}
		}

		private void setSoundDeviceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var dialog = new SoundDeviceDialog(_preferences);
			dialog.ShowDialog();
			dialog.Dispose();
		}

		private void SetTimerTraceFlag()
		{
			if (((Host.GetDebugValue("TraceTimers") == bool.TrueString) &&
			     (DateTime.Now >= DateTime.Parse(Host.GetDebugValue("TraceStart")))) &&
			    (DateTime.Now <= DateTime.Parse(Host.GetDebugValue("TraceEnd"))))
			{
				Host.SetDebugValue("TimerTrace");
			}
			else
			{
				Host.ResetDebugValue("TimerTrace");
			}
		}

		private void SetVendorData()
		{
			Text = Vendor.ProductName;
		}

		private void shutdownTimer_Tick(object sender, EventArgs e)
		{
			if ((DateTime.Now.Hour == _shutdownAt.Hour) && (DateTime.Now.Minute == _shutdownAt.Minute))
			{
				shutdownTimer.Stop();
				Process.Start("shutdown", string.Format("/s /d P:4:1 /c \"Automatic shutdown by {0}\"", Vendor.ProductName));
				Thread.Sleep(0x3e8);
				new ShutdownDialog().Show();
			}
		}

		private void StartAddins()
		{
			List<LoadedObject> list = LoadObjects(Paths.AddinPath, "IAddIn");
			addInsToolStripMenuItem.Visible = list.Count > 0;
			foreach (LoadedObject obj2 in list)
			{
				var @in = (IAddIn) Activator.CreateInstance(obj2.ObjectType);
				var item = new CheckableToolStripMenuItem();
				item.Click += AddInClickHandler;
				item.Text = @in.Name;
				item.Tag = obj2;
				item.CheckClick += loadableMenuItem_CheckClick;
				addInsToolStripMenuItem.DropDownItems.Add(item);
				if (
					_loadableData.RootNode.SelectSingleNode(string.Format("IAddInData/IAddIn[@name=\"{0}\" and @enabled=\"{1}\"]",
					                                                      @in.Name, bool.TrueString)) != null)
				{
					InstantiateObject(obj2);
					item.Checked = true;
				}
			}
		}

		private void StartTriggers()
		{
			List<LoadedObject> list = LoadObjects(Paths.TriggerPluginPath, "ITriggerPlugin");
			triggersToolStripMenuItem.Visible = list.Count > 0;
			foreach (LoadedObject obj2 in list)
			{
				var plugin = (ITriggerPlugin) Activator.CreateInstance(obj2.ObjectType);
				var item = new CheckableToolStripMenuItem();
				item.Click += TriggerClickHandler;
				item.Text = plugin.Name;
				item.Tag = obj2;
				item.CheckClick += loadableMenuItem_CheckClick;
				triggersToolStripMenuItem.DropDownItems.Add(item);
				if (
					_loadableData.RootNode.SelectSingleNode(
						string.Format("ITriggerPluginData/ITriggerPlugin[@name=\"{0}\" and @enabled=\"{1}\"]", plugin.Name,
						              bool.TrueString)) != null)
				{
					InstantiateObject(obj2);
					item.Checked = true;
				}
			}
		}

		private void tileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.TileHorizontal);
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			SetTimerTraceFlag();
			foreach (Timer timer in _timers.StartingTimers())
			{
				_timerExecutor.SpawnExecutorFor(timer);
			}
		}

		private void timersToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var dialog = new ScheduleDialog(_timers);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				_timers = dialog.Timers;
				_timers.TimersDisabled = dialog.ScheduleDisabled;
				XmlDocument contextNode = Xml.CreateXmlDocument();
				_timers.SaveToXml(contextNode);
				contextNode.Save(_timersPath);
				scheduleTimer.Enabled = !_timers.TimersDisabled;
			}
			dialog.Dispose();
		}

		private void TriggerClickHandler(object sender, EventArgs e)
		{
			var item = (ToolStripMenuItem) sender;
			var tag = (LoadedObject) item.Tag;
			if (tag.Instance == null)
			{
				if (!InstantiateObject(tag))
				{
					return;
				}
				item.Checked = true;
			}
			try
			{
				((ITriggerPlugin) tag.Instance).Setup();
			}
			catch (Exception exception)
			{
				MessageBox.Show("Trigger error:\n\n" + exception.Message, Vendor.ProductName, MessageBoxButtons.OK,
				                MessageBoxIcon.Exclamation);
			}
		}

		private void turnOnQueryServerToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		private void UpdateHistoryImages(string baseFilePath)
		{
			if (File.Exists(baseFilePath))
			{
				int integer = _preferences.GetInteger("HistoryImages");
				if (integer != 0)
				{
					string[] files = Directory.GetFiles(Paths.SequencePath, Path.GetFileName(baseFilePath) + ".bak*");
					int num2 = files.Length + 1;
					if (files.Length >= integer)
					{
						num2--;
						for (int i = 2; i <= integer; i++)
						{
							File.Copy(string.Format("{0}.bak{1}", baseFilePath, i), string.Format("{0}.bak{1}", baseFilePath, i - 1), true);
						}
					}
					File.Copy(baseFilePath, string.Format("{0}.bak{1}", baseFilePath, num2), true);
				}
			}
		}

		private void viewRegisteredResponsesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_iTriggerImpl.ShowRegistrations();
		}

		private void visualChannelLayoutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			IExecutable executableObject;
			if (ActiveMdiChild != null && ActiveMdiChild is IVixenMDI && ((IVixenMDI) ActiveMdiChild).Sequence != null)
			{
				executableObject = ((IVixenMDI) ActiveMdiChild).Sequence;
			}
			else
			{
				string str = _preferences.GetString("DefaultProfile");
				executableObject = (str.Length == 0) ? null : (new Profile(Path.Combine(Paths.ProfilePath, str + ".pro")));
			}
			if (executableObject == null)
			{
				MessageBox.Show("There is no sequence open, nor is there a default profile set.\nYou have no channels to layout.",
				                Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else
			{
				var dialog = new ChannelLayoutDialog();
				dialog.ShowDialog();
				dialog.Dispose();
			}
		}

		private delegate Form InstantiateFormDelegate(ConstructorInfo constructorInfo, params object[] parameters);
	}
}