using System;
using System.Windows.Forms;

namespace Vixen.Dialogs
{
	public partial class ParallelSetupDialog : Form
	{
		private readonly int m_otherAddressIndex;

		public ParallelSetupDialog(int portAddress)
		{
			InitializeComponent();
			m_otherAddressIndex = 3;
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
				return Convert.ToUInt16(textBoxPort.Text, 0x10);
			}
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (comboBoxPort.SelectedIndex == m_otherAddressIndex)
			{
				try
				{
					Convert.ToUInt16(textBoxPort.Text, 0x10);
				}
				catch
				{
					MessageBox.Show("The port number is not a valid hexadecimal number.", Vendor.ProductName, MessageBoxButtons.OK,
					                MessageBoxIcon.Hand);
					base.DialogResult = DialogResult.None;
				}
			}
		}

		private void comboBoxPort_SelectedIndexChanged(object sender, EventArgs e)
		{
			textBoxPort.Enabled = comboBoxPort.SelectedIndex == m_otherAddressIndex;
		}
	}
}