namespace Vixen
{
    using System;
    using System.Collections;
    using System.Windows.Forms;

    internal class ListViewItemSorter : IComparer
    {
        private int m_sortColumn = 0;
        private System.Windows.Forms.SortOrder m_sortOrder;

        public int Compare(object x, object y)
        {
            string text = ((ListViewItem) x).SubItems[this.m_sortColumn].Text;
            string strB = ((ListViewItem) y).SubItems[this.m_sortColumn].Text;
            return string.Compare(text, strB, true);
        }

        public int SortColumn
        {
            get
            {
                return this.m_sortColumn;
            }
            set
            {
                this.m_sortColumn = value;
            }
        }

        public System.Windows.Forms.SortOrder SortOrder
        {
            get
            {
                return this.m_sortOrder;
            }
            set
            {
                this.m_sortOrder = value;
            }
        }
    }
}

