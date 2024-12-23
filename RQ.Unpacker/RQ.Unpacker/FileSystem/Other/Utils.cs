using System;
using System.IO;
using System.Reflection;

namespace RQ.Unpacker
{
    class Utils
    {
        public static UInt32 iAlignUInt32(UInt32 dwValue, UInt32 dwAlignSize)
        {
            if (dwValue == 0) {
                return dwValue;
            }

            return dwValue + ((dwAlignSize - (dwValue % dwAlignSize)) % dwAlignSize);
        }

        public static String iGetApplicationPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public static String iGetApplicationVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static void iSetInfo(String m_String)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(m_String);
            Console.ResetColor();
        }

        public static void iSetError(String m_String)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(m_String + "!");
            Console.ResetColor();
        }

        public static void iSetWarning(String m_String)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(m_String + "!");
            Console.ResetColor();
        }

        public static String iCheckArgumentsPath(String m_Arg)
        {
            if (m_Arg.EndsWith("\\") == false)
            {
                m_Arg = m_Arg + @"\";
            }
            return m_Arg;
        }

        public static void iCreateDirectory(String m_Directory)
        {
            if (!Directory.Exists(Path.GetDirectoryName(m_Directory)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(m_Directory));
            }
        }
    }
}
