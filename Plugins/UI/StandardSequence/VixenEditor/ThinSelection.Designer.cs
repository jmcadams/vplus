using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenEditor {
    internal partial class ThinSelection {
        private IContainer components = null;

        #region Windows Form Designer generated code
        private ListBox listBox;

        private void InitializeComponent() {
            this.listBox = new ListBox();
            this.SuspendLayout();
            // 
            // listBox
            // 
            this.listBox.Dock = DockStyle.Fill;
            this.listBox.DrawMode = DrawMode.OwnerDrawFixed;
            this.listBox.FormattingEnabled = true;
            this.listBox.ItemHeight = 17;
            this.listBox.Location = new Point(0, 0);
            this.listBox.Name = "listBox";
            this.listBox.Size = new Size(168, 260);
            this.listBox.TabIndex = 0;
            this.listBox.DrawItem += new DrawItemEventHandler(this.listBox_DrawItem);
            this.listBox.SelectedIndexChanged += new EventHandler(this.listBox_SelectedIndexChanged);
            this.listBox.DoubleClick += new EventHandler(this.listBox_DoubleClick);
            // 
            // ThinSelection
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(168, 260);
            this.ControlBox = false;
            this.Controls.Add(this.listBox);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ThinSelection";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.KeyDown += new KeyEventHandler(this.ThinSelection_KeyDown);
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