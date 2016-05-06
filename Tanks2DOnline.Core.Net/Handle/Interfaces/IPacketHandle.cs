namespace Tanks2DOnline.Core.Net.Handle.Interfaces
{
    public interface IPacketHandle
    {
        void Process(Packet.Packet packet);
    }
}
