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
            using (var manager = new DataTransferManager(IPAddress.Any, IPAddress.Any))
            {
                var small = manager.RecvData<Serializable>(DataSize.Small);
                var big = manager.RecvData<BigTestObject>(DataSize.Big);

                Console.WriteLine("======================== Data ============================");
                Console.WriteLine(big.Message);
                Console.WriteLine(small.Inner.PropString);
                Console.ReadKey();
            }
        }

    }
}
