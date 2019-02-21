using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAVAppBackend.MAV;

namespace MAVAppBackend.Parser.Statements
{
    /// <summary>
    /// Tells how much a train is delayed coming from the dynamic TRAINS API
    /// </summary>
    public class TrainDelayStatement : ParserStatement
    {
        /// <summary>
        /// Identifies the train
        /// </summary>
        public TrainIdStatement Id { get; }

        /// <summary>
        /// Delay in minutes
        /// </summary>
        public int Delay { get; }

        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="id">Identifies the train</param>
        /// <param name="delay">Delay in minutes</param>
        public TrainDelayStatement(APIResponse origin, TrainIdStatement id, int delay)
            : base(origin)
        {
            Id = id;
            Delay = delay;
        }

        protected override void InternalProcess(AppContext appContext)
        {
            if (Id.DbTrainInstance == null) return;

            Id.DbTrainInstance.Delay = Delay;
        }
    }
}
