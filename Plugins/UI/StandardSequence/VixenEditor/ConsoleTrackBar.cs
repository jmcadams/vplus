using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using VixenPlus.Properties;

using CommonUtils;

namespace VixenEditor {


    public partial class ConsoleTrackBar : UserControl {
        private bool _cascadeMasterEvents;
        private ConsoleTrackBar _master;
        private int _resetIndex = -1;
        private int _selectedTextIndex = -1;
        private Channel[] _channelNames;
        public event ValueChangedHandler ValueChanged;


        public ConsoleTrackBar() {
            InitializeComponent();
            ResetAssignment();
        }


        private void MasterValueChanged(object sender) {
            SetValue(Master.Value, true);
        }


        protected override void OnTextChanged(EventArgs e) {
            _panelText.Refresh();
        }


        protected virtual void OnValueChanged() {
            if (ValueChanged != null) {
                ValueChanged(this);
            }
        }


        private void panelText_Click(object sender, EventArgs e) {
            if (!_panelText.Enabled) {
                return;
            }

            var selection = new ThinSelection(_channelNames) {Location = PointToScreen(new Point(_panelText.Right, _panelText.Top))};
            if (selection.ShowDialog() != DialogResult.OK) {
                return;
            }

            if (selection.SelectedIndex != ResetIndex) {
                Text = _channelNames[_selectedTextIndex = selection.SelectedIndex].Name;
                _panelText.BackColor = _channelNames[_selectedTextIndex].Color;
            }
            else {
                ResetAssignment();
            }
        }


        private void panelText_EnabledChanged(object sender, EventArgs e) {
            _panelText.Cursor = _panelText.Enabled ? Cursors.Hand : Cursors.Default;
        }


        private void panelText_Paint(object sender, PaintEventArgs e) {
            e.Graphics.TranslateTransform(2f, -(_panelText.Bottom - 5), MatrixOrder.Append);
            e.Graphics.RotateTransform(-90f, MatrixOrder.Append);
            e.Graphics.TranslateTransform(2f, _panelText.Bottom - 5, MatrixOrder.Append);
            e.Graphics.DrawString(Text, _panelText.Font, _panelText.Enabled ? _panelText.BackColor.GetTextColor() : Brushes.WhiteSmoke, 2f, _panelText.Bottom - 5);
            e.Graphics.ResetTransform();
        }


        private void ResetAssignment() {
            Text = Resources.Unassigned;
            _panelText.BackColor = Color.Gainsboro;
            _selectedTextIndex = -1;
        }


        private void SetValue(int value, bool masterEventResponse = false) {
            _trackBar.Value = value;
            _textBox.Text = value.ToString(CultureInfo.InvariantCulture);
            _label.Text = value.ToPercentage() + @"%";
            if (!(masterEventResponse && !_cascadeMasterEvents)) {
                OnValueChanged();
            }
        }


        private void textBox_KeyPress(object sender, KeyPressEventArgs e) {
            if ((e.KeyChar >= (Char) Keys.D0 && e.KeyChar <= (Char) Keys.D9) || e.KeyChar == (char) Keys.Back) {
                return;
            }
            e.Handled = true;
        }


        private void trackBar_Scroll(object sender, EventArgs e) {
            SetValue(_trackBar.Value);
        }


        public bool AllowText {
            get { return _panelText.Visible; }
            set { _panelText.Visible = value; }
        }

        public bool CascadeMasterEvents {
            get { return _cascadeMasterEvents; }
            set { _cascadeMasterEvents = value; }
        }

        public ConsoleTrackBar Master {
            get { return _master; }
            set {
                if (_master != null) {
                    _master.ValueChanged = (ValueChangedHandler) Delegate.Remove(_master.ValueChanged, new ValueChangedHandler(MasterValueChanged));
                }
                _master = value;
                if (_master != null) {
                    _master.ValueChanged = (ValueChangedHandler) Delegate.Combine(_master.ValueChanged, new ValueChangedHandler(MasterValueChanged));
                }
            }
        }

        public int ResetIndex {
            get { return _resetIndex; }
            set {
                _resetIndex = value;
                if (SelectedTextIndex == _resetIndex) {
                    ResetAssignment();
                }
            }
        }

        public int SelectedTextIndex {
            get { return _selectedTextIndex; }
        }

        public Channel[] TextStrings {
            set {
                _channelNames = value;
                _panelText.Enabled = (_channelNames != null) && (_channelNames.Length > 0);
            }
        }

        public int Value {
            get { return _trackBar.Value; }
            set {
                if ((value >= _trackBar.Minimum) && (value <= _trackBar.Maximum)) {
                    SetValue(value);
                }
            }
        }

        public delegate void ValueChangedHandler(object sender);

        private void textBox_Leave(object sender, EventArgs e) {
            int enteredValue;
            if (int.TryParse(_textBox.Text, out enteredValue)) {
                SetValue(Math.Max(0, Math.Min(255, enteredValue)));
            }
            else {
                _textBox.Text = _trackBar.Value.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
