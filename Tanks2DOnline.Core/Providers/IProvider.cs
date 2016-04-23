namespace Tanks2DOnline.Core.Providers
{
    public interface IProvider<in TKey, out TValue>
    {
        TValue Get(TKey key);
        void Init(object collection);
    }
}