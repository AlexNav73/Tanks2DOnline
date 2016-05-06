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
using Tanks2DOnline.Core.Net.Handle.Builder;
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

        public Server(ServerConfiguration config, PacketManagerBuilder builder)
        {
            _udpClient = new UdpClient(builder);
            _udpClient.Bind(IPAddress.Any, config.Port);
        }

        public void Listen()
        {
            var remote = (EndPoint)new IPEndPoint(IPAddress.Loopback, 4242);
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
