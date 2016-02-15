using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.CommonData;
using Tanks2DOnline.Server.ServerApp.Match;

namespace Tanks2DOnline.Server.ServerApp.PacketProcessors
{
    public class ProcessorContext
    {
        public EndPoint Point { get; set; }
        public ClientInfo Info { get; set; }
        public MatchMaker MatchMaker { get; set; }
        public PacketType PacketType { get; set; }

        public ProcessorContext(EndPoint point, Packet packet)
        {
            Point = point;
            Info = packet.Client;
            PacketType = packet.Type;
        }
    }
}
