using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Data;
using Tanks2DOnline.Core.Factory;
using Tanks2DOnline.Core.Logging;
using Tanks2DOnline.Core.Net;
using Tanks2DOnline.Core.Net.DataTransfer;
using Tanks2DOnline.Core.Net.DataTransfer.Base;
using Tanks2DOnline.Core.Net.Packet;
using Tanks2DOnline.Core.Net.TestObjects;
using Tanks2DOnline.Core.Providers.Implementations;
using Tanks2DOnline.Core.Serialization;
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

            var factory = new ConfigurationFactory(paramsProvider, configProvider);

            using(var server = new Server(factory.Create<ServerConfiguration>()))
                server.Listen();
        }
    }
}
