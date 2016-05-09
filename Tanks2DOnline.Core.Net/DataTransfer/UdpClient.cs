using System.Net;
using System.Net.Sockets;
using Tanks2DOnline.Core.Net.Action;
using Tanks2DOnline.Core.Net.Action.Base;
using Tanks2DOnline.Core.Net.DataTransfer.Base;

namespace Tanks2DOnline.Core.Net.DataTransfer
{
    public class UdpClient : UdpSocket
    {
        private readonly ActionManager _manager;

        public UdpClient(ActionManagerBuilder builder, IUdpClientState state)
        {
            state.Client = this;
            _manager = builder.Build(state);

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
