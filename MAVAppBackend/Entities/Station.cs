using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Entities
{
    public class Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool FullKnowledge { get; set; }

        private Station() { }

        public Station(string name)
        {
            Name = name;
        }
    }
}
