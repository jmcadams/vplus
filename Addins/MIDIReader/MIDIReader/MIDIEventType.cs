namespace MIDIReader
{
    using System;

    internal enum MIDIEventType
    {
        AuthorizationSysExEvent = 0xf7,
        ChannelAftertouch = 13,
        Controller = 11,
        MetaEvent = 0xff,
        NoteAftertouch = 10,
        NoteOff = 8,
        NoteOn = 9,
        PitchBend = 14,
        ProgramChange = 12,
        SysExEvent = 240
    }
}

