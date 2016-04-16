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
        private readonly char[] _separator = new[] {':'};
        public BlockingDatagrams(Socket socket) : base(socket) { }

        public void Send<T>(EndPoint remote, T obj, PacketType type) where T : SerializableObjectBase
        {
            SendPacket(obj as Packet.Packet, remote);
        }

        public void Recv<T>(ref EndPoint remote, Action<T> callback) where T : SerializableObjectBase
        {
            var packet = RecvPacket(ref remote);
            var parts = remote.ToString().Split(_separator, StringSplitOptions.RemoveEmptyEntries);
            packet.Address = new IPEndPoint(IPAddress.Parse(parts[0]), int.Parse(parts[1]));
            callback(packet as T);
        }
    }
}
