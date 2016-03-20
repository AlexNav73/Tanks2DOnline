using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.Serialization;
using Tanks2DOnline.Core.Net.Serialization.Attributes;

namespace Tanks2DOnline.Core.Net.CommonData
{
    public class Packet : SerializableObjectBase, IComparable
    {
        [Mark] public int Id { get; set; }
        [Mark] public int Count { get; set; }
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

        public Packet(int id, int count, PacketType type)
        {
            Id = id;
            Count = count;
            Type = type;
            Client = new ClientInfo();
        }

        public int CompareTo(object obj)
        {
            Packet lhs = obj as Packet;
            return lhs != null ? Id.CompareTo(lhs.Id) : -1;
        }
    }
}
