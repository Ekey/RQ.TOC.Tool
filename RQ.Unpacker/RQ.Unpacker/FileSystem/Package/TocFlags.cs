using System;

namespace RQ.Unpacker
{
    [Flags]
    public enum TocFlags : UInt32
    {
        NONE = 1,
        ZLIB = 2,
        LZMA = 3, // unused
    }
}
