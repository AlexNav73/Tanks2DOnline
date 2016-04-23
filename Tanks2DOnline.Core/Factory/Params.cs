using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Factory.Base;
using Tanks2DOnline.Core.Providers;

namespace Tanks2DOnline.Core.Factory
{
    public sealed class Params : Flyweight<string, object>
    {
        public const string Port = "Port";
        public const string ServerIP = "ServerIP";
        public const string ServerPort = "ServerPort";
        public const string ServerRegistrationTimeout = "ServerRegistrationTimeout";

        public Params(IProvider<string, object> provider)
        {
            Argument.IsNull(provider, "provider");

            LoadValues(provider);
        }

        protected override void LoadValues(IProvider<string, object> provider)
        {
            this[Port] = provider.Get(Port);
            this[ServerIP] = provider.Get(ServerIP);
            this[ServerPort] = provider.Get(ServerPort);
            this[ServerRegistrationTimeout] = provider.Get(ServerRegistrationTimeout);
        }

        public TValue GetValue<TValue>(string key)
        {
            Argument.IsNull(key, "key");

            var val = this[key];
            return (TValue)Convert.ChangeType(val, typeof (TValue));
        }
    }
}
