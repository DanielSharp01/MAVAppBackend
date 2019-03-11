using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAVAppBackend.Entities;
using MAVAppBackend.MAV;

namespace MAVAppBackend.Parser.Statements
{
    /// <summary>
    /// Links two train stations together (only TRAIN API rows produce these)
    /// </summary>
    public class TrainStationLinkStatement : ParserStatement
    {
        /// <summary>
        /// Identifies the train
        /// </summary>
        public TrainIdStatement TrainId { get; }

        /// <summary>
        /// Station the link starts with
        /// </summary>
        public StationIdStatement From { get; }

        /// <summary>
        /// Station the link ends with
        /// </summary>
        public StationIdStatement To { get; }

        /// <summary>
        /// Indicates whether the link is absolutely direct between the stations (for the specific train, only the TrainParser can make such calls)
        /// </summary>
        public bool IsDirect { get; }

        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="trainId">Identifies the train</param>
        /// <param name="from">Station the link starts with</param>
        /// <param name="to">Station the link ends with</param>
        /// <param name="isDirect">Indicates whether the link is absolutely direct between the stations (for the specific train)</param>
        public TrainStationLinkStatement(APIResponse origin, TrainIdStatement trainId, StationIdStatement from, StationIdStatement to, bool isDirect)
            : base(origin)
        {
            TrainId = trainId;
            From = from;
            To = to;
            IsDirect = isDirect;
        }

        public TrainStationLink? DbTrainStationLink { get; private set; }

        protected override void InternalProcess(AppContext appContext)
        {
            if (TrainId.DbTrain == null) return;
            if (From.DbStation == null) return;
            if (To.DbStation == null) return;
            if (!IsDirect) return; // For now

            DbTrainStationLink = appContext.TrainStationLinks.Where(s => s.TrainId == TrainId.DbTrain.Id && s.From.Id == From.DbStation.Id && s.To.Id == To.DbStation.Id).FirstOrDefault();
            if (DbTrainStationLink == null)
            {
                DbTrainStationLink = new TrainStationLink(TrainId.DbTrain, From.DbStation, To.DbStation);
                appContext.TrainStationLinks.Add(DbTrainStationLink);
            }
        }
    }
}
