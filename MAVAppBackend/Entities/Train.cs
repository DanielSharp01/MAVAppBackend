using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Entities
{
    public class Train
    {
        public int Id { get; set; }
        public int TrainNumber { get; set; }
        public TrainType? Type { get; set; }
        public Station? From { get; }
        public Station? To { get; }
        public string? EncodedPolyline { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool FullKnowledge { get; set; }

        public Train(int trainNumber)
        {
            TrainNumber = trainNumber;
        }
    }
}
