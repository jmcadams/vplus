namespace CometConversion
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class OptionsDialog : Form
    {
        private Button buttonCancel;
        private Button buttonColor;
        private Button buttonOK;
        private ColorDialog colorDialog;
        private IContainer components = null;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox textBoxEventPeriodLength;

        public OptionsDialog()
        {
            this.InitializeComponent();
        }

        private void buttonColor_Click(object sender, EventArgs e)
        {
            if (this.colorDialog.ShowDialog() == DialogResult.OK)
            {
                this.buttonColor.BackColor = this.colorDialog.Color;
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
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.textBoxEventPeriodLength = new TextBox();
            this.label3 = new Label();
            this.buttonColor = new Button();
            this.colorDialog = new ColorDialog();
            this.label4 = new Label();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.buttonColor);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxEventPeriodLength);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x10f, 0xd3);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Conversion Options";
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(0x7c, 0xe7);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonCancel.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0xcd, 0xe7);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(12, 0x15);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0xf8, 0x27);
            this.label1.TabIndex = 0;
            this.label1.Text = "The default event period length is 100 ms.\r\nYou may want to change this to account for timings\r\nsuch as 250 ms in your Comet playlist.";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(12, 0x4d);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x66, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Event period length:";
            this.textBoxEventPeriodLength.Location = new Point(0x79, 0x4a);
            this.textBoxEventPeriodLength.MaxLength = 4;
            this.textBoxEventPeriodLength.Name = "textBoxEventPeriodLength";
            this.textBoxEventPeriodLength.Size = new Size(0x21, 20);
            this.textBoxEventPeriodLength.TabIndex = 2;
            this.textBoxEventPeriodLength.Text = "100";
            this.label3.AutoSize = true;
            this.label3.Location = new Point(12, 0x71);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0xfc, 0x27);
            this.label3.TabIndex = 3;
            this.label3.Text = "The preview has a black background, so you may\r\nnot want any black channels.  Choose the color you\r\nwould like to replace black with.";
            this.buttonColor.BackColor = Color.White;
            this.buttonColor.Location = new Point(0x62, 0xa5);
            this.buttonColor.Name = "buttonColor";
            this.buttonColor.Size = new Size(0x4b, 0x17);
            this.buttonColor.TabIndex = 4;
            this.buttonColor.UseVisualStyleBackColor = false;
            this.buttonColor.Click += new EventHandler(this.buttonColor_Click);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0x9c, 0x4d);
            this.label4.Name = "label4";
            this.label4.Size = new Size(20, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "ms";
            base.AcceptButton = this.buttonOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0x124, 0x10a);
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "OptionsDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
        }

        public Color BlackReplacement
        {
            get
            {
                return Color.FromArgb(-16777216 | this.buttonColor.BackColor.ToArgb());
            }
        }

        public int EventPeriod
        {
            get
            {
                try
                {
                    return Convert.ToInt32(this.textBoxEventPeriodLength.Text);
                }
                catch
                {
                    return 100;
                }
            }
        }
    }
}

