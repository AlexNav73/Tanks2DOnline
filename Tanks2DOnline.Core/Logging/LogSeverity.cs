using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks2DOnline.Core.Logging
{
    enum LogSeverity
    {
        Debug,
        Info,
        Warn,
        Error
    }

    public static class EnumExt
    {
        public static string GetKey(this Enum e)
        {
            return e.ToString();
        }
    }
}
