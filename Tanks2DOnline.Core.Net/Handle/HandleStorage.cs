using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using Tanks2DOnline.Core.Factory.Base;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Core.Net.Handle
{
    public class HandleStorage : Flyweight<DataType, IPacketHandle>
    {
        public new IPacketHandle this[DataType key]
        {
            get { return GetValue(key); }
        }

        public void AddHandle(DataType type, IPacketHandle handle)
        {
            Add(type, handle);
        }
    }
}
