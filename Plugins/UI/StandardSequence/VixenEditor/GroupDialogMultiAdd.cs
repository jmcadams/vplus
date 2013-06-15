using System;
using System.Windows.Forms;

using VixenPlus;

namespace VixenEditor {
    public partial class GroupDialogMultiAdd : Form {
        public GroupDialogMultiAdd() {
            InitializeComponent();
            tbName.Focus();
        }

        public string GroupName { get { return tbName.Text; } }

        public int GroupCount { get { return (int)nudCount.Value; } }

        private void GroupDialogMultiAdd_FormClosing(object sender, FormClosingEventArgs e) {
            if (DialogResult == DialogResult.Cancel) return;

            var msg = String.Empty;

            if (GroupName.Contains(Group.GroupTextDivider) || GroupName.Contains(",")) {
                msg = String.Format("A group name can not contain a {0} or a ,", Group.GroupTextDivider);
            }
            else if (GroupCount < 1) {
                msg = "Need to pick how many groups to create.";
            }

            if (String.IsNullOrEmpty(msg)) {
                return;
            }

            MessageBox.Show(msg, @"Bad Group Name or Count", MessageBoxButtons.OK);
            e.Cancel = true;
        }

        private void nudCount_Enter(object sender, EventArgs e) {
            nudCount.Select(0,nudCount.Text.Length);
        }
    }
}
