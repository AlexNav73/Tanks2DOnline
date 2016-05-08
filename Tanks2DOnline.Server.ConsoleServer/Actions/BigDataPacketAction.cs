using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.Action.Base;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Server.ConsoleServer.Actions
{
    public class BigDataPacketAction : PacketTypeActionBase
    {
        private readonly ConcurrentQueue<IPEndPoint> _queue;

        public BigDataPacketAction(ConcurrentQueue<IPEndPoint> queue)
        {
            _queue = queue;
        }

        protected override bool IsSupported(Packet packet)
        {
            return packet.Type == PacketType.PacketAcceptRequest;
        }

        protected override void Handle(Packet packet)
        {
            _queue.Enqueue(packet.Address);
        }
    }
}
