using System;
using System.Globalization;
using System.Windows.Forms;

namespace VixenPlus.Dialogs
{
	public partial class ParallelSetupDialog : Form
	{
		private readonly int _otherAddressIndex;

		public ParallelSetupDialog(int portAddress)
		{
			InitializeComponent();
			_otherAddressIndex = 3;
			switch (portAddress)
			{
				case 0x278:
					comboBoxPort.SelectedIndex = 1;
					break;

				case 0x378:
					comboBoxPort.SelectedIndex = 0;
					break;

				case 0x3bc:
					comboBoxPort.SelectedIndex = 2;
					break;

				case 0:
					comboBoxPort.SelectedIndex = 0;
					break;

				default:
					textBoxPort.Text = portAddress.ToString("X4");
					comboBoxPort.SelectedIndex = 3;
					break;
			}
		}

		public ushort PortAddress
		{
			get
			{
				switch (comboBoxPort.SelectedIndex)
				{
					case 0:
						return 0x378;

					case 1:
						return 0x278;

					case 2:
						return 0x3bc;
				}
				return Convert.ToUInt16(textBoxPort.Text, 16);
			}
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			ushort ignore;

			if (comboBoxPort.SelectedIndex != _otherAddressIndex ||
			    UInt16.TryParse(textBoxPort.Text, NumberStyles.HexNumber, null, out ignore))
			{
				return;
			}

			MessageBox.Show("The port number is not a valid hexadecimal number.", Vendor.ProductName, MessageBoxButtons.OK,
			                MessageBoxIcon.Hand);
			DialogResult = DialogResult.None;
		}

		private void comboBoxPort_SelectedIndexChanged(object sender, EventArgs e)
		{
			textBoxPort.Enabled = comboBoxPort.SelectedIndex == _otherAddressIndex;
		}
	}
}