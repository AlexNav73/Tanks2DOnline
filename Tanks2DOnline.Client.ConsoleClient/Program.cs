using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net;
using Tanks2DOnline.Core.Net.CommonData;

namespace Tanks2DOnline.Client.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (UdpSocket socket = new UdpSocket())
            {
                Packet packet = new Packet
                {
                    Type = PacketType.HoldsData,
                    Client = new ClientInfo()
                    {
                        Data = Enumerable.Repeat<byte>(255, 1323).ToArray(),
                        Name = "Hello"
                    }
                };

                var ipAddress = IPAddress.Parse("127.0.0.1");
                EndPoint to = new IPEndPoint(ipAddress, 4242);
                Console.WriteLine("Packet sended: {0}", socket.SendPacket(packet, ref to));
            }
        }
    }
}
