using MAVAppBackend.MAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Parser.Statements
{
    /// <summary>
    /// Describes a [from station - to station as train type] subrelation of a train
    /// </summary>
    public class TrainSubrelation : ParserStatement
    {
        /// <summary>
        /// Identifies the train (TRAIN API header uses this)
        /// </summary>
        public TrainIdentification Id { get; }

        /// <summary>
        /// Station the train departs from
        /// </summary>
        public StationIdentification From { get; }

        /// <summary>
        /// Station the train arrives to
        /// </summary>
        public StationIdentification To { get; }

        /// <summary>
        /// Type of the train on this relation
        /// </summary>
        public TrainType Type { get; }

        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="id">Identifies the train</param>
        /// <param name="from">Identifies the train</param>
        /// <param name="to">Station the train arrives to</param>
        /// <param name="type">Type of the train on this subrelation</param>
        public TrainSubrelation(APIResponse origin, TrainIdentification id, StationIdentification from, StationIdentification to, TrainType type)
            : base(origin)
        {
            Id = id;
            From = from;
            To = to;
            Type = type;
        }
    }
}
