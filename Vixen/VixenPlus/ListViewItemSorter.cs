using System.Collections;
using System.Windows.Forms;

namespace VixenPlus
{
	internal class ListViewItemSorter : IComparer
	{
		private int m_sortColumn;

		public int SortColumn
		{
			get { return m_sortColumn; }
			set { m_sortColumn = value; }
		}

		public System.Windows.Forms.SortOrder SortOrder { get; set; }

		public int Compare(object x, object y)
		{
			string text = ((ListViewItem) x).SubItems[m_sortColumn].Text;
			string strB = ((ListViewItem) y).SubItems[m_sortColumn].Text;
			return string.Compare(text, strB, true);
		}
	}
}