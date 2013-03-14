namespace K8055
{
    using System;
    using System.Runtime.InteropServices;

    internal static class K8055D
    {
        [DllImport("K8055D")]
        public static extern void ClearAnalogChannel(long channel);
        [DllImport("K8055D")]
        public static extern void ClearDigitalChannel(long channel);
        [DllImport("K8055D")]
        public static extern void CloseDevice();
        [DllImport("K8055D")]
        public static extern long OpenDevice(long address);
        [DllImport("K8055D")]
        public static extern void OutputAnalogChannel(long channel, long data);
        [DllImport("K8055D")]
        public static extern long ReadAllDigital();
        [DllImport("K8055D")]
        public static extern long SearchDevices();
        [DllImport("K8055D")]
        public static extern void SetAnalogChannel(long channel);
        [DllImport("K8055D")]
        public static extern long SetCurrentDevice(long address);
        [DllImport("K8055D")]
        public static extern void SetDigitalChannel(long channel);
        [DllImport("K8055D")]
        public static extern void Version();
        [DllImport("K8055D")]
        public static extern void WriteAllDigital(long data);
    }
}

