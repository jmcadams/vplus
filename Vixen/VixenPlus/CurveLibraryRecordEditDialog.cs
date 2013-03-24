using System;
using System.Drawing;
using System.Windows.Forms;

namespace VixenPlus
{
	internal partial class CurveLibraryRecordEditDialog : Form
	{
		private readonly CurveLibrary _curveLibrary;
		private CurveLibraryRecord _curveLibraryRecord;

		public CurveLibraryRecordEditDialog(CurveLibraryRecord clr)
		{
			InitializeComponent();
			_curveLibraryRecord = clr;
			_curveLibrary = new CurveLibrary();
			textBoxManufacturer.AutoCompleteCustomSource.AddRange(_curveLibrary.GetAllManufacturers());
			textBoxController.AutoCompleteCustomSource.AddRange(_curveLibrary.GetAllControllers());
			if (_curveLibraryRecord != null)
			{
				textBoxManufacturer.Text = _curveLibraryRecord.Manufacturer;
				textBoxLightCount.Text = _curveLibraryRecord.LightCount;
				buttonColor.BackColor = Color.FromArgb(_curveLibraryRecord.Color);
				textBoxController.Text = _curveLibraryRecord.Controller;
			}
		}

		public CurveLibraryRecord LibraryRecord
		{
			get { return _curveLibraryRecord; }
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
			else if (_curveLibraryRecord == null)
			{
				_curveLibraryRecord = new CurveLibraryRecord(textBoxManufacturer.Text, textBoxLightCount.Text, buttonColor.BackColor.ToArgb(),
				                               textBoxController.Text);
			}
			else
			{
				_curveLibraryRecord.Manufacturer = textBoxManufacturer.Text;
				_curveLibraryRecord.LightCount = textBoxLightCount.Text;
				_curveLibraryRecord.Color = buttonColor.BackColor.ToArgb();
				_curveLibraryRecord.Controller = textBoxController.Text;
			}
		}

		private void CurveLibraryRecordEditDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			_curveLibrary.Dispose();
		}
	}
}