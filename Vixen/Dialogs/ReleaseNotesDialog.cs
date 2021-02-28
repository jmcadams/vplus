﻿using System;
using System.Net;
using System.Text;
using System.Windows.Forms;

using VixenPlusCommon;

namespace VixenPlus.Dialogs {
    public sealed partial class ReleaseNotesDialog : Form {
        public ReleaseNotesDialog() {
            InitializeComponent();
            using (var client = new WebClient()) {
                var notes = Encoding.ASCII.GetString(client.DownloadData(Vendor.Protocol + Preference2.GetInstance().GetString(Vendor.DomainLS) + Vendor.DistDir + Vendor.UpdateReleaseNote));
                tbNotes.Text = notes.Replace("\n", "\r\n");
                tbNotes.SelectionStart = 0;
                tbNotes.SelectionLength = 0;
                MinimumSize = Size;
            }
        }

        private void btnDone_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
        }
    }
}   
