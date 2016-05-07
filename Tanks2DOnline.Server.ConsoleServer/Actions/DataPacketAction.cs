using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.Action.Base;
using Tanks2DOnline.Core.Net.Packet;

namespace Tanks2DOnline.Server.ConsoleServer.Actions
{
    public class DataPacketAction : ParallelPacketAction
    {
        private readonly UserMapCollection _users;

        public DataPacketAction(UserMapCollection users)
        {
            _users = users;
        }

        protected override bool IsSupported(PacketType type)
        {
            return type == PacketType.Data;
        }

        protected override void HandleAsync(Packet packet)
        {
            foreach (var endPoint in _users.GetAllExcept(packet.Address))
            {
                LogManager.Info("Packet from {0} redirected to {1}", packet.UserName, endPoint);
                Client.Send(packet, endPoint);
            }
        }
    }
}
