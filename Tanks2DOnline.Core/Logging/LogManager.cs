﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Tanks2DOnline.Core.Logging
{
    public class LogManager
    {
        private static readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public static void Debug(string text)
        {
#if (DEBUG)
            _logger.Debug(text);
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
            _logger.Error(text);
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
            _logger.Info(text);
        }
    }
}
