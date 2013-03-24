namespace VixenEditor {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using VixenPlus;

	internal partial class DrawingIntensityDialog : Form {

		private bool m_actualLevels;

		public DrawingIntensityDialog(EventSequence sequence, byte currentLevel, bool actualLevels) {
			this.InitializeComponent();
			this.m_actualLevels = actualLevels;
			if (actualLevels) {
				this.numericUpDownLevel.Minimum = sequence.MinimumLevel;
				this.numericUpDownLevel.Maximum = sequence.MaximumLevel;
				this.numericUpDownLevel.Value = currentLevel;
			}
			else {
				this.numericUpDownLevel.Minimum = (int)Math.Round((double)((sequence.MinimumLevel * 100f) / 255f), MidpointRounding.AwayFromZero);
				this.numericUpDownLevel.Maximum = (int)Math.Round((double)((sequence.MaximumLevel * 100f) / 255f), MidpointRounding.AwayFromZero);
				this.numericUpDownLevel.Value = (int)Math.Round((double)((currentLevel * 100f) / 255f), MidpointRounding.AwayFromZero);
			}
		}

		private void buttonReset_Click(object sender, EventArgs e) {
			this.numericUpDownLevel.Value = this.numericUpDownLevel.Maximum;
		}

		//ComponentResourceManager manager = new ComponentResourceManager(typeof(DrawingIntensityDialog));
		//this.label1.Text = manager.GetString("label1.Text");

		public byte SelectedIntensity {
			get {
				if (this.m_actualLevels) {
					return (byte)this.numericUpDownLevel.Value;
				}
				return (byte)((this.numericUpDownLevel.Value / 100M) * 255M);
			}
		}
	}
}