using System;
using System.Windows.Forms;

using VixenPlus;


namespace VixenEditor {
    internal partial class ThinSelection : Form {
        private int _selectedIndex = -1;
        private readonly Channel[] _channels;


        public ThinSelection(Channel[] values) {
            InitializeComponent();
            _channels = values;
            foreach (var c in _channels) {
                listBox.Items.Add(c.Name);
            }
        }


        private void listBox_DoubleClick(object sender, EventArgs e) {
            if (SelectedIndex != -1) {
                DialogResult = DialogResult.OK;
            }
        }


        private void listBox_DrawItem(object sender, DrawItemEventArgs e) {
            var lb = sender as ListBox;
            if (lb == null) {
                return;
            }

            Channel.DrawItem(lb, e, _channels[e.Index]);
        }


        private void listBox_SelectedIndexChanged(object sender, EventArgs e) {
            _selectedIndex = listBox.SelectedIndex;
        }


        private void ThinSelection_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Escape:
                    DialogResult = DialogResult.Cancel;
                    break;
                default:
                    if ((e.KeyCode == Keys.Return) && (_selectedIndex != -1)) {
                        DialogResult = DialogResult.OK;
                    }
                    break;
            }
        }


        public int SelectedIndex {
            get { return _selectedIndex; }
        }
    }
}
