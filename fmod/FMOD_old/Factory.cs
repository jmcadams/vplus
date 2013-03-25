namespace FMOD
{
    using System;
    using System.Runtime.InteropServices;

    public class Factory
    {
        [DllImport("fmodex", EntryPoint="FMOD_System_Create")]
        private static extern RESULT FMOD_System_Create_32(ref IntPtr system);
        [DllImport("fmodex64", EntryPoint="FMOD_System_Create")]
        private static extern RESULT FMOD_System_Create_64(ref IntPtr system);
        public static RESULT System_Create(ref _System system)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            _System system2 = null;
            if (VERSION.platform == Platform.X64)
            {
                oK = FMOD_System_Create_64(ref ptr);
            }
            else
            {
                oK = FMOD_System_Create_32(ref ptr);
            }
            if (oK == RESULT.OK)
            {
                system2 = new _System();
                system2.setRaw(ptr);
                system = system2;
            }
            return oK;
        }
    }
}

