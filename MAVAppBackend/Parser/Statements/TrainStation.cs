using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAVAppBackend.MAV;

namespace MAVAppBackend.Parser.Statements
{
    /// <summary>
    /// Links a train and a station together
    /// </summary>
    public class TrainStation : ParserStatement
    {
        /// <summary>
        /// Identifies the train
        /// </summary>
        public TrainIdentification TrainId { get; }

        /// <summary>
        /// Station of the train
        /// </summary>
        public StationIdentification StationId { get; }

        /// <summary>
        /// Time the train arrives at (null may indicate first station or no time tuple data <see cref="HasTimeInfo">HasTimeInfo</see> for more info)
        /// </summary>
        public TimeTuple? Arrival { get; }

        /// <summary>
        /// Time the train departs at (null may indicate last station or no time tuple data <see cref="HasTimeInfo">HasTimeInfo</see> for more info)
        /// </summary>
        public TimeTuple? Departure { get; }

        public bool HasTimeInfo { get; }

        /// <param name="origin"></param>
        /// <param name="trainId">Identifies the train</param>
        /// <param name="stationId">Station of the train</param>
        /// <param name="arrival">Time the train arrives at (null may indicate first station, this will set <see cref="HasTimeInfo">HasTimeInfo</see> to false if departure is also null)</param>
        /// <param name="departure">Time the train departs at (null may indicate last station, this will set <see cref="HasTimeInfo">HasTimeInfo</see> to false if arrival is also null)</param>
        public TrainStation(APIResponse origin, TrainIdentification trainId, StationIdentification stationId, TimeTuple? arrival, TimeTuple? departure)
            : base(origin)
        {
            TrainId = trainId;
            StationId = stationId;
            Arrival = arrival;
            Departure = departure;
            HasTimeInfo = arrival != null || departure != null;
        }
    }
}
