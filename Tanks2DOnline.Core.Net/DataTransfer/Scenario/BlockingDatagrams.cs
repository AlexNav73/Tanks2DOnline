using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.DataTransfer.Base;
using Tanks2DOnline.Core.Net.DataTransfer.Interfaces;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Core.Net.DataTransfer.Scenario
{
    public class BlockingDatagrams : SimplePasketTransfer, IDataTransferer
    {
        public BlockingDatagrams(Socket socket) : base(socket) { }

        public void Send<T>(EndPoint remote, T obj, PacketType type) where T : SerializableObjectBase
        {
            var packet = obj as Packet.Packet;
            packet.Type = type;
            SendPacket(packet, remote);
        }

        public void Recv(Type objTpe, ref EndPoint remote, Action<object> callback)
        {
            var packet = RecvPacket(ref remote);
            var endPoint = ((IPEndPoint) remote);
            packet.Address = new IPEndPoint(endPoint.Address, endPoint.Port);
            callback(packet);
        }
    }
}
