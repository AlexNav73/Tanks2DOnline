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
using Tanks2DOnline.Core.Net.DataTransfer;
using Tanks2DOnline.Tests.Tests.TestEntities;

namespace Tanks2DOnline.Client.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var manager = new DataTransferManager(null, IPAddress.Loopback))
            {
                Serializable obj = new Serializable();
                obj.Init();
                BigTestObject test = new BigTestObject();

                manager.SendData(DataSize.Small, obj, PacketType.HoldsData);
                manager.SendData(DataSize.Big, test, PacketType.HoldsData);

                Console.ReadKey();
                Console.WriteLine("Object is sended");
            }
        }
    }
}
