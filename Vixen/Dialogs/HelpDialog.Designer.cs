using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenPlus.Dialogs{
    public sealed partial class HelpDialog{
        private IContainer components = null;

        #region Windows Form Designer generated code
        private LinkLabel linkLabelClose;

        private void InitializeComponent()
        {
            this.linkLabelClose = new LinkLabel();
            this.SuspendLayout();
            // 
            // linkLabelClose
            // 
            this.linkLabelClose.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.linkLabelClose.AutoSize = true;
            this.linkLabelClose.LinkBehavior = LinkBehavior.NeverUnderline;
            this.linkLabelClose.Location = new Point(448, 230);
            this.linkLabelClose.Name = "linkLabelClose";
            this.linkLabelClose.Size = new Size(33, 13);
            this.linkLabelClose.TabIndex = 0;
            this.linkLabelClose.TabStop = true;
            this.linkLabelClose.Text = "Close";
            this.linkLabelClose.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabelClose_LinkClicked);
            // 
            // HelpDialog
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = Color.White;
            this.ClientSize = new Size(493, 252);
            this.ControlBox = false;
            this.Controls.Add(this.linkLabelClose);
            this.FormBorderStyle = FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "HelpDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "HelpDialog";
            this.KeyPress += new KeyPressEventHandler(this.HelpDialog_KeyPress);
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
