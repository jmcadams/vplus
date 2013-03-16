namespace RGBLEDAddIn {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using Vixen;

	public partial class MainDialog : Form {
		public MainDialog(EventSequence sequence) {
			int num2;
			string str;
			this.components = null;
			this.InitializeComponent();
			this.m_sequence = sequence;
			int num = sequence.ChannelCount / 3;
			this.labelCapacity.Text = num.ToString();
			for (num2 = 1; num2 <= num; num2++) {
				this.comboBoxRGBChannel.Items.Add(num2.ToString());
			}
			int num3 = sequence.Time / 60;
			int num4 = (int)Math.Round((double)(1000f / ((float)sequence.EventPeriod)), MidpointRounding.AwayFromZero);
			for (num2 = 0; num2 < num3; num2++) {
				str = num2.ToString();
				this.comboBoxStartMinute.Items.Add(str);
				this.comboBoxDurationMinute.Items.Add(str);
			}
			for (num2 = 0; num2 < 60; num2++) {
				str = num2.ToString();
				this.comboBoxStartSecond.Items.Add(str);
				this.comboBoxDurationSecond.Items.Add(str);
			}
			for (num2 = 0; num2 < num4; num2++) {
				str = num2.ToString();
				this.comboBoxStartEvent.Items.Add(str);
				this.comboBoxDurationEvent.Items.Add(str);
			}
			if (this.comboBoxStartMinute.Items.Count == 0) {
				this.comboBoxStartMinute.Items.Add("0");
			}
			if (this.comboBoxDurationMinute.Items.Count == 0) {
				this.comboBoxDurationMinute.Items.Add("0");
			}
			this.comboBoxStartMinute.SelectedIndex = this.comboBoxDurationMinute.SelectedIndex = this.comboBoxStartSecond.SelectedIndex = this.comboBoxDurationSecond.SelectedIndex = this.comboBoxStartEvent.SelectedIndex = this.comboBoxDurationEvent.SelectedIndex = 0;
			if (this.comboBoxRGBChannel.Items.Count > 0) {
				this.comboBoxRGBChannel.SelectedIndex = 0;
			}
		}

		private void buttonOK_Click(object sender, EventArgs e) {
			if (this.comboBoxRGBChannel.SelectedIndex == -1) {
				base.DialogResult = System.Windows.Forms.DialogResult.None;
				MessageBox.Show("Please select an RGB channel, if one is available.\nOtherwise cancel the operation.", "RGBLED", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				base.ActiveControl = this.comboBoxRGBChannel;
			}
		}

		private void buttonPickColor_Click(object sender, EventArgs e) {
			if (this.colorDialog.ShowDialog() == DialogResult.OK) {
				this.panelColor.BackColor = this.colorDialog.Color;
				this.numericUpDownRed.Value = this.colorDialog.Color.R;
				this.numericUpDownGreen.Value = this.colorDialog.Color.G;
				this.numericUpDownBlue.Value = this.colorDialog.Color.B;
			}
		}

		private void buttonPullEnd_Click(object sender, EventArgs e) {
			this.panelEndColor.BackColor = this.panelColor.BackColor;
		}

		private void buttonPullStart_Click(object sender, EventArgs e) {
			this.panelStartColor.BackColor = this.panelColor.BackColor;
		}

		private void numericUpDownBlue_ValueChanged(object sender, EventArgs e) {
			this.labelBIntensity.Text = ((int)Math.Round((double)((((float)this.numericUpDownBlue.Value) / 255f) * 100f), MidpointRounding.AwayFromZero)).ToString() + "%";
			this.panelColor.BackColor = Color.FromArgb((int)this.numericUpDownRed.Value, (int)this.numericUpDownGreen.Value, (int)this.numericUpDownBlue.Value);
		}

		private void numericUpDownGreen_ValueChanged(object sender, EventArgs e) {
			this.labelGIntensity.Text = ((int)Math.Round((double)((((float)this.numericUpDownGreen.Value) / 255f) * 100f), MidpointRounding.AwayFromZero)).ToString() + "%";
			this.panelColor.BackColor = Color.FromArgb((int)this.numericUpDownRed.Value, (int)this.numericUpDownGreen.Value, (int)this.numericUpDownBlue.Value);
		}

		private void numericUpDownRed_Enter(object sender, EventArgs e) {
			NumericUpDown down = (NumericUpDown)sender;
			int num = (int)down.Value;
			down.Select(0, num.ToString().Length);
		}

		private void numericUpDownRed_ValueChanged(object sender, EventArgs e) {
			this.labelRIntensity.Text = ((int)Math.Round((double)((((float)this.numericUpDownRed.Value) / 255f) * 100f), MidpointRounding.AwayFromZero)).ToString() + "%";
			this.panelColor.BackColor = Color.FromArgb((int)this.numericUpDownRed.Value, (int)this.numericUpDownGreen.Value, (int)this.numericUpDownBlue.Value);
		}

		public int DurationEventCount {
			get {
				float num = 1000f / ((float)this.m_sequence.EventPeriod);
				return (int)Math.Round((double)((this.comboBoxDurationEvent.SelectedIndex + (this.comboBoxDurationSecond.SelectedIndex * num)) + ((this.comboBoxDurationMinute.SelectedIndex * 60) * num)), MidpointRounding.AwayFromZero);
			}
		}

		public Color EndColor {
			get {
				return this.panelEndColor.BackColor;
			}
		}

		public int StartChannel {
			get {
				return (this.comboBoxRGBChannel.SelectedIndex * 3);
			}
		}

		public Color StartColor {
			get {
				return this.panelStartColor.BackColor;
			}
		}

		public int StartEventIndex {
			get {
				float num = 1000f / ((float)this.m_sequence.EventPeriod);
				return (int)Math.Round((double)((this.comboBoxStartEvent.SelectedIndex + (this.comboBoxStartSecond.SelectedIndex * num)) + ((this.comboBoxStartMinute.SelectedIndex * 60) * num)), MidpointRounding.AwayFromZero);
			}
		}
	}
}