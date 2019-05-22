using System.Collections.Generic;

namespace DatenMeister.Runtime.Functions.Algorithm
{
    /// <summary>
    /// Implements an algorithm for permutations
    /// </summary>
    public static class Permutation
    {
        /// <summary>
        /// Swaps the two variables
        /// </summary>
        /// <param name="list">List of elements whose element shall be swapped</param>
        /// <param name="a">Index of the first variable</param>
        /// <param name="b">Index of the second variable</param>
        private static void Swap<T>(IList<T> list, int a, int b)
        {
            if (a.Equals(b))
            {
                return;
            }

            var c = list[a];
            list[a] = list[b];
            list[b] = c;
        }

        /// <summary>
        /// Gets all permutations of an array
        /// </summary>
        /// <param name="list">List to be evaluated</param>
        /// <returns>Enumeration of all combinations</returns>
        public static IEnumerable<IList<T>> GetPer<T>(IList<T> list)
        {
            var x = list.Count - 1;
            return GetPer(list, 0, x);
        }

        private static IEnumerable<IList<T>> GetPer<T>(IList<T> list, int k, int m)
        {
            if (k == m)
            {
                yield return list;
            }
            else
            {
                for (var i = k; i <= m; i++)
                {
                    Swap(list, k, i);
                    foreach (var result in GetPer(list, k + 1, m))
                    {
                        yield return result;
                    }
                    Swap(list,k,i);
                }
            }
        }

        /// <summary>
        /// Gets the factorial for a certain number
        /// </summary>
        /// <param name="number">Number to be used</param>
        /// <returns>The factorial</returns>
        public static long Factorial(int number)
        {
            var result = 1L;

            for (var n = 1; n <= number; n++)
            {
                result *= n;
            }
            return result;
        }
    }
}