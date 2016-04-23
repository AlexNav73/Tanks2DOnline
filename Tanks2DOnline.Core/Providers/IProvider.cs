using System.Collections.Generic;

namespace Tanks2DOnline.Core.Providers
{
    public interface IProvider<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        TValue Get(TKey key);
    }
}