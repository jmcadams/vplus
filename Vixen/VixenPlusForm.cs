using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using Common;

using CommonControls;

using VixenPlus.Dialogs;
using VixenPlus.Properties;

namespace VixenPlus {
    internal sealed partial class VixenPlusForm : Form, ISystem {
        private List<string> _history;

        private DateTime _shutdownAt;

        private string _lastWindowsClipboardValue = "";

        private readonly Dictionary<string, IUIPlugIn> _registeredFileTypes;

        private EventHandler _historyItemClick;
        private EventHandler _newMenuItemClick;

        private readonly Host _host;

        private readonly Preference2 _preferences;

        private readonly Timers _timers;


        public VixenPlusForm(IEnumerable<string> args) {
            var startupArgs = args as IList<string> ?? args.ToList();
            SetDataPath();
            Ensure(Paths.DataPath);
            Ensure(Paths.SequencePath);
            Ensure(Paths.ProgramPath);
            Ensure(Paths.ImportExportPath);
            Ensure(Paths.AudioPath);
            Ensure(Paths.ProfilePath);
            Ensure(Paths.MapperPath);
            Ensure(Paths.NutcrackerDataPath);
            Ensure(Paths.RoutinePath);
            Ensure(Paths.CurveLibraryPath);
            _preferences = Preference2.GetInstance();
            PrepareUpdateSupportFiles();
            using (var splash = new Splash()) {
                var screen = Preference2.GetScreen(_preferences.GetString("PrimaryDisplay"));
                splash.FadeIn(screen);
                
                if (CheckForUpdates(screen, true)) {
                    Environment.Exit(0);
                } 
                
                InitializeComponent();
                Icon = Resources.VixenPlus;
                SetVendorData();
                _registeredFileTypes = new Dictionary<string, IUIPlugIn>();
                var timersPath = Path.Combine(Paths.DataPath, "timers");
                _preferences.PreferenceChange += PreferencesPreferenceChange;
                _host = new Host(this);
                //_loadables = new Dictionary<string, List<LoadedObject>>();
                Interfaces.Available["ISystem"] = this;
                Interfaces.Available["IExecution"] = new ExecutionImpl(_host);
                _newMenuItemClick = NewMenuItemClick;
                _historyItemClick = HistoryItemClick;
                LoadHistory();
                var loadableData = new LoadableData();
                loadableData.LoadFromXml(_preferences.XmlDoc.DocumentElement);
                LoadUIPlugins();
                Cursor = Cursors.WaitCursor;
                try {
                    foreach (var sequence in startupArgs.Where(File.Exists)) {
                        OpenSequence(sequence);
                    }
                }
                finally {
                    Cursor = Cursors.Default;
                }
                _timers = new Timers();
                if (File.Exists(timersPath)) {
                    _timers.LoadFromXml(Xml.LoadDocument(timersPath));
                }
                if (_preferences.GetBoolean("EnableBackgroundSequence")) {
                    _host.BackgroundSequenceName = _preferences.GetString("BackgroundSequence");
                }
                _host.StartBackgroundObjects();
                SetShutdownTime(_preferences.GetString("ShutdownTime"));

                splash.FadeOut();
                Left = screen.Bounds.Left;
                Top = screen.Bounds.Top;
            }
        }


        private static void PrepareUpdateSupportFiles() {
            if (File.Exists(Vendor.UpdateSupport7zrProtected)) {
                if (File.Exists(Vendor.UpdateSupport7zrReal)) {
                    File.Delete(Vendor.UpdateSupport7zrReal);
                }
                File.Move(Vendor.UpdateSupport7zrProtected, Vendor.UpdateSupport7zrReal);
            }

            if (!File.Exists(Vendor.UpdateSupportBatchProtected)) {
                return;
            }

            if (File.Exists(Vendor.UpdateSupportBatchReal)) {
                File.Delete(Vendor.UpdateSupportBatchReal);
            }
            File.Move(Vendor.UpdateSupportBatchProtected, Vendor.UpdateSupportBatchReal);
        }


