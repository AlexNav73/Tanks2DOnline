using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Client.ConsoleClient.Handles
{
    public interface IHandle
    {
        void Process<T>(T obj) where T : SerializableObjectBase;
    }
}
