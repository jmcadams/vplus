using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using CommonControls;

using VixenPlus;

namespace NutcrackerEffectsControl {
    public sealed partial class NutcrackerEffectControl : UserControl {
        private readonly Dictionary<string, INutcrackerEffect> _effectCache = new Dictionary<string, INutcrackerEffect>();

        public delegate void ControlChangedHandler(object sender, EventArgs e);

        public event ControlChangedHandler ControlChanged;


        private void OnControlChanged(object sender, EventArgs e) {
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

        public int Speed {
            get { return tbSpeed.Value; }
            set { tbSpeed.Value = value; }
        }

        private void LoadEffects() {
            foreach (var str in Directory.GetFiles(Paths.NutcrackerEffectsPath, Vendor.All + Vendor.AppExtension)) {
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

        public void SetEffect(string effectName, List<string> setupData, bool isFirst) {
            setupData.Insert(0, isFirst ? "1" : "2");

            SetPaletteAndSpeed(setupData);

            var newEffectItem = cbEffects.Items.IndexOf(effectName);
            if (newEffectItem >= 0) {
                cbEffects.SelectedIndex = newEffectItem;
                if (newEffectItem != 0) {
                    _effectCache[cbEffects.SelectedItem.ToString()].Settings = setupData;
                }
            }

            setupData.RemoveAt(0);
        }

        public string GetEffect(bool isFirst) {
            var effectSettings = new StringBuilder();
            foreach (var e in _effectCache[cbEffects.SelectedItem.ToString()].Settings) {
                effectSettings.Append(e.Replace("{0}", isFirst ? "1" : "2")).Append(",");
            }
            effectSettings.Remove(effectSettings.Length - 1, 1);

            return effectSettings.ToString();
        }


        public string GetPaletteSettings(bool isFirst) {
            var colors = new Color[6];
            colors[0] = palette1.BackColor;
            colors[1] = palette2.BackColor;
            colors[2] = palette3.BackColor;
            colors[3] = palette4.BackColor;
            colors[4] = palette5.BackColor;
            colors[5] = palette6.BackColor;

            var checkBox = new bool[6];
            checkBox[0] = chkBoxPalette1.Checked;
            checkBox[1] = chkBoxPalette2.Checked;
            checkBox[2] = chkBoxPalette3.Checked;
            checkBox[3] = chkBoxPalette4.Checked;
            checkBox[4] = chkBoxPalette5.Checked;
            checkBox[5] = chkBoxPalette6.Checked;

            const string templ = "ID_CHECKBOX_Palette{0}_{1}={2},ID_BUTTON_Palette{0}_{1}={3}";
            // ID_CHECKBOX_Palette1_1=1,ID_BUTTON_Palette1_1=#FF0000

            var pal = new StringBuilder();
            for (var i = 0; i < 6; i++) {
                pal.Append(string.Format(templ, isFirst ? "1" : "2", i + 1, checkBox[i] ? "1" : "0", ToHtml(colors[i]))).Append(",");
            }
            pal.Remove(pal.Length - 1, 1);

            return pal.ToString();
        }

        public string GetSpeed(bool isFirst) {
            return "ID_SLIDER_Speed" + (isFirst ? "1" : "2") + "=" + tbSpeed.Value;
        }


        private void SetPaletteAndSpeed(IList<string> setupData) {
            var panelNum = setupData[0];
            
            var chkBoxPrefix = "ID_CHECKBOX_Palette" + panelNum;
            var paletteChecked = new bool[6];

            var colorPrefix = "ID_BUTTON_Palette" + panelNum;
            var paletteColor = new Color[6];

            var speedPrefix = "ID_SLIDER_Speed" + panelNum;

            foreach (var s in setupData) {
                var keyValue = s.Split(new[] {'='});
                if (s.StartsWith(chkBoxPrefix)) {
                    var index = keyValue[0].Substring(keyValue[0].Length - 1, 1).ToInt() - 1;
                    if (index >= 0) {
                        paletteChecked[index] = keyValue[1].Equals("1");
                    }
                }
                else if (s.StartsWith(colorPrefix)) {
                    var index = keyValue[0].Substring(keyValue[0].Length - 1, 1).ToInt() - 1;
                    if (index >= 0) {
                        paletteColor[index] = ColorTranslator.FromHtml(keyValue[1]);
                    }
                }
                else if (s.StartsWith(speedPrefix)) {
                    tbSpeed.Value = keyValue[1].ToInt();
                }
            }

            chkBoxPalette1.Checked = paletteChecked[0];
            chkBoxPalette2.Checked = paletteChecked[1];
            chkBoxPalette3.Checked = paletteChecked[2];
            chkBoxPalette4.Checked = paletteChecked[3];
            chkBoxPalette5.Checked = paletteChecked[4];
            chkBoxPalette6.Checked = paletteChecked[5];

            SetPalette(paletteColor);
        }


        public Control GetCurrentEffectControl() {
            return cbEffects.SelectedIndex == 0 || panel1.Controls.Count < 1 ? null : panel1.Controls[0];
        }

        private void cbEffects_SelectedIndexChanged(object sender, EventArgs e) {
            foreach (Control control in panel1.Controls) {
                panel1.Controls.Remove(control);
                ((INutcrackerEffect)control).OnControlChanged -= NutcrackerEffect_ControlChanged;
            }

            if (cbEffects.SelectedIndex <= 0) {
                return;
            }

            var newControl = _effectCache[cbEffects.SelectedItem.ToString()];
            panel1.Controls.Add((UserControl) newControl);
            tbNotes.Text = newControl.Notes;
            SetPaletteEnabled(newControl.UsesPalette);
            SetSpeedEnabled(newControl.UsesSpeed);
            _effectCache[cbEffects.SelectedItem.ToString()].OnControlChanged += NutcrackerEffect_ControlChanged;
            
            OnControlChanged(this, EventArgs.Empty);
        }

        private void SetPaletteEnabled(bool isEnabled) {
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

        private void SetSpeedEnabled(bool isEnabled) {
            lblSpeed.Enabled = isEnabled;
            tbSpeed.Enabled = isEnabled;
        }

        private void NutcrackerEffect_ControlChanged(object sender, EventArgs e) {
            OnControlChanged(sender, e);
        }

        private void palette_Click(object sender, EventArgs e) {
            using (var colorDialog = new System.Windows.Forms.ColorDialog {AllowFullOpen = true, AnyColor = true, FullOpen = true}) {
                colorDialog.CustomColors = Preference2.GetInstance().CustomColors;
                if (colorDialog.ShowDialog() != DialogResult.OK) {
                    return;
                }
                ((Label) sender).BackColor = colorDialog.Color;
                Preference2.GetInstance().CustomColors = colorDialog.CustomColors;
            }
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

            return count > 0 ? result : new[] { Color.White };

        }


        private void SetPalette(Color[] colors) {
            palette1.BackColor = colors[0];
            palette1.ForeColor = colors[0].GetForeColor();
            palette2.BackColor = colors[1];
            palette2.ForeColor = colors[1].GetForeColor();
            palette3.BackColor = colors[2];
            palette3.ForeColor = colors[2].GetForeColor();
            palette4.BackColor = colors[3];
            palette4.ForeColor = colors[3].GetForeColor();
            palette5.BackColor = colors[4];
            palette5.ForeColor = colors[4].GetForeColor();
            palette6.BackColor = colors[5];
            palette6.ForeColor = colors[5].GetForeColor();
            OnControlChanged(null, null);
        }


        private string[] GetHtmlPalette() {
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
