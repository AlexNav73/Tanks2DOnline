using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.Action.Base;
using Tanks2DOnline.Core.Net.Handle.Base;
using Tanks2DOnline.Core.Net.Packet;

namespace Tanks2DOnline.Client.ConsoleClient.Actions
{
    public class LogOnAction : PacketTypeActionBase
    {
        public bool IsConnected { get; set; }

        protected override bool IsSupported(PacketType type)
        {
            return type == PacketType.LogOn;
        }

        protected override void Handle(Packet packet)
        {
            LogManager.Info("Logged On");
            IsConnected = true;
        }
    }
}
