using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Net;
using Tanks2DOnline.Core.Net.CommonData;
using Tanks2DOnline.Core.Net.DataTransfer;
using Tanks2DOnline.Tests.Tests.TestEntities;

namespace Tanks2DOnline.Server.ConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (UdpClient socket = new UdpClient(IPAddress.Any))
            {
                socket.SetRemote(IPAddress.Any);

                var packet = socket.Recv<BigTestObject>();
//                var packet = socket.Recv<Serializable>();

                Console.WriteLine(packet.Message);
//                Console.WriteLine(packet.Inner.PropString);
                Console.ReadKey();
            }
        }

    }
}
