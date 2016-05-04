using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.Packet;

namespace Tanks2DOnline.Core.Net.DataTransfer.Base
{
    public abstract class PacketTransferWithApproval
    {
        private readonly TimeSpan _timeout = new TimeSpan(0, 0, 0, 0, 50);
        private const int RetryCount = 10;

        protected PacketTransferWithApproval(Socket socket)
        {
        }

        public void Send(Packet.Packet packet, EndPoint remote)
        {
            int i = RetryCount;
            while (i-- > 0)
            {
//                base.Send(packet, remote);

                var task = Task.Factory.StartNew(() =>
                {
                    var responce = Recv(ref remote);
                    LogManager.Debug("Send: Packet with id {0} and type {1} received", responce.Id, responce.Type);
                    return responce.Type == PacketType.PacketAcceptRequest;
                });

                if (!task.Wait(_timeout))
                {
                    LogManager.Debug("Send: Interval elapsed");
                    continue;
                }
                if (task.Result) return;
            }
        }

        public Packet.Packet Recv(ref EndPoint remote)
        {
//            var packet = base.Recv(ref remote);
//            LogManager.Debug("Recv: Packet with id {0} and type {1} received", packet.Id, packet.Type);
//            var responce = new Packet.Packet(packet.Id, 0, PacketType.PacketAcceptRequest) {UserName = "Server"};
//            base.Send(responce, remote);
//            LogManager.Debug("Recv: Packet approval sended");
//            return packet;
            return null;
        }
    }
}
