using System;

namespace RQ.Unpacker
{
    class TocDiskFileEntry
    {
        public UInt32 dwFlags { get; set; } // 1
        public Byte[] lpMD5 { get; set; }
        public UInt32 dwLowDateTime { get; set; }
        public UInt32 dwHighDateTime { get; set; }
        public UInt32 dwFileSize { get; set; }
        public String m_FileName { get; set; } // 256 (unicode)
    }
}
