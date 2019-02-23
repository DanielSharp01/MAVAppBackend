using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAVAppBackend
{
    public class Polyline
    {
        public const double EncodingFactor = 1e5;

        public List<ProjVector2> Points = new List<ProjVector2>();

        public Polyline(List<ProjVector2> points, Projection projection)
        {
            Points.AddRange(points);
            foreach (var point in Points)
            {
                point.IntoProjection(projection);
            }
        }

        public Polyline(string encodedPolyline, Projection projection)
            : this(DecodePoints(encodedPolyline, EncodingFactor), projection)
        { }

        private double? meterLength = null;
        public double MeterLength
        {
            get
            {
                if (meterLength == null)
                {
                    meterLength = 0;
                    for (int i = 1; i < Points.Count; i++)
                    {
                        meterLength += (Points[i] - Points[i - 1]).MeterLength;
                    }
                }

                return meterLength ?? 0;
            }
        }

        public ProjVector2 AtDistance(double kmDistance)
        {
            for (int i = 1; i < Points.Count; i++)
            {
                double pdist = (Points[i] - Points[i - 1]).MeterLength / 1000;
                if (kmDistance < pdist)
                {
                    return Points[i - 1] * (1 - kmDistance / pdist) + Points[i] * kmDistance / pdist;
                }
                else kmDistance -= pdist;
            }

            return new ProjVector2(Points.Last().Vec, Points.Last().Projection);
        }

        public double? ProjectedDistance(ProjVector2 point, double distanceLimit)
        {
            point = point.ToProjection(Points.First().Projection);

            int bestIndex = -1;
            double bestProj = 0;
            double bestDist = 0;

            for (int i = 1; i < Points.Count; i++)
            {
                ProjVector2 a = Points[i] - Points[i - 1];
                ProjVector2 b = point - Points[i - 1];
                double proj = a.Dot(b) / a.LengthSquared;

                if (proj < 0) proj = 0;
                else if (proj > 1) proj = 1;

                double dist = (point - (Points[i] + a * proj)).MeterLength / 1000;

                if ((bestIndex == -1 || dist < bestDist) && dist < distanceLimit)
                {
                    bestIndex = i;
                    bestProj = proj;
                    bestDist = dist;
                }
            }

            // The point could not be projected
            if (bestIndex == -1)
                return null;

            double km = 0;
            for (int i = 1; i <= bestIndex; i++)
            {
                km += (Points[i] - Points[i - 1]).MeterLength / 1000;
            }

            km += bestProj * (Points[bestIndex + 1] - Points[bestIndex]).MeterLength / 1000;

            return km;
        }

        public string ToEncodedPolyline()
        {
            return EncodePoints(Points, EncodingFactor);
        }

        /// <summary>
        /// <para>Decodes a polyline from string into points as GPS Position as latitude (X) longitude (Y)</para>
        /// <para>Source: https://github.com/mapbox/polyline/blob/master/src/polyline.js</para>
        /// </summary>
        /// <param name="encodedPolyline">Encoded polyline</param>
        /// <param name="precisionFactor">Precision factor of the encoded polyline. Same as the one you encoded with if you encoded yourself.</param>
        /// <returns>Points of the polyline as latitude, longitude</returns>
        private static List<ProjVector2> DecodePoints(string encodedPolyline, double precisionFactor)
        {
            List<ProjVector2> points = new List<ProjVector2>();
            int latitude = 0;
            int longitude = 0;
            for (int i = 0; i < encodedPolyline.Length;)
            {
                int b;
                int shift = 0;
                int result = 0;
                do
                {
                    b = encodedPolyline[i++] - 63;
                    result |= (b & 31) << shift;
                    shift += 5;
                }
                while (32 <= b);
                latitude += (result & 1) > 0 ? ~(result >> 1) : result >> 1;

                result = shift = 0;
                do
                {
                    b = encodedPolyline[i++] - 63;
                    result |= (b & 31) << shift;
                    shift += 5;
                }
                while (32 <= b);
                longitude += (result & 1) > 0 ? ~(result >> 1) : result >> 1;
                ProjVector2 p = new ProjVector2(latitude / precisionFactor, longitude / precisionFactor, Projection.LatitudeLongitude);
                points.Add(p);
            }

            return points;
        }

        /// <summary>
        /// <para>Encodes a polyline into string from points as GPS Position as latitude (X) longitude (Y)</para>
        /// <para>Source: https://github.com/mapbox/polyline/blob/master/src/polyline.js</para>
        /// </summary>
        /// <param name="points">Points as latitude, longitude</param>
        /// <param name="precisionFactor">Precision factor to encode with.</param>
        /// <returns>Encoded polyline</returns>
        private static string EncodePoints(IReadOnlyList<ProjVector2> points, double precisionFactor)
        {
            List<Vector2> latLonPoints = points.Select(p => p.ToProjection(Projection.LatitudeLongitude).Vec).ToList();
            string output = EncodeHelper(points[0].X, 0, precisionFactor) + EncodeHelper(points[0].Y, 0, precisionFactor);

            for (var i = 1; i < latLonPoints.Count; i++)
            {
                Vector2 current = latLonPoints[i], previous = latLonPoints[i - 1];
                output += EncodeHelper(current.X, previous.X, precisionFactor);
                output += EncodeHelper(current.Y, previous.Y, precisionFactor);
            }

            return output;
        }

        /// <summary>
        /// <para>Helper method for encoding polylines</para>
        /// <para>Source: https://github.com/mapbox/polyline/blob/master/src/polyline.js</para>
        /// </summary>
        /// <param name="current">Current coordinate.X or coordinate.Y (Latitude or Longitude)</param>
        /// <param name="previous">Previous coordinate.X or coordinate.Y (Latitude or Longitude)</param>
        /// <param name="precisionFactor">Precision factor to encode with.</param>
        /// <returns></returns>
        private static string EncodeHelper(double current, double previous, double precisionFactor)
        {
            int coordinate = PythonRound(current * precisionFactor) - PythonRound(previous * precisionFactor);
            coordinate <<= 1;
            if (current - previous < 0)
            {
                coordinate = ~coordinate;
            }

            string output = "";
            while (coordinate >= 32)
            {
                output += (char)((32 | (coordinate & 31)) + 63);
                coordinate >>= 5;
            }
            output += (char)(coordinate + 63);
            return output;
        }

        /// <summary>
        /// Python's rounding function used by the encoding algorithms
        /// </summary>
        /// <param name="value">Value to round</param>
        /// <returns>Rounded value</returns>
        private static int PythonRound(double value)
        {
            return (int)Math.Floor(Math.Abs(value) + 0.5) * (value >= 0 ? 1 : -1);
        }
    }
}
