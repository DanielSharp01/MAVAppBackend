using MAVAppBackend.MAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MAVAppBackend.Parser.Statements
{
    /// <summary>
    /// Statement that a train (with a number) was found
    /// </summary>
    public class TrainIdentification : ParserStatement
    {
        /// <summary>
        /// Unique number of the train (more like a train class id)
        /// </summary>
        public int? Number { get; }
        /// <summary>
        /// Unique id of the train instance (more like a train instance id)
        /// </summary>
        public string? ElviraId { get; }

        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="number">Unique number of the train (more like a train class id)</param>
        /// <param name="elviraID">Unique id of the train instance (more like a train instance id)</param>
        public TrainIdentification(APIResponse origin, int? number, string? elviraID)
            : base(origin)
        {
            Number = number;
            ElviraId = elviraID;
        }
    }
}
