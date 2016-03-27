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
                byte[] buf = new byte[MaxBuffSize];
                int recv = _socket.ReceiveFrom(buf, ref point);

                if (recv != 0)
                {
                    Packet packet = new Packet();

                    byte[] response = new byte[recv];
                    Array.Copy(buf, response, recv);

                    packet.Desirialize(response);

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
