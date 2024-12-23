using System;

namespace RQ.Unpacker
{
    class TocComprHeader
    {
        public Byte[] lpMD5 { get; set; }
        public UInt32 dwMagic { get; set; } // RQCD (0x44435152)
        public UInt32 dwReserved { get; set; } // 0
        public TocFlags z_Compression { get; set; } // 1 NONE, 2 ZLIB, 3 LZMA
        public UInt32 dwCompressedSize { get; set; }
        public UInt32 dwDecompressedSize { get; set; }
    }
}
