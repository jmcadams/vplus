using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VixenPlus {
    internal static class ListViewSortIcons
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private static extern IntPtr SendMessageLVCOLUMN(IntPtr hWnd, int msg, IntPtr wParam, ref Lvcolumn lPlvcolumn);

        public static void SetSortIcon(ListView listViewControl, int columnIndex, SortOrder order)
        {
            var hWnd = SendMessage(listViewControl.Handle, 0x101f, IntPtr.Zero, IntPtr.Zero);
            for (var i = 0; i <= (listViewControl.Columns.Count - 1); i++)
            {
                var wParam = new IntPtr(i);
                var lPlvcolumn = new Lvcolumn {mask = 4};
                SendMessageLVCOLUMN(hWnd, 0x120b, wParam, ref lPlvcolumn);
                if ((order != SortOrder.None) && (i == columnIndex))
                {
                    switch (order)
                    {
                        case SortOrder.Ascending:
                            lPlvcolumn.fmt &= -513;
                            lPlvcolumn.fmt |= 0x400;
                            goto Label_00DE;

                        case SortOrder.Descending:
                            lPlvcolumn.fmt &= -1025;
                            lPlvcolumn.fmt |= 0x200;
                            goto Label_00DE;
                    }
                }
                else
                {
                    lPlvcolumn.fmt &= -0x601;
                }
                Label_00DE:
                SendMessageLVCOLUMN(hWnd, 0x120c, wParam, ref lPlvcolumn);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Lvcolumn
        {
            public int mask;
            private readonly int cx;
            [MarshalAs(UnmanagedType.LPTStr)] private readonly string pszText;
            private readonly IntPtr hbm;
            private readonly int cchTextMax;
            public int fmt;
            private readonly int iSubItem;
            private readonly int iImage;
            private readonly int iOrder;
        }
    }
}