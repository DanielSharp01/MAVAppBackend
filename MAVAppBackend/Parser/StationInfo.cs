using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Parser
{
    /// <summary>
    /// Information that the parser recovers from the STATION API
    /// </summary>
    public class StationInfo
    {
        /// <summary>
        /// Station reference containing name and maybe id of the station
        /// </summary>
        public StationReference IdReference { get; }

        /// <summary>
        /// Information about the trains going through the station
        /// </summary>
        public List<StationEntry> StationTrains { get; } = new List<StationEntry>();

        /// <param name="reference">Station reference containing name and maybe id of the station</param>
        public StationInfo(StationReference reference)
        {
            IdReference = reference;
        }
    }
}
