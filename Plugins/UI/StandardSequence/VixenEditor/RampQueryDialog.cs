using System;
using System.Globalization;
using System.Windows.Forms;

using CommonControls;

using CommonUtils;

namespace VixenEditor {


    internal partial class RampQueryDialog : Form {
        private readonly bool _actualLevels;


        public RampQueryDialog(int minimum, int maximum, bool isDescending, bool actualLevels) {
            InitializeComponent();
            _actualLevels = actualLevels;
            numericUpDownStart.Minimum = numericUpDownEnd.Minimum = actualLevels ? minimum : minimum.ToPercentage();
            numericUpDownStart.Maximum = numericUpDownEnd.Maximum = actualLevels ? maximum : maximum.ToPercentage();
            numericUpDownStart.Value = isDescending ? numericUpDownEnd.Maximum : numericUpDownEnd.Minimum;
            numericUpDownEnd.Value = isDescending ? numericUpDownEnd.Minimum : numericUpDownEnd.Maximum;
        }


        private void numericUpDownEnd_Enter(object sender, EventArgs e) {
            numericUpDownEnd.Select(0, numericUpDownEnd.Value.ToString(CultureInfo.InvariantCulture).Length);
        }


        private void numericUpDownStart_Enter(object sender, EventArgs e) {
            numericUpDownStart.Select(0, numericUpDownStart.Value.ToString(CultureInfo.InvariantCulture).Length);
        }


        public int EndingLevel {
            get { return (int) (_actualLevels ? numericUpDownEnd.Value : ((int) numericUpDownEnd.Value).ToValue()); }
        }

        public int StartingLevel {
            get { return (int)(_actualLevels ? numericUpDownStart.Value : ((int)numericUpDownStart.Value).ToValue()); }
        }
    }
}
