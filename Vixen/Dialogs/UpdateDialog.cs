using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Forms;

using CommonUtils;

namespace VixenPlus.Dialogs {
    public sealed partial class UpdateDialog : Form {
        private readonly Preference2 _preferences;
        private readonly string _version;
        private DialogResult _result;
        public string UpdateFile;


        public UpdateDialog(Screen screen, string version) {
            InitializeComponent();
            _version = version;
            lblPrompt.Text = string.Format("Version {0} of {1} is available would you like to download and install this version?\n\nYou are running version {2}", 
                version, Vendor.ProductName, Utils.GetVersion());
            _preferences = Preference2.GetInstance();
            Left = screen.Bounds.X + (screen.WorkingArea.Width - Width) / 2;
            Top = screen.Bounds.Y + (screen.WorkingArea.Height - Height) / 2;
            MinimumSize = Size;
            MaximumSize = Size;
        }


        private void btnDownloadAndInstall_Click(object sender, EventArgs e) {
            GetUpdateFile(false);
            _result = DialogResult.Yes;
        }


        private void btnDownloadOnly_Click(object sender, EventArgs e) {
            GetUpdateFile(true);
            _result = DialogResult.No;
        }


        private void btnDoNotDownload_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.No;
        }


        private void cbTurnOffAutoUpdate_CheckedChanged(object sender, EventArgs e) {
            _preferences.SetBoolean("DisableAutoUpdate", cbTurnOffAutoUpdate.Checked);
            _preferences.SaveSettings();
        }


        private void GetUpdateFile(bool selectFolder) {
            SetupDialog();
            var path = Application.StartupPath;
            if (selectFolder) {
                using (var dialog = new FolderBrowserDialog()) {
                    dialog.ShowDialog();
                    path = dialog.SelectedPath;
                }
            }

            Text = "Download in progess";
            lblPrompt.Text = "Starting download...";
            DoAsyncDownload(path);
        }


        private void SetupDialog() {
            btnDownloadAndInstall.Visible = false;
            btnDownloadOnly.Visible = false;
            btnDoNotDownload.Visible = false;
            cbTurnOffAutoUpdate.Visible = false;
            pbDownload.Visible = true;
        }


        private void DoAsyncDownload(string path) {
            UpdateFile = Path.Combine(path, Vendor.ProductName + "." + _version + Vendor.UpdateFileExtension);
            var url = Vendor.UpdateFileURL + (Utils.IsWindows64BitOS() ? "64" : "32") + Vendor.UpdateFileExtension;
            var client = new WebClient();
            client.DownloadProgressChanged += client_DownloadProgressChanged;
            client.DownloadFileCompleted += client_DownloadFileCompleted;
            try {
                client.DownloadFileAsync(new Uri(url), UpdateFile);
            }
            catch (Exception e) {
                e.StackTrace.Log();
            }
        }


        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            BeginInvoke((MethodInvoker) delegate {
                lblPrompt.Text = String.Format("Downloaded {0}k bytes of {1}k bytes.", 
                    e.BytesReceived / Utils.BytesPerK,
                    e.TotalBytesToReceive / Utils.BytesPerK);
                pbDownload.Value = e.ProgressPercentage;
            });
        }


        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e) {
            if (e.Error == null) {
                if (_result == DialogResult.No) {
                    MessageBox.Show(@"File download successful.");
                }
                DialogResult = _result;
            }
            else {
                MessageBox.Show("Download failed: " + e.Error.Message);
                DialogResult = DialogResult.No;
            }
        }

        private void btnReleaseNotes_Click(object sender, EventArgs e) {
            using (var dialog = new ReleaseNotesDialog()) {
                dialog.ShowDialog();
            }
        }

    }
}
