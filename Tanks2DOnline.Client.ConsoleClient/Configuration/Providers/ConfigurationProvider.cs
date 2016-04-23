using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Client.ConsoleClient.Configuration.Creators;
using Tanks2DOnline.Core.Factory.Interfaces;
using Tanks2DOnline.Core.Providers;

namespace Tanks2DOnline.Client.ConsoleClient.Configuration.Providers
{
    public class ConfigurationProvider : IProvider<Type, ICreator>
    {
        private readonly Dictionary<Type, ICreator> _creators = new Dictionary<Type, ICreator>()
        {
            {typeof (ClientConfiguration), new ClientConfigurationCreator()}
        };

        public ICreator Get(Type key)
        {
            return _creators.ContainsKey(key) ? _creators[key] : null;
        }

        public IEnumerator<KeyValuePair<Type, ICreator>> GetEnumerator()
        {
            return _creators.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
