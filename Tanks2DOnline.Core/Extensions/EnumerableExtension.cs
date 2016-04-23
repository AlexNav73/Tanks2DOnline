using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks2DOnline.Core.Extensions
{
    public static class EnumerableExtension
    {
        public static void ForEach<T>(this IEnumerable<T> seq, Action<T> predicate)
        {
            Argument.IsNull(predicate, "predicate");

            foreach (T item in seq)
            {
                predicate(item);
            }
        }
    }
}
