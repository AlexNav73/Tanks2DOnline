using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks2DOnline.Core.Net.Handle
{
    public interface IPacketHandle
    {
        void Process(Packet.Packet packet);
    }
}
