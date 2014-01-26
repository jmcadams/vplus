using System;
using System.Globalization;
using System.Windows.Forms;

using VixenPlus;
using VixenPlus.Properties;

namespace Dialogs {
    public partial class SequenceSettingsDialog : Form {
        private readonly EventSequence _eventSequence;


        public SequenceSettingsDialog(EventSequence sequence) {
            InitializeComponent();
            _eventSequence = sequence;
            numericUpDownMinimum.Value = sequence.MinimumLevel;
            numericUpDownMaximum.Value = sequence.MaximumLevel;
            textBoxEventPeriodLength.Text = sequence.EventPeriod.ToString(CultureInfo.InvariantCulture);
        }


        private void buttonOK_Click(object sender, EventArgs e) {
            if (numericUpDownMinimum.Value >= numericUpDownMaximum.Value) {
                MessageBox.Show(Resources.MinLessThanMax, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                DialogResult = DialogResult.None;
            }
            else {
                _eventSequence.MinimumLevel = (byte) numericUpDownMinimum.Value;
                _eventSequence.MaximumLevel = (byte) numericUpDownMaximum.Value;
                Cursor = Cursors.WaitCursor;
                int num;
                if (int.TryParse(textBoxEventPeriodLength.Text, out num)) {
                    _eventSequence.EventPeriod = num;
                }
                Cursor = Cursors.Default;
            }
        }
    }
}
