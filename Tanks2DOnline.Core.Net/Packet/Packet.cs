using System;
using System.Net;
using Tanks2DOnline.Core.Serialization;
using Tanks2DOnline.Core.Serialization.Attributes;

namespace Tanks2DOnline.Core.Net.Packet
{
    public class Packet : SerializableObjectBase, IComparable
    {
        [Mark] public int Id { get; set; }
        [Mark] public int Count { get; set; }
        [Mark] public PacketType Type { get; set; }
        [Mark] public byte[] Data { get; set; }

        public EndPoint UserEndPoint { get; set; }

        public Packet() { }

        public Packet(int id, int count, PacketType type)
        {
            Id = id;
            Count = count;
            Type = type;
        }

        public int CompareTo(object obj)
        {
            Packet lhs = obj as Packet;
            return lhs != null ? Id.CompareTo(lhs.Id) : -1;
        }

        public static Packet FromBytes(byte[] data, int count)
        {
            var packet = new Packet();
            packet.Desirialize(data, count);
            return packet;
        }
    }
}
