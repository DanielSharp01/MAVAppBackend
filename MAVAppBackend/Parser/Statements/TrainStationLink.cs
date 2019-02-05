using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAVAppBackend.MAV;

namespace MAVAppBackend.Parser.Statements
{
    /// <summary>
    /// Links two train stations together (only TRAIN API rows produce these)
    /// </summary>
    public class TrainStationLink : ParserStatement
    {
        /// <summary>
        /// Identifies the train
        /// </summary>
        public TrainIdentification TrainId { get; }

        /// <summary>
        /// Station the link starts with
        /// </summary>
        public StationIdentification From { get; }

        /// <summary>
        /// Station the link ends with
        /// </summary>
        public StationIdentification To { get; }

        /// <summary>
        /// Indicates whether the link is absolutely direct between the stations (for the specific train, only the TrainParser can make such calls)
        /// </summary>
        public bool IsDirect { get; }

        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="trainId">Identifies the train</param>
        /// <param name="from">Station the link starts with</param>
        /// <param name="to">Station the link ends with</param>
        /// <param name="isDirect">Indicates whether the link is absolutely direct between the stations (for the specific train)</param>
        public TrainStationLink(APIResponse origin, TrainIdentification trainId, StationIdentification from, StationIdentification to, bool isDirect)
            : base(origin)
        {
            TrainId = trainId;
            From = from;
            To = to;
            IsDirect = isDirect;
        }
    }
}
