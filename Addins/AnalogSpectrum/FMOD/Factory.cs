namespace FMOD
{
    using System;
    using System.Runtime.InteropServices;

    public class Factory
    {
        [DllImport("fmodex.dll")]
        private static extern RESULT FMOD_System_Create(ref IntPtr system);
        public static RESULT System_Create(ref System system)
        {
            RESULT oK = RESULT.OK;
            IntPtr ptr = new IntPtr();
            System system2 = null;
            oK = FMOD_System_Create(ref ptr);
            if (oK == RESULT.OK)
            {
                system2 = new System();
                system2.setRaw(ptr);
                system = system2;
            }
            return oK;
        }
    }
}

