using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Data;
using Tanks2DOnline.Core.Net;
using Tanks2DOnline.Core.Net.DataTransfer;
using Tanks2DOnline.Core.Net.TestObjects;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Server.ConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var manager = new DataTransferManager(IPAddress.Any, IPAddress.Any))
            {
                while (true)
                {
                    var big = manager.RecvData<BigTestObject>();
                    //                var file = manager.RecvData<FileData>();

                    Console.WriteLine("======================== Data ============================");
                    Console.WriteLine(big.Message);
                }
//                Console.ReadKey();
            }
        }

    }
}
