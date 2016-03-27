using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Core.Net.DataTransfer.Base
{
    public abstract class PacketTransferBase : IDisposable
    {
        public int Port { get; set; }

        private bool _isDisposed = false;
        private readonly UdpSocket _socket;
        private EndPoint _remoteIp;

        public delegate void OnTransmitionComplete<in T>(string userName, T item);

        protected PacketTransferBase(Socket socket)
        {
            _socket = new UdpSocket(socket);
            Port = 4242;
        }

        public void SetRemote(IPAddress remote)
        {
            _remoteIp = new IPEndPoint(remote, Port);
        }

        protected virtual void Send(Packet.Packet packet)
        {
            _socket.SendPacket(packet, _remoteIp);
        }

        protected virtual Packet.Packet Recv()
        {
            return _socket.RecvPacket(ref _remoteIp);
        }

        public abstract void Send<T>(string userName, T item, PacketType type) where T : SerializableObjectBase;
        public abstract void Recv<T>(OnTransmitionComplete<T> callback) where T : SerializableObjectBase;

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
