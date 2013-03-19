using System;
using System.Runtime.CompilerServices;

namespace FMOD
{
    public delegate RESULT CHANNEL_CALLBACK(IntPtr channelraw, CHANNEL_CALLBACKTYPE type, int command, uint commanddata1, uint commanddata2);
}

