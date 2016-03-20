using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks2DOnline.Core.Logging
{
    public static class LogManager
    {
        public static void Debug(string text)
        {
#if (DEBUG)
            Console.WriteLine(text);
#endif
        }

        public static void Debug(string templ, params object[] args)
        {
#if (DEBUG)
            Console.WriteLine(templ, args);
#endif
        }
    }
}
