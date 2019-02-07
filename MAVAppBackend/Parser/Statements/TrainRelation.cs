﻿using MAVAppBackend.MAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Parser.Statements
{
    /// <summary>
    /// Describes a [from station @ time - to station @ time] relation of the train
    /// </summary>
    public class TrainRelation : ParserStatement
    {
        /// <summary>
        /// Identifies the train (TRAIN API header uses this)
        /// </summary>
        public TrainIdentification Id { get; }

        /// <summary>
        /// Station the train departs from
        /// </summary>
        public StationIdentification From { get; }

        /// <summary>
        /// Time the train departs from the starting station
        /// </summary>
        public TimeSpan? FromTime { get; }

        /// <summary>
        /// Station the train arrives to
        /// </summary>
        public StationIdentification To { get; }

        /// <summary>
        /// Time the train arrives at the destination station
        /// </summary>
        public TimeSpan? ToTime { get; }

        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="id">Identifies the train</param>
        /// <param name="from">Identifies the train</param>
        /// <param name="to">Station the train arrives to</param>
        /// <param name="type">Type of the train on this subrelation</param>
        public TrainRelation(APIResponse origin, TrainIdentification id, StationIdentification from, StationIdentification to, TimeSpan? fromTime = null, TimeSpan? toTime = null)
            : base(origin)
        {
            Id = id;
            From = from;
            To = to;
            FromTime = fromTime;
            ToTime = toTime;
        }
    }
}
