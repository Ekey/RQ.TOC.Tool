using System;

namespace RQ.Unpacker
{
    class CacheEntry
    {
        public Byte[] lpMD5 { get; set; }
        public UInt16 wCompression { get; set; } // 1 NONE, 2 ZLIB, 3 LZMA
        public UInt16 wReserved { get; set; }
        public UInt32 dwCompressedSize { get; set; }
        public UInt32 dwDecompressedSize { get; set; }
        public UInt32 dwNumOfFiles { get; set; }
    }
}
