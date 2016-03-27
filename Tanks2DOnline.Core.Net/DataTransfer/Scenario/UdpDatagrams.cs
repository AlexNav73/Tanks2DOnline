using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.Packet;

namespace Tanks2DOnline.Core.Net.DataTransfer.Scenario
{
    public class UdpDatagrams : Base.PacketTransferBase
    {
        public UdpDatagrams(Socket socket) : base(socket) { }

        public override void Send<T>(string userName, T item, PacketType type)
        {
            Task.Factory.StartNew(() =>
            {
                var packets = DataHelper.SplitToPackets(item, type).ToArray();
                if (packets.Length == 1)
                {
                    var packet = packets[0];
                    packet.UserName = userName;
                    Send(packet);
                    LogManager.Debug("Packet sended");
                }
                else LogManager.Debug("Object size was to big to be send it with 1 packet");
            });
        }

        public override void Recv<T>(OnTransmitionComplete<T> callback)
        {
            Task.Factory.StartNew(() =>
            {
                var packet = Recv();
                LogManager.Debug("Packet with type {0} received", packet.Type);
                callback(packet.UserName, packet.Type == PacketType.HoldsData
                    ? DataHelper.ExtractData<T>(new List<Packet.Packet>() { packet })
                    : null);
            });
        }
    }
}
