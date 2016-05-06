using System.Configuration;
using Tanks2DOnline.Core.Factory;
using Tanks2DOnline.Core.Net.Handle.Builder;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Providers.Implementations;
using Tanks2DOnline.Server.ConsoleServer.Actions;
using Tanks2DOnline.Server.ConsoleServer.Configuration;
using Tanks2DOnline.Server.ConsoleServer.Configuration.Providers;

namespace Tanks2DOnline.Server.ConsoleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var paramsProvider = new AppConfigProvider(ConfigurationManager.AppSettings);
            var configProvider = new ConfigurationProvider();

            var factory = new ConfigurationFactory(new ServerParams(paramsProvider), configProvider);
            var builder = new PacketManagerBuilder();

            builder.AddAction(PacketType.LogOn, new RegisterPacketAction());

            using(var server = new Server(factory.Create<ServerConfiguration>(), builder))
                server.Listen();
        }
    }
}
