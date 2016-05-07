using System.Collections.Generic;
using Tanks2DOnline.Core.Net.Action.Base;
using Tanks2DOnline.Core.Net.Packet;

namespace Tanks2DOnline.Core.Net.DataTransfer.Builder
{
    public class PacketManagerBuilder
    {
        private readonly PacketManager _manager = new PacketManager();

        public PacketTypeActionBase AddAction(PacketType type, PacketTypeActionBase action)
        {
            return _manager.AddAction(type, action);
        }

        public PacketManager Build(UdpClient client)
        {
            foreach (var action in _manager)
                action.Value.SetClient(client);
            return _manager;
        }
    }
}
