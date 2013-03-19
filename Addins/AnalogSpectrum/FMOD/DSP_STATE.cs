using System;
using System.Runtime.InteropServices;

namespace FMOD
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DSP_STATE
    {
        public IntPtr instance;
        public IntPtr plugindata;
    }
}

