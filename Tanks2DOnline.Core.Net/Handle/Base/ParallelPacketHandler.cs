using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.DataTransfer;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Net.TestObjects;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Core.Net.Handle.Base
{
    public abstract class ParallelPacketHandler : PacketTypeHandlerBase
    {
        private static readonly BlockingCollection<Packet.Packet> ReceivingQueue = new BlockingCollection<Packet.Packet>();

        protected ParallelPacketHandler()
        {
            Task.Factory.StartNew(ProcessingLoop);
        }

        protected abstract void HandleAsync(Packet.Packet packet);

        private void ProcessingLoop()
        {
            foreach (var packet in ReceivingQueue.GetConsumingEnumerable())
            {
                HandleAsync(packet);
            }
        }

        protected override void Handle(Packet.Packet packet)
        {
            ReceivingQueue.Add(packet);
        }
    }
}
