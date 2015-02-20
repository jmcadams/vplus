using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

using VixenPlus.Properties;

using VixenPlusCommon;
using VixenPlusCommon.Annotations;

namespace VixenPlus.Dialogs {
    public partial class PlugInsTab : UserControl {

        private readonly IExecutable _contextProfile;

        public PlugInsTab(IExecutable profile) {
            InitializeComponent();
            _contextProfile = profile;
            InitializePlugInTab();
        }

        #region Plugins tab - initial cut

        private List<Channel> _channels;
        private IExecutable _executableObject;
        private List<IHardwarePlugin> _sequencePlugins;
        private SetupData _setupData;
        private bool _internalUpdate;

        private const string PlugInColSetup = "colPlugInSetup";
        private const string PlugInColEnabled = "colPlugInEnabled";
        private const string PlugInColStartCh = "colPlugInStartChannel";
        private const string PlugInColEndCh = "colPlugInEndChannel";
        private const string PlugInColConfig = "colPlugInConfiguration";

        private const string PlugInAttrStartCh = "from";
        private const string PlugInAttrEndCh = "to";
        private const string PlugInAttrName = "name";
        private const string PlugInAttrEnabled = "enabled";
        private const string PlugInAttrId = "id";

        private const string DefaultConfig = "n/a";
        private const int DefaultChannel = 1;


        private void PluginListDialog() {
            var executableObject = _contextProfile;
            _setupData = executableObject.PlugInData;
            _executableObject = executableObject;

            Cursor = Cursors.WaitCursor;

            List<IHardwarePlugin> hardwarePlugins;
            try {
                hardwarePlugins = OutputPlugins.LoadPlugins();
                OutputPlugins.VerifyPlugIns(_executableObject);
            }
            finally {
                Cursor = Cursors.Default;
            }

            _channels = executableObject.Channels;
            _sequencePlugins = new List<IHardwarePlugin>();

            if (cbAvailablePlugIns.Items.Count > 0 || null == hardwarePlugins) {
                return;
            }

            cbAvailablePlugIns.Items.Clear();
            cbAvailablePlugIns.Items.Add("Please select a plug in to add");
            foreach (var plugin in hardwarePlugins) {
                cbAvailablePlugIns.Items.Add(plugin.Name);
            }
            cbAvailablePlugIns.SelectedIndex = 0;
        }


        // Used in Standard Sequence to get all mapped plugins - move?
        [UsedImplicitly]
        public IEnumerable<object> MappedPluginList {
            get {
                return (from XmlNode node in _setupData.GetAllPluginData()
                        let attributes = node.Attributes
                        where attributes != null
                        select
                            new object[] {
                            string.Format("{0} ({1}-{2})", attributes[PlugInAttrName].Value,
                                attributes[PlugInAttrStartCh].Value, attributes[PlugInAttrEndCh].Value),
                            bool.Parse(attributes[PlugInAttrEnabled].Value),
                            Convert.ToInt32(attributes[PlugInAttrId].Value)
                        }).ToArray();
            }
        }


        public void SavingInvoked() {
            if (_lastRow == NoRow) {
                return;
            }

            var lastPlugIn = GetPluginForIndex(_lastRow);
            lastPlugIn.GetSetup();
            lastPlugIn.CloseSetup();
            SetDirty();
        }


        private void buttonRemove_Click(object sender, EventArgs e) {
            RemoveSelectedPlugIn();
            SetPluginsTabButtons();
        }


        private void buttonUse_Click(object sender, EventArgs e) {
            UsePlugin();
            SetPluginsTabButtons();
        }

        private void InitializePlugin(IHardwarePlugin plugin, XmlNode setupNode) {
            var eventDrivenOutputPlugIn = plugin as IEventDrivenOutputPlugIn;
            if (eventDrivenOutputPlugIn != null) {
                eventDrivenOutputPlugIn.Initialize(_executableObject, _setupData, setupNode);
            }
        }


        private void InitializePlugInTab() {
            PluginListDialog();
            Cursor = Cursors.WaitCursor;
            try {
                _internalUpdate = true;
                _lastRow = NoRow;
                dgvPlugIns.Rows.Clear();
                foreach (XmlNode node in _setupData.GetAllPluginData()) {
                    var plugin = node.Attributes != null && (node.Attributes[PlugInAttrName] != null)
                        ? OutputPlugins.FindPlugin(node.Attributes[PlugInAttrName].Value, true)
                        : null;

                    if (plugin == null) {
                        continue;
                    }

                    InitializePlugin(plugin, node);
                    AddPlugInRow(node, plugin);
                }
                _internalUpdate = false;
            }
            finally {
                Cursor = Cursors.Default;
                _internalUpdate = true;
                dgvPlugIns.Focus();
                _internalUpdate = false;
                SetPluginsTabButtons();
            }
        }


