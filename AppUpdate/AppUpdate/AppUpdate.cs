using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Checksums;

namespace AppUpdate {
    
    public class AppUpdate {
        private string _args = string.Empty;
        private string _appPath = string.Empty;
        private WebClient _client;
        private Process _currentProcess;
        private Version _currentVersion;
        private Migration _currentVersionMigration;
        private List<Migration> _migrations;
        private string _updateRootPath;
        private string _updateServerURI;


        public AppUpdate(string updateServerURI, string updateRootPath) {
            _updateServerURI = updateServerURI;
            _updateRootPath = updateRootPath;
        }


        private void DoMigration() {
            var catalogItem = new MigrationCatalogItem();
            var crc = new Crc32();
            var flag = false;
            var flag2 = false;
            try {
                _currentProcess = Process.GetCurrentProcess();
                var directoryName = Path.GetDirectoryName(_currentProcess.MainModule.FileName);
                if (directoryName == null) {
                    return;
                }
                if (_updateServerURI[_updateServerURI.Length - 1] == '/') {
                    _updateServerURI = _updateServerURI.Remove(_updateServerURI.Length - 1);
                }
                if ((_updateRootPath.Length != 1) && (_updateRootPath[_updateRootPath.Length - 1] == '/')) {
                    _updateRootPath = _updateRootPath.Remove(_updateRootPath.Length - 1);
                }
                _updateRootPath = string.Format(_updateRootPath[0] == '/' ? "{0}{1}" : "{0}/{1}", _updateServerURI, _updateRootPath);
                _currentVersion = new Version(_currentProcess.MainModule.FileVersionInfo.FileVersion);
                _migrations = new List<Migration>();
                _client = new WebClient();
                Stream stream;
                try {
                    try {
                        stream = _client.OpenRead(_updateRootPath + "/update.master");
                    }
                    catch {
                        return;
                    }
                    if (stream == null) {
                        return;
                    }
                    using (var reader = new StreamReader(stream)) {
                        string str2;
                        while ((str2 = reader.ReadLine()) != null) {
                            var item = new Migration(str2);
                            if (_currentVersion == item.KeyFileVersion) {
                                _currentVersionMigration = item;
                            }
                            else if (_currentVersion < item.KeyFileVersion) {
                                _migrations.Add(item);
                            }
                        }
                    }
                    _appPath = _currentProcess.MainModule.FileName;
                    _args = _currentProcess.StartInfo.Arguments;
                }
                catch {
                    _client = null;
                    throw;
                }
                if (_currentVersionMigration != null) {
                    try {
                        stream = _currentVersionMigration.VersionCatalogFile[0] == '/'
                                     ? _client.OpenRead(_updateRootPath + _currentVersionMigration.VersionCatalogFile)
                                     : _client.OpenRead(string.Format("{0}/{1}", _updateRootPath, _currentVersionMigration.VersionCatalogFile));
                        if (stream == null) {
                            return;
                        }

                        var reader = new StreamReader(stream);
                        string str2;
                        while ((str2 = reader.ReadLine()) != null) {
                            catalogItem.Parse(str2);
                            if (catalogItem.HasFlag("UpgradeOnly")) {
                                continue;
                            }

                            var str3 =
                                Path.Combine(!catalogItem.IsData ? directoryName : Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                                             catalogItem.DestPath);
                            if (catalogItem.HasFlag("Delete")) {
                                if (catalogItem.HasFlag("Directory")) {
                                    if (Directory.Exists(str3)) {
                                        flag2 = true;
                                    }
                                }
                                else if (File.Exists(str3)) {
                                    flag2 = true;
                                }
                            }
                            else if (catalogItem.HasFlag("Directory")) {
                                if (!Directory.Exists(str3)) {
                                    flag2 = true;
                                }
                            }
                            else if (!File.Exists(str3)) {
                                if (!catalogItem.HasFlag("Optional")) {
                                    flag2 = true;
                                }
                            }
                            else if (!catalogItem.HasFlag("NoOverwrite")) {
                                crc.Reset();
                                crc.Update(File.ReadAllBytes(str3));
                                if (catalogItem.CRC32 != crc.Value) {
                                    flag2 = true;
                                    if (RunningModule(catalogItem)) {
                                        flag = true;
                                    }
                                    break;
                                }
                            }
                        }
                        reader.Close();
                    }
                    catch (Exception exception1) {
                        MessageBox.Show(exception1.StackTrace + "\n\n" + exception1.Message + "\n1\nThe update will not continue.", "Update",
                                        MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }
                }
                if ((_migrations.Count == 0) && !flag2) {
                    return;
                }

                var restartRequired = (_migrations.Count > 0) || flag;
                using (var dialog2 = new UpdateNotificationDialog(restartRequired)) {
                    if (dialog2.ShowDialog() != DialogResult.Yes) {
                        return;
                    }

                    var tempFileName = Path.GetTempFileName();
                    using (var writer = File.CreateText(tempFileName)) {
                        writer.WriteLine("version=" + _currentVersion);
                        if (restartRequired) {
                            writer.WriteLine("restart=" + _appPath);
                            writer.WriteLine("args=" + _args);
                            writer.WriteLine("process=" + _currentProcess.Id);
                        }
                        writer.WriteLine("sourcePathRoot=" + _updateRootPath);
                        writer.WriteLine("destPathRoot=" + directoryName);

                    }
                    var currentProcessPath = Path.GetDirectoryName(_currentProcess.MainModule.FileName);
                    if (currentProcessPath == null) {
                        return;
                    }

                    var path = Path.Combine(currentProcessPath, "Update.bin");
                    var destFileName = Path.Combine(currentProcessPath, "Update.exe");
                    if (File.Exists(path)) {
                        File.Copy(path, destFileName, true);
                        File.Delete(path);
                    }
                    var process = new Process {StartInfo = {Arguments = tempFileName.Replace(" ", "%20"), FileName = destFileName}};
                    process.Start();
                    if (restartRequired) {
                        var dialog3 = new ShutdownDialog();
                        while (!ShutdownApplication()) {
                            switch (dialog3.ShowDialog()) {
                                case DialogResult.OK:
                                    _currentProcess.Kill();
                                    break;

                                case DialogResult.Abort:
                                    throw new Exception("Update process aborted.");

                                case DialogResult.Retry: {
                                    Thread.Sleep(500);
                                    continue;
                                }
                            }
                        }
                        dialog3.Dispose();
                    }
                    else {
                        process.WaitForExit();
                        MessageBox.Show("Update completed.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
            }
            catch (Exception exception2) {
                MessageBox.Show(exception2.StackTrace + "\n2\n" + exception2.Message + "\n\nThe update will not continue.", "Update",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        public void ExecuteMigration() {
            new Thread(DoMigration).Start();
        }


        private static bool RunningModule(MigrationCatalogItem catalogItem) {
            var comparer = new CaseInsensitiveComparer();
            var fileName = Path.GetFileName(catalogItem.DestPath);
            //foreach (ProcessModule module in Process.GetCurrentProcess().Modules) {
            //    if (comparer.Compare(Path.GetFileName(module.FileName), fileName) == 0) {
            //        return true;
            //    }
            //}
            //return false; 
            return Process.GetCurrentProcess().Modules.Cast<ProcessModule>().Any(module => comparer.Compare(Path.GetFileName(module.FileName), fileName) == 0);
        }


        private bool ShutdownApplication() {
            return _currentProcess.CloseMainWindow();
        }
    }
}
