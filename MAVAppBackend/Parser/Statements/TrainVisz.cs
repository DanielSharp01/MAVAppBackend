using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAVAppBackend.MAV;

namespace MAVAppBackend.Parser.Statements
{
    /// <summary>
    /// Tells the visz (it stands for something in Hungarian) number of a train
    /// </summary>
    public class TrainVisz : ParserStatement
    {
        /// <summary>
        /// Identifies the train
        /// </summary>
        public TrainIdentification Id { get; }

        /// <summary>
        /// Visz number of the train
        /// </summary>
        public string? ViszNumber { get; }

        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="id">Identifies the train</param>
        /// <param name="viszNumber"> Visz number of the train</param>
        public TrainVisz(APIResponse origin, TrainIdentification id, string? viszNumber)
            : base(origin)
        {
            Id = id;
            ViszNumber = viszNumber;
        }
    }
}
