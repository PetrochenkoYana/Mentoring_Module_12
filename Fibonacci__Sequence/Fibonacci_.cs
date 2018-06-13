using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fibonacci__Sequence
{
    public interface IFibonacci_Sequence
    {
        int GetByIndex(int index);
    }

    public class Fibonacci : IFibonacci_Sequence
    {
        public ICache Cache { get; private set; }

        public Fibonacci(ICache cache)
        {
            Cache = cache;
        }
        public int GetByIndex(int index)
        {
            if (index < 0)
            {
                throw new ArgumentException("Index should be equal or more than 0");
            }

            var number = Cache.Get(index.ToString());
            if (number != default(int))
            {
                return number;
            }

            switch (index)
            {
                case 0:
                    return 0;
                case 1:
                case 2:
                    return 1;
                default:
                    var num = GetByIndex((index - 2)) + GetByIndex(index - 1);
                    Cache.Set(index.ToString(), num);
                    return num;
            }
        }
    }
}
