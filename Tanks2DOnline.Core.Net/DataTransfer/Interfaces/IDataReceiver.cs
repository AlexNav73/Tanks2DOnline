using System;
using System.Net;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Core.Net.DataTransfer.Interfaces
{
    public interface IDataReceiver
    {
        void Recv(Type objType, ref EndPoint remote, Action<object> callback);
    }
}