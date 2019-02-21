using MAVAppBackend.MAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Parser.Statements
{
    /// <summary>
    /// Describes a [from station - to station as train type] subrelation of a train
    /// </summary>
    public class TrainSubrelationStatement : ParserStatement
    {
        /// <summary>
        /// Identifies the train (TRAIN API header uses this)
        /// </summary>
        public TrainIdStatement Id { get; }

        /// <summary>
        /// Station the train departs from
        /// </summary>
        public StationIdStatement From { get; }

        /// <summary>
        /// Station the train arrives to
        /// </summary>
        public StationIdStatement To { get; }

        /// <summary>
        /// Type of the train on this relation
        /// </summary>
        public TrainType Type { get; }

        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="id">Identifies the train</param>
        /// <param name="from">Identifies the train</param>
        /// <param name="to">Station the train arrives to</param>
        /// <param name="type">Type of the train on this subrelation</param>
        public TrainSubrelationStatement(APIResponse origin, TrainIdStatement id, StationIdStatement from, StationIdStatement to, TrainType type)
            : base(origin)
        {
            Id = id;
            From = from;
            To = to;
            Type = type;
        }

        protected override void InternalProcess(AppContext appContext)
        {
            if (From.DbStation != null)
                new TrainStationStatement(Origin, Id, From, null, null).Process(appContext);

            if (To.DbStation != null);
                new TrainStationStatement(Origin, Id, To, null, null).Process(appContext);
        }
    }
}
