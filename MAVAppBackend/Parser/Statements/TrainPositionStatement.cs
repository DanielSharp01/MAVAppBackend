using MAVAppBackend.MAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Parser.Statements
{
    /// <summary>
    /// Tells the latitude, longitude of a moving train coming from the dynamic TRAINS API
    /// </summary>
    public class TrainPositionStatement : ParserStatement
    {
        /// <summary>
        /// Identifies the train
        /// </summary>
        public TrainIdStatement Id { get; }

        /// <summary>
        /// Latitude in degrees
        /// </summary>
        public double Latitude { get; }

        /// <summary>
        /// Longitude in degrees
        /// </summary>
        public double Longitude { get; }

        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="id">Identifies the train</param>
        /// <param name="latitude">Latitude in degrees</param>
        /// <param name="longitude">Longitude in degrees</param>
        public TrainPositionStatement(APIResponse origin, TrainIdStatement id, double latitude, double longitude)
            : base(origin)
        {
            Id = id;
            Latitude = latitude;
            Longitude = longitude;
        }

        protected override void InternalProcess(AppContext appContext)
        {
            if (Id.DbTrainInstance == null) return;

            Id.DbTrainInstance.Latitude = Latitude;
            Id.DbTrainInstance.Longitude = Longitude;
        }
    }
}
