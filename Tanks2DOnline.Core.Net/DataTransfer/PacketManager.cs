using Tanks2DOnline.Core.Factory.Base;
using Tanks2DOnline.Core.Net.Action.Base;
using Tanks2DOnline.Core.Net.Packet;

namespace Tanks2DOnline.Core.Net.DataTransfer
{
    public class PacketManager : Flyweight<PacketType, PacketTypeActionBase>
    {
        public PacketTypeActionBase AddAction(PacketType type, PacketTypeActionBase action)
        {
            Add(type, action);
            return action;
        }

        public void Manage(Packet.Packet packet)
        {
            this[packet.Type].Process(packet);
        }
    }
}
