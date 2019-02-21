using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAVAppBackend.MAV;

namespace MAVAppBackend.Parser.Statements
{
    /// <summary>
    /// Tells the train's name
    /// </summary>
    public class TrainNameStatement : ParserStatement
    {
        /// <summary>
        /// Identifies the train
        /// </summary>
        public TrainIdStatement Id { get; }

        /// <summary>
        /// Name of the train
        /// </summary>
        public string? Name { get; }

        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="id">Identifies the train</param>
        /// <param name="name">Name of the train</param>
        public TrainNameStatement(APIResponse origin, TrainIdStatement id, string? name)
            : base(origin)
        {
            Id = id;
            Name = name;
        }

        protected override void InternalProcess(AppContext appContext)
        {
            if (Id.DbTrain == null) return;

            Id.DbTrain.Name = Name;
        }
    }
}
