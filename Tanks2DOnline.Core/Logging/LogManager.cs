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
            var logFileName = assembly.FullName.Substring(0, assembly.FullName.IndexOf(','));
            _logFileName = string.Format("{0}.txt", Path.Combine(Path.GetDirectoryName(assembly.Location), "Logs", logFileName));
            var logDirectory = Path.GetDirectoryName(_logFileName);

            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);

            var fileInfo = new FileInfo(_logFileName);
            if (fileInfo.Exists)
            {
                if (fileInfo.CreationTime.Date < DateTime.Now.Date)
                {
                    var fileName = Path.GetFileNameWithoutExtension(_logFileName);
                    var dateTimePostfix = fileInfo.CreationTime.Date.ToString("MM_dd_yyyy");
                    var newPath = string.Format("{0}_{1}.txt", fileName, dateTimePostfix);
                    File.Move(_logFileName, Path.Combine(logDirectory, newPath));
                }
            }
            else
            {
                File.WriteAllText(_logFileName, "");
            }
        }

        private static void WriteToFile(string text)
        {
            Task.Factory.StartNew(() =>
            {
                lock (Instance._mutex)
                {
                    var lines = File.ReadAllLines(Instance._logFileName).ToList();
                    lines.Insert(0, text);
                    File.WriteAllLines(Instance._logFileName, lines);
                }
            });
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
            var log = FormatMessage(LogSeverity.Debug, text);
            PrintToConsole(ConsoleColor.White, log);
            WriteToFile(log);
#endif
        }

        public static void Debug(string templ, params object[] args)
        {
#if (DEBUG)
            Debug(string.Format(templ, args));
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

        public static void Info(string templ, params object[] args)
        {
            Info(string.Format(templ, args));
        }

        public static void Info(string text)
        {
            string log = FormatMessage(LogSeverity.Info, text);
            PrintToConsole(ConsoleColor.Yellow, log);
            WriteToFile(log);
        }
    }
}
