namespace FMOD
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate RESULT FILE_READCALLBACK(IntPtr handle, IntPtr buffer, uint sizebytes, ref uint bytesread, IntPtr userdata);
}

