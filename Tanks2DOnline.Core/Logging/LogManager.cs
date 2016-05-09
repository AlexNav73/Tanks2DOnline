using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.LayoutRenderers;

namespace Tanks2DOnline.Core.Logging
{
    public class LogManager
    {
        private static readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public static void Debug(string text)
        {
#if (DEBUG)
            Argument.IsNull(text, "text");

            _logger.Debug(text);
#endif
        }

        public static void Debug(string templ, params object[] args)
        {
#if (DEBUG)
            Argument.IsNull(templ, "template");

            Debug(string.Format(templ, args));
#endif
        }

        public static void Error(string text)
        {
            Argument.IsNull(text, "text");

            _logger.Error(text);
        }

        public static void Error(string templ, params object[] args)
        {
            Argument.IsNull(templ, "template");

            Error(string.Format(templ, args));
        }

        public static void Error(Exception e, string templ, params object[] args)
        {
            Argument.IsNull(e, "e");
            Argument.IsNull(templ, "template");
            
            _logger.Error(e, templ, args);
        }

        public static void Info(string templ, params object[] args)
        {
            Argument.IsNull(templ, "template");

            Info(string.Format(templ, args));
        }

        public static void Info(string text)
        {
            Argument.IsNull(text, "text");

            _logger.Info(text);
        }

        public static void Warn(string text)
        {
            Argument.IsNull(text, "text");

            _logger.Warn(text);
        }

        public static void Warn(string templ, params object[] args)
        {
            Argument.IsNull(templ, "template");

            Warn(string.Format(templ, args));
        }
    }
}
