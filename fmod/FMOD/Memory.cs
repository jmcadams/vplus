namespace FMOD
{
    using System;
    using System.Runtime.InteropServices;

    public class Memory
    {
        [DllImport("fmodex")]
        private static extern RESULT FMOD_Memory_GetStats(ref int currentalloced, ref int maxalloced);
        public static RESULT GetStats(ref int currentalloced, ref int maxalloced)
        {
            return FMOD_Memory_GetStats(ref currentalloced, ref maxalloced);
        }
    }
}

