using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.DataTransfer;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Net.TestObjects;
using Tanks2DOnline.Server.ConsoleServer.Actions;
using Tanks2DOnline.Server.ConsoleServer.Actions.Implementations;
using Tanks2DOnline.Server.ConsoleServer.Configuration;
using UdpClient = Tanks2DOnline.Core.Net.DataTransfer.UdpClient;

namespace Tanks2DOnline.Server.ConsoleServer
{
    public class Server : IDisposable
    {
        private readonly BlockingCollection<Packet> _queue = new BlockingCollection<Packet>();
        private readonly ServerState _state = null;
        private readonly Dictionary<PacketType, IAction> _actions = new Dictionary<PacketType, IAction>()
        {
            {PacketType.LogOn, new RegisterUserAction()},
            {PacketType.Data, new ProcessDataAction()}
        };

        private bool _isDisposed = false;

        public Server(ServerConfiguration config, int tasksCount = 10)
        {
            var udpClient = new UdpClient();
            udpClient.Bind(IPAddress.Any, config.Port);

            _state = new ServerState
            {
                Client = udpClient,
                Users = new UserMapCollection()
            };

            for (int i = 0; i < tasksCount; i++)
            {
                Task.Factory.StartNew(ProcessCollection);
            }
        }

        public void Listen()
        {
            var remote = (EndPoint)new IPEndPoint(IPAddress.Loopback, 4242);
            while (true)
            {
                try
                {
                    _state.Client.Recv(ref remote);
                }
                catch (SocketException e)
                {
                    LogManager.Info("User has quit, but not log off.");
                }
            }
        }

        private void ProcessCollection()
        {
            foreach (var packet in _queue.GetConsumingEnumerable())
            {
                if (_actions.ContainsKey(packet.Type))
                    _actions[packet.Type].Process(_state, packet);
            }
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                _state.Client.Dispose();
            }
        }
    }
}
