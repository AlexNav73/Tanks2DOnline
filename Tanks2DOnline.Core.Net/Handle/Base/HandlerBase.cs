using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.Handle.Interfaces;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Core.Net.Handle.Base
{
    public abstract class HandlerBase<T> : IHandler where T: SerializableObjectBase
    {
        public abstract void Process(T obj);

        public virtual bool CanProcess(object obj)
        {
            return obj is T;
        }

        public void Process(object obj)
        {
            if (CanProcess(obj))
                Process((T)obj);
        }
    }
}
