namespace K8055
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class SetupDialog : Form
    {
        private Button buttonCancel;
        private Button buttonDriverVersion;
        private Button buttonOK;
        private Button buttonSearchDevices;
        private CheckBox checkBoxDev0;
        private CheckBox checkBoxDev1;
        private CheckBox checkBoxDev2;
        private CheckBox checkBoxDev3;
        private IContainer components = null;
        private GroupBox groupBox1;
        private Label label1;
        private Label label3;
        private Label label5;
        private Label label7;
        private Label labelDev0ChannelRange;
        private Label labelDev1ChannelRange;
        private Label labelDev2ChannelRange;
        private Label labelDev3ChannelRange;
        private NumericUpDown numericUpDownDev0;
        private NumericUpDown numericUpDownDev1;
        private NumericUpDown numericUpDownDev2;
        private NumericUpDown numericUpDownDev3;

        public SetupDialog(int channelCount, int[] deviceStartChannels)
        {
            this.InitializeComponent();
            int num = channelCount - 7;
            this.numericUpDownDev0.Maximum = this.numericUpDownDev1.Maximum = this.numericUpDownDev2.Maximum = this.numericUpDownDev3.Maximum = num;
            this.numericUpDownDev0.Value = Math.Min(deviceStartChannels[0] + 1, num);
            this.UpdateRange(this.numericUpDownDev0, this.labelDev0ChannelRange);
            this.numericUpDownDev1.Value = Math.Min(deviceStartChannels[1] + 1, num);
            this.UpdateRange(this.numericUpDownDev1, this.labelDev1ChannelRange);
            this.numericUpDownDev2.Value = Math.Min(deviceStartChannels[2] + 1, num);
            this.UpdateRange(this.numericUpDownDev2, this.labelDev2ChannelRange);
            this.numericUpDownDev3.Value = Math.Min(deviceStartChannels[3] + 1, num);
            this.UpdateRange(this.numericUpDownDev3, this.labelDev3ChannelRange);
            this.SearchDevices();
        }

        private void buttonDriverVersion_Click(object sender, EventArgs e)
        {
            K8055.K8055.Version();
        }

        private void buttonSearchDevices_Click(object sender, EventArgs e)
        {
            this.SearchDevices();
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
            this.buttonDriverVersion = new Button();
            this.buttonSearchDevices = new Button();
            this.checkBoxDev3 = new CheckBox();
            this.checkBoxDev2 = new CheckBox();
            this.checkBoxDev1 = new CheckBox();
            this.checkBoxDev0 = new CheckBox();
            this.labelDev3ChannelRange = new Label();
            this.numericUpDownDev3 = new NumericUpDown();
            this.label7 = new Label();
            this.labelDev2ChannelRange = new Label();
            this.numericUpDownDev2 = new NumericUpDown();
            this.label5 = new Label();
            this.labelDev1ChannelRange = new Label();
            this.numericUpDownDev1 = new NumericUpDown();
            this.label3 = new Label();
            this.labelDev0ChannelRange = new Label();
            this.numericUpDownDev0 = new NumericUpDown();
            this.label1 = new Label();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.groupBox1.SuspendLayout();
            this.numericUpDownDev3.BeginInit();
            this.numericUpDownDev2.BeginInit();
            this.numericUpDownDev1.BeginInit();
            this.numericUpDownDev0.BeginInit();
            base.SuspendLayout();
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.buttonDriverVersion);
            this.groupBox1.Controls.Add(this.buttonSearchDevices);
            this.groupBox1.Controls.Add(this.checkBoxDev3);
            this.groupBox1.Controls.Add(this.checkBoxDev2);
            this.groupBox1.Controls.Add(this.checkBoxDev1);
            this.groupBox1.Controls.Add(this.checkBoxDev0);
            this.groupBox1.Controls.Add(this.labelDev3ChannelRange);
            this.groupBox1.Controls.Add(this.numericUpDownDev3);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.labelDev2ChannelRange);
            this.groupBox1.Controls.Add(this.numericUpDownDev2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.labelDev1ChannelRange);
            this.groupBox1.Controls.Add(this.numericUpDownDev1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.labelDev0ChannelRange);
            this.groupBox1.Controls.Add(this.numericUpDownDev0);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x16c, 180);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Velleman K8805";
            this.buttonDriverVersion.Location = new Point(0xba, 0x91);
            this.buttonDriverVersion.Name = "buttonDriverVersion";
            this.buttonDriverVersion.Size = new Size(0x74, 0x17);
            this.buttonDriverVersion.TabIndex = 0x11;
            this.buttonDriverVersion.Text = "K8805 driver version";
            this.buttonDriverVersion.UseVisualStyleBackColor = true;
            this.buttonDriverVersion.Click += new EventHandler(this.buttonDriverVersion_Click);
            this.buttonSearchDevices.Location = new Point(0x40, 0x91);
            this.buttonSearchDevices.Name = "buttonSearchDevices";
            this.buttonSearchDevices.Size = new Size(0x74, 0x17);
            this.buttonSearchDevices.TabIndex = 0x10;
            this.buttonSearchDevices.Text = "Search devices";
            this.buttonSearchDevices.UseVisualStyleBackColor = true;
            this.buttonSearchDevices.Click += new EventHandler(this.buttonSearchDevices_Click);
            this.checkBoxDev3.AutoSize = true;
            this.checkBoxDev3.Enabled = false;
            this.checkBoxDev3.Location = new Point(0x15, 0x67);
            this.checkBoxDev3.Name = "checkBoxDev3";
            this.checkBoxDev3.Size = new Size(15, 14);
            this.checkBoxDev3.TabIndex = 15;
            this.checkBoxDev3.UseVisualStyleBackColor = true;
            this.checkBoxDev2.AutoSize = true;
            this.checkBoxDev2.Enabled = false;
            this.checkBoxDev2.Location = new Point(0x15, 0x4d);
            this.checkBoxDev2.Name = "checkBoxDev2";
            this.checkBoxDev2.Size = new Size(15, 14);
            this.checkBoxDev2.TabIndex = 14;
            this.checkBoxDev2.UseVisualStyleBackColor = true;
            this.checkBoxDev1.AutoSize = true;
            this.checkBoxDev1.Enabled = false;
            this.checkBoxDev1.Location = new Point(0x15, 0x33);
            this.checkBoxDev1.Name = "checkBoxDev1";
            this.checkBoxDev1.Size = new Size(15, 14);
            this.checkBoxDev1.TabIndex = 13;
            this.checkBoxDev1.UseVisualStyleBackColor = true;
            this.checkBoxDev0.AutoSize = true;
            this.checkBoxDev0.Enabled = false;
            this.checkBoxDev0.Location = new Point(0x15, 0x19);
            this.checkBoxDev0.Name = "checkBoxDev0";
            this.checkBoxDev0.Size = new Size(15, 14);
            this.checkBoxDev0.TabIndex = 12;
            this.checkBoxDev0.UseVisualStyleBackColor = true;
            this.labelDev3ChannelRange.AutoSize = true;
            this.labelDev3ChannelRange.Location = new Point(0x137, 0x67);
            this.labelDev3ChannelRange.Name = "labelDev3ChannelRange";
            this.labelDev3ChannelRange.Size = new Size(0x1f, 13);
            this.labelDev3ChannelRange.TabIndex = 11;
            this.labelDev3ChannelRange.Text = "to 32";
            this.numericUpDownDev3.Location = new Point(0xff, 0x65);
            int[] bits = new int[4];
            bits[0] = 1;
            this.numericUpDownDev3.Minimum = new decimal(bits);
            this.numericUpDownDev3.Name = "numericUpDownDev3";
            this.numericUpDownDev3.Size = new Size(50, 20);
            this.numericUpDownDev3.TabIndex = 10;
            bits = new int[4];
            bits[0] = 0x19;
            this.numericUpDownDev3.Value = new decimal(bits);
            this.numericUpDownDev3.ValueChanged += new EventHandler(this.numericUpDownDev3_ValueChanged);
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x3d, 0x67);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0xbc, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Device address 3 will handle channels";
            this.labelDev2ChannelRange.AutoSize = true;
            this.labelDev2ChannelRange.Location = new Point(0x137, 0x4d);
            this.labelDev2ChannelRange.Name = "labelDev2ChannelRange";
            this.labelDev2ChannelRange.Size = new Size(0x1f, 13);
            this.labelDev2ChannelRange.TabIndex = 8;
            this.labelDev2ChannelRange.Text = "to 24";
            this.numericUpDownDev2.Location = new Point(0xff, 0x4b);
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDownDev2.Minimum = new decimal(bits);
            this.numericUpDownDev2.Name = "numericUpDownDev2";
            this.numericUpDownDev2.Size = new Size(50, 20);
            this.numericUpDownDev2.TabIndex = 7;
            bits = new int[4];
            bits[0] = 0x11;
            this.numericUpDownDev2.Value = new decimal(bits);
            this.numericUpDownDev2.ValueChanged += new EventHandler(this.numericUpDownDev2_ValueChanged);
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x3d, 0x4d);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0xbc, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Device address 2 will handle channels";
            this.labelDev1ChannelRange.AutoSize = true;
            this.labelDev1ChannelRange.Location = new Point(0x137, 0x33);
            this.labelDev1ChannelRange.Name = "labelDev1ChannelRange";
            this.labelDev1ChannelRange.Size = new Size(0x1f, 13);
            this.labelDev1ChannelRange.TabIndex = 5;
            this.labelDev1ChannelRange.Text = "to 16";
            this.numericUpDownDev1.Location = new Point(0xff, 0x31);
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDownDev1.Minimum = new decimal(bits);
            this.numericUpDownDev1.Name = "numericUpDownDev1";
            this.numericUpDownDev1.Size = new Size(50, 20);
            this.numericUpDownDev1.TabIndex = 4;
            bits = new int[4];
            bits[0] = 9;
            this.numericUpDownDev1.Value = new decimal(bits);
            this.numericUpDownDev1.ValueChanged += new EventHandler(this.numericUpDownDev1_ValueChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x3d, 0x33);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0xbc, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Device address 1 will handle channels";
            this.labelDev0ChannelRange.AutoSize = true;
            this.labelDev0ChannelRange.Location = new Point(0x137, 0x19);
            this.labelDev0ChannelRange.Name = "labelDev0ChannelRange";
            this.labelDev0ChannelRange.Size = new Size(0x19, 13);
            this.labelDev0ChannelRange.TabIndex = 2;
            this.labelDev0ChannelRange.Text = "to 8";
            this.numericUpDownDev0.Location = new Point(0xff, 0x17);
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDownDev0.Minimum = new decimal(bits);
            this.numericUpDownDev0.Name = "numericUpDownDev0";
            this.numericUpDownDev0.Size = new Size(50, 20);
            this.numericUpDownDev0.TabIndex = 1;
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDownDev0.Value = new decimal(bits);
            this.numericUpDownDev0.ValueChanged += new EventHandler(this.numericUpDownDev0_ValueChanged);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x3d, 0x19);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0xbc, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Device address 0 will handle channels";
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(220, 0xc6);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x12d, 0xc6);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x184, 0xe9);
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
            this.numericUpDownDev3.EndInit();
            this.numericUpDownDev2.EndInit();
            this.numericUpDownDev1.EndInit();
            this.numericUpDownDev0.EndInit();
            base.ResumeLayout(false);
        }

        private void numericUpDownDev0_ValueChanged(object sender, EventArgs e)
        {
            this.UpdateRange(this.numericUpDownDev0, this.labelDev0ChannelRange);
        }

        private void numericUpDownDev1_ValueChanged(object sender, EventArgs e)
        {
            this.UpdateRange(this.numericUpDownDev1, this.labelDev1ChannelRange);
        }

        private void numericUpDownDev2_ValueChanged(object sender, EventArgs e)
        {
            this.UpdateRange(this.numericUpDownDev2, this.labelDev2ChannelRange);
        }

        private void numericUpDownDev3_ValueChanged(object sender, EventArgs e)
        {
            this.UpdateRange(this.numericUpDownDev3, this.labelDev3ChannelRange);
        }

        private void SearchDevices()
        {
            long num = 0L;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                num = K8055.K8055.SearchDevices();
                this.checkBoxDev0.Checked = (num & 1L) != 0L;
                this.checkBoxDev1.Checked = (num & 2L) != 0L;
                this.checkBoxDev2.Checked = (num & 4L) != 0L;
                this.checkBoxDev3.Checked = (num & 8L) != 0L;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            if ((num & 15L) == 0L)
            {
                MessageBox.Show("No devices were found.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void UpdateRange(NumericUpDown upDownStart, Label labelEnd)
        {
            labelEnd.Text = string.Format("to {0}", upDownStart.Value + 7M);
        }

        public int[] DeviceStartChannels
        {
            get
            {
                return new int[] { (((int) this.numericUpDownDev0.Value) - 1), (((int) this.numericUpDownDev1.Value) - 1), (((int) this.numericUpDownDev2.Value) - 1), (((int) this.numericUpDownDev3.Value) - 1) };
            }
        }
    }
}

