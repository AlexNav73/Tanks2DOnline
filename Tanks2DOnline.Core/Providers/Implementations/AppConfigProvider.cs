using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks2DOnline.Core.Providers.Implementations
{
    public class AppConfigProvider : IProvider<string, object>
    {
        private readonly NameValueCollection _collection = null;

        public AppConfigProvider(NameValueCollection collection)
        {
            Argument.IsNull(collection, "collection");

            _collection = collection;
        }

        public object Get(string key)
        {
            Argument.IsNull(key, "key");

            return _collection.AllKeys.Contains(key) ? _collection[key] : null;
        }

        public void Init(object collection) { }
    }
}
