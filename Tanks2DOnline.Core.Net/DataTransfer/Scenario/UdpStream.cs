using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Xml;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.DataTransfer.Base;
using Tanks2DOnline.Core.Net.Packet;

namespace Tanks2DOnline.Core.Net.DataTransfer.Scenario
{
    public class UdpStream : PacketTransferWithApproval
    {
        private readonly Dictionary<string, List<Packet.Packet>> _packets = 
            new Dictionary<string, List<Packet.Packet>>();

        public UdpStream(Socket socket) : base(socket)
        {
        }

        public override void Send<T>(string userName, T item, PacketType type)
        {
            Task.Factory.StartNew(() =>
            {
                foreach (var packet in DataHelper.SplitToPackets(item, type))
                {
                    packet.UserName = userName;
                    Send(packet);
                    LogManager.Debug("Send: Packet with id {0} sended!", packet.Id);
                }
            });
        }

        public override void Recv<T>(OnTransmitionComplete<T> callback)
        {
            Task.Factory.StartNew(() =>
            {
                var first = Recv();
                AddPacketToUser(first);

                while (!IsAllPacketsReceived())
                {
                    AddPacketToUser(Recv());
                }

                string userName;
                if (IsPacketSeriaComplete(out userName))
                {
                    callback(userName, DataHelper.ExtractData<T>(_packets[userName]));
                    _packets[userName].Clear();
                }
            });
        }

        private bool IsAllPacketsReceived()
        {
            return _packets.All(v =>
            {
                var packet = v.Value.FirstOrDefault();
                if (packet != null)
                    return packet.Count == v.Value.Count;
                return false;
            });
        }

        private bool IsPacketSeriaComplete(out string userName)
        {
            string user = null;

            bool res = _packets.Any(v =>
            {
                var packet = v.Value.FirstOrDefault();
                if (packet != null)
                {
                    var count = packet.Count;
                    if (v.Value.Count == count)
                    {
                        user = v.Key;
                        return true;
                    }
                }
                return false;
            });

            userName = user;
            return res;
        }

        private Packet.Packet AddPacketToUser(Packet.Packet packet)
        {
            if (_packets.ContainsKey(packet.UserName))
            {
                _packets[packet.UserName].Add(packet);
            }
            else
            {
                var packets = new List<Packet.Packet> {packet};
                _packets.Add(packet.UserName, packets);
            }

            return packet;
        }
    }
}