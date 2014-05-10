using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using Nutcracker.Models;

using VixenPlusCommon;

namespace Nutcracker {
    public partial class NutcrackerModelDialog : Form {
        private readonly Dictionary<string, NutcrackerModelBase> _modelCache = new Dictionary<string, NutcrackerModelBase>();
        private const string DefaultModel = "Tree";


        public NutcrackerModelDialog() {
            InitializeComponent();
            cbColorLayout.SelectedIndex = 0;
            LoadModelsTypes();
            PopulateModelTypeDropDown();
        }


        private void LoadModelsTypes() {
            foreach (var str in Directory.GetFiles(Paths.UIPluginPath, Vendor.All + Vendor.AppExtension)) {
                var assembly = Assembly.LoadFile(str);
                foreach (var type in assembly.GetExportedTypes().Where(t => t.BaseType != null && t.BaseType.Name == NutcrackerModelBase.TypeName)) {
                    var plugin = (NutcrackerModelBase) Activator.CreateInstance(type);
                    var key = plugin.EffectName;
                    if (!_modelCache.ContainsKey(key)) {
                        _modelCache[key] = plugin;
                    }
                }
            }
        }


        private void PopulateModelTypeDropDown() {
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
            panel1.Controls.Add(newControl);
            newControl.IsLtoR = rbLtoR.Checked;
            lblNotes.Text = newControl.Notes;
        }
    }
}
