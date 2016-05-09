using Tanks2DOnline.Core.Net.DataTransfer;
using Tanks2DOnline.Core.Net.Packet;

namespace Tanks2DOnline.Core.Net.Action.Base
{
    public class ActionManagerBuilder
    {
        private readonly ActionManager _manager = new ActionManager();

        public ActionBase AddAction(PacketType type, ActionBase action)
        {
            return _manager.AddAction(type, action);
        }

        public ActionManager Build(IUdpClientState state)
        {
            foreach (var action in _manager)
                action.Value.SetClient(state);
            return _manager;
        }
    }
}
