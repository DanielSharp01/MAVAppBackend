using MAVAppBackend.MAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Parser.Statements
{
    /// <summary>
    /// Tells the distance from the start of the train journey
    /// </summary>
    public class TrainStationDistance : ParserStatement
    {
        /// <summary>
        /// Identifies the train and its station
        /// </summary>
        public TrainStation TrainStationId { get; }

        /// <summary>
        /// Distance from the start (in kms), null when the station is not domestic (or otherwise not known)
        /// </summary>
        public int? Distance { get; }

        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="trainStationId">Identifies the train and its station</param>
        /// <param name="distance">Distance from the start (in kms), null when the station is not domestic (or otherwise not known)</param>
        public TrainStationDistance(APIResponse origin, TrainStation trainStationId, int? distance)
            : base(origin)
        {
            TrainStationId = trainStationId;
            Distance = distance;
        }
    }
}
