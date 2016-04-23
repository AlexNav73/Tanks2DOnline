using System;
using Tanks2DOnline.Core.Factory;
using Tanks2DOnline.Core.Factory.Base;
using Tanks2DOnline.Core.Factory.Interfaces;
using Tanks2DOnline.Core.Providers;
using Tanks2DOnline.Server.ConsoleServer.Configuration.Creators;

namespace Tanks2DOnline.Server.ConsoleServer.Configuration
{
    public class ConfigurationFactory : Flyweight<Type, ICreator>
    {
        private readonly Params _prms = null;

        public ConfigurationFactory(IProvider<string, object> provider)
        {
            _prms = new Params(provider);
        }

        public T Create<T>()
        {
            return (T)this[typeof (T)].Create(_prms);
        }

        protected override void LoadValues(IProvider<Type, ICreator> provider)
        {
            this[typeof(ServerConfiguration)] = new ServerConfigCreator();
        }
    }
}
