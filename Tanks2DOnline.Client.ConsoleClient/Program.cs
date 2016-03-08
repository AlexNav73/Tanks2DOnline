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
using Tanks2DOnline.Tests.Tests.TestEntities;

namespace Tanks2DOnline.Client.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Tanks2DOnline.Core.Net.UdpClient socket = new Tanks2DOnline.Core.Net.UdpClient(null))
            {
                socket.SetRemote(IPAddress.Parse("127.0.0.1"));
                Serializable obj = new Serializable();
                obj.Init();

                socket.Send(obj, PacketType.HoldsData);

                Console.WriteLine("Object is sended");
            }
        }
    }
}
