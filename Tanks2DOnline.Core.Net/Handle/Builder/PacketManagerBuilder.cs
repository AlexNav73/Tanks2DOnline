using System.Collections.Generic;
using Tanks2DOnline.Core.Net.Action.Base;
using Tanks2DOnline.Core.Net.DataTransfer;
using Tanks2DOnline.Core.Net.Packet;

namespace Tanks2DOnline.Core.Net.Handle.Builder
{
    public class PacketManagerBuilder
    {
        private readonly Dictionary<PacketType, PacketTypeActionBase> _actions;

        public PacketManagerBuilder()
        {
            _actions = new Dictionary<PacketType, PacketTypeActionBase>();
        }

        public PacketTypeActionBase AddAction(PacketType type, PacketTypeActionBase action)
        {
            if (_actions.ContainsKey(type))
                return _actions[type];
            _actions[type] = action;
            return _actions[type];
        }

        public PacketManager Build(UdpClient client)
        {
            foreach (var action in _actions)
                action.Value.SetClient(client);
            return new PacketManager(_actions);
        }
    }
}
