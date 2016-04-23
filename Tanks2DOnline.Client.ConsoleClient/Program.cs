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
using Tanks2DOnline.Core.Logging;
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
            var rand = new Random();
            using (var manager = new DataTransferManager(IPAddress.Loopback, rand.Next() % 1000 + 25000))
            {
                var small = SmallTestObject.Create();
                var packet = new Packet() {Type = PacketType.Registration, Data = Encoding.ASCII.GetBytes(Console.ReadLine())};
                
                var serverSocket = (EndPoint)new IPEndPoint(IPAddress.Loopback, 4242);
                var remote = (EndPoint)new IPEndPoint(IPAddress.Loopback, 4242);

                manager.SendData(serverSocket, packet, PacketType.Registration);
                var task = Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        small.Message = Console.ReadKey().KeyChar + "";
                        Console.WriteLine();
                        manager.SendData(serverSocket, small, PacketType.SmallData);
                    }
                });
                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        manager.RecvData(ref remote, (SmallTestObject o) => LogManager.Info(o.Message));
                    }
                });

                task.Wait();
            }
        }
    }
}
