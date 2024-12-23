using System;

namespace RQ.Unpacker
{
    class TocEntry
    {
        public UInt32 dwUserPtr { get; set; } // 0
        public UInt32 dwFlags { get; set; }
        public UInt32 dwOffset { get; set; }
        public UInt32 dwCompressedSize { get; set; }
        public UInt32 dwDecompressedSize { get; set; }
        public UInt16 wArchiveIndex { get; set; }
        public UInt16 wFileNameLength { get; set; }
        public String m_FileName { get; set; }
        public String m_ArchiveName { get; set; }
}
}
