using System.Collections.Concurrent;
using System.Net;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.Action.Base;
using Tanks2DOnline.Core.Net.Packet;

namespace Tanks2DOnline.Server.ConsoleServer.Actions
{
    public class BigDataPacketAction : ParallelActionBase
    {
        private readonly ConcurrentQueue<IPEndPoint> _queue;

        public BigDataPacketAction(ConcurrentQueue<IPEndPoint> queue)
        {
            _queue = queue;
        }

        protected override bool IsSupported(Packet packet)
        {
            return packet.Type == PacketType.PacketAcceptResponse;
        }

        protected override void HandleAsync(Packet packet)
        {
            _queue.Enqueue(packet.Address);
        }
    }
}
