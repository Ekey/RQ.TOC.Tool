﻿using System;
using System.IO;
using System.IO.Compression;

namespace RQ.Unpacker
{
    class Zlib
    {
        public static Byte[] iDecompress(Byte[] lpBuffer, Int64 dwPosition = 2)
        {
            var TOutMemoryStream = new MemoryStream();

            using (MemoryStream TMemoryStream = new MemoryStream(lpBuffer) { Position = dwPosition })
            {
                using (DeflateStream TDeflateStream = new DeflateStream(TMemoryStream, CompressionMode.Decompress, false))
                {
                    TDeflateStream.CopyTo(TOutMemoryStream);
                    TDeflateStream.Dispose();
                }

                TMemoryStream.Dispose();
            }

            return TOutMemoryStream.ToArray();
        }
    }
}
