﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Entities
{
    public class Train
    {
        public int Id { get; set; }
        public int TrainNumber { get; set; }
        public string? Name { get; set; }
        public string? ViszNumber { get; set; }
        public TrainType? Type { get; set; }
        public Station? From { get; set; }
        public Station? To { get; set; }
        public string? EncodedPolyline { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool FullKnowledge { get; set; }

        public Train(int trainNumber)
        {
            TrainNumber = trainNumber;
        }
    }
}
