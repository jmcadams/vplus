using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using VixenPlusCommon.Properties;

namespace Preview {
    public partial class PreviewDialog {

        #region Windows Form Designer generated code

        private PictureBox pictureBoxShowGrid;


        private void InitializeComponent() {
            this.pictureBoxShowGrid = new PictureBox();
            ((ISupportInitialize) (this.pictureBoxShowGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxShowGrid
            // 
            this.pictureBoxShowGrid.BackColor = Color.Transparent;
            this.pictureBoxShowGrid.BackgroundImageLayout = ImageLayout.None;
            this.pictureBoxShowGrid.Location = new Point(0, 0);
            this.pictureBoxShowGrid.Name = "pictureBoxShowGrid";
            this.pictureBoxShowGrid.Size = new Size(576, 288);
            this.pictureBoxShowGrid.TabIndex = 14;
            this.pictureBoxShowGrid.TabStop = false;
            this.pictureBoxShowGrid.Paint += new PaintEventHandler(this.pictureBoxShowGrid_Paint);
            // 
            // PreviewDialog
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            this.ClientSize = new Size(576, 286);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBoxShowGrid);
            this.Icon = Resources.VixenPlus;
            this.Name = "PreviewDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.Manual;
            this.Text = "Sequence AdjustablePreview";
            this.FormClosing += new FormClosingEventHandler(this.PreviewDialog_FormClosing);
            ((ISupportInitialize) (this.pictureBoxShowGrid)).EndInit();
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
