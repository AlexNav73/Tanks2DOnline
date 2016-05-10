using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.Action.Base;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Server.ConsoleServer.Actions
{
    public class DataPacketAction : ParallelActionBase
    {
        private ServerState _state;

        protected override bool IsSupported(Packet packet)
        {
            return packet.Type == PacketType.State;
        }

        protected override void HandleAsync(Packet packet)
        {
            _state = _state ?? (ServerState) State;

            foreach (var endPoint in _state.Users.GetAllExcept(packet.Address))
            {
                LogManager.Debug("Packet from {0} redirected to {1}", packet.UserName, endPoint);
                _state.Client.Send(packet, endPoint);
            }
        }
    }
}
