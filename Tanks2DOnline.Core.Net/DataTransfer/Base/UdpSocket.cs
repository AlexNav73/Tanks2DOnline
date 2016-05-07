using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using Tanks2DOnline.Core.Logging;

namespace Tanks2DOnline.Core.Net.DataTransfer.Base
{
    public abstract class UdpSocket : IDisposable
    {
        protected Socket Socket;
        private const int MaxBuffSize = 65536;
        private readonly byte[] _buffer = new byte[MaxBuffSize];
        private readonly object _mutex = new object();

        protected void Init(Socket soc)
        {
            Socket = soc;
        }

        public Packet.Packet RecvPacket(ref EndPoint point)
        {
            Packet.Packet packet = null;

            lock (_mutex)
            {
                var recv = Socket.ReceiveFrom(_buffer, ref point);
                if (recv != 0)
                {
                    packet = Packet.Packet.FromBytes(_buffer, recv);
                    packet.Address = (IPEndPoint)point;
                }
            }

            return packet;
        }

        public int SendPacket(Packet.Packet packet, EndPoint dest)
        {
            return Socket.SendTo(packet.Serialize(), dest);
        }

        #region IDisposable
        private bool _isDisposed;
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                Socket.Close();
            }
        }
        #endregion
    }
}
