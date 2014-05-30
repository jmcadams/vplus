using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;



using VixenPlusCommon;

namespace VixenPlus.Dialogs {
    public sealed partial class UpdateDialog : Form {
        private readonly Preference2 _preferences;
        private string _version;
        private readonly bool _isInStartup;
        private DialogResult _result;
        private string _updateFile;

        private const string CheckFrequency = "AutoUpdateCheckFreq";
        private const string LastChecked = "LastUpdateCheck";
        private const string SkippedVersion = "SkippedVersion";
        private const string ErrorIndicatorVersion = "0.0.0.0";

        public UpdateDialog(Screen startupScreen, bool isInStartup) {
            Log("Instantiate UpdateDialog start");
            _isInStartup = isInStartup;
            _preferences = Preference2.GetInstance();

            InitializeComponent();

            Left = startupScreen.Bounds.X + (startupScreen.WorkingArea.Width - Width) / 2;
            Top = startupScreen.Bounds.Y + (startupScreen.WorkingArea.Height - Height) / 2;
            MinimumSize = Size;
            MaximumSize = Size;
            Log("Instantiate UpdateDialog complete");
        }

        
        /// <summary>
        /// Returns if it is time to run the check for update routine<br/>
        /// Short Circuit for On Startup and Never
        /// </summary>
        /// <returns>bool representing if it is time to check for an update</returns>
        public bool IsTimeToCheckForUpdate() {
            Log("IsTimeToCheckForUpdate Start");
            const string onStartup = "On Statup";
            const string never = "Never";
            const string daily = "Daily";
            const string weekly = "Weekly";
            const string monthly = "Monthly";
            const string quarterly = "Quarterly";
            const string annually = "Annually";

            // First short circuit 
            var freq = _preferences.GetString(CheckFrequency);
            if (freq == never || freq == onStartup) {
                Log("IsTimeToCheckForUpdate short circuit: " + (freq == onStartup));
                return freq == onStartup;
            }

            // Next, get how many hours we need to wait until updating again. Default to On Startup.
            var waitHours = 0.0;
            switch (freq) {
                case daily:
                    waitHours = Utils.UpdateDaily;
                    break;
                case weekly:
                    waitHours = Utils.UpdateWeekly;
                    break;
                case monthly:
                    waitHours = Utils.UpdateMonthly;
                    break;
                case quarterly:
                    waitHours = Utils.UpdateQuarterly;
                    break;
                case annually:
                    waitHours = Utils.UpdateAnnually;
                    break;
            }

            // Finally, calculate elapsed time in seconds and return result
            var lastChecked = DateTime.Parse(_preferences.GetString(LastChecked), CultureInfo.InvariantCulture);
            var elapsedSeconds = (DateTime.Now - lastChecked).TotalHours;
            Log("IsTimeToCheckForUpdate normal: " + (elapsedSeconds >= waitHours));
            return elapsedSeconds >= waitHours;
        }


        /// <summary>
        /// Go out to the vixenplus.com website and see if there is an update available<br/>
        /// If an error occurs or the update dialog can't connect, a value indicating an error is returned 
        /// </summary>
        /// <returns>string with default or latest version value</returns>
        private string GetAvailableVersion() {
            Log("GetAvailableVersion start");
            var result = ErrorIndicatorVersion;
            using (var client = new WebClient()) {
                try {
                    var response = client.DownloadData(Vendor.UpdateURL + Vendor.UpdateFile + "?ver=" + Utils.GetVersion(GetType()));
                    var xml = XDocument.Parse(Encoding.ASCII.GetString(response));
                    var rev = (from r in xml.Descendants("version") where r.Attribute("format") != null select r.Attribute("rev")).SingleOrDefault();
                    if (null != rev) {
                        result = rev.Value;
                    }
                }
                catch (Exception e) {
                    Log(e.StackTrace);
                }
            }
            Log("Available version: " + result);
            return result;
        }


        /// <summary>
        /// Update that dialog that we're checking.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateDialog_Shown(object sender, EventArgs e) {
            Log("Dialog being shown start");
            lblPrompt.Text = _isInStartup ? string.Format("Performing {0} update check.  You can change the frequency of these update checks in preferences.",
                _preferences.GetString(CheckFrequency)) : "Checking for updates.";
            SetupDialogShowHide(false);
            Application.DoEvents();

            CheckForUpdate();
            Log("Dialog being shown end");
        }


        /// <summary>
        /// If we get a good version back, show the user their options<br/>
        /// 1) There's a new version<br/>
        /// 2) They're up to date<br/>
        /// 3) Communication or file error<br/>
        /// 4) Nothing if we're the version is ignored or if they are up to date and in startup
        /// </summary>
        private void CheckForUpdate() {
            Log("Check for update start");
            _version = GetAvailableVersion();
            var thisVersion = Utils.GetVersion(GetType());
            if (_version != ErrorIndicatorVersion) {
                _preferences.SetString(LastChecked, DateTime.Now.ToString(CultureInfo.InvariantCulture));
                _preferences.SaveSettings();
                if (_version == _preferences.GetString(SkippedVersion)) {
                    Log("Version skipped");
                    DialogResult = DialogResult.No;
                    return;
                }
                if (IsNewerVersionAvailable(_version, thisVersion)) {
                    Log(string.Format("current: {1}, new: {0}", _version, thisVersion));
                    SetupDialogShowHide(true);
                    pbDownload.Visible = false;
                    Text = "New update available";
                    lblPrompt.Text =
                        string.Format(
                            "Version {0} of {1} is available would you like to download and install this version?\n\nYou are running version {2}",
                            _version, Vendor.ProductName, thisVersion);
                }
                else if (!_isInStartup) {
                    Log("Up to date NOT in startup");
                    SetupDialogForOkay();
                    Text = "You're up to date";
                    lblPrompt.Text = string.Format("You have version {0}, which is the latest version of {1} ", thisVersion, Vendor.ProductName);
                }
                else {
                    Log("Up to date in startup");
                    DialogResult = DialogResult.No;
                }
            }
            else {
                Log("Communication/firewall error");
                SetupDialogForOkay();
                Text = "OOPS!";
                lblPrompt.Text = string.Format("Could not reach the {0} web site, check your internet connection or firewall.", Vendor.ProductName);
            }
        }


