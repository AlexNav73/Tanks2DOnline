using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.CommonData;
using Tanks2DOnline.Core.Net.DataTransfer.Base;
using Tanks2DOnline.Core.Net.Serialization;

namespace Tanks2DOnline.Core.Net.DataTransfer
{
    public class UdpClient : PacketTransferWithApproval
    {
        public UdpClient(IPAddress ipAddress) : base(ipAddress) { }

        public void Send<T>(T item, PacketType type) where T: SerializableObjectBase
        {
            Task.Factory.StartNew(() =>
            {
                foreach (var packet in DataHelper.SplitToPackets(item, type))
                {
                    Send(packet);
                }
            });
        }

        public T Recv<T>() where T : SerializableObjectBase
        {
            var task = Task.Factory.StartNew(() =>
            {
                var packets = new List<Packet>();

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