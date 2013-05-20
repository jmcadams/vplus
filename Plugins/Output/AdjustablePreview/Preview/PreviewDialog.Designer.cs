namespace Preview {
    using System;
    using System.Windows.Forms;
    using System.Drawing;
    using System.ComponentModel;
    using System.Collections;

    public partial class PreviewDialog {

        #region Windows Form Designer generated code

        private PictureBox pictureBoxShowGrid;


        private void InitializeComponent() {
            this.pictureBoxShowGrid = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxShowGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxShowGrid
            // 
            this.pictureBoxShowGrid.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxShowGrid.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBoxShowGrid.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxShowGrid.Name = "pictureBoxShowGrid";
            this.pictureBoxShowGrid.Size = new System.Drawing.Size(576, 288);
            this.pictureBoxShowGrid.TabIndex = 14;
            this.pictureBoxShowGrid.TabStop = false;
            this.pictureBoxShowGrid.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxShowGrid_Paint);
            // 
            // PreviewDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(576, 286);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBoxShowGrid);
            this.Icon = global::Properties.Resources.VixenPlus;
            this.Name = "PreviewDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Sequence Preview";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PreviewDialog_FormClosing);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBoxShowGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected override void Dispose(bool disposing) {
            if (_channelBrush != null) {
                _channelBrush.Dispose();
            }
            if (_originalBackground != null) {
                _originalBackground.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
