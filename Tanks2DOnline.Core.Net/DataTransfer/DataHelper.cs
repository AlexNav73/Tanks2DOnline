using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.CommonData;
using Tanks2DOnline.Core.Net.Serialization;

namespace Tanks2DOnline.Core.Net.DataTransfer
{
    public static class DataHelper
    {
        private const long UdpPacketMaxSize = 25535;

        public static IEnumerable<Packet> SplitToPackets<T>(T item, PacketType type) where T : SerializableObjectBase
        {
            var data = item.Serialize();
            long dataSize = sizeof(byte) * data.Length;
            var countOfPackets = (long)Math.Ceiling((double)dataSize / UdpPacketMaxSize);

            for (int i = 0; i < countOfPackets; i++)
            {
                var packet = new Packet(i, (int)countOfPackets, type);

                var currentBatchSize = (double)(dataSize - (i * UdpPacketMaxSize)) / UdpPacketMaxSize;
                var batchSize = currentBatchSize >= 1
                    ? UdpPacketMaxSize
                    : (long)(currentBatchSize * UdpPacketMaxSize);
                var batch = new byte[batchSize];

                Array.Copy(data, i * UdpPacketMaxSize, batch, 0, batchSize);
                packet.Client.Data = batch;

                yield return packet;
            }
        }

        public static T ExtractData<T>(List<Packet> packets) where T : SerializableObjectBase
        {
            packets.Sort();
            packets = packets.Distinct().ToList();

            if (!ValidatePacketSeq(packets)) return null;

            int total = packets.First().Count;
            byte[] data = new byte[total * UdpPacketMaxSize];

            for (int i = 0; i < total; i++)
            {
                int dataLength = packets[i].Client.Data.Length;
                Array.Copy(packets[i].Client.Data, 0, data, i * UdpPacketMaxSize, dataLength);
            }

            var res = Activator.CreateInstance<T>();
            res.Desirialize(data);
            return res;
        }

        private static bool ValidatePacketSeq(List<Packet> packets)
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
