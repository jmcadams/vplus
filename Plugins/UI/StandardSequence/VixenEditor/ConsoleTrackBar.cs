using System.Globalization;

namespace VixenEditor
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class ConsoleTrackBar : UserControl
    {
        private readonly IContainer components = null;
        private Label _label;
        private bool _cascadeMasterEvents;
        private ConsoleTrackBar _master;
        private int _resetIndex = -1;
        private int _selectedTextIndex = -1;
        private string[] _channelNames;
        private Panel _panelText;
        private TextBox _textBox;
        private TrackBar _trackBar;

        public event ValueChangedHandler ValueChanged;

        public ConsoleTrackBar()
        {
            InitializeComponent();
            ResetAssignment();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            _trackBar = new TrackBar();
            _textBox = new TextBox();
            _label = new Label();
            _panelText = new Panel();
            _trackBar.BeginInit();
            SuspendLayout();
            _trackBar.Anchor = AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;
            _trackBar.LargeChange = 0x40;
            _trackBar.Location = new Point(0x21, 3);
            _trackBar.Maximum = 0xff;
            _trackBar.Name = "_trackBar";
            _trackBar.Orientation = Orientation.Vertical;
            _trackBar.Size = new Size(0x2d, 0xe5);
            _trackBar.TabIndex = 1;
            _trackBar.TickFrequency = 0x40;
            _trackBar.TickStyle = TickStyle.Both;
            _trackBar.ValueChanged += trackBar_ValueChanged;
            _trackBar.Scroll += trackBar_Scroll;
            _textBox.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            _textBox.Location = new Point(0x26, 0xee);
            _textBox.Name = "_textBox";
            _textBox.Size = new Size(0x23, 20);
            _textBox.TabIndex = 2;
            _textBox.Text = @"0";
            _textBox.KeyPress += textBox_KeyPress;
            _label.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            _label.AutoSize = true;
            _label.Location = new Point(40, 0x105);
            _label.Name = "_label";
            _label.Size = new Size(0x15, 13);
            _label.TabIndex = 3;
            _label.Text = @"0%";
            _panelText.BackColor = Color.Gainsboro;
            _panelText.Dock = DockStyle.Left;
            _panelText.Enabled = false;
            _panelText.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            _panelText.Location = new Point(0, 0);
            _panelText.Name = "_panelText";
            _panelText.Size = new Size(0x12, 0x11c);
            _panelText.TabIndex = 5;
            _panelText.Paint += panelText_Paint;
            _panelText.Click += panelText_Click;
            _panelText.EnabledChanged += panelText_EnabledChanged;
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(_panelText);
            Controls.Add(_label);
            Controls.Add(_textBox);
            Controls.Add(_trackBar);
            Name = "ConsoleTrackBar";
            Size = new Size(90, 0x11c);
            _trackBar.EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void MasterValueChanged(object sender)
        {
            SetValue(Master.Value, true);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            _panelText.Refresh();
        }

        protected virtual void OnValueChanged()
        {
            if (ValueChanged != null)
            {
                ValueChanged(this);
            }
        }

        private void panelText_Click(object sender, EventArgs e)
        {
            if (_panelText.Enabled)
            {
                var selection = new ThinSelection(_channelNames) {
                                                                     Location =
                                                                         PointToScreen(
                                                                             new Point(_panelText.Right, _panelText.Top))
                                                                 };
                if (selection.ShowDialog() == DialogResult.OK)
                {
                    if (selection.SelectedIndex != ResetIndex)
                    {
                        Text = _channelNames[_selectedTextIndex = selection.SelectedIndex];
                    }
                    else
                    {
                        ResetAssignment();
                    }
                }
            }
        }

        private void panelText_EnabledChanged(object sender, EventArgs e)
        {
            _panelText.Cursor = _panelText.Enabled ? Cursors.Hand : Cursors.Default;
        }

        private void panelText_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(2f, -(_panelText.Bottom - 5), MatrixOrder.Append);
            e.Graphics.RotateTransform(-90f, MatrixOrder.Append);
            e.Graphics.TranslateTransform(2f, _panelText.Bottom - 5, MatrixOrder.Append);
            e.Graphics.DrawString(Text, _panelText.Font, _panelText.Enabled ? Brushes.White : Brushes.WhiteSmoke, 2f, _panelText.Bottom - 5);
            e.Graphics.ResetTransform();
        }

        private void ResetAssignment()
        {
            Text = " -- unassigned -- ";
            _selectedTextIndex = -1;
        }

        private void SetValue(int value, bool masterEventResponse = false)
        {
            _trackBar.Value = value;
            _textBox.Text = value.ToString(CultureInfo.InvariantCulture);
            _label.Text = (value * 100) / 255 + @"%";
            if (!(masterEventResponse && !_cascadeMasterEvents))
            {
                OnValueChanged();
            }
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                int num;
                e.Handled = true;
                if (int.TryParse(_textBox.Text, out num))
                {
                    if ((num >= 0) && (num <= 0xff))
                    {
                        SetValue(num);
                    }
                }
                else
                {
                    _textBox.Text = _trackBar.Value.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            SetValue(_trackBar.Value);
        }

        private void trackBar_ValueChanged(object sender, EventArgs e)
        {
        }

        public bool AllowText
        {
            get
            {
                return _panelText.Visible;
            }
            set
            {
                _panelText.Visible = value;
            }
        }

        public bool CascadeMasterEvents
        {
            get
            {
                return _cascadeMasterEvents;
            }
            set
            {
                _cascadeMasterEvents = value;
            }
        }

        public ConsoleTrackBar Master
        {
            get
            {
                return _master;
            }
            set
            {
                if (_master != null)
                {
                    _master.ValueChanged = (ValueChangedHandler) Delegate.Remove(_master.ValueChanged, new ValueChangedHandler(MasterValueChanged));
                }
                _master = value;
                if (_master != null)
                {
                    _master.ValueChanged = (ValueChangedHandler) Delegate.Combine(_master.ValueChanged, new ValueChangedHandler(MasterValueChanged));
                }
            }
        }

        public int ResetIndex
        {
            get
            {
                return _resetIndex;
            }
            set
            {
                _resetIndex = value;
                if (SelectedTextIndex == _resetIndex)
                {
                    ResetAssignment();
                }
            }
        }

        public int SelectedTextIndex
        {
            get
            {
                return _selectedTextIndex;
            }
        }

        public string[] TextStrings
        {
            set
            {
                _channelNames = value;
                _panelText.Enabled = (_channelNames != null) && (_channelNames.Length > 0);
            }
        }

        public int Value
        {
            get
            {
                return _trackBar.Value;
            }
            set
            {
                if ((value >= _trackBar.Minimum) && (value <= _trackBar.Maximum))
                {
                    SetValue(value);
                }
            }
        }

        public delegate void ValueChangedHandler(object sender);
    }
}

