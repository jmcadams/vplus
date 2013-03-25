namespace FMOD
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct DSP_PARAMETERDESC
    {
        public float min;
        public float max;
        public float defaultval;
        public string name;
        public string label;
        public string description;
    }
}

