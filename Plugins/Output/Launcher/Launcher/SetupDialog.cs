using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace Launcher {
	public partial class SetupDialog : Form {
		private int _selectedIndex = -1;

		public SetupDialog(string[][] programs) {
			this.InitializeComponent();
			foreach (string[] strArray in programs) {
				this.listViewPrograms.Items.Add(new ListViewItem(strArray));
			}
		}

		private void buttonAdd_Click(object sender, EventArgs e) {
			if (this.openFileDialog.ShowDialog() == DialogResult.OK) {
				this.listViewPrograms.Items.Add(new ListViewItem(new string[] { this.openFileDialog.FileName, "", "100" }));
			}
		}

		private void buttonFileDialog_Click(object sender, EventArgs e) {
			if (this.openFileDialog.ShowDialog() == DialogResult.OK) {
				this.textBoxPath.Text = this.openFileDialog.FileName;
			}
		}

		private void buttonRemove_Click(object sender, EventArgs e) {
			this.listViewPrograms.Items.RemoveAt(this._selectedIndex);
			this._selectedIndex = -1;
			this.SelectIndex(-1);
		}

		private void listViewPrograms_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e) {
			this.textBoxPath.Width = (this.listViewPrograms.Columns[0].Width - 40) + 8;
			this.textBoxParameters.Width = this.listViewPrograms.Columns[1].Width - 8;
			this.textBoxTriggerValue.Width = this.listViewPrograms.Columns[2].Width - 8;
		}

		private void listViewPrograms_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e) {
			e.DrawDefault = true;
		}

		private void listViewPrograms_DrawItem(object sender, DrawListViewItemEventArgs e) {
			e.DrawDefault = true;
		}

		private void listViewPrograms_MouseDown(object sender, MouseEventArgs e) {
			ListViewItem itemAt = this.listViewPrograms.GetItemAt(5, e.Y);
			if (itemAt != null) {
				this.SelectIndex(itemAt.Index);
			}
			else {
				this.SelectIndex(-1);
			}
		}

		private void listViewPrograms_SelectedIndexChanged(object sender, EventArgs e) {
			this.listViewPrograms.SelectedItems.Clear();
		}

		private void SelectIndex(int index) {
			if (this._selectedIndex != -1) {
				this.listViewPrograms.Items[this._selectedIndex].SubItems[0].Text = this.textBoxPath.Text;
				this.listViewPrograms.Items[this._selectedIndex].SubItems[1].Text = this.textBoxParameters.Text;
				this.listViewPrograms.Items[this._selectedIndex].SubItems[2].Text = this.textBoxTriggerValue.Text;
			}
			this._selectedIndex = index;
			if (index >= 0) {
				this.listViewPrograms.RedrawItems(index, index, false);
				ListViewItem item = this.listViewPrograms.Items[this._selectedIndex];
				this.buttonFileDialog.Top = item.Position.Y + this.listViewPrograms.Top;
				this.buttonFileDialog.Visible = true;
				this.textBoxPath.Top = 2 + this.buttonFileDialog.Top;
				this.textBoxPath.Width = ((this.listViewPrograms.Columns[0].Width - 40) + 8) + 4;
				this.textBoxPath.Left = this.buttonFileDialog.Right;
				this.textBoxPath.Text = item.SubItems[0].Text;
				this.textBoxPath.Visible = true;
				this.textBoxParameters.Top = this.textBoxPath.Top;
				this.textBoxParameters.Width = this.listViewPrograms.Columns[1].Width - 8;
				this.textBoxParameters.Left = (8 + this.listViewPrograms.Columns[0].Width) + this.listViewPrograms.Left;
				this.textBoxParameters.Text = item.SubItems[1].Text;
				this.textBoxParameters.Visible = true;
				this.textBoxTriggerValue.Top = this.textBoxParameters.Top;
				this.textBoxTriggerValue.Width = this.listViewPrograms.Columns[2].Width - 8;
				this.textBoxTriggerValue.Left = ((8 + this.listViewPrograms.Columns[0].Width) + this.listViewPrograms.Columns[1].Width) + this.listViewPrograms.Left;
				this.textBoxTriggerValue.Text = item.SubItems[2].Text;
				this.textBoxTriggerValue.Visible = true;
				this.buttonRemove.Enabled = true;
			}
			else {
				this.buttonFileDialog.Visible = false;
				this.textBoxPath.Visible = false;
				this.textBoxParameters.Visible = false;
				this.textBoxTriggerValue.Visible = false;
				this.buttonRemove.Enabled = false;
			}
		}

		private void textBoxPath_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Return) {
				this.SelectIndex(-1);
				e.Handled = true;
			}
		}

		private void textBoxPath_Leave(object sender, EventArgs e) {
			this.SelectIndex(this._selectedIndex);
		}

		public string[][] Programs {
			get {
				string[][] strArray = new string[this.listViewPrograms.Items.Count][];
				int index = 0;
				foreach (ListViewItem item in this.listViewPrograms.Items) {
					strArray[index] = new string[] { item.SubItems[0].Text, item.SubItems[1].Text, item.SubItems[2].Text };
					index++;
				}
				return strArray;
			}
		}
	}
}