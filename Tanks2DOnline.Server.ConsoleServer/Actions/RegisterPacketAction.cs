using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.Handle.Base;
using Tanks2DOnline.Core.Net.Packet;

namespace Tanks2DOnline.Server.ConsoleServer.Actions
{
    public class RegisterPacketAction : PacketTypeHandlerBase
    {
        protected override bool IsSupported(PacketType type)
        {
            return type == PacketType.LogOn;
        }

        protected override void Handle(Packet packet)
        {
            LogManager.Info("User {0} with address {1} has logged on", packet.UserName, packet.Address);
            Client.Send(PacketFactory.RegisterAccept(), packet.Address);
        }
    }
}
