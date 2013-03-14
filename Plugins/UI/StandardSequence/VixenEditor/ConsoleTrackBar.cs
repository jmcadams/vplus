namespace VixenEditor
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class ConsoleTrackBar : UserControl
    {
        private IContainer components = null;
        private Label label;
        private bool m_cascadeMasterEvents = false;
        private ConsoleTrackBar m_master = null;
        private int m_resetIndex = -1;
        private int m_selectedTextIndex = -1;
        private string[] m_strings;
        private Panel panelText;
        private TextBox textBox;
        private TrackBar trackBar;
        private const string UNASSIGNED_TEXT = " -- unassigned -- ";

        public event ValueChangedHandler ValueChanged;

        public ConsoleTrackBar()
        {
            this.InitializeComponent();
            this.ResetAssignment();
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
            this.trackBar = new TrackBar();
            this.textBox = new TextBox();
            this.label = new Label();
            this.panelText = new Panel();
            this.trackBar.BeginInit();
            base.SuspendLayout();
            this.trackBar.Anchor = AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;
            this.trackBar.LargeChange = 0x40;
            this.trackBar.Location = new Point(0x21, 3);
            this.trackBar.Maximum = 0xff;
            this.trackBar.Name = "trackBar";
            this.trackBar.Orientation = Orientation.Vertical;
            this.trackBar.Size = new Size(0x2d, 0xe5);
            this.trackBar.TabIndex = 1;
            this.trackBar.TickFrequency = 0x40;
            this.trackBar.TickStyle = TickStyle.Both;
            this.trackBar.ValueChanged += new EventHandler(this.trackBar_ValueChanged);
            this.trackBar.Scroll += new EventHandler(this.trackBar_Scroll);
            this.textBox.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.textBox.Location = new Point(0x26, 0xee);
            this.textBox.Name = "textBox";
            this.textBox.Size = new Size(0x23, 20);
            this.textBox.TabIndex = 2;
            this.textBox.Text = "0";
            this.textBox.KeyPress += new KeyPressEventHandler(this.textBox_KeyPress);
            this.label.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.label.AutoSize = true;
            this.label.Location = new Point(40, 0x105);
            this.label.Name = "label";
            this.label.Size = new Size(0x15, 13);
            this.label.TabIndex = 3;
            this.label.Text = "0%";
            this.panelText.BackColor = Color.Gainsboro;
            this.panelText.Dock = DockStyle.Left;
            this.panelText.Enabled = false;
            this.panelText.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.panelText.Location = new Point(0, 0);
            this.panelText.Name = "panelText";
            this.panelText.Size = new Size(0x12, 0x11c);
            this.panelText.TabIndex = 5;
            this.panelText.Paint += new PaintEventHandler(this.panelText_Paint);
            this.panelText.Click += new EventHandler(this.panelText_Click);
            this.panelText.EnabledChanged += new EventHandler(this.panelText_EnabledChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.panelText);
            base.Controls.Add(this.label);
            base.Controls.Add(this.textBox);
            base.Controls.Add(this.trackBar);
            base.Name = "ConsoleTrackBar";
            base.Size = new Size(90, 0x11c);
            this.trackBar.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void masterValueChanged(object sender)
        {
            this.SetValue(this.Master.Value, true);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            this.panelText.Refresh();
        }

        protected virtual void OnValueChanged()
        {
            if (this.ValueChanged != null)
            {
                this.ValueChanged(this);
            }
        }

        private void panelText_Click(object sender, EventArgs e)
        {
            if (this.panelText.Enabled)
            {
                ThinSelection selection = new ThinSelection(this.m_strings);
                selection.Location = base.PointToScreen(new Point(this.panelText.Right, this.panelText.Top));
                if (selection.ShowDialog() == DialogResult.OK)
                {
                    if (selection.SelectedIndex != this.ResetIndex)
                    {
                        this.Text = this.m_strings[this.m_selectedTextIndex = selection.SelectedIndex];
                    }
                    else
                    {
                        this.ResetAssignment();
                    }
                }
            }
        }

        private void panelText_EnabledChanged(object sender, EventArgs e)
        {
            this.panelText.Cursor = this.panelText.Enabled ? Cursors.Hand : Cursors.Default;
        }

        private void panelText_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(2f, (float) -(this.panelText.Bottom - 5), MatrixOrder.Append);
            e.Graphics.RotateTransform(-90f, MatrixOrder.Append);
            e.Graphics.TranslateTransform(2f, (float) (this.panelText.Bottom - 5), MatrixOrder.Append);
            e.Graphics.DrawString(this.Text, this.panelText.Font, this.panelText.Enabled ? Brushes.White : Brushes.WhiteSmoke, 2f, (float) (this.panelText.Bottom - 5));
            e.Graphics.ResetTransform();
        }

        private void ResetAssignment()
        {
            this.Text = " -- unassigned -- ";
            this.m_selectedTextIndex = -1;
        }

        private void SetValue(int value)
        {
            this.SetValue(value, false);
        }

        private void SetValue(int value, bool masterEventResponse)
        {
            this.trackBar.Value = value;
            this.textBox.Text = value.ToString();
            this.label.Text = (((value * 100) / 0xff)).ToString() + "%";
            if (!(masterEventResponse && (!masterEventResponse || !this.m_cascadeMasterEvents)))
            {
                this.OnValueChanged();
            }
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                int num;
                e.Handled = true;
                if (int.TryParse(this.textBox.Text, out num))
                {
                    if ((num >= 0) && (num <= 0xff))
                    {
                        this.SetValue(num);
                    }
                }
                else
                {
                    this.textBox.Text = this.trackBar.Value.ToString();
                }
            }
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            this.SetValue(this.trackBar.Value);
        }

        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
        }

        public bool AllowText
        {
            get
            {
                return this.panelText.Visible;
            }
            set
            {
                this.panelText.Visible = value;
            }
        }

        public bool CascadeMasterEvents
        {
            get
            {
                return this.m_cascadeMasterEvents;
            }
            set
            {
                this.m_cascadeMasterEvents = value;
            }
        }

        public ConsoleTrackBar Master
        {
            get
            {
                return this.m_master;
            }
            set
            {
                if (this.m_master != null)
                {
                    this.m_master.ValueChanged = (ValueChangedHandler) Delegate.Remove(this.m_master.ValueChanged, new ValueChangedHandler(this.masterValueChanged));
                }
                this.m_master = value;
                if (this.m_master != null)
                {
                    this.m_master.ValueChanged = (ValueChangedHandler) Delegate.Combine(this.m_master.ValueChanged, new ValueChangedHandler(this.masterValueChanged));
                }
            }
        }

        public int ResetIndex
        {
            get
            {
                return this.m_resetIndex;
            }
            set
            {
                this.m_resetIndex = value;
                if (this.SelectedTextIndex == this.m_resetIndex)
                {
                    this.ResetAssignment();
                }
            }
        }

        public int SelectedTextIndex
        {
            get
            {
                return this.m_selectedTextIndex;
            }
        }

        public string[] TextStrings
        {
            set
            {
                this.m_strings = value;
                this.panelText.Enabled = (this.m_strings != null) && (this.m_strings.Length > 0);
            }
        }

        public int Value
        {
            get
            {
                return this.trackBar.Value;
            }
            set
            {
                if ((value >= this.trackBar.Minimum) && (value <= this.trackBar.Maximum))
                {
                    this.SetValue(value);
                }
            }
        }

        public delegate void ValueChangedHandler(object sender);
    }
}

