using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Iwenli.Cache
{
    /// <summary>
    /// 基于内存的缓存管理类的实现
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class MemoryCache<K, V> : ICache<K, V>
    {
        /// <summary>
        /// 返回内存缓存实现的单例模式
        /// </summary>
        public static MemoryCache<K, V> Instance
        {
            get
            {
                return SingletonHelper<MemoryCache<K, V>>.Instance;
            }
        }

        #region 构造函数

        private ObjectCache _memoryCache;

        public MemoryCache(): this(null)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        public MemoryCache(string name)
        {
            _memoryCache = new MemoryCache(string.Format("{0}-{1}-{2}", typeof(K).Name, typeof(V).Name, name));
        }
        #endregion


        /// <summary>
        /// 从缓存中提取对象，如果没有缓存对象，则通过函数[getUncachedValue]提取
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="getUncachedValue"></param>
        /// <returns></returns>
        public V Get<V>(K cacheKey, Func<K, object, V> getUncachedValue)
        {
            if (_memoryCache.Contains(ParseKey(cacheKey)))
            {
                return (V)_memoryCache[ParseKey(cacheKey)];
            }
            else
            {
                V _value = getUncachedValue(cacheKey, this);
                return _value;
            }
        }

        /// <summary>
        /// 从缓存中提取对象，如果没有缓存对象，则通过函数[getUncachedValue]提取，并且根据缓存策略缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="getUncachedValue"></param>
        /// <param name="dateTimeOffset"></param>
        /// <returns></returns>
        public V Get<V>(K cacheKey, Func<K, V> getUncachedValue, DateTimeOffset dateTimeOffset)
        {
            if (_memoryCache.Contains(ParseKey(cacheKey)))
            {
                return (V)_memoryCache[ParseKey(cacheKey)];
            }
            else
            {
                V _value = getUncachedValue(cacheKey);
                _memoryCache.Set(ParseKey(cacheKey), _value, dateTimeOffset);
                return _value;
            }
        }

        /// <summary>
        /// 从缓存中提取对象，如果没有缓存对象，则通过函数[getUncachedValue]提取，并且根据缓存策略缓存
        /// </summary>
        /// <typeparam name="V"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="getUncachedValue"></param>
        /// <param name="policy"></param>
        /// <returns></returns>
        public V Get<V>(K cacheKey, Func<K, V> getUncachedValue, CacheItemPolicy policy)
        {
            if (_memoryCache.Contains(ParseKey(cacheKey)))
            {
                return (V)_memoryCache[ParseKey(cacheKey)];
            }
            else
            {
                V _value = getUncachedValue(cacheKey);
                _memoryCache.Set(ParseKey(cacheKey), _value, policy);
                return _value;
            }
        }


        #region 添加缓存
        /// <summary>
        /// 向缓存中插入缓存项，同时指定基于时间的过期详细信息。
        /// </summary>
        /// <param name="key">该缓存项的唯一标识</param>
        /// <param name="value">要插入的对象</param>
        /// <param name="dateTimeOffset">缓存项的固定的过期日期和时间</param>
        public void Set(K key, V value, DateTimeOffset dateTimeOffset)
        {
            if (!_memoryCache.Contains(ParseKey(key)))
            {
                _memoryCache.Set(ParseKey(key), value, dateTimeOffset);
            }
        }
        /// <summary>
        /// 向缓存中插入缓存项。
        /// </summary>
        /// <param name="key">该缓存项的唯一标识符。</param>
        /// <param name="value">要插入的对象。</param>
        /// <param name="policy"> 一个包含该缓存项的逐出详细信息的对象。 此对象提供比简单绝对过期更多的逐出选项。</param>
        public void Set(K key, V value, CacheItemPolicy policy)
        {
            if (!_memoryCache.Contains(ParseKey(key)))
            {
                _memoryCache.Set(ParseKey(key), value, policy);
            }
        } 
        #endregion


        public void Remove(K cacheKey)
        {
            _memoryCache.Remove(ParseKey(cacheKey));
        }

        private string ParseKey(K key)
        {
            return key.GetHashCode().ToString();
        }

    }
}
