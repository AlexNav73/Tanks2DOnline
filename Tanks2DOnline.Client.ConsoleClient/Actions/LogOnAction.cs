using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.Handle.Base;
using Tanks2DOnline.Core.Net.Packet;

namespace Tanks2DOnline.Client.ConsoleClient.Actions
{
    public class LogOnAction : ParallelPacketHandler
    {
        protected override bool IsSupported(PacketType type)
        {
            return type == PacketType.LogOn;
        }

        protected override void HandleAsync(Packet packet)
        {
            LogManager.Info("Logged On");
        }
    }
}
