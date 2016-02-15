using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.CommonData;

namespace Tanks2DOnline.Core.Net
{
    public class UdpSocket
    {
        private readonly Socket _socket;

        public readonly int Port = 4242;

        public Socket Socket { get { return _socket; }}

        public UdpSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        public void Bind(IPEndPoint point)
        {
            _socket.Bind(point);
        }

        public Packet RecvPacket(ref EndPoint point)
        {
            List<byte> totalBytes = new List<byte>();
            int recv = 0;

            Packet packet = new Packet();

            do
            {
                byte[] buf = new byte[_socket.ReceiveBufferSize];
                recv = _socket.ReceiveFrom(buf, ref point);

                if (recv != 0) totalBytes.AddRange(buf);

            } while (recv != 0);

            packet.Desirialize(totalBytes.ToArray());

            return packet;
        }
    }
}
