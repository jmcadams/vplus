namespace VixenEditor {
    using System;
    using System.Windows.Forms;
    using System.Drawing;
    using System.Collections;

    internal partial class ThinSelection {
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code
        private ListBox listBox;

        private void InitializeComponent() {
            this.listBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBox.FormattingEnabled = true;
            this.listBox.ItemHeight = 17;
            this.listBox.Location = new System.Drawing.Point(0, 0);
            this.listBox.Name = "listBox1";
            this.listBox.Size = new System.Drawing.Size(168, 260);
            this.listBox.TabIndex = 0;
            this.listBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox_DrawItem);
            this.listBox.SelectedIndexChanged += new System.EventHandler(this.listBox_SelectedIndexChanged);
            this.listBox.DoubleClick += new System.EventHandler(this.listBox_DoubleClick);
            // 
            // ThinSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(168, 260);
            this.ControlBox = false;
            this.Controls.Add(this.listBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ThinSelection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ThinSelection_KeyDown);
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