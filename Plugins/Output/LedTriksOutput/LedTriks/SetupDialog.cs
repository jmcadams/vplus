namespace LedTriks {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;
	using VixenPlus.Dialogs;

	public partial class SetupDialog : Form {

		private ushort m_portAddress;

		public SetupDialog(ushort portAddress, bool useWithScript) {
			this.InitializeComponent();
			this.m_portAddress = portAddress;
			this.checkBoxUseWithScript.Checked = useWithScript;
		}

		private void buttonParallelSetup_Click(object sender, EventArgs e) {
			ParallelSetupDialog dialog = new ParallelSetupDialog(this.m_portAddress);
			if (dialog.ShowDialog() == DialogResult.OK) {
				this.m_portAddress = dialog.PortAddress;
			}
		}

		public ushort PortAddress {
			get {
				return this.m_portAddress;
			}
		}

		public bool UseWithScript {
			get {
				return this.checkBoxUseWithScript.Checked;
			}
		}
	}
}