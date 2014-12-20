using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VixenEditor {
    internal partial class IntensityAdjustDialog {
        private IContainer components = null;

        #region Windows Form Designer generated code
        private Label lblDelta;

        private void InitializeComponent() {
            this.lblDelta = new Label();
            this.SuspendLayout();
            // 
            // lblDelta
            // 
            this.lblDelta.Font = new Font("Microsoft Sans Serif", 20F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.lblDelta.Location = new Point(1, 10);
            this.lblDelta.Name = "lblDelta";
            this.lblDelta.Size = new Size(121, 31);
            this.lblDelta.TabIndex = 0;
            this.lblDelta.Text = "+100%";
            this.lblDelta.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // IntensityAdjustDialog
            // 
            this.ClientSize = new Size(123, 50);
            this.ControlBox = false;
            this.Controls.Add(this.lblDelta);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "IntensityAdjustDialog";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Intensity Adjustment";
            this.TopMost = true;
            this.VisibleChanged += new EventHandler(this.IntensityAdjustDialog_VisibleChanged);
            this.KeyDown += new KeyEventHandler(this.IntensityAdjustDialog_KeyDown);
            this.KeyUp += new KeyEventHandler(this.IntensityAdjustDialog_KeyUp);
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