using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Parser
{
    /// <summary>
    /// Information that the parser recovers from the TRAIN API
    /// </summary>
    public class TrainInfo
    {
        /// <summary>
        /// Number of the train (eg. 2620)
        /// </summary>
        public int Number { get; }
        /// <summary>
        /// Date of the schedule, time part unused
        /// </summary>
        public DateTime Date { get; }
        /// <summary>
        /// Instance ID of the train in the format XXXXXX-YYMMDD, can be null if not known
        /// </summary>
        public string? ElviraID { get; }
        /// <summary>
        /// Name of the train, can be null if has none
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Name of the train can be null if has none
        /// </summary>
        public string? ViszNumber { get; set; }
        /// <summary>
        /// Estimated expiry of the schedule
        /// </summary>
        public DateTime? EstimatedExpiry { get; set; }
        /// <summary>
        /// Train relations which contain from, to and which train type is used for the specific journey
        /// </summary>
        public List<TrainRelation> TrainRelations { get; } = new List<TrainRelation>();

        public List<TrainStation> TrainStations { get; } = new List<TrainStation>();

        /// <summary>
        /// Encoded polyline, can be null if for some reason not provided
        /// </summary>
        public string? EncodedPolyline { get; set; }

        /// <summary>
        /// Tip: Set other parameters using property initializers
        /// </summary>
        /// <param name="number">Number of the train (eg. 2620)</param>
        /// <param name="date">Date of the schedule, time part unused</param>
        /// <param name="elviraID">Elvira ID in the format XXXXXX-YYMMDD</param>
        public TrainInfo(int number, DateTime date, string? elviraID = null)
        {
            Number = number;
            ElviraID = elviraID;
            Date = date;
        }
    }

    public enum TrainType
    {
        Local,
        Fast,
        InterCity,
        InterRegion,
        EuroCity,
        SubstitutionBus
    }

    /// <summary>
    /// From, to and which train type is used for the specific journey
    /// </summary>
    public class TrainRelation
    {
        /// <summary>
        /// Station's name where the train starts from
        /// </summary>
        public string From { get; }
        /// <summary>
        /// Station's name where the train arrives 
        /// </summary>
        public string To { get; }
        /// <summary>
        /// Train's type for the specific journey
        /// </summary>
        public TrainType Type { get; }

        /// <param name="from">Station's name where the train starts from</param>
        /// <param name="to">Station's name where the train arrives </param>
        /// <param name="type">Train's type for the specific journey</param>
        public TrainRelation(string from, string to, TrainType type)
        {
            From = from;
            To = to;
            Type = type;
        }
    }

    /// <summary>
    /// Information about a train station, row in the TRAIN API table
    /// </summary>
    public class TrainStation
    {
        /// <summary>
        /// Distance from the beginning in kms, null if not provided
        /// </summary>
        public int? Distance { get; }
        /// <summary>
        /// References a station by it's ID and name
        /// </summary>
        public StationReference Station { get; }
        /// <summary>
        /// Scheduled and actual arrival time, null if departs from here
        /// </summary>
        public TimeTuple? Arrival { get; }
        /// <summary>
        /// Scheduled and actual departure time, null if arrives to here
        /// </summary>
        public TimeTuple? Departure { get; }
        /// <summary>
        /// Whether this station is indicated to be hit (extremely unreliable information, never believe negatives only positives)
        /// </summary>
        public bool Hit { get; }
        /// <summary>
        /// Platform that the train arrives to at that specific station, can be null if not filled (mostly not filled even when ambiguous)
        /// </summary>
        public string? Platform { get; }

        /// <param name="distance">Distance from the beginning in kms</param>
        /// <param name="station">References a station by it's ID and name</param>
        /// <param name="arrival">Scheduled and actual arrival time, null if departs from here</param>
        /// <param name="departure">Scheduled and actual departure time, null if arrives to here</param>
        /// <param name="hit">Whether this station is indicated to be hit (extremely unreliable information, never believe negatives only positives)</param>
        /// <param name="platform">Platform that the train arrives to at that specific station, can be null if not filled (mostly not filled even when ambiguous)</param>
        public TrainStation(int? distance, StationReference station, TimeTuple? arrival, TimeTuple? departure, bool hit, string? platform = null)
        {
            Distance = distance;
            Station = station;
            Arrival = arrival;
            Departure = departure;
            Hit = hit;
            Platform = platform;
        }
    }
}
