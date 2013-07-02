using System;
using System.Drawing;
using System.Windows.Forms;

using VixenPlus.Dialogs;

namespace NutcrackerEffectsControl {
    public partial class NutcrackerPaletteManager : Form {

        private readonly Action<Color[]> _setPalette;
        private readonly Func<string[]> _getPalette; 
        private readonly NutcrackerXmlManager _nutcrackerData;

        public NutcrackerPaletteManager(Action<Color[]> setPalette, Func<string[]> getPalette ) {
            InitializeComponent();
            _setPalette = setPalette;
            _getPalette = getPalette;
            _nutcrackerData = new NutcrackerXmlManager();
            PopulatePalettes();
            EnableButtons();
        }


        private void PopulatePalettes() {
            lbPalettes.Items.Clear();
            foreach (var pal in _nutcrackerData.GetPalettes()) {
                lbPalettes.Items.Add(pal);
            }
        }


        private void Save_Click(object sender, EventArgs e) {
            using (var getName = new TextQueryDialog("Palette Name", "What do you want to name the palette?", string.Empty)) {
                if (getName.ShowDialog() == DialogResult.OK) {
                    _nutcrackerData.SavePalette(getName.Response, _getPalette.Invoke());
                    PopulatePalettes();
                }
            }
        }

        private void Load_Click(object sender, EventArgs e) {
            if (lbPalettes.SelectedIndex < 0) return;

            _setPalette(_nutcrackerData.GetColors(lbPalettes.SelectedItem.ToString()));
        }

        private void Delete_Click(object sender, EventArgs e) {
            if (lbPalettes.SelectedIndex < 0 || lbPalettes.SelectedItem.ToString() == "default") return;

            _nutcrackerData.RemovePalette(lbPalettes.SelectedItem.ToString());
            PopulatePalettes();
        }

        private void EnableButtons() {
            btnLoad1.Enabled = lbPalettes.SelectedIndex >= 0;
            btnDeletePalette.Enabled = lbPalettes.SelectedIndex >= 0 && lbPalettes.SelectedItem.ToString() != "default";
        }

        private void lbPalettes_SelectedIndexChanged(object sender, EventArgs e) {
            EnableButtons();
        }
    }
}
