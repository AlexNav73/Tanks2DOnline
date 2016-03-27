using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Data;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net;
using Tanks2DOnline.Core.Net.DataTransfer;
using Tanks2DOnline.Core.Net.DataTransfer.Base;
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
                manager.RecvData<BigTestObject>(OnRecved);
//                manager.RecvData<SmallTestObject>(OnSmallRecved);
//                    manager.RecvData<FileData>(OnFileRecved);
                }
                Console.WriteLine("======================== Data ============================");
                Console.ReadKey();
            }
        }

        private static void OnSmallRecved<T>(string userName, T item)
        {
            var data = item as SmallTestObject;
            if (data != null)
                LogManager.Info(data.Message);
            else
                LogManager.Error("Object not transfered");
        }

        private static void OnRecved<T>(string userName, T item)
        {
            var data = item as BigTestObject;
            if (data != null)
                LogManager.Info(data.Message);
            else
                LogManager.Error("Object not transfered");
        }

        private static void OnFileRecved<T>(string userName, T item)
        {
            var data = item as FileData;
            if (data != null)
                LogManager.Info("File transfered");
            else
                LogManager.Error("File not transfered");
        }
    }
}
