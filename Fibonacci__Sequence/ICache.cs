using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fibonacci__Sequence
{
    public interface ICache
    {
        int Get(string key);
        void Set(string key, int number);
    }
}
