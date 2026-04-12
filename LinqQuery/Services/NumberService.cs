using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqQuery.Interfaces;


namespace LinqQuery.Services
{
    public class NumberService : INumberService
    {
        // Generates numbers from 1 to N
        public List<int> GenerateNumbers(int number)
        {
            return Enumerable.Range(1, number).ToList();
        }

        // Retrieves Even Numbers
        public List<int> GetEvenNumbers(List<int> numbers)
        {
            return numbers.Where(n => n % 2 == 0).ToList();
        }

        // Returns numbers divisible by 3 or 5 but not both
        public List<int> GetThreeOrFiveDividends(List<int> numbers)
        {
            var result = new List<int>();

            foreach (var n in numbers)
            {
                if ((n % 3 == 0 || n % 5 == 0) && !(n % 3 == 0 && n % 5 == 0))
                {
                    result.Add(n);
                }
            }

            return result;
        }

    }
}
