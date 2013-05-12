namespace Preview{
    using System;
    using System.Windows.Forms;
    using System.Drawing;
    using System.ComponentModel;
    using System.Collections;

    public partial class PreviewDialog{

        #region Windows Form Designer generated code
        private PictureBox pictureBoxShowGrid;

        private void InitializeComponent() {
            pictureBoxShowGrid = new PictureBox();
            ((ISupportInitialize) pictureBoxShowGrid).BeginInit();
            SuspendLayout();
            pictureBoxShowGrid.BackColor = Color.Transparent;
            pictureBoxShowGrid.BackgroundImageLayout = ImageLayout.None;
            pictureBoxShowGrid.Location = new Point(0, 0);
            pictureBoxShowGrid.Name = "_pictureBoxShowGrid";
            pictureBoxShowGrid.Size = new Size(576, 288);
            pictureBoxShowGrid.TabIndex = 14;
            pictureBoxShowGrid.TabStop = false;
            pictureBoxShowGrid.Paint += pictureBoxShowGrid_Paint;
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(576, 286);
            ControlBox = false;
            Controls.Add(pictureBoxShowGrid);
            Name = "PreviewDialog";
            StartPosition = FormStartPosition.Manual;
            Text = "Sequence Preview";
            FormClosing += PreviewDialog_FormClosing;
            ((ISupportInitialize) pictureBoxShowGrid).EndInit();
            ResumeLayout(false);
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
