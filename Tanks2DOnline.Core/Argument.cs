using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks2DOnline.Core
{
    public static class Argument
    {
        public static void IsNull<T>(T arg, string name)
        {
            if (arg == null)
                throw new ArgumentNullException(name);
        }
    }
}
