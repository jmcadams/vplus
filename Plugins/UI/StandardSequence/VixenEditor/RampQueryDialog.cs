namespace VixenEditor
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal class RampQueryDialog : Form
    {
        private Button buttonCancel;
        private Button buttonOK;
        private IContainer components;
        private Label label1;
        private Label label2;
        private bool m_actualLevels;
        private NumericUpDown numericUpDownEnd;
        private NumericUpDown numericUpDownStart;

        public RampQueryDialog(int minLevel, int maxLevel, bool descending, bool actualLevels)
        {
            int num;
            int num2;
            this.components = null;
            this.InitializeComponent();
            this.m_actualLevels = actualLevels;
            if (actualLevels)
            {
                num = minLevel;
                num2 = maxLevel;
            }
            else
            {
                num = (int) Math.Round((double) ((((float) minLevel) / 255f) * 100f), MidpointRounding.AwayFromZero);
                num2 = (int) Math.Round((double) ((((float) maxLevel) / 255f) * 100f), MidpointRounding.AwayFromZero);
            }
            this.numericUpDownStart.Minimum = num;
            this.numericUpDownEnd.Minimum = num;
            this.numericUpDownStart.Maximum = num2;
            this.numericUpDownEnd.Maximum = num2;
            if (!descending)
            {
                this.numericUpDownStart.Value = this.numericUpDownEnd.Minimum;
                this.numericUpDownEnd.Value = this.numericUpDownEnd.Maximum;
            }
            else
            {
                this.numericUpDownStart.Value = this.numericUpDownEnd.Maximum;
                this.numericUpDownEnd.Value = this.numericUpDownEnd.Minimum;
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
            this.label1 = new Label();
            this.numericUpDownStart = new NumericUpDown();
            this.label2 = new Label();
            this.numericUpDownEnd = new NumericUpDown();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.numericUpDownStart.BeginInit();
            this.numericUpDownEnd.BeginInit();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(12, 0x15);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Starting level";
            this.numericUpDownStart.Location = new Point(0x63, 0x13);
            this.numericUpDownStart.Name = "numericUpDownStart";
            this.numericUpDownStart.Size = new Size(0x2f, 20);
            this.numericUpDownStart.TabIndex = 1;
            this.numericUpDownStart.Enter += new EventHandler(this.numericUpDownStart_Enter);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(12, 0x30);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x41, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Ending level";
            this.numericUpDownEnd.Location = new Point(0x63, 0x2d);
            this.numericUpDownEnd.Name = "numericUpDownEnd";
            this.numericUpDownEnd.Size = new Size(0x2e, 20);
            this.numericUpDownEnd.TabIndex = 3;
            this.numericUpDownEnd.Enter += new EventHandler(this.numericUpDownEnd_Enter);
            this.buttonOK.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonOK.DialogResult = DialogResult.OK;
            this.buttonOK.Location = new Point(0x18, 0x55);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new Size(0x4b, 0x17);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonCancel.DialogResult = DialogResult.Cancel;
            this.buttonCancel.Location = new Point(0x69, 0x55);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(0x4b, 0x17);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            base.AcceptButton = this.buttonOK;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.CancelButton = this.buttonCancel;
            base.ClientSize = new Size(0xc0, 120);
            base.ControlBox = false;
            base.Controls.Add(this.buttonCancel);
            base.Controls.Add(this.buttonOK);
            base.Controls.Add(this.numericUpDownEnd);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.numericUpDownStart);
            base.Controls.Add(this.label1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "RampQueryDialog";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Ramp parameters";
            this.numericUpDownStart.EndInit();
            this.numericUpDownEnd.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void numericUpDownEnd_Enter(object sender, EventArgs e)
        {
            this.numericUpDownEnd.Select(0, this.numericUpDownEnd.Value.ToString().Length);
        }

        private void numericUpDownStart_Enter(object sender, EventArgs e)
        {
            this.numericUpDownStart.Select(0, this.numericUpDownStart.Value.ToString().Length);
        }

        public int EndingLevel
        {
            get
            {
                if (this.m_actualLevels)
                {
                    return (int) this.numericUpDownEnd.Value;
                }
                return (int) ((this.numericUpDownEnd.Value / 100M) * 255M);
            }
        }

        public int StartingLevel
        {
            get
            {
                if (this.m_actualLevels)
                {
                    return (int) this.numericUpDownStart.Value;
                }
                return (int) ((this.numericUpDownStart.Value / 100M) * 255M);
            }
        }
    }
}

