using System.Windows.Forms;

namespace VixenEditor {
    internal partial class IntensityAdjustDialog {
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code
        private Label lblDelta;

        private void InitializeComponent() {
            this.lblDelta = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblDelta
            // 
            this.lblDelta.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDelta.Location = new System.Drawing.Point(1, 10);
            this.lblDelta.Name = "lblDelta";
            this.lblDelta.Size = new System.Drawing.Size(121, 31);
            this.lblDelta.TabIndex = 0;
            this.lblDelta.Text = "+100%";
            this.lblDelta.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IntensityAdjustDialog
            // 
            this.ClientSize = new System.Drawing.Size(123, 50);
            this.ControlBox = false;
            this.Controls.Add(this.lblDelta);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "IntensityAdjustDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Intensity Adjustment";
            this.TopMost = true;
            this.VisibleChanged += new System.EventHandler(this.IntensityAdjustDialog_VisibleChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.IntensityAdjustDialog_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.IntensityAdjustDialog_KeyUp);
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