        private static void ShowSetupError(Exception exception) {
            MessageBox.Show(Resources.PluginInitError + exception.Message, Vendor.ProductName, MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
        }


        private void RemoveSelectedPlugIn() {
            if (dgvPlugIns.SelectedCells.Count == 0) {
                return;
            }

            ClearSetup();

            var index = GetTagForRow(dgvPlugIns.SelectedCells[0].RowIndex);

            _setupData.RemovePlugInData(index.ToString(CultureInfo.InvariantCulture));
            _sequencePlugins.RemoveAt(index);
            SetDirty();

            _internalUpdate = true;
            dgvPlugIns.Rows.RemoveAt(index);
            foreach (DataGridViewRow row in dgvPlugIns.Rows) {
                var tag = row.Parse();
                if (tag > index) {
                    row.Tag = --tag;
                }
            }
            _lastRow = NoRow;
            _internalUpdate = false;

            if (dgvPlugIns.Rows.Count > 0 && null != dgvPlugIns.CurrentCell) {
                dgvPlugIns_RowEnter(null, new DataGridViewCellEventArgs(dgvPlugIns.CurrentCell.ColumnIndex, dgvPlugIns.CurrentCell.RowIndex)); 
            }
        }


        private void UsePlugin() {
            if (cbAvailablePlugIns.SelectedIndex <= 0) {
                return;
            }

            var plugIn = OutputPlugins.FindPlugin(cbAvailablePlugIns.SelectedItem.ToString(), true);
            var node = _setupData.CreatePlugInData(plugIn);
            Xml.SetAttribute(node, PlugInAttrStartCh, DefaultChannel.ToString(CultureInfo.InvariantCulture));
            Xml.SetAttribute(node, PlugInAttrEndCh, _channels.Count.ToString(CultureInfo.InvariantCulture));
            Cursor = Cursors.WaitCursor;
            try {
                InitializePlugin(plugIn, node);
            }
            catch (Exception exception) {
                MessageBox.Show(string.Format(Resources.PluginSetupErrorInvalidStatePossible, exception.Message),
                    Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally {
                Cursor = Cursors.Default;
            }
            AddPlugInRow(node, plugIn);
        }


        private void AddPlugInRow(XmlNode n, IHardwarePlugin p) {
            // ReSharper disable PossibleNullReferenceException
            dgvPlugIns.SuspendLayout();

            _internalUpdate = true;
            var index = dgvPlugIns.Rows.Count;

            var row =
                dgvPlugIns.Rows.Add(n.Attributes[PlugInAttrName].Value, n.Attributes[PlugInAttrEnabled].Value == bool.TrueString, n.Attributes[PlugInAttrStartCh].Value, n.Attributes[PlugInAttrEndCh].Value, DefaultConfig, p.SupportsLiveSetup() ? "Inline Setup" : "Setup...");
            // ReSharper restore PossibleNullReferenceException

            ((DataGridViewDisableButtonCell)dgvPlugIns.Rows[row].Cells[PlugInColSetup]).Visible =
                !p.SupportsLiveSetup();

            dgvPlugIns.Rows[row].Tag = index;
            _lastRow = index;
            _sequencePlugins.Add(p);
            _internalUpdate = false;
            UpdateRowConfig(index);
            SetDirty();

            dgvPlugIns.ResumeLayout();
        }


        private void SetDirty() {
            var profile = _contextProfile as Profile;
            if (profile != null ) {
                profile.IsDirty = true;
            }
            else {
                var eventSequence = _contextProfile as EventSequence;
                if (eventSequence != null) {
                    eventSequence.IsDirty = true;
                }
            }
        }


        private void dgvPlugIns_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex == -1 || e.ColumnIndex == -1) return;

            var cell = dgvPlugIns.Rows[e.RowIndex].Cells[e.ColumnIndex];
            var value = cell.Value.ToString();
            int channel;

            switch (cell.OwningColumn.Name) {
                case PlugInColEnabled:
                    var enabled = dgvPlugIns.Rows[e.RowIndex].Cells[PlugInColEnabled].Value.ToString();
                    SetPlugInData(e.RowIndex, PlugInAttrEnabled, enabled);
                    break;
                case PlugInColStartCh:
                    if (!int.TryParse(value, out channel) || channel < DefaultChannel) {
                        channel = DefaultChannel;
                        cell.Value = channel;
                    }
                    SetPlugInData(e.RowIndex, PlugInAttrStartCh, channel.ToString(CultureInfo.InvariantCulture));
                    break;
                case PlugInColEndCh:
                    if (!int.TryParse(value, out channel) || channel > _channels.Count) {
                        channel = _channels.Count;
                        cell.Value = channel;
                    }
                    SetPlugInData(e.RowIndex, PlugInAttrEndCh, channel.ToString(CultureInfo.InvariantCulture));
                    break;
            }
        }


        private void SetPlugInData(int index, string key, string value) {
            var node = _setupData.GetPlugInData(GetTagForRow(index).ToString(CultureInfo.InvariantCulture));
            Xml.SetAttribute(node, key, value);
        }


        private const int NoRow = -1;
        private int _lastRow = NoRow;


        private void dgvPlugIns_RowEnter(object sender, DataGridViewCellEventArgs e) {
            if (_internalUpdate || e.RowIndex == -1 || e.ColumnIndex == -1) {
                return;
            }

            ClearSetup();

            var plugIn = GetPluginForIndex(e.RowIndex);
            _lastRow = GetTagForRow(e.RowIndex);

            if (!plugIn.SupportsLiveSetup()) {
                return;
            }

            try {
                var setup = plugIn.Setup();
                if (null == setup) {
                    return;
                }

                pSetup.Controls.Add(setup);
                setup.Show();
            }
            catch (Exception exception) {
                ShowSetupError(exception);
            }
        }


        private void ClearSetup() {
            if (pSetup.Controls.Count <= 0) {
                return;
            }

            SavingInvoked();

            pSetup.Controls.Clear();
        }


        private void dgvPlugIns_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (_internalUpdate || e.RowIndex == -1 || e.ColumnIndex == -1) {
                return;
            }

            if (dgvPlugIns.Columns[e.ColumnIndex].Name != PlugInColSetup ||
                !((DataGridViewDisableButtonCell)dgvPlugIns.Rows[e.RowIndex].Cells[PlugInColSetup]).Visible) {
                UpdateRowConfig(e.RowIndex);
                return;
            }

            try {
                GetPluginForIndex(e.RowIndex).Setup();
                UpdateRowConfig(e.RowIndex);
            }
            catch (Exception exception) {
                ShowSetupError(exception);
            }
        }


