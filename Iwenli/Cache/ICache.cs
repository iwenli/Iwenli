using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Cache
{
    public interface ICache<K, V>
    {
        V Get<V>(K cacheKey, Func<K, object, V> getUncachedValue);

        V Get<V>(K cacheKey, Func<K, V> getUncachedValue, DateTimeOffset dateTimeOffset);

        V Get<V>(K cacheKey, Func<K, V> getUncachedValue, CacheItemPolicy policy);

        void Set(K cacheKey, V cacheValue, DateTimeOffset dateTimeOffset);

        void Set(K cacheKey, V cacheValue, CacheItemPolicy policy);

        void Remove(K cacheKey);
    }
}
