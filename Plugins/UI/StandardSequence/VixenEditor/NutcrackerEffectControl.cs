using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using VixenPlus;

namespace VixenEditor
{
    public partial class NutcrackerEffectControl: UserControl
    {
        private readonly Dictionary<string, INutcrackerEffect> _effectCache = new Dictionary<string, INutcrackerEffect>();

        public NutcrackerEffectControl()
        {
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
                result[count] = label1.BackColor;
                count++;
            }

            if (chkBoxPalette2.Checked) {
                result[count] = label2.BackColor;
                count++;
            }
            if (chkBoxPalette3.Checked) {
                result[count] = label3.BackColor;
                count++;
            }
            if (chkBoxPalette4.Checked) {
                result[count] = label4.BackColor;
                count++;
            }
            if (chkBoxPalette5.Checked) {
                result[count] = label5.BackColor;
                count++;
            }
            if (chkBoxPalette6.Checked) {
                result[count] = label6.BackColor;
            }

            return count > 0 ? result : new[] {Color.White};

        }

        private void LoadEffects() {
            foreach (var str in Directory.GetFiles(Paths.NutcrackerPath, "*.dll")) {
                var assembly = Assembly.LoadFile(str);
                foreach (var type in assembly.GetExportedTypes()) {
                    foreach (var type2 in type.GetInterfaces()) {
                        if (type2.Name != "INutcrackerEffect") {
                            continue;
                        }
                        var plugin = (INutcrackerEffect)Activator.CreateInstance(type);
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
            return cbEffects.SelectedIndex == 0 ? null : panel1.Controls[0];
        }

        private event EventHandler OnPaletteChange;
        private event EventHandler OnSpeedChange;
        private event EventHandler OnSubControlChange;

        private void cbEffects_SelectedIndexChanged(object sender, EventArgs e) {
            foreach (Control control in panel1.Controls) {
                panel1.Controls.Remove(control);
                ((INutcrackerEffect)control).OnControlChanged -= NutcrackerEffect_ControlChanged;
            }

            //OnEffectChanged(this, new EventArgs());

            if (cbEffects.SelectedIndex <= 0) {
                return;
            }

            panel1.Controls.Add((UserControl)_effectCache[cbEffects.SelectedItem.ToString()]);
            _effectCache[cbEffects.SelectedItem.ToString()].OnControlChanged += NutcrackerEffect_ControlChanged;
        }

        private void NutcrackerEffect_ControlChanged(object sender, EventArgs e) {
            lblSpeed.Text = ((INutcrackerEffect)sender).EffectName;
            //OnEffectChanged(sender, e);
        }
    }
}
