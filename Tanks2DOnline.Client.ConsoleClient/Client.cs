using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading.Tasks;
using Tanks2DOnline.Client.ConsoleClient.Configuration;
using Tanks2DOnline.Core.Net.DataTransfer;
using Tanks2DOnline.Core.Net.Handle.Builder;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Client.ConsoleClient
{
    public class Client : IDisposable
    {
        private bool _isDisposed = false;

        private readonly UdpClient _udpClient;
        private readonly ClientConfiguration _config;
        private readonly EndPoint _serverSocket;

        private readonly BlockingCollection<SerializableObjectBase> _sendingQueue = new BlockingCollection<SerializableObjectBase>();

        public bool IsConnected { get; set; }

        public Client(ClientConfiguration config, PacketManagerBuilder builder)
        {
            _udpClient = new UdpClient(builder);
            _udpClient.Bind(IPAddress.Any, config.Port);

            IsConnected = false;
            _config = config;
            _serverSocket = new IPEndPoint(IPAddress.Parse(config.ServerIP), config.ServerPort);
        }

        public void Start(string userName, Action work)
        {
            Task.Factory.StartNew(() =>
            {
                _udpClient.Send(PacketFactory.CreateLogOnPacket(userName), _serverSocket);

                var remote = (EndPoint) new IPEndPoint(IPAddress.Any, 0);
                _udpClient.Recv(ref remote);
            }).Wait(new TimeSpan(0, 0, 0, 0, _config.RegistrationTimeout));

            if (IsConnected)
            {
                Task.Factory.StartNew(SendingLoop);
                Task.Factory.StartNew(ReceivingLoop);
                work();
            }
        }

        private void SendingLoop()
        {
            foreach (var obj in _sendingQueue.GetConsumingEnumerable())
            {
                _udpClient.Send(PacketFactory.Wrap(obj), _serverSocket);
            }
        }

        private void ReceivingLoop()
        {
            var remote = (EndPoint)new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                _udpClient.Recv(ref remote);
            }
        }

        public void SendObject<T>(T obj) where T : SerializableObjectBase
        {
            _sendingQueue.Add(obj);
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                _udpClient.Dispose();
            }
        }
    }
}
