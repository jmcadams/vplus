namespace Spectrum {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal partial class AutoMapDialog : Form {

        public AutoMapDialog(List<VixenPlus.Channel> channels, List<FrequencyBand> bands, int initialStartChannelIndex, int initialStartBandIndex) {
            this.InitializeComponent();
            this.comboBoxStartChannel.Items.AddRange(channels.ToArray());
            this.comboBoxStartBand.Items.AddRange(bands.ToArray());
            this.comboBoxStartChannel.SelectedIndex = Math.Min(initialStartChannelIndex, this.comboBoxStartChannel.Items.Count - 1);
            this.comboBoxStartBand.SelectedIndex = Math.Min(initialStartBandIndex, this.comboBoxStartBand.Items.Count - 1);
        }

        public int StartBandIndex {
            get {
                return this.comboBoxStartBand.SelectedIndex;
            }
        }

        public int StartChannelIndex {
            get {
                return this.comboBoxStartChannel.SelectedIndex;
            }
        }
    }
}