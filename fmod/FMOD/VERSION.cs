namespace FMOD
{
    using System;
    using System.Runtime.InteropServices;

    public class VERSION
    {
        public const string dll = "fmodex";
        public const string dll32 = "fmodex";
        public const string dll64 = "fmodex64";
        public const int number = 0x42003;
        public static Platform platform = GetPlatform();
        internal const ushort PROCESSOR_ARCHITECTURE_AMD64 = 9;
        internal const ushort PROCESSOR_ARCHITECTURE_IA64 = 6;
        internal const ushort PROCESSOR_ARCHITECTURE_INTEL = 0;
        internal const ushort PROCESSOR_ARCHITECTURE_UNKNOWN = 0xffff;

        [DllImport("kernel32.dll")]
        internal static extern void GetNativeSystemInfo(ref SYSTEM_INFO lpSystemInfo);
        private static Platform GetPlatform()
        {
            SYSTEM_INFO lpSystemInfo = new SYSTEM_INFO();
            try
            {
                GetNativeSystemInfo(ref lpSystemInfo);
            }
            catch
            {
                return Platform.X86;
            }
            switch (lpSystemInfo.wProcessorArchitecture)
            {
                case 0:
                    return Platform.X86;

                case 9:
                    return Platform.X64;
            }
            return Platform.Unknown;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct SYSTEM_INFO
        {
            public ushort wProcessorArchitecture;
            public ushort wReserved;
            public uint dwPageSize;
            public IntPtr lpMinimumApplicationAddress;
            public IntPtr lpMaximumApplicationAddress;
            public UIntPtr dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public ushort wProcessorLevel;
            public ushort wProcessorRevision;
        }
    }
}

