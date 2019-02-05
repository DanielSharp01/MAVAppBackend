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
    public class TrainName : ParserStatement
    {
        /// <summary>
        /// Identifies the train
        /// </summary>
        public TrainIdentification Id { get; }

        /// <summary>
        /// Name of the train
        /// </summary>
        public string? Name { get; }

        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="id">Identifies the train</param>
        /// <param name="name">Name of the train</param>
        public TrainName(APIResponse origin, TrainIdentification id, string? name)
            : base(origin)
        {
            Id = id;
            Name = name;
        }
    }
}
