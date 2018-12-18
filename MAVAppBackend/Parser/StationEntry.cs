namespace MAVAppBackend.Parser
{
    /// <summary>
    /// Represents an entry in the STATION API table
    /// </summary>
    public class StationEntry
    {
        /// <summary>
        /// Arrival date (at this station), may be null if the train departs from here
        /// </summary>
        public TimeTuple? Arrival { get; }
        /// <summary>
        /// Departure date (from this station), may be null if the train arrives to here
        /// </summary>
        public TimeTuple? Departure { get; }
        /// <summary>
        /// Reference to the train arriving and or departing from this station
        /// </summary>
        public TrainReference Train { get; }
        /// <summary>
        /// Platform where the train arrives
        /// </summary>
        public string? Platform { get; }

        /// <param name="arrival">Arrival date (at this station), may be null if the train departs from here</param>
        /// <param name="departure">Departure date (from this station), may be null if the train arrives to here</param>
        /// <param name="train">Reference to the train arriving and or departing from this station</param>
        /// <param name="platform">Platform where the train arrives</param>
        public StationEntry(TimeTuple? arrival, TimeTuple? departure, TrainReference train, string platform)
        {
            Arrival = arrival;
            Departure = departure;
            Train = train;
            Platform = platform;
        }
    }
}
