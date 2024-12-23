using System;

namespace RQ.Unpacker
{
    class CacheHeader
    {
        public Byte[] lpAfMD5 { get; set; }
        public Byte[] lpMD5 { get; set; }
        public UInt32 dwMagic { get; set; } // RQAF (0x46415152)
        public UInt32 dwFormatVersion { get; set; } // 0
        public UInt32 dwRevision { get; set; } // 0
        public UInt32 dwReserved { get; set; } // 0
        public UInt32 dwValidSize { get; set; }
        public UInt16 wCompression { get; set; } // 1 NONE, 2 ZLIB, 3 LZMA
        public UInt16 wReserved { get; set; }
        public UInt32 dwCompressedSize { get; set; }
        public UInt32 dwDecompressedSize { get; set; }
    }
}
