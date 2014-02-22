using System;
using System.Windows.Forms;



using VixenPlus;

using VixenPlusCommon;

namespace VixenEditor {
    internal partial class DrawingIntensityDialog : Form {

        private readonly bool _actualLevels;


        public DrawingIntensityDialog(EventSequence sequence, byte currentLevel, bool actualLevels) {
            InitializeComponent();
            _actualLevels = actualLevels;

            if (actualLevels) {
                udLevel.Minimum = sequence.MinimumLevel;
                udLevel.Maximum = sequence.MaximumLevel;
                udLevel.Value = currentLevel;
            }
            else {
                udLevel.Minimum = sequence.MinimumLevel.ToPercentage();
                udLevel.Maximum = sequence.MaximumLevel.ToPercentage();
                udLevel.Value = currentLevel.ToPercentage();
            }

            lblInfo.Text = string.Format("Current Settings:\nIntensity: {0}{3}\nMinimum Allowed: {1}{3}\nMaximum Allowed: {2}{3}",
                udLevel.Value, udLevel.Minimum, udLevel.Maximum, actualLevels ? "" : "%");
        }


        private void buttonReset_Click(object sender, EventArgs e) {
            udLevel.Value = udLevel.Maximum;
        }


        public byte SelectedIntensity {
            get { return (byte)(_actualLevels ? udLevel.Value : ((int)udLevel.Value).ToValue()); }
        }
    }
}
