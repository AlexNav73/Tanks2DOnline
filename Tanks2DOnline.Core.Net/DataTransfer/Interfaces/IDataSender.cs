using System.Net;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Core.Net.DataTransfer.Interfaces
{
    public interface IDataSender
    {
        void Send<T>(EndPoint remote, T obj, PacketType type) where T : SerializableObjectBase;
    }
}