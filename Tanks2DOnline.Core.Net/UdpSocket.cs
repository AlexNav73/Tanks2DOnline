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
    public class UdpSocket : IDisposable
    {
        private readonly Socket _socket;
        private bool _isDisposed;

        public readonly int Port = 4242;

        public Socket Socket { get { return _socket; }}

        public UdpSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        public void Bind(IPAddress ipAddress)
        {
            _socket.Bind(new IPEndPoint(ipAddress, Port));
        }

        public Packet RecvPacket(ref EndPoint point)
        {
            byte[] buf = new byte[_socket.ReceiveBufferSize];
            int recv = _socket.ReceiveFrom(buf, ref point);

            if (recv != 0)
            {
                Packet packet = new Packet();

                byte[] response = new byte[recv];
                Array.Copy(buf, response, recv);

                packet.Desirialize(response);

                return packet;
            }

            return null;
        }

        public int SendPacket(Packet packet, ref EndPoint dest)
        {
            return _socket.SendTo(packet.Serialize(), dest);
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                _socket.Close();
            }
        }
    }
}
