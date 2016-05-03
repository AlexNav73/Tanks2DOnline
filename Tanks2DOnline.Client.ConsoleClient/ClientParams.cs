using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Factory;
using Tanks2DOnline.Core.Providers;

namespace Tanks2DOnline.Client.ConsoleClient
{
    public class ClientParams : Params
    {
        public const string Port = "Port";
        public const string ServerIP = "ServerIP";
        public const string ServerPort = "ServerPort";
        public const string ServerRegistrationTimeout = "ServerRegistrationTimeout";

        public ClientParams(IProvider<string, object> provider) : base(provider)
        {
            LoadValue(Port);
            LoadValue(ServerIP);
            LoadValue(ServerPort);
            LoadValue(ServerRegistrationTimeout);
        }
    }
}
