using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenEditor {
    internal partial class ProgressDialog {
        private IContainer components = null;

        #region Windows Form Designer generated code
        private Label lblMessage;
        private Panel panel1;

        private void InitializeComponent() {
            this.lblMessage = new Label();
            this.panel1 = new Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new Point(11, 8);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new Size(309, 54);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Loading...";
            this.lblMessage.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.BorderStyle = BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblMessage);
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(333, 72);
            this.panel1.TabIndex = 1;
            // 
            // ProgressDialog
            // 
            this.AutoSize = true;
            this.ClientSize = new Size(333, 72);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Name = "ProgressDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}