using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqApplication
{
    class Program
    {
        static void pooMain(string[] args)
        {
            int[] temp = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            var numbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 2, 4, 6, 8, 12, 2, 4, 6 };


            var results = temp.Select(x => x * x)
                .OrderByDescending(x => x)
                .Select(x => x.ToString())
                .ToList();
            Console.WriteLine("Square reverse:");
            results.ForEach(x => Console.WriteLine(x));
          
            var evenNumberHistogram = numbers
                .Where(x => x % 2 == 0)
                .GroupBy(x => x)
                .Select(x => new Tuple<int, int>(x.Key, x.ToList().Count))
                .OrderBy(x => x.Item1);
            Console.WriteLine("Even Historgram:");
            evenNumberHistogram.ToList().ForEach(x => Console.WriteLine(x.Item1 + " " + x.Item2));

            var primeNums = numbers
                .Where(x => isPrime(x))
                .ToList();
            Console.WriteLine("Primes:");
            primeNums.ForEach(x => Console.WriteLine(x));
            
            Console.Read();
        }

        static bool isPrime(int number)
        {
            for (int i = 2; i < number; i++)
            {
                if (number % i == 0 && i != number) return false;
            }
            return true;
        }
    }
}
