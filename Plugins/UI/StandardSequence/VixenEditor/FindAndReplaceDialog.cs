using System.Globalization;

namespace VixenEditor {
	using System;
    using System.Windows.Forms;

	public partial class FindAndReplaceDialog : Form {
		private readonly bool _actualLevels;

		public FindAndReplaceDialog(byte minimum, byte maximum, bool actualLevels) {
			int num;
			int num2;
			components = null;
			InitializeComponent();
			_actualLevels = actualLevels;
			if (actualLevels) {
				num = minimum;
				num2 = maximum;
			}
			else {
				num = (int)Math.Round(minimum / 255f * 100f, MidpointRounding.AwayFromZero);
				num2 = (int)Math.Round(maximum / 255f * 100f, MidpointRounding.AwayFromZero);
			}
			numericUpDownFind.Minimum = num;
			numericUpDownReplaceWith.Minimum = num;
			numericUpDownFind.Maximum = num2;
			numericUpDownReplaceWith.Maximum = num2;
		}

		private void numericUpDownFind_Enter(object sender, EventArgs e) {
			numericUpDownFind.Select(0, numericUpDownFind.Value.ToString(CultureInfo.InvariantCulture).Length);
		}

		private void numericUpDownReplaceWith_Enter(object sender, EventArgs e) {
			numericUpDownReplaceWith.Select(0, numericUpDownReplaceWith.Value.ToString(CultureInfo.InvariantCulture).Length);
		}

		public byte FindValue {
			get {
				if (_actualLevels) {
					return (byte)numericUpDownFind.Value;
				}
				return (byte)numericUpDownFind.Value;
			}
		}

		public byte ReplaceWithValue {
			get {
				if (_actualLevels) {
					return (byte)numericUpDownReplaceWith.Value;
				}
				return (byte)numericUpDownReplaceWith.Value;
			}
		}
	}
}