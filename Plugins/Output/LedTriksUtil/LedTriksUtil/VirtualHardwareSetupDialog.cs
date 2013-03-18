namespace LedTriksUtil {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;

	public partial class VirtualHardwareSetupDialog : Form {
		private Size m_layout;

		public VirtualHardwareSetupDialog(bool allowLayoutChange) {
			this.InitializeComponent();
			this.BoardLayout = new Size(1, 1);
			this.gbLedTriksSetup.Enabled = allowLayoutChange;
			this.LEDSize = 3;
			this.LEDColor = Color.Red;
			this.DotPitch = 9;
		}

		private void buttonBoardLayout_Click(object sender, EventArgs e) {
			BoardLayoutDialog dialog = new BoardLayoutDialog(this.m_layout);
			if (dialog.ShowDialog() == DialogResult.OK) {
				this.BoardLayout = dialog.BoardLayout;
			}
		}

		private void buttonLEDColor_Click(object sender, EventArgs e) {
			this.colorDialog.Color = this.buttonLEDColor.BackColor;
			if (this.colorDialog.ShowDialog() == DialogResult.OK) {
				this.LEDColor = this.colorDialog.Color;
			}
		}

		public Size BoardLayout {
			get {
				return this.m_layout;
			}
			set {
				this.m_layout = value;
				this.labelBoardLayout.Text = this.m_layout.ToString();
			}
		}

		public int DotPitch {
			get {
				return (int)this.numericUpDownDotPitch.Value;
			}
			set {
				this.numericUpDownDotPitch.Value = value;
			}
		}

		public Color LEDColor {
			get {
				return this.buttonLEDColor.BackColor;
			}
			set {
				this.buttonLEDColor.BackColor = value;
			}
		}

		public int LEDSize {
			get {
				return (int)this.numericUpDownLEDSize.Value;
			}
			set {
				this.numericUpDownLEDSize.Value = value;
			}
		}
	}
}