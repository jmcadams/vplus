using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenPlus {
    public partial class UIBase {
        private IContainer components = null;

        #region Windows Form Designer generated code


        private void InitializeComponent() {
            this.SuspendLayout();
            // 
            // UIBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 444);
            this.Name = "UIBase";
            this.Text = "UIBase";
            this.ResumeLayout(false);

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