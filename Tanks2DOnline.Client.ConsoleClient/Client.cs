using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Client.ConsoleClient.Configuration;
using Tanks2DOnline.Client.ConsoleClient.Handles;
using Tanks2DOnline.Core.Net;
using Tanks2DOnline.Core.Net.DataTransfer;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Net.TestObjects;
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
        private readonly Dictionary<DataType, List<IHandle>> _handles;
        private readonly Dictionary<DataType, Type> _maps = new Dictionary<DataType, Type>()
        {
            {DataType.Small, typeof(SmallTestObject)},
            {DataType.Big, typeof(BigTestObject)}
        };
        private DataType _currentDataFlowType = DataType.Small;

        public bool IsConnected { get; set; }

        public Client(ClientConfiguration config, Dictionary<DataType, IEnumerable<IHandle>> handles)
        {
            _manager = new DataTransferManager(IPAddress.Any, config.Port);
            IsConnected = false;
            _config = config;
            _serverSocket = new IPEndPoint(IPAddress.Parse(config.ServerIP), config.ServerPort);
            _handles = handles.ToDictionary(p => p.Key, p => p.Value.ToList());
        }

        public void Start(string userName, Action work)
        {
            Task.Factory.StartNew(() =>
            {
                var packet = new Packet() {Data = Encoding.ASCII.GetBytes(userName)};
                _manager.SendData(_serverSocket, packet, PacketType.Registration);

                var remote = (EndPoint) new IPEndPoint(IPAddress.Any, 0);
                _manager.RecvData(typeof(Packet), ref remote, p => IsConnected = ((Packet)p).Type == PacketType.Registration);
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
                _manager.SendData(_serverSocket, obj, PacketType.Data);
            }
        }

        private void ReceivingLoop()
        {
            var remote = (EndPoint)new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                _manager.RecvData(_maps[_currentDataFlowType], ref remote, o =>
                {
                    var obj = o as SerializableObjectBase;
                    _receivingQueue.Add(obj);
                    _currentDataFlowType = obj.GetDataType();
                });
            }
        }

        private void ProcessingLoop()
        {
            foreach (var obj in _receivingQueue.GetConsumingEnumerable())
            {
                var handles = _handles[obj.GetDataType()];
                for (int i = 0; i < _handles.Count; i++)
                    handles[i].Process(obj);
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
