using System;
using System.IO;

namespace RQ.Unpacker
{
    class Program
    {
        private static String m_Title = "Royal Quest Online Unpacker";
        static void Main(string[] args)
        {
            Console.Title = m_Title;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(m_Title);
            Console.WriteLine("(c) 2024 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    RQ.Unpacker <m_TocFile> <m_Directory>\n");
                Console.WriteLine("    m_File - Source of .toc file");
                Console.WriteLine("    m_Directory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    RQ.Unpacker E:\\Games\\RQ\\client_2962300\\.toc D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_TocFile = args[0];
            String m_Output = Utils.iCheckArgumentsPath(args[1]);

            if (!File.Exists(m_TocFile))
            {
                Utils.iSetError("[ERROR]: Input file -> " + m_TocFile + " <- does not exist");
                return;
            }

            TocUnpack.iDoIt(m_TocFile, m_Output);
        }
    }
}
