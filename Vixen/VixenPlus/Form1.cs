using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
	internal partial class Form1 : Form, ISystem
	{
		private readonly EventHandler m_historyItemClick;
		private readonly Host m_host;
		private readonly ITriggerImpl m_iTriggerImpl;
		private readonly LoadableData m_loadableData;
		private readonly Dictionary<string, List<LoadedObject>> m_loadables;
		private readonly EventHandler m_newMenuItemClick;
		private readonly Preference2 m_preferences;
		private readonly Dictionary<string, IUIPlugIn> m_registeredFileTypes;
		private readonly TimerExecutor m_timerExecutor;
		private readonly string m_timersPath;
		private string[] m_audioDevices;
		private List<string> m_history;
		private const int m_historyMax = 7;
		private string m_knownFileTypesFilter;
		private string m_lastWindowsClipboardValue = "";
		private DateTime m_shutdownAt;
		private Timers m_timers;

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
			m_registeredFileTypes = new Dictionary<string, IUIPlugIn>();
			m_timersPath = Path.Combine(Paths.DataPath, "timers");
			helpProvider.HelpNamespace = Path.Combine(Paths.BinaryPath, Vendor.ProductName + ".chm");
			string path = Path.Combine(Paths.BinaryPath, "prepare.exe");
			if (File.Exists(path))
			{
				Process.Start(path).WaitForExit();
				File.Delete(path);
			}
			m_preferences = Preference2.GetInstance();
			m_preferences.PreferenceChange += m_preferences_PreferenceChange;
			m_host = new Host(this);
			m_loadables = new Dictionary<string, List<LoadedObject>>();
			Interfaces.Available["ISystem"] = this;
			Interfaces.Available["IExecution"] = new IExecutionImpl(m_host);
			Interfaces.Available["ITrigger"] = m_iTriggerImpl = new ITriggerImpl(m_host);
			m_newMenuItemClick = NewMenuItemClick;
			m_historyItemClick = HistoryItemClick;
			LoadHistory();
			m_loadableData = new LoadableData();
			m_loadableData.LoadFromXml(m_preferences.XmlDoc.DocumentElement);
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
			scheduleTimer.Interval = m_preferences.GetInteger("TimerCheckFrequency")*1000;
			m_timers = new Timers();
			if (File.Exists(m_timersPath))
			{
				m_timers.LoadFromXml(Xml.LoadDocument(m_timersPath));
			}
			m_timerExecutor = new TimerExecutor(m_host);
			scheduleTimer.Enabled = !m_timers.TimersDisabled;
			if (m_preferences.GetBoolean("EnableBackgroundSequence"))
			{
				m_host.BackgroundSequenceName = m_preferences.GetString("BackgroundSequence");
			}
			m_host.StartBackgroundObjects();
			SetShutdownTime(m_preferences.GetString("ShutdownTime"));
			splash.Hide();
			splash.Dispose();
			if (!(list.Contains("no_update") || File.Exists(Path.Combine(Paths.DataPath, "no.update"))))
			{
				CheckForUpdates();
			}
		}

		public int GetExecutingTimerExecutionContextHandle(int executingTimerIndex)
		{
			return m_timerExecutor.GetExecutingTimerExecutionContextHandle(executingTimerIndex);
		}

		public Form InstantiateForm(ConstructorInfo constructorInfo, params object[] parameters)
		{
			if (base.InvokeRequired)
			{
				return (Form) base.Invoke(new InstantiateFormDelegate(InstantiateForm), new object[] {constructorInfo, parameters});
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
				string str = executable.Key.ToString();
				XmlNode node2 = null;
				XmlNode node = ((XmlNode) Host.Communication["SetupNode_" + str]).SelectSingleNode("DialogPositions");
				object obj2;
				if (Host.Communication.TryGetValue("KeyInterceptor_" + str, out obj2))
				{
					base2.ExecutionParent = (VixenMDI) obj2;
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
				if ((node2 != null) && m_preferences.GetBoolean("SavePlugInDialogPositions"))
				{
					XmlAttribute attribute = node2.Attributes["x"];
					XmlAttribute attribute2 = node2.Attributes["y"];
					if ((attribute != null) && (attribute2 != null))
					{
						child.Location = new Point(Convert.ToInt32(attribute.Value), Convert.ToInt32(attribute2.Value));
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
			if (m_loadables.ContainsKey(interfaceName))
			{
				foreach (LoadedObject obj2 in m_loadables[interfaceName])
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
			get { return m_audioDevices ?? (m_audioDevices = fmod.GetSoundDeviceList()); }
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
						if (m_lastWindowsClipboardValue == System.Windows.Forms.Clipboard.GetText())
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
				m_lastWindowsClipboardValue = System.Windows.Forms.Clipboard.GetText();
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
			get { return m_timerExecutor.ExecutingTimerCount; }
		}

		public string KnownFileTypesFilter
		{
			get { return m_knownFileTypesFilter; }
		}

		public Preference2 UserPreferences
		{
			get { return m_preferences; }
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var dialog = new AboutDialog();
			dialog.ShowDialog();
			dialog.Dispose();
		}

		private void AddInClickHandler(object Sender, EventArgs e)
		{
			var item = (ToolStripMenuItem) Sender;
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
			if ((base.ActiveMdiChild != null) && (base.ActiveMdiChild is VixenMDI))
			{
				sequence = ((VixenMDI) base.ActiveMdiChild).Sequence;
			}
			try
			{
				if (((IAddIn) tag.Instance).Execute(sequence) && (sequence != null))
				{
					((VixenMDI) base.ActiveMdiChild).Notify(Notification.SequenceChange, null);
				}
			}
			catch (Exception exception)
			{
				MessageBox.Show("Add-in error:\n\n" + exception.Message, Vendor.ProductName, MessageBoxButtons.OK,
				                MessageBoxIcon.Exclamation);
			}
			finally
			{
				if (tag.Instance.DataLocationPreference == LoadableDataLocation.Sequence)
				{
					tag.Instance.Unloading();
					tag.Instance = null;
				}
			}
		}

		private void AddToFileHistory(string fileName)
		{
			string item = Path.GetFileName(fileName);
			m_history.Remove(item);
			m_history.Insert(0, item);
			if (m_history.Count > m_historyMax)
			{
				m_history.RemoveAt(m_history.Count - 1);
			}
			FlushHistory();
			LoadHistory();
		}

		private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			base.LayoutMdi(MdiLayout.Cascade);
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
			var activeMdiChild = base.ActiveMdiChild as IUIPlugIn;
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
						if (!(str5 == "server"))
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
			new DiagnosticsDialog(m_timers).ShowDialog();
		}


		private void Ensure(string path)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}

		private int FindMdiChildIndex(Form childForm)
		{
			for (int i = 0; i < base.MdiChildren.Length; i++)
			{
				if (childForm == base.MdiChildren[i])
				{
					return i;
				}
			}
			return -1;
		}

		private Form FindUIBase()
		{
			if (base.ActiveMdiChild is UIBase)
			{
				return base.ActiveMdiChild;
			}
			for (int i = FindMdiChildIndex(base.ActiveMdiChild) - 1; i > 0; i--)
			{
				if (base.MdiChildren[i] is UIBase)
				{
					return base.MdiChildren[i];
				}
			}
			return null;
		}

		private void FlushHistory()
		{
			XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(m_preferences.XmlDoc.DocumentElement, "History");
			foreach (string str in m_history)
			{
				Xml.SetNewValue(emptyNodeAlways, "Item", str);
			}
			m_preferences.Flush();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			foreach (Form form in base.MdiChildren)
			{
				if ((form is IUIPlugIn) && (CheckDirty((IUIPlugIn) form) == DialogResult.Cancel))
				{
					e.Cancel = true;
					return;
				}
			}
			m_host.StopBackgroundObjects();
			m_host.BackgroundSequenceName = null;
			foreach (ToolStripItem item in addInsToolStripMenuItem.DropDownItems)
			{
				if (item.Tag is LoadedObject)
				{
					var tag = (LoadedObject) item.Tag;
					XmlNode loadableData = m_loadableData.GetLoadableData(tag.InterfaceImplemented, item.Text);
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
				if (item.Tag is LoadedObject)
				{
					var obj3 = (LoadedObject) item.Tag;
					XmlNode node = m_loadableData.GetLoadableData(obj3.InterfaceImplemented, item.Text);
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
			m_preferences.Flush();
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			if (base.ActiveMdiChild != null)
			{
				if ((base.ActiveMdiChild is OutputPlugInUIBase) &&
				    (((OutputPlugInUIBase) base.ActiveMdiChild).ExecutionParent != null))
				{
					((OutputPlugInUIBase) base.ActiveMdiChild).ExecutionParent.Notify(Notification.KeyDown, e);
				}
				else if (base.ActiveMdiChild is VixenMDI)
				{
					((VixenMDI) base.ActiveMdiChild).Notify(Notification.KeyDown, e);
				}
			}
		}

		private void Form1_MdiChildActivate(object sender, EventArgs e)
		{
			if (base.ActiveMdiChild is IUIPlugIn)
			{
				saveToolStripMenuItem.Enabled = (base.ActiveMdiChild as IUIPlugIn).IsDirty;
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
				m_history.Remove(text);
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
				if ((((loadedObject.Instance.DataLocationPreference == LoadableDataLocation.Sequence) &&
				      (base.ActiveMdiChild != null)) && (base.ActiveMdiChild is VixenMDI)) &&
				    (((VixenMDI) base.ActiveMdiChild).Sequence != null))
				{
					loadedObject.Instance.Loading(
						((VixenMDI) base.ActiveMdiChild).Sequence.LoadableData.GetLoadableData(loadedObject.InterfaceImplemented,
						                                                                       loadedObject.Instance.Name));
				}
				else
				{
					loadedObject.Instance.Loading(m_loadableData.GetLoadableData(loadedObject.InterfaceImplemented,
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
				if (item2.Tag is LoadedObject)
				{
					var tag = (LoadedObject) item2.Tag;
					((ToolStripMenuItem) item2).Checked = tag.Instance != null;
				}
			}
		}

		private void LoadHistory()
		{
			recentToolStripMenuItem.DropDownItems.Clear();
			m_history = new List<string>();
			XmlNodeList list = m_preferences.XmlDoc.SelectNodes("//User/History/*");
			foreach (XmlNode node in list)
			{
				m_history.Add(node.InnerText);
				recentToolStripMenuItem.DropDownItems.Add(node.InnerText, null, m_historyItemClick);
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
				m_loadables[interfaceName] = list;
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
				foreach (IUIPlugIn in2 in m_registeredFileTypes.Values)
				{
					ToolStripItem item = newLightingProgramToolStripMenuItem.DropDownItems.Add(in2.FileTypeDescription);
					item.Tag = in2;
					item.Click += m_newMenuItemClick;
				}
				var builder = new StringBuilder();
				foreach (IUIPlugIn in2 in m_registeredFileTypes.Values)
				{
					builder.AppendFormat("|{0}|*{1}", in2.FileTypeDescription, in2.FileExtension);
				}
				m_knownFileTypesFilter = builder.ToString().Remove(0, 1);
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

		private void m_preferences_PreferenceChange(string preferenceName)
		{
			switch (preferenceName)
			{
				case "TimerCheckFrequency":
					scheduleTimer.Interval = m_preferences.GetInteger("TimerCheckFrequency")*0x3e8;
					break;

				case "EnableBackgroundSequence":
					if (!m_preferences.GetBoolean("EnableBackgroundSequence"))
					{
						m_host.StopBackgroundSequence();
						break;
					}
					m_host.BackgroundSequenceName = m_preferences.GetString("BackgroundSequence");
					m_host.StartBackgroundSequence();
					break;

				case "EnableBackgroundMusic":
					if (!m_preferences.GetBoolean("EnableBackgroundMusic"))
					{
						m_host.StopBackgroundMusic();
						break;
					}
					m_host.StartBackgroundMusic();
					break;

				case "EventPeriod":
					if (m_preferences.GetInteger("EventPeriod") < 0x19)
					{
						m_preferences.SetInteger("EventPeriod", 0x19);
						MessageBox.Show(
							"The event period length cannot be less than 25 milliseconds.\nThe length has been reset to 25 milliseconds.",
							Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
					break;

				case "BackgroundMusicDelay":
					if (m_preferences.GetInteger("BackgroundMusicDelay") < 1)
					{
						MessageBox.Show("Delay cannot be less than 1 second.\nResetting to 1 second", Vendor.ProductName,
						                MessageBoxButtons.OK, MessageBoxIcon.Hand);
						m_preferences.SetInteger("BackgroundMusicDelay", 1);
					}
					break;

				case "BackgroundSequenceDelay":
					if (m_preferences.GetInteger("BackgroundSequenceDelay") < 1)
					{
						MessageBox.Show("Delay cannot be less than 1 second.\nResetting to 1 second", Vendor.ProductName,
						                MessageBoxButtons.OK, MessageBoxIcon.Hand);
						m_preferences.SetInteger("BackgroundSequenceDelay", 1);
					}
					break;

				case "ShutdownTime":
					SetShutdownTime(m_preferences.GetString("ShutdownTime"));
					break;
			}
		}

		private void manageProgramsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var dialog = new ProgramManagerDialog(m_host);
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
			m_host.MusicPlayer.ShowDialog();
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
			if (m_preferences.GetBoolean("WizardForNewSequences"))
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
							base.DialogResult = DialogResult.None;
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
				(tag as UIBase).DirtyChanged += plugin_DirtyChanged;
				tag.MdiParent = this;
				tag.Show();
			}
		}

		private void NotifyAll(Notification notification)
		{
			foreach (Form form in base.MdiChildren)
			{
				if (form is VixenMDI)
				{
					((VixenMDI) form).Notify(notification, null);
				}
			}
		}

		private void onlineSupportForumToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var process = new Process();
			process.StartInfo.FileName = Vendor.SupportURL;
			process.StartInfo.UseShellExecute = true;
			process.Start();
		}

		private void openALightingProgramToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int num = 0;
			int num2 = 1;
			string str = m_preferences.GetString("PreferredSequenceType");
			foreach (IUIPlugIn @in in m_registeredFileTypes.Values)
			{
				if (str == @in.FileExtension)
				{
					num = num2;
					break;
				}
				num2++;
			}
			openFileDialog1.Filter = m_knownFileTypesFilter;
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
			var document = new XmlDocument();
			if (m_registeredFileTypes.TryGetValue(Path.GetExtension(fileName).ToLower(), out @in))
			{
				AddToFileHistory(fileName);
				@in = (IUIPlugIn) Activator.CreateInstance(@in.GetType());
				@in.Sequence = @in.Open(fileName);
				(@in as UIBase).DirtyChanged += plugin_DirtyChanged;
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
			saveToolStripMenuItem.Enabled = (sender as IUIPlugIn).IsDirty;
		}

		private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var array = new IUIPlugIn[m_registeredFileTypes.Values.Count];
			m_registeredFileTypes.Values.CopyTo(array, 0);
			var dialog = new PreferencesDialog(m_preferences, array);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				m_preferences.Reload();
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
			if (base.ActiveMdiChild is IUIPlugIn)
			{
				var activeMdiChild = (IUIPlugIn) base.ActiveMdiChild;
				if ((activeMdiChild.Sequence.Name != null) && (activeMdiChild.Sequence.Name.Length > 0))
				{
					saveToolStripMenuItem.Text = string.Format("Save ({0})", activeMdiChild.Sequence.Name);
				}
				else
				{
					saveToolStripMenuItem.Text = "Save";
				}
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
			if (m_registeredFileTypes.TryGetValue(fileExtension, out @in))
			{
				return false;
			}
			m_registeredFileTypes[fileExtension] = inputPlugin;
			return true;
		}

		private void restartCurrentTimerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (m_timers.TimersDisabled)
			{
				MessageBox.Show("Schedule is currently disabled.", Vendor.ProductName, MessageBoxButtons.OK,
				                MessageBoxIcon.Exclamation);
			}
			else
			{
				SetTimerTraceFlag();
				List<Timer> list = m_timers.CurrentlyEffectiveTimers();
				if (list.Count == 0)
				{
					MessageBox.Show("There is nothing scheduled to execute at this time.", Vendor.ProductName, MessageBoxButtons.OK,
					                MessageBoxIcon.Asterisk);
				}
				else
				{
					foreach (Timer timer in list)
					{
						m_timerExecutor.SpawnExecutorFor(timer);
					}
					if (m_timerExecutor.ExecutingTimerCount == 0)
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
			if ((@in.IsDirty && ((@in.Sequence.Name == null) || (@in.Sequence.Name == string.Empty))) &&
			    !GetNewName(pluginInstance))
			{
				return false;
			}
			UpdateHistoryImages(@in.Sequence.FileName);
			@in.SaveTo(@in.Sequence.FileName);
			AddToFileHistory(@in.Sequence.FileName);
			if (m_preferences.GetBoolean("ShowSaveConfirmation"))
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
			SaveAs((UIBase) base.ActiveMdiChild);
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Save((UIBase) base.ActiveMdiChild);
		}

		private void setBackgroundSequenceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var dialog = new BackgroundSequenceDialog(m_preferences.GetString("BackgroundSequence"), Paths.SequencePath);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				m_preferences.SetString("BackgroundSequence", dialog.BackgroundSequenceFileName);
				m_host.BackgroundSequenceName = dialog.BackgroundSequenceFileName;
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
				m_shutdownAt = DateTime.Parse(time);
				shutdownTimer.Start();
			}
		}

		private void setSoundDeviceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var dialog = new SoundDeviceDialog(m_preferences);
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
			if ((DateTime.Now.Hour == m_shutdownAt.Hour) && (DateTime.Now.Minute == m_shutdownAt.Minute))
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
					m_loadableData.RootNode.SelectSingleNode(string.Format("IAddInData/IAddIn[@name=\"{0}\" and @enabled=\"{1}\"]",
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
					m_loadableData.RootNode.SelectSingleNode(
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
			base.LayoutMdi(MdiLayout.TileHorizontal);
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			SetTimerTraceFlag();
			foreach (Timer timer in m_timers.StartingTimers())
			{
				m_timerExecutor.SpawnExecutorFor(timer);
			}
		}

		private void timersToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var dialog = new ScheduleDialog(m_timers);
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				m_timers = dialog.Timers;
				m_timers.TimersDisabled = dialog.ScheduleDisabled;
				XmlDocument contextNode = Xml.CreateXmlDocument();
				m_timers.SaveToXml(contextNode);
				contextNode.Save(m_timersPath);
				scheduleTimer.Enabled = !m_timers.TimersDisabled;
			}
			dialog.Dispose();
		}

		private void TriggerClickHandler(object Sender, EventArgs e)
		{
			var item = (ToolStripMenuItem) Sender;
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
				int integer = m_preferences.GetInteger("HistoryImages");
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
			m_iTriggerImpl.ShowRegistrations();
		}

		private void visualChannelLayoutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			IExecutable executableObject;
			if (((base.ActiveMdiChild != null) && (base.ActiveMdiChild is VixenMDI)) &&
			    (((VixenMDI) base.ActiveMdiChild).Sequence != null))
			{
				executableObject = ((VixenMDI) base.ActiveMdiChild).Sequence;
			}
			else
			{
				string str = m_preferences.GetString("DefaultProfile");
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