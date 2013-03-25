namespace FMOD
{
    using System;

    public enum TIMEUNIT
    {
        BUFFERED = 0x10000000,
        MODORDER = 0x100,
        MODPATTERN = 0x400,
        MODROW = 0x200,
        MS = 1,
        PCM = 2,
        PCMBYTES = 4,
        RAWBYTES = 8,
        SENTENCE = 0x80000,
        SENTENCE_MS = 0x10000,
        SENTENCE_PCM = 0x20000,
        SENTENCE_PCMBYTES = 0x40000,
        SENTENCE_SUBSOUND = 0x100000
    }
}

