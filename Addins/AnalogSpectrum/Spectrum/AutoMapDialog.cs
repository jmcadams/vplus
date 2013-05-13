using System.Collections.Generic;
using System.Windows.Forms;

namespace Spectrum {
    internal partial class AutoMapDialog : Form {
        public AutoMapDialog(List<VixenPlus.Channel> channels, List<FrequencyBand> bands) {
            InitializeComponent();
            comboBoxStartChannel.Items.AddRange(channels.ToArray());
            comboBoxStartBand.Items.AddRange(bands.ToArray());
            comboBoxStartChannel.SelectedIndex = 0;
            comboBoxStartBand.SelectedIndex = 0;
        }

        public int StartBandIndex {
            get {
                return comboBoxStartBand.SelectedIndex;
            }
        }

        public int StartChannelIndex {
            get {
                return comboBoxStartChannel.SelectedIndex;
            }
        }
    }
}