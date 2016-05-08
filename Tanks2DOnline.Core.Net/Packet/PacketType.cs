namespace Tanks2DOnline.Core.Net.Packet
{
    public enum PacketType : byte
    {
        State,
        BigDataBatch,
        LogOn,
        LogOff,
        Register,
        PacketAcceptRequest
    }
}
