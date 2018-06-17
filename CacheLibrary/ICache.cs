using CachePolicyService;

namespace CacheLibrary
{
    public interface ICache<T>
    {
        ICachePolicyService CachePolicy { get; set; }
        T Get(string key);
        void Set(string key, T entities);
    }
}
