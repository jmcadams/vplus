namespace FMOD
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct CDTOC
    {
        public int numtracks;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=100)]
        public int[] min;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=100)]
        public int[] sec;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst=100)]
        public int[] frame;
    }
}

