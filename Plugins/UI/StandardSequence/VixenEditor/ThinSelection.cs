namespace VixenEditor {
	using System;
    using System.Drawing;
	using System.Windows.Forms;

	internal partial class ThinSelection : Form {
		private int _selectedIndex = -1;

		public ThinSelection(string[] values) {
			InitializeComponent();
			listBox.Items.AddRange(values);
		}

		private void listBox_DoubleClick(object sender, EventArgs e) {
			if (SelectedIndex != -1) {
				DialogResult = DialogResult.OK;
			}
		}

		private void listBox_DrawItem(object sender, DrawItemEventArgs e) {
			e.DrawBackground();
			e.Graphics.DrawString(listBox.Items[e.Index].ToString(), listBox.Font, ((e.State & DrawItemState.Selected) != DrawItemState.None) ? Brushes.White : Brushes.Black, e.Bounds.Left + 3, e.Bounds.Top + 2);
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
			get {
				return _selectedIndex;
			}
		}
	}
}