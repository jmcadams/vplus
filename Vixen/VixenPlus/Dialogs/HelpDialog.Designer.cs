namespace VixenPlus.Dialogs{
    using System.Windows.Forms;

    public sealed partial class HelpDialog{
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code
        private LinkLabel linkLabelClose;

        private void InitializeComponent()
        {
            this.linkLabelClose = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // linkLabelClose
            // 
            this.linkLabelClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabelClose.AutoSize = true;
            this.linkLabelClose.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelClose.Location = new System.Drawing.Point(448, 230);
            this.linkLabelClose.Name = "linkLabelClose";
            this.linkLabelClose.Size = new System.Drawing.Size(33, 13);
            this.linkLabelClose.TabIndex = 0;
            this.linkLabelClose.TabStop = true;
            this.linkLabelClose.Text = "Close";
            this.linkLabelClose.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelClose_LinkClicked);
            // 
            // HelpDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(493, 252);
            this.ControlBox = false;
            this.Controls.Add(this.linkLabelClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "HelpDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HelpDialog";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HelpDialog_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            if (this._bigFont != null)
            {
                this._bigFont.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
