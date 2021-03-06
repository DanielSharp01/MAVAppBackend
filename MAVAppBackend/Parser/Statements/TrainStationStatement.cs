﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MAVAppBackend.Entities;
using MAVAppBackend.MAV;

namespace MAVAppBackend.Parser.Statements
{
    /// <summary>
    /// Links a train and a station together
    /// </summary>
    public class TrainStationStatement : ParserStatement
    {
        /// <summary>
        /// Identifies the train
        /// </summary>
        public TrainIdStatement TrainId { get; }

        /// <summary>
        /// Station of the train
        /// </summary>
        public StationIdStatement StationId { get; }

        /// <summary>
        /// Time the train arrives at (null may indicate first station or no time tuple data <see cref="HasTimeInfo">HasTimeInfo</see> for more info)
        /// </summary>
        public TimeTuple? Arrival { get; }

        /// <summary>
        /// Time the train departs at (null may indicate last station or no time tuple data <see cref="HasTimeInfo">HasTimeInfo</see> for more info)
        /// </summary>
        public TimeTuple? Departure { get; }

        public bool HasTimeInfo { get; }

        /// <param name="origin"></param>
        /// <param name="trainId">Identifies the train</param>
        /// <param name="stationId">Station of the train</param>
        /// <param name="arrival">Time the train arrives at (null may indicate first station, this will set <see cref="HasTimeInfo">HasTimeInfo</see> to false if departure is also null)</param>
        /// <param name="departure">Time the train departs at (null may indicate last station, this will set <see cref="HasTimeInfo">HasTimeInfo</see> to false if arrival is also null)</param>
        public TrainStationStatement(APIResponse origin, TrainIdStatement trainId, StationIdStatement stationId, TimeTuple? arrival, TimeTuple? departure)
            : base(origin)
        {
            TrainId = trainId;
            StationId = stationId;
            Arrival = arrival;
            Departure = departure;
            HasTimeInfo = arrival != null || departure != null;
        }

        public TrainStation? DbTrainStation { get; private set; }

        protected override void InternalProcess(AppContext appContext)
        {
            if (TrainId.DbTrain == null) return;
            if (StationId.DbStation == null) return;

            DbTrainStation = appContext.TrainStations.Where(s => s.TrainId == TrainId.DbTrain.Id && s.StationId == StationId.DbStation.Id).FirstOrDefault();
            if (DbTrainStation == null)
            {
                DbTrainStation = new TrainStation(TrainId.DbTrain, StationId.DbStation);
                appContext.TrainStations.Add(DbTrainStation);
                DbTrainStation.Arrival = Arrival?.Scheduled;
                DbTrainStation.Departure = Departure?.Scheduled;
            }
        }
    }
}
