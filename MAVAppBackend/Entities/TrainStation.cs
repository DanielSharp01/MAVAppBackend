using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Entities
{
    public class TrainStation
    {
        public int Id { get; set; }
        public int TrainId { get; private set; }
        public Train Train { get; set; }
        public int StationId { get; private set; }
        public Station Station { get; set; }
        public double? Distance { get; set; }
        public TimeSpan? Arrival { get; set; }
        public TimeSpan? Departure { get; set; }

        private TrainStation() { }

        public TrainStation(Train train, Station station)
        {
            TrainId = train.Id;
            Train = train;
            StationId = station.Id;
            Station = station;
        }
    }
}
