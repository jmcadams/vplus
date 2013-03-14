namespace VixenEditor
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Timers;
    using System.Windows.Forms;

    internal class SparkleParamsDialog : Form
    {
        private Button buttonCancel;
        private Button buttonOK;
        private IContainer components = null;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private bool m_actualLevels;
        private SolidBrush m_brush = null;
        private int m_decay = 0;
        private System.Timers.Timer m_drawTimer;
        private FrequencyEffectGenerator m_effectGenerator;
        private byte[,] m_effectValues;
        private int m_frequency = 0;
        private int m_max = 100;
        private int m_maxColumn;
        private int m_min = 0;
        private MethodInvoker m_refreshInvoker;
        private int m_tickCount;
        private Point[][] m_treePoints;
        private NumericUpDown numericUpDownMax;
        private NumericUpDown numericUpDownMin;
        private PictureBox pictureBoxExample;
        private TrackBar trackBarDecay;
        private TrackBar trackBarFrequency;

        public SparkleParamsDialog(int maxFrequency, FrequencyEffectGenerator effectGenerator, byte sequenceMin, byte sequenceMax, byte currentDrawingIntensity, bool actualLevels)
        {
            this.InitializeComponent();
            this.m_actualLevels = actualLevels;
            this.m_brush = new SolidBrush(Color.Black);
            this.trackBarFrequency.Maximum = maxFrequency;
            this.m_effectGenerator = effectGenerator;
            this.m_refreshInvoker = new MethodInvoker(this.pictureBoxExample.Refresh);
            this.m_effectValues = new byte[4, maxFrequency * 5];
            this.m_maxColumn = this.m_effectValues.GetLength(1);
            this.m_min = sequenceMin;
            this.m_max = currentDrawingIntensity;
            int[] effectParameters = new int[4];
            effectParameters[2] = this.m_min;
            effectParameters[3] = this.m_max;
            effectGenerator(this.m_effectValues, effectParameters);
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
            if (actualLevels)
            {
                this.numericUpDownMin.Minimum = this.numericUpDownMax.Minimum = this.numericUpDownMin.Value = sequenceMin;
                this.numericUpDownMin.Maximum = this.numericUpDownMax.Maximum = sequenceMax;
                this.numericUpDownMax.Value = this.m_max;
            }
            else
            {
                this.numericUpDownMin.Minimum = this.numericUpDownMax.Minimum = this.numericUpDownMin.Value = (int) Math.Round((double) ((sequenceMin * 100f) / 255f), MidpointRounding.AwayFromZero);
                this.numericUpDownMin.Maximum = this.numericUpDownMax.Maximum = (int) Math.Round((double) ((sequenceMax * 100f) / 255f), MidpointRounding.AwayFromZero);
                this.numericUpDownMax.Value = (int) Math.Round((double) ((this.m_max * 100f) / 255f), MidpointRounding.AwayFromZero);
            }
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

        private void InitializeComponent()
        {
            this.buttonCancel = new Button();
            this.buttonOK = new Button();
            this.groupBox1 = new GroupBox();
            this.numericUpDownMax = new NumericUpDown();
            this.label4 = new Label();
            this.numericUpDownMin = new NumericUpDown();
            this.label3 = new Label();
            this.trackBarDecay = new TrackBar();
            this.label2 = new Label();
            this.label1 = new Label();
            this.pictureBoxExample = new PictureBox();
            this.trackBarFrequency = new TrackBar();
            this.groupBox1.SuspendLayout();
            this.numericUpDownMax.BeginInit();
            this.numericUpDownMin.BeginInit();
            this.trackBarDecay.BeginInit();
            ((ISupportInitialize) this.pictureBoxExample).BeginInit();
            this.trackBarFrequency.BeginInit();
            base.SuspendLayout();
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x125, 0x117);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(0xd4, 0x117);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.numericUpDownMax);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.numericUpDownMin);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.trackBarDecay);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.pictureBoxExample);
            this.groupBox1.Controls.Add(this.trackBarFrequency);
            this.groupBox1.Location = new Point(12, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x164, 0x107);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sparkle Parameters";
            this.numericUpDownMax.Location = new Point(0x114, 0x79);
            this.numericUpDownMax.Name = "numericUpDownMax";
            this.numericUpDownMax.Size = new Size(0x34, 20);
            this.numericUpDownMax.TabIndex = 8;
            int[] bits = new int[4];
            bits[0] = 100;
            this.numericUpDownMax.Value = new decimal(bits);
            this.numericUpDownMax.ValueChanged += new EventHandler(this.numericUpDownMax_ValueChanged);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x111, 0x69);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x33, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Maximum";
            this.numericUpDownMin.Location = new Point(0x114, 0x42);
            this.numericUpDownMin.Name = "numericUpDownMin";
            this.numericUpDownMin.Size = new Size(0x34, 20);
            this.numericUpDownMin.TabIndex = 6;
            this.numericUpDownMin.ValueChanged += new EventHandler(this.numericUpDownMin_ValueChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x111, 50);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x30, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Minimum";
            this.trackBarDecay.LargeChange = 0x3e8;
            this.trackBarDecay.Location = new Point(0x49, 0xc2);
            this.trackBarDecay.Maximum = 0x1388;
            this.trackBarDecay.Name = "trackBarDecay";
            this.trackBarDecay.Size = new Size(0xc5, 0x2d);
            this.trackBarDecay.SmallChange = 500;
            this.trackBarDecay.TabIndex = 4;
            this.trackBarDecay.TickFrequency = 500;
            this.trackBarDecay.TickStyle = TickStyle.Both;
            this.trackBarDecay.Scroll += new EventHandler(this.trackBarDecay_Scroll);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0x4f, 0xb3);
            this.label2.Name = "label2";
            this.label2.Size = new Size(60, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Decay time";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(12, 0x26);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x39, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Frequency";
            this.pictureBoxExample.BackColor = Color.Black;
            this.pictureBoxExample.Location = new Point(0x52, 0x27);
            this.pictureBoxExample.Name = "pictureBoxExample";
            this.pictureBoxExample.Size = new Size(0xb3, 0x89);
            this.pictureBoxExample.TabIndex = 1;
            this.pictureBoxExample.TabStop = false;
            this.pictureBoxExample.Paint += new PaintEventHandler(this.pictureBoxExample_Paint);
            this.trackBarFrequency.Location = new Point(20, 0x36);
            this.trackBarFrequency.Name = "trackBarFrequency";
            this.trackBarFrequency.Orientation = Orientation.Vertical;
            this.trackBarFrequency.Size = new Size(0x2d, 0x84);
            this.trackBarFrequency.TabIndex = 0;
            this.trackBarFrequency.TickStyle = TickStyle.Both;
            this.trackBarFrequency.Scroll += new EventHandler(this.trackBarFrequency_Scroll);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(380, 0x138);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "SparkleParamsDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Sparkle";
            base.FormClosing += new FormClosingEventHandler(this.SparkleParamsDialog_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.numericUpDownMax.EndInit();
            this.numericUpDownMin.EndInit();
            this.trackBarDecay.EndInit();
            ((ISupportInitialize) this.pictureBoxExample).EndInit();
            this.trackBarFrequency.EndInit();
            base.ResumeLayout(false);
        }

        private void m_drawTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            base.BeginInvoke(this.m_refreshInvoker);
            if (++this.m_tickCount == this.m_maxColumn)
            {
                this.Regenerate();
            }
        }

        private void numericUpDownMax_ValueChanged(object sender, EventArgs e)
        {
            if (this.m_actualLevels)
            {
                this.m_max = (int) this.numericUpDownMax.Value;
            }
            else
            {
                this.m_max = (((int) this.numericUpDownMax.Value) * 0xff) / 100;
            }
            this.Regenerate();
        }

        private void numericUpDownMin_ValueChanged(object sender, EventArgs e)
        {
            if (this.m_actualLevels)
            {
                this.m_min = (int) this.numericUpDownMin.Value;
            }
            else
            {
                this.m_min = (((int) this.numericUpDownMin.Value) * 0xff) / 100;
            }
            this.Regenerate();
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
            this.m_drawTimer.Stop();
            this.m_effectGenerator(this.m_effectValues, new int[] { this.m_frequency, this.m_decay, this.m_min, this.m_max });
            this.m_tickCount = 0;
            this.m_drawTimer.Start();
        }

        private void SparkleParamsDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.m_drawTimer.Stop();
        }

        private void trackBarDecay_Scroll(object sender, EventArgs e)
        {
            this.m_decay = this.trackBarDecay.Value;
            this.Regenerate();
        }

        private void trackBarFrequency_Scroll(object sender, EventArgs e)
        {
            this.m_frequency = this.trackBarFrequency.Value;
            this.Regenerate();
        }

        public int DecayTime
        {
            get
            {
                return this.trackBarDecay.Value;
            }
        }

        public int Frequency
        {
            get
            {
                return this.trackBarFrequency.Value;
            }
        }

        public byte MaximumIntensity
        {
            get
            {
                return (byte) this.m_max;
            }
        }

        public byte MinimumIntensity
        {
            get
            {
                return (byte) this.m_min;
            }
        }
    }
}

