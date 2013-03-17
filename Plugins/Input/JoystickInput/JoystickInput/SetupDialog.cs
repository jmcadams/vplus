namespace JoystickInput
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Vixen;

    public partial class SetupDialog : Form
    {
        private Input[] m_inputs;

		public SetupDialog(Input[] inputs)
        {
            this.InitializeComponent();
            this.m_inputs = inputs;
            this.radioButtonDigital.Checked = JoystickInput.SetupData.IsQuadDigitalPOV;
            foreach (JoystickInputResource resource in inputs)
            {
                if (resource.IsButton)
                {
                    this.listBoxButtons.Items.Add(resource);
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            JoystickInput.SetupData.IsQuadDigitalPOV = this.radioButtonDigital.Checked;
            foreach (Input input in this.m_inputs)
            {
                JoystickInput.SetupData.SetIsIterator(input.Name, input.IsMappingIterator);
            }
        }

        private void checkBoxIsIterator_Click(object sender, EventArgs e)
        {
            Input selectedItem = this.listBoxButtons.SelectedItem as Input;
            if (selectedItem != null)
            {
                selectedItem.IsMappingIterator = this.checkBoxIsIterator.Checked;
            }
        }

		private void linkLabelButtons_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ButtonDialog dialog = new ButtonDialog();
            dialog.ShowDialog();
            dialog.Dispose();
        }

        private void listBoxInputs_SelectedIndexChanged(object sender, EventArgs e)
        {
            Input selectedItem = this.listBoxButtons.SelectedItem as Input;
            if (selectedItem != null)
            {
                this.checkBoxIsIterator.Checked = selectedItem.IsMappingIterator;
            }
        }
    }
}

