using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAVAppBackend.MAV;

namespace MAVAppBackend.Parser.Statements
{
    /// <summary>
    /// Indicates whether the station was hit (according to the API, this is innacurate as hell)
    /// </summary>
    public class TrainStationHit : ParserStatement
    {
        /// <summary>
        /// Identifies the train and its station
        /// </summary>
        public TrainStation TrainStationId { get; }

        /// <summary>
        /// Indicates whether the station was hit
        /// </summary>
        public bool IsHit { get; }

        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="trainStationId">Identifies the train and its station</param>
        /// <param name="isHit">Indicates whether the station was hit</param>
        public TrainStationHit(APIResponse origin, TrainStation trainStationId, bool isHit)
            : base(origin)
        {
            TrainStationId = trainStationId;
            IsHit = isHit;
        }
    }
}
