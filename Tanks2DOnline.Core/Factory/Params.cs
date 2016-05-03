using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tanks2DOnline.Core.Factory.Base;
using Tanks2DOnline.Core.Providers;

namespace Tanks2DOnline.Core.Factory
{
    public abstract class Params : Flyweight<string, object>
    {
        protected IProvider<string, object> Provider;

        protected void LoadValue(string key)
        {
            this[key] = Provider.Get(key);
        }

        protected Params(IProvider<string, object> provider)
        {
            Argument.IsNull(provider, "provider");
            Provider = provider;
        }

        public TValue GetValue<TValue>(string key)
        {
            Argument.IsNull(key, "key");

            var val = this[key];
            return (TValue)Convert.ChangeType(val, typeof (TValue));
        }
    }
}
