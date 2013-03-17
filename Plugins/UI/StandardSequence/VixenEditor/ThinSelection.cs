namespace VixenEditor {
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Windows.Forms;

	internal class ThinSelection : Form {
		private int m_selectedIndex = -1;

		public ThinSelection(string[] values) {
			this.InitializeComponent();
			this.listBox.Items.AddRange(values);
		}

		private void listBox_DoubleClick(object sender, EventArgs e) {
			if (this.SelectedIndex != -1) {
				base.DialogResult = DialogResult.OK;
			}
		}

		private void listBox_DrawItem(object sender, DrawItemEventArgs e) {
			e.DrawBackground();
			e.Graphics.DrawString(this.listBox.Items[e.Index].ToString(), this.listBox.Font, ((e.State & DrawItemState.Selected) != DrawItemState.None) ? Brushes.White : Brushes.Black, (float)(e.Bounds.Left + 3), (float)(e.Bounds.Top + 2));
		}

		private void listBox_SelectedIndexChanged(object sender, EventArgs e) {
			this.m_selectedIndex = this.listBox.SelectedIndex;
		}

		private void ThinSelection_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Escape) {
				base.DialogResult = DialogResult.Cancel;
			}
			else if ((e.KeyCode == Keys.Return) && (this.m_selectedIndex != -1)) {
				base.DialogResult = DialogResult.OK;
			}
		}

		public int SelectedIndex {
			get {
				return this.m_selectedIndex;
			}
		}
	}
}