using System;
using System.Drawing;
using System.Windows.Forms;

using Properties;

namespace VixenPlus
{
    internal partial class CurveLibraryRecordEditDialog : Form
    {
        private readonly CurveLibrary _curveLibrary;


        public CurveLibraryRecordEditDialog(CurveLibraryRecord clr)
        {
            InitializeComponent();
            LibraryRecord = clr;
            _curveLibrary = new CurveLibrary();
            textBoxManufacturer.AutoCompleteCustomSource.AddRange(_curveLibrary.GetAllManufacturers());
            textBoxController.AutoCompleteCustomSource.AddRange(_curveLibrary.GetAllControllers());
            if (LibraryRecord == null) {
                return;
            }
            textBoxManufacturer.Text = LibraryRecord.Manufacturer;
            textBoxLightCount.Text = LibraryRecord.LightCount;
            buttonColor.BackColor = Color.FromArgb(LibraryRecord.Color);
            textBoxController.Text = LibraryRecord.Controller;
        }


        public CurveLibraryRecord LibraryRecord { get; private set; }


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
                MessageBox.Show(Resources.AllFieldsRequired, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (LibraryRecord == null)
            {
                LibraryRecord = new CurveLibraryRecord(textBoxManufacturer.Text, textBoxLightCount.Text,
                                                             buttonColor.BackColor.ToArgb(),
                                                             textBoxController.Text);
            }
            else
            {
                LibraryRecord.Manufacturer = textBoxManufacturer.Text;
                LibraryRecord.LightCount = textBoxLightCount.Text;
                LibraryRecord.Color = buttonColor.BackColor.ToArgb();
                LibraryRecord.Controller = textBoxController.Text;
            }
        }

        private void CurveLibraryRecordEditDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            _curveLibrary.Dispose();
        }
    }
}