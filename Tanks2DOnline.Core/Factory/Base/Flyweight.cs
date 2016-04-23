using System.Collections.Generic;
using Tanks2DOnline.Core.Providers;

namespace Tanks2DOnline.Core.Factory.Base
{
    public abstract class Flyweight<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _maps = new Dictionary<TKey, TValue>();

        protected TValue this[TKey key]
        {
            get { return GetValue(key); }
            set { Add(key, value); }
        }

        protected abstract void LoadValues(IProvider<TKey, TValue> provider);

        protected virtual TValue GetValue(TKey key)
        {
            return _maps.ContainsKey(key) ? _maps[key] : default(TValue);
        }

        protected virtual void Add(TKey key, TValue value)
        {
            if (!_maps.ContainsKey(key))
                _maps.Add(key, value);
        }
    }
}