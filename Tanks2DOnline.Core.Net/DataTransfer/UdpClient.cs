using System.Net;
using System.Net.Sockets;
using Tanks2DOnline.Core.Net.DataTransfer.Base;
using Tanks2DOnline.Core.Net.DataTransfer.Builder;
using Tanks2DOnline.Core.Net.Handle.Interfaces;

namespace Tanks2DOnline.Core.Net.DataTransfer
{
    public class UdpClient : UdpSocket
    {
        private readonly PacketManager _manager;

        public UdpClient(PacketManagerBuilder builder)
        {
            _manager = builder.Build(this);
            Init(new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp));
        }

        public void Bind(IPAddress self, int port)
        {
            Socket.Bind(new IPEndPoint(self, port));
        }

        public void Recv(ref EndPoint remote)
        {
            _manager.Manage(RecvPacket(ref remote));
        }

        public void Send(Packet.Packet packet, EndPoint remote)
        {
            SendPacket(packet, remote);
        }
    }
}
