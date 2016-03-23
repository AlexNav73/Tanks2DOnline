using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.CommonData;

namespace Tanks2DOnline.Core.Net.DataTransfer.Scenario
{
    public class UdpDatagrams : Base.PacketTransferBase
    {
        public UdpDatagrams(Socket socket) : base(socket) { }

        public override void Send<T>(T item, PacketType type)
        {
            Task.Factory.StartNew(() =>
            {
                var packets = DataHelper.SplitToPackets(item, type).ToArray();
                if (packets.Length == 1)
                {
                    Send(packets[0]);
                    LogManager.Debug("Packet sended");
                }
                else LogManager.Debug("Object size was to big to be send it with 1 packet");
            });
        }

        public override T Recv<T>()
        {
            var task = Task.Factory.StartNew(() =>
            {
                var packet = Recv();
                LogManager.Debug("Packet with type {0} received", packet.Type);
                return packet.Type == PacketType.HoldsData
                    ? DataHelper.ExtractData<T>(new List<Packet>() {packet})
                    : null;
            });

            return task.Result;
        }
    }
}
