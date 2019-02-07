﻿using MAVAppBackend.MAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Parser.Statements
{
    /// <summary>
    /// Indicates that the train has a specific type
    /// </summary>
    public class TrainHasType : ParserStatement
    {
        /// <summary>
        /// Identifies the train
        /// </summary>
        public TrainIdentification Id { get; }

        /// <summary>
        /// Type of the train
        /// </summary>
        public TrainType Type { get; }

        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="id">Identifies the train</param>
        /// <param name="type">Type of the train</param>
        public TrainHasType(APIResponse origin, TrainIdentification id, TrainType type)
            : base(origin)
        {
            Id = id;
            Type = type;
        }
    }
}
