using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Fibonacci_Sequence
{
    public interface IFibonacci_Sequence
    {
        int[] GetByNumberOfValues(int number);
        int[] GetUpToCertainValue(int value);
    }
    public class Fibonacci_Sequence : IFibonacci_Sequence
    {
        public int[] Sequence { get; set; }

        public Fibonacci_Sequence()
        {
            Sequence = new int[10];
        }
        public int[] GetByNumberOfValues(int number)
        {
            throw new NotImplementedException();
        }

        public int[] GetUpToCertainValue(int value)
        {
            throw new NotImplementedException();
        }
    }
}
