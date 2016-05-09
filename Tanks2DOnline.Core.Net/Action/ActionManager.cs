using Tanks2DOnline.Core.Factory.Base;
using Tanks2DOnline.Core.Net.Action.Base;
using Tanks2DOnline.Core.Net.Packet;

namespace Tanks2DOnline.Core.Net.Action
{
    public class ActionManager : Flyweight<PacketType, ActionBase>
    {
        public ActionBase AddAction(PacketType type, ActionBase action)
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
