using System;
using System.Runtime.CompilerServices;

namespace FMOD
{
    public delegate RESULT FILE_READCALLBACK(IntPtr handle, IntPtr buffer, uint sizebytes, ref uint bytesread, IntPtr userdata);
}

