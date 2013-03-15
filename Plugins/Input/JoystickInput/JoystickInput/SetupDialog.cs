namespace JoystickInput
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Vixen;

    public class SetupDialog : Form
    {
        private Button buttonCancel;
        private Button buttonOK;
        private CheckBox checkBoxIsIterator;
        private IContainer components = null;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
        private LinkLabel linkLabelButtons;
        private ListBox listBoxButtons;
        private Input[] m_inputs;
        private RadioButton radioButtonAnalog;
        private RadioButton radioButtonDigital;

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

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new GroupBox();
            this.radioButtonAnalog = new RadioButton();
            this.radioButtonDigital = new RadioButton();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.groupBox2 = new GroupBox();
            this.checkBoxIsIterator = new CheckBox();
            this.listBoxButtons = new ListBox();
            this.label1 = new Label();
            this.linkLabelButtons = new LinkLabel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.radioButtonAnalog);
            this.groupBox1.Controls.Add(this.radioButtonDigital);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x113, 0x5e);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "POV Controls";
            this.radioButtonAnalog.AutoSize = true;
            this.radioButtonAnalog.Location = new Point(0x20, 0x36);
            this.radioButtonAnalog.Name = "radioButtonAnalog";
            this.radioButtonAnalog.Size = new Size(0xc7, 0x11);
            this.radioButtonAnalog.TabIndex = 1;
            this.radioButtonAnalog.TabStop = true;
            this.radioButtonAnalog.Text = "Treat POV controls as 1 analog input";
            this.radioButtonAnalog.UseVisualStyleBackColor = true;
            this.radioButtonDigital.AutoSize = true;
            this.radioButtonDigital.Location = new Point(0x20, 0x1f);
            this.radioButtonDigital.Name = "radioButtonDigital";
            this.radioButtonDigital.Size = new Size(0xc7, 0x11);
            this.radioButtonDigital.TabIndex = 0;
            this.radioButtonDigital.TabStop = true;
            this.radioButtonDigital.Text = "Treat POV controls as 4 digital inputs";
            this.radioButtonDigital.UseVisualStyleBackColor = true;
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(0x83, 0x132);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0xd4, 0x132);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.groupBox2.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox2.Controls.Add(this.linkLabelButtons);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.checkBoxIsIterator);
            this.groupBox2.Controls.Add(this.listBoxButtons);
            this.groupBox2.Location = new Point(12, 0x70);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x113, 0xbc);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mapping Set Iterators";
            this.checkBoxIsIterator.CheckAlign = ContentAlignment.TopLeft;
            this.checkBoxIsIterator.Location = new Point(0xa8, 0x2d);
            this.checkBoxIsIterator.Name = "checkBoxIsIterator";
            this.checkBoxIsIterator.Size = new Size(0x65, 0x3a);
            this.checkBoxIsIterator.TabIndex = 1;
            this.checkBoxIsIterator.Text = "Button will be used to iterate input-output mapping sets";
            this.checkBoxIsIterator.UseVisualStyleBackColor = true;
            this.checkBoxIsIterator.Click += new EventHandler(this.checkBoxIsIterator_Click);
            this.listBoxButtons.FormattingEnabled = true;
            this.listBoxButtons.Location = new Point(6, 0x2d);
            this.listBoxButtons.Name = "listBoxButtons";
            this.listBoxButtons.Size = new Size(0x97, 0x86);
            this.listBoxButtons.TabIndex = 0;
            this.listBoxButtons.SelectedIndexChanged += new EventHandler(this.listBoxInputs_SelectedIndexChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(8, 0x18);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x56, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Joystick buttons:";
            this.linkLabelButtons.ActiveLinkColor = Color.Blue;
            this.linkLabelButtons.Location = new Point(0xac, 0x7c);
            this.linkLabelButtons.Name = "linkLabelButtons";
            this.linkLabelButtons.Size = new Size(0x57, 0x37);
            this.linkLabelButtons.TabIndex = 3;
            this.linkLabelButtons.TabStop = true;
            this.linkLabelButtons.Text = "Let me see which buttons are which";
            this.linkLabelButtons.VisitedLinkColor = Color.Blue;
            this.linkLabelButtons.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabelButtons_LinkClicked);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x12b, 0x155);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "SetupDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Setup";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            base.ResumeLayout(false);
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

