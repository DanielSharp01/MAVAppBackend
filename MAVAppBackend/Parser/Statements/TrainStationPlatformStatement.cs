using MAVAppBackend.MAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Parser.Statements
{
    /// <summary>
    /// Tells which platform the train arrives to (and/or departs from)
    /// </summary>
    public class TrainStationPlatformStatement : ParserStatement
    {
        /// <summary>
        /// Identifies the train and its station
        /// </summary>
        public TrainStationStatement Id { get; }

        /// <summary>
        /// Platform where the train arrives (can be null if not known or there aren't multiple platforms in the direction)
        /// </summary>
        public string? Platform { get; }

        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="trainStationId">Identifies the train and its station</param>
        /// <param name="platform">Platform where the train arrives (can be null if not known or there aren't multiple platforms in the direction)</param>
        public TrainStationPlatformStatement(APIResponse origin, TrainStationStatement trainStationId, string? platform)
            : base(origin)
        {
            Id = trainStationId;
            Platform = platform;
        }

        protected override void InternalProcess(AppContext appContext)
        {
            if (Id.DbTrainStation == null) return;

            Id.DbTrainStation.Platform = Platform;
        }
    }
}
