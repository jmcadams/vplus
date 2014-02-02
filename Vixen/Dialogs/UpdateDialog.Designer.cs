namespace VixenPlus.Dialogs {
    partial class UpdateDialog {
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
            this.cbTurnOffAutoUpdate = new System.Windows.Forms.CheckBox();
            this.btnDownloadAndInstall = new System.Windows.Forms.Button();
            this.btnDownloadOnly = new System.Windows.Forms.Button();
            this.btnDoNotDownload = new System.Windows.Forms.Button();
            this.pbDownload = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // lblPrompt
            // 
            this.lblPrompt.Location = new System.Drawing.Point(13, 13);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(269, 87);
            this.lblPrompt.TabIndex = 0;
            this.lblPrompt.Text = "label1";
            // 
            // cbTurnOffAutoUpdate
            // 
            this.cbTurnOffAutoUpdate.AutoSize = true;
            this.cbTurnOffAutoUpdate.Location = new System.Drawing.Point(12, 150);
            this.cbTurnOffAutoUpdate.Name = "cbTurnOffAutoUpdate";
            this.cbTurnOffAutoUpdate.Size = new System.Drawing.Size(163, 17);
            this.cbTurnOffAutoUpdate.TabIndex = 1;
            this.cbTurnOffAutoUpdate.Text = "Turn off Auto Update Check.";
            this.cbTurnOffAutoUpdate.UseVisualStyleBackColor = true;
            this.cbTurnOffAutoUpdate.CheckedChanged += new System.EventHandler(this.cbTurnOffAutoUpdate_CheckedChanged);
            // 
            // btnDownloadAndInstall
            // 
            this.btnDownloadAndInstall.Location = new System.Drawing.Point(12, 103);
            this.btnDownloadAndInstall.Name = "btnDownloadAndInstall";
            this.btnDownloadAndInstall.Size = new System.Drawing.Size(75, 41);
            this.btnDownloadAndInstall.TabIndex = 2;
            this.btnDownloadAndInstall.Text = "Download and Install";
            this.btnDownloadAndInstall.UseVisualStyleBackColor = true;
            this.btnDownloadAndInstall.Click += new System.EventHandler(this.btnDownloadAndInstall_Click);
            // 
            // btnDownloadOnly
            // 
            this.btnDownloadOnly.Location = new System.Drawing.Point(93, 103);
            this.btnDownloadOnly.Name = "btnDownloadOnly";
            this.btnDownloadOnly.Size = new System.Drawing.Size(75, 41);
            this.btnDownloadOnly.TabIndex = 3;
            this.btnDownloadOnly.Text = "Download Only";
            this.btnDownloadOnly.UseVisualStyleBackColor = true;
            this.btnDownloadOnly.Click += new System.EventHandler(this.btnDownloadOnly_Click);
            // 
            // btnDoNotDownload
            // 
            this.btnDoNotDownload.Location = new System.Drawing.Point(207, 103);
            this.btnDoNotDownload.Name = "btnDoNotDownload";
            this.btnDoNotDownload.Size = new System.Drawing.Size(75, 41);
            this.btnDoNotDownload.TabIndex = 4;
            this.btnDoNotDownload.Text = "Do Not Download";
            this.btnDoNotDownload.UseVisualStyleBackColor = true;
            this.btnDoNotDownload.Click += new System.EventHandler(this.btnDoNotDownload_Click);
            // 
            // pbDownload
            // 
            this.pbDownload.Location = new System.Drawing.Point(12, 112);
            this.pbDownload.Name = "pbDownload";
            this.pbDownload.Size = new System.Drawing.Size(270, 23);
            this.pbDownload.TabIndex = 5;
            this.pbDownload.Visible = false;
            // 
            // UpdateDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 190);
            this.ControlBox = false;
            this.Controls.Add(this.pbDownload);
            this.Controls.Add(this.btnDoNotDownload);
            this.Controls.Add(this.btnDownloadOnly);
            this.Controls.Add(this.btnDownloadAndInstall);
            this.Controls.Add(this.cbTurnOffAutoUpdate);
            this.Controls.Add(this.lblPrompt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "UpdateDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "A new version is available.";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.CheckBox cbTurnOffAutoUpdate;
        private System.Windows.Forms.Button btnDownloadAndInstall;
        private System.Windows.Forms.Button btnDownloadOnly;
        private System.Windows.Forms.Button btnDoNotDownload;
        private System.Windows.Forms.ProgressBar pbDownload;
    }
}