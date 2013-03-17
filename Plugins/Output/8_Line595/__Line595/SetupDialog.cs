namespace __Line595 {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using Vixen.Dialogs;

	public partial class SetupDialog : Form {

		private int m_portAddress;

		public SetupDialog(int channelCount, int portAddress, byte linesUsed) {
			this.InitializeComponent();
			this.textBoxChannels.Text = channelCount.ToString();
			this.checkBox1.Checked = (linesUsed & 1) > 0;
			this.checkBox2.Checked = (linesUsed & 2) > 0;
			this.checkBox3.Checked = (linesUsed & 4) > 0;
			this.checkBox4.Checked = (linesUsed & 8) > 0;
			this.checkBox5.Checked = (linesUsed & 0x10) > 0;
			this.checkBox6.Checked = (linesUsed & 0x20) > 0;
			this.checkBox7.Checked = (linesUsed & 0x40) > 0;
			this.checkBox8.Checked = (linesUsed & 0x80) > 0;
			this.m_portAddress = portAddress;
		}

		private void buttonPortSetup_Click(object sender, EventArgs e) {
			ParallelSetupDialog dialog = new ParallelSetupDialog(this.m_portAddress);
			if (dialog.ShowDialog() == DialogResult.OK) {
				this.m_portAddress = dialog.PortAddress;
			}
		}

		public int ChannelCount {
			get {
				try {
					return Convert.ToInt32(this.textBoxChannels.Text);
				}
				catch {
					return 0;
				}
			}
		}

		public byte LinesUsed {
			get {
				byte num = 0;
				for (int i = 0; i < 8; i++) {
					int num4 = i + 1;
					num = (byte)(num | ((byte)(((((CheckBox)base.Controls.Find("checkBox" + num4.ToString(), 1)[0]).Checked != null) ? 1 : 0) << i)));
				}
				return num;
			}
		}

		public int PortAddress {
			get {
				return this.m_portAddress;
			}
		}
	}
}