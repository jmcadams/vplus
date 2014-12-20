using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenPlus.Dialogs {
    sealed partial class UpdateDialog {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.lblPrompt = new Label();
            this.btnInstallNow = new Button();
            this.btnDownloadOnly = new Button();
            this.btnAskMeLater = new Button();
            this.pbDownload = new ProgressBar();
            this.btnReleaseNotes = new Button();
            this.btnSkipVersion = new Button();
            this.SuspendLayout();
            // 
            // lblPrompt
            // 
            this.lblPrompt.Location = new Point(13, 13);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new Size(313, 92);
            this.lblPrompt.TabIndex = 0;
            this.lblPrompt.Text = "Words, they go here.";
            // 
            // btnInstallNow
            // 
            this.btnInstallNow.Location = new Point(12, 108);
            this.btnInstallNow.Name = "btnInstallNow";
            this.btnInstallNow.Size = new Size(91, 23);
            this.btnInstallNow.TabIndex = 2;
            this.btnInstallNow.Text = "Install Now";
            this.btnInstallNow.UseVisualStyleBackColor = true;
            this.btnInstallNow.Click += new EventHandler(this.btnInstallNow_Click);
            // 
            // btnDownloadOnly
            // 
            this.btnDownloadOnly.Location = new Point(121, 108);
            this.btnDownloadOnly.Name = "btnDownloadOnly";
            this.btnDownloadOnly.Size = new Size(91, 23);
            this.btnDownloadOnly.TabIndex = 3;
            this.btnDownloadOnly.Text = "Download Only";
            this.btnDownloadOnly.UseVisualStyleBackColor = true;
            this.btnDownloadOnly.Click += new EventHandler(this.btnDownloadOnly_Click);
            // 
            // btnAskMeLater
            // 
            this.btnAskMeLater.Location = new Point(235, 108);
            this.btnAskMeLater.Name = "btnAskMeLater";
            this.btnAskMeLater.Size = new Size(91, 23);
            this.btnAskMeLater.TabIndex = 4;
            this.btnAskMeLater.Text = "Ask Me Later";
            this.btnAskMeLater.UseVisualStyleBackColor = true;
            this.btnAskMeLater.Click += new EventHandler(this.btnAskMeLater_Click);
            // 
            // pbDownload
            // 
            this.pbDownload.Location = new Point(12, 166);
            this.pbDownload.Name = "pbDownload";
            this.pbDownload.Size = new Size(314, 23);
            this.pbDownload.TabIndex = 5;
            this.pbDownload.Visible = false;
            // 
            // btnReleaseNotes
            // 
            this.btnReleaseNotes.Location = new Point(12, 137);
            this.btnReleaseNotes.Name = "btnReleaseNotes";
            this.btnReleaseNotes.Size = new Size(147, 23);
            this.btnReleaseNotes.TabIndex = 6;
            this.btnReleaseNotes.Text = "View Release Notes";
            this.btnReleaseNotes.UseVisualStyleBackColor = true;
            this.btnReleaseNotes.Click += new EventHandler(this.btnReleaseNotes_Click);
            // 
            // btnSkipVersion
            // 
            this.btnSkipVersion.Location = new Point(179, 137);
            this.btnSkipVersion.Name = "btnSkipVersion";
            this.btnSkipVersion.Size = new Size(147, 23);
            this.btnSkipVersion.TabIndex = 7;
            this.btnSkipVersion.Text = "Skip This Version";
            this.btnSkipVersion.UseVisualStyleBackColor = true;
            this.btnSkipVersion.Click += new EventHandler(this.btnSkipVersion_Click);
            // 
            // UpdateDialog
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(338, 201);
            this.ControlBox = false;
            this.Controls.Add(this.btnSkipVersion);
            this.Controls.Add(this.btnReleaseNotes);
            this.Controls.Add(this.pbDownload);
            this.Controls.Add(this.btnAskMeLater);
            this.Controls.Add(this.btnDownloadOnly);
            this.Controls.Add(this.btnInstallNow);
            this.Controls.Add(this.lblPrompt);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdateDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = SizeGripStyle.Hide;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Checking for available updates";
            this.TopMost = true;
            this.Shown += new EventHandler(this.UpdateDialog_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private Label lblPrompt;
        private Button btnInstallNow;
        private Button btnDownloadOnly;
        private Button btnAskMeLater;
        private ProgressBar pbDownload;
        private Button btnReleaseNotes;
        private Button btnSkipVersion;
    }
}