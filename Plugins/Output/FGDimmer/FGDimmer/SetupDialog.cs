namespace FGDimmer {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.IO.Ports;
	using System.Windows.Forms;
	using Vixen.Dialogs;

	internal partial class SetupDialog : Form {
		private Module[] m_modules;
		private SerialPort m_selectedPort;

		public SetupDialog(SerialPort selectedPort, Module[] modules, int startChannel, int endChannel, bool holdPort, bool acOperation) {
			this.InitializeComponent();
			this.m_selectedPort = selectedPort;
			this.m_modules = modules;
			while (startChannel <= endChannel) {
				this.comboBoxModule1.Items.Add(startChannel);
				this.comboBoxModule2.Items.Add(startChannel);
				this.comboBoxModule3.Items.Add(startChannel);
				this.comboBoxModule4.Items.Add(startChannel);
				startChannel++;
			}
			this.checkBoxModule1.Checked = this.m_modules[0].Enabled;
			if (this.checkBoxModule1.Checked && (this.m_modules[0].StartChannel >= ((int)this.comboBoxModule1.Items[0]))) {
				this.comboBoxModule1.SelectedItem = this.m_modules[0].StartChannel;
			}
			this.checkBoxModule2.Checked = this.m_modules[1].Enabled;
			if (this.checkBoxModule2.Checked && (this.m_modules[1].StartChannel >= ((int)this.comboBoxModule2.Items[0]))) {
				this.comboBoxModule2.SelectedItem = this.m_modules[1].StartChannel;
			}
			this.checkBoxModule3.Checked = this.m_modules[2].Enabled;
			if (this.checkBoxModule3.Checked && (this.m_modules[2].StartChannel >= ((int)this.comboBoxModule3.Items[0]))) {
				this.comboBoxModule3.SelectedItem = this.m_modules[2].StartChannel;
			}
			this.checkBoxModule4.Checked = this.m_modules[3].Enabled;
			if (this.checkBoxModule4.Checked && (this.m_modules[3].StartChannel >= ((int)this.comboBoxModule4.Items[0]))) {
				this.comboBoxModule4.SelectedItem = this.m_modules[3].StartChannel;
			}
			this.checkBoxHoldPort.Checked = holdPort;
			if (acOperation) {
				this.radioButtonAC.Checked = true;
			}
			else {
				this.radioButtonPWM.Checked = true;
			}
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			this.m_modules[0].Enabled = this.checkBoxModule1.Checked;
			if (this.m_modules[0].Enabled) {
				this.m_modules[0].StartChannel = (int)this.comboBoxModule1.SelectedItem;
			}

			this.m_modules[1].Enabled = this.checkBoxModule2.Checked;
			if (this.m_modules[1].Enabled) {
				this.m_modules[1].StartChannel = (int)this.comboBoxModule2.SelectedItem;
			}

			this.m_modules[2].Enabled = this.checkBoxModule3.Checked;
			if (this.m_modules[2].Enabled) {
				this.m_modules[2].StartChannel = (int)this.comboBoxModule3.SelectedItem;
			}

			this.m_modules[3].Enabled = this.checkBoxModule4.Checked;
			if (this.m_modules[3].Enabled) {
				this.m_modules[3].StartChannel = (int)this.comboBoxModule4.SelectedItem;
			}
		}

		private void buttonSerialSetup_Click(object sender, EventArgs e) {
			SerialSetupDialog dialog = new SerialSetupDialog(this.m_selectedPort);
			if (dialog.ShowDialog() == DialogResult.OK) {
				this.m_selectedPort = dialog.SelectedPort;
			}
		}

		public bool ACOperation {
			get {
				return this.radioButtonAC.Checked;
			}
		}

		public bool HoldPort {
			get {
				return this.checkBoxHoldPort.Checked;
			}
		}

		public int Module1StartChannel {
			get {
				return (int)this.comboBoxModule1.SelectedItem;
			}
		}

		public int Module2StartChannel {
			get {
				return (int)this.comboBoxModule2.SelectedItem;
			}
		}

		public int Module3StartChannel {
			get {
				return (int)this.comboBoxModule3.SelectedItem;
			}
		}

		public int Module4StartChannel {
			get {
				return (int)this.comboBoxModule4.SelectedItem;
			}
		}

		public SerialPort SelectedPort {
			get {
				return this.m_selectedPort;
			}
		}

		public bool UsingModule1 {
			get {
				return this.checkBoxModule1.Checked;
			}
		}

		public bool UsingModule2 {
			get {
				return this.checkBoxModule2.Checked;
			}
		}

		public bool UsingModule3 {
			get {
				return this.checkBoxModule3.Checked;
			}
		}

		public bool UsingModule4 {
			get {
				return this.checkBoxModule4.Checked;
			}
		}
	}
}