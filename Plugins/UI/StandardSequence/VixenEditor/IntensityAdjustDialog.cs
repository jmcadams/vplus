namespace VixenEditor
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal class IntensityAdjustDialog : Form
    {
        private IContainer components = null;
        private Label labelDelta;
        private bool m_actualLevels;
        private int m_delta;
        private Graphics m_graphics = null;
        private int m_largeDelta = 5;
        private int m_maxValue;

        public IntensityAdjustDialog(bool actualLevels)
        {
            this.InitializeComponent();
            this.m_graphics = base.CreateGraphics();
            this.ActualLevels = actualLevels;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            if (this.m_graphics != null)
            {
                this.m_graphics.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.labelDelta = new Label();
            base.SuspendLayout();
            this.labelDelta.AutoSize = true;
            this.labelDelta.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labelDelta.Location = new Point(12, 10);
            this.labelDelta.Name = "labelDelta";
            this.labelDelta.Size = new Size(0x63, 0x1f);
            this.labelDelta.TabIndex = 0;
            this.labelDelta.Text = "+100%";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x7b, 50);
            base.ControlBox = false;
            base.Controls.Add(this.labelDelta);
            base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            base.KeyPreview = true;
            base.Name = "IntensityAdjustDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Intensity Adjustment";
            base.TopMost = true;
            base.VisibleChanged += new EventHandler(this.IntensityAdjustDialog_VisibleChanged);
            base.KeyUp += new KeyEventHandler(this.IntensityAdjustDialog_KeyUp);
            base.KeyDown += new KeyEventHandler(this.IntensityAdjustDialog_KeyDown);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void IntensityAdjustDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                this.Delta = e.Alt ? -1 : -this.m_largeDelta;
            }
            else if (e.KeyCode == Keys.Up)
            {
                this.Delta = e.Alt ? 1 : this.m_largeDelta;
            }
        }

        private void IntensityAdjustDialog_KeyUp(object sender, KeyEventArgs e)
        {
            if (!e.Control)
            {
                base.Hide();
            }
        }

        private void IntensityAdjustDialog_VisibleChanged(object sender, EventArgs e)
        {
            if (base.Visible)
            {
                this.m_delta = 0;
            }
        }

        public bool ActualLevels
        {
            set
            {
                this.m_actualLevels = value;
                this.m_maxValue = value ? 0xff : 100;
            }
        }

        public int Delta
        {
            get
            {
                return this.m_delta;
            }
            set
            {
                if ((this.m_delta + value) < -this.m_maxValue)
                {
                    value = -this.m_maxValue - this.m_delta;
                }
                else if ((this.m_delta + value) > this.m_maxValue)
                {
                    value = this.m_maxValue - this.m_delta;
                }
                this.m_delta += value;
                string str = (this.m_delta > 0) ? string.Format("+{0}", this.m_delta) : this.m_delta.ToString();
                if (!this.m_actualLevels)
                {
                    str = str + "%";
                }
                this.labelDelta.Text = str;
                this.labelDelta.Left = ((int) (base.Width - this.m_graphics.MeasureString(this.labelDelta.Text, this.labelDelta.Font).Width)) >> 1;
            }
        }

        public int LargeDelta
        {
            set
            {
                this.m_largeDelta = value;
            }
        }
    }
}

