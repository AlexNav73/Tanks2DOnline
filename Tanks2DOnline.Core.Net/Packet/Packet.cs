using System;
using System.Net;
using System.Text;
using Tanks2DOnline.Core.Serialization;
using Tanks2DOnline.Core.Serialization.Attributes;

namespace Tanks2DOnline.Core.Net.Packet
{
    public class Packet : SerializableObjectBase
    {
        [Mark] public int Id { get; set; }
        [Mark] public int Count { get; set; }
        [Mark] public PacketType Type { get; set; }
        [Mark] public DataType DataType { get; set; }
        [Mark] public byte[] Data { get; set; }

        public string UserName { get; set; }
        public IPEndPoint Address { get; set; }

        public Packet() { }

        public Packet(int id, int count, PacketType type)
        {
            Id = id;
            Count = count;
            Type = type;
        }

        public static Packet FromBytes(byte[] data, int count)
        {
            var packet = new Packet();
            packet.Desirialize(data, count);
            return packet;
        }

        protected override void AfterDeserialization()
        {
            if (Type == PacketType.LogOn && Data != null)
            {
                UserName = Encoding.ASCII.GetString(Data);
            }
        }
    }
}
