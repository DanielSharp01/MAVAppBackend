using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Entities
{
    /// <summary>
    /// Tell information about the train instance state
    /// </summary>
    public enum TrainInstanceState
    {
        NotDeparted,
        Running,
        StoppedUnknown,
        StoppedArrived
    }

    public class TrainInstance
    {
        public int Id { get; set; }
        public string ElviraId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Delay { get; set; }
        public TrainInstanceState State { get; set; }

        private TrainInstance() { }

        public TrainInstance(string elviraId)
        {
            ElviraId = elviraId;
        }
    }
}
