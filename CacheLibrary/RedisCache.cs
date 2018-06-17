using System;
using System.IO;
using System.Runtime.Serialization;
using CachePolicyService;
using StackExchange.Redis;

namespace CacheLibrary
{
    public class RedisCache<T> : ICache<T>
    {
        private ConnectionMultiplexer redisConnection;
        string prefix = "Cache_by_";

        DataContractSerializer serializer = new DataContractSerializer(
            typeof(T));

        public ICachePolicyService CachePolicy { get; set; }

        public RedisCache(string hostName, ICachePolicyService cachePolicyService)
        {
            redisConnection = ConnectionMultiplexer.Connect(hostName);
            CachePolicy = cachePolicyService;
        }

        public T Get(string key)
        {

            var db = redisConnection.GetDatabase();
            byte[] s = db.StringGet(prefix + key);
            if (s == null)
                return default(T);

            return (T)serializer
                .ReadObject(new MemoryStream(s));

        }

        public void Set(string key, T entities)
        {
            var db = redisConnection.GetDatabase();
            var cacheKey = prefix + key;

            if (entities == null)
            {
                db.StringSet(cacheKey, RedisValue.Null, CachePolicy.GetCachePolicy().AbsoluteExpiration - DateTime.Now);
            }
            else
            {
                var stream = new MemoryStream();
                serializer.WriteObject(stream, entities);
                db.StringSet(cacheKey, stream.ToArray(), CachePolicy.GetCachePolicy().AbsoluteExpiration - DateTime.Now);
            }
        }
    }
}