using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Parser
{
    public static class Helpers
    {
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
    }
}
