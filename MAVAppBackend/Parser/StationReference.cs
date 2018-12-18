using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Parser
{
    /// <summary>
    /// References a station by it's ID and name
    /// </summary>
    public class StationReference
    {
        /// <summary>
        /// MAV's internal ID, doesn't seem to be used for anything, null if not provided (or parsing error occured)
        /// </summary>
        public int? ID { get; }
        /// <summary>
        /// The station's name
        /// </summary>
        public string Name { get; }

        /// <param name="id">MAV's internal ID, doesn't seem to be used for anything, null if not provided (or parsing error occured)</param>
        /// <param name="name">The station's name</param>
        public StationReference(int? id, string name)
        {
            ID = id;
            Name = name;
        }

        /// <summary>
        /// Parses the map.getData({ ... }) like looking javascript from the attribute
        /// </summary>
        /// <param name="script">JavaScript to parse</param>
        /// <returns>Parsed station reference or null if parsing error occurs</returns>
        public static StationReference? FromScript(string? script)
        {
            if (script == null) return null;

            var mapGetData = script.Split(new char[] { ';' }, 2)[0];
            try
            {
                var data = JObject.Parse(mapGetData.Substr(mapGetData.IndexOf("{"), mapGetData.LastIndexOf("}")));
                var id = CSExtensions.ParseInt(data["i"]?.ToString());
                var stationName = data["a"]?.ToString();

                if (stationName == null) return null;
                return new StationReference(id, stationName);
            }
            catch (JsonReaderException)
            {
                return null;
            }
        }
    }
}
