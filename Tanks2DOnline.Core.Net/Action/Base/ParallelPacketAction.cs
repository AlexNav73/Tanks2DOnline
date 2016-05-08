using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Tanks2DOnline.Core.Net.Action.Base
{
    public abstract class ParallelPacketAction : PacketTypeActionBase
    {
        private static readonly BlockingCollection<Packet.Packet> ReceivingQueue = new BlockingCollection<Packet.Packet>();

        protected ParallelPacketAction()
        {
            Task.Factory.StartNew(ProcessingLoop);
        }

        protected abstract void HandleAsync(Packet.Packet packet);

        private void ProcessingLoop()
        {
            foreach (var packet in ReceivingQueue.GetConsumingEnumerable())
            {
                if (IsSupported(packet))
                    HandleAsync(packet);
            }
        }

        protected override void Handle(Packet.Packet packet)
        {
            ReceivingQueue.Add(packet);
        }
    }
}
