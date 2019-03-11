using MAVAppBackend.Entities;
using MAVAppBackend.MAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend.Parser.Statements
{
    /// <summary>
    /// Tells the path of a train
    /// </summary>
    public class TrainPolylineStatement : ParserStatement
    {
        /// <summary>
        /// Identifies the train
        /// </summary>
        public TrainIdStatement Id { get; }

        /// <summary>
        /// Encoded polyline of the train path (google maps encoding)
        /// </summary>
        public string EncodedPolyline { get; }

        /// <param name="origin">API response that was processed to make this statement</param>
        /// <param name="id">Identifies the train</param>
        /// <param name="encodedPolyline">Encoded polyline of the train path (google maps encoding)</param>
        public TrainPolylineStatement(APIResponse origin, TrainIdStatement id, string encodedPolyline)
            : base(origin)
        {
            Id = id;
            EncodedPolyline = encodedPolyline;
        }

        private const double searchRadius = 2000;

        protected override void InternalProcess(AppContext appContext)
        {
            if (Id.DbTrain == null) return;

            if (Id.DbTrain.EncodedPolyline == null)
                Id.DbTrain.EncodedPolyline = EncodedPolyline;

            if (EncodedPolyline != null)
            {
                Station? currentStation = appContext.TrainStationLinks.Where(sl => sl.TrainId == Id.DbTrain.Id && sl.FromId == 0).Select(sl => sl.To).FirstOrDefault();
                double currentDistance = 0;

                while (currentStation != null)
                {
                    Polyline polyline = new Polyline(EncodedPolyline, Projection.WebMercatorHungary);
                    ProjVector2? stationPosition = null;
                    while (stationPosition == null && currentDistance <= polyline.MeterLength / 1000)
                    {
                        stationPosition = PlacesAPI.Search(currentStation, polyline.AtDistance(currentDistance), searchRadius, 3);
                        currentDistance += searchRadius;
                    }

                    if (stationPosition != null)
                    {
                        stationPosition.IntoProjection(Projection.LatitudeLongitude);
                        var newDist = polyline.ProjectedDistance(stationPosition, 0.5);
                        if (newDist != null)
                        {
                            currentDistance = newDist.Value;
                            appContext.TrainStations.Where(st => st.TrainId == Id.DbTrain.Id && st.StationId == currentStation.Id).FirstOrDefault().Distance = currentDistance;
                        }

                        currentStation.Latitude = stationPosition.X;
                        currentStation.Longitude = stationPosition.Y;

                        if (newDist >= polyline.MeterLength / 1000) return;
                    }

                    currentStation = appContext.TrainStationLinks.Where(sl => sl.TrainId == Id.DbTrain.Id && sl.FromId == currentStation.Id && sl.ToId != 0).Select(sl => sl.To).FirstOrDefault();
                }
            }
        }
    }
}
