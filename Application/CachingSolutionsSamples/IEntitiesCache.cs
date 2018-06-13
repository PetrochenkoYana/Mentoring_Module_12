using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CachingSolutionsSamples
{
    public interface IEntitiesCache<T>
    {
        IEnumerable<T> Get(string key);
        void Set(string key, IEnumerable<T> entities,DateTimeOffset dateTime);
    }
}
