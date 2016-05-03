using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.Packet;

namespace Tanks2DOnline.Server.ConsoleServer.Actions.Implementations
{
    public class ProcessDataAction : IAction
    {
        public void Process(ServerState serverState, Packet packet)
        {
            var users = serverState.Users.GetAllExcept(packet.Address);
            for (int i = 0; i < users.Count; i++)
            {
                serverState.DataTransferManager.SendData(users[i], packet, packet.Type);
                LogManager.Debug("Reply to user {0} sended", serverState.Users.Get(users[i]));
            }
        }
    }
}
