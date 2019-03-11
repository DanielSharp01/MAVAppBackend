using MAVAppBackend.MAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Parser.Statements
{
    /// <summary>
    /// Describes a [from station @ time - to station @ time] relation of the train
    /// </summary>
    public class TrainRelationStatement : ParserStatement
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
        /// Time the train departs from the starting station
        /// </summary>
        public TimeSpan? FromTime { get; }

        /// <summary>
        /// Station the train arrives to
        /// </summary>
        public StationIdStatement To { get; }

        /// <summary>
        /// Time the train arrives at the destination station
        /// </summary>
        public TimeSpan? ToTime { get; }

        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="id">Identifies the train</param>
        /// <param name="from">Identifies the train</param>
        /// <param name="to">Station the train arrives to</param>
        /// <param name="type">Type of the train on this subrelation</param>
        public TrainRelationStatement(APIResponse origin, TrainIdStatement id, StationIdStatement from, StationIdStatement to, TimeSpan? fromTime = null, TimeSpan? toTime = null)
            : base(origin)
        {
            Id = id;
            From = from;
            To = to;
            FromTime = fromTime;
            ToTime = toTime;
        }

        protected override void InternalProcess(AppContext appContext)
        {
            if (Id.DbTrain == null) return;

            if (From.DbStation != null)
            {
                Id.DbTrain.From = From.DbStation;
                new TrainStationStatement(Origin, Id, From, null, FromTime != null ? new TimeTuple(FromTime.Value, null) : null).Process(appContext);
            }

            if (To.DbStation != null)
            {
                Id.DbTrain.To = To.DbStation;
                new TrainStationStatement(Origin, Id, To, ToTime != null ? new TimeTuple(ToTime.Value, null) : null, null).Process(appContext);
            }
        }
    }
}