        private void dgvPlugIns_RowLeave(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex == -1 || e.ColumnIndex == -1 || _internalUpdate) {
                return;
            }
            Debug.Print("Update {0:000}:{1:000}", e.RowIndex, e.ColumnIndex);
            UpdateRowConfig(e.RowIndex);
        }


        private IHardwarePlugin GetPluginForIndex(int index) {
            return _sequencePlugins[GetTagForRow(index)];
        }


        private int GetTagForRow(int index) {
            return dgvPlugIns.Rows[index].Parse();
        }


        private void UpdateRowConfig(int rowIndex) {
            var plugin = GetPluginForIndex(rowIndex);
            var val = plugin.HardwareMap ?? DefaultConfig;
            dgvPlugIns.Rows[rowIndex].Cells[PlugInColConfig].Value = val;
        }


        private void cbAvailablePlugIns_SelectedIndexChanged(object sender, EventArgs e) {
            SetPluginsTabButtons();
        }

        private void SetPluginsTabButtons() {
            btnRemovePlugIn.Enabled = dgvPlugIns.Rows.Count > 0;
            btnAddPlugIn.Enabled = cbAvailablePlugIns.SelectedIndex > 0;
        }

        private void DoPluginsKeys(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Delete:
                    buttonRemove_Click(null, null);
                    e.Handled = true;
                    break;
                case Keys.Home:
                    if (dgvPlugIns.Rows.Count > 0 && dgvPlugIns.SelectedCells.Count > 0) {
                        dgvPlugIns.CurrentCell = dgvPlugIns.Rows[0].Cells[dgvPlugIns.SelectedCells[0].ColumnIndex];
                        e.Handled = true;
                    }
                    break;
                case Keys.End:
                    if (dgvPlugIns.Rows.Count > 0 && dgvPlugIns.SelectedCells.Count > 0) {
                        dgvPlugIns.CurrentCell = dgvPlugIns.Rows[dgvPlugIns.Rows.Count - 1].Cells[dgvPlugIns.SelectedCells[0].ColumnIndex];
                        e.Handled = true;
                    }
                    break;
            }
        }

        #endregion

    }
}
