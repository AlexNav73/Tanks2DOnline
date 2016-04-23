using System;
using Tanks2DOnline.Core.Factory.Base;
using Tanks2DOnline.Core.Factory.Interfaces;
using Tanks2DOnline.Core.Providers;

namespace Tanks2DOnline.Core.Factory
{
    public sealed class ConfigurationFactory : Flyweight<Type, ICreator>
    {
        private readonly Params _prms = null;

        public ConfigurationFactory(IProvider<string, object> paramsProvider, IProvider<Type, ICreator> configProvider)
        {
            _prms = new Params(paramsProvider);
            LoadValues(configProvider);
        }

        public T Create<T>()
        {
            return (T)this[typeof (T)].Create(_prms);
        }

        protected override void LoadValues(IProvider<Type, ICreator> provider)
        {
            foreach (var pair in provider)
            {
                this[pair.Key] = pair.Value;
            }
        }
    }
}
