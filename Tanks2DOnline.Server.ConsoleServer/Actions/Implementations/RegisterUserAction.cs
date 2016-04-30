using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.Packet;

namespace Tanks2DOnline.Server.ConsoleServer.Actions.Implementations
{
    public class RegisterUserAction : IAction
    {
        public void Process(Server serverState, Packet packet)
        {
            if (packet.Type == PacketType.LogOn)
            {
                serverState.RegisterUser(packet.UserName, packet.Address);
            }
        }
    }
}
