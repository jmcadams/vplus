using System;
using System.IO;
using System.Windows.Forms;

namespace VixenPlus.Dialogs {
    public partial class FirstRunPathDialog : Form {
        public FirstRunPathDialog() {
            InitializeComponent();
        }


        public string DataPath {
            get { return (rbUseAppDir.Checked ? Paths.BinaryPath : rbMyDocs.Checked ? Paths.DataPath : tbFolder.Text) + @"\Data"; }
        }


        private void radioButton_CheckedChanged(object sender, EventArgs e) {
            btnFolder.Enabled = rbCustom.Checked;
            tbFolder.Enabled = rbCustom.Checked;
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
                MessageBox.Show(@"Please select a folder", @"OOPS!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if ((rbMyDocs.Checked && Directory.Exists(DataPath)) || (rbUseAppDir.Checked && Directory.Exists(DataPath))) {
                if (
                    MessageBox.Show(
                        @"It looks like you already have data in that folder.  " +
                        @"This is okay, but should only be used if you're sure you want to potentially overwrite some of that data.  " +
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

            DialogResult = DialogResult.OK;
        }
    }
}
