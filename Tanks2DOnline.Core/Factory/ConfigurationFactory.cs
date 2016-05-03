using System;
using Tanks2DOnline.Core.Factory.Base;
using Tanks2DOnline.Core.Factory.Interfaces;
using Tanks2DOnline.Core.Providers;

namespace Tanks2DOnline.Core.Factory
{
    public sealed class ConfigurationFactory : Flyweight<Type, ICreator>
    {
        private readonly Params _prms = null;

        public ConfigurationFactory(Params prms, IProvider<Type, ICreator> configProvider)
        {
            _prms = prms;
            foreach (var pair in configProvider)
            {
                this[pair.Key] = pair.Value;
            }
        }

        public T Create<T>()
        {
            return (T)this[typeof (T)].Create(_prms);
        }
    }
}
