using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.Serialization;
using Tanks2DOnline.Core.Net.Serialization.Attributes;

namespace Tanks2DOnline.Core.Net.CommonData
{
    public class Packet : SerializableObjectBase
    {
        [Mark] public PacketType Type { get; set; }
        [Mark] public ClientInfo Client { get; set; }

        public Packet()
        {
            Client = new ClientInfo();
        }

        public Packet(PacketType type)
        {
            Type = type;
        }
    }
}
