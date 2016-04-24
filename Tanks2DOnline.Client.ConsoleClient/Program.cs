using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Tanks2DOnline.Client.ConsoleClient.Configuration;
using Tanks2DOnline.Client.ConsoleClient.Configuration.Providers;
using Tanks2DOnline.Client.ConsoleClient.Handles.Implementations;
using Tanks2DOnline.Core.Data;
using Tanks2DOnline.Core.Factory;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net;
using Tanks2DOnline.Core.Net.DataTransfer;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Net.TestObjects;
using Tanks2DOnline.Core.Providers.Implementations;
using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Client.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var rand = new Random();

            var paramsProvider = new AppConfigProvider(ConfigurationManager.AppSettings);
            var configProvider = new ConfigurationProvider();
            var configFactory = new ConfigurationFactory(paramsProvider, configProvider);
            var clientConfig = configFactory.Create<ClientConfiguration>();
            clientConfig.Port = rand.Next()%1000 + 25000;
            var client = new Client(clientConfig, new[] {new SmallObjectProcessHandle()});

            client.Start(Console.ReadLine(), () =>
            {
                var small = SmallTestObject.Create();
                while (true)
                {
                    small.Message = Console.ReadKey().KeyChar.ToString();
                    client.SendObject(small);
                }
            });
        }

        private void DoWork()
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
