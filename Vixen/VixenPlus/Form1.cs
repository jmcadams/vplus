namespace Vixen {
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Drawing;
	using System.IO;
	using System.Reflection;
	using System.Resources;
	using System.Text;
	using System.Threading;
	using System.Windows.Forms;
	using System.Xml;
	using AppUpdate;
	using FMOD;
	using Vixen.Dialogs;

	internal partial class Form1 : Form, ISystem {
		private string[] m_audioDevices = null;
		private List<string> m_history;
		private EventHandler m_historyItemClick;
		private int m_historyMax = 7;
		private Host m_host;
		private ITriggerImpl m_iTriggerImpl;
		private string m_knownFileTypesFilter;
		private string m_lastWindowsClipboardValue = "";
		private LoadableData m_loadableData;
		private Dictionary<string, List<LoadedObject>> m_loadables;
		private EventHandler m_newMenuItemClick;
		private Preference2 m_preferences;
		private Dictionary<string, IUIPlugIn> m_registeredFileTypes;
		private DateTime m_shutdownAt;
		private TimerExecutor m_timerExecutor;
		private Timers m_timers;
		private string m_timersPath;

		public Form1(string[] args) {
			List<string> list = new List<string>();
			list.AddRange(args);
			//this.LoadVendorData();
			this.SetDataPath();
			this.Ensure(Paths.DataPath);
			this.Ensure(Paths.SequencePath);
			this.Ensure(Paths.ProgramPath);
			this.Ensure(Paths.ImportExportPath);
			this.Ensure(Paths.AudioPath);
			this.Ensure(Paths.ProfilePath);
			this.Ensure(Paths.RoutinePath);
			this.Ensure(Paths.SourceFilePath);
			this.Ensure(Paths.CurveLibraryPath);
			Splash splash = new Splash();
			splash.Show();
			splash.Refresh();
			this.InitializeComponent();
			this.SetVendorData();
			this.m_registeredFileTypes = new Dictionary<string, IUIPlugIn>();
			this.m_timersPath = Path.Combine(Paths.DataPath, "timers");
			this.helpProvider.HelpNamespace = Path.Combine(Paths.BinaryPath, Vendor.ProductName + ".chm");
			string path = Path.Combine(Paths.BinaryPath, "prepare.exe");
			if (File.Exists(path)) {
				Process.Start(path).WaitForExit();
				File.Delete(path);
			}
			this.m_preferences = Preference2.GetInstance();
			this.m_preferences.PreferenceChange += new Preference2.OnPreferenceChange(this.m_preferences_PreferenceChange);
			this.m_host = new Host(this);
			this.m_loadables = new Dictionary<string, List<LoadedObject>>();
			Interfaces.Available["ISystem"] = this;
			Interfaces.Available["IExecution"] = new IExecutionImpl(this.m_host);
			Interfaces.Available["ITrigger"] = this.m_iTriggerImpl = new ITriggerImpl(this.m_host);
			this.m_newMenuItemClick = new EventHandler(this.NewMenuItemClick);
			this.m_historyItemClick = new EventHandler(this.HistoryItemClick);
			this.LoadHistory();
			this.m_loadableData = new LoadableData();
			this.m_loadableData.LoadFromXml(this.m_preferences.XmlDoc.DocumentElement);
			splash.Task = "Starting add-ins";
			if (!list.Contains("no_addins")) {
				this.StartAddins();
			}
			splash.Task = "Starting triggers";
			if (!list.Contains("no_triggers")) {
				this.StartTriggers();
			}
			splash.Task = "Loading UI";
			this.LoadUIPlugins();
			this.Cursor = Cursors.WaitCursor;
			try {
				foreach (string str2 in args) {
					if (File.Exists(str2)) {
						this.OpenSequence(str2);
					}
				}
			}
			finally {
				this.Cursor = Cursors.Default;
			}
			this.scheduleTimer.Interval = this.m_preferences.GetInteger("TimerCheckFrequency") * 1000;
			this.m_timers = new Timers();
			if (File.Exists(this.m_timersPath)) {
				this.m_timers.LoadFromXml(Xml.LoadDocument(this.m_timersPath));
			}
			this.m_timerExecutor = new TimerExecutor(this.m_host);
			this.scheduleTimer.Enabled = !this.m_timers.TimersDisabled;
			if (this.m_preferences.GetBoolean("EnableBackgroundSequence")) {
				this.m_host.BackgroundSequenceName = this.m_preferences.GetString("BackgroundSequence");
			}
			this.m_host.StartBackgroundObjects();
			this.SetShutdownTime(this.m_preferences.GetString("ShutdownTime"));
			splash.Hide();
			splash.Dispose();
			if (!(list.Contains("no_update") || File.Exists(Path.Combine(Paths.DataPath, "no.update")))) {
				this.CheckForUpdates();
			}
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
			AboutDialog dialog = new AboutDialog();
			dialog.ShowDialog();
			dialog.Dispose();
		}

		private void AddInClickHandler(object Sender, EventArgs e) {
			ToolStripMenuItem item = (ToolStripMenuItem)Sender;
			LoadedObject tag = (LoadedObject)item.Tag;
			if (tag.Instance == null) {
				if (!this.InstantiateObject(tag)) {
					return;
				}
				item.Checked = true;
			}
			EventSequence sequence = null;
			if ((base.ActiveMdiChild != null) && (base.ActiveMdiChild is VixenMDI)) {
				sequence = ((VixenMDI)base.ActiveMdiChild).Sequence;
			}
			try {
				if (((IAddIn)tag.Instance).Execute(sequence) && (sequence != null)) {
					((VixenMDI)base.ActiveMdiChild).Notify(Notification.SequenceChange, null);
				}
			}
			catch (Exception exception) {
				MessageBox.Show("Add-in error:\n\n" + exception.Message, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			finally {
				if (tag.Instance.DataLocationPreference == LoadableDataLocation.Sequence) {
					tag.Instance.Unloading();
					tag.Instance = null;
				}
			}
		}

		private void AddToFileHistory(string fileName) {
			string item = Path.GetFileName(fileName);
			this.m_history.Remove(item);
			this.m_history.Insert(0, item);
			if (this.m_history.Count > this.m_historyMax) {
				this.m_history.RemoveAt(this.m_history.Count - 1);
			}
			this.FlushHistory();
			this.LoadHistory();
		}

		private void cascadeToolStripMenuItem_Click(object sender, EventArgs e) {
			base.LayoutMdi(MdiLayout.Cascade);
		}

		private void ChangeSequenceName(IUIPlugIn pluginInstance, string newName) {
			if (!newName.EndsWith(pluginInstance.FileExtension)) {
				newName = newName + pluginInstance.FileExtension;
			}
			pluginInstance.Sequence.Name = newName;
			this.saveToolStripMenuItem.Text = string.Format("Save ({0})", pluginInstance.Sequence.Name);
			((Form)pluginInstance).Text = pluginInstance.Sequence.Name;
		}

		private void channelDimmingCurvesToolStripMenuItem_Click(object sender, EventArgs e) {
			IUIPlugIn activeMdiChild = base.ActiveMdiChild as IUIPlugIn;
			if (activeMdiChild != null) {
				DimmingCurveDialog dialog = new DimmingCurveDialog(activeMdiChild.Sequence, null);
				if (dialog.ShowDialog() == DialogResult.OK) {
					activeMdiChild.IsDirty = true;
				}
			}
		}

		private DialogResult CheckDirty(IUIPlugIn pluginInstance) {
			DialogResult none = DialogResult.None;
			if (pluginInstance.IsDirty) {
				string str = (pluginInstance.Sequence.Name == null) ? "this unnamed sequence" : pluginInstance.Sequence.Name;
				none = MessageBox.Show(string.Format("[{0}]\nSave changes to {1}?", pluginInstance.FileTypeDescription, str), Vendor.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				if (none == DialogResult.Yes) {
					this.Save((UIBase)pluginInstance);
				}
			}
			return none;
		}

		private void CheckForUpdates() {
			string updateServerURI = Vendor.UpdateFile;
			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			string updateRootPath = string.Format("{0}/{1}.{2}", Vendor.UpdateFile, version.Major, version.Minor);
			string path = Path.Combine(Paths.DataPath, "target.update");
			if (File.Exists(path)) {
				string str4;
				StreamReader reader = new StreamReader(path);
				while ((str4 = reader.ReadLine()) != null) {
					string[] strArray = str4.Split(new char[] { '=' });
					string str5 = strArray[0];
					if (str5 != null) {
						if (!(str5 == "server")) {
							if (str5 == "root") {
								goto Label_00B8;
							}
						}
						else {
							updateServerURI = strArray[1].Trim();
						}
					}
					goto Label_00C4;
				Label_00B8:
					updateRootPath = strArray[1].Trim();
				Label_00C4: ;
				}
				reader.Close();
				reader.Dispose();
			}
			new AppUpdate(updateServerURI, updateRootPath).ExecuteMigration();
		}

		private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e) {
			this.CheckForUpdates();
		}

		private void contentsToolStripMenuItem_Click(object sender, EventArgs e) {
			Help.ShowHelp(this, this.helpProvider.HelpNamespace);
		}

		private void copyASequenceToolStripMenuItem_Click(object sender, EventArgs e) {
			CopySequenceDialog dialog = new CopySequenceDialog();
			dialog.ShowDialog();
			dialog.Dispose();
		}

		private void copyChannelColorsToolStripMenuItem_Click(object sender, EventArgs e) {
			CopyChannelColorsDialog dialog = new CopyChannelColorsDialog();
			dialog.ShowDialog();
			dialog.Dispose();
		}

		private void copyPluginaddinDataToolStripMenuItem_Click(object sender, EventArgs e) {
			CopyDataDialog2 dialog = new CopyDataDialog2();
			dialog.ShowDialog();
			dialog.Dispose();
		}

		private void diagnosticsToolStripMenuItem_Click(object sender, EventArgs e) {
			new DiagnosticsDialog(this.m_timers).ShowDialog();
		}



		private void Ensure(string path) {
			if (!Directory.Exists(path)) {
				Directory.CreateDirectory(path);
			}
		}

		private int FindMdiChildIndex(Form childForm) {
			for (int i = 0; i < base.MdiChildren.Length; i++) {
				if (childForm == base.MdiChildren[i]) {
					return i;
				}
			}
			return -1;
		}

		private Form FindUIBase() {
			if (base.ActiveMdiChild is UIBase) {
				return base.ActiveMdiChild;
			}
			for (int i = this.FindMdiChildIndex(base.ActiveMdiChild) - 1; i > 0; i--) {
				if (base.MdiChildren[i] is UIBase) {
					return base.MdiChildren[i];
				}
			}
			return null;
		}

		private void FlushHistory() {
			XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(this.m_preferences.XmlDoc.DocumentElement, "History");
			foreach (string str in this.m_history) {
				Xml.SetNewValue(emptyNodeAlways, "Item", str);
			}
			this.m_preferences.Flush();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
			foreach (Form form in base.MdiChildren) {
				if ((form is IUIPlugIn) && (this.CheckDirty((IUIPlugIn)form) == DialogResult.Cancel)) {
					e.Cancel = true;
					return;
				}
			}
			this.m_host.StopBackgroundObjects();
			this.m_host.BackgroundSequenceName = null;
			foreach (ToolStripItem item in this.addInsToolStripMenuItem.DropDownItems) {
				if (item.Tag is LoadedObject) {
					LoadedObject tag = (LoadedObject)item.Tag;
					XmlNode loadableData = this.m_loadableData.GetLoadableData(tag.InterfaceImplemented, item.Text);
					if ((tag.Instance != null) && (tag.Instance.DataLocationPreference == LoadableDataLocation.Application)) {
						Xml.SetAttribute(loadableData, "enabled", bool.TrueString);
						tag.Instance.Unloading();
					}
					else {
						Xml.SetAttribute(loadableData, "enabled", bool.FalseString);
					}
				}
			}
			foreach (ToolStripItem item in this.triggersToolStripMenuItem.DropDownItems) {
				if (item.Tag is LoadedObject) {
					LoadedObject obj3 = (LoadedObject)item.Tag;
					XmlNode node = this.m_loadableData.GetLoadableData(obj3.InterfaceImplemented, item.Text);
					if (obj3.Instance != null) {
						Xml.SetAttribute(node, "enabled", bool.TrueString);
						obj3.Instance.Unloading();
					}
					else {
						Xml.SetAttribute(node, "enabled", bool.FalseString);
					}
				}
			}
			this.m_preferences.Flush();
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e) {
			if (base.ActiveMdiChild != null) {
				if ((base.ActiveMdiChild is OutputPlugInUIBase) && (((OutputPlugInUIBase)base.ActiveMdiChild).ExecutionParent != null)) {
					((OutputPlugInUIBase)base.ActiveMdiChild).ExecutionParent.Notify(Notification.KeyDown, e);
				}
				else if (base.ActiveMdiChild is VixenMDI) {
					((VixenMDI)base.ActiveMdiChild).Notify(Notification.KeyDown, e);
				}
			}
		}

		private void Form1_MdiChildActivate(object sender, EventArgs e) {
			if (base.ActiveMdiChild is IUIPlugIn) {
				this.saveToolStripMenuItem.Enabled = (base.ActiveMdiChild as IUIPlugIn).IsDirty;
				this.saveAsToolStripMenuItem.Enabled = true;
			}
			else {
				this.saveToolStripMenuItem.Enabled = false;
				this.saveAsToolStripMenuItem.Enabled = false;
			}
		}

		private bool FormContainsChild(Form parent, Form child) {
			foreach (Form form in parent.MdiChildren) {
				if (form == child) {
					return true;
				}
			}
			return false;
		}

		public int GetExecutingTimerExecutionContextHandle(int executingTimerIndex) {
			return this.m_timerExecutor.GetExecutingTimerExecutionContextHandle(executingTimerIndex);
		}

		private bool GetNewName(IUIPlugIn pluginInstance) {
			this.saveFileDialog1.Filter = string.Format("{0}|*{1}", pluginInstance.FileTypeDescription, pluginInstance.FileExtension);
			this.saveFileDialog1.InitialDirectory = Paths.SequencePath;
			this.saveFileDialog1.FileName = string.Empty;
			if (this.saveFileDialog1.ShowDialog() == DialogResult.OK) {
				this.ChangeSequenceName(pluginInstance, this.saveFileDialog1.FileName);
				return true;
			}
			return false;
		}

		private void HistoryItemClick(object sender, EventArgs e) {
			string text = ((ToolStripItem)sender).Text;
			string path = Path.Combine(Paths.SequencePath, text);
			if (File.Exists(path)) {
				this.OpenSequence(path);
			}
			else {
				this.m_history.Remove(text);
				this.FlushHistory();
				this.LoadHistory();
				MessageBox.Show("File does not exist.\nItem has been removed from the history.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}
		//ComponentResourceManager manager = new ComponentResourceManager(typeof(Form1));
		//this.toolStripStatusLabelMusic.Image = (Image)manager.GetObject("toolStripStatusLabelMusic.Image");
		//base.Icon = (Icon)manager.GetObject("$this.Icon");



		public Form InstantiateForm(ConstructorInfo constructorInfo, params object[] parameters) {
			if (base.InvokeRequired) {
				return (Form)base.Invoke(new InstantiateFormDelegate(this.InstantiateForm), new object[] { constructorInfo, parameters });
			}
			Form child = (Form)constructorInfo.Invoke(parameters);
			if (child == null) {
				return null;
			}
			if (!(child is OutputPlugInUIBase)) {
				return null;
			}
			OutputPlugInUIBase base2 = (OutputPlugInUIBase)child;
			IExecutable executable = (IExecutable)Host.Communication["CurrentObject"];
			if (executable != null) {
				string str = executable.Key.ToString();
				XmlNode node = null;
				XmlNode node2 = null;
				node = ((XmlNode)Host.Communication["SetupNode_" + str]).SelectSingleNode("DialogPositions");
				object obj2 = null;
				if (Host.Communication.TryGetValue("KeyInterceptor_" + str, out obj2)) {
					base2.ExecutionParent = (VixenMDI)obj2;
				}
				if (Host.Communication.TryGetValue("SetupNode_" + str, out obj2)) {
					base2.DataNode = (XmlNode)obj2;
				}
				child.ControlBox = true;
				if (!this.FormContainsChild(this, child)) {
					child.MdiParent = this;
				}
				((Vixen.ExecutionContext)Host.Communication["ExecutionContext_" + str]).OutputPlugInForms.Add(child);
				child.Show();
				if (node != null) {
					node2 = node.SelectSingleNode(child.Name);
				}
				if ((node2 != null) && this.m_preferences.GetBoolean("SavePlugInDialogPositions")) {
					XmlAttribute attribute = node2.Attributes["x"];
					XmlAttribute attribute2 = node2.Attributes["y"];
					if ((attribute != null) && (attribute2 != null)) {
						child.Location = new Point(Convert.ToInt32(attribute.Value), Convert.ToInt32(attribute2.Value));
					}
				}
			}
			return child;
		}

		private bool InstantiateObject(LoadedObject loadedObject) {
			loadedObject.Instance = (ILoadable)Activator.CreateInstance(loadedObject.ObjectType);
			try {
				if ((((loadedObject.Instance.DataLocationPreference == LoadableDataLocation.Sequence) && (base.ActiveMdiChild != null)) && (base.ActiveMdiChild is VixenMDI)) && (((VixenMDI)base.ActiveMdiChild).Sequence != null)) {
					loadedObject.Instance.Loading(((VixenMDI)base.ActiveMdiChild).Sequence.LoadableData.GetLoadableData(loadedObject.InterfaceImplemented, loadedObject.Instance.Name));
				}
				else {
					loadedObject.Instance.Loading(this.m_loadableData.GetLoadableData(loadedObject.InterfaceImplemented, loadedObject.Instance.Name));
				}
			}
			catch (Exception exception) {
				MessageBox.Show(string.Format("Error when initializing loadable {0}:\n\n{1}", loadedObject.Instance.Name, exception.Message), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}
			return true;
		}

		public bool InvokeSave(UIBase pluginInstance) {
			return this.Save(pluginInstance);
		}

		public List<ILoadable> LoadableList(string interfaceName) {
			List<ILoadable> list = new List<ILoadable>();
			if (this.m_loadables.ContainsKey(interfaceName)) {
				foreach (LoadedObject obj2 in this.m_loadables[interfaceName]) {
					if (obj2.Instance != null) {
						list.Add(obj2.Instance);
					}
				}
			}
			return list;
		}

		private void loadableMenuItem_CheckClick(object sender, EventArgs e) {
			ToolStripMenuItem item = (ToolStripMenuItem)sender;
			LoadedObject tag = (LoadedObject)item.Tag;
			if (item.Checked) {
				this.InstantiateObject(tag);
			}
			else {
				try {
					tag.Instance.Unloading();
				}
				catch (Exception exception) {
					MessageBox.Show(string.Format("{0} caused the following error while unloading:\n\n{1}", tag.Instance.Name, exception.Message), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				tag.Instance = null;
			}
		}

		private void loadableToolStripMenuItem_DropDownOpening(object sender, EventArgs e) {
			ToolStripMenuItem item = (ToolStripMenuItem)sender;
			foreach (ToolStripItem item2 in item.DropDownItems) {
				if (item2.Tag is LoadedObject) {
					LoadedObject tag = (LoadedObject)item2.Tag;
					((ToolStripMenuItem)item2).Checked = tag.Instance != null;
				}
			}
		}

		private void LoadHistory() {
			this.recentToolStripMenuItem.DropDownItems.Clear();
			this.m_history = new List<string>();
			XmlNodeList list = this.m_preferences.XmlDoc.SelectNodes("//User/History/*");
			foreach (XmlNode node in list) {
				this.m_history.Add(node.InnerText);
				this.recentToolStripMenuItem.DropDownItems.Add(node.InnerText, null, this.m_historyItemClick);
			}
			this.recentToolStripMenuItem.Enabled = this.recentToolStripMenuItem.DropDownItems.Count > 0;
		}

		private List<LoadedObject> LoadObjects(string directory, string interfaceName) {
			List<LoadedObject> list = new List<LoadedObject>();
			if (Directory.Exists(directory)) {
				foreach (string str in Directory.GetFiles(directory, "*.dll")) {
					Exception exception;
					try {
						Assembly assembly = Assembly.LoadFile(str);
						foreach (System.Type type in assembly.GetExportedTypes()) {
							foreach (System.Type type2 in type.GetInterfaces()) {
								if (type2.Name == interfaceName) {
									try {
										list.Add(new LoadedObject(type, interfaceName));
									}
									catch (Exception exception1) {
										exception = exception1;
										MessageBox.Show(string.Format("Error when loading object from {0}:\n{1}", Path.GetFileNameWithoutExtension(str), exception.Message), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
									}
								}
							}
						}
					}
					catch (BadImageFormatException) {
					}
					catch (Exception exception3) {
						exception = exception3;
						MessageBox.Show(string.Format("{0}:\n{1}", Path.GetFileName(str), exception.Message));
					}
				}
				this.m_loadables[interfaceName] = list;
			}
			return list;
		}

		private void LoadUIPlugins() {
			if (Directory.Exists(Paths.UIPluginPath)) {
				foreach (string str in Directory.GetFiles(Paths.UIPluginPath, "*.dll")) {
					Exception exception;
					try {
						Assembly assembly = Assembly.LoadFile(str);
						foreach (System.Type type in assembly.GetExportedTypes()) {
							foreach (System.Type type2 in type.GetInterfaces()) {
								if (type2.Name == "IUIPlugIn") {
									try {
										Debug.WriteLine("Type: " + type.Name + " Type2: " + type2.Name);
										IUIPlugIn inputPlugin = (IUIPlugIn)Activator.CreateInstance(type);
										if (!this.RegisterFileType(inputPlugin.FileExtension, inputPlugin)) {
											MessageBox.Show(string.Format("Could not register UI plugin {0}.\nFile type is already handled.", inputPlugin.Name), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
										}
									}
									catch (Exception exception1) {
										exception = exception1;
										MessageBox.Show(string.Format("Error when loading UI plugin from {0}:\n{1}", Path.GetFileNameWithoutExtension(str), exception.StackTrace), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
									}
								}
							}
						}
					}
					catch (BadImageFormatException) {
					}
					catch (Exception exception3) {
						exception = exception3;
						MessageBox.Show(string.Format("{0}:\n{1}", Path.GetFileName(str), exception.Message));
					}
				}
				foreach (IUIPlugIn in2 in this.m_registeredFileTypes.Values) {
					ToolStripItem item = this.newLightingProgramToolStripMenuItem.DropDownItems.Add(in2.FileTypeDescription);
					item.Tag = in2;
					item.Click += this.m_newMenuItemClick;
				}
				StringBuilder builder = new StringBuilder();
				foreach (IUIPlugIn in2 in this.m_registeredFileTypes.Values) {
					builder.AppendFormat("|{0}|*{1}", in2.FileTypeDescription, in2.FileExtension);
				}
				this.m_knownFileTypesFilter = builder.ToString().Remove(0, 1);
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

		private void m_preferences_PreferenceChange(string preferenceName) {
			switch (preferenceName) {
				case "TimerCheckFrequency":
					this.scheduleTimer.Interval = this.m_preferences.GetInteger("TimerCheckFrequency") * 0x3e8;
					break;

				case "EnableBackgroundSequence":
					if (!this.m_preferences.GetBoolean("EnableBackgroundSequence")) {
						this.m_host.StopBackgroundSequence();
						break;
					}
					this.m_host.BackgroundSequenceName = this.m_preferences.GetString("BackgroundSequence");
					this.m_host.StartBackgroundSequence();
					break;

				case "EnableBackgroundMusic":
					if (!this.m_preferences.GetBoolean("EnableBackgroundMusic")) {
						this.m_host.StopBackgroundMusic();
						break;
					}
					this.m_host.StartBackgroundMusic();
					break;

				case "EventPeriod":
					if (this.m_preferences.GetInteger("EventPeriod") < 0x19) {
						this.m_preferences.SetInteger("EventPeriod", 0x19);
						MessageBox.Show("The event period length cannot be less than 25 milliseconds.\nThe length has been reset to 25 milliseconds.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
					break;

				case "BackgroundMusicDelay":
					if (this.m_preferences.GetInteger("BackgroundMusicDelay") < 1) {
						MessageBox.Show("Delay cannot be less than 1 second.\nResetting to 1 second", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
						this.m_preferences.SetInteger("BackgroundMusicDelay", 1);
					}
					break;

				case "BackgroundSequenceDelay":
					if (this.m_preferences.GetInteger("BackgroundSequenceDelay") < 1) {
						MessageBox.Show("Delay cannot be less than 1 second.\nResetting to 1 second", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
						this.m_preferences.SetInteger("BackgroundSequenceDelay", 1);
					}
					break;

				case "ShutdownTime":
					this.SetShutdownTime(this.m_preferences.GetString("ShutdownTime"));
					break;
			}
		}

		private void manageProgramsToolStripMenuItem_Click(object sender, EventArgs e) {
			ProgramManagerDialog dialog = new ProgramManagerDialog(this.m_host);
			dialog.ShowDialog();
			dialog.Dispose();
		}

		private void manageToolStripMenuItem_Click(object sender, EventArgs e) {
			ProfileManagerDialog dialog = new ProfileManagerDialog(null);
			if (dialog.ShowDialog() == DialogResult.OK) {
				this.NotifyAll(Notification.ProfileChange);
			}
			dialog.Dispose();
		}

		private void musicPlayerToolStripMenuItem_Click(object sender, EventArgs e) {
			this.m_host.MusicPlayer.ShowDialog();
		}

		private void NewMenuItemClick(object sender, EventArgs e) {
			ToolStripItem item = (ToolStripItem)sender;
			if (!(item.Tag is IUIPlugIn)) {
				return;
			}
			IUIPlugIn tag = (IUIPlugIn)item.Tag;
			tag = (IUIPlugIn)Activator.CreateInstance(tag.GetType());
			tag.Sequence = null;
			if (this.m_preferences.GetBoolean("WizardForNewSequences")) {
				EventSequence resultSequence = null;
				switch (tag.RunWizard(ref resultSequence)) {
					case DialogResult.None:
						MessageBox.Show("No wizard available.\n\nA blank sequence will be created for you.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
						tag.Sequence = tag.New();
						goto Label_00DF;

					case DialogResult.OK:
						tag.Sequence = tag.New(resultSequence);
						if (!this.SaveAs((UIBase)tag)) {
							base.DialogResult = DialogResult.None;
						}
						goto Label_00DF;

					case DialogResult.Cancel:
						return;
				}
			}
			else {
				tag.Sequence = tag.New();
			}
		Label_00DF:
			if (tag.Sequence != null) {
				(tag as UIBase).DirtyChanged += new EventHandler(this.plugin_DirtyChanged);
				tag.MdiParent = this;
				tag.Show();
			}
		}

		private void NotifyAll(Notification notification) {
			foreach (Form form in base.MdiChildren) {
				if (form is VixenMDI) {
					((VixenMDI)form).Notify(notification, null);
				}
			}
		}

		private void onlineSupportForumToolStripMenuItem_Click(object sender, EventArgs e) {
			Process process = new Process();
			process.StartInfo.FileName = Vendor.SupportURL;
			process.StartInfo.UseShellExecute = true;
			process.Start();
		}

		private void openALightingProgramToolStripMenuItem_Click(object sender, EventArgs e) {
			int num = 0;
			int num2 = 1;
			string str = this.m_preferences.GetString("PreferredSequenceType");
			foreach (IUIPlugIn @in in this.m_registeredFileTypes.Values) {
				if (str == @in.FileExtension) {
					num = num2;
					break;
				}
				num2++;
			}
			this.openFileDialog1.Filter = this.m_knownFileTypesFilter;
			this.openFileDialog1.InitialDirectory = Paths.SequencePath;
			this.openFileDialog1.FileName = string.Empty;
			this.openFileDialog1.FilterIndex = num;
			if (this.openFileDialog1.ShowDialog() == DialogResult.OK) {
				this.Cursor = Cursors.WaitCursor;
				try {
					this.OpenSequence(this.openFileDialog1.FileName);
				}
				finally {
					this.Cursor = Cursors.Default;
				}
			}
		}

		public void OpenSequence(string fileName) {
			IUIPlugIn @in;
			XmlDocument document = new XmlDocument();
			if (this.m_registeredFileTypes.TryGetValue(Path.GetExtension(fileName).ToLower(), out @in)) {
				this.AddToFileHistory(fileName);
				@in = (IUIPlugIn)Activator.CreateInstance(@in.GetType());
				@in.Sequence = @in.Open(fileName);
				(@in as UIBase).DirtyChanged += new EventHandler(this.plugin_DirtyChanged);
				@in.MdiParent = this;
				@in.Show();
			}
			else {
				MessageBox.Show("There is no known editor for that file type.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private void plugin_DirtyChanged(object sender, EventArgs e) {
			this.saveToolStripMenuItem.Enabled = (sender as IUIPlugIn).IsDirty;
		}

		private void preferencesToolStripMenuItem_Click(object sender, EventArgs e) {
			IUIPlugIn[] array = new IUIPlugIn[this.m_registeredFileTypes.Values.Count];
			this.m_registeredFileTypes.Values.CopyTo(array, 0);
			PreferencesDialog dialog = new PreferencesDialog(this.m_preferences, array);
			if (dialog.ShowDialog() == DialogResult.OK) {
				this.m_preferences.Reload();
				this.NotifyAll(Notification.PreferenceChange);
			}
			dialog.Dispose();
		}

		private void programsToolStripMenuItem_DropDownOpening(object sender, EventArgs e) {
			this.manageProgramsToolStripMenuItem.Enabled = ((ISystem)Interfaces.Available["ISystem"]).ExecutingTimerCount == 0;
		}

		private void programToolStripMenuItem_DropDownOpening(object sender, EventArgs e) {
			if (base.ActiveMdiChild is IUIPlugIn) {
				IUIPlugIn activeMdiChild = (IUIPlugIn)base.ActiveMdiChild;
				if ((activeMdiChild.Sequence.Name != null) && (activeMdiChild.Sequence.Name.Length > 0)) {
					this.saveToolStripMenuItem.Text = string.Format("Save ({0})", activeMdiChild.Sequence.Name);
				}
				else {
					this.saveToolStripMenuItem.Text = "Save";
				}
				this.channelDimmingCurvesToolStripMenuItem.Enabled = true;
			}
			else {
				this.saveToolStripMenuItem.Text = "Save";
				this.channelDimmingCurvesToolStripMenuItem.Enabled = false;
			}
		}

		private bool RegisterFileType(string fileExtension, IUIPlugIn inputPlugin) {
			IUIPlugIn @in;
			fileExtension = fileExtension.ToLower();
			if (this.m_registeredFileTypes.TryGetValue(fileExtension, out @in)) {
				return false;
			}
			this.m_registeredFileTypes[fileExtension] = inputPlugin;
			return true;
		}

		private void restartCurrentTimerToolStripMenuItem_Click(object sender, EventArgs e) {
			if (this.m_timers.TimersDisabled) {
				MessageBox.Show("Schedule is currently disabled.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else {
				this.SetTimerTraceFlag();
				List<Vixen.Timer> list = this.m_timers.CurrentlyEffectiveTimers();
				if (list.Count == 0) {
					MessageBox.Show("There is nothing scheduled to execute at this time.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
				else {
					foreach (Vixen.Timer timer in list) {
						this.m_timerExecutor.SpawnExecutorFor(timer);
					}
					if (this.m_timerExecutor.ExecutingTimerCount == 0) {
						MessageBox.Show("Timers were not executed.\n\nThe scheduled items may be longer than their remaining time or there may have been an error.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					}
				}
			}
		}

		private bool Save(UIBase pluginInstance) {
			if (pluginInstance == null) {
				return false;
			}
			IUIPlugIn @in = pluginInstance;
			if ((@in.IsDirty && ((@in.Sequence.Name == null) || (@in.Sequence.Name == string.Empty))) && !this.GetNewName(pluginInstance)) {
				return false;
			}
			this.UpdateHistoryImages(@in.Sequence.FileName);
			@in.SaveTo(@in.Sequence.FileName);
			this.AddToFileHistory(@in.Sequence.FileName);
			if (this.m_preferences.GetBoolean("ShowSaveConfirmation")) {
				MessageBox.Show("Sequence saved", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			return true;
		}

		private bool SaveAs(UIBase pluginInstance) {
			return (this.GetNewName(pluginInstance) && this.Save(pluginInstance));
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
			this.SaveAs((UIBase)base.ActiveMdiChild);
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Save((UIBase)base.ActiveMdiChild);
		}

		private void setBackgroundSequenceToolStripMenuItem_Click(object sender, EventArgs e) {
			BackgroundSequenceDialog dialog = new BackgroundSequenceDialog(this.m_preferences.GetString("BackgroundSequence"), Paths.SequencePath);
			if (dialog.ShowDialog() == DialogResult.OK) {
				this.m_preferences.SetString("BackgroundSequence", dialog.BackgroundSequenceFileName);
				this.m_host.BackgroundSequenceName = dialog.BackgroundSequenceFileName;
			}
			dialog.Dispose();
		}

		private void SetDataPath() {
			if (File.Exists("redirect.data")) {
				Paths.DataPath = Path.Combine(Paths.BinaryPath, "Data");
			}
		}

		private void SetShutdownTime(string time) {
			if (time == string.Empty) {
				this.shutdownTimer.Stop();
			}
			else if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "shutdown.exe"))) {
				this.m_shutdownAt = DateTime.Parse(time);
				this.shutdownTimer.Start();
			}
		}

		private void setSoundDeviceToolStripMenuItem_Click(object sender, EventArgs e) {
			SoundDeviceDialog dialog = new SoundDeviceDialog(this.m_preferences);
			dialog.ShowDialog();
			dialog.Dispose();
		}

		private void SetTimerTraceFlag() {
			if (((Host.GetDebugValue("TraceTimers") == bool.TrueString) && (DateTime.Now >= DateTime.Parse(Host.GetDebugValue("TraceStart")))) && (DateTime.Now <= DateTime.Parse(Host.GetDebugValue("TraceEnd")))) {
				Host.SetDebugValue("TimerTrace");
			}
			else {
				Host.ResetDebugValue("TimerTrace");
			}
		}

		private void SetVendorData() {
			this.Text = Vendor.ProductName;
		}

		private void shutdownTimer_Tick(object sender, EventArgs e) {
			if ((DateTime.Now.Hour == this.m_shutdownAt.Hour) && (DateTime.Now.Minute == this.m_shutdownAt.Minute)) {
				this.shutdownTimer.Stop();
				Process.Start("shutdown", string.Format("/s /d P:4:1 /c \"Automatic shutdown by {0}\"", Vendor.ProductName));
				Thread.Sleep(0x3e8);
				new Vixen.ShutdownDialog().Show();
			}
		}

		private void StartAddins() {
			List<LoadedObject> list = this.LoadObjects(Paths.AddinPath, "IAddIn");
			this.addInsToolStripMenuItem.Visible = list.Count > 0;
			foreach (LoadedObject obj2 in list) {
				IAddIn @in = (IAddIn)Activator.CreateInstance(obj2.ObjectType);
				CheckableToolStripMenuItem item = new CheckableToolStripMenuItem();
				item.Click += new EventHandler(this.AddInClickHandler);
				item.Text = @in.Name;
				item.Tag = obj2;
				item.CheckClick += new EventHandler(this.loadableMenuItem_CheckClick);
				this.addInsToolStripMenuItem.DropDownItems.Add(item);
				if (this.m_loadableData.RootNode.SelectSingleNode(string.Format("IAddInData/IAddIn[@name=\"{0}\" and @enabled=\"{1}\"]", @in.Name, bool.TrueString)) != null) {
					this.InstantiateObject(obj2);
					item.Checked = true;
				}
			}
		}

		private void StartTriggers() {
			List<LoadedObject> list = this.LoadObjects(Paths.TriggerPluginPath, "ITriggerPlugin");
			this.triggersToolStripMenuItem.Visible = list.Count > 0;
			foreach (LoadedObject obj2 in list) {
				ITriggerPlugin plugin = (ITriggerPlugin)Activator.CreateInstance(obj2.ObjectType);
				CheckableToolStripMenuItem item = new CheckableToolStripMenuItem();
				item.Click += new EventHandler(this.TriggerClickHandler);
				item.Text = plugin.Name;
				item.Tag = obj2;
				item.CheckClick += new EventHandler(this.loadableMenuItem_CheckClick);
				this.triggersToolStripMenuItem.DropDownItems.Add(item);
				if (this.m_loadableData.RootNode.SelectSingleNode(string.Format("ITriggerPluginData/ITriggerPlugin[@name=\"{0}\" and @enabled=\"{1}\"]", plugin.Name, bool.TrueString)) != null) {
					this.InstantiateObject(obj2);
					item.Checked = true;
				}
			}
		}

		private void tileToolStripMenuItem_Click(object sender, EventArgs e) {
			base.LayoutMdi(MdiLayout.TileHorizontal);
		}

		private void timer1_Tick(object sender, EventArgs e) {
			this.SetTimerTraceFlag();
			foreach (Vixen.Timer timer in this.m_timers.StartingTimers()) {
				this.m_timerExecutor.SpawnExecutorFor(timer);
			}
		}

		private void timersToolStripMenuItem_Click(object sender, EventArgs e) {
			ScheduleDialog dialog = new ScheduleDialog(this.m_timers);
			if (dialog.ShowDialog() == DialogResult.OK) {
				this.m_timers = dialog.Timers;
				this.m_timers.TimersDisabled = dialog.ScheduleDisabled;
				XmlDocument contextNode = Xml.CreateXmlDocument();
				this.m_timers.SaveToXml(contextNode);
				contextNode.Save(this.m_timersPath);
				this.scheduleTimer.Enabled = !this.m_timers.TimersDisabled;
			}
			dialog.Dispose();
		}

		private void TriggerClickHandler(object Sender, EventArgs e) {
			ToolStripMenuItem item = (ToolStripMenuItem)Sender;
			LoadedObject tag = (LoadedObject)item.Tag;
			if (tag.Instance == null) {
				if (!this.InstantiateObject(tag)) {
					return;
				}
				item.Checked = true;
			}
			try {
				((ITriggerPlugin)tag.Instance).Setup();
			}
			catch (Exception exception) {
				MessageBox.Show("Trigger error:\n\n" + exception.Message, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void turnOnQueryServerToolStripMenuItem_Click(object sender, EventArgs e) {
		}

		private void UpdateHistoryImages(string baseFilePath) {
			if (File.Exists(baseFilePath)) {
				int integer = this.m_preferences.GetInteger("HistoryImages");
				if (integer != 0) {
					string[] files = Directory.GetFiles(Paths.SequencePath, Path.GetFileName(baseFilePath) + ".bak*");
					int num2 = files.Length + 1;
					if (files.Length >= integer) {
						num2--;
						for (int i = 2; i <= integer; i++) {
							File.Copy(string.Format("{0}.bak{1}", baseFilePath, i), string.Format("{0}.bak{1}", baseFilePath, i - 1), true);
						}
					}
					File.Copy(baseFilePath, string.Format("{0}.bak{1}", baseFilePath, num2), true);
				}
			}
		}

		public void VerifySequenceHardwarePlugins(EventSequence sequence) {
			OutputPlugins.VerifyPlugIns(sequence);
			InputPlugins.VerifyPlugIns(sequence);
		}

		private void viewRegisteredResponsesToolStripMenuItem_Click(object sender, EventArgs e) {
			this.m_iTriggerImpl.ShowRegistrations();
		}

		private void visualChannelLayoutToolStripMenuItem_Click(object sender, EventArgs e) {
			IExecutable executableObject = null;
			if (((base.ActiveMdiChild != null) && (base.ActiveMdiChild is VixenMDI)) && (((VixenMDI)base.ActiveMdiChild).Sequence != null)) {
				executableObject = ((VixenMDI)base.ActiveMdiChild).Sequence;
			}
			else {
				string str = this.m_preferences.GetString("DefaultProfile");
				executableObject = (str.Length == 0) ? null : ((IExecutable)new Profile(Path.Combine(Paths.ProfilePath, str + ".pro")));
			}
			if (executableObject == null) {
				MessageBox.Show("There is no sequence open, nor is there a default profile set.\nYou have no channels to layout.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else {
				ChannelLayoutDialog dialog = new ChannelLayoutDialog(executableObject);
				dialog.ShowDialog();
				dialog.Dispose();
			}
		}

		public string[] AudioDevices {
			get {
				if (this.m_audioDevices == null) {
					this.m_audioDevices = fmod.GetSoundDeviceList();
				}
				return this.m_audioDevices;
			}
		}

		public byte[,] Clipboard {
			get {
				byte[,] array = null;
				if (System.Windows.Forms.Clipboard.ContainsText()) {
					try {
						if (this.m_lastWindowsClipboardValue == System.Windows.Forms.Clipboard.GetText()) {
							return Host.Clipboard;
						}
						string[] strArray = System.Windows.Forms.Clipboard.GetText().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
						int maxCols = 0;
						Array.ForEach<string>(strArray, delegate(string s) {
							maxCols = Math.Max(s.Split(new char[] { ',' }).Length, maxCols);
						});
						array = new byte[strArray.Length, maxCols];
						Array.Clear(array, 0, array.Length);
						for (int i = 0; i < strArray.Length; i++) {
							string[] strArray2 = strArray[i].Split(new char[] { ',' });
							for (int j = 0; j < strArray2.Length; j++) {
								array[i, j] = byte.Parse(strArray2[j]);
							}
						}
					}
					catch {
						array = null;
					}
				}
				if (array == null) {
					return Host.Clipboard;
				}
				Host.Clipboard = array;
				this.m_lastWindowsClipboardValue = System.Windows.Forms.Clipboard.GetText();
				return array;
			}
			set {
				if ((value != null) && (value.Length > 0)) {
					Host.Clipboard = value;
					StringBuilder builder = new StringBuilder();
					for (int i = 0; i < value.GetLength(0); i++) {
						for (int j = 0; j < value.GetLength(1); j++) {
							builder.AppendFormat("{0},", value[i, j]);
						}
						builder.Remove(builder.Length - 1, 1);
						builder.AppendLine();
					}
					System.Windows.Forms.Clipboard.SetText(builder.ToString());
				}
			}
		}

		public int ExecutingTimerCount {
			get {
				return this.m_timerExecutor.ExecutingTimerCount;
			}
		}

		public string KnownFileTypesFilter {
			get {
				return this.m_knownFileTypesFilter;
			}
		}

		public Preference2 UserPreferences {
			get {
				return this.m_preferences;
			}
		}

		private delegate Form InstantiateFormDelegate(ConstructorInfo constructorInfo, params object[] parameters);
	}
}

