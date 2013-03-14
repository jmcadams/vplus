namespace LedTriks
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public static class Win32
    {
        [DllImport("Gdi32.dll", CharSet=CharSet.Unicode)]
        private static extern bool DeleteObject(IntPtr hdc);
        public static TEXTMETRIC GetTextMetrics(Graphics graphics, Font font)
        {
            TEXTMETRIC textmetric;
            IntPtr hdc = graphics.GetHdc();
            IntPtr hgdiobj = font.ToHfont();
            try
            {
                IntPtr ptr3 = SelectObject(hdc, hgdiobj);
                bool textMetrics = GetTextMetrics(hdc, out textmetric);
                SelectObject(hdc, ptr3);
            }
            finally
            {
                DeleteObject(hgdiobj);
                graphics.ReleaseHdc(hdc);
            }
            return textmetric;
        }

        [DllImport("Gdi32.dll", CharSet=CharSet.Unicode)]
        private static extern bool GetTextMetrics(IntPtr hdc, out TEXTMETRIC lptm);
        [DllImport("Gdi32.dll", CharSet=CharSet.Unicode)]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [Serializable, StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
        public struct TEXTMETRIC
        {
            public int tmHeight;
            public int tmAscent;
            public int tmDescent;
            public int tmInternalLeading;
            public int tmExternalLeading;
            public int tmAveCharWidth;
            public int tmMaxCharWidth;
            public int tmWeight;
            public int tmOverhang;
            public int tmDigitizedAspectX;
            public int tmDigitizedAspectY;
            public char tmFirstChar;
            public char tmLastChar;
            public char tmDefaultChar;
            public char tmBreakChar;
            public byte tmItalic;
            public byte tmUnderlined;
            public byte tmStruckOut;
            public byte tmPitchAndFamily;
            public byte tmCharSet;
        }
    }
}

