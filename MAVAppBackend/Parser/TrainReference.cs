using HtmlAgilityPack;
using System;

namespace MAVAppBackend.Parser
{
    /// <summary>
    /// Reference to a train from the STATION or the ROUTE APIs
    /// </summary>
    public class TrainReference
    {
        /// <summary>
        /// Number of the train, null if not known
        /// </summary>
        public int? Number { get; }
        /// <summary>
        /// ElviraID of the train, null if not known
        /// </summary>
        public string? ElviraID { get; }
        /// <summary>
        /// Type of the train, null if not known
        /// </summary>
        public TrainType? Type { get; set; }
        /// <summary>
        /// Station the train departs from, null if not known
        /// </summary>
        public StationReference? From { get; set; }
        /// <summary>
        /// Time the train departs at, null if not known
        /// </summary>
        public TimeSpan? FromTime { get; set; }
        /// <summary>
        /// Station the train arrives to, null if not known
        /// </summary>
        public StationReference? To { get; set; }
        /// <summary>
        /// Time the train arrives at, null if not known
        /// </summary>
        public TimeSpan? ToTime { get; set; }

        /// <param name="number">Number of the train, null if not known</param>
        /// <param name="elviraId">ElviraID of the train, null if not known</param>
        public TrainReference(int? number, string? elviraId)
        {
            Number = number;
            ElviraID = elviraId;
        }

        /// <summary>
        /// Parse station reference from the STATION API table cell
        /// </summary>
        /// <param name="htmlNode">Table cell</param>
        /// <returns>Parsed reference or null if parsing fails</returns>
        public static TrainReference? ParseFromStation(HtmlNode htmlNode)
        {
            return null;
        }
    }
}
