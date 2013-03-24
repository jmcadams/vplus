using System;
using System.Drawing;
using System.Windows.Forms;

namespace VixenPlus
{
	internal partial class CurveLibraryRecordEditDialog : Form
	{
		private readonly CurveLibrary m_library;
		private CurveLibraryRecord m_clr;

		public CurveLibraryRecordEditDialog(CurveLibraryRecord clr)
		{
			InitializeComponent();
			m_clr = clr;
			m_library = new CurveLibrary();
			textBoxManufacturer.AutoCompleteCustomSource.AddRange(m_library.GetAllManufacturers());
			textBoxController.AutoCompleteCustomSource.AddRange(m_library.GetAllControllers());
			if (m_clr != null)
			{
				textBoxManufacturer.Text = m_clr.Manufacturer;
				textBoxLightCount.Text = m_clr.LightCount;
				buttonColor.BackColor = Color.FromArgb(m_clr.Color);
				textBoxController.Text = m_clr.Controller;
			}
		}

		public CurveLibraryRecord LibraryRecord
		{
			get { return m_clr; }
		}

		private void buttonColor_Click(object sender, EventArgs e)
		{
			if (colorDialog.ShowDialog() == DialogResult.OK)
			{
				buttonColor.BackColor = colorDialog.Color;
			}
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (((textBoxManufacturer.Text.Trim().Length == 0) || (textBoxLightCount.Text.Trim().Length == 0)) ||
			    (textBoxController.Text.Trim().Length == 0))
			{
				MessageBox.Show("All fields are required.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else if (m_clr == null)
			{
				m_clr = new CurveLibraryRecord(textBoxManufacturer.Text, textBoxLightCount.Text, buttonColor.BackColor.ToArgb(),
				                               textBoxController.Text);
			}
			else
			{
				m_clr.Manufacturer = textBoxManufacturer.Text;
				m_clr.LightCount = textBoxLightCount.Text;
				m_clr.Color = buttonColor.BackColor.ToArgb();
				m_clr.Controller = textBoxController.Text;
			}
		}

		private void CurveLibraryRecordEditDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			m_library.Dispose();
		}
	}
}