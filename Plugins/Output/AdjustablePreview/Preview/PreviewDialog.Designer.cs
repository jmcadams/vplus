namespace Preview{
    using System;
    using System.Windows.Forms;
    using System.Drawing;
    using System.ComponentModel;
    using System.Collections;

    public partial class PreviewDialog{
        private IContainer components;

        #region Windows Form Designer generated code
        private PictureBox _pictureBoxShowGrid;

        private void InitializeComponent() {
            _pictureBoxShowGrid = new PictureBox();
            ((ISupportInitialize) _pictureBoxShowGrid).BeginInit();
            SuspendLayout();
            _pictureBoxShowGrid.BackColor = Color.Transparent;
            _pictureBoxShowGrid.BackgroundImageLayout = ImageLayout.None;
            _pictureBoxShowGrid.Location = new Point(0, 0);
            _pictureBoxShowGrid.Name = "_pictureBoxShowGrid";
            _pictureBoxShowGrid.Size = new Size(0x240, 0x120);
            _pictureBoxShowGrid.TabIndex = 14;
            _pictureBoxShowGrid.TabStop = false;
            _pictureBoxShowGrid.Paint += pictureBoxShowGrid_Paint;
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(0x240, 0x11e);
            ControlBox = false;
            Controls.Add(_pictureBoxShowGrid);
            Name = "PreviewDialog";
            StartPosition = FormStartPosition.Manual;
            Text = "Sequence Preview";
            FormClosing += PreviewDialog_FormClosing;
            ((ISupportInitialize) _pictureBoxShowGrid).EndInit();
            ResumeLayout(false);
        }
        #endregion

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
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
