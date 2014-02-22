using System;
using System.Globalization;
using System.Windows.Forms;



using VixenPlus.Properties;

using VixenPlusCommon;

namespace VixenEditor {

    //TODO I would like to add <=, >=, == & between to this.
    public partial class FindAndReplaceDialog : Form {

        public FindAndReplaceDialog(byte minimum, byte maximum, bool actualLevels, string whereToSearch) {
            InitializeComponent();
            lblHeading.Text = Resources.Replace_selected_values_in_the_ + whereToSearch + @".";
            udFind.Minimum = udReplace.Minimum = actualLevels ? minimum : minimum.ToPercentage();
            udFind.Maximum = udReplace.Maximum = actualLevels ? maximum : maximum.ToPercentage();
        }


        private void numericUpDownFind_Enter(object sender, EventArgs e) {
            udFind.Select(0, udFind.Value.ToString(CultureInfo.InvariantCulture).Length);
        }


        private void numericUpDownReplaceWith_Enter(object sender, EventArgs e) {
            udReplace.Select(0, udReplace.Value.ToString(CultureInfo.InvariantCulture).Length);
        }


        public byte FindValue {
            get { return (byte) udFind.Value; }
        }

        public byte ReplaceWithValue {
            get { return (byte) udReplace.Value; }
        }
    }
}
