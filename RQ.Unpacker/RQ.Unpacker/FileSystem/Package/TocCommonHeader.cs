using System;

namespace RQ.Unpacker
{
    class TocCommonHeader
    {
        public UInt32 dwVersion { get; set; } // 0
        public Byte[] lpTotalMD5 { get; set; }
        public UInt32[] dwReserved1 { get; set; } // x4 - 0, 0, 0, 0
        public UInt32 dwNumOfArchives { get; set; } // *.af archives, -2 => (pcache.af & startup.af not included)
        public UInt32 dwNumOfDiskFiles { get; set; }
        public UInt32 dwNumOfFiles { get; set; }
        public UInt32 dwNumOfHashItems { get; set; }
        public UInt32[] dwReserved2 { get; set; } // x4 - 0, 0, 0, 0

        public TocCommonHeader()
        {
            dwReserved1 = new UInt32[4];
            dwReserved2 = new UInt32[4];
        }
    }
}
