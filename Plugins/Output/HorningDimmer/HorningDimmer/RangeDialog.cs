namespace HorningDimmer {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;

	public partial class RangeDialog : Form {
		private int m_end;
		private int m_from = 0;
		private bool m_okClick = false;
		private bool m_selected = false;
		private int m_start;
		private int m_to = 0;

		public RangeDialog(int startChannel, int endChannel) {
			this.InitializeComponent();
			this.m_start = startChannel;
			this.m_end = endChannel;
		}

		private void buttonCancel_Click(object sender, EventArgs e) {
			this.m_okClick = false;
		}

		private void buttonOK_Click(object sender, EventArgs e) {
			this.m_okClick = true;
		}

		private void RangeDialog_FormClosing(object sender, FormClosingEventArgs e) {
			if (this.m_okClick) {
				int num = Convert.ToInt32(this.textBoxFrom.Text);
				int num2 = Convert.ToInt32(this.textBoxTo.Text);
				if ((num < this.m_start) || (num > this.m_end)) {
					MessageBox.Show("Range starting value is invalid", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					e.Cancel = true;
				}
				else if ((num2 < this.m_start) || (num2 > this.m_end)) {
					MessageBox.Show("Range ending value is invalid", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					e.Cancel = true;
				}
				else if (num > num2) {
					MessageBox.Show("Range starting value cannot be greater than the ending value", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					e.Cancel = true;
				}
				else {
					this.m_from = num;
					this.m_to = num2;
					this.m_selected = this.radioButtonSelected.Checked;
				}
			}
		}

		public int From {
			get {
				return this.m_from;
			}
		}

		public bool Selected {
			get {
				return this.m_selected;
			}
		}

		public int To {
			get {
				return this.m_to;
			}
		}
	}
}