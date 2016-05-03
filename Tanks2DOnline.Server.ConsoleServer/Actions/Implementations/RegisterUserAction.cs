using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.Packet;

namespace Tanks2DOnline.Server.ConsoleServer.Actions.Implementations
{
    public class RegisterUserAction : IAction
    {
        public void Process(ServerState serverState, Packet packet)
        {
            if (packet.Type == PacketType.LogOn)
            {
                serverState.Users.Add(packet.UserName, packet.Address);
                LogManager.Info("User {0} was registered with address {1}", packet.UserName, packet.Address);
                serverState.DataTransferManager.SendData(packet.Address, new Packet(), PacketType.LogOn);
            }
        }
    }
}
