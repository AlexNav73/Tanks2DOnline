using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net;
using Tanks2DOnline.Core.Net.CommonData;
using Tanks2DOnline.Server.ServerApp.Match;
using Tanks2DOnline.Server.ServerApp.PacketProcessors;

namespace Tanks2DOnline.Server.ServerApp
{
    public class Server
    {
        private readonly UdpSocket _socket;
        private readonly MainProcessor _processor;
        private readonly MatchMaker _matchMaker;

        public Server()
        {
            _socket = new UdpSocket();
            _processor = new MainProcessor();
            _matchMaker = new MatchMaker();
            
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, _socket.Port);
            _socket.Bind(endpoint);
        }

        public void Start()
        {
            while (true)
            {
                IPEndPoint client = new IPEndPoint(IPAddress.Any, _socket.Port);
                ThreadPool.QueueUserWorkItem(ProcessClient, client);
            }
        }

        public void ProcessClient(object sender)
        {
            EndPoint client = (EndPoint) sender;
            Packet packet = _socket.RecvPacket(ref client);

            ProcessorContext context = new ProcessorContext(client, packet)
            {
                MatchMaker = _matchMaker
            };

            object ret = _processor.Process(context);
            SendReply(ret as Packet);
        }

        public void SendReply(Packet packet)
        {
            if (packet != null)
            {
                // TODO: Send reply packet to client over udp socket
            }
        }
    }
}
