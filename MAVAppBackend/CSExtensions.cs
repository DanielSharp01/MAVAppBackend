using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        /// <summary>
        /// Tries parsing an integer from the input
        /// </summary>
        /// <param name="input">Input string will be trimmed (from whitespace)</param>
        /// <returns>The integer parsed, null if failed</returns>
        public static int? ParseInt(string? input)
        {
            if (input == null) return null;

            if (int.TryParse(input.Trim(), out int res))
            {
                return res;
            }

            return null;
        }

        public static Task WhenCancelled(this CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
            return tcs.Task;
        }
    }
}
