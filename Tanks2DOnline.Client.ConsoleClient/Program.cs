using System;
using System.Collections.Generic;
using System.Configuration;
using Tanks2DOnline.Client.ConsoleClient.Actions;
using Tanks2DOnline.Client.ConsoleClient.Configuration;
using Tanks2DOnline.Client.ConsoleClient.Configuration.Providers;
using Tanks2DOnline.Client.ConsoleClient.Handles;
using Tanks2DOnline.Core.Factory;
using Tanks2DOnline.Core.Net.Handle.Builder;
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
            var configFactory = new ConfigurationFactory(new ClientParams(paramsProvider), configProvider);
            var clientConfig = configFactory.Create<ClientConfiguration>();
            clientConfig.Port = rand.Next()%1000 + 25000;

            var builder = new PacketManagerBuilder();

            builder.AddAction(PacketType.Data, new DataTypeParallelAction())
                .AddHandle(DataType.State, new SmallObjectProcessHandle());
            builder.AddAction(PacketType.LogOn, new LogOnAction());

            var client = new Client(clientConfig, builder);

            client.Start(Console.ReadLine(), () =>
            {
                var small = SmallTestObject.Create();
                while (true)
                {
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Enter)
                        break;

                    small.Message = key.KeyChar.ToString();
                    client.SendObject(small);
                }
            });
        }
    }
}
