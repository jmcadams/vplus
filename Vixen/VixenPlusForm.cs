using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using VixenPlus.Dialogs;
using VixenPlus.Properties;

using VixenPlusCommon;

using common = VixenPlusCommon.Properties;

namespace VixenPlus {
    internal sealed partial class VixenPlusForm : Form, ISystem {
        private List<string> _history;

        private DateTime _shutdownAt;

        private string _lastWindowsClipboardValue = "";

        private readonly Dictionary<string, IUIPlugIn> _registeredFileTypes;

        private EventHandler _historyItemClick;
        //private EventHandler _newMenuItemClick;

        private readonly Preference2 _preferences;

        public VixenPlusForm(IEnumerable<string> args) {
            var startupArgs = args as IList<string> ?? args.ToList();
            SetDataPath();
            Ensure(Paths.DataPath);
            Ensure(Paths.SequencePath);
            Ensure(Paths.ImportExportPath);
            Ensure(Paths.AudioPath);
            Ensure(Paths.ProfilePath);
            Ensure(Paths.SupportingFilesPath);
            Ensure(Paths.ProfileGeneration);
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
                Icon = common.Resources.VixenPlus;
                SetVendorData();
                _registeredFileTypes = new Dictionary<string, IUIPlugIn>();
                _preferences.PreferenceChange += PreferencesPreferenceChange;
                Interfaces.Available["ISystem"] = this;
                Interfaces.Available["IExecution"] = new ExecutionImpl(new Host(this));
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
                return (Form)Invoke(new InstantiateFormDelegate(InstantiateForm), constructorInfo, parameters);
            }
            var child = (Form)constructorInfo.Invoke(parameters);
            if (!(child is OutputPlugInUIBase)) {
                return null;
            }
            var outputPlugInUIBase = (OutputPlugInUIBase)child;
            var executable = (IExecutable)Host.Communication["CurrentObject"];
            if (executable == null) {
                return child;
            }
            
            var str = executable.Key.ToString(CultureInfo.InvariantCulture);
            object obj2;
            if (Host.Communication.TryGetValue("KeyInterceptor_" + str, out obj2)) {
                outputPlugInUIBase.ExecutionParent = (IVixenMDI) obj2;
            }
            if (Host.Communication.TryGetValue("SetupNode_" + str, out obj2)) {
                outputPlugInUIBase.DataNode = (XmlNode) obj2;
            }
            child.ControlBox = true;
            ((ExecutionContext) Host.Communication["ExecutionContext_" + str]).OutputPlugInForms.Add(child);
            child.Show();

            return child;
        }


        public void InvokeSave(UIBase pluginInstance) {
            Save(pluginInstance);
        }


        public void InvokeSaveAs(UIBase pluginInstance) {
            SaveAs(pluginInstance);
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
                        Array.ForEach(strArray, delegate(string s) { maxCols = Math.Max(s.Split(',').Length, maxCols); });
                        array = new byte[strArray.Length,maxCols];
                        Array.Clear(array, 0, array.Length);
                        for (var i = 0; i < strArray.Length; i++) {
                            var strArray2 = strArray[i].Split(',');
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

        //private string KnownFileTypesFilter { get; set; }

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
            var fileExt = pluginInstance.Sequence.FileIOHandler.FileExtension();
            if (!newName.EndsWith(fileExt)) {
                newName = newName + fileExt;
            }
            pluginInstance.Sequence.Name = newName;
            saveToolStripMenuItem.Text = string.Format("Save ({0})", pluginInstance.Sequence.Name);
            ((Form) pluginInstance).Text = pluginInstance.Sequence.Name + " - " + pluginInstance.Sequence.FileIOHandler.Name();
        }


        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e) {
            if (CheckForUpdates(Preference2.GetScreen(_preferences.GetString("PrimaryDisplay")), false)) {
                Close();
            }
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

            _preferences.SaveSettings();
            _historyItemClick = null;
        }

