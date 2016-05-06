using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.DataTransfer;
using Tanks2DOnline.Core.Net.Handle.Base;
using Tanks2DOnline.Core.Net.Packet;

namespace Tanks2DOnline.Core.Net.Handle.Builder
{
    public class PacketManagerBuilder
    {
        private readonly Dictionary<PacketType, PacketTypeHandlerBase> _actions;

        public PacketManagerBuilder()
        {
            _actions = new Dictionary<PacketType, PacketTypeHandlerBase>();
        }

        public PacketTypeHandlerBase AddAction(PacketType type, PacketTypeHandlerBase action)
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