        private static bool IsNewerVersionAvailable(string newVersion, string currentVersion) {
            var newV = newVersion.Split('.');
            var curV = currentVersion.Split('.');

            var isGreater = false;

            for (var i = 0; i < newV.Length && i < curV.Length; i++) {
                isGreater |= int.Parse(newV[i]) > int.Parse(curV[i]);
            }

            return isGreater;
        }

        /// <summary>
        /// Launch the batch file that performs the install
        /// </summary>
        private void InstallDownload() {
            Log("Launch batch file (" + Process.GetCurrentProcess().Id + " \"" + _updateFile + "\" \"" + Application.StartupPath + @"\" + "\")");
            Process.Start(Vendor.UpdateSupportBatchReal, Process.GetCurrentProcess().Id + " \"" + _updateFile + "\" \"" + Application.StartupPath + @"\" + "\"");
        }


        private void btnInstallNow_Click(object sender, EventArgs e) {
            Log("Install now");
            GetUpdateFile(false);

            // clear the skipped file since we are installing a new version
            _preferences.SetString(SkippedVersion, string.Empty);
            _preferences.SaveSettings();
            InstallDownload();
            _result = DialogResult.Yes;
        }


        private void btnDownloadOnly_Click(object sender, EventArgs e) {
            Log("Download Only");
            GetUpdateFile(true);
            _result = DialogResult.No;
        }


        private void btnAskMeLater_Click(object sender, EventArgs e) {
            Log("Ask Me Later");
            DialogResult = DialogResult.No;
        }


        private void btnReleaseNotes_Click(object sender, EventArgs e) {
            Log("View release notes start");
            using (var dialog = new ReleaseNotesDialog()) {
                dialog.ShowDialog();
            }
            Log("View release notes end");
        }


        private void btnSkipVersion_Click(object sender, EventArgs e) {
            Log("Skipping version " + _version);
            _preferences.SetString(SkippedVersion, _version);
            _preferences.SaveSettings();
            DialogResult = DialogResult.No;
        }


        private void GetUpdateFile(bool selectFolder) {
            Log("Gettting update file start");
            SetupDialogForDownload();
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
            Log("Getting update file to " + path);
        }


        private void SetupDialogForDownload() {
            Log("SetupDialogForDownload");
            SetupDialogShowHide(false);
            pbDownload.Visible = true;
        }


        private void SetupDialogForOkay() {
            Log("SetupDialogForOkay");
            SetupDialogShowHide(false);
            btnAskMeLater.Visible = true;
            btnAskMeLater.Text = "Okay";
        }



        private void SetupDialogShowHide(bool areShown) {
            Log("SetupDialogShowHide: " + areShown);
            btnInstallNow.Visible = areShown;
            btnDownloadOnly.Visible = areShown;
            btnAskMeLater.Visible = areShown;
            btnReleaseNotes.Visible = areShown;
            btnSkipVersion.Visible = areShown;
            pbDownload.Visible = areShown;
        }

        private void DoAsyncDownload(string path) {
            Log("Starting Async download");
            _updateFile = Path.Combine(path, Vendor.ProductName + "." + _version + Vendor.UpdateFileExtension);
            var url = Vendor.UpdateFileURL + (Utils.IsWindows64BitOS() ? "64" : "32") + Vendor.UpdateFileExtension;
            var client = new WebClient();
            client.DownloadProgressChanged += client_DownloadProgressChanged;
            client.DownloadFileCompleted += client_DownloadFileCompleted;
            try {
                client.DownloadFileAsync(new Uri(url), _updateFile);
            }
            catch (Exception e) {
                Log(e.StackTrace);
            }
        }


        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            Log("Async progress change: " + e.ProgressPercentage);
            BeginInvoke((MethodInvoker) delegate {
                lblPrompt.Text = String.Format("Downloaded {0}k bytes of {1}k bytes.", 
                    e.BytesReceived / Utils.BytesPerK,
                    e.TotalBytesToReceive / Utils.BytesPerK);
                pbDownload.Value = e.ProgressPercentage;
            });
        }


        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e) {
            Log("Async Download Complete");
            if (e.Error == null) {
                Log("Async Download success");
                if (_result == DialogResult.No) {
                    MessageBox.Show(@"File download successful.");
                }
                DialogResult = _result;
            }
            else {
                Log("Async download error: " + e.Error.Message);
                MessageBox.Show("Download failed: " + e.Error.Message);
                DialogResult = DialogResult.No;
            }
        }


        private static void Log(string message) {
            string.Format("{0:} {1}", DateTime.Now, message).UpdateLog();
        }
    }
}