        private DialogResult CheckDirty(IUIPlugIn pluginInstance) {
            var none = DialogResult.None;
            if (!pluginInstance.IsDirty) {
                return none;
            }
            var str = pluginInstance.Sequence.Name ?? "this unnamed sequence";
            none = MessageBox.Show(string.Format("Save changes to {0}?", str), Vendor.ProductName,
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
            var activeChild = ActiveMdiChild as OutputPlugInUIBase;
            if ((activeChild != null) && (activeChild.ExecutionParent != null)) {
                activeChild.ExecutionParent.Notify(Notification.KeyDown, e);
            }
            else {
                var activeMdiChild = ActiveMdiChild as IVixenMDI;
                if (activeMdiChild != null) {
                    activeMdiChild.Notify(Notification.KeyDown, e);
                }
            }
        }


        private void Form1_MdiChildActivate(object sender, EventArgs e) {
            var activeChild = ActiveMdiChild as IUIPlugIn;
            if (activeChild != null) {
                saveToolStripMenuItem.Enabled = activeChild.IsDirty;
                saveAsToolStripMenuItem.Enabled = true;
            }
            else {
                saveToolStripMenuItem.Enabled = false;
                saveAsToolStripMenuItem.Enabled = false;
            }
        }


        private bool GetNewSequenceInfo(IUIPlugIn pluginInstance) {
            var saveFilters = FileIOHelper.GetSaveFilters();
            var filters = saveFilters.Split('|');
            var currentFilter = pluginInstance.Sequence.FileIOHandler.DialogFilterList();
            var currentFilterIndex = int.MinValue;
            for (var i = 0; i < filters.Count(); i += 2) {
                if (!currentFilter.StartsWith(filters[i])) {
                    continue;
                }
                currentFilterIndex = i / 2;
                break;
            }

            saveFileDialog1.Filter = saveFilters;
            saveFileDialog1.FilterIndex = currentFilterIndex == int.MinValue ? 0 : currentFilterIndex;
            saveFileDialog1.InitialDirectory = Paths.SequencePath;
            saveFileDialog1.FileName = String.IsNullOrEmpty(pluginInstance.Sequence.FileName) ? string.Empty : Path.GetFileNameWithoutExtension(pluginInstance.Sequence.FileName);
            saveFileDialog1.AddExtension = true;
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) {
                return false;
            }

            var newFilterIndex = saveFileDialog1.FilterIndex - 1;

            // Okay format changed...
            if (currentFilterIndex != newFilterIndex) {

                var newFileIOHandler = FileIOHelper.GetHelperByName(filters[newFilterIndex * 2]);

                // Since Vixen+ is native, it has the lowest filter index of any of the list
                // Other file formats will be higher and thus may lose data or functionality when down versioning
                // or cross sequence formatting.  In some cases it won't matter, in other it will be very important.
                if (newFilterIndex > currentFilterIndex) {
                    if (
                        MessageBox.Show(
                            string.Format("{0}\n\nOld Format: {1}\nNewFormat: {2}\n\n{3}\n\n{4}\n\n{5}",
                                "WARNING! A possible loss of data may occur from this change!",
                                pluginInstance.Sequence.FileIOHandler.Name(), 
                                newFileIOHandler.Name(),
                                "This will impact your sequence and " + ((pluginInstance.Sequence.Profile == null ? "embedded" : "attached") + " profile"),
                                "Proceed with saving in this format?",
                                "In other words, press OK if you have a backup.")
                        , "Possible loss of data!",
                        MessageBoxButtons.OKCancel, 
                        MessageBoxIcon.Warning, 
                        MessageBoxDefaultButton.Button2) == DialogResult.Cancel) 
                    {

                        return false;
                    }
                }

                //Okay they want the change, get the new handler and make it so

                pluginInstance.Sequence.FileIOHandler = newFileIOHandler; // this automatically handles embedded data
                
                // he we handle profiles.
                if (pluginInstance.Sequence.Profile != null) {
                    pluginInstance.Sequence.Profile.FileIOHandler = newFileIOHandler;
                    if (newFileIOHandler.SupportsProfiles) {
                        newFileIOHandler.SaveProfile(pluginInstance.Sequence.Profile);
                    }
                }
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
                            if (!RegisterFileType(".vix", inputPlugin)) {
                                MessageBox.Show(
                                    string.Format("Could not register UI plugin {0}.\nFile type is already handled.", inputPlugin.Name),
                                    Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                            newLightingProgramToolStripMenuItem.Tag = inputPlugin;
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
        }


        private void PreferencesPreferenceChange(string preferenceName) {
            switch (preferenceName) {
                case "EventPeriod":
                    if (_preferences.GetInteger("EventPeriod") < Vendor.MinimumEventPeriod) {
                        _preferences.SetInteger("EventPeriod", Vendor.MinimumEventPeriod);
                        MessageBox.Show(String.Format(Resources.VixenPlusForm_EventPeriodMin, Vendor.MinimumEventPeriod), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    break;

                case "ShutdownTime":
                    SetShutdownTime(_preferences.GetString("ShutdownTime"));
                    break;
            }
        }


        private void manageToolStripMenuItem_Click(object sender, EventArgs e) {
            using (var dialog = new VixenPlusRoadie()) {
                if (dialog.ShowDialog() == DialogResult.OK) {
                    NotifyAll(Notification.ProfileChange);
                }
            }
        }


        // ReSharper disable once UnusedParameter.Local
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
            openFileDialog1.Filter = FileIOHelper.GetOpenFilters();
            openFileDialog1.InitialDirectory = Paths.SequencePath;
            openFileDialog1.FileName = string.Empty;
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
            var fileIOHandler = FileIOHelper.GetByExtension(fileName);
            if (fileIOHandler == null) {
                MessageBox.Show(Resources.VixenPlusForm_NoKnowEditor, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            if (!fileIOHandler.CanOpen()) {
                MessageBox.Show(string.Format("Sorry, we can only export {0} files.", fileIOHandler.Name()), Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;   
            }

            AddToFileHistory(fileName);

            var plugInInterface = (IUIPlugIn) Activator.CreateInstance(_registeredFileTypes[".vix"].GetType());

            plugInInterface.Sequence = fileIOHandler.OpenSequence(fileName);

            ((Form) plugInInterface).Text = plugInInterface.Sequence.Name + " - " + plugInInterface.Sequence.FileIOHandler.Name();
            var uiBase = plugInInterface as UIBase;
            if (uiBase != null) {
                uiBase.DirtyChanged += plugin_DirtyChanged;
            }

            plugInInterface.MdiParent = this;
            plugInInterface.Show();
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
            using (var preferencesDialog = new PreferencesDialog()) {
                if (preferencesDialog.ShowDialog() != DialogResult.OK) {
                    return;
                }

                _preferences.Reload();
                NotifyAll(Notification.PreferenceChange);
            }
        }


        private void programToolStripMenuItem_DropDownOpening(object sender, EventArgs e) {
            saveToolStripMenuItem.Text = Resources.VixenPlusForm_Save;
            
            var plugIn = ActiveMdiChild as IUIPlugIn;
            if (plugIn != null && !string.IsNullOrEmpty(plugIn.Sequence.Name)) {
                saveToolStripMenuItem.Text += string.Format(" ({0})", plugIn.Sequence.Name);
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

        /// <summary>
        /// Save the file, routed to the appropriate plugin
        /// </summary>
        /// <param name="pluginInstance"></param>
        /// <returns>If a save action was performed</returns>
        private bool Save(IUIPlugIn pluginInstance) {
            
            if (pluginInstance == null) {
                return false;
            }

            var plugInInterface = pluginInstance;
            if ((plugInInterface.IsDirty && string.IsNullOrEmpty(plugInInterface.Sequence.Name)) && !GetNewSequenceInfo(pluginInstance)) {
                return false;
            }
            
            UpdateHistoryImages(plugInInterface.Sequence.FileName);

            // If the sequenceType is not set, set it to Vixen Plus
            if (plugInInterface.Sequence.FileIOHandler == null) {
                // this should never be true, if it is we don't want to blow up, but notify and continue.
                MessageBox.Show("Interesting!",
                    "You should never see this, but if you do, I didn't want to blow up on you\n" + "Let Macebobo on Diychristmas.org know, please.\n" +
                    "I'm going to set your sequence file handler type to Vixen Plus - he'll know what that means\n" + "And I can continue gracefully.\n"+
                    "Sorry about that! Carry on.");
                plugInInterface.Sequence.FileIOHandler = FileIOHelper.GetNativeHelper();
            }

            plugInInterface.SaveTo();

            if (plugInInterface.Sequence.FileIOHandler.CanOpen()) {
                AddToFileHistory(plugInInterface.Sequence.FileName);
            }

            if (_preferences.GetBoolean("ShowSaveConfirmation")) {
                MessageBox.Show(Resources.VixenPlusForm_SequenceSaved, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

            return true;
        }


        private bool SaveAs(IUIPlugIn pluginInstance) {
            return (GetNewSequenceInfo(pluginInstance) && Save(pluginInstance));
        }


        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            SaveAs((UIBase) ActiveMdiChild);
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            Save((UIBase) ActiveMdiChild);
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
            Thread.Sleep(1000); //todo replace with Task.Delay() when using 4.5
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
            var isValidData = (e.Data.GetDataPresent(DataFormats.FileDrop));
            
            if (isValidData) {
                var files = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
                isValidData = files.Aggregate(true, (current, file) => current & FileIOHelper.GetValidOpeningExtensions().Contains(file.Substring(file.Length - 4, 4)));
            }

            e.Effect = isValidData ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void VixenPlusForm_DragDrop(object sender, DragEventArgs e) {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            foreach (var f in files) {

                OpenSequence(f);
            }
        }
    }
}