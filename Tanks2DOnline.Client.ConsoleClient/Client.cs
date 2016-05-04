using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Client.ConsoleClient.Configuration;
using Tanks2DOnline.Client.ConsoleClient.Handles;
using Tanks2DOnline.Client.ConsoleClient.Handles.ClientActions;
using Tanks2DOnline.Core.Net;
using Tanks2DOnline.Core.Net.DataTransfer;
using Tanks2DOnline.Core.Net.Handle;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Net.TestObjects;
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

        public Client(ClientConfiguration config, Dictionary<DataType, IHandle> handles)
        {
            var handle = new ConsumingHandler(handles);

            // Needs to register every DataType to one handle (ConsumingHandler)
            // because he process all data in separete thread
            _udpClient = new UdpClient(new Dictionary<DataType, IPacketHandle>()
            {
                {DataType.State, handle}
            });

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
