namespace FMOD
{
    using System;

    public enum MODE : uint
    {
        _2D = 8,
        _3D = 0x10,
        _3D_CUSTOMROLLOFF = 0x4000000,
        _3D_HEADRELATIVE = 0x40000,
        _3D_IGNOREGEOMETRY = 0x40000000,
        _3D_LINEARROLLOFF = 0x200000,
        _3D_LOGROLLOFF = 0x100000,
        _3D_WORLDRELATIVE = 0x80000,
        ACCURATETIME = 0x4000,
        CDDA_FORCEASPI = 0x400000,
        CDDA_JITTERCORRECT = 0x800000,
        CREATECOMPRESSEDSAMPLE = 0x200,
        CREATESAMPLE = 0x100,
        CREATESTREAM = 0x80,
        DEFAULT = 0,
        HARDWARE = 0x20,
        IGNORETAGS = 0x2000000,
        LOADSECONDARYRAM = 0x20000000,
        LOOP_BIDI = 4,
        LOOP_NORMAL = 2,
        LOOP_OFF = 1,
        LOWMEM = 0x8000000,
        MPEGSEARCH = 0x8000,
        NONBLOCKING = 0x10000,
        OPENMEMORY = 0x800,
        OPENMEMORY_POINT = 0x10000000,
        OPENONLY = 0x2000,
        OPENRAW = 0x1000,
        OPENUSER = 0x400,
        SOFTWARE = 0x40,
        UNICODE = 0x1000000,
        UNIQUE = 0x20000,
        VIRTUAL_PLAYFROMSTART = 0x80000000
    }
}

