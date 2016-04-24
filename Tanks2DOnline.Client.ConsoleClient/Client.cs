using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Client.ConsoleClient.Configuration;
using Tanks2DOnline.Client.ConsoleClient.Handles;
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
        private readonly BlockingCollection<SerializableObjectBase> _sendingQueue = new BlockingCollection<SerializableObjectBase>();
        private readonly BlockingCollection<SerializableObjectBase> _receivingQueue = new BlockingCollection<SerializableObjectBase>();
        private readonly List<IHandle> _handles; 

        public bool IsConnected { get; set; }

        public Client(ClientConfiguration config, IEnumerable<IHandle> handles)
        {
            _manager = new DataTransferManager(IPAddress.Any, config.Port);
            IsConnected = false;
            _config = config;
            _serverSocket = new IPEndPoint(IPAddress.Parse(config.ServerIP), config.ServerPort);
            _handles = handles.ToList();
        }

        public void Start(string userName, Action work)
        {
//            var packet = new Packet() { Data = Encoding.ASCII.GetBytes(userName) };
//            _manager.SendData(_serverSocket, packet, PacketType.Registration);
//            return;

            Task.Factory.StartNew(() =>
            {
                var packet = new Packet() {Data = Encoding.ASCII.GetBytes(userName)};
                _manager.SendData(_serverSocket, packet, PacketType.Registration);

                var remote = (EndPoint) new IPEndPoint(IPAddress.Any, 0);
                _manager.RecvData<Packet>(ref remote, p => IsConnected = p.Type == PacketType.Registration);
            }).Wait(new TimeSpan(0, 0, 0, 0, _config.RegistrationTimeout));

            if (IsConnected)
            {
                var task = Task.Factory.StartNew(SendingLoop);
                Task.Factory.StartNew(ReceivingLoop);
                Task.Factory.StartNew(ProcessingLoop);
                work();
                task.Wait();
            }
        }

        private void SendingLoop()
        {
            foreach (var obj in _sendingQueue.GetConsumingEnumerable())
            {
                _manager.SendData(_serverSocket, obj, PacketType.SmallData);
            }
        }

        private void ReceivingLoop()
        {
            var remote = (EndPoint)new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                // TODO: I need to switch type depends of packet type
                //       SerializableObjectBase can't be used here
                _manager.RecvData<SerializableObjectBase>(ref remote, OnReceived);
            }
        }

        // TODO: Replase this method with some concrete type of argument
        private void OnReceived(SerializableObjectBase obj)
        {
            _receivingQueue.Add(obj);
        }

        private void ProcessingLoop()
        {
            foreach (var obj in _receivingQueue.GetConsumingEnumerable())
            {
                for (int i = 0; i < _handles.Count; i++)
                    _handles[i].Process(obj);
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
                _manager.Dispose();
            }
        }
    }
}
