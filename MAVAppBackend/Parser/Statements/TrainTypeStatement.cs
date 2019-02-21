using MAVAppBackend.MAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Parser.Statements
{
    /// <summary>
    /// Indicates that the train has a specific type
    /// </summary>
    public class TrainTypeStatement : ParserStatement
    {
        /// <summary>
        /// Identifies the train
        /// </summary>
        public TrainIdStatement Id { get; }

        /// <summary>
        /// Type of the train
        /// </summary>
        public TrainType Type { get; }

        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="id">Identifies the train</param>
        /// <param name="type">Type of the train</param>
        public TrainTypeStatement(APIResponse origin, TrainIdStatement id, TrainType type)
            : base(origin)
        {
            Id = id;
            Type = type;
        }

        protected override void InternalProcess(AppContext appContext)
        {
            if (Id.DbTrain == null) return;

            Id.DbTrain.Type = Type;
        }
    }
}
