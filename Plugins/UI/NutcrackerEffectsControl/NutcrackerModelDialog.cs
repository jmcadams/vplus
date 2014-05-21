using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

using Nutcracker.Effects;
using Nutcracker.Models;

using VixenPlusCommon;

namespace Nutcracker {
    public partial class NutcrackerModelDialog : Form {
        private readonly Dictionary<string, NutcrackerModelBase> _modelCache = new Dictionary<string, NutcrackerModelBase>();
        private const string DefaultModel = "Tree";
        private string _modelPath;
        private XDocument _doc;

        public NutcrackerModelDialog(string path) {
            InitializeComponent();
            _modelPath = path;
            cbColorLayout.SelectedIndex = 0;
            LoadModelsTypes();
            PopulateModelTypeDropDown();
            if (!string.IsNullOrEmpty(_modelPath)) {
                LoadXml();
                lblPreviewAs.Visible = false;
                cbPreviewAs.Visible = false;
            }
            else {
                lblModelNameValue.Text = "none";
            }
        }


        public int Rows {
            get {
                var cache = _modelCache[cbPreviewAs.SelectedItem.ToString()];
                if (cache.Rows < 1) {
                    cache.InitializePreview(PreviewRectangle);
                }
                return cache.Rows;
            }
        }

        public int Cols {
            get {
                var cache = _modelCache[cbPreviewAs.SelectedItem.ToString()];
                if (cache.Cols < 1) {
                    cache.InitializePreview(PreviewRectangle);
                }
                return cache.Cols;
            }
        }

        public string ColorLayout {
            get { return cbColorLayout.SelectedItem.ToString(); }
        }

        private Rectangle _prevRect;

        public Rectangle PreviewRectangle {
            private get { return _prevRect.IsEmpty ? _prevRect = new Rectangle(0, 0, 100, 100) : _prevRect; }
            set { _prevRect = value; }
        }

        public NutcrackerNodes[,] Nodes {
            get {
                _modelCache[cbPreviewAs.SelectedItem.ToString()].InitializePreview(PreviewRectangle);
                return _modelCache[cbPreviewAs.SelectedItem.ToString()].Nodes;
            }
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


        public string ModelName {
            get {
                return string.IsNullOrEmpty(_modelPath) ? "none" : Path.GetFileNameWithoutExtension(_modelPath);
            }
        }

        private void LoadXml() {
            lblModelNameValue.Text = Path.GetFileNameWithoutExtension(_modelPath);

            _doc = XDocument.Load(_modelPath);
            var root = _doc.Element(NutcrackerModelBase.TypeName);
            if (null == root) {
                return;
            }

            cbPreviewAs.SelectedIndex = cbPreviewAs.FindStringExact(XmlConvert.DecodeName(NutcrackerModelBase.FindAttribute(root, "Type")));
            _modelCache[cbPreviewAs.SelectedItem.ToString()].Settings = _doc;

            var common = root.Element("ModelCommon");

            if (null == common) {
                return;
            }

            var lToR = bool.Parse(NutcrackerModelBase.FindAttribute(common, "LtoR"));
            _modelCache[cbPreviewAs.SelectedItem.ToString()].IsLtoR = lToR;
            rbLtoR.Checked = lToR;
            rbRtoL.Checked = !lToR;

            cbColorLayout.SelectedIndex = cbColorLayout.FindStringExact(NutcrackerModelBase.FindAttribute(common, "ColorOrder"));
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
            //newControl.Settings = _doc;
            lblNotes.Text = newControl.Notes;
        }

        private void btnOk_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(_modelPath)) {
                using (var modelName = new TextQueryDialog("Model Name", "What would you like to name this model", "")) {
                    var isDone = false;
                    do {
                        modelName.ShowDialog();
                        if (modelName.DialogResult != DialogResult.OK) {
                            return;
                        }
                        var tempModelPath = Path.Combine(Paths.NutcrackerDataPath, modelName.Response + Vendor.NutcrakerModelExtension);
                        if (File.Exists(tempModelPath)) {
                            if (
                                MessageBox.Show("That model exists, do you want to overwrite the existing model?", "Overwrite?",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) {
                                continue;
                            }
                            _modelPath = tempModelPath;
                            isDone = true;
                        }
                        else {
                            _modelPath = tempModelPath;
                            isDone = true;
                        }
                    } while (!isDone);
                }
            }

            SaveModel();
        }


        private void SaveModel() {
            var control = _modelCache[cbPreviewAs.SelectedItem.ToString()];
            
            var settings = control.Settings;
            var root = settings.Element(NutcrackerModelBase.TypeName);

            if (root == null) {
                throw new XmlException("Base settings not returned from model properly");
            }

            root.Add(new XElement("ModelCommon",
                        new XAttribute("LtoR", rbLtoR.Checked),
                        new XAttribute("ColorOrder", cbColorLayout.SelectedItem)
                    ));

            settings.Save(_modelPath);
        }
    }
}
