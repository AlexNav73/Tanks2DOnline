using System;
using System.Net;
using System.Net.Sockets;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.CommonData;

namespace Tanks2DOnline.Core.Net.DataTransfer.Base
{
    public class UdpSocket : IDisposable
    {
        private const int MaxBuffSize = 65536;
        private readonly byte[] _buffer = new byte[MaxBuffSize];

        private readonly Socket _socket;
        private bool _isDisposed;

        public Socket Socket { get { return _socket; }}

        public UdpSocket(Socket socket)
        {
            _socket = socket;
        }

        public Packet RecvPacket(ref EndPoint point)
        {
            try
            {
                int recv = _socket.ReceiveFrom(_buffer, ref point);

                if (recv != 0)
                {
                    Packet packet = new Packet();
                    packet.Desirialize(_buffer, recv);
                    return packet;
                }
            }
            catch (Exception e)
            {
                LogManager.Error("UdpSocket: {0}", e.Message);
                throw;
            }
            return null;
        }

        public int SendPacket(Packet packet, EndPoint dest)
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
