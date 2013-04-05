namespace VixenEditor {
	using System;
    using System.Windows.Forms;
	using VixenPlus;

	internal partial class DrawingIntensityDialog : Form {

		private readonly bool _actualLevels;

		public DrawingIntensityDialog(EventSequence sequence, byte currentLevel, bool actualLevels) {
			InitializeComponent();
			_actualLevels = actualLevels;
			if (actualLevels) {
				numericUpDownLevel.Minimum = sequence.MinimumLevel;
				numericUpDownLevel.Maximum = sequence.MaximumLevel;
				numericUpDownLevel.Value = currentLevel;
			}
			else {
				numericUpDownLevel.Minimum = (int)Math.Round(sequence.MinimumLevel * 100f / 255f, MidpointRounding.AwayFromZero);
				numericUpDownLevel.Maximum = (int)Math.Round(sequence.MaximumLevel * 100f / 255f, MidpointRounding.AwayFromZero);
				numericUpDownLevel.Value = (int)Math.Round(currentLevel * 100f / 255f, MidpointRounding.AwayFromZero);
			}
		}

		private void buttonReset_Click(object sender, EventArgs e) {
			numericUpDownLevel.Value = numericUpDownLevel.Maximum;
		}

		//ComponentResourceManager manager = new ComponentResourceManager(typeof(DrawingIntensityDialog));
		//label1.Text = manager.GetString("label1.Text");

		public byte SelectedIntensity {
			get {
				if (_actualLevels) {
					return (byte)numericUpDownLevel.Value;
				}
				return (byte)((numericUpDownLevel.Value / 100M) * 255M);
			}
		}
	}
}