using MAVAppBackend.MAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Parser
{
    /// <summary>
    /// Something that the Parser states after processing an API response
    /// </summary>
    public class ParserStatement
    {
        /// <summary>
        /// API response that was processed to make this statement
        /// </summary>
        public APIResponse Origin { get; }

        /// <param name="origin">API response that was processed to make this statement</param>
        public ParserStatement(APIResponse origin)
        {
            Origin = origin;
        }
    }
}
