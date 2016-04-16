using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Xml;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.DataTransfer.Base;
using Tanks2DOnline.Core.Net.DataTransfer.Interfaces;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Core.Net.DataTransfer.Scenario
{
    public class UdpStream : PacketTransferWithApproval, IDataTransferer
    {
        public UdpStream(Socket socket) : base(socket) { }

        public void Send<T>(EndPoint remote, T obj, PacketType type) where T : SerializableObjectBase
        {
            Task.Factory.StartNew(() =>
            {
                foreach (var packet in DataHelper.SplitToPackets(obj, type))
                {
                    Send(packet, remote);
                    LogManager.Debug("Send: Packet with id {0} sended!", packet.Id);
                }
            });
        }

        public void Recv<T>(ref EndPoint remote, Action<T> callback) where T : SerializableObjectBase
        {
            var first = Recv(ref remote);
            var packets = new List<Packet.Packet>(first.Count);

            for (int i = 0; i < first.Count; i++)
                packets.Add(Recv(ref remote));

            Task.Factory.StartNew(() =>
            {
                callback(DataHelper.ExtractData<T>(packets));
            });
        }

    }
}