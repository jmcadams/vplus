using System;
using System.Collections;
using System.Windows.Forms;

namespace VixenPlus
{
	internal class ListViewItemSorter : IComparer
	{
		private int _sortColumn;

		public int SortColumn
		{
			get { return _sortColumn; }
			set { _sortColumn = value; }
		}

		public System.Windows.Forms.SortOrder SortOrder { get; set; }

		public int Compare(object x, object y)
		{
			string text = ((ListViewItem) x).SubItems[_sortColumn].Text;
			string strB = ((ListViewItem) y).SubItems[_sortColumn].Text;
			return String.Compare(text, strB, StringComparison.OrdinalIgnoreCase);
		}
	}
}