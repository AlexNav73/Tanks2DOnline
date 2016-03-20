using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.CommonData;
using Tanks2DOnline.Core.Net.Serialization;

namespace Tanks2DOnline.Core.Net.DataTransfer.Base
{
    public abstract class PacketTransferBase : IDisposable
    {
        public int Port { get; set; }

        private bool _isDisposed = false;
        private readonly UdpSocket _socket;
        private EndPoint _remoteIp;

        protected PacketTransferBase(IPAddress ipAddress)
        {
            _socket = new UdpSocket();
            Port = 4242;

            if (ipAddress != null) _socket.Bind(ipAddress);
        }

        public void SetRemote(IPAddress remote)
        {
            _remoteIp = new IPEndPoint(remote, Port);
        }

        protected virtual void Send(Packet packet)
        {
            _socket.SendPacket(packet, ref _remoteIp);
        }

        protected virtual Packet Recv()
        {
            return _socket.RecvPacket(ref _remoteIp);
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                _socket.Dispose();
            }
        }

    }
}
