using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvoType.src
{
    public static class ArrayExtensions
    {
        public static void Print<T>(this T[] array)
        {
            Console.WriteLine(string.Join("\n", array));
        }
    }
}