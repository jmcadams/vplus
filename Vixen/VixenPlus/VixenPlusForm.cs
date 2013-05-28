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
using Properties;
using VixenPlus.Dialogs;
using CommonUtils;

namespace VixenPlus
{
    internal sealed partial class VixenPlusForm : Form, ISystem
    {
#if debug
        private const int ExpectationDelay = 0;
#else
        private const int ExpectationDelay = 1500;
#endif

        private const int HistoryMax = 7;
        private List<string> _history;

        private DateTime _shutdownAt;

        private string _lastWindowsClipboardValue = "";

        private string[] _audioDevices;

        private readonly Dictionary<string, IUIPlugIn> _registeredFileTypes;
        private readonly Dictionary<string, List<LoadedObject>> _loadables;

        private readonly EventHandler _historyItemClick;
        private readonly EventHandler _newMenuItemClick;

        private readonly Host _host;

        private readonly LoadableData _loadableData;

        private readonly Preference2 _preferences;

        private readonly TimerExecutor _timerExecutor;
        private readonly Timers _timers;
        private readonly string _timersPath;

        public VixenPlusForm(string[] args)
        {
            var list = new List<string>();
            list.AddRange(args);
            SetDataPath();
            Ensure(Paths.DataPath);
            Ensure(Paths.SequencePath);
            Ensure(Paths.ProgramPath);
            Ensure(Paths.ImportExportPath);
            Ensure(Paths.AudioPath);
            Ensure(Paths.ProfilePath);
            Ensure(Paths.RoutinePath);
            //Ensure(Paths.SourceFilePath);
            Ensure(Paths.CurveLibraryPath);
            using (var splash = new Splash()) {
                _preferences = Preference2.GetInstance();
                var screen = _preferences.GetScreen(_preferences.GetString("PrimaryDisplay"));
                splash.FadeIn(screen);
                InitializeComponent();
                SetVendorData();
                _registeredFileTypes = new Dictionary<string, IUIPlugIn>();
                _timersPath = Path.Combine(Paths.DataPath, "timers");
                _preferences.PreferenceChange += PreferencesPreferenceChange;
                _host = new Host(this);
                _loadables = new Dictionary<string, List<LoadedObject>>();
                Interfaces.Available["ISystem"] = this;
                Interfaces.Available["IExecution"] = new ExecutionImpl(_host);
                _newMenuItemClick = NewMenuItemClick;
                _historyItemClick = HistoryItemClick;
                LoadHistory();
                _loadableData = new LoadableData();
                _loadableData.LoadFromXml(_preferences.XmlDoc.DocumentElement);
                LoadUIPlugins();
                Cursor = Cursors.WaitCursor;
                try
                {
                    foreach (var sequence in args)
                    {
                        if (File.Exists(sequence))
                        {
                            OpenSequence(sequence);
                        }
                    }
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
                scheduleTimer.Interval = _preferences.GetInteger("TimerCheckFrequency")* Utils.MillsPerSecond;
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

                if (!(list.Contains("no_update") || File.Exists(Path.Combine(Paths.DataPath, "no.update")))) {
                    CheckForUpdates();
                }

                Thread.Sleep(ExpectationDelay);

                splash.FadeOut();
                Left = screen.Bounds.Left;
                Top = screen.Bounds.Top;
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
                var str = executable.Key.ToString(CultureInfo.InvariantCulture);
                XmlNode node2 = null;
                var node = ((XmlNode) Host.Communication["SetupNode_" + str]).SelectSingleNode("DialogPositions");
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
                        var attribute = node2.Attributes["x"];
                        var attribute2 = node2.Attributes["y"];
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
                foreach (var obj2 in _loadables[interfaceName])
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


        public void InvokeNew(object sender) {
             NewMenuItemClick(sender, null);
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
                        var strArray = System.Windows.Forms.Clipboard.GetText()
                                                  .Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
                        var maxCols = 0;
                        Array.ForEach(strArray, delegate(string s) { maxCols = Math.Max(s.Split(new[] {','}).Length, maxCols); });
                        array = new byte[strArray.Length,maxCols];
                        Array.Clear(array, 0, array.Length);
                        for (var i = 0; i < strArray.Length; i++)
                        {
                            var strArray2 = strArray[i].Split(new[] {','});
                            for (var j = 0; j < strArray2.Length; j++)
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
                if ((value == null) || (value.Length <= 0)) {
                    return;
                }
                Host.Clipboard = value;
                var builder = new StringBuilder();
                for (var i = 0; i < value.GetLength(0); i++)
                {
                    for (var j = 0; j < value.GetLength(1); j++)
                    {
                        builder.AppendFormat("{0},", value[i, j]);
                    }
                    builder.Remove(builder.Length - 1, 1);
                    builder.AppendLine();
                }
                System.Windows.Forms.Clipboard.SetText(builder.ToString());
            }
        }

        public int ExecutingTimerCount
        {
            get { return _timerExecutor.ExecutingTimerCount; }
        }

        public string KnownFileTypesFilter { get; private set; }

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

        private void AddToFileHistory(string fileName)
        {
            var item = Path.GetFileName(fileName);
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
            if (activeMdiChild == null) {
                return;
            }
            var dialog = new DimmingCurveDialog(activeMdiChild.Sequence, null);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                activeMdiChild.IsDirty = true;
            }
        }

        private DialogResult CheckDirty(IUIPlugIn pluginInstance)
        {
            var none = DialogResult.None;
            if (pluginInstance.IsDirty)
            {
                var str = pluginInstance.Sequence.Name ?? "this unnamed sequence";
                none = MessageBox.Show(string.Format("[{0}]\nSave changes to {1}?", pluginInstance.FileTypeDescription, str),
                                       Vendor.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (none == DialogResult.Yes)
                {
                    Save(pluginInstance);
                }
            }
            return none;
        }

        //TODO Redo this whole scheme
        private static void CheckForUpdates()
        {
            var updateServerURI = Vendor.UpdateFile;
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var updateRootPath = string.Format("{0}/{1}.{2}", Vendor.UpdateFile, version.Major, version.Minor);
            var path = Path.Combine(Paths.DataPath, "target.update");
            if (File.Exists(path))
            {
                using (var reader = new StreamReader(path)) {
                    string updateLine;
                    while ((updateLine = reader.ReadLine()) != null)
                    {
                        var strArray = updateLine.Split(new[] {'='});
                        var parameter = strArray[0];
                        if (parameter == null) {
                            continue;
                        }
                        switch (parameter) {
                            case "server":
                                updateServerURI = strArray[1].Trim();
                                break;
                            case "root":
                                updateRootPath = strArray[1].Trim();
                                break;
                        }
                    }
                }
            }
            new AppUpdate.AppUpdate(updateServerURI, updateRootPath).ExecuteMigration();
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckForUpdates();
        }


        private void diagnosticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new DiagnosticsDialog(_timers).ShowDialog();
        }

        private static void Ensure(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private void FlushHistory()
        {
            var emptyNodeAlways = Xml.GetEmptyNodeAlways(_preferences.XmlDoc.DocumentElement, "History");
            foreach (var str in _history)
            {
                Xml.SetNewValue(emptyNodeAlways, "Item", str);
            }
            _preferences.SaveSettings();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var form in MdiChildren)
            {
                if ((!(form is IUIPlugIn)) || (CheckDirty((IUIPlugIn) form) != DialogResult.Cancel)) {
                    continue;
                }
                e.Cancel = true;
                return;
            }
            _host.StopBackgroundObjects();
            _host.BackgroundSequenceName = null;
            _preferences.SaveSettings();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            if (ActiveMdiChild == null) {
                return;
            }
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

        private static bool FormContainsChild(Form parent, Form child)
        {
            foreach (var form in parent.MdiChildren)
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
            var text = ((ToolStripItem) sender).Text;
            var path = Path.Combine(Paths.SequencePath, text);
            if (File.Exists(path))
            {
                OpenSequence(path);
            }
            else
            {
                _history.Remove(text);
                FlushHistory();
                LoadHistory();
                MessageBox.Show(Resources.VixenPlusForm_HistoryRemovalMsg, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //ComponentResourceManager manager = new ComponentResourceManager(typeof(VixenPlusForm));
        //this.toolStripStatusLabelMusic.Image = (Image)manager.GetObject("toolStripStatusLabelMusic.Image");
        //base.Icon = (Icon)manager.GetObject("$this.Icon");

/*
        private void loadableToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            var item = (ToolStripMenuItem) sender;
            foreach (ToolStripItem item2 in item.DropDownItems)
            {
                var loadedObject = item2.Tag as LoadedObject;
                if (loadedObject != null)
                {
                    var tag = loadedObject;
                    ((ToolStripMenuItem) item2).Checked = tag.Instance != null;
                }
            }
        }
*/

        private void LoadHistory()
        {
            recentToolStripMenuItem.DropDownItems.Clear();
            _history = new List<string>();
            var list = _preferences.XmlDoc.SelectNodes("//User/History/*");
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

        private void LoadUIPlugins()
        {
            if (!Directory.Exists(Paths.UIPluginPath)) {
                return;
            }
            foreach (var str in Directory.GetFiles(Paths.UIPluginPath, "*.dll"))
            {
                Exception exception;
                try
                {
                    var assembly = Assembly.LoadFile(str);
                    foreach (var exportedTypes in assembly.GetExportedTypes())
                    {
                        foreach (var interfaceTypes in exportedTypes.GetInterfaces()) {
                            if (interfaceTypes.Name != "IUIPlugIn") {
                                continue;
                            }
                            try
                            {
                                var inputPlugin = (IUIPlugIn) Activator.CreateInstance(exportedTypes);
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
                catch (BadImageFormatException)
                {
                }
                catch (Exception exception3)
                {
                    exception = exception3;
                    MessageBox.Show(string.Format("{0}:\n{1}", Path.GetFileName(str), exception.Message));
                }
            }
            foreach (var in2 in _registeredFileTypes.Values)
            {
                var item = newLightingProgramToolStripMenuItem.DropDownItems.Add(in2.FileTypeDescription);
                item.Tag = in2;
                item.Click += _newMenuItemClick;
            }
            var builder = new StringBuilder();
            foreach (var in2 in _registeredFileTypes.Values)
            {
                builder.AppendFormat("|{0}|*{1}", in2.FileTypeDescription, in2.FileExtension);
            }
            KnownFileTypesFilter = builder.ToString().Remove(0, 1);
        }

        private void PreferencesPreferenceChange(string preferenceName)
        {
            switch (preferenceName)
            {
                case "TimerCheckFrequency":
                    scheduleTimer.Interval = _preferences.GetInteger("TimerCheckFrequency")*1000;
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
                    if (_preferences.GetInteger("EventPeriod") < 25)
                    {
                        _preferences.SetInteger("EventPeriod", 25);
                        MessageBox.Show(
                            Resources.VixenPlusForm_EventPeriodMin,
                            Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    break;

                case "BackgroundMusicDelay":
                    if (_preferences.GetInteger("BackgroundMusicDelay") < 1)
                    {
                        MessageBox.Show(Resources.VixenPlusForm_DelayPeriodMin, Vendor.ProductName,
                                        MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        _preferences.SetInteger("BackgroundMusicDelay", 1);
                    }
                    break;

                case "BackgroundSequenceDelay":
                    if (_preferences.GetInteger("BackgroundSequenceDelay") < 1)
                    {
                        MessageBox.Show(Resources.VixenPlusForm_DelayPeriodMin, Vendor.ProductName,
                                        MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        _preferences.SetInteger("BackgroundSequenceDelay", 1);
                    }
                    break;

                case "ShutdownTime":
                    SetShutdownTime(_preferences.GetString("ShutdownTime"));
                    break;
            }
        }

/*
        private void manageProgramsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new ProgramManagerDialog();
            dialog.ShowDialog();
            dialog.Dispose();
        }
*/

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
                        MessageBox.Show(Resources.VixenPlusForm_NoWizardMsg, Vendor.ProductName,
                                        MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        tag.Sequence = tag.New();
                        break;

                    case DialogResult.OK:
                        tag.Sequence = tag.New(resultSequence);
                        if (!SaveAs(tag))
                        {
                            DialogResult = DialogResult.None;
                        }
                        break;

                    case DialogResult.Cancel:
                        return;
                }
            }
            else
            {
                tag.Sequence = tag.New();
            }

            if (tag.Sequence == null) {
                return;
            }

            var uiBase = tag as UIBase;
            if (uiBase != null)
            {
                uiBase.DirtyChanged += plugin_DirtyChanged;
            }
            tag.MdiParent = this;
            tag.Show();
        }

        private void NotifyAll(Notification notification)
        {
            foreach (var form in MdiChildren)
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

        public void openALightingProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var num = 0;
            var num2 = 1;
            var str = _preferences.GetString("PreferredSequenceType");
            foreach (var @in in _registeredFileTypes.Values)
            {
                if (str == @in.FileExtension)
                {
                    num = num2;
                    break;
                }
                num2++;
            }
            openFileDialog1.Filter = KnownFileTypesFilter;
            openFileDialog1.InitialDirectory = Paths.SequencePath;
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.FilterIndex = num;
            if (openFileDialog1.ShowDialog() != DialogResult.OK) {
                return;
            }
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

        public void OpenSequence(string fileName)
        {
            IUIPlugIn plugInInterface;
            //new XmlDocument();
            var extension = Path.GetExtension(fileName);
            if (extension != null && _registeredFileTypes.TryGetValue(extension.ToLower(), out plugInInterface))
            {
                AddToFileHistory(fileName);
                plugInInterface = (IUIPlugIn) Activator.CreateInstance(plugInInterface.GetType());
                plugInInterface.Sequence = plugInInterface.Open(fileName);
                var uiBase = plugInInterface as UIBase;
                if (uiBase != null)
                {
                    uiBase.DirtyChanged += plugin_DirtyChanged;
                }
                plugInInterface.MdiParent = this;
                plugInInterface.Show();
            }
            else
            {
                MessageBox.Show(Resources.VixenPlusForm_NoKnowEditor, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

        //private void programsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        //{
        //    manageProgramsToolStripMenuItem.Enabled = ((ISystem) Interfaces.Available["ISystem"]).ExecutingTimerCount == 0;
        //}

        private void programToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            var @in = ActiveMdiChild as IUIPlugIn;
            if (@in != null)
            {
                var activeMdiChild = @in;
                saveToolStripMenuItem.Text = !string.IsNullOrEmpty(activeMdiChild.Sequence.Name)
                                                 ? string.Format("{0} ({1})", Resources.VixenPlusForm_Save, activeMdiChild.Sequence.Name)
                                                 : Resources.VixenPlusForm_Save;
                channelDimmingCurvesToolStripMenuItem.Enabled = true;
            }
            else
            {
                saveToolStripMenuItem.Text = Resources.VixenPlusForm_Save;
                channelDimmingCurvesToolStripMenuItem.Enabled = false;
            }
        }

        private bool RegisterFileType(string fileExtension, IUIPlugIn inputPlugin)
        {
            IUIPlugIn plugInInterface;
            fileExtension = fileExtension.ToLower();
            if (_registeredFileTypes.TryGetValue(fileExtension, out plugInInterface))
            {
                return false;
            }
            _registeredFileTypes[fileExtension] = inputPlugin;
            return true;
        }

/*
        private void restartCurrentTimerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_timers.TimersDisabled)
            {
                MessageBox.Show(Resources.VixenPlusForm_ScheduleDisabledMsg, Vendor.ProductName, MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
            }
            else
            {
                SetTimerTraceFlag();
                var list = _timers.CurrentlyEffectiveTimers();
                if (list.Count == 0)
                {
                    MessageBox.Show(Resources.VixenPlusForm_NothingToDoMsg, Vendor.ProductName, MessageBoxButtons.OK,
                                    MessageBoxIcon.Asterisk);
                }
                else
                {
                    foreach (var timer in list)
                    {
                        _timerExecutor.SpawnExecutorFor(timer);
                    }
                    if (_timerExecutor.ExecutingTimerCount == 0)
                    {
                        MessageBox.Show(Resources.VixenPlusForm_TimersNotStarted_TooLong_OrError, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }
        }
*/

        private bool Save(IUIPlugIn pluginInstance)
        {
            if (pluginInstance == null)
            {
                return false;
            }
            var plugInInterface = pluginInstance;
            if ((plugInInterface.IsDirty && string.IsNullOrEmpty(plugInInterface.Sequence.Name)) &&
                !GetNewName(pluginInstance))
            {
                return false;
            }
            UpdateHistoryImages(plugInInterface.Sequence.FileName);
            plugInInterface.SaveTo(plugInInterface.Sequence.FileName);
            AddToFileHistory(plugInInterface.Sequence.FileName);
            if (_preferences.GetBoolean("ShowSaveConfirmation"))
            {
                MessageBox.Show(Resources.VixenPlusForm_SequenceSaved, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            return true;
        }

        private bool SaveAs(IUIPlugIn pluginInstance)
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

        //todo move to preferences
        private static void SetDataPath()
        {
            if (!File.Exists("redirect.data")) {
                return;
            }

            var path = "";
            using (var data = new StreamReader("redirect.data")) {
                path = data.ReadLine();
            }
            if (!String.IsNullOrEmpty(path) && Directory.Exists(path)) {
                Paths.DataPath = path;
            }
            else {
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
            if ((DateTime.Now.Hour != _shutdownAt.Hour) || (DateTime.Now.Minute != _shutdownAt.Minute)) {
                return;
            }
            shutdownTimer.Stop();
            Process.Start("shutdown", string.Format("/s /d P:4:1 /c \"Automatic shutdown by {0}\"", Vendor.ProductName));
            Thread.Sleep(1000);
            new ShutdownDialog().Show();
        }

        private void tileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SetTimerTraceFlag();
            foreach (var timer in _timers.StartingTimers())
            {
                _timerExecutor.SpawnExecutorFor(timer);
            }
        }

/*
        private void timersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new ScheduleDialog(_timers);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _timers = dialog.Timers;
                _timers.TimersDisabled = dialog.ScheduleDisabled;
                var contextNode = Xml.CreateXmlDocument();
                _timers.SaveToXml(contextNode);
                contextNode.Save(_timersPath);
                scheduleTimer.Enabled = !_timers.TimersDisabled;
            }
            dialog.Dispose();
        }
*/

        private void UpdateHistoryImages(string baseFilePath)
        {
            if (!File.Exists(baseFilePath)) {
                return;
            }
            var integer = _preferences.GetInteger("HistoryImages");
            if (integer == 0) {
                return;
            }
            var files = Directory.GetFiles(Paths.SequencePath, Path.GetFileName(baseFilePath) + ".bak*");
            var num2 = files.Length + 1;
            if (files.Length >= integer)
            {
                num2--;
                for (var i = 2; i <= integer; i++)
                {
                    File.Copy(string.Format("{0}.bak{1}", baseFilePath, i), string.Format("{0}.bak{1}", baseFilePath, i - 1), true);
                }
            }
            File.Copy(baseFilePath, string.Format("{0}.bak{1}", baseFilePath, num2), true);
        }

        //private void visualChannelLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    IExecutable executableObject;
        //    var mdi = ActiveMdiChild as IVixenMDI;
        //    if (/*ActiveMdiChild != null &&*/ mdi != null && mdi.Sequence != null)
        //    {
        //        executableObject = mdi.Sequence;
        //    }
        //    else
        //    {
        //        var str = _preferences.GetString("DefaultProfile");
        //        executableObject = (str.Length == 0) ? null : (new Profile(Path.Combine(Paths.ProfilePath, str + "." +  Vendor.ProfilExtension)));
        //    }
        //    if (executableObject == null)
        //    {
        //        MessageBox.Show(Resources.VixenPlusForm_NoOpenSequence, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        //    }
        //    else
        //    {
        //        var dialog = new ChannelLayoutDialog();
        //        dialog.ShowDialog();
        //        dialog.Dispose();
        //    }
        //}

        private delegate Form InstantiateFormDelegate(ConstructorInfo constructorInfo, params object[] parameters);

        private void iLikeLutefiskToolStripMenuItem_Click(object sender, EventArgs e) {
            using (var lutefisk = new Lutefisk()) {
                lutefisk.ShowDialog();
            }
        }
    }
}
