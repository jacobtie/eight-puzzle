using System;
using System.Text;
using System.IO;

namespace assignment1.logging
{
    public static class Logger
    {
        private static StringBuilder _sb = new StringBuilder();

        public static void WriteLine(String line = "")
        {
            Write(line + "\n");
        }

        public static void Write(String entry)
        {
            Console.Write(entry);
            _sb.Append(entry);
        }

        public static void WriteLogFile()
        {
            using (var writer = new StreamWriter($"logs/log_{DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond}.txt"))
            {
                writer.Write(_sb.ToString());
            }
        }
    }
}
