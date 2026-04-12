using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqQuery.Interfaces
{
    public interface INumberService
    {
        List<int> GenerateNumbers(int number);
        List<int> GetEvenNumbers(List<int> numbers);
        List<int> GetThreeOrFiveDividends(List<int> numbers);
    }
}