        private static bool CheckForUpdates(Screen startupScreen, bool isInStartup) {
            var result = false;
            using (var dialog = new UpdateDialog(startupScreen, isInStartup)) {
                if (isInStartup && !dialog.IsTimeToCheckForUpdate()) {
                    return false;
                }

                dialog.ShowDialog();
                
                switch (dialog.DialogResult) {
                    case DialogResult.Yes:
                        result = true;
                        break;
                }
            }
            return result;
        }



        public Form InstantiateForm(ConstructorInfo constructorInfo, params object[] parameters) {
            if (InvokeRequired) {
                return (Form) Invoke(new InstantiateFormDelegate(InstantiateForm), new object[] {constructorInfo, parameters});
            }
            var child = (Form) constructorInfo.Invoke(parameters);
            if (child == null) {
                return null;
            }
            if (!(child is OutputPlugInUIBase)) {
                return null;
            }
            var base2 = (OutputPlugInUIBase) child;
            var executable = (IExecutable) Host.Communication["CurrentObject"];
            if (executable == null) {
                return child;
            }
            var str = executable.Key.ToString(CultureInfo.InvariantCulture);
            XmlNode node2 = null;
            var node = ((XmlNode) Host.Communication["SetupNode_" + str]).SelectSingleNode("DialogPositions");
            object obj2;
            if (Host.Communication.TryGetValue("KeyInterceptor_" + str, out obj2)) {
                base2.ExecutionParent = (IVixenMDI) obj2;
            }
            if (Host.Communication.TryGetValue("SetupNode_" + str, out obj2)) {
                base2.DataNode = (XmlNode) obj2;
            }
            child.ControlBox = true;
            if (!FormContainsChild(this, child)) {
                child.MdiParent = this;
            }
            ((ExecutionContext) Host.Communication["ExecutionContext_" + str]).OutputPlugInForms.Add(child);
            child.Show();
            if (node != null) {
                node2 = node.SelectSingleNode(child.Name);
            }
            if ((node2 == null) || !_preferences.GetBoolean("SavePlugInDialogPositions")) {
                return child;
            }
            if (node2.Attributes == null) {
                return child;
            }
            var attribute = node2.Attributes["x"];
            var attribute2 = node2.Attributes["y"];
            if ((attribute != null) && (attribute2 != null)) {
                child.Location = new Point(Convert.ToInt32(attribute.Value), Convert.ToInt32(attribute2.Value));
            }
            return child;
        }


        public void InvokeSave(UIBase pluginInstance) {
            Save(pluginInstance);
        }


        public void VerifySequenceHardwarePlugins(EventSequence sequence) {
            OutputPlugins.VerifyPlugIns(sequence);
            InputPlugins.VerifyPlugIns(sequence);
        }


        public void InvokeNew(object sender) {
            NewMenuItemClick(sender, null);
        }

