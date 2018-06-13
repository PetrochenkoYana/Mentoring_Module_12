using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Fibonacci__Sequence
{
    public class RedisCache : ICache
    {
        private ConnectionMultiplexer redisConnection;
        DataContractSerializer serializer = new DataContractSerializer(
            typeof(int));

        public RedisCache(string hostName)
        {
            redisConnection = ConnectionMultiplexer.Connect(hostName);
        }

        public int Get(string key)
        {
            var db = redisConnection.GetDatabase();
            byte[] s = db.StringGet(key);
            if (s == null)
                return default(int);

            return (int)serializer
                .ReadObject(new MemoryStream(s));
        }

        public void Set(string key, int number)
        {
            var db = redisConnection.GetDatabase();
            var stream = new MemoryStream();
            serializer.WriteObject(stream, number);
            db.StringSet(key, stream.ToArray());

        }
    }
}

