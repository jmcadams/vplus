namespace FMOD
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate RESULT SOUND_PCMSETPOSCALLBACK(IntPtr soundraw, int subsound, uint position, TIMEUNIT postype);
}

