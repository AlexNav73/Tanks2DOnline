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

namespace Tanks2DOnline.Server.ConsoleServer
{
    public class Server : IDisposable
    {
        private readonly BlockingCollection<Packet> _queue = new BlockingCollection<Packet>();
        private readonly Dictionary<string, EndPoint> _users = new Dictionary<string, EndPoint>();
        private readonly Dictionary<PacketType, IAction> _actions = new Dictionary<PacketType, IAction>()
        {
            {PacketType.LogOn, new RegisterUserAction()},
            {PacketType.Data, new ProcessDataAction()}
        };

        private readonly DataTransferManager _manager;
        private bool _isDisposed = false;

        public Server(ServerConfiguration config, int tasksCount = 10)
        {
            _manager = new DataTransferManager(IPAddress.Any, config.Port);

            for (int i = 0; i < tasksCount; i++)
            {
                Task.Factory.StartNew(ProcessCollection);
            }
        }

        public void Listen()
        {
            var remote = (EndPoint)new IPEndPoint(IPAddress.Loopback, 4242);
            var packetType = typeof (Packet);
            while (true)
            {
                try
                {
                    _manager.RecvData(packetType, ref remote, p => _queue.Add(p as Packet));
                }
                catch (SocketException e)
                {
                    LogManager.Info("User has quit, but not log off.");
                }
            }
        }

        public void RegisterUser(string name, EndPoint remote)
        {
            lock (_users)
            {
                if (!_users.ContainsKey(name))
                {
                    _users.Add(name, remote);
                    LogManager.Info("User {0} was registered with address {1}", name, remote);
                    _manager.SendData(remote, new Packet(), PacketType.LogOn);
                }
                else LogManager.Info("User {0} has logged on already", name);
            }
        }

        public List<KeyValuePair<string, EndPoint>> GetUsersExcept(EndPoint address)
        {
            lock (_users)
            {
                return _users.Where(u => !u.Value.Equals(address)).ToList();
            }
        }

        public void SendReply(EndPoint remote, Packet packet, PacketType type)
        {
            _manager.SendData(remote, packet, type);
            LogManager.Debug("Reply to user {0} sended", _users.FirstOrDefault(u => u.Value.Equals(remote)).Key);
        }

        private void ProcessCollection()
        {
            foreach (var packet in _queue.GetConsumingEnumerable())
            {
                if (_actions.ContainsKey(packet.Type))
                    _actions[packet.Type].Process(this, packet);
            }
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
