using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
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

        public override DataType GetDataType()
        {
            return DataType;
        }

        protected override void AfterDeserialization()
        {
            if (Type == PacketType.Registration && Data != null)
            {
                UserName = Encoding.ASCII.GetString(Data);
            }
        }
    }

    public class PacketComparer : IComparer<Packet>, IEqualityComparer<Packet>
    {
        public int Compare(Packet x, Packet y)
        {
            return x.Id.CompareTo(y.Id);
        }

        public bool Equals(Packet x, Packet y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Packet obj)
        {
            return obj.Address != null
                ? obj.Address.GetHashCode()
                : obj.UserName != null ? obj.UserName.GetHashCode() : obj.Data.GetHashCode();
        }
    }
}
