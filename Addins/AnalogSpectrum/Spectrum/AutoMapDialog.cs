using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Spectrum {
	internal partial class AutoMapDialog : Form {
		public AutoMapDialog(List<Vixen.Channel> channels, List<FrequencyBand> bands) {
			this.InitializeComponent();
			this.comboBoxStartChannel.Items.AddRange(new object[] {channels.ToArray()});
			this.comboBoxStartBand.Items.AddRange(new object[] {bands.ToArray()});
			this.comboBoxStartChannel.SelectedIndex = 0;
			this.comboBoxStartBand.SelectedIndex = 0;
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