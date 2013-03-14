namespace FMOD
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct DSP_STATE
    {
        public IntPtr instance;
        public IntPtr plugindata;
    }
}

