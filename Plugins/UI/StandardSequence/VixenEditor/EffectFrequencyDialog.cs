namespace VixenEditor {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Timers;
	using System.Windows.Forms;

	internal partial class EffectFrequencyDialog : Form {
		private SolidBrush m_brush = null;
		private System.Timers.Timer m_drawTimer;
		private FrequencyEffectGenerator m_effectGenerator;
		private byte[,] m_effectValues;
		private int m_frequency = 0;
		private int m_maxColumn;
		private int m_tickCount;
		private Point[][] m_treePoints;

		public EffectFrequencyDialog(string effectName, int maxFrequency, FrequencyEffectGenerator effectGenerator) {
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
			Point[] pointArray2 = new Point[] { new Point(22, 0x2e), new Point(0x25, 0x5b), new Point(7, 0x5b) };
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

		private void EffectFrequencyDialog_FormClosing(object sender, FormClosingEventArgs e) {
			this.m_drawTimer.Stop();
		}

		private void m_drawTimer_Elapsed(object sender, ElapsedEventArgs e) {
			if (this.m_frequency != 0) {
				base.BeginInvoke(this.m_refreshInvoker);
				if (++this.m_tickCount == this.m_maxColumn) {
					this.Regenerate();
				}
			}
		}

		private void pictureBoxExample_Paint(object sender, PaintEventArgs e) {
			if (this.m_drawTimer.Enabled) {
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

		private void Regenerate() {
			if (this.m_frequency != 0) {
				this.m_drawTimer.Stop();
				this.m_effectGenerator(this.m_effectValues, new int[] { this.m_frequency });
				this.m_tickCount = 0;
				this.m_drawTimer.Start();
			}
		}

		private void trackBarFrequency_Scroll(object sender, EventArgs e) {
			this.m_frequency = this.trackBarFrequency.Value;
			this.Regenerate();
		}

		public int Frequency {
			get {
				return this.trackBarFrequency.Value;
			}
		}
	}
}