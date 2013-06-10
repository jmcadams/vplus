using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using VixenPlus;

namespace VixenEditor
{
    public partial class NutcrackerEffectControl: UserControl
    {
        private static readonly Dictionary<string, INutcrackerEffect> EffectCache = new Dictionary<string, INutcrackerEffect>();

        public NutcrackerEffectControl()
        {
            InitializeComponent();
            // These lines need to be commented out to view in the designer - don't know how to defeat that behavior
            LoadEffects();
            PopulateEffects();
            // End commenting
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
                        if (!EffectCache.ContainsKey(key)) {
                            EffectCache[key] = plugin;
                        }
                    }
                }
            }
        }


        private void PopulateEffects() {
            foreach (var nutcrackerEffect in EffectCache) {
                cbEffects.Items.Add(nutcrackerEffect.Value.EffectName);
            }
        }


        private void cbEffects_SelectedIndexChanged(object sender, EventArgs e) {
            foreach (Control control in panel1.Controls) {
                panel1.Controls.Remove(control);
            }

            if (cbEffects.SelectedIndex == -1) {
                return;
            }

            panel1.Controls.Add((UserControl) EffectCache[cbEffects.SelectedItem.ToString()]);
        }
    }
}
