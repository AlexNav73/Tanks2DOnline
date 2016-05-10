using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading.Tasks;
using Tanks2DOnline.Client.ConsoleClient.Actions;
using Tanks2DOnline.Client.ConsoleClient.Configuration;
using Tanks2DOnline.Client.ConsoleClient.Handles;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.Action.Base;
using Tanks2DOnline.Core.Net.DataTransfer;
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

        private LogOnAction _logOnAction;

        public Client(ClientConfiguration config)
        {
            _udpClient = new UdpClient(CreateBuilder(), CreateClientState());
            _udpClient.Bind(IPAddress.Any, config.Port);

            _config = config;
            _serverSocket = new IPEndPoint(IPAddress.Parse(config.ServerIP), config.ServerPort);
        }

        private ActionManagerBuilder CreateBuilder()
        {
            _logOnAction = new LogOnAction();
            var builder = new ActionManagerBuilder();

            builder.AddAction(PacketType.LogOn, _logOnAction);
            builder.AddAction(PacketType.State, new StateParallelAction())
                .AddHandle(DataType.State, new SmallObjectProcessHandle());
            builder.AddAction(PacketType.BigDataBatch, new BigDataParallelAction())
                .AddHandle(DataType.BigData, new BigObjectProcessHandle());

            return builder;
        }

        private ClientState CreateClientState()
        {
            return new ClientState();
        }

        public void Start(string userName, Action work)
        {
            Task.Factory.StartNew(() =>
            {
                _udpClient.Send(PacketFactory.CreateRegistrationPacket(userName), _serverSocket);

                var remote = (EndPoint) new IPEndPoint(IPAddress.Any, 0);
                _udpClient.Recv(ref remote);
            }).Wait(new TimeSpan(0, 0, 0, 0, _config.RegistrationTimeout));

            if (_logOnAction.IsConnected)
            {
                Task.Factory.StartNew(SendingLoop);
                Task.Factory.StartNew(ReceivingLoop);
                work();
            }
            else LogManager.Error("Connection failed.");
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
                try
                {
                    _udpClient.Recv(ref remote);
                }
                catch (Exception e)
                {
                    LogManager.Error(e, "Error occured while receiving data: {0}", e.Message);
                }
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

        ~Client() { this.Dispose(); }

    }
}
