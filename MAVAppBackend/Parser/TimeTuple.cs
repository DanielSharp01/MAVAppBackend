using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Parser
{
    /// <summary>
    /// Time tuple that has a Scheduled and an Actual field appearing in time referencing fields in the API
    /// </summary>
    public class TimeTuple
    {
        /// <summary>
        /// Scheduled time
        /// </summary>
        public TimeSpan Scheduled { get; }
        /// <summary>
        /// Actual time (including delays, should not neccesarily be used to determine it though)
        /// </summary>
        public TimeSpan Actual { get; }
    }
}
