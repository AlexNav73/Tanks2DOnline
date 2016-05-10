using System;
using System.Collections.Generic;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Core.Net.Helpers
{
    public static class DataHelper
    {
        private const long UdpPacketMaxSize = SerializableObjectBase.UdpPacketMaxSize;
        private static readonly PacketComparer _comparer = new PacketComparer();

        public static IEnumerable<Packet.Packet> SplitToPackets<T>(T item, PacketType type) where T : SerializableObjectBase
        {
            var data = item.Serialize();
            long dataSize = sizeof(byte) * data.Length;
            var countOfPackets = (long)Math.Ceiling((double)dataSize / UdpPacketMaxSize);

            for (int i = 0; i < countOfPackets; i++)
            {
                var packet = new Packet.Packet(i, (int)countOfPackets, type) { DataType = item.GetDataType() };

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

        public static object ExtractData(Type objType, List<Packet.Packet> packets)
        {
            int total = packets[0].Count;
            packets.Sort(_comparer);

            if (total != packets.Count)
            {
                LogManager.Warn("Can't extract data from packets, because not all packets recieved");
                return null;
            }
            if (!ValidatePacketSeq(packets))
            {
                LogManager.Warn("Packet sequance is not valid");
                return null;
            }

            byte[] data = new byte[total * UdpPacketMaxSize];

            for (int i = 0; i < total; i++)
            {
                int dataLength = packets[i].Data.Length;
                Array.Copy(packets[i].Data, 0, data, i * UdpPacketMaxSize, dataLength);
            }

            var res = Activator.CreateInstance(objType) as SerializableObjectBase;
            res.Desirialize(data, data.Length);
            return res;
        }

        public static object ExtractData(Type objType, Packet.Packet packet)
        {
            var res = Activator.CreateInstance(objType) as SerializableObjectBase;
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
