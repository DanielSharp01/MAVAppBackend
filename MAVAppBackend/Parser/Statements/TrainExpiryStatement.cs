using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAVAppBackend.MAV;

namespace MAVAppBackend.Parser.Statements
{
    /// <summary>
    /// Tells when the data for this train expires
    /// </summary>
    public class TrainExpiryStatement : ParserStatement
    {
        /// <summary>
        /// Identifies the train
        /// </summary>
        public TrainIdStatement Id { get; }

        /// <summary>
        /// Expiry date of the data for this train (null indicates good until the end of this year probably)
        /// </summary>
        public DateTime? ExpiryDate { get; }

        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="id">Identifies the train</param>
        /// <param name="expiryDate">Expiry date of the data for this train</param>
        public TrainExpiryStatement(APIResponse origin, TrainIdStatement id, DateTime? expiryDate)
            : base(origin)
        {
            Id = id;
            ExpiryDate = expiryDate;
        }

        protected override void InternalProcess(AppContext appContext)
        {
            if (Id.DbTrain == null) return;

            Id.DbTrain.ExpiryDate = ExpiryDate ?? new DateTime(DateTime.Now.Year + 1, 1, 1, 0, 0, 0);
        }
    }
}
