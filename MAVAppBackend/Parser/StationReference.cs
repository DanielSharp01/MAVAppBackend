using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Parser
{
    /// <summary>
    /// References a station by it's ID and name
    /// </summary>
    public struct StationReference
    {
        /// <summary>
        /// MAV's internal ID, doesn't seem to be used for anything
        /// </summary>
        public int ID { get; }
        /// <summary>
        /// The station's name
        /// </summary>
        public string Name { get; }

        /// <param name="id">MAV's internal ID, doesn't seem to be used for anything</param>
        /// <param name="name">The station's name</param>
        public StationReference(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
