using MAVAppBackend.Entities;
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
    public class TrainIdStatement : ParserStatement
    {
        /// <summary>
        /// Unique number of the train (more like a train class id)
        /// </summary>
        public int? Number { get; }
        /// <summary>
        /// Unique id of the train instance (more like a train instance id)
        /// </summary>
        public string? ElviraId { get; }

        /// <param name="origin">API response that was Db to make this statement</param>
        /// <param name="number">Unique number of the train (more like a train class id)</param>
        /// <param name="elviraID">Unique id of the train instance (more like a train instance id)</param>
        public TrainIdStatement(APIResponse origin, int? number, string? elviraID)
            : base(origin)
        {
            Number = number;
            ElviraId = elviraID;
        }

        public Train? DbTrain { get; private set; } = null;
        public TrainInstance? DbTrainInstance { get; private set; } = null;

        protected override void InternalProcess(AppContext appContext)
        {
            if (Number == null) return;

            DbTrain = appContext.Trains.Where(t => t.TrainNumber == Number).FirstOrDefault();
            if (DbTrain == null)
            {
                appContext.Trains.Add(DbTrain = new Train(Number.Value));
            }

            if (ElviraId == null) return;

            DbTrainInstance = appContext.TrainInstances.Where(t => t.ElviraId == ElviraId).FirstOrDefault();
            if (DbTrainInstance == null)
            {
                DbTrainInstance = new TrainInstance(ElviraId, DbTrain);
                appContext.TrainInstances.Add(DbTrainInstance);
            }
        }
    }
}
