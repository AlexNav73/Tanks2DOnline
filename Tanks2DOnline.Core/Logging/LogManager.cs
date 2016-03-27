using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tanks2DOnline.Core.Logging
{
    public class LogManager
    {
        private static readonly Lazy<LogManager> _instance = new Lazy<LogManager>(() => new LogManager());
        private readonly string _logFileName = null;
        private readonly object _mutex = new object();

        public static LogManager Instance { get { return _instance.Value; }}

        public LogManager()
        {
            var assembly = Assembly.GetExecutingAssembly();
            _logFileName = string.Format("{0}.txt", Path.Combine(assembly.Location, "Logs", assembly.FullName));

            if (Directory.Exists(Path.GetDirectoryName(_logFileName)))
                Directory.CreateDirectory(Path.GetDirectoryName(_logFileName));

            var fileInfo = new FileInfo(_logFileName);
            if (fileInfo.Exists)
            {
                if (fileInfo.CreationTime.AddDays(1).Date == DateTime.Now.Date)
                    File.Move(_logFileName, string.Format("{0}{1}.txt", _logFileName, fileInfo.CreationTime));
            }
            File.WriteAllText(_logFileName, "");
        }

        private static void WriteToFile(string text)
        {
            lock (Instance._mutex)
            {
                File.AppendAllText(Instance._logFileName, text);
            }
        }

        private static string FormatMessage(LogSeverity type, string text)
        {
            return string.Format("{0} [{1}]: {2}", DateTime.Now, type.GetKey(), text);
        }

        private static void PrintToConsole(ConsoleColor color, string text)
        {
            var old = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = old;
        }

        public static void Debug(string text)
        {
#if (DEBUG)
            PrintToConsole(ConsoleColor.White, FormatMessage(LogSeverity.Debug, text));
#endif
        }

        public static void Debug(string templ, params object[] args)
        {
#if (DEBUG)
            PrintToConsole(ConsoleColor.White, FormatMessage(LogSeverity.Debug, string.Format(templ, args)));
#endif
        }

        public static void Error(string text)
        {
            string log = FormatMessage(LogSeverity.Error, text);
            PrintToConsole(ConsoleColor.Red, log);
            WriteToFile(log);
        }

        public static void Error(string templ, params object[] args)
        {
            Error(string.Format(templ, args));
        }
    }
}
