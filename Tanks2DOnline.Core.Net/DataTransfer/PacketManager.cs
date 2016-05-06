using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.Handle;
using Tanks2DOnline.Core.Net.Handle.Base;
using Tanks2DOnline.Core.Net.Packet;

namespace Tanks2DOnline.Core.Net.DataTransfer
{
    public class PacketManager
    {
        private readonly Dictionary<PacketType, PacketTypeHandlerBase> _actions;

        public PacketManager(Dictionary<PacketType, PacketTypeHandlerBase> actions)
        {
            _actions = actions;
        }

        public void Manage(Packet.Packet packet)
        {
            _actions[packet.Type].Process(packet);
        }
    }
}
