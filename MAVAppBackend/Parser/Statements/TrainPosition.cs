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
    public class TrainPosition : ParserStatement
    {
        /// <summary>
        /// Identifies the train
        /// </summary>
        public TrainIdentification Id { get; }

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
        public TrainPosition(APIResponse origin, TrainIdentification id, double latitude, double longitude)
            : base(origin)
        {
            Id = id;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
