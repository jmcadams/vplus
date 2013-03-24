using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Vixen
{
	internal static class ListViewSortIcons
	{
		private const int HDF_LEFT = 0;
		private const int HDF_SORTDOWN = 0x200;
		private const int HDF_SORTUP = 0x400;
		private const int HDF_STRING = 0x4000;
		private const int HDI_FORMAT = 4;
		private const int HDM_GETITEM = 0x120b;
		private const int HDM_SETITEM = 0x120c;
		private const int LVM_GETHEADER = 0x101f;

		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		private static extern IntPtr SendMessageLVCOLUMN(IntPtr hWnd, int Msg, IntPtr wParam, ref LVCOLUMN lPLVCOLUMN);

		public static void SetSortIcon(ListView ListViewControl, int ColumnIndex, System.Windows.Forms.SortOrder Order)
		{
			IntPtr hWnd = SendMessage(ListViewControl.Handle, 0x101f, IntPtr.Zero, IntPtr.Zero);
			for (int i = 0; i <= (ListViewControl.Columns.Count - 1); i++)
			{
				var wParam = new IntPtr(i);
				var lPLVCOLUMN = new LVCOLUMN();
				lPLVCOLUMN.mask = 4;
				SendMessageLVCOLUMN(hWnd, 0x120b, wParam, ref lPLVCOLUMN);
				if ((Order != System.Windows.Forms.SortOrder.None) && (i == ColumnIndex))
				{
					switch (Order)
					{
						case System.Windows.Forms.SortOrder.Ascending:
							lPLVCOLUMN.fmt &= -513;
							lPLVCOLUMN.fmt |= 0x400;
							goto Label_00DE;

						case System.Windows.Forms.SortOrder.Descending:
							lPLVCOLUMN.fmt &= -1025;
							lPLVCOLUMN.fmt |= 0x200;
							goto Label_00DE;
					}
				}
				else
				{
					lPLVCOLUMN.fmt &= -1537;
				}
				Label_00DE:
				SendMessageLVCOLUMN(hWnd, 0x120c, wParam, ref lPLVCOLUMN);
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct LVCOLUMN
		{
			public int mask;
			public readonly int cx;
			[MarshalAs(UnmanagedType.LPTStr)] public readonly string pszText;
			public readonly IntPtr hbm;
			public readonly int cchTextMax;
			public int fmt;
			public readonly int iSubItem;
			public readonly int iImage;
			public readonly int iOrder;
		}
	}
}