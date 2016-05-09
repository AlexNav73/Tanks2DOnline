using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Factory.Base;
using Tanks2DOnline.Core.Net.Handle.Interfaces;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Core.Net.Handle
{
    public class HandleManager : Flyweight<DataType, IHandler>
    {
        public new IHandler this[DataType key]
        {
            get { return GetValue(key); }
        }

        public HandleManager AddHandle(DataType key, IHandler handler)
        {
            Add(key, handler);
            return this;
        }
    }
}
