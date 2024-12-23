using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace RQ.Unpacker
{
    class TocUnpack
    {
        private static String m_StartupFile = "data/startup.af".Replace("/", @"\");
        private static String m_PCacheFile = "data/pcache.af".Replace("/", @"\");

        private static List<TocDiskFileEntry> m_ArchiveTable = new List<TocDiskFileEntry>();
        private static List<TocDiskFileEntry> m_DiskTable = new List<TocDiskFileEntry>();
        private static List<TocEntry> m_EntryTable = new List<TocEntry>();

        public static void iDoIt(String m_TocFile, String m_DstFolder)
        {
            using (FileStream TTocStream = File.OpenRead(m_TocFile))
            {
                var m_ComprHeader = new TocComprHeader();

                m_ComprHeader.lpMD5 = TTocStream.ReadBytes(16);
                m_ComprHeader.dwMagic = TTocStream.ReadUInt32();
                m_ComprHeader.dwReserved = TTocStream.ReadUInt32();
                m_ComprHeader.z_Compression = (TocFlags)TTocStream.ReadUInt32();
                m_ComprHeader.dwCompressedSize = TTocStream.ReadUInt32();
                m_ComprHeader.dwDecompressedSize = TTocStream.ReadUInt32();

                if (m_ComprHeader.dwMagic != 0x44435152)
                {
                    throw new Exception("[ERROR]: Invalid magic of TOC file!");
                }

                if (m_ComprHeader.dwReserved != 0)
                {
                    throw new Exception("[ERROR]: Invalid TOC file!");
                }

                var lpTableSrcBuffer = TTocStream.ReadBytes((Int32)m_ComprHeader.dwCompressedSize);
                var lpTableDstBuffer = Zlib.iDecompress(lpTableSrcBuffer);

                TTocStream.Dispose();

                using (MemoryStream TTocMemoryStream = new MemoryStream(lpTableDstBuffer))
                {
                    var m_CommonHeader = new TocCommonHeader();

                    m_CommonHeader.dwVersion = TTocMemoryStream.ReadUInt32();

                    if (m_CommonHeader.dwVersion != 0)
                    {
                        throw new Exception("[ERROR]: Invalid TOC version!");
                    }

                    m_CommonHeader.lpTotalMD5 = TTocMemoryStream.ReadBytes(16);
                    m_CommonHeader.dwReserved1[0] = TTocMemoryStream.ReadUInt32();
                    m_CommonHeader.dwReserved1[1] = TTocMemoryStream.ReadUInt32();
                    m_CommonHeader.dwReserved1[2] = TTocMemoryStream.ReadUInt32();
                    m_CommonHeader.dwReserved1[3] = TTocMemoryStream.ReadUInt32();
                    m_CommonHeader.dwNumOfArchives = TTocMemoryStream.ReadUInt32();
                    m_CommonHeader.dwNumOfDiskFiles = TTocMemoryStream.ReadUInt32();
                    m_CommonHeader.dwNumOfFiles = TTocMemoryStream.ReadUInt32();
                    m_CommonHeader.dwNumOfHashItems = TTocMemoryStream.ReadUInt32();
                    m_CommonHeader.dwReserved2[0] = TTocMemoryStream.ReadUInt32();
                    m_CommonHeader.dwReserved2[1] = TTocMemoryStream.ReadUInt32();
                    m_CommonHeader.dwReserved2[2] = TTocMemoryStream.ReadUInt32();
                    m_CommonHeader.dwReserved2[3] = TTocMemoryStream.ReadUInt32();

                    m_DiskTable.Clear();
                    for (Int32 i = 0; i < m_CommonHeader.dwNumOfArchives + 2; i++)
                    {
                        var m_DiskFileEntry = new TocDiskFileEntry();

                        m_DiskFileEntry.dwFlags = TTocMemoryStream.ReadUInt32();
                        m_DiskFileEntry.lpMD5 = TTocMemoryStream.ReadBytes(16);
                        m_DiskFileEntry.dwLowDateTime = TTocMemoryStream.ReadUInt32();
                        m_DiskFileEntry.dwHighDateTime = TTocMemoryStream.ReadUInt32();
                        m_DiskFileEntry.dwFileSize = TTocMemoryStream.ReadUInt32();
                        m_DiskFileEntry.m_FileName = TTocMemoryStream.ReadUnicodeString();

                        m_ArchiveTable.Add(m_DiskFileEntry);
                    }

                    m_DiskTable.Clear();
                    for (Int32 i = 0; i < m_CommonHeader.dwNumOfDiskFiles - m_CommonHeader.dwNumOfArchives - 2; i++)
                    {
                        var m_DiskFileEntry = new TocDiskFileEntry();

                        m_DiskFileEntry.dwFlags = TTocMemoryStream.ReadUInt32();
                        m_DiskFileEntry.lpMD5 = TTocMemoryStream.ReadBytes(16);
                        m_DiskFileEntry.dwLowDateTime = TTocMemoryStream.ReadUInt32();
                        m_DiskFileEntry.dwHighDateTime = TTocMemoryStream.ReadUInt32();
                        m_DiskFileEntry.dwFileSize = TTocMemoryStream.ReadUInt32();
                        m_DiskFileEntry.m_FileName = TTocMemoryStream.ReadUnicodeString();

                        m_DiskTable.Add(m_DiskFileEntry);
                    }

                    m_EntryTable.Clear();
                    for (Int32 i = 0; i < m_CommonHeader.dwNumOfFiles; i++)
                    {
                        UInt32 dwEntryOffset = TTocMemoryStream.ReadUInt32();
                        Int64 dwSavePos = TTocMemoryStream.Position;

                        TTocMemoryStream.Seek(dwEntryOffset, SeekOrigin.Begin);

                        var m_Entry = new TocEntry();

                        m_Entry.dwUserPtr = TTocMemoryStream.ReadUInt32();
                        m_Entry.dwFlags = TTocMemoryStream.ReadUInt32();
                        m_Entry.dwOffset = TTocMemoryStream.ReadUInt32();
                        m_Entry.dwCompressedSize = TTocMemoryStream.ReadUInt32();
                        m_Entry.dwDecompressedSize = TTocMemoryStream.ReadUInt32();
                        m_Entry.wArchiveIndex = TTocMemoryStream.ReadUInt16();
                        m_Entry.wFileNameLength = TTocMemoryStream.ReadUInt16();
                        m_Entry.m_FileName = Encoding.ASCII.GetString(TTocMemoryStream.ReadBytes(m_Entry.wFileNameLength)).Replace("/", @"\");


                        if (m_Entry.wArchiveIndex > m_ArchiveTable.Count)
                        {
                            switch(m_Entry.wArchiveIndex)
                            {
                                case 0xFFFE: m_Entry.m_ArchiveName = "data/startup.af".Replace("/", @"\"); break;
                                case 0xFFFF: m_Entry.m_ArchiveName = "data/pcache.af".Replace("/", @"\"); break;
                            }
                        }
                        else
                        {
                            m_Entry.m_ArchiveName = m_ArchiveTable[m_Entry.wArchiveIndex].m_FileName.Replace("/", @"\");
                        }

                        m_EntryTable.Add(m_Entry);

                        TTocMemoryStream.Seek(dwSavePos, SeekOrigin.Begin);
                    }

                    TTocMemoryStream.Dispose();
                }

                Utils.iSetInfo("[INFO]: Loading cache file -> " + m_StartupFile);
                var TStartupStream = CacheFile.iLoadFile(Path.GetDirectoryName(m_TocFile) + @"\" + m_StartupFile);

                Utils.iSetInfo("[INFO]: Loading cache file -> " + m_PCacheFile);
                var TPCacheStream = CacheFile.iLoadFile(Path.GetDirectoryName(m_TocFile) + @"\" + m_PCacheFile);

                foreach (var m_Entry in m_EntryTable)
                {
                    String m_FullPath = m_DstFolder + m_Entry.m_FileName;

                    Utils.iSetInfo("[UNPACKING]: " + m_Entry.m_FileName);
                    Utils.iCreateDirectory(m_FullPath);

                    if (m_Entry.wArchiveIndex > m_ArchiveTable.Count)
                    {
                        switch (m_Entry.wArchiveIndex)
                        {
                            case 0xFFFE: CacheFile.iReadData(TStartupStream, m_FullPath, m_Entry.dwOffset, m_Entry.dwCompressedSize, m_Entry.dwDecompressedSize); break;
                            case 0xFFFF: CacheFile.iReadData(TPCacheStream, m_FullPath, m_Entry.dwOffset, m_Entry.dwCompressedSize, m_Entry.dwDecompressedSize); break;
                            default: continue;
                        }
                    }
                    else
                    {
                        using (FileStream TArchiveStream = File.OpenRead(Path.GetDirectoryName(m_TocFile) + @"\" + m_Entry.m_ArchiveName))
                        {
                            TArchiveStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);

                            if (m_Entry.dwCompressedSize != m_Entry.dwDecompressedSize)
                            {
                                var lpSrcBuffer = TArchiveStream.ReadBytes((Int32)m_Entry.dwCompressedSize);
                                var lpDstBuffer = Zlib.iDecompress(lpSrcBuffer);

                                File.WriteAllBytes(m_FullPath, lpDstBuffer);
                            }
                            else
                            {
                                var lpBuffer = TArchiveStream.ReadBytes((Int32)m_Entry.dwDecompressedSize);
                                File.WriteAllBytes(m_FullPath, lpBuffer);
                            }

                            TArchiveStream.Dispose();
                        }
                    }
                }

                TPCacheStream.Dispose();
                TStartupStream.Dispose();
            }
        }
    }
}
