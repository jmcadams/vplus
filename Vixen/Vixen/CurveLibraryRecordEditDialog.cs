namespace Vixen
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal partial class CurveLibraryRecordEditDialog : Form
    {
        private CurveLibraryRecord m_clr = null;
        private CurveLibrary m_library;

        public CurveLibraryRecordEditDialog(CurveLibraryRecord clr)
        {
            this.InitializeComponent();
            this.m_clr = clr;
            this.m_library = new CurveLibrary();
            this.textBoxManufacturer.AutoCompleteCustomSource.AddRange(this.m_library.GetAllManufacturers());
            this.textBoxController.AutoCompleteCustomSource.AddRange(this.m_library.GetAllControllers());
            if (this.m_clr != null)
            {
                this.textBoxManufacturer.Text = this.m_clr.Manufacturer;
                this.textBoxLightCount.Text = this.m_clr.LightCount;
                this.buttonColor.BackColor = Color.FromArgb(this.m_clr.Color);
                this.textBoxController.Text = this.m_clr.Controller;
            }
        }

        private void buttonColor_Click(object sender, EventArgs e)
        {
            if (this.colorDialog.ShowDialog() == DialogResult.OK)
            {
                this.buttonColor.BackColor = this.colorDialog.Color;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (((this.textBoxManufacturer.Text.Trim().Length == 0) || (this.textBoxLightCount.Text.Trim().Length == 0)) || (this.textBoxController.Text.Trim().Length == 0))
            {
                MessageBox.Show("All fields are required.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (this.m_clr == null)
            {
                this.m_clr = new CurveLibraryRecord(this.textBoxManufacturer.Text, this.textBoxLightCount.Text, this.buttonColor.BackColor.ToArgb(), this.textBoxController.Text);
            }
            else
            {
                this.m_clr.Manufacturer = this.textBoxManufacturer.Text;
                this.m_clr.LightCount = this.textBoxLightCount.Text;
                this.m_clr.Color = this.buttonColor.BackColor.ToArgb();
                this.m_clr.Controller = this.textBoxController.Text;
            }
        }

        private void CurveLibraryRecordEditDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.m_library.Dispose();
        }

        

        

        public CurveLibraryRecord LibraryRecord
        {
            get
            {
                return this.m_clr;
            }
        }
    }
}

