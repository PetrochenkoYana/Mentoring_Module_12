using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using NorthwindLibrary;
using StackExchange.Redis;

namespace CachingSolutionsSamples
{
    class EntitiesRedisCache<T> : IEntitiesCache<T>
    {
        private ConnectionMultiplexer redisConnection;
        string prefix = "Cache_Categories";

        DataContractSerializer serializer = new DataContractSerializer(
            typeof(IEnumerable<T>));

        public EntitiesRedisCache(string hostName)
        {
            redisConnection = ConnectionMultiplexer.Connect(hostName);
        }

        public IEnumerable<T> Get(string key)
        {
            var db = redisConnection.GetDatabase();
            byte[] s = db.StringGet(prefix + key);
            if (s == null)
                return null;

            return (IEnumerable<T>)serializer
                .ReadObject(new MemoryStream(s));

        }

        public void Set(string key, IEnumerable<T> entities, DateTimeOffset dateTime)
        {
            var db = redisConnection.GetDatabase();
            var cacheKey = prefix + key;

            if (entities == null)
            {
                db.StringSet(cacheKey, RedisValue.Null);
            }
            else
            {
                var stream = new MemoryStream();
                serializer.WriteObject(stream, entities);
                db.StringSet(cacheKey, stream.ToArray(), TimeSpan.FromMilliseconds((DateTimeOffset.Now - dateTime).TotalMilliseconds));
            }
        }
    }
}