using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Core.Net.DataTransfer
{
    public static class DataHelper
    {
        private const long UdpPacketMaxSize = 25535;

        public static IEnumerable<Packet.Packet> SplitToPackets<T>(T item, PacketType type) where T : SerializableObjectBase
        {
            var data = item.Serialize();
            long dataSize = sizeof(byte) * data.Length;
            var countOfPackets = (long)Math.Ceiling((double)dataSize / UdpPacketMaxSize);

            for (int i = 0; i < countOfPackets; i++)
            {
                var packet = new Packet.Packet(i, (int)countOfPackets, type);

                var currentBatchSize = (double)(dataSize - (i * UdpPacketMaxSize)) / UdpPacketMaxSize;
                var batchSize = currentBatchSize >= 1
                    ? UdpPacketMaxSize
                    : (long)(currentBatchSize * UdpPacketMaxSize);
                var batch = new byte[batchSize];

                Array.Copy(data, i * UdpPacketMaxSize, batch, 0, batchSize);
                packet.Data = batch;

                yield return packet;
            }
        }

        public static T ExtractData<T>(List<Packet.Packet> packets) where T : SerializableObjectBase
        {
            packets.Sort();
            packets = packets.Distinct().ToList();

            int total = packets.First().Count;

            if (total != packets.Count) return null;
            if (!ValidatePacketSeq(packets)) return null;

            byte[] data = new byte[total * UdpPacketMaxSize];

            for (int i = 0; i < total; i++)
            {
                int dataLength = packets[i].Data.Length;
                Array.Copy(packets[i].Data, 0, data, i * UdpPacketMaxSize, dataLength);
            }

            var res = Activator.CreateInstance<T>();
            res.Desirialize(data, data.Length);
            return res;
        }

        public static T ExtractData<T>(Packet.Packet packet) where T : SerializableObjectBase
        {
            var res = Activator.CreateInstance<T>();
            res.Desirialize(packet.Data, packet.Data.Length);
            return res;
        }

        private static bool ValidatePacketSeq(List<Packet.Packet> packets)
        {
            var id = 0;
            for (int i = 0; i < packets.Count; i++)
            {
                if (id == packets[i].Id) id++;
                else return false;
            }
            return true;
        }
    }
}
