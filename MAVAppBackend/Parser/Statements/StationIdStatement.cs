using MAVAppBackend.Entities;
using MAVAppBackend.MAV;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MAVAppBackend.Parser.Statements
{
    /// <summary>
    /// Indicates that a station (with a name) was found
    /// </summary>
    public class StationIdStatement : ParserStatement
    {
        /// <summary>
        /// Name of the station
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// MÁV's own internal number (only set when applicable, should not matter too much)
        /// </summary>
        public int? Number { get; }

        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="name">Name of the station</param>
        /// <param name="number">MÁV's own internal number (only set when applicable, should not matter too much)</param>
        public StationIdStatement(APIResponse origin, string name, int? number)
            : base(origin)
        {
            Name = name;
            Number = number;
        }

        /// <summary>
        /// Parses the map.getData({ ... }) like looking javascript from the attribute
        /// </summary>
        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="script">JavaScript to parse</param>
        /// <returns>StationIdentification statement or null if parsing fails</returns>
        public static StationIdStatement? FromScript(APIResponse origin, string? script)
        {
            if (script == null) return null;

            var mapGetData = script.Split(new char[] { ';' }, 2)[0];
            try
            {
                var data = JObject.Parse(mapGetData.Substr(mapGetData.IndexOf("{"), mapGetData.LastIndexOf("}")));
                var id = CSExtensions.ParseInt(data["i"]?.ToString());
                var stationName = data["a"]?.ToString();

                if (stationName == null) return null;
                return new StationIdStatement(origin, stationName, id);
            }
            catch (JsonReaderException)
            {
                return null;
            }
        }

        public Station? DbStation { get; private set; } = null;

        protected override void InternalProcess(AppContext appContext)
        {
            DbStation = appContext.Stations.Where(s => s.NormalizedName == Station.NormalizeName(Name)).FirstOrDefault();
            if (DbStation == null)
            {
                DbStation = new Station(Name);
                appContext.Stations.Add(DbStation);
            }
        }
    }
}
