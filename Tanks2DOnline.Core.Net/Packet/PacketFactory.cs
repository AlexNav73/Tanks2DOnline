using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.DataTransfer;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Core.Net.Packet
{
    public static class PacketFactory
    {
        public static Packet CreateLogOnPacket(string userName)
        {
            return new Packet()
            {
                Data = Encoding.ASCII.GetBytes(userName),
                Type = PacketType.LogOn
            };
        }

        public static Packet Wrap<T>(T obj) where T : SerializableObjectBase
        {
            var data = obj.Serialize();
            if (data.Length > SerializableObjectBase.UdpPacketMaxSize)
                throw new Exception("Data size exceeded max udp packet size.");

            return new Packet()
            {
                Data = data,
                Type = PacketType.State,
                DataType = obj.GetDataType()
            };
        }

        public static List<Packet> WrapBigObject<T>(T obj) where T : SerializableObjectBase
        {
            return DataHelper.SplitToPackets(obj, PacketType.State).ToList();
        }

        public static Packet TypedPacket(PacketType type)
        {
            return new Packet()
            {
                Type = type
            };
        }
    }
}
