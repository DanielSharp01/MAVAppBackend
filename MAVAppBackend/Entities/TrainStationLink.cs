using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Entities
{
    public class TrainStationLink
    {
        public int Id { get; set; }
        public int TrainId { get; private set; }
        public Train Train { get; set; }
        public int? FromId { get; private set; }
        public Station? From { get; set; }
        public int? ToId { get; private set; }
        public Station? To { get; set; }

        private TrainStationLink() { }

        public TrainStationLink(Train train, Station? from, Station? to)
        {
            TrainId = train.Id;
            Train = train;
            FromId = from?.Id;
            From = from;
            ToId = to?.Id;
            To = to;
        }
    }
}
