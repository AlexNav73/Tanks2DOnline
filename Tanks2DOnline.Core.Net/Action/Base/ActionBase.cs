using Tanks2DOnline.Core.Net.DataTransfer;
using Tanks2DOnline.Core.Net.Handle;
using Tanks2DOnline.Core.Net.Handle.Interfaces;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Core.Net.Action.Base
{
    public abstract class ActionBase
    {
        protected abstract bool IsSupported(Packet.Packet packet);
        protected abstract void Handle(Packet.Packet packet);

        protected readonly HandleManager Handles;
        protected IUdpClientState State;

        public void SetClient(IUdpClientState state)
        {
            State = state;
        }

        protected ActionBase()
        {
            Handles = new HandleManager();
        }

        public HandleManager AddHandle(DataType type, IHandler handler)
        {
            Handles.AddHandle(type, handler);
            return Handles;
        }

        public void Process(Packet.Packet packet)
        {
            if (IsSupported(packet))
                Handle(packet);
        }
    }
}
