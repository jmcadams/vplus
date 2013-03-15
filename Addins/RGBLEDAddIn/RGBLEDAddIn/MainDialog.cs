namespace RGBLEDAddIn
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Vixen;

    public class MainDialog : Form
    {
        private Button buttonCancel;
        private Button buttonOK;
        private Button buttonPickColor;
        private Button buttonPullEnd;
        private Button buttonPullStart;
        private ColorDialog colorDialog;
        private ComboBox comboBoxDurationEvent;
        private ComboBox comboBoxDurationMinute;
        private ComboBox comboBoxDurationSecond;
        private ComboBox comboBoxRGBChannel;
        private ComboBox comboBoxStartEvent;
        private ComboBox comboBoxStartMinute;
        private ComboBox comboBoxStartSecond;
        private IContainer components;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label16;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label labelBIntensity;
        private Label labelCapacity;
        private Label labelGIntensity;
        private Label labelRIntensity;
        private EventSequence m_sequence;
        private NumericUpDown numericUpDownBlue;
        private NumericUpDown numericUpDownGreen;
        private NumericUpDown numericUpDownRed;
        private Panel panelColor;
        private Panel panelEndColor;
        private Panel panelStartColor;

        public MainDialog(EventSequence sequence)
        {
            int num2;
            string str;
            this.components = null;
            this.InitializeComponent();
            this.m_sequence = sequence;
            int num = sequence.ChannelCount / 3;
            this.labelCapacity.Text = num.ToString();
            for (num2 = 1; num2 <= num; num2++)
            {
                this.comboBoxRGBChannel.Items.Add(num2.ToString());
            }
            int num3 = sequence.Time / 60;
            int num4 = (int) Math.Round((double) (1000f / ((float) sequence.EventPeriod)), MidpointRounding.AwayFromZero);
            for (num2 = 0; num2 < num3; num2++)
            {
                str = num2.ToString();
                this.comboBoxStartMinute.Items.Add(str);
                this.comboBoxDurationMinute.Items.Add(str);
            }
            for (num2 = 0; num2 < 60; num2++)
            {
                str = num2.ToString();
                this.comboBoxStartSecond.Items.Add(str);
                this.comboBoxDurationSecond.Items.Add(str);
            }
            for (num2 = 0; num2 < num4; num2++)
            {
                str = num2.ToString();
                this.comboBoxStartEvent.Items.Add(str);
                this.comboBoxDurationEvent.Items.Add(str);
            }
            if (this.comboBoxStartMinute.Items.Count == 0)
            {
                this.comboBoxStartMinute.Items.Add("0");
            }
            if (this.comboBoxDurationMinute.Items.Count == 0)
            {
                this.comboBoxDurationMinute.Items.Add("0");
            }
            this.comboBoxStartMinute.SelectedIndex = this.comboBoxDurationMinute.SelectedIndex = this.comboBoxStartSecond.SelectedIndex = this.comboBoxDurationSecond.SelectedIndex = this.comboBoxStartEvent.SelectedIndex = this.comboBoxDurationEvent.SelectedIndex = 0;
            if (this.comboBoxRGBChannel.Items.Count > 0)
            {
                this.comboBoxRGBChannel.SelectedIndex = 0;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (this.comboBoxRGBChannel.SelectedIndex == -1)
            {
                base.DialogResult = System.Windows.Forms.DialogResult.None;
                MessageBox.Show("Please select an RGB channel, if one is available.\nOtherwise cancel the operation.", "RGBLED", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                base.ActiveControl = this.comboBoxRGBChannel;
            }
        }

        private void buttonPickColor_Click(object sender, EventArgs e)
        {
            if (this.colorDialog.ShowDialog() == DialogResult.OK)
            {
                this.panelColor.BackColor = this.colorDialog.Color;
                this.numericUpDownRed.Value = this.colorDialog.Color.R;
                this.numericUpDownGreen.Value = this.colorDialog.Color.G;
                this.numericUpDownBlue.Value = this.colorDialog.Color.B;
            }
        }

        private void buttonPullEnd_Click(object sender, EventArgs e)
        {
            this.panelEndColor.BackColor = this.panelColor.BackColor;
        }

        private void buttonPullStart_Click(object sender, EventArgs e)
        {
            this.panelStartColor.BackColor = this.panelColor.BackColor;
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
            this.colorDialog = new ColorDialog();
            this.groupBox1 = new GroupBox();
            this.labelBIntensity = new Label();
            this.labelGIntensity = new Label();
            this.labelRIntensity = new Label();
            this.label4 = new Label();
            this.numericUpDownBlue = new NumericUpDown();
            this.numericUpDownGreen = new NumericUpDown();
            this.numericUpDownRed = new NumericUpDown();
            this.label3 = new Label();
            this.label2 = new Label();
            this.label1 = new Label();
            this.panelColor = new Panel();
            this.buttonPickColor = new Button();
            this.groupBox2 = new GroupBox();
            this.labelCapacity = new Label();
            this.comboBoxDurationEvent = new ComboBox();
            this.comboBoxDurationSecond = new ComboBox();
            this.comboBoxDurationMinute = new ComboBox();
            this.label16 = new Label();
            this.comboBoxStartEvent = new ComboBox();
            this.comboBoxStartSecond = new ComboBox();
            this.comboBoxStartMinute = new ComboBox();
            this.label14 = new Label();
            this.label13 = new Label();
            this.label12 = new Label();
            this.label11 = new Label();
            this.comboBoxRGBChannel = new ComboBox();
            this.label10 = new Label();
            this.label9 = new Label();
            this.label8 = new Label();
            this.buttonPullEnd = new Button();
            this.panelEndColor = new Panel();
            this.label7 = new Label();
            this.buttonPullStart = new Button();
            this.panelStartColor = new Panel();
            this.label6 = new Label();
            this.label5 = new Label();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.groupBox1.SuspendLayout();
            this.numericUpDownBlue.BeginInit();
            this.numericUpDownGreen.BeginInit();
            this.numericUpDownRed.BeginInit();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.colorDialog.Color = Color.White;
            this.groupBox1.Controls.Add(this.labelBIntensity);
            this.groupBox1.Controls.Add(this.labelGIntensity);
            this.groupBox1.Controls.Add(this.labelRIntensity);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.numericUpDownBlue);
            this.groupBox1.Controls.Add(this.numericUpDownGreen);
            this.groupBox1.Controls.Add(this.numericUpDownRed);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.panelColor);
            this.groupBox1.Controls.Add(this.buttonPickColor);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x149, 120);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "RGB calculator";
            this.labelBIntensity.AutoSize = true;
            this.labelBIntensity.Location = new Point(0x103, 0x59);
            this.labelBIntensity.Name = "labelBIntensity";
            this.labelBIntensity.Size = new Size(0x15, 13);
            this.labelBIntensity.TabIndex = 11;
            this.labelBIntensity.Text = "0%";
            this.labelGIntensity.AutoSize = true;
            this.labelGIntensity.Location = new Point(0xd3, 0x59);
            this.labelGIntensity.Name = "labelGIntensity";
            this.labelGIntensity.Size = new Size(0x15, 13);
            this.labelGIntensity.TabIndex = 10;
            this.labelGIntensity.Text = "0%";
            this.labelRIntensity.AutoSize = true;
            this.labelRIntensity.Location = new Point(0xa3, 0x59);
            this.labelRIntensity.Name = "labelRIntensity";
            this.labelRIntensity.Size = new Size(0x15, 13);
            this.labelRIntensity.TabIndex = 9;
            this.labelRIntensity.Text = "0%";
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0xb3, 0x45);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x5f, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Channel intensities";
            this.numericUpDownBlue.Location = new Point(0xfb, 0x1f);
            int[] bits = new int[4];
            bits[0] = 0xff;
            this.numericUpDownBlue.Maximum = new decimal(bits);
            this.numericUpDownBlue.Name = "numericUpDownBlue";
            this.numericUpDownBlue.Size = new Size(0x2a, 20);
            this.numericUpDownBlue.TabIndex = 7;
            this.numericUpDownBlue.Enter += new EventHandler(this.numericUpDownRed_Enter);
            this.numericUpDownBlue.ValueChanged += new EventHandler(this.numericUpDownBlue_ValueChanged);
            this.numericUpDownGreen.Location = new Point(0xcb, 0x1f);
            bits = new int[4];
            bits[0] = 0xff;
            this.numericUpDownGreen.Maximum = new decimal(bits);
            this.numericUpDownGreen.Name = "numericUpDownGreen";
            this.numericUpDownGreen.Size = new Size(0x2a, 20);
            this.numericUpDownGreen.TabIndex = 6;
            this.numericUpDownGreen.Enter += new EventHandler(this.numericUpDownRed_Enter);
            this.numericUpDownGreen.ValueChanged += new EventHandler(this.numericUpDownGreen_ValueChanged);
            this.numericUpDownRed.Location = new Point(0x9b, 0x1f);
            bits = new int[4];
            bits[0] = 0xff;
            this.numericUpDownRed.Maximum = new decimal(bits);
            this.numericUpDownRed.Name = "numericUpDownRed";
            this.numericUpDownRed.Size = new Size(0x2a, 20);
            this.numericUpDownRed.TabIndex = 5;
            this.numericUpDownRed.Enter += new EventHandler(this.numericUpDownRed_Enter);
            this.numericUpDownRed.ValueChanged += new EventHandler(this.numericUpDownRed_ValueChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0x102, 15);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x1c, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Blue";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0xce, 15);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x24, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Green";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0xa3, 15);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x1b, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Red";
            this.panelColor.Location = new Point(15, 0x42);
            this.panelColor.Name = "panelColor";
            this.panelColor.Size = new Size(0x76, 0x24);
            this.panelColor.TabIndex = 1;
            this.buttonPickColor.Location = new Point(15, 0x1c);
            this.buttonPickColor.Name = "buttonPickColor";
            this.buttonPickColor.Size = new Size(0x4b, 0x17);
            this.buttonPickColor.TabIndex = 0;
            this.buttonPickColor.Text = "Pick a color";
            this.buttonPickColor.UseVisualStyleBackColor = true;
            this.buttonPickColor.Click += new EventHandler(this.buttonPickColor_Click);
            this.groupBox2.Controls.Add(this.labelCapacity);
            this.groupBox2.Controls.Add(this.comboBoxDurationEvent);
            this.groupBox2.Controls.Add(this.comboBoxDurationSecond);
            this.groupBox2.Controls.Add(this.comboBoxDurationMinute);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.comboBoxStartEvent);
            this.groupBox2.Controls.Add(this.comboBoxStartSecond);
            this.groupBox2.Controls.Add(this.comboBoxStartMinute);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.comboBoxRGBChannel);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.buttonPullEnd);
            this.groupBox2.Controls.Add(this.panelEndColor);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.buttonPullStart);
            this.groupBox2.Controls.Add(this.panelStartColor);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new Point(12, 0x8a);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x149, 0x141);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Create a gradient";
            this.labelCapacity.AutoSize = true;
            this.labelCapacity.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelCapacity.Location = new Point(0xbd, 0x87);
            this.labelCapacity.Name = "labelCapacity";
            this.labelCapacity.Size = new Size(14, 13);
            this.labelCapacity.TabIndex = 8;
            this.labelCapacity.Text = "0";
            this.comboBoxDurationEvent.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxDurationEvent.FormattingEnabled = true;
            this.comboBoxDurationEvent.Location = new Point(0xf3, 0x11e);
            this.comboBoxDurationEvent.Name = "comboBoxDurationEvent";
            this.comboBoxDurationEvent.Size = new Size(0x35, 0x15);
            this.comboBoxDurationEvent.TabIndex = 0x16;
            this.comboBoxDurationSecond.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxDurationSecond.FormattingEnabled = true;
            this.comboBoxDurationSecond.Location = new Point(170, 0x11e);
            this.comboBoxDurationSecond.Name = "comboBoxDurationSecond";
            this.comboBoxDurationSecond.Size = new Size(0x35, 0x15);
            this.comboBoxDurationSecond.TabIndex = 0x15;
            this.comboBoxDurationMinute.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxDurationMinute.FormattingEnabled = true;
            this.comboBoxDurationMinute.Location = new Point(0x62, 0x11e);
            this.comboBoxDurationMinute.Name = "comboBoxDurationMinute";
            this.comboBoxDurationMinute.Size = new Size(0x35, 0x15);
            this.comboBoxDurationMinute.TabIndex = 20;
            this.label16.AutoSize = true;
            this.label16.Location = new Point(0x12, 0x121);
            this.label16.Name = "label16";
            this.label16.Size = new Size(50, 13);
            this.label16.TabIndex = 0x13;
            this.label16.Text = "Duration:";
            this.comboBoxStartEvent.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxStartEvent.FormattingEnabled = true;
            this.comboBoxStartEvent.Location = new Point(0xf3, 0x103);
            this.comboBoxStartEvent.Name = "comboBoxStartEvent";
            this.comboBoxStartEvent.Size = new Size(0x35, 0x15);
            this.comboBoxStartEvent.TabIndex = 0x12;
            this.comboBoxStartSecond.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxStartSecond.FormattingEnabled = true;
            this.comboBoxStartSecond.Location = new Point(170, 0x103);
            this.comboBoxStartSecond.Name = "comboBoxStartSecond";
            this.comboBoxStartSecond.Size = new Size(0x35, 0x15);
            this.comboBoxStartSecond.TabIndex = 0x11;
            this.comboBoxStartMinute.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxStartMinute.FormattingEnabled = true;
            this.comboBoxStartMinute.Location = new Point(0x62, 0x103);
            this.comboBoxStartMinute.Name = "comboBoxStartMinute";
            this.comboBoxStartMinute.Size = new Size(0x35, 0x15);
            this.comboBoxStartMinute.TabIndex = 0x10;
            this.label14.AutoSize = true;
            this.label14.Location = new Point(0x12, 0x106);
            this.label14.Name = "label14";
            this.label14.Size = new Size(0x2c, 13);
            this.label14.TabIndex = 15;
            this.label14.Text = "Start at:";
            this.label13.AutoSize = true;
            this.label13.Location = new Point(0xfc, 0xec);
            this.label13.Name = "label13";
            this.label13.Size = new Size(0x23, 13);
            this.label13.TabIndex = 14;
            this.label13.Text = "Event";
            this.label12.AutoSize = true;
            this.label12.Location = new Point(0xae, 0xec);
            this.label12.Name = "label12";
            this.label12.Size = new Size(0x2c, 13);
            this.label12.TabIndex = 13;
            this.label12.Text = "Second";
            this.label11.AutoSize = true;
            this.label11.Location = new Point(0x6b, 0xec);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x27, 13);
            this.label11.TabIndex = 12;
            this.label11.Text = "Minute";
            this.comboBoxRGBChannel.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxRGBChannel.FormattingEnabled = true;
            this.comboBoxRGBChannel.Location = new Point(0x84, 0xb6);
            this.comboBoxRGBChannel.Name = "comboBoxRGBChannel";
            this.comboBoxRGBChannel.Size = new Size(0x41, 0x15);
            this.comboBoxRGBChannel.TabIndex = 11;
            this.label10.AutoSize = true;
            this.label10.Location = new Point(0x12, 0x9e);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x11c, 13);
            this.label10.TabIndex = 10;
            this.label10.Text = "Which RGB channel do you want to apply this gradient to?";
            this.label9.AutoSize = true;
            this.label9.Location = new Point(0xca, 0x87);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x4f, 13);
            this.label9.TabIndex = 9;
            this.label9.Text = "RGB channels.";
            this.label8.AutoSize = true;
            this.label8.Location = new Point(0x11, 0x87);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0xad, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "This sequence has the capacity for";
            this.buttonPullEnd.Location = new Point(0xc1, 0x53);
            this.buttonPullEnd.Name = "buttonPullEnd";
            this.buttonPullEnd.Size = new Size(0x6d, 0x17);
            this.buttonPullEnd.TabIndex = 6;
            this.buttonPullEnd.Text = "Pull from calculator";
            this.buttonPullEnd.UseVisualStyleBackColor = true;
            this.buttonPullEnd.Click += new EventHandler(this.buttonPullEnd_Click);
            this.panelEndColor.Location = new Point(0x5c, 0x55);
            this.panelEndColor.Name = "panelEndColor";
            this.panelEndColor.Size = new Size(0x53, 0x10);
            this.panelEndColor.TabIndex = 5;
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0x12, 0x58);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x34, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "End color";
            this.buttonPullStart.Location = new Point(0xc1, 0x36);
            this.buttonPullStart.Name = "buttonPullStart";
            this.buttonPullStart.Size = new Size(0x6d, 0x17);
            this.buttonPullStart.TabIndex = 3;
            this.buttonPullStart.Text = "Pull from calculator";
            this.buttonPullStart.UseVisualStyleBackColor = true;
            this.buttonPullStart.Click += new EventHandler(this.buttonPullStart_Click);
            this.panelStartColor.Location = new Point(0x5c, 0x38);
            this.panelStartColor.Name = "panelStartColor";
            this.panelStartColor.Size = new Size(0x53, 0x10);
            this.panelStartColor.TabIndex = 2;
            this.label6.AutoSize = true;
            this.label6.Location = new Point(0x12, 0x3b);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x37, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Start color";
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x11, 0x17);
            this.label5.Name = "label5";
            this.label5.Size = new Size(260, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Use this to create color gradients within the sequence";
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new Point(0xb9, 0x1d2);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x10a, 0x1d2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x161, 0x1f5);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            base.Name = "MainDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "RGBLED";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.numericUpDownBlue.EndInit();
            this.numericUpDownGreen.EndInit();
            this.numericUpDownRed.EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            base.ResumeLayout(false);
        }

        private void numericUpDownBlue_ValueChanged(object sender, EventArgs e)
        {
            this.labelBIntensity.Text = ((int) Math.Round((double) ((((float) this.numericUpDownBlue.Value) / 255f) * 100f), MidpointRounding.AwayFromZero)).ToString() + "%";
            this.panelColor.BackColor = Color.FromArgb((int) this.numericUpDownRed.Value, (int) this.numericUpDownGreen.Value, (int) this.numericUpDownBlue.Value);
        }

        private void numericUpDownGreen_ValueChanged(object sender, EventArgs e)
        {
            this.labelGIntensity.Text = ((int) Math.Round((double) ((((float) this.numericUpDownGreen.Value) / 255f) * 100f), MidpointRounding.AwayFromZero)).ToString() + "%";
            this.panelColor.BackColor = Color.FromArgb((int) this.numericUpDownRed.Value, (int) this.numericUpDownGreen.Value, (int) this.numericUpDownBlue.Value);
        }

        private void numericUpDownRed_Enter(object sender, EventArgs e)
        {
            NumericUpDown down = (NumericUpDown) sender;
            int num = (int) down.Value;
            down.Select(0, num.ToString().Length);
        }

        private void numericUpDownRed_ValueChanged(object sender, EventArgs e)
        {
            this.labelRIntensity.Text = ((int) Math.Round((double) ((((float) this.numericUpDownRed.Value) / 255f) * 100f), MidpointRounding.AwayFromZero)).ToString() + "%";
            this.panelColor.BackColor = Color.FromArgb((int) this.numericUpDownRed.Value, (int) this.numericUpDownGreen.Value, (int) this.numericUpDownBlue.Value);
        }

        public int DurationEventCount
        {
            get
            {
                float num = 1000f / ((float) this.m_sequence.EventPeriod);
                return (int) Math.Round((double) ((this.comboBoxDurationEvent.SelectedIndex + (this.comboBoxDurationSecond.SelectedIndex * num)) + ((this.comboBoxDurationMinute.SelectedIndex * 60) * num)), MidpointRounding.AwayFromZero);
            }
        }

        public Color EndColor
        {
            get
            {
                return this.panelEndColor.BackColor;
            }
        }

        public int StartChannel
        {
            get
            {
                return (this.comboBoxRGBChannel.SelectedIndex * 3);
            }
        }

        public Color StartColor
        {
            get
            {
                return this.panelStartColor.BackColor;
            }
        }

        public int StartEventIndex
        {
            get
            {
                float num = 1000f / ((float) this.m_sequence.EventPeriod);
                return (int) Math.Round((double) ((this.comboBoxStartEvent.SelectedIndex + (this.comboBoxStartSecond.SelectedIndex * num)) + ((this.comboBoxStartMinute.SelectedIndex * 60) * num)), MidpointRounding.AwayFromZero);
            }
        }
    }
}

