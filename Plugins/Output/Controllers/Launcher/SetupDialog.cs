using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Controllers.Launcher {
    public partial class SetupDialog : UserControl {
        private int _selectedIndex = -1;

        private const int ColumnMargin = 8;
        private const int FileDialogWidth = 40;
        private const int TopMargin = 2;

        private const int ColPath = 0;
        private const int ColParams = 1;
        private const int ColTrigger = 2;


        public SetupDialog(IEnumerable<string[]> programs) {
            InitializeComponent();
            foreach (var strArray in programs) {
                listViewPrograms.Items.Add(new ListViewItem(strArray));
            }
        }


        private void buttonAdd_Click(object sender, EventArgs e) {
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                listViewPrograms.Items.Add(new ListViewItem(new[] {openFileDialog.FileName, "", "100"}));
            }
        }


        private void buttonFileDialog_Click(object sender, EventArgs e) {
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                textBoxPath.Text = openFileDialog.FileName;
            }
        }


        private void buttonRemove_Click(object sender, EventArgs e) {
            listViewPrograms.Items.RemoveAt(_selectedIndex);
            _selectedIndex = -1;
            SelectIndex(-1);
        }


        private void listViewPrograms_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e) {
            textBoxPath.Width = (listViewPrograms.Columns[ColPath].Width - FileDialogWidth) + ColumnMargin;
            textBoxParameters.Width = listViewPrograms.Columns[ColParams].Width - ColumnMargin;
            textBoxTriggerValue.Width = listViewPrograms.Columns[ColTrigger].Width - ColumnMargin;
        }


        private void listViewPrograms_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e) {
            e.DrawDefault = true;
        }


        private void listViewPrograms_DrawItem(object sender, DrawListViewItemEventArgs e) {
            e.DrawDefault = true;
        }


        private void listViewPrograms_MouseDown(object sender, MouseEventArgs e) {
            var itemAt = listViewPrograms.GetItemAt(5, e.Y);
            if (itemAt != null) {
                SelectIndex(itemAt.Index);
            }
            else {
                SelectIndex(-1);
            }
        }


        private void listViewPrograms_SelectedIndexChanged(object sender, EventArgs e) {
            listViewPrograms.SelectedItems.Clear();
        }


        private void SelectIndex(int index) {
            if (_selectedIndex != -1) {
                listViewPrograms.Items[_selectedIndex].SubItems[ColPath].Text = textBoxPath.Text;
                listViewPrograms.Items[_selectedIndex].SubItems[ColParams].Text = textBoxParameters.Text;
                listViewPrograms.Items[_selectedIndex].SubItems[ColTrigger].Text = textBoxTriggerValue.Text;
            }

            _selectedIndex = index;

            if (index >= 0) {
                listViewPrograms.RedrawItems(index, index, false);
                var item = listViewPrograms.Items[_selectedIndex];

                buttonFileDialog.Top = item.Position.Y + listViewPrograms.Top;
                buttonFileDialog.Visible = true;

                textBoxPath.Top = TopMargin + buttonFileDialog.Top;
                textBoxPath.Width = ((listViewPrograms.Columns[ColPath].Width - FileDialogWidth) + ColumnMargin) +
                                    ColumnMargin/2;
                textBoxPath.Left = buttonFileDialog.Right;
                textBoxPath.Text = item.SubItems[ColPath].Text;
                textBoxPath.Visible = true;

                textBoxParameters.Top = textBoxPath.Top;
                textBoxParameters.Width = listViewPrograms.Columns[ColParams].Width - ColumnMargin;
                textBoxParameters.Left = (ColumnMargin + listViewPrograms.Columns[ColPath].Width) +
                                         listViewPrograms.Left;
                textBoxParameters.Text = item.SubItems[ColParams].Text;
                textBoxParameters.Visible = true;

                textBoxTriggerValue.Top = textBoxParameters.Top;
                textBoxTriggerValue.Width = listViewPrograms.Columns[ColTrigger].Width - ColumnMargin;
                textBoxTriggerValue.Left = ((ColumnMargin + listViewPrograms.Columns[ColPath].Width) +
                                            listViewPrograms.Columns[ColParams].Width) + listViewPrograms.Left;
                textBoxTriggerValue.Text = item.SubItems[ColTrigger].Text;
                textBoxTriggerValue.Visible = true;

                buttonRemove.Enabled = true;
            }
            else {
                buttonFileDialog.Visible = false;
                textBoxPath.Visible = false;
                textBoxParameters.Visible = false;
                textBoxTriggerValue.Visible = false;
                buttonRemove.Enabled = false;
            }
        }


        private void textBoxPath_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode != Keys.Return) {
                return;
            }
            SelectIndex(-1);
            e.Handled = true;
        }


        private void textBoxPath_Leave(object sender, EventArgs e) {
            SelectIndex(_selectedIndex);
        }


        public IEnumerable<string[]> Programs {
            get {
                var strArray = new string[listViewPrograms.Items.Count][];
                var index = 0;
                foreach (ListViewItem item in listViewPrograms.Items) {
                    strArray[index] = new[]
                    {item.SubItems[ColPath].Text, item.SubItems[ColParams].Text, item.SubItems[ColTrigger].Text};
                    index++;
                }
                return strArray;
            }
        }
    }
}
