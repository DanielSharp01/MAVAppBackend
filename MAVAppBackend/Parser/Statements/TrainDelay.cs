using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAVAppBackend.MAV;

namespace MAVAppBackend.Parser.Statements
{
    /// <summary>
    /// Tells how much a train is delayed coming from the dynamic TRAINS API
    /// </summary>
    public class TrainDelay : ParserStatement
    {
        /// <summary>
        /// Identifies the train
        /// </summary>
        public TrainIdentification Id { get; }

        /// <summary>
        /// Delay in minutes
        /// </summary>
        public int Delay { get; }

        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="id">Identifies the train</param>
        /// <param name="delay">Delay in minutes</param>
        public TrainDelay(APIResponse origin, TrainIdentification id, int delay)
            : base(origin)
        {
            Id = id;
            Delay = delay;
        }
    }
}
