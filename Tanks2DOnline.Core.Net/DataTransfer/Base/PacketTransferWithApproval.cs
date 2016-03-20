using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.CommonData;

namespace Tanks2DOnline.Core.Net.DataTransfer.Base
{
    public abstract class PacketTransferWithApproval : PacketTransferBase
    {
        private readonly TimeSpan _timeout = new TimeSpan(0, 0, 0, 0, 50);
        private const int RetryCount = 10;

        protected PacketTransferWithApproval(IPAddress ipAddress) : base(ipAddress)
        {
        }

        protected override void Send(Packet packet)
        {
            int i = RetryCount;
            while (i-- > 0)
            {
                base.Send(packet);
                LogManager.Debug("Send: Packet with id {0} sended!", packet.Id);

                var task = Task.Factory.StartNew(() =>
                {
                    var responce = base.Recv();
                    LogManager.Debug("Send: Packet with id {0} and type {1} recved", responce.Id, responce.Type);
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

        protected override Packet Recv()
        {
            var packet = base.Recv();
            LogManager.Debug("Recv: Packet with id {0} and type {1} received", packet.Id, packet.Type);
            base.Send(new Packet(packet.Id, 0, PacketType.PacketAcceptRequest));
            LogManager.Debug("Recv: Packet approval sended");
            return packet;
        }
    }
}
