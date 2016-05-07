using Tanks2DOnline.Core.Net.DataTransfer;
using Tanks2DOnline.Core.Net.Handle;
using Tanks2DOnline.Core.Net.Handle.Interfaces;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Core.Net.Action.Base
{
    public abstract class PacketTypeActionBase
    {
        protected abstract bool IsSupported(PacketType type);
        protected abstract void Handle(Packet.Packet packet);

        protected readonly HandleStorage Handles;
        protected UdpClient Client;

        public void SetClient(UdpClient client)
        {
            Client = client;
        }

        protected PacketTypeActionBase()
        {
            Handles = new HandleStorage();
        }

        public HandleStorage AddHandle(DataType type, IMsgHandler handler)
        {
            Handles.AddHandle(type, handler);
            return Handles;
        }

        public void Process(Packet.Packet packet)
        {
            if (IsSupported(packet.Type))
                Handle(packet);
        }
    }
}
