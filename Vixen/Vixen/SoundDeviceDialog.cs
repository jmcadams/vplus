namespace Vixen
{
    using FMOD;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal class SoundDeviceDialog : Form
    {
        private Button buttonDone;
        private Button buttonSet;
        private ComboBox comboBoxDevice;
        private IContainer components = null;
        private GroupBox groupBox1;
        private bool m_internal = false;
        private int m_lastSelection = -1;
        private Preference2 m_preferences;

        public SoundDeviceDialog(Preference2 preferences)
        {
            this.InitializeComponent();
            this.m_preferences = preferences;
        }

        private void buttonSet_Click(object sender, EventArgs e)
        {
            this.m_preferences.SetInteger("SoundDevice", this.comboBoxDevice.SelectedIndex, 0);
            this.m_preferences.Flush();
            this.buttonSet.Enabled = false;
            MessageBox.Show("Please restart the application for this change to take effect", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void comboBoxDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.m_internal)
            {
                this.buttonSet.Enabled = this.m_lastSelection != this.comboBoxDevice.SelectedIndex;
                this.m_lastSelection = this.comboBoxDevice.SelectedIndex;
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
            this.buttonSet = new Button();
            this.comboBoxDevice = new ComboBox();
            this.buttonDone = new Button();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.buttonSet);
            this.groupBox1.Controls.Add(this.comboBoxDevice);
            this.groupBox1.Location = new Point(15, 0x11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x103, 0x74);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Available devices";
            this.buttonSet.Enabled = false;
            this.buttonSet.Location = new Point(0x5c, 0x45);
            this.buttonSet.Name = "buttonSet";
            this.buttonSet.Size = new Size(0x4b, 0x17);
            this.buttonSet.TabIndex = 1;
            this.buttonSet.Text = "Set";
            this.buttonSet.UseVisualStyleBackColor = true;
            this.buttonSet.Click += new EventHandler(this.buttonSet_Click);
            this.comboBoxDevice.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxDevice.FormattingEnabled = true;
            this.comboBoxDevice.Location = new Point(0x12, 30);
            this.comboBoxDevice.Name = "comboBoxDevice";
            this.comboBoxDevice.Size = new Size(220, 0x15);
            this.comboBoxDevice.TabIndex = 0;
            this.comboBoxDevice.SelectedIndexChanged += new EventHandler(this.comboBoxDevice_SelectedIndexChanged);
            this.buttonDone.DialogResult = DialogResult.OK;
            this.buttonDone.Location = new Point(0xc7, 0x8b);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new Size(0x4b, 0x17);
            this.buttonDone.TabIndex = 1;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x11f, 0xa8);
            base.Controls.Add(this.buttonDone);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "SoundDeviceDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Sound Device";
            base.Load += new EventHandler(this.SoundDeviceDialog_Load);
            this.groupBox1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void SoundDeviceDialog_Load(object sender, EventArgs e)
        {
            this.comboBoxDevice.Items.AddRange(fmod.GetSoundDeviceList());
            int integer = this.m_preferences.GetInteger("SoundDevice");
            if (integer < this.comboBoxDevice.Items.Count)
            {
                this.m_internal = true;
                this.comboBoxDevice.SelectedIndex = integer;
                this.m_internal = false;
            }
        }
    }
}

