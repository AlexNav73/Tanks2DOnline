using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks2DOnline.Core.Net.CommonData
{
    public class ObjectInfo<T>
    {
        public T Object { get; set; }
        public PacketType PacketType { get; set; }
    }
}
