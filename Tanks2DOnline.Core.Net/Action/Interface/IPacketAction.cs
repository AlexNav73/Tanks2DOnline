namespace Tanks2DOnline.Core.Net.Action.Interface
{
    public interface IPacketAction
    {
        void Execute(Packet.Packet packet);
    }
}
