namespace MIDIReader
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class TranscribeProgressDialog : Form
    {
        private IContainer components = null;
        private GroupBox groupBox1;
        private Label label1;
        private ProgressBar progressBarTrack;

        public TranscribeProgressDialog(int maxTracks, int maxEvents)
        {
            this.InitializeComponent();
            this.progressBarTrack.Maximum = maxTracks;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new GroupBox();
            this.progressBarTrack = new ProgressBar();
            this.label1 = new Label();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.progressBarTrack);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x18f, 0x51);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Transcription Progress";
            this.progressBarTrack.Location = new Point(0x40, 0x21);
            this.progressBarTrack.Name = "progressBarTrack";
            this.progressBarTrack.Size = new Size(0x13f, 0x13);
            this.progressBarTrack.TabIndex = 1;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(6, 0x27);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x23, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Track";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x1a7, 0x69);
            base.ControlBox = false;
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "TranscribeProgressDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "MIDI Transcription";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
        }

        public void StepTrack()
        {
            this.progressBarTrack.Value++;
            this.Refresh();
        }
    }
}

