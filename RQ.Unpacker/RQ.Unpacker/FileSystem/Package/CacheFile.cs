using System;
using System.IO;

namespace RQ.Unpacker
{
    class CacheFile
    {
        public static MemoryStream iLoadFile(String m_FileName)
        {
            using (FileStream TCacheStream = File.OpenRead(m_FileName))
            {
                var m_CacheHeader = new CacheHeader();

                m_CacheHeader.lpAfMD5 = TCacheStream.ReadBytes(16);
                m_CacheHeader.lpMD5 = TCacheStream.ReadBytes(16);
                m_CacheHeader.dwMagic = TCacheStream.ReadUInt32();
                m_CacheHeader.dwFormatVersion = TCacheStream.ReadUInt32();
                m_CacheHeader.dwRevision = TCacheStream.ReadUInt32();
                m_CacheHeader.dwReserved = TCacheStream.ReadUInt32();
                m_CacheHeader.dwValidSize = TCacheStream.ReadUInt32();
                m_CacheHeader.wCompression = TCacheStream.ReadUInt16();
                m_CacheHeader.wReserved = TCacheStream.ReadUInt16();
                m_CacheHeader.dwCompressedSize = TCacheStream.ReadUInt32();
                m_CacheHeader.dwDecompressedSize = TCacheStream.ReadUInt32();

                var m_CacheEntry = new CacheEntry();

                m_CacheEntry.lpMD5 = TCacheStream.ReadBytes(16);
                m_CacheEntry.wCompression = TCacheStream.ReadUInt16();
                m_CacheEntry.wReserved = TCacheStream.ReadUInt16();
                m_CacheEntry.dwCompressedSize = TCacheStream.ReadUInt32();
                m_CacheEntry.dwDecompressedSize = TCacheStream.ReadUInt32();
                m_CacheEntry.dwNumOfFiles = TCacheStream.ReadUInt32();

                var lpSrcBuffer = TCacheStream.ReadBytes((Int32)m_CacheHeader.dwCompressedSize);
                var lpDstBuffer = Zlib.iDecompress(lpSrcBuffer);

                TCacheStream.Dispose();

                var TCacheMemoryStream = new MemoryStream(lpDstBuffer);

                return TCacheMemoryStream;
            }
        }

        public static void iReadData(Stream TStream, String m_FileName, UInt32 dwOffset, UInt32 dwCompressedSize, UInt32 dwDecompressedSize)
        {
            TStream.Seek(dwOffset, SeekOrigin.Begin);

            if (dwCompressedSize == dwDecompressedSize)
            {
                var lpBuffer = TStream.ReadBytes((Int32)dwDecompressedSize);

                File.WriteAllBytes(m_FileName, lpBuffer);
            }
            else
            {
                var lpSrcBuffer = TStream.ReadBytes((Int32)dwCompressedSize);
                var lpDstBuffer = Zlib.iDecompress(lpSrcBuffer);

                File.WriteAllBytes(m_FileName, lpDstBuffer);
            }
        }
    }
}
