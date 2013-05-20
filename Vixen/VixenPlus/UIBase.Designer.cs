using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;

namespace VixenPlus {
    public partial class UIBase {
        private IContainer components = null;

        #region Windows Form Designer generated code

        private void InitializeComponent() {
            base.SuspendLayout();
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(740, 0x1bc);
            base.Name = "UIBase";
            this.Text = "UIBase";
            this.Icon = global::Properties.Resources.VixenPlus;
            base.ResumeLayout(false);
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
