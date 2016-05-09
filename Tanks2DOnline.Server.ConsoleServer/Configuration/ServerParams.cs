using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Factory;
using Tanks2DOnline.Core.Providers;

namespace Tanks2DOnline.Server.ConsoleServer.Configuration
{
    public class ServerParams : Params
    {
        public const string Port = "Port";
        public const string SendBigDataDelay = "SendBigDataDelay";

        public ServerParams(IProvider<string, object> provider) : base(provider)
        {
            LoadValue(Port);
            LoadValue(SendBigDataDelay);
        }
    }
}
