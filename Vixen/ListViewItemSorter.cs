using System;
using System.Collections;
using System.Windows.Forms;

internal class ListViewItemSorter : IComparer
{
    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    private int SortColumn { get; set; }

    //public System.Windows.Forms.SortOrder SortOrder { get; set; }

    public int Compare(object x, object y)
    {
        var text = ((ListViewItem) x).SubItems[SortColumn].Text;
        var strB = ((ListViewItem) y).SubItems[SortColumn].Text;
        return String.Compare(text, strB, StringComparison.OrdinalIgnoreCase);
    }
}