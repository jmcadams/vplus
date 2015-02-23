using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using VixenPlus.Properties;

using VixenPlusCommon;

using TabControl = System.Windows.Forms.TabControl;

namespace VixenPlus.Dialogs
{
    partial class VixenPlusRoadie
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cbProfiles = new System.Windows.Forms.ComboBox();
            this.gbProfiles = new System.Windows.Forms.GroupBox();
            this.btnProfileSave = new System.Windows.Forms.Button();
            this.btnProfileDelete = new System.Windows.Forms.Button();
            this.btnProfileRename = new System.Windows.Forms.Button();
            this.btnProfileCopy = new System.Windows.Forms.Button();
            this.btnProfileAdd = new System.Windows.Forms.Button();
            this.tcProfile = new System.Windows.Forms.TabControl();
            this.tpChannels = new System.Windows.Forms.TabPage();
            this.pChannels = new System.Windows.Forms.Panel();
            this.tpPlugins = new System.Windows.Forms.TabPage();
            this.pPlugIns = new System.Windows.Forms.Panel();
            this.tpGroups = new System.Windows.Forms.TabPage();
            this.pGroups = new System.Windows.Forms.Panel();
            this.btnOkay = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ttRoadie = new System.Windows.Forms.ToolTip(this.components);
            this.dataGridViewDisableButtonColumn1 = new VixenPlus.Dialogs.DataGridViewDisableButtonColumn();
            this.dataGridViewDisableButtonColumn2 = new VixenPlus.Dialogs.DataGridViewDisableButtonColumn();
            this.dataGridViewDisableButtonColumn3 = new VixenPlus.Dialogs.DataGridViewDisableButtonColumn();
            this.btnTimer = new System.Windows.Forms.Timer(this.components);
            this.gbProfiles.SuspendLayout();
            this.tcProfile.SuspendLayout();
            this.tpChannels.SuspendLayout();
            this.tpPlugins.SuspendLayout();
            this.tpGroups.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbProfiles
            // 
            this.cbProfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfiles.FormattingEnabled = true;
            this.cbProfiles.Location = new System.Drawing.Point(10, 19);
            this.cbProfiles.Name = "cbProfiles";
            this.cbProfiles.Size = new System.Drawing.Size(281, 21);
            this.cbProfiles.TabIndex = 0;
            this.cbProfiles.SelectedIndexChanged += new System.EventHandler(this.cbProfiles_SelectedIndexChanged);
            // 
            // gbProfiles
            // 
            this.gbProfiles.Controls.Add(this.btnProfileSave);
            this.gbProfiles.Controls.Add(this.btnProfileDelete);
            this.gbProfiles.Controls.Add(this.btnProfileRename);
            this.gbProfiles.Controls.Add(this.btnProfileCopy);
            this.gbProfiles.Controls.Add(this.btnProfileAdd);
            this.gbProfiles.Controls.Add(this.cbProfiles);
            this.gbProfiles.Location = new System.Drawing.Point(12, 12);
            this.gbProfiles.Name = "gbProfiles";
            this.gbProfiles.Size = new System.Drawing.Size(704, 52);
            this.gbProfiles.TabIndex = 0;
            this.gbProfiles.TabStop = false;
            this.gbProfiles.Text = "Profile";
            // 
            // btnProfileSave
            // 
            this.btnProfileSave.Location = new System.Drawing.Point(621, 19);
            this.btnProfileSave.Name = "btnProfileSave";
            this.btnProfileSave.Size = new System.Drawing.Size(75, 23);
            this.btnProfileSave.TabIndex = 5;
            this.btnProfileSave.Text = "&Save";
            this.ttRoadie.SetToolTip(this.btnProfileSave, "Save current profile");
            this.btnProfileSave.UseVisualStyleBackColor = true;
            this.btnProfileSave.Click += new System.EventHandler(this.btnProfileSave_Click);
            // 
            // btnProfileDelete
            // 
            this.btnProfileDelete.Location = new System.Drawing.Point(540, 19);
            this.btnProfileDelete.Name = "btnProfileDelete";
            this.btnProfileDelete.Size = new System.Drawing.Size(75, 23);
            this.btnProfileDelete.TabIndex = 4;
            this.btnProfileDelete.Text = "Delete";
            this.ttRoadie.SetToolTip(this.btnProfileDelete, "Delete current profile");
            this.btnProfileDelete.UseVisualStyleBackColor = true;
            this.btnProfileDelete.Click += new System.EventHandler(this.btnProfileDelete_Click);
            // 
            // btnProfileRename
            // 
            this.btnProfileRename.Location = new System.Drawing.Point(378, 19);
            this.btnProfileRename.Name = "btnProfileRename";
            this.btnProfileRename.Size = new System.Drawing.Size(75, 23);
            this.btnProfileRename.TabIndex = 2;
            this.btnProfileRename.Text = "Rename";
            this.ttRoadie.SetToolTip(this.btnProfileRename, "Rename current profile");
            this.btnProfileRename.UseVisualStyleBackColor = true;
            this.btnProfileRename.Click += new System.EventHandler(this.btnProfileRename_Click);
            // 
            // btnProfileCopy
            // 
            this.btnProfileCopy.Location = new System.Drawing.Point(459, 19);
            this.btnProfileCopy.Name = "btnProfileCopy";
            this.btnProfileCopy.Size = new System.Drawing.Size(75, 23);
            this.btnProfileCopy.TabIndex = 3;
            this.btnProfileCopy.Text = "Copy";
            this.ttRoadie.SetToolTip(this.btnProfileCopy, "Copy current profile");
            this.btnProfileCopy.UseVisualStyleBackColor = true;
            this.btnProfileCopy.Click += new System.EventHandler(this.btnProfileCopy_Click);
            // 
            // btnProfileAdd
            // 
            this.btnProfileAdd.Location = new System.Drawing.Point(297, 19);
            this.btnProfileAdd.Name = "btnProfileAdd";
            this.btnProfileAdd.Size = new System.Drawing.Size(75, 23);
            this.btnProfileAdd.TabIndex = 1;
            this.btnProfileAdd.Text = "Add";
            this.ttRoadie.SetToolTip(this.btnProfileAdd, "Add a new profile");
            this.btnProfileAdd.UseVisualStyleBackColor = true;
            this.btnProfileAdd.Click += new System.EventHandler(this.btnProfileAdd_Click);
            // 
            // tcProfile
            // 
            this.tcProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tcProfile.Controls.Add(this.tpChannels);
            this.tcProfile.Controls.Add(this.tpPlugins);
            this.tcProfile.Controls.Add(this.tpGroups);
            this.tcProfile.HotTrack = true;
            this.tcProfile.Location = new System.Drawing.Point(12, 70);
            this.tcProfile.Name = "tcProfile";
            this.tcProfile.SelectedIndex = 0;
            this.tcProfile.Size = new System.Drawing.Size(984, 561);
            this.tcProfile.TabIndex = 3;
            this.ttRoadie.SetToolTip(this.tcProfile, "Manage profile channels");
            this.tcProfile.Visible = false;
            this.tcProfile.SelectedIndexChanged += new System.EventHandler(this.tcProfile_SelectedIndexChanged);
            // 
            // tpChannels
            // 
            this.tpChannels.Controls.Add(this.pChannels);
            this.tpChannels.Location = new System.Drawing.Point(4, 22);
            this.tpChannels.Name = "tpChannels";
            this.tpChannels.Padding = new System.Windows.Forms.Padding(3);
            this.tpChannels.Size = new System.Drawing.Size(976, 535);
            this.tpChannels.TabIndex = 0;
            this.tpChannels.Text = "Channels";
            this.tpChannels.UseVisualStyleBackColor = true;
            // 
            // pChannels
            // 
            this.pChannels.Location = new System.Drawing.Point(0, 0);
            this.pChannels.Name = "pChannels";
            this.pChannels.Size = new System.Drawing.Size(976, 535);
            this.pChannels.TabIndex = 0;
            // 
            // tpPlugins
            // 
            this.tpPlugins.Controls.Add(this.pPlugIns);
            this.tpPlugins.Location = new System.Drawing.Point(4, 22);
            this.tpPlugins.Name = "tpPlugins";
            this.tpPlugins.Size = new System.Drawing.Size(976, 535);
            this.tpPlugins.TabIndex = 1;
            this.tpPlugins.Text = "Plugins";
            this.tpPlugins.UseVisualStyleBackColor = true;
            // 
            // pPlugIns
            // 
            this.pPlugIns.Location = new System.Drawing.Point(0, 0);
            this.pPlugIns.Name = "pPlugIns";
            this.pPlugIns.Size = new System.Drawing.Size(976, 535);
            this.pPlugIns.TabIndex = 0;
            // 
            // tpGroups
            // 
            this.tpGroups.Controls.Add(this.pGroups);
            this.tpGroups.Location = new System.Drawing.Point(4, 22);
            this.tpGroups.Name = "tpGroups";
            this.tpGroups.Size = new System.Drawing.Size(976, 535);
            this.tpGroups.TabIndex = 2;
            this.tpGroups.Text = "Groups";
            this.tpGroups.UseVisualStyleBackColor = true;
            // 
            // pGroups
            // 
            this.pGroups.Location = new System.Drawing.Point(0, 0);
            this.pGroups.Name = "pGroups";
            this.pGroups.Size = new System.Drawing.Size(976, 535);
            this.pGroups.TabIndex = 2;
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOkay.Location = new System.Drawing.Point(830, 31);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 1;
            this.btnOkay.Text = "OK";
            this.ttRoadie.SetToolTip(this.btnOkay, "Save all changes");
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(911, 31);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.ttRoadie.SetToolTip(this.btnCancel, "Cancel all changes");
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dataGridViewDisableButtonColumn1
            // 
            this.dataGridViewDisableButtonColumn1.HeaderText = "Setup";
            this.dataGridViewDisableButtonColumn1.Name = "dataGridViewDisableButtonColumn1";
            this.dataGridViewDisableButtonColumn1.ReadOnly = true;
            this.dataGridViewDisableButtonColumn1.Text = "Setup";
            // 
            // dataGridViewDisableButtonColumn2
            // 
            this.dataGridViewDisableButtonColumn2.HeaderText = "Setup";
            this.dataGridViewDisableButtonColumn2.Name = "dataGridViewDisableButtonColumn2";
            this.dataGridViewDisableButtonColumn2.ReadOnly = true;
            this.dataGridViewDisableButtonColumn2.Text = "Setup";
            // 
            // dataGridViewDisableButtonColumn3
            // 
            this.dataGridViewDisableButtonColumn3.HeaderText = "Setup";
            this.dataGridViewDisableButtonColumn3.Name = "dataGridViewDisableButtonColumn3";
            this.dataGridViewDisableButtonColumn3.ReadOnly = true;
            this.dataGridViewDisableButtonColumn3.Text = "Setup";
            // 
            // btnTimer
            // 
            this.btnTimer.Enabled = true;
            this.btnTimer.Tick += new System.EventHandler(this.btnTimer_Tick);
            // 
            // VixenPlusRoadie
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 643);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.tcProfile);
            this.Controls.Add(this.gbProfiles);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "VixenPlusRoadie";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Vixen+ {Beta} - ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VixenPlusRoadie_FormClosing);
            this.Resize += new System.EventHandler(this.frmProfileManager_Resize);
            this.gbProfiles.ResumeLayout(false);
            this.tcProfile.ResumeLayout(false);
            this.tpChannels.ResumeLayout(false);
            this.tpPlugins.ResumeLayout(false);
            this.tpGroups.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComboBox cbProfiles;
        private GroupBox gbProfiles;
        private Button btnProfileDelete;
        private Button btnProfileRename;
        private Button btnProfileCopy;
        private Button btnProfileAdd;
        private TabControl tcProfile;
        private TabPage tpChannels;
        private Button btnOkay;
        private Button btnCancel;
        private TabPage tpGroups;
        private ToolTip ttRoadie;
        private Button btnProfileSave;
        private DataGridViewDisableButtonColumn dataGridViewDisableButtonColumn1;
        private Panel pGroups;
        private DataGridViewDisableButtonColumn dataGridViewDisableButtonColumn2;
        private DataGridViewDisableButtonColumn dataGridViewDisableButtonColumn3;
        private TabPage tpPlugins;
        private Panel pPlugIns;
        private Panel pChannels;
        private Timer btnTimer;
    }
}