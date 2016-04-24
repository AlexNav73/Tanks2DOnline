using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.Packet;

namespace Tanks2DOnline.Server.ConsoleServer.Actions.Implementations
{
    public class ProcessDataAction : IAction
    {
        public void Process(Server serverState, Packet packet)
        {
            var users = serverState.GetUsersExcept(packet.Address);
            for (int i = 0; i < users.Count; i++)
            {
                serverState.SendReply(users[i].Value, packet, packet.Type);
            }
        }
    }
}
