namespace VixenEditor
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Timers;
    using System.Windows.Forms;

    internal class EffectFrequencyDialog : Form
    {
        private Button buttonCancel;
        private Button buttonOK;
        private IContainer components = null;
        private GroupBox groupBox1;
        private SolidBrush m_brush = null;
        private System.Timers.Timer m_drawTimer;
        private FrequencyEffectGenerator m_effectGenerator;
        private byte[,] m_effectValues;
        private int m_frequency = 0;
        private int m_maxColumn;
        private MethodInvoker m_refreshInvoker;
        private int m_tickCount;
        private Point[][] m_treePoints;
        private PictureBox pictureBoxExample;
        private TrackBar trackBarFrequency;

        public EffectFrequencyDialog(string effectName, int maxFrequency, FrequencyEffectGenerator effectGenerator)
        {
            this.InitializeComponent();
            this.m_brush = new SolidBrush(Color.Black);
            this.Text = effectName;
            this.groupBox1.Text = effectName + " Frequency";
            this.trackBarFrequency.Maximum = maxFrequency;
            this.m_effectGenerator = effectGenerator;
            this.m_refreshInvoker = new MethodInvoker(this.pictureBoxExample.Refresh);
            this.m_effectValues = new byte[4, maxFrequency * 5];
            this.m_maxColumn = this.m_effectValues.GetLength(1);
            effectGenerator(this.m_effectValues, new int[] { 1 });
            this.m_tickCount = 0;
            Point[][] pointArray = new Point[4][];
            Point[] pointArray2 = new Point[] { new Point(0x16, 0x2e), new Point(0x25, 0x5b), new Point(7, 0x5b) };
            pointArray[0] = pointArray2;
            pointArray2 = new Point[] { new Point(0x43, 0x2e), new Point(0x52, 0x5b), new Point(0x34, 0x5b) };
            pointArray[1] = pointArray2;
            pointArray2 = new Point[] { new Point(0x70, 0x2e), new Point(0x7f, 0x5b), new Point(0x61, 0x5b) };
            pointArray[2] = pointArray2;
            pointArray2 = new Point[] { new Point(0x9d, 0x2e), new Point(0xac, 0x5b), new Point(0x8e, 0x5b) };
            pointArray[3] = pointArray2;
            this.m_treePoints = pointArray;
            this.m_drawTimer = new System.Timers.Timer(100.0);
            this.m_drawTimer.Elapsed += new ElapsedEventHandler(this.m_drawTimer_Elapsed);
            this.m_drawTimer.Start();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            if (this.m_brush != null)
            {
                this.m_brush.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EffectFrequencyDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.m_drawTimer.Stop();
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new GroupBox();
            this.pictureBoxExample = new PictureBox();
            this.trackBarFrequency = new TrackBar();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.groupBox1.SuspendLayout();
            ((ISupportInitialize) this.pictureBoxExample).BeginInit();
            this.trackBarFrequency.BeginInit();
            base.SuspendLayout();
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.pictureBoxExample);
            this.groupBox1.Controls.Add(this.trackBarFrequency);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x10c, 0xa8);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            this.pictureBoxExample.BackColor = Color.Black;
            this.pictureBoxExample.Location = new Point(0x4a, 0x13);
            this.pictureBoxExample.Name = "pictureBoxExample";
            this.pictureBoxExample.Size = new Size(0xb3, 0x89);
            this.pictureBoxExample.TabIndex = 1;
            this.pictureBoxExample.TabStop = false;
            this.pictureBoxExample.Paint += new PaintEventHandler(this.pictureBoxExample_Paint);
            this.trackBarFrequency.Location = new Point(12, 0x13);
            this.trackBarFrequency.Name = "trackBarFrequency";
            this.trackBarFrequency.Orientation = Orientation.Vertical;
            this.trackBarFrequency.Size = new Size(0x2d, 0x93);
            this.trackBarFrequency.TabIndex = 0;
            this.trackBarFrequency.TickStyle = TickStyle.Both;
            this.trackBarFrequency.Scroll += new EventHandler(this.trackBarFrequency_Scroll);
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(0x7c, 0xba);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0xcd, 0xba);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x124, 0xd9);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Name = "EffectFrequencyDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "EffectFrequencyDialog";
            base.FormClosing += new FormClosingEventHandler(this.EffectFrequencyDialog_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((ISupportInitialize) this.pictureBoxExample).EndInit();
            this.trackBarFrequency.EndInit();
            base.ResumeLayout(false);
        }

        private void m_drawTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.m_frequency != 0)
            {
                base.BeginInvoke(this.m_refreshInvoker);
                if (++this.m_tickCount == this.m_maxColumn)
                {
                    this.Regenerate();
                }
            }
        }

        private void pictureBoxExample_Paint(object sender, PaintEventArgs e)
        {
            if (this.m_drawTimer.Enabled)
            {
                this.m_brush.Color = Color.FromArgb(this.m_effectValues[0, this.m_tickCount], Color.Red);
                e.Graphics.FillPolygon(this.m_brush, this.m_treePoints[0]);
                this.m_brush.Color = Color.FromArgb(this.m_effectValues[1, this.m_tickCount], Color.Green);
                e.Graphics.FillPolygon(this.m_brush, this.m_treePoints[1]);
                this.m_brush.Color = Color.FromArgb(this.m_effectValues[2, this.m_tickCount], Color.Blue);
                e.Graphics.FillPolygon(this.m_brush, this.m_treePoints[2]);
                this.m_brush.Color = Color.FromArgb(this.m_effectValues[3, this.m_tickCount], Color.White);
                e.Graphics.FillPolygon(this.m_brush, this.m_treePoints[3]);
            }
        }

        private void Regenerate()
        {
            if (this.m_frequency != 0)
            {
                this.m_drawTimer.Stop();
                this.m_effectGenerator(this.m_effectValues, new int[] { this.m_frequency });
                this.m_tickCount = 0;
                this.m_drawTimer.Start();
            }
        }

        private void trackBarFrequency_Scroll(object sender, EventArgs e)
        {
            this.m_frequency = this.trackBarFrequency.Value;
            this.Regenerate();
        }

        public int Frequency
        {
            get
            {
                return this.trackBarFrequency.Value;
            }
        }
    }
}

