using System;
using System.Configuration;
using Tanks2DOnline.Client.ConsoleClient.Configuration;
using Tanks2DOnline.Client.ConsoleClient.Configuration.Providers;
using Tanks2DOnline.Core.Factory;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net.TestObjects;
using Tanks2DOnline.Core.Providers.Implementations;

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

            LogManager.Debug("Address is : 127.0.0.1:{0}", clientConfig.Port);

            var client = new Client(clientConfig);

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
