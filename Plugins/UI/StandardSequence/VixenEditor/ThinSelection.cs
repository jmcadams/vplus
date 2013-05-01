using System;
using System.Drawing;
using System.Windows.Forms;

using CommonUtils;

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
            e.DrawBackground();

            using (var backgroundBrush = new SolidBrush(_channels[e.Index].Color))
            using (var g = e.Graphics) {
                g.FillRectangle(backgroundBrush, e.Bounds);

                var contrastingBrush = Utils.GetTextColor(backgroundBrush.Color);
                g.DrawString(_channels[e.Index].Name, e.Font, contrastingBrush, listBox.GetItemRectangle(e.Index).Location);
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) {
                    g.DrawString("\u2714", e.Font, contrastingBrush, e.Bounds.Width - e.Bounds.Height, e.Bounds.Y);
                }
            }
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
