using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.DataTransfer.Base;
using Tanks2DOnline.Core.Net.Packet;

namespace Tanks2DOnline.Core.Net.DataTransfer.Scenario
{
    public class UdpStream : PacketTransferWithApproval
    {
        public UdpStream(Socket socket) : base(socket) { }

        public override void Send<T>(T item, PacketType type)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (var packet in DataHelper.SplitToPackets(item, type))
                {
                    Send(packet);
                    LogManager.Debug("Send: Packet with id {0} sended!", packet.Id);
                }
            });
        }

        public override T Recv<T>()
        {
            var task = Task.Factory.StartNew(() =>
            {
                var packets = new List<Packet.Packet>();

                var first = Recv();
                packets.Add(first);
                LogManager.Debug("Packet {0} received", packets.Last().Id);

                for(int i = 0; i < first.Count - 1; i++)
                {
                    packets.Add(Recv());
                    LogManager.Debug("Packet {0} received", packets.Last().Id);
                }

                return first.Count == packets.Count ? DataHelper.ExtractData<T>(packets) : null;
            });

            return task.Result;
        }
    }
}