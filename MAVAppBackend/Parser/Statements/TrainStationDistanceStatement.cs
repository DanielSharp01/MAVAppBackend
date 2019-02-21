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
    public class TrainStationDistanceStatement : ParserStatement
    {
        /// <summary>
        /// Identifies the train and its station
        /// </summary>
        public TrainStationStatement Id { get; }

        /// <summary>
        /// Distance from the start (in kms), null when the station is not domestic (or otherwise not known)
        /// </summary>
        public int? Distance { get; }

        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="trainStationId">Identifies the train and its station</param>
        /// <param name="distance">Distance from the start (in kms), null when the station is not domestic (or otherwise not known)</param>
        public TrainStationDistanceStatement(APIResponse origin, TrainStationStatement trainStationId, int? distance)
            : base(origin)
        {
            Id = trainStationId;
            Distance = distance;
        }

        protected override void InternalProcess(AppContext appContext)
        {
            if (Id.DbTrainStation == null) return;

            Id.DbTrainStation.IntDistance = Distance;
        }
    }
}
