using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.DataTransfer.Interfaces;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Core.Net.DataTransfer.Scenario
{
    public class UdpDatagrams : Base.SimplePasketTransfer, IDataTransferer
    {
        public UdpDatagrams(Socket socket) : base(socket) { }

        public void Send<T>(EndPoint remote, T obj, PacketType type) where T : SerializableObjectBase
        {
            Task.Factory.StartNew(() =>
            {
                var packets = DataHelper.SplitToPackets(obj, type).ToArray();
                if (packets.Length == 1)
                {
                    var packet = packets[0];
                    Send(packet, remote);
                    LogManager.Debug("Packet with id {0} sended", packet.Id);
                }
                else LogManager.Debug("Object size was to big to be send it with 1 packet");
            });
        }

        public void Recv<T>(ref EndPoint remote, Action<T> callback) where T : SerializableObjectBase
        {
            var packet = Recv(ref remote);
            LogManager.Debug("Packet with type {0} received", packet.Type);

            Task.Factory.StartNew(() =>
            {
                callback(packet.Type == PacketType.Data
                    ? DataHelper.ExtractData<T>(packet)
                    : null);
            });
        }
    }
}
