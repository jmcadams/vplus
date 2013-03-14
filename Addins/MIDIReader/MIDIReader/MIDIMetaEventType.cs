namespace MIDIReader
{
    using System;

    internal enum MIDIMetaEventType
    {
        CopyrightNotice = 2,
        CuePoint = 7,
        EndOfTrack = 0x2f,
        InstrumentName = 4,
        KeySignature = 0x59,
        Lyrics = 5,
        Marker = 6,
        MIDIChannelPrefix = 0x20,
        SequenceNumber = 0,
        SequenceOrTrackName = 3,
        SequencerSpecific = 0x7f,
        SetTempo = 0x51,
        SMPTEOffset = 0x54,
        TextEvent = 1,
        TimeSignature = 0x58
    }
}

