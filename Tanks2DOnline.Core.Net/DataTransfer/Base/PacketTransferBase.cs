using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Core.Net.DataTransfer.Base
{
    public abstract class PacketTransferBase : UdpSocket
    {
        public int Port { get; set; }

        protected PacketTransferBase(Socket socket) : base(socket)
        {
            Port = 4242;
        }

        /// <summary>
        /// Synchronously send packet to remote ip
        /// </summary>
        /// <returns></returns>
        public virtual void Send(Packet.Packet packet, EndPoint remote)
        {
            SendPacket(packet, remote);
        }

        /// <summary>
        /// Synchronously receive packet from remote ip
        /// </summary>
        /// <returns></returns>
        public virtual Packet.Packet Recv(ref EndPoint remote)
        {
            return RecvPacket(ref remote);
        }
    }
}
