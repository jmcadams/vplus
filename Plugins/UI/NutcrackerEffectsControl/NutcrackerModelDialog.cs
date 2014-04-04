using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using NutcrackerEffects.Models;

using VixenPlus;

using VixenPlusCommon;

namespace Nutcracker {
    public partial class NutcrackerModelDialog : Form {
        private readonly EventSequence _sequence;
        private int _lastGroupSelected;
        private int _lastChannelSelected;
        private readonly Dictionary<string, INutcrackerModel> _modelCache = new Dictionary<string, INutcrackerModel>();
        private readonly bool _useCheckmark = Preference2.GetInstance().GetBoolean("UseCheckmark");
        private const string DefaultModel = "Tree";

        public NutcrackerModelDialog(EventSequence sequence) {
            InitializeComponent();
            cbColorLayout.SelectedIndex = 0;
            _sequence = sequence;
            LoadGroups();
            LoadModels();
            PopulateModels();
        }


        private void LoadGroups() {
            if (_sequence.Groups != null) {
                cbGroups.Items.Clear();
                foreach (var g in _sequence.Groups) {
                    cbGroups.Items.Add(g.Key);
                }
                cbGroups.SelectedIndex = _lastGroupSelected;
            }
            else {
                chkBoxUseGroup.Checked = false;
                chkBoxUseGroup.Enabled = false;
                LoadChannels();
            }
        }


        private void LoadChannels() {
            cbGroups.Items.Clear();
            foreach (var c in _sequence.FullChannels) {
                cbGroups.Items.Add(c);
            }
            cbGroups.SelectedIndex = _lastChannelSelected;
        }

        private void chkBoxUseGroup_CheckedChanged(object sender, EventArgs e) {
            if (chkBoxUseGroup.Checked) {
                LoadGroups();
            }
            else {
                LoadChannels();
            }
        }


        private void cbGroups_SelectedIndexChanged(object sender, EventArgs e) {
            if (chkBoxUseGroup.Checked) {
                _lastGroupSelected = cbGroups.SelectedIndex;
            }
            else {
                _lastChannelSelected = cbGroups.SelectedIndex;
            }
        }


        private void cbGroups_DrawItem(object sender, DrawItemEventArgs e) {
            if (e.Index < 0) return;


            if (chkBoxUseGroup.Checked) {
                var indexedItem = _sequence.Groups[cbGroups.Items[e.Index].ToString()];
                e.DrawItem(indexedItem.Name, indexedItem.GroupColor, _useCheckmark);
            }
            else {
                var indexedItem = _sequence.FullChannels[e.Index];
                e.DrawItem(indexedItem.Name, indexedItem.Color, _useCheckmark);
            }
        }

        private void LoadModels() {
            foreach (var str in Directory.GetFiles(Paths.UIPluginPath, Vendor.All + Vendor.AppExtension)) {
                var assembly = Assembly.LoadFile(str);
                foreach (var type in assembly.GetExportedTypes()) {
                    foreach (var type2 in type.GetInterfaces()) {
                        if (type2.Name != "INutcrackerModel") {
                            continue;
                        }
                        var plugin = (INutcrackerModel)Activator.CreateInstance(type);
                        var key = plugin.EffectName;
                        if (!_modelCache.ContainsKey(key)) {
                            _modelCache[key] = plugin;
                        }
                    }
                }
            }
        }


        private void PopulateModels() {
            foreach (var nutcrackerEffect in _modelCache) {
                cbPreviewAs.Items.Add(nutcrackerEffect.Value.EffectName);
            }
            cbPreviewAs.SelectedIndex = cbPreviewAs.Items.Contains(DefaultModel) ? cbPreviewAs.Items.IndexOf(DefaultModel) : 0;
        }


        private void cbPreviewAs_SelectedIndexChanged(object sender, EventArgs e) {
            foreach (Control control in panel1.Controls) {
                panel1.Controls.Remove(control);
            }

            if (cbPreviewAs.SelectedIndex < 0) {
                return;
            }

            var newControl = _modelCache[cbPreviewAs.SelectedItem.ToString()];
            panel1.Controls.Add((UserControl)newControl);
            newControl.IsLtoR = rbLtoR.Checked;
            lblNotes.Text = newControl.Notes;
        }
    }
}
