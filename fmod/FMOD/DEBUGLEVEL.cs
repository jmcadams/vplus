namespace FMOD
{
    using System;

    public enum DEBUGLEVEL
    {
        ALL = -1,
        DISPLAY_ALL = 0xf000000,
        DISPLAY_COMPRESS = 0x4000000,
        DISPLAY_LINENUMBERS = 0x2000000,
        DISPLAY_TIMESTAMPS = 0x1000000,
        LEVEL_ALL = 0xff,
        LEVEL_ERROR = 2,
        LEVEL_HINT = 8,
        LEVEL_LOG = 1,
        LEVEL_NONE = 0,
        LEVEL_WARNING = 4,
        TYPE_ALL = 0xffff,
        TYPE_EVENT = 0x1000,
        TYPE_FILE = 0x400,
        TYPE_MEMORY = 0x100,
        TYPE_NET = 0x800,
        TYPE_THREAD = 0x200
    }
}

