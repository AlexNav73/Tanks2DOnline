using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net.CommonData;
using Tanks2DOnline.Core.Net.Serialization;

namespace Tanks2DOnline.Core.Net
{
    public class UdpClient : IDisposable
    {
        private const long UdpPacketMaxSize = 65535;

        public int Port { get; set; }

        private bool _isDisposed = false;
        private readonly UdpSocket _socket;
        private EndPoint _remoteIp;

        public UdpClient(IPAddress ipAddress)
        {
            _socket = new UdpSocket();
            Port = 4242;

            if (ipAddress != null) _socket.Bind(ipAddress);
        }

        public void SetRemote(IPAddress remote)
        {
            _remoteIp = new IPEndPoint(remote, Port);
        }

        public void Send<T>(T item, PacketType type) where T : SerializableObjectBase
        {
            byte[] data = item.Serialize();
            long dataSize = sizeof (byte)*data.Length;
            long countOfPackets = (long)Math.Ceiling((double) dataSize/UdpPacketMaxSize);

            Parallel.For(0, countOfPackets, i =>
            {
                Packet packet = CreatePacket((int) i, (int) countOfPackets, type);

                double currentBatchSize = (double) (dataSize - (i*UdpPacketMaxSize))/UdpPacketMaxSize;
                long batchSize = currentBatchSize >= 1 ? UdpPacketMaxSize : (long)(currentBatchSize*UdpPacketMaxSize);
                byte[] batch = new byte[batchSize];

                Array.Copy(data, i * UdpPacketMaxSize, batch, 0, batchSize);
                packet.Client.Data = batch;

                _socket.SendPacket(packet, ref _remoteIp);
            });
        }

        public T Recv<T>() where T : SerializableObjectBase
        {
            List<Packet> packets = new List<Packet>();

            Packet first = _socket.RecvPacket(ref _remoteIp);
            packets.Add(first);

            Parallel.For(0, first.Count - 1, i =>
            {
                packets.Add(_socket.RecvPacket(ref _remoteIp));
            });

            return first.Count == packets.Count ? ExstractData<T>(packets) : null;
        }

        private static T ExstractData<T>(List<Packet> packets) where T : SerializableObjectBase
        {
            packets.Sort();

            int total = packets.First().Count;
            byte[] data = new byte[total * UdpPacketMaxSize];

            for (int i = 0; i < total; i++)
            {
                int dataLength = packets[i].Client.Data.Length;
                Array.Copy(packets[i].Client.Data, 0, data, i * UdpPacketMaxSize, dataLength);
            }

            T res = Activator.CreateInstance<T>();
            res.Desirialize(data);
            return res;
        }

        private static Packet CreatePacket(int id, int count, PacketType type)
        {
            return new Packet()
            {
                Id = id,
                Type = type,
                Count = count
            };
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                _socket.Dispose();
            }
        }

    }
}