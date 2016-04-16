using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Data;
using Tanks2DOnline.Core.Net;
using Tanks2DOnline.Core.Net.DataTransfer;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Net.TestObjects;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Client.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var manager = new DataTransferManager(null))
            {
                var big = new BigTestObject();
                var small = SmallTestObject.Create();
                var packet = new Packet() {Type = PacketType.Registration, Data = Encoding.ASCII.GetBytes(Console.ReadLine())};

                Console.WriteLine("Press Enter to send object ...");
                Console.ReadKey();
                
                var remote = (EndPoint)new IPEndPoint(IPAddress.Loopback, 4242);

                manager.SendData(remote, packet, PacketType.Registration);
                while (true)
                {
//                    manager.SendData(remote, big, PacketType.SmallData);
                    manager.SendData(remote, small, PacketType.SmallData);
//                    manager.RecvData(ref remote, (SmallTestObject v) => Console.WriteLine(v.Message));
//                    manager.SendData(userName, new FileData(userName + ".txt"), PacketType.HoldsData);
                }
            }
        }
    }
}
