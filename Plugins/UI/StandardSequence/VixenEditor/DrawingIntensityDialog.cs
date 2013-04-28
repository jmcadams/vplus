namespace VixenEditor {
    using System;
    using System.Windows.Forms;

    using VixenPlus;
    using CommonUtils;

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
                udLevel.Minimum = Utils.ToPercentage(sequence.MinimumLevel);
                udLevel.Maximum = Utils.ToPercentage(sequence.MaximumLevel);
                udLevel.Value = Utils.ToPercentage(currentLevel);
            }

            lblInfo.Text = string.Format("Current Settings:\nIntensity: {0}{3}\nMinimum Allowed: {1}{3}\nMaximum Allowed: {2}{3}",
                udLevel.Value, udLevel.Minimum, udLevel.Maximum, actualLevels ? "" : "%");
        }


        private void buttonReset_Click(object sender, EventArgs e) {
            udLevel.Value = udLevel.Maximum;
        }


        public byte SelectedIntensity {
            get { return (byte)(_actualLevels ? udLevel.Value : Utils.ToPercentage((int)udLevel.Value)); }
        }
    }
}
