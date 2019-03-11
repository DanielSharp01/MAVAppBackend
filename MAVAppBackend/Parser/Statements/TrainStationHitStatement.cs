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
    public class TrainStationHitStatement : ParserStatement
    {
        /// <summary>
        /// Identifies the train and its station
        /// </summary>
        public TrainStationStatement TrainStationId { get; }

        /// <summary>
        /// Indicates whether the station was hit
        /// </summary>
        public bool IsHit { get; }

        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="trainStationId">Identifies the train and its station</param>
        /// <param name="isHit">Indicates whether the station was hit</param>
        public TrainStationHitStatement(APIResponse origin, TrainStationStatement trainStationId, bool isHit)
            : base(origin)
        {
            TrainStationId = trainStationId;
            IsHit = isHit;
        }

        protected override void InternalProcess(AppContext appContext)
        {
            // We don't process this for now
        }
    }
}