        public byte[,] Clipboard {
            get {
                byte[,] array = null;
                if (System.Windows.Forms.Clipboard.ContainsText()) {
                    try {
                        if (_lastWindowsClipboardValue == System.Windows.Forms.Clipboard.GetText()) {
                            return Host.Clipboard;
                        }
                        var strArray = System.Windows.Forms.Clipboard.GetText().Split(new[] {Environment.NewLine},
                            StringSplitOptions.RemoveEmptyEntries);
                        var maxCols = 0;
                        Array.ForEach(strArray, delegate(string s) { maxCols = Math.Max(s.Split(new[] {','}).Length, maxCols); });
                        array = new byte[strArray.Length,maxCols];
                        Array.Clear(array, 0, array.Length);
                        for (var i = 0; i < strArray.Length; i++) {
                            var strArray2 = strArray[i].Split(new[] {','});
                            for (var j = 0; j < strArray2.Length; j++) {
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
                _lastWindowsClipboardValue = System.Windows.Forms.Clipboard.GetText();
                return array;
            }
            set {
                if ((value == null) || (value.Length <= 0)) {
                    return;
                }
                Host.Clipboard = value;
                var builder = new StringBuilder();
                for (var i = 0; i < value.GetLength(0); i++) {
                    for (var j = 0; j < value.GetLength(1); j++) {
                        builder.AppendFormat("{0},", value[i, j]);
                    }
                    builder.Remove(builder.Length - 1, 1);
                    builder.AppendLine();
                }
                System.Windows.Forms.Clipboard.SetText(builder.ToString());
            }
        }

        private string KnownFileTypesFilter { get; set; }

        public Preference2 UserPreferences {
            get { return _preferences; }
        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            using (var aboutDialog = new AboutDialog()) {
                aboutDialog.ShowDialog();
            }
        }


        private void AddToFileHistory(string fileName) {
            var item = Path.GetFileName(fileName);
            _history.Remove(item);
            _history.Insert(0, item);
            var maxCount = _preferences.GetInteger("RecentFiles");
            while (_history.Count > maxCount) {
                _history.RemoveAt(_history.Count - 1);
            }
            FlushHistory();
            LoadHistory();
        }


        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e) {
            LayoutMdi(MdiLayout.Cascade);
        }


        private void ChangeSequenceName(IUIPlugIn pluginInstance, string newName) {
            if (!newName.EndsWith(pluginInstance.FileExtension)) {
                newName = newName + pluginInstance.FileExtension;
            }
            pluginInstance.Sequence.Name = newName;
            saveToolStripMenuItem.Text = string.Format("Save ({0})", pluginInstance.Sequence.Name);
            ((Form) pluginInstance).Text = pluginInstance.Sequence.Name;
        }


        private void channelDimmingCurvesToolStripMenuItem_Click(object sender, EventArgs e) {
            var activeMdiChild = ActiveMdiChild as IUIPlugIn;
            if (activeMdiChild == null) {
                return;
            }
            var dialog = new DimmingCurveDialog(activeMdiChild.Sequence, null);
            if (dialog.ShowDialog() == DialogResult.OK) {
                activeMdiChild.IsDirty = true;
            }
        }


        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e) {
            if (CheckForUpdates(Preference2.GetScreen(_preferences.GetString("PrimaryDisplay")), false)) {
                Close();
            }
        }


        private void diagnosticsToolStripMenuItem_Click(object sender, EventArgs e) {
            new DiagnosticsDialog(_timers).ShowDialog();
        }


        private static void Ensure(string path) {
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
        }


        private void FlushHistory() {
            var emptyNodeAlways = Xml.GetEmptyNodeAlways(_preferences.XmlDoc.DocumentElement, "History");
            foreach (var str in _history) {
                Xml.SetNewValue(emptyNodeAlways, "Item", str);
            }
            _preferences.SaveSettings();
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            if (MdiChildren.Any(form => (form is IUIPlugIn) && (CheckDirty((IUIPlugIn) form) == DialogResult.Cancel))) {
                e.Cancel = true;
                return;
            }

            _host.StopBackgroundObjects();
            _host.BackgroundSequenceName = null;
            _preferences.SaveSettings();
            _historyItemClick = null;
            _newMenuItemClick = null;
        }

        private DialogResult CheckDirty(IUIPlugIn pluginInstance) {
            var none = DialogResult.None;
            if (!pluginInstance.IsDirty) {
                return none;
            }
            var str = pluginInstance.Sequence.Name ?? "this unnamed sequence";
            none = MessageBox.Show(string.Format("[{0}]\nSave changes to {1}?", pluginInstance.FileTypeDescription, str), Vendor.ProductName,
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (none == DialogResult.Yes) {
                Save(pluginInstance);
            }
            return none;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            if (ActiveMdiChild == null) {
                return;
            }
            if ((ActiveMdiChild is OutputPlugInUIBase) && (((OutputPlugInUIBase) ActiveMdiChild).ExecutionParent != null)) {
                ((OutputPlugInUIBase) ActiveMdiChild).ExecutionParent.Notify(Notification.KeyDown, e);
            }
            else {
                var activeMdiChild = ActiveMdiChild as IVixenMDI;
                if (activeMdiChild != null) {
                    activeMdiChild.Notify(Notification.KeyDown, e);
                }
            }
        }


        private void Form1_MdiChildActivate(object sender, EventArgs e) {
            if (ActiveMdiChild is IUIPlugIn) {
                saveToolStripMenuItem.Enabled = (ActiveMdiChild as IUIPlugIn).IsDirty;
                saveAsToolStripMenuItem.Enabled = true;
            }
            else {
                saveToolStripMenuItem.Enabled = false;
                saveAsToolStripMenuItem.Enabled = false;
            }
        }


        private static bool FormContainsChild(Form parent, Form child) {
            return parent.MdiChildren.Any(form => form == child);
        }


        private bool GetNewName(IUIPlugIn pluginInstance) {
            saveFileDialog1.Filter = string.Format("{0}|*{1}", pluginInstance.FileTypeDescription, pluginInstance.FileExtension);
            saveFileDialog1.InitialDirectory = Paths.SequencePath;
            saveFileDialog1.FileName = string.Empty;
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) {
                return false;
            }
            ChangeSequenceName(pluginInstance, saveFileDialog1.FileName);
            return true;
        }


        private void HistoryItemClick(object sender, EventArgs e) {
            var text = ((ToolStripItem) sender).Text;
            var path = Path.Combine(Paths.SequencePath, text);
            if (File.Exists(path)) {
                OpenSequence(path);
            }
            else {
                _history.Remove(text);
                FlushHistory();
                LoadHistory();
                MessageBox.Show(Resources.VixenPlusForm_HistoryRemovalMsg, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        private void LoadHistory() {
            recentToolStripMenuItem.DropDownItems.Clear();
            _history = new List<string>();
            var list = _preferences.XmlDoc.SelectNodes("//User/History/*");
            var maxCount = _preferences.GetInteger("RecentFiles");
            var currentCount = 0;
            if (list != null) {
                foreach (XmlNode node in list) {
                    currentCount++;
                    if (currentCount > maxCount) {
                        continue;
                    }
                    _history.Add(node.InnerText);
                    recentToolStripMenuItem.DropDownItems.Add(node.InnerText, null, _historyItemClick);
                }
            }
            recentToolStripMenuItem.Enabled = recentToolStripMenuItem.DropDownItems.Count > 0;
        }


        private void LoadUIPlugins() {
            if (!Directory.Exists(Paths.UIPluginPath)) {
                return;
            }

            foreach (var str in Directory.GetFiles(Paths.UIPluginPath, Vendor.All + Vendor.AppExtension)) {
                Exception exception;
                try {
                    var assembly = Assembly.LoadFile(str);
                    foreach (var exportedTypes in 
                        from exportedTypes in assembly.GetExportedTypes() 
                        from interfaceTypes in exportedTypes.GetInterfaces().Where(interfaceTypes => interfaceTypes.Name == "IUIPlugIn") 
                        select exportedTypes) {
                        try {
                            var inputPlugin = (IUIPlugIn) Activator.CreateInstance(exportedTypes);
                            if (!RegisterFileType(inputPlugin.FileExtension, inputPlugin)) {
                                MessageBox.Show(
                                    string.Format("Could not register UI plugin {0}.\nFile type is already handled.", inputPlugin.Name),
                                    Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                        catch (Exception exception1) {
                            exception = exception1;
                            MessageBox.Show(
                                string.Format("Error when loading UI plugin from {0}:\n{1}", Path.GetFileNameWithoutExtension(str),
                                    exception.StackTrace), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
                catch (BadImageFormatException) {}
                catch (Exception exception3) {
                    exception = exception3;
                    MessageBox.Show(string.Format("{0}:\n{1}", Path.GetFileName(str), exception.Message));
                }
            }
            foreach (var in2 in _registeredFileTypes.Values) {
                var item = newLightingProgramToolStripMenuItem.DropDownItems.Add(in2.FileTypeDescription);
                item.Tag = in2;
                item.Click += _newMenuItemClick;
            }
            var builder = new StringBuilder();
            foreach (var in2 in _registeredFileTypes.Values) {
                builder.AppendFormat("|{0}|*{1}", in2.FileTypeDescription, in2.FileExtension);
            }
            KnownFileTypesFilter = builder.ToString().Remove(0, 1);
        }


        private void PreferencesPreferenceChange(string preferenceName) {
            switch (preferenceName) {
                case "EnableBackgroundSequence":
                    if (!_preferences.GetBoolean("EnableBackgroundSequence")) {
                        _host.StopBackgroundSequence();
                        break;
                    }
                    _host.BackgroundSequenceName = _preferences.GetString("BackgroundSequence");
                    _host.StartBackgroundSequence();
                    break;

                case "EnableBackgroundMusic":
                    if (!_preferences.GetBoolean("EnableBackgroundMusic")) {
                        _host.StopBackgroundMusic();
                        break;
                    }
                    _host.StartBackgroundMusic();
                    break;

                case "EventPeriod":
                    if (_preferences.GetInteger("EventPeriod") < 25) {
                        _preferences.SetInteger("EventPeriod", 25);
                        MessageBox.Show(Resources.VixenPlusForm_EventPeriodMin, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    break;

                case "BackgroundMusicDelay":
                    if (_preferences.GetInteger("BackgroundMusicDelay") < 1) {
                        MessageBox.Show(Resources.VixenPlusForm_DelayPeriodMin, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        _preferences.SetInteger("BackgroundMusicDelay", 1);
                    }
                    break;

                case "BackgroundSequenceDelay":
                    if (_preferences.GetInteger("BackgroundSequenceDelay") < 1) {
                        MessageBox.Show(Resources.VixenPlusForm_DelayPeriodMin, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        _preferences.SetInteger("BackgroundSequenceDelay", 1);
                    }
                    break;

                case "ShutdownTime":
                    SetShutdownTime(_preferences.GetString("ShutdownTime"));
                    break;
            }
        }


        private void manageToolStripMenuItem_Click(object sender, EventArgs e) {
            using (var dialog = new FrmProfileManager()) {
            //using (var dialog = new ProfileManagerDialog(null)) {
                if (dialog.ShowDialog() == DialogResult.OK) {
            //        NotifyAll(Notification.ProfileChange);
                }
            }
        }


        public void InvokeGroupChange(object data) {
            NotifyAll(Notification.GroupChange, data);
        }

        private void musicPlayerToolStripMenuItem_Click(object sender, EventArgs e) {
            _host.MusicPlayer.ShowDialog();
        }


        private void NewMenuItemClick(object sender, EventArgs e) {
            var item = (ToolStripItem) sender;
            if (!(item.Tag is IUIPlugIn)) {
                return;
            }

            var tag = (IUIPlugIn) item.Tag;
            var instance = (IUIPlugIn) Activator.CreateInstance(tag.GetType());
            instance.Sequence = null;
            if (_preferences.GetBoolean("WizardForNewSequences")) {
                EventSequence resultSequence = null;
                switch (instance.RunWizard(ref resultSequence)) {
                    case DialogResult.None:
                        MessageBox.Show(Resources.VixenPlusForm_NoWizardMsg, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        instance.Sequence = instance.New();
                        break;

                    case DialogResult.OK:
                        instance.Sequence = instance.New(resultSequence);
                        if (!SaveAs(instance)) {
                            DialogResult = DialogResult.None;
                        }
                        break;

                    case DialogResult.Cancel:
                        return;
                }
            }
            else {
                instance.Sequence = instance.New();
            }

            if (instance.Sequence == null) {
                return;
            }

            var uiBase = instance as UIBase;
            if (uiBase != null) {
                uiBase.DirtyChanged += plugin_DirtyChanged;
                uiBase.IsDirty = DialogResult == DialogResult.None;
            }
            instance.MdiParent = this;
            instance.Show();
        }


        private void NotifyAll(Notification notification, object data = null) {
            foreach (var vixenMdi in MdiChildren.OfType<IVixenMDI>()) {
                vixenMdi.Notify(notification, data);
            }
        }




        private void onlineSupportForumToolStripMenuItem_Click(object sender, EventArgs e) {
            var process = new Process {StartInfo = {FileName = Vendor.SupportURL, UseShellExecute = true}};
            process.Start();
        }


        private void openALightingProgramToolStripMenuItem_Click(object sender, EventArgs e) {
            var filterIndex = 0;
            var filterIndexCount = 1;
            var preferredType = _preferences.GetString("PreferredSequenceType");
            foreach (var thisType in _registeredFileTypes.Values) {
                if (preferredType == thisType.FileExtension) {
                    filterIndex = filterIndexCount;
                    break;
                }
                filterIndexCount++;
            }
            openFileDialog1.Filter = KnownFileTypesFilter;
            openFileDialog1.InitialDirectory = Paths.SequencePath;
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.FilterIndex = filterIndex;
            if (openFileDialog1.ShowDialog() != DialogResult.OK) {
                return;
            }
            Cursor = Cursors.WaitCursor;
            try {
                OpenSequence(openFileDialog1.FileName);
            }
            finally {
                Cursor = Cursors.Default;
            }
        }


        private void OpenSequence(string fileName) {
            IUIPlugIn plugInInterface;
            //new XmlDocument();
            var extension = Path.GetExtension(fileName);
            if (extension != null && _registeredFileTypes.TryGetValue(extension.ToLower(), out plugInInterface)) {
                AddToFileHistory(fileName);
                plugInInterface = (IUIPlugIn) Activator.CreateInstance(plugInInterface.GetType());
                plugInInterface.Sequence = plugInInterface.Open(fileName);
                var uiBase = plugInInterface as UIBase;
                if (uiBase != null) {
                    uiBase.DirtyChanged += plugin_DirtyChanged;
                }
                plugInInterface.MdiParent = this;
                plugInInterface.Show();
            }
            else {
                MessageBox.Show(Resources.VixenPlusForm_NoKnowEditor, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }


        private void plugin_DirtyChanged(object sender, EventArgs e) {
            var uiPlugIn = sender as IUIPlugIn;
            if (uiPlugIn != null) {
                saveToolStripMenuItem.Enabled = uiPlugIn.IsDirty;
            }
        }


        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e) {
            var plugIns = new IUIPlugIn[_registeredFileTypes.Values.Count];
            _registeredFileTypes.Values.CopyTo(plugIns, 0);
            using (var preferencesDialog = new PreferencesDialog(plugIns)) {
                if (preferencesDialog.ShowDialog() != DialogResult.OK) {
                    return;
                }

                _preferences.Reload();
                NotifyAll(Notification.PreferenceChange);
            }
        }


        private void programToolStripMenuItem_DropDownOpening(object sender, EventArgs e) {
            var plugIn = ActiveMdiChild as IUIPlugIn;
            if (plugIn != null) {
                var activeMdiChild = plugIn;
                saveToolStripMenuItem.Text = !string.IsNullOrEmpty(activeMdiChild.Sequence.Name)
                    ? string.Format("{0} ({1})", Resources.VixenPlusForm_Save, activeMdiChild.Sequence.Name)
                    : Resources.VixenPlusForm_Save;
                channelDimmingCurvesToolStripMenuItem.Enabled = true;
            }
            else {
                saveToolStripMenuItem.Text = Resources.VixenPlusForm_Save;
                channelDimmingCurvesToolStripMenuItem.Enabled = false;
            }
        }


        private bool RegisterFileType(string fileExtension, IUIPlugIn inputPlugin) {
            IUIPlugIn plugInInterface;
            fileExtension = fileExtension.ToLower();
            if (_registeredFileTypes.TryGetValue(fileExtension, out plugInInterface)) {
                return false;
            }
            _registeredFileTypes[fileExtension] = inputPlugin;
            return true;
        }


        private bool Save(IUIPlugIn pluginInstance) {
            if (pluginInstance == null) {
                return false;
            }
            var plugInInterface = pluginInstance;
            if ((plugInInterface.IsDirty && string.IsNullOrEmpty(plugInInterface.Sequence.Name)) && !GetNewName(pluginInstance)) {
                return false;
            }
            UpdateHistoryImages(plugInInterface.Sequence.FileName);
            plugInInterface.SaveTo(plugInInterface.Sequence.FileName);
            AddToFileHistory(plugInInterface.Sequence.FileName);
            if (_preferences.GetBoolean("ShowSaveConfirmation")) {
                MessageBox.Show(Resources.VixenPlusForm_SequenceSaved, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            return true;
        }


        private bool SaveAs(IUIPlugIn pluginInstance) {
            return (GetNewName(pluginInstance) && Save(pluginInstance));
        }


        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveAs((UIBase) ActiveMdiChild);
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            Save((UIBase) ActiveMdiChild);
        }

        //TODO this can be removed, for now yes. (1/26/2014)
        private void setBackgroundSequenceToolStripMenuItem_Click(object sender, EventArgs e) {
            using (var backgroundSequenceDialog = new BackgroundSequenceDialog(_preferences.GetString("BackgroundSequence"), Paths.SequencePath)) {
                if (backgroundSequenceDialog.ShowDialog() != DialogResult.OK) {
                    return;
                }

                _preferences.SetString("BackgroundSequence", backgroundSequenceDialog.BackgroundSequenceFileName);
                _host.BackgroundSequenceName = backgroundSequenceDialog.BackgroundSequenceFileName;
            }
        }



        private static void SetDataPath() {
            CheckIfFirstRun();

            if (!File.Exists(Paths.DataDir)) {
                throw new FileNotFoundException(string.Format("Delete the {0} file and try restarting {1} or allow {1} to create one.",Paths.DataDir,Vendor.ProductName));
            }

            string path;
            using (var data = new StreamReader(Paths.DataDir)) {
                path = data.ReadLine();
            }

            if (!String.IsNullOrEmpty(path)) {
                path = Environment.ExpandEnvironmentVariables(path);
                if (!Directory.Exists(path)) {
                    return;
                }
                Paths.DataPath = path;
                return;
            }

            Paths.DataPath = Path.Combine(Paths.BinaryPath, Paths.DataFolder);
        }


        private static void CheckIfFirstRun() {
            if (File.Exists(Paths.DataDir)) {
                return;
            }

            using (var firstRunPath = new FirstRunPathDialog(true)) {
                firstRunPath.ShowDialog();
            }
        }


        private void SetShutdownTime(string time) {
            if (time == string.Empty) {
                shutdownTimer.Stop();
            }
            else if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "shutdown.exe"))) {
                _shutdownAt = DateTime.Parse(time);
                shutdownTimer.Start();
            }
        }


        private void setSoundDeviceToolStripMenuItem_Click(object sender, EventArgs e) {
            using (var soundDeviceDialog = new SoundDeviceDialog(_preferences)) {
                soundDeviceDialog.ShowDialog();
            }
        }


        private void SetVendorData() {
            Text = Vendor.ProductName + @" (" + Vendor.ModuleAuthoring + @")";
        }


        private void shutdownTimer_Tick(object sender, EventArgs e) {
            if ((DateTime.Now.Hour != _shutdownAt.Hour) || (DateTime.Now.Minute != _shutdownAt.Minute)) {
                return;
            }
            shutdownTimer.Stop();
            Process.Start("shutdown", string.Format("/s /d P:4:1 /c \"Automatic shutdown by {0}\"", Vendor.ProductName));
            Thread.Sleep(1000);
            new ShutdownDialog().Show();
        }


        private void tileToolStripMenuItem_Click(object sender, EventArgs e) {
            LayoutMdi(MdiLayout.TileHorizontal);
        }


        private void UpdateHistoryImages(string baseFilePath) {
            if (!File.Exists(baseFilePath)) {
                return;
            }
            var maxBackupCount = _preferences.GetInteger("HistoryImages");
            if (maxBackupCount == 0) {
                return;
            }
            var backupFiles = Directory.GetFiles(Paths.SequencePath, Path.GetFileName(baseFilePath) + ".bak*");
            var nextBackupCount = backupFiles.Length + 1;
            if (backupFiles.Length >= maxBackupCount) {
                nextBackupCount--;
                for (var i = 2; i <= maxBackupCount; i++) {
                    File.Copy(string.Format("{0}.bak{1}", baseFilePath, i), string.Format("{0}.bak{1}", baseFilePath, i - 1), true);
                }
            }
            File.Copy(baseFilePath, string.Format("{0}.bak{1}", baseFilePath, nextBackupCount), true);
        }


        private delegate Form InstantiateFormDelegate(ConstructorInfo constructorInfo, params object[] parameters);


        private void iLikeLutefiskToolStripMenuItem_Click(object sender, EventArgs e) {
            using (var lutefisk = new Lutefisk()) {
                lutefisk.ShowDialog();
            }
        }

        private void VixenPlusForm_DragEnter(object sender, DragEventArgs e) {
            var validFileTypes = (e.Data.GetDataPresent(DataFormats.FileDrop));
            
            if (validFileTypes) {
                var files = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
                if (files.Any(f => f.Substring(f.Length - 4, 4) != Vendor.SequenceExtension)) {
                    validFileTypes = false;
                }
            }

            e.Effect = validFileTypes ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void VixenPlusForm_DragDrop(object sender, DragEventArgs e) {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (var f in files) {
                OpenSequence(f);
            }
        }
    }
}