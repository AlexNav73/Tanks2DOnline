using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.Packet;

namespace Tanks2DOnline.Client.ConsoleClient.Actions.Implementations
{
    public class RegistrationRequestAction : IAction
    {
        public void Execute(Client client, Packet packet)
        {
            if (packet.Type == PacketType.Registration)
                client.IsConnected = true;
        }
    }
}
