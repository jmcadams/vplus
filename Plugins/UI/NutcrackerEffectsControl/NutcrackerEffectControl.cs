using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using CommonUtils;

using VixenPlus;

namespace NutcrackerEffectsControl {
    public partial class NutcrackerEffectControl : UserControl {
        private readonly Dictionary<string, INutcrackerEffect> _effectCache = new Dictionary<string, INutcrackerEffect>();

        public delegate void ControlChangedHandler(object sender, EventArgs e);

        public event ControlChangedHandler ControlChanged;


        protected virtual void OnControlChanged(object sender, EventArgs e) {
            var handler = ControlChanged;
            if (handler != null) {
                handler(sender, e);
            }
        }


        public NutcrackerEffectControl() {
            InitializeComponent();
            LoadEffects();
            PopulateEffects();
        }


        public int GetSpeed() {
            return tbSpeed.Value;
        }


        public Color[] GetPalette() {
            var count = 0;
            if (chkBoxPalette1.Checked) count++;
            if (chkBoxPalette2.Checked) count++;
            if (chkBoxPalette3.Checked) count++;
            if (chkBoxPalette4.Checked) count++;
            if (chkBoxPalette5.Checked) count++;
            if (chkBoxPalette6.Checked) count++;

            var result = new Color[count];
            count = 0;

            if (chkBoxPalette1.Checked) {
                result[count] = palette1.BackColor;
                count++;
            }

            if (chkBoxPalette2.Checked) {
                result[count] = palette2.BackColor;
                count++;
            }
            if (chkBoxPalette3.Checked) {
                result[count] = palette3.BackColor;
                count++;
            }
            if (chkBoxPalette4.Checked) {
                result[count] = palette4.BackColor;
                count++;
            }
            if (chkBoxPalette5.Checked) {
                result[count] = palette5.BackColor;
                count++;
            }
            if (chkBoxPalette6.Checked) {
                result[count] = palette6.BackColor;
            }

            return count > 0 ? result : new[] {Color.White};

        }


        private void LoadEffects() {
            foreach (var str in Directory.GetFiles(Paths.NutcrackerEffectsPath, "*.dll")) {
                var assembly = Assembly.LoadFile(str);
                foreach (var type in assembly.GetExportedTypes()) {
                    foreach (var type2 in type.GetInterfaces()) {
                        if (type2.Name != "INutcrackerEffect") {
                            continue;
                        }
                        var plugin = (INutcrackerEffect) Activator.CreateInstance(type);
                        var key = plugin.EffectName;
                        if (!_effectCache.ContainsKey(key)) {
                            _effectCache[key] = plugin;
                        }
                    }
                }
            }
        }


        private void PopulateEffects() {
            cbEffects.Items.Add("None");
            foreach (var nutcrackerEffect in _effectCache) {
                cbEffects.Items.Add(nutcrackerEffect.Value.EffectName);
            }
            cbEffects.SelectedIndex = 0;
        }


        public Control GetCurrentEffectControl() {
            return cbEffects.SelectedIndex == 0 || panel1.Controls.Count < 1 ? null : panel1.Controls[0];
        }


        //private event EventHandler OnPaletteChange;
        //private event EventHandler OnSpeedChange;
        //private event EventHandler OnSubControlChange;

        private void cbEffects_SelectedIndexChanged(object sender, EventArgs e) {
            foreach (Control control in panel1.Controls) {
                panel1.Controls.Remove(control);
                ((INutcrackerEffect)control).OnControlChanged -= NutcrackerEffect_ControlChanged;
            }

            //OnEffectChanged(this, new EventArgs());

            if (cbEffects.SelectedIndex <= 0) {
                return;
            }

            var newControl = _effectCache[cbEffects.SelectedItem.ToString()];
            panel1.Controls.Add((UserControl) newControl);
            tbNotes.Text = newControl.Notes;
            UpdatePalette(newControl.UsesPalette);
            UpdateSpeed(newControl.UsesSpeed);
            _effectCache[cbEffects.SelectedItem.ToString()].OnControlChanged += NutcrackerEffect_ControlChanged;
            OnControlChanged(this, EventArgs.Empty);
        }

        private void UpdatePalette(bool isEnabled) {
            btnPalette.Enabled = isEnabled;
            chkBoxPalette1.Enabled = isEnabled;
            chkBoxPalette2.Enabled = isEnabled;
            chkBoxPalette3.Enabled = isEnabled;
            chkBoxPalette4.Enabled = isEnabled;
            chkBoxPalette5.Enabled = isEnabled;
            chkBoxPalette5.Enabled = isEnabled;
            palette1.Enabled = isEnabled;
            palette2.Enabled = isEnabled;
            palette3.Enabled = isEnabled;
            palette4.Enabled = isEnabled;
            palette5.Enabled = isEnabled;
            palette6.Enabled = isEnabled;
        }

        private void UpdateSpeed(bool isEnabled) {
            lblSpeed.Enabled = isEnabled;
            tbSpeed.Enabled = isEnabled;
        }

        private void NutcrackerEffect_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(sender, e);
        }


        private void palette_Click(object sender, EventArgs e) {
            using (var colorDialog = new ColorDialog {AllowFullOpen = true, AnyColor = true, FullOpen = true}) {
                colorDialog.CustomColors = Preference2.GetInstance().CustomColors;
                if (colorDialog.ShowDialog() != DialogResult.OK) {
                    return;
                }
                ((Label) sender).BackColor = colorDialog.Color;
                Preference2.GetInstance().CustomColors = colorDialog.CustomColors;
            }
        }


        public void SetPalette(Color[] colors) {
            palette1.BackColor = colors[0];
            palette1.ForeColor = Utils.GetForeColor(colors[0]);
            palette2.BackColor = colors[1];
            palette2.ForeColor = Utils.GetForeColor(colors[1]);
            palette3.BackColor = colors[2];
            palette3.ForeColor = Utils.GetForeColor(colors[2]);
            palette4.BackColor = colors[3];
            palette4.ForeColor = Utils.GetForeColor(colors[3]);
            palette5.BackColor = colors[4];
            palette5.ForeColor = Utils.GetForeColor(colors[4]);
            palette6.BackColor = colors[5];
            palette6.ForeColor = Utils.GetForeColor(colors[5]);
            OnControlChanged(null, null);
        }


        public string[] GetHtmlPalette() {
            return new[] {
                ToHtml(palette1.BackColor), ToHtml(palette2.BackColor), ToHtml(palette3.BackColor), ToHtml(palette4.BackColor),
                ToHtml(palette5.BackColor), ToHtml(palette6.BackColor)
            };
        }


        private static string ToHtml(Color color) {
            return string.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
        }


        private void btnPalette_Click(object sender, EventArgs e) {
            Action<Color[]> setPalette = SetPalette;
            Func<string[]> getPalette = GetHtmlPalette;
            using (var pm = new NutcrackerPaletteManager(setPalette, getPalette)) {
                pm.ShowDialog();
            }
        }
    }
}
