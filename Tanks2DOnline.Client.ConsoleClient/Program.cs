using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net;
using Tanks2DOnline.Core.Net.CommonData;
using Tanks2DOnline.Tests.Tests.TestEntities;
using UdpClient = Tanks2DOnline.Core.Net.DataTransfer.UdpClient;

namespace Tanks2DOnline.Client.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (UdpClient socket = new UdpClient(null))
            {
                socket.SetRemote(IPAddress.Parse("127.0.0.1"));

                Serializable obj = new Serializable();
                obj.Init();
                BigTestObject test = new BigTestObject();

                socket.Send(test, PacketType.HoldsData);

                Console.ReadKey();
                Console.WriteLine("Object is sended");
            }
        }
    }
}
