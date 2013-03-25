namespace FMOD
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate RESULT FILE_OPENCALLBACK(string name, int unicode, ref uint filesize, ref IntPtr handle, ref IntPtr userdata);
}

