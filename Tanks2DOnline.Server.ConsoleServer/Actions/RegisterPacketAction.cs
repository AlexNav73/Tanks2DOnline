using System.Collections.Concurrent;
using System.Net;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.Action.Base;
using Tanks2DOnline.Core.Net.Packet;

namespace Tanks2DOnline.Server.ConsoleServer.Actions
{
    public class RegisterPacketAction : ActionBase
    {
        private ServerState _state;
        private readonly ConcurrentQueue<IPEndPoint> _queue;

        public RegisterPacketAction(ConcurrentQueue<IPEndPoint> queue)
        {
            _queue = queue;
        }

        protected override bool IsSupported(Packet packet)
        {
            return packet.Type == PacketType.LogOn;
        }

        protected override void Handle(Packet packet)
        {
            _state = _state ?? (ServerState) State;

            LogManager.Info("User {0} with address {1} has logged on", packet.UserName, packet.Address);

            _state.Users.Add(packet.UserName, packet.Address);
            _queue.Enqueue(packet.Address);

            State.Client.Send(PacketFactory.TypedPacket(PacketType.LogOn), packet.Address);
        }
    }
}
