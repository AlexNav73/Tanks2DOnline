using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.DataTransfer.Builder;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Net.TestObjects;
using Tanks2DOnline.Server.ConsoleServer.Actions;
using Tanks2DOnline.Server.ConsoleServer.Configuration;
using UdpClient = Tanks2DOnline.Core.Net.DataTransfer.UdpClient;

namespace Tanks2DOnline.Server.ConsoleServer
{
    public class Server : IDisposable
    {
        private bool _isDisposed = false;
        private readonly UdpClient _udpClient;
        private readonly BigDataSender _sender;
        private readonly UserMapCollection _users;
        private readonly ConcurrentQueue<IPEndPoint> _queue; 

        public Server(ServerConfiguration config)
        {
            _users = new UserMapCollection();
            _queue = new ConcurrentQueue<IPEndPoint>();
            _udpClient = new UdpClient(CreateBuilder(_queue), CreateServerState());

            _sender = new BigDataSender(_udpClient, _queue);

            _udpClient.Bind(IPAddress.Any, config.Port);
        }

        private PacketManagerBuilder CreateBuilder(ConcurrentQueue<IPEndPoint> queue)
        {
            var builder = new PacketManagerBuilder();

            builder.AddAction(PacketType.LogOn, new RegisterPacketAction(queue));
            builder.AddAction(PacketType.State, new DataPacketAction());
            builder.AddAction(PacketType.PacketAcceptRequest, new BigDataPacketAction(queue));

            return builder;
        }

        private ServerState CreateServerState()
        {
            return new ServerState() { Users = _users };
        }

        public void Listen()
        {
            Task.Factory.StartNew(SendData);

            var remote = (EndPoint)new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    _udpClient.Recv(ref remote);
                }
                catch (SocketException e)
                {
                    LogManager.Info("User has quit, but not log off.");
                }
            }
        }

        private void SendData()
        {
            IPEndPoint user;
            while (!_queue.TryPeek(out user)) Thread.Sleep(10000);

            while (true)
            {
                Thread.Sleep(10000);
                LogManager.Info("Start sending big data ...");
                _sender.Send(new BigTestObject(), _users.GetAll());
                LogManager.Info("Big object sended");
            }
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
