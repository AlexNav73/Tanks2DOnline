using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Factory.Interfaces;
using Tanks2DOnline.Core.Providers;
using Tanks2DOnline.Server.ConsoleServer.Configuration.Creators;

namespace Tanks2DOnline.Server.ConsoleServer.Configuration.Providers
{
    public class ConfigurationProvider : IProvider<Type, ICreator>
    {
        private readonly Dictionary<Type, ICreator> _creators = new Dictionary<Type, ICreator>()
        {
            {typeof(ServerConfiguration), new ServerConfigCreator()}
        };

        public IEnumerator<KeyValuePair<Type, ICreator>> GetEnumerator()
        {
            return _creators.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public ICreator Get(Type key)
        {
            return _creators.ContainsKey(key) ? _creators[key] : null;
        }
    }
}
