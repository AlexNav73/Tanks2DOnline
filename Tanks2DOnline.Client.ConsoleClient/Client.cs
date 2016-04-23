using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Client.ConsoleClient.Actions;
using Tanks2DOnline.Client.ConsoleClient.Actions.Implementations;
using Tanks2DOnline.Client.ConsoleClient.Configuration;
using Tanks2DOnline.Core.Net.DataTransfer;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Client.ConsoleClient
{
    public class Client : IDisposable
    {
        private bool _isDisposed = false;
        private readonly DataTransferManager _manager;
        private readonly ClientConfiguration _config;
        private readonly EndPoint _serverSocket;
        private readonly BlockingCollection<SerializableObjectBase> _queue = new BlockingCollection<SerializableObjectBase>();

        //public delegate void OnStateReceived(T state);
        //public delegate void OnDataReceived<T>(T data);
        //public event OnStateReceived<T> OnState<T>; 

        private readonly Dictionary<PacketType, IAction> _actions = new Dictionary<PacketType, IAction>()
        {
            {PacketType.Registration, new RegistrationRequestAction()}
        };

        public bool IsConnected { get; set; }

        public Client(ClientConfiguration config)
        {
            _manager = new DataTransferManager(IPAddress.Loopback, config.Port);
            IsConnected = false;
            _config = config;
            _serverSocket = new IPEndPoint(IPAddress.Parse(config.ServerIP), config.ServerPort);
        }

        public void Start(string userName)
        {
            Task.Factory.StartNew(() =>
            {
                var packet = new Packet() {Data = Encoding.ASCII.GetBytes(userName)};
                _manager.SendData(_serverSocket, packet, PacketType.Registration);
            }).Wait(new TimeSpan(0, 0, 0, 0, _config.RegistrationTimeout));
            if (IsConnected)
            {
                Task.Factory.StartNew(SendingLoop);
                // TODO: Start receiving loop for small objects
                // TODO: Start receiving loop for large objects
            }
        }

        private void SendingLoop()
        {
            foreach (var obj in _queue.GetConsumingEnumerable())
            {
                _manager.SendData(_serverSocket, obj, PacketType.SmallData);
            }
        }

        public void SendObject<T>(T obj) where T : SerializableObjectBase
        {
            _queue.Add(obj);
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                _manager.Dispose();
            }
        }
    }
}
