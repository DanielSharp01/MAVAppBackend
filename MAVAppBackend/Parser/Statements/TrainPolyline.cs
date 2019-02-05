using MAVAppBackend.MAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Parser.Statements
{
    /// <summary>
    /// Tells the path of a train
    /// </summary>
    public class TrainPolyline : ParserStatement
    {
        /// <summary>
        /// Identifies the train
        /// </summary>
        public TrainIdentification Id { get; }

        /// <summary>
        /// Encoded polyline of the train path (google maps encoding)
        /// </summary>
        public string EncodedPolyline { get; }

        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="id">Identifies the train</param>
        /// <param name="encodedPolyline">Encoded polyline of the train path (google maps encoding)</param>
        public TrainPolyline(APIResponse origin, TrainIdentification id, string encodedPolyline)
            : base(origin)
        {
            Id = id;
            EncodedPolyline = encodedPolyline;
        }
    }
}
