namespace AppUpdate
{
    using ICSharpCode.SharpZipLib.Checksums;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Threading;
    using System.Windows.Forms;

    public class AppUpdate
    {
        private string m_appArgs = string.Empty;
        private string m_appPath = string.Empty;
        private WebClient m_client = null;
        private Process m_currentProcess;
        private Version m_currentVersion;
        private Migration m_currentVersionMigration = null;
        private string m_keyFile;
        private List<Migration> m_migrations;
        private string m_updateRootPath;
        private string m_updateServerURI;

        public AppUpdate(string updateServerURI, string updateRootPath)
        {
            this.m_updateServerURI = updateServerURI;
            this.m_updateRootPath = updateRootPath;
        }

        private void DoMigration()
        {
            Exception exception;
            MigrationCatalogItem catalogItem = new MigrationCatalogItem();
            Crc32 crc = new Crc32();
            bool flag = false;
            bool flag2 = false;
            try
            {
                Stream stream;
                StreamReader reader;
                string str2;
                this.m_currentProcess = Process.GetCurrentProcess();
                string directoryName = Path.GetDirectoryName(this.m_currentProcess.MainModule.FileName);
                if (this.m_updateServerURI[this.m_updateServerURI.Length - 1] == '/')
                {
                    this.m_updateServerURI = this.m_updateServerURI.Remove(this.m_updateServerURI.Length - 1);
                }
                if ((this.m_updateRootPath.Length != 1) && (this.m_updateRootPath[this.m_updateRootPath.Length - 1] == '/'))
                {
                    this.m_updateRootPath = this.m_updateRootPath.Remove(this.m_updateRootPath.Length - 1);
                }
                if (this.m_updateRootPath[0] == '/')
                {
                    this.m_updateRootPath = string.Format("{0}{1}", this.m_updateServerURI, this.m_updateRootPath);
                }
                else
                {
                    this.m_updateRootPath = string.Format("{0}/{1}", this.m_updateServerURI, this.m_updateRootPath);
                }
                this.m_keyFile = Path.GetFileName(this.m_currentProcess.MainModule.FileName);
                this.m_currentVersion = new Version(this.m_currentProcess.MainModule.FileVersionInfo.FileVersion);
                ProgressDialog dialog = new ProgressDialog();
                dialog.Message = "Contacting update site";
                dialog.Show();
                dialog.Refresh();
                this.m_migrations = new List<Migration>();
                this.m_client = new WebClient();
                try
                {
                    try
                    {
                        stream = this.m_client.OpenRead(this.m_updateRootPath + "/update.master");
                    }
                    catch
                    {
                        dialog.Hide();
                        dialog.Dispose();
                        return;
                    }
                    reader = new StreamReader(stream);
                    while ((str2 = reader.ReadLine()) != null)
                    {
                        Migration item = new Migration(str2);
                        if (this.m_currentVersion == item.KeyFileVersion)
                        {
                            this.m_currentVersionMigration = item;
                        }
                        else if (this.m_currentVersion < item.KeyFileVersion)
                        {
                            this.m_migrations.Add(item);
                        }
                    }
                    stream.Close();
                    this.m_appPath = this.m_currentProcess.MainModule.FileName;
                    this.m_appArgs = this.m_currentProcess.StartInfo.Arguments;
                }
                catch
                {
                    this.m_client = null;
                    throw;
                }
                finally
                {
                    dialog.Hide();
                    dialog.Dispose();
                }
                if (this.m_currentVersionMigration != null)
                {
                    dialog = new ProgressDialog();
                    dialog.Message = "Checking for updates";
                    dialog.Show();
                    dialog.Refresh();
                    try
                    {
                        if (this.m_currentVersionMigration.VersionCatalogFile[0] == '/')
                        {
                            stream = this.m_client.OpenRead(this.m_updateRootPath + this.m_currentVersionMigration.VersionCatalogFile);
                        }
                        else
                        {
                            stream = this.m_client.OpenRead(string.Format("{0}/{1}", this.m_updateRootPath, this.m_currentVersionMigration.VersionCatalogFile));
                        }
                        reader = new StreamReader(stream);
                        while ((str2 = reader.ReadLine()) != null)
                        {
                            catalogItem.Parse(str2);
                            if (!catalogItem.HasFlag("UpgradeOnly"))
                            {
                                string str3;
                                if (!catalogItem.IsData)
                                {
                                    str3 = Path.Combine(directoryName, catalogItem.DestPath);
                                }
                                else
                                {
                                    str3 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), catalogItem.DestPath);
                                }
                                if (catalogItem.HasFlag("Delete"))
                                {
                                    if (catalogItem.HasFlag("Directory"))
                                    {
                                        if (Directory.Exists(str3))
                                        {
                                            flag2 = true;
                                        }
                                    }
                                    else if (System.IO.File.Exists(str3))
                                    {
                                        flag2 = true;
                                    }
                                }
                                else if (catalogItem.HasFlag("Directory"))
                                {
                                    if (!Directory.Exists(str3))
                                    {
                                        flag2 = true;
                                    }
                                }
                                else if (!System.IO.File.Exists(str3))
                                {
                                    if (!catalogItem.HasFlag("Optional"))
                                    {
                                        flag2 = true;
                                    }
                                }
                                else if (!catalogItem.HasFlag("NoOverwrite"))
                                {
                                    crc.Reset();
                                    crc.Update(System.IO.File.ReadAllBytes(str3));
                                    if (catalogItem.CRC32 != crc.Value)
                                    {
                                        flag2 = true;
                                        if (this.RunningModule(catalogItem))
                                        {
                                            flag = true;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                        reader.Close();
                    }
                    catch (Exception exception1)
                    {
                        exception = exception1;
                        MessageBox.Show(exception.StackTrace + "\n\n" + exception.Message + "\n1\nThe update will not continue.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }
                    finally
                    {
                        dialog.Hide();
                        dialog.Dispose();
                    }
                }
                if ((this.m_migrations.Count != 0) || flag2)
                {
                    bool restartRequired = (this.m_migrations.Count > 0) || flag;
                    UpdateNotificationDialog dialog2 = new UpdateNotificationDialog(restartRequired);
                    if (dialog2.ShowDialog() != DialogResult.Yes)
                    {
                        dialog2.Dispose();
                    }
                    else
                    {
                        dialog2.Dispose();
                        string tempFileName = Path.GetTempFileName();
                        StreamWriter writer = System.IO.File.CreateText(tempFileName);
                        writer.WriteLine("version=" + this.m_currentVersion);
                        if (restartRequired)
                        {
                            writer.WriteLine("restart=" + this.m_appPath);
                            writer.WriteLine("args=" + this.m_appArgs);
                            writer.WriteLine("process=" + this.m_currentProcess.Id);
                        }
                        writer.WriteLine("sourcePathRoot=" + this.m_updateRootPath);
                        writer.WriteLine("destPathRoot=" + directoryName);
                        writer.Flush();
                        writer.Close();
                        string str5 = Path.GetDirectoryName(this.m_currentProcess.MainModule.FileName);
                        string path = Path.Combine(str5, "Update.bin");
                        string destFileName = Path.Combine(str5, "Update.exe");
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Copy(path, destFileName, true);
                            System.IO.File.Delete(path);
                        }
                        Process process = new Process();
                        process.StartInfo.Arguments = tempFileName.Replace(" ", "%20");
                        process.StartInfo.FileName = destFileName;
                        process.Start();
                        if (restartRequired)
                        {
                            ShutdownDialog dialog3 = new ShutdownDialog();
                            while (!this.ShutdownApplication())
                            {
                                switch (dialog3.ShowDialog())
                                {
                                    case DialogResult.OK:
                                        this.m_currentProcess.Kill();
                                        break;

                                    case DialogResult.Abort:
                                        throw new Exception("Update process aborted.");

                                    case DialogResult.Retry:
                                    {
                                        Thread.Sleep(500);
                                        continue;
                                    }
                                }
                            }
                            dialog3.Dispose();
                        }
                        else
                        {
                            process.WaitForExit();
                            MessageBox.Show("Update completed.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                }
            }
            catch (Exception exception2)
            {
                exception = exception2;
                MessageBox.Show(exception.StackTrace + "\n2\n" + exception.Message + "\n\nThe update will not continue.", "Update", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void ExecuteMigration()
        {
            new Thread(new ThreadStart(this.DoMigration)).Start();
        }

        private bool RunningModule(MigrationCatalogItem catalogItem)
        {
            CaseInsensitiveComparer comparer = new CaseInsensitiveComparer();
            string fileName = Path.GetFileName(catalogItem.DestPath);
            foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
            {
                if (comparer.Compare(Path.GetFileName(module.FileName), fileName) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        private bool ShutdownApplication()
        {
            return this.m_currentProcess.CloseMainWindow();
        }
    }
}

