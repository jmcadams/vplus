namespace VixenEditor
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class AudioSpeedDialog : Form
    {
        private Button buttonSet;
        private IContainer components = null;
        private Label labelValue;
        private TrackBar trackBar;

        public AudioSpeedDialog()
        {
            this.InitializeComponent();
        }

        private void AudioSpeedDialog_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            if (e.KeyChar == '\r')
            {
                base.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else if (e.KeyChar == '\x001b')
            {
                base.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
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
            this.trackBar = new TrackBar();
            this.labelValue = new Label();
            this.buttonSet = new Button();
            this.trackBar.BeginInit();
            base.SuspendLayout();
            this.trackBar.Location = new Point(12, 12);
            this.trackBar.Maximum = 100;
            this.trackBar.Minimum = 1;
            this.trackBar.Name = "trackBar";
            this.trackBar.Orientation = Orientation.Vertical;
            this.trackBar.Size = new Size(0x2d, 240);
            this.trackBar.TabIndex = 0;
            this.trackBar.Value = 100;
            this.trackBar.Scroll += new EventHandler(this.trackBar_Scroll);
            this.labelValue.AutoSize = true;
            this.labelValue.Font = new Font("Microsoft Sans Serif", 14f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labelValue.Location = new Point(0x3f, 110);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new Size(0x37, 0x18);
            this.labelValue.TabIndex = 1;
            this.labelValue.Text = "100%";
            this.buttonSet.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.buttonSet.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonSet.Location = new Point(0x1a, 0x106);
            this.buttonSet.Name = "buttonSet";
            this.buttonSet.Size = new Size(0x4b, 0x17);
            this.buttonSet.TabIndex = 2;
            this.buttonSet.Text = "Set";
            this.buttonSet.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x7f, 0x129);
            base.ControlBox = false;
            base.Controls.Add(this.buttonSet);
            base.Controls.Add(this.labelValue);
            base.Controls.Add(this.trackBar);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            base.KeyPreview = true;
            base.Name = "AudioSpeedDialog";
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "Audio Playback Speed";
            base.KeyPress += new KeyPressEventHandler(this.AudioSpeedDialog_KeyPress);
            this.trackBar.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            this.labelValue.Text = this.trackBar.Value + "%";
        }

        public float Rate
        {
            get
            {
                return (((float) this.trackBar.Value) / 100f);
            }
            set
            {
                if ((value > 0f) && (value <= 1f))
                {
                    this.trackBar.Value = (int) (value * 100f);
                    this.labelValue.Text = this.trackBar.Value + "%";
                }
            }
        }
    }
}

