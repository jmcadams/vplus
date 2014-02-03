using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using VixenPlus.Properties;

namespace VixenPlus.Dialogs {
    public partial class FirstRunPathDialog : Form {
        public FirstRunPathDialog(bool isFirstRun) {
            InitializeComponent();
            Icon = Resources.VixenPlus;
            tbPrompt.Text = string.Format(isFirstRun ? Resources.FirstRunPrompt : Resources.LocationChangePrompt, Vendor.ProductName);
            if (isFirstRun) {
                btnOk.Location = btnCancel.Location;
            }
            else {
                btnCancel.Visible = true;
            }
            pbIcon.Image = new Icon(Resources.VixenPlus, new Size(64, 64)).ToBitmap();
            lblAppDir.Text = Paths.BinaryPath + Paths.DataFolder;
            lblMyDocs.Text = Paths.MyDocutments + Paths.DataFolder;
            lblCustom.Text = Paths.DataFolder;
        }


        private string DataPath {
            get { return (rbUseAppDir.Checked ? Paths.BinaryPath : rbMyDocs.Checked ? Paths.DataPath : tbFolder.Text) + Paths.DataFolder; }
        }


        private void radioButton_CheckedChanged(object sender, EventArgs e) {
            btnFolder.Enabled = rbCustom.Checked;
            tbFolder.Enabled = rbCustom.Checked;
            lblCustom.Visible = rbCustom.Checked;
        }


        private void btnFolder_Click(object sender, EventArgs e) {
            using (var folderDialog = new FolderBrowserDialog()) {
                if (folderDialog.ShowDialog() != DialogResult.OK) {
                    return;
                }

                tbFolder.Text = folderDialog.SelectedPath;
            }
        }


        private void btnOk_Click(object sender, EventArgs e) {
            if (rbCustom.Checked && tbFolder.Text == String.Empty) {
                MessageBox.Show(@"Please select a folder or enter a path", @"OOPS!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if ((rbMyDocs.Checked && Directory.Exists(DataPath)) || (rbUseAppDir.Checked && Directory.Exists(DataPath))) {
                if (
                    MessageBox.Show(
                        @"It looks like you already have data in that folder.  " +
                        @"While this is okay, it should only be used if you're sure you want to potentially overwrite some of that data.  " +
                        Environment.NewLine + Environment.NewLine + @"Do you want to continue to use this folder?", @"Verify Current Folder",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }

            var path = Environment.ExpandEnvironmentVariables(DataPath);
            if (!Directory.Exists(path)) {
                var valid = true;
                try {
                    if (Path.GetFullPath(path) != path) {
                        valid = false;
                    }
                }
                catch {
                    valid = false;
                }

                if (!valid) {
                    MessageBox.Show(path + @" is not a valid path.  Please enter a valid path.", @"Invalid path", MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    tbFolder.Text = "";
                    return;
                }

                if (MessageBox.Show(path + @" does not exist.  Create it?", @"Create Folder", MessageBoxButtons.YesNo, MessageBoxIcon.Question) !=
                    DialogResult.Yes) {
                    return;
                }
                Directory.CreateDirectory(path);
            }

            using (var file = new StreamWriter(Paths.DataDir)) {
                file.WriteLine(path);
            }

            DialogResult = DialogResult.OK;
        }

        private void tbFolder_TextChanged(object sender, EventArgs e) {
            lblCustom.Text = Environment.ExpandEnvironmentVariables(tbFolder.Text + Paths.DataFolder);
        }
    }
}
