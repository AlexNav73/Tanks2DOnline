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
            string userName = Console.ReadLine();
            using (var manager = new DataTransferManager(null, IPAddress.Loopback))
            {
                var big = new BigTestObject();
                var small = SmallTestObject.Create();

                while (true)
                {
                    manager.SendData(userName, big, PacketType.HoldsData);
//                    manager.SendData(userName, small, PacketType.HoldsData);
//                    manager.SendData(userName, new FileData(userName + ".txt"), PacketType.HoldsData);

                    Console.ReadKey();
                    Console.WriteLine("Object is sended");
                }
            }
        }
    }
}
