namespace VixenPlus.Dialogs {
    sealed partial class UpdateDialog {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.lblPrompt = new System.Windows.Forms.Label();
            this.btnInstallNow = new System.Windows.Forms.Button();
            this.btnDownloadOnly = new System.Windows.Forms.Button();
            this.btnAskMeLater = new System.Windows.Forms.Button();
            this.pbDownload = new System.Windows.Forms.ProgressBar();
            this.btnReleaseNotes = new System.Windows.Forms.Button();
            this.btnSkipVersion = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblPrompt
            // 
            this.lblPrompt.Location = new System.Drawing.Point(13, 13);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(313, 92);
            this.lblPrompt.TabIndex = 0;
            this.lblPrompt.Text = "Words, they go here.";
            // 
            // btnInstallNow
            // 
            this.btnInstallNow.Location = new System.Drawing.Point(12, 108);
            this.btnInstallNow.Name = "btnInstallNow";
            this.btnInstallNow.Size = new System.Drawing.Size(91, 23);
            this.btnInstallNow.TabIndex = 2;
            this.btnInstallNow.Text = "Install Now";
            this.btnInstallNow.UseVisualStyleBackColor = true;
            this.btnInstallNow.Click += new System.EventHandler(this.btnInstallNow_Click);
            // 
            // btnDownloadOnly
            // 
            this.btnDownloadOnly.Location = new System.Drawing.Point(121, 108);
            this.btnDownloadOnly.Name = "btnDownloadOnly";
            this.btnDownloadOnly.Size = new System.Drawing.Size(91, 23);
            this.btnDownloadOnly.TabIndex = 3;
            this.btnDownloadOnly.Text = "Download Only";
            this.btnDownloadOnly.UseVisualStyleBackColor = true;
            this.btnDownloadOnly.Click += new System.EventHandler(this.btnDownloadOnly_Click);
            // 
            // btnAskMeLater
            // 
            this.btnAskMeLater.Location = new System.Drawing.Point(235, 108);
            this.btnAskMeLater.Name = "btnAskMeLater";
            this.btnAskMeLater.Size = new System.Drawing.Size(91, 23);
            this.btnAskMeLater.TabIndex = 4;
            this.btnAskMeLater.Text = "Ask Me Later";
            this.btnAskMeLater.UseVisualStyleBackColor = true;
            this.btnAskMeLater.Click += new System.EventHandler(this.btnAskMeLater_Click);
            // 
            // pbDownload
            // 
            this.pbDownload.Location = new System.Drawing.Point(12, 166);
            this.pbDownload.Name = "pbDownload";
            this.pbDownload.Size = new System.Drawing.Size(314, 23);
            this.pbDownload.TabIndex = 5;
            this.pbDownload.Visible = false;
            // 
            // btnReleaseNotes
            // 
            this.btnReleaseNotes.Location = new System.Drawing.Point(12, 137);
            this.btnReleaseNotes.Name = "btnReleaseNotes";
            this.btnReleaseNotes.Size = new System.Drawing.Size(147, 23);
            this.btnReleaseNotes.TabIndex = 6;
            this.btnReleaseNotes.Text = "View Release Notes";
            this.btnReleaseNotes.UseVisualStyleBackColor = true;
            this.btnReleaseNotes.Click += new System.EventHandler(this.btnReleaseNotes_Click);
            // 
            // btnSkipVersion
            // 
            this.btnSkipVersion.Location = new System.Drawing.Point(179, 137);
            this.btnSkipVersion.Name = "btnSkipVersion";
            this.btnSkipVersion.Size = new System.Drawing.Size(147, 23);
            this.btnSkipVersion.TabIndex = 7;
            this.btnSkipVersion.Text = "Skip This Version";
            this.btnSkipVersion.UseVisualStyleBackColor = true;
            this.btnSkipVersion.Click += new System.EventHandler(this.btnSkipVersion_Click);
            // 
            // UpdateDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 201);
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
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Checking for available updates";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.UpdateDialog_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.Button btnInstallNow;
        private System.Windows.Forms.Button btnDownloadOnly;
        private System.Windows.Forms.Button btnAskMeLater;
        private System.Windows.Forms.ProgressBar pbDownload;
        private System.Windows.Forms.Button btnReleaseNotes;
        private System.Windows.Forms.Button btnSkipVersion;
    }
}