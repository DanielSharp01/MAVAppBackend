using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend
{
    public static class CSExtensions
    {
        /// <summary>
        /// Substring between two character indices (both inclusive)
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="from">Index where the substring starts (inclusive)</param>
        /// <param name="to">Index where the substring ends (inclusive)</param>
        /// <returns>Substring between the two character indices (both inclusive)</returns>
        public static string Substr(this string str, int from, int to)
        {
            if (from < 0) from = str.Length + from;
            if (to < 0) to = str.Length + to;

            return str.Substring(from, to - from + 1);
        }

        /// <summary>
        /// Used to get a typed IEnumerable from an array without casting
        /// </summary>
        /// <typeparam name="T">Type of the array</typeparam>
        /// <param name="arr">Array</param>
        /// <returns>A typed enumerable</returns>
        public static IEnumerable<T> GetEnumerable<T>(this T[] arr)
        {
            return arr;
        }
    }
}
