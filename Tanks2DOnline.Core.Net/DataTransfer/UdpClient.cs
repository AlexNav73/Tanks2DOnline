using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.DataTransfer.Base;
using Tanks2DOnline.Core.Net.Handle;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Core.Net.DataTransfer
{
    public class UdpClient : UdpSocket
    {
        private readonly HandleStorage _handles = new HandleStorage();

        public UdpClient(IDictionary<DataType, IPacketHandle> handles)
        {
            foreach (var packetHandle in handles)
                _handles.AddHandle(packetHandle.Key, packetHandle.Value);

            Init(new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp));
        }

        public void Bind(IPAddress self, int port)
        {
            Socket.Bind(new IPEndPoint(self, port));
        }

        public void Recv(ref EndPoint remote)
        {
            var packet = RecvPacket(ref remote);
            _handles[packet.DataType].Process(packet);
        }

        public void Send(Packet.Packet packet, EndPoint remote)
        {
            SendPacket(packet, remote);
        }
    }
}